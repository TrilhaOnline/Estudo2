using DC.Domain.Entity.Security;
using DC.Domain.Entity.User;
using DC.Infra.Data.Repository.Security;
using DC.Infra.Data.Repository.User;
using DC.Resources.Utils.Converter;
using System;
using System.Linq;

namespace DC.Services.Business.User
{
    public class UserBusiness
    {
        private UserRepository<UserResponseDTO, UserRequestDTO> _userRepository;
        private UserResponseDTO _response;
        private SecurityRepository<AuthenticationResponseDTO, AuthenticationRequestDTO> _securityResponse;
        private AuthenticationResponseDTO _responseAuth;
        public UserBusiness()
        {
            _userRepository = new UserRepository<UserResponseDTO, UserRequestDTO>();
            _securityResponse = new SecurityRepository<AuthenticationResponseDTO, AuthenticationRequestDTO>();
            _response = new UserResponseDTO();
            _responseAuth = new AuthenticationResponseDTO();
        }

        public AuthenticationResponseDTO SelectUserLogin(AuthenticationRequestDTO data)
        {
            var resultGuid = _userRepository.SelectUserByEmail(new UserRequestDTO { email = data.email });

            if(resultGuid != null)
            {
                var resultUser = _userRepository.SelectUserByEmailPass(new UserRequestDTO
                {
                    email = data.email,
                    codeIn = data.codeIn
                });
                if(resultUser != null && resultUser.Count() > 0)
                {
                    if (resultGuid.FirstOrDefault().guid == resultUser.FirstOrDefault().guid)
                    {
                        var resultoken = _securityResponse.ValidateToken(new AuthenticationRequestDTO
                        {
                            email = resultUser.FirstOrDefault().email,
                            nameSystem = data.nameSystem
                        });

                        if(resultoken != null && resultoken.Count() > 0)
                        {
                            if (EncodeDecode.CrashOut(resultUser.FirstOrDefault().password) == EncodeDecode.CrashOut(data.codeIn))
                            {
                                if(resultoken.FirstOrDefault().nameSystem == data.nameSystem)
                                {
                                    _responseAuth.email = data.email;
                                    _responseAuth.nameSystem = resultoken.FirstOrDefault().nameSystem;
                                    _responseAuth.guid = resultUser.FirstOrDefault().guid;
                                    _responseAuth.hasAccess = true;
                                    _responseAuth.token = resultoken.FirstOrDefault().token;
                                    _responseAuth.userName = resultUser.FirstOrDefault().userName;
                                    _responseAuth.statusCode = System.Net.HttpStatusCode.OK;
                                    _responseAuth.message = "Usuário validado com sucesso!";
                                }
                                else
                                {
                                    _responseAuth.email = string.Empty;
                                    _responseAuth.hasAccess = false;
                                    _responseAuth.statusCode = System.Net.HttpStatusCode.Unauthorized;
                                    _responseAuth.message = "Usuário não encontrado!";
                                    return _responseAuth;
                                }
                            }
                            else
                            {
                                _responseAuth.email = string.Empty;
                                _responseAuth.hasAccess = false;
                                _responseAuth.statusCode = System.Net.HttpStatusCode.Unauthorized;
                                _responseAuth.message = "Usuário não encontrado!";
                                return _responseAuth;
                            }
                        }
                        else
                        {
                            _responseAuth.email = string.Empty;
                            _responseAuth.hasAccess = false;
                            _responseAuth.statusCode = System.Net.HttpStatusCode.Unauthorized;
                            _responseAuth.message = "Usuário não encontrado!";
                            return _responseAuth;
                        }
                    }
                    else
                    {
                        _responseAuth.email = string.Empty;
                        _responseAuth.hasAccess = false;
                        _responseAuth.statusCode = System.Net.HttpStatusCode.Unauthorized;
                        _responseAuth.message = "Usuário não encontrado!";
                        return _responseAuth;
                    }
                }
                else
                {
                    _responseAuth.email = string.Empty;
                    _responseAuth.hasAccess = false;
                    _responseAuth.statusCode = System.Net.HttpStatusCode.Unauthorized;
                    _responseAuth.message = "Usuário não encontrado!";
                    return _responseAuth;
                }
            }
            
            return _responseAuth;
        }
        public AuthenticationResponseDTO SelectUserByData(string nameSystem, string token, string sKey)
        {
            var resultMail = _userRepository.SelectTokenBySkey(token, nameSystem);
            if(resultMail == null)
            {
                _responseAuth.email = resultMail.FirstOrDefault();
                _responseAuth.hasAccess = false;
                _responseAuth.statusCode = System.Net.HttpStatusCode.Unauthorized;
                _responseAuth.message = "Usuário não encontrado!";
                return _responseAuth;
            }
            var resultUser = _userRepository.SelectUserByEmail(new UserRequestDTO { email = resultMail.FirstOrDefault()});

            if(resultUser == null)
            {
                _responseAuth.email = resultMail.FirstOrDefault();
                _responseAuth.hasAccess = false;
                _responseAuth.statusCode = System.Net.HttpStatusCode.Unauthorized;
                _responseAuth.message = "Usuário não encontrado!";
                return _responseAuth;
            }

            if (resultUser.FirstOrDefault().guid == sKey)
            {
                _responseAuth.email = resultMail.FirstOrDefault();
                _responseAuth.hasAccess = true;
                _responseAuth.statusCode = System.Net.HttpStatusCode.OK;
                _responseAuth.message = "Usuário validado com sucesso!";
            }
            return _responseAuth;
        }
        public bool SelectUserByData(string userName, string email, string guid, out string messageError)
        {
            messageError = string.Empty;
            bool blnReturn = false;
            try
            {
                var result = _userRepository.SelectUserByData(new UserRequestDTO
                {
                    email = email,
                    nickName = userName
                });
                if (result.ToList().Count > 0)
                    if (result.ToList().FirstOrDefault().guid == guid)
                        blnReturn = true;
            }
            catch(Exception ex)
            {
                messageError = "Erro ao recuperar dados do usuário | " + ex.Message;
                return false;
            }
            return blnReturn;
        }
        public UserResponseDTO InsertUser(UserRequestDTO data)
        {
            var resultGuid = Guid.NewGuid();

            try
            {
                var resultMailExist = VerifyUserExist(data, out _response);

                if(!resultMailExist)
                {
                    if (_userRepository.InsertUser(new UserRequestDTO
                    {
                        email = data.email,
                        codeIn = EncodeDecode.CrashIn(data.codeIn),
                        nickName = data.nickName
                    }, resultGuid.ToString(), EncodeDecode.CrashIn(data.codeIn)) == 1)
                    {
                        _response.guid = resultGuid.ToString();
                        _response.hasSuccess = true;
                        _response.logged = true;
                        _response.statusCode = System.Net.HttpStatusCode.OK;
                        _response.message = "Usuário cadastrado com sucesso!";
                    }
                    else
                    {
                        _response.guid = null;
                        _response.hasSuccess = false;
                        _response.logged = false;
                        _response.statusCode = System.Net.HttpStatusCode.Unauthorized;
                        _response.message = "Erro ao cadastrar usuário!";
                    }
                }               
            }
            catch (Exception ex)
            {
                _response.guid = null;
                _response.hasSuccess = false;
                _response.logged = false;
                _response.statusCode = System.Net.HttpStatusCode.Unauthorized;
                _response.message = "Erro no cadastro de usuários. | "  + ex.Message;
            }
            return _response;
        }

        private bool VerifyUserExist(UserRequestDTO data, out UserResponseDTO resultResponse)
        {
            var blnReturn = false;
            resultResponse = new UserResponseDTO();

            var resultMailExist = _userRepository.SelectUserByEmail(data);

            if (resultMailExist.Count() > 0)
            {
                resultResponse.guid = null;
                resultResponse.hasSuccess = false;
                resultResponse.hasExist = true;
                resultResponse.logged = false;
                resultResponse.statusCode = System.Net.HttpStatusCode.Unauthorized;
                resultResponse.message = "Existe um usuário para o e-mail cadastrado! Por favor recupere seus dados ou entre com um novo cadastro!";
                blnReturn = true;
            }

            return blnReturn;
        }
    }
}
