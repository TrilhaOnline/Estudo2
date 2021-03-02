using DC.Domain.Entity.Security;
using DC.Domain.Entity.User;
using DC.Resources.Utils.Converter;
using DC.Services.Business.Security;
using DC.Services.Business.User;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace DC.Application.Api.Controllers
{
    [RoutePrefix("api/security")]
    public class SecurityController : ApiController
    {
        private UserBusiness _userBusiness;
        private AuthenticationBusiness _authBusiness;
        
        public SecurityController()
        {
            _userBusiness = new UserBusiness();
            _authBusiness = new AuthenticationBusiness();
        }

        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(AuthenticationResponseDTO))]
        [SwaggerResponse(HttpStatusCode.NotFound, "NotFound")]
        [SwaggerResponse(HttpStatusCode.GatewayTimeout, "GatewayTimeout")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "BadRequest")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "InternalServerError")]
        [ResponseType(typeof(HttpResponseMessage))]
        [HttpPost]
        [Route("accesstoken")]
        public HttpResponseMessage GetAccessToken([FromBody] AuthenticationRequestDTO data)
        {
            var respMessage = new HttpResponseMessage();
            if (data == null)
                return BaseController.Security.GetMessageUnauthorizedLogin(respMessage);
            if(data.sKey == null)
                return BaseController.Security.GetMessageUnauthorizedLogin(respMessage);
            var sKey = string.Empty;
            var isUserNamePasswordValid = false;

            var loginRequest = new UserRequestDTO
            {
                nickName = data.userName.ToLower(),
                email = data.email
            };
            if(data != null)
            {
                var messageError = string.Empty;
                if (_userBusiness.SelectUserByData(data.userName, data.email, data.sKey, out messageError))
                    sKey = data.sKey;
                else
                    sKey = null;
                if (!string.IsNullOrWhiteSpace(messageError))
                   return BaseController.Security.GetMessageInternalError(respMessage, messageError);
                isUserNamePasswordValid = sKey == null ? false : true;
            }

            if(isUserNamePasswordValid)
            {
                var expires = string.Empty;
                var sToken = string.Empty;
                var proccessDate = DateTime.Now;
                var token = BaseController.Security.CreteToken(loginRequest.nickName, sKey);
                if (string.IsNullOrWhiteSpace(token))
                    return BaseController.Security.GetMessageUnauthorizedLogin(respMessage);

                BaseController.Security.GetMessageAuthorizedLogin(respMessage, BaseController.Security.SetCookieSession(loginRequest, token, Request, out expires, out sToken), sToken, proccessDate, data.email, data.nameSystem);

                if (!_authBusiness.InsertToken(new AuthenticationResponseDTO
                {
                    email = data.email,
                    expires = Convert.ToDateTime(expires),
                    nameSystem = data.nameSystem,
                    processDate = proccessDate,
                    token = token
                }))
                    respMessage = BaseController.Security.MessageUnauthorized(Request);
            }
            else
            {
                respMessage = BaseController.Security.MessageUnauthorized(Request);
            }

            return respMessage;
        }


        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(UserResponseDTO))]
        [SwaggerResponse(HttpStatusCode.NotFound, "NotFound")]
        [SwaggerResponse(HttpStatusCode.GatewayTimeout, "GatewayTimeout")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "BadRequest")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "InternalServerError")]
        [ResponseType(typeof(HttpResponseMessage))]
        [HttpPost]
        [Route("createuser")]
        public HttpResponseMessage CreateUser([FromBody] UserRequestDTO data)
        {
            var respMessage = new HttpResponseMessage();

            if (data == null)
                return BaseController.Security.GetMessageNotCreateUser(respMessage, "Dados inválidos");
            if(string.IsNullOrWhiteSpace(data.email))
                return BaseController.Security.GetMessageNotCreateUser(respMessage, "E-mail não informado");
            if (string.IsNullOrWhiteSpace(data.nickName))
                return BaseController.Security.GetMessageNotCreateUser(respMessage, "Usuário não informado");
            if (string.IsNullOrWhiteSpace(data.codeIn))
                return BaseController.Security.GetMessageNotCreateUser(respMessage, "Senha não informada");

            if (!BaseController.Security.IsValidEmail(data.email))
            {
                return BaseController.Security.GetMessageInvalidMail(respMessage);
            }
            else
            {
                data.codeIn = EncodeDecode.CrashOut(data.codeIn);
                data.repeatCodeIn = EncodeDecode.CrashOut(data.repeatCodeIn);
                BaseController.Security.GetMessageCreateUser(respMessage, _userBusiness.InsertUser(data));
            }
            return respMessage;
        }

        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(AuthenticationResponseDTO))]
        [SwaggerResponse(HttpStatusCode.NotFound, "NotFound")]
        [SwaggerResponse(HttpStatusCode.GatewayTimeout, "GatewayTimeout")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "BadRequest")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "InternalServerError")]
        [ResponseType(typeof(HttpResponseMessage))]
        [HttpPost]
        [Route("getpartnermail")]
        public HttpResponseMessage GetPartnerMail(string token, [FromBody] AuthenticationRequestDTO data)
        {
            var respMessage = new HttpResponseMessage();

            if (data == null)
                return BaseController.Security.GetMessageNotCreateUser(respMessage, "Dados inválidos");
            if (string.IsNullOrWhiteSpace(token))
                return BaseController.Security.GetMessageNotCreateUser(respMessage, "Unauthorized");

            BaseController.Security.GetMessageCreateUser(respMessage, _userBusiness.SelectUserByData(data.nameSystem, token, data.sKey));

            return respMessage;
        }

        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(AuthenticationResponseDTO))]
        [SwaggerResponse(HttpStatusCode.NotFound, "NotFound")]
        [SwaggerResponse(HttpStatusCode.GatewayTimeout, "GatewayTimeout")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "BadRequest")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "InternalServerError")]
        [ResponseType(typeof(HttpResponseMessage))]
        [HttpPost]
        [Route("getloginuser")]
        public HttpResponseMessage GelLoginUser([FromBody] AuthenticationRequestDTO data)
        {
            var token = string.Empty;
            var respMessage = new HttpResponseMessage();

            if (data == null)
                return BaseController.Security.GetMessageNotCreateUser(respMessage, "Dados inválidos");
            BaseController.Security.GetMessageCreateUser(respMessage, _userBusiness.SelectUserLogin(data));

            return respMessage;
        }
    }
}
