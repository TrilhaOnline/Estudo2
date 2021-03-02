using DC.Domain.Entity.Security;
using DC.Domain.Entity.User;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace DC.Application.Api.BaseController
{
    public class Security
    {
        public static HttpResponseMessage MessageUnauthorized(HttpRequestMessage req)
        {
            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Unauthorized
            };
        }

        public static UserRequestDTO SetDataParameters(AuthenticationRequestDTO data, string token = null, string expires = null)
        {
            return new UserRequestDTO
            {
                nickName = data.userName,
                email = data.email
            };
        }

        public static CookieHeaderValue SetCookieSession(UserRequestDTO authorizedRequest, string token, HttpRequestMessage request, out string expiresTime, out string sToken)
        {
            var time = 60;
            sToken = string.Empty;
            expiresTime = string.Empty;
            expiresTime = DateTimeOffset.Now.AddMinutes(time).ToString();
            var nv = new NameValueCollection();
            nv["Token"] = token;
            nv["Expires"] = expiresTime;
            sToken = token;
            var cookie = new CookieHeaderValue("Authorization", nv);
            cookie.Expires = DateTimeOffset.Now.AddMinutes(time);
            cookie.Domain = request.RequestUri.Host;
            cookie.Path = "/";
            return cookie;
        }

        public static HttpResponseMessage GetMessageInternalError(HttpResponseMessage respMessage, string messageError)
        {
            var authorize = new UserResponseDTO
            {
                hasSuccess = false,
                guid = null,
                message = messageError,
                logged = false,
                statusCode = HttpStatusCode.Unauthorized
            };
            var listAuthorize = new List<UserResponseDTO>
            {
                authorize
            };
            respMessage.Content = new ObjectContent<UserResponseDTO[]>(listAuthorize.ToArray(), new JsonMediaTypeFormatter());

            return respMessage;
        }

        public static HttpResponseMessage GetMessageInvalidMail(HttpResponseMessage respMessage)
        {
            var authorize = new UserResponseDTO
            {
                hasSuccess = false,
                guid = null,
                message = "E-mail inválido",
                logged = false,
                statusCode = HttpStatusCode.Unauthorized
            };
            var listAuthorize = new List<UserResponseDTO>
            {
                authorize
            };
            respMessage.Content = new ObjectContent<UserResponseDTO[]>(listAuthorize.ToArray(), new JsonMediaTypeFormatter());

            return respMessage;
        }

        public static HttpResponseMessage GetMessageNotCreateUser(HttpResponseMessage respMessage, string message)
        {
            var authorize = new UserResponseDTO
            {
                hasSuccess = false,
                guid = null,
                message = message,
                logged = false,
                statusCode = HttpStatusCode.Unauthorized
            };
            var listAuthorize = new List<UserResponseDTO>
            {
                authorize
            };
            respMessage.Content = new ObjectContent<UserResponseDTO[]>(listAuthorize.ToArray(), new JsonMediaTypeFormatter());

            return respMessage;
        }

        public static HttpResponseMessage GetMessageCreateUser(HttpResponseMessage respMessage, UserResponseDTO response)
        {
            var authorize = new UserResponseDTO
            {
                hasSuccess = response.hasSuccess,
                hasExist = response.hasExist,
                guid = response.guid,
                message = response.message,
                logged = response.logged,
                statusCode = response.statusCode
            };
            var listAuthorize = new List<UserResponseDTO>
            {
                authorize
            };
            respMessage.Content = new ObjectContent<UserResponseDTO[]>(listAuthorize.ToArray(), new JsonMediaTypeFormatter());

            return respMessage;
        }

        public static HttpResponseMessage GetMessageCreateUser(HttpResponseMessage respMessage, AuthenticationResponseDTO response)
        {
            var authorize = new AuthenticationResponseDTO
            {
                hasAccess = response.hasAccess,               
                email = response.email,
                userName = response.userName,
                guid = response.guid,
                token = response.token,
                nameSystem = response.nameSystem,
                message = response.message,
                statusCode = response.statusCode
            };
            var listAuthorize = new List<AuthenticationResponseDTO>
            {
                authorize
            };
            respMessage.Content = new ObjectContent<AuthenticationResponseDTO[]>(listAuthorize.ToArray(), new JsonMediaTypeFormatter());

            return respMessage;
        }

        public static HttpResponseMessage GetMessageUnauthorizedLogin(HttpResponseMessage respMessage)
        {
            var authorize = new AuthenticationResponseDTO
            {
                hasAccess = false,
                expires = null,
                message = "Unauthorized",
                token = null,
                statusCode = HttpStatusCode.Unauthorized
            };
            var listAuthorize = new List<AuthenticationResponseDTO>
            {
                authorize
            };
            respMessage.Content = new ObjectContent<AuthenticationResponseDTO[]>(listAuthorize.ToArray(), new JsonMediaTypeFormatter());

            return respMessage;
        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
              

        public static void GetMessageAuthorizedLogin(HttpResponseMessage respMessage, CookieHeaderValue cookie, string sToken, DateTime processDate, string email, string nameSystem)
        {
            var listAuthorize = new List<AuthenticationResponseDTO>
            {
                new AuthenticationResponseDTO
                {
                    hasAccess = true,
                    email = email,
                    expires = cookie.Expires,
                    processDate = processDate,
                    message = "Authorized",
                    nameSystem = nameSystem,
                    token = sToken,
                    statusCode = HttpStatusCode.OK
                }
            };
            respMessage.Content = new ObjectContent<AuthenticationResponseDTO[]>(listAuthorize.ToArray(), new JsonMediaTypeFormatter());
        }
        public static string CreteToken(string userName, string sKey)
        {
            var sec = sKey;
            var issuedAt = DateTime.UtcNow;
            var expires = DateTime.UtcNow.AddMinutes(30);
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = new JwtSecurityToken();

            try
            {
                var claimsIdentity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, userName)
                });
                var securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(sec));
                var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

                token = (JwtSecurityToken)
                    tokenHandler.CreateJwtSecurityToken(issuer: "http://localhost", audience: "http://localhost",
                    subject: claimsIdentity, notBefore: issuedAt, expires: expires, signingCredentials: signingCredentials);
            }
            catch (Exception ex)
            {
                token = null;
                return string.Empty;
            }
            return tokenHandler.WriteToken(token);
        }
    }
}