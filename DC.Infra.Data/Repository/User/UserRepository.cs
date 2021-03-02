using Dapper;
using DC.Domain.Entity.Security;
using DC.Domain.Entity.User;
using DC.Domain.Interface.User;
using DC.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Data;

namespace DC.Infra.Data.Repository.User
{
    public class UserRepository<response, request> : IUser<response, request> where response : UserResponseDTO where request : UserRequestDTO
    {
        private response _response;

        public UserRepository() : base()
        {
            _response = (response)new UserResponseDTO();
        }

        internal IDbConnection Connect
        {
            get => BaseContext.Conn();
        }

        public int InsertUser(request data, string guid, string hashKeyUser)
        {
            using (IDbConnection cn = Connect)
            {
                if (cn.State == ConnectionState.Closed)
                {
                    cn.Open();
                }
                var result = cn.Execute("SP_DC_INSERT_USER", new
                {
                    @email = data.email,
                    @guid = guid,
                    @password = data.codeIn,
                    @hashKeyUser = hashKeyUser,
                    @userName = data.nickName,
                    @proccessDate = DateTime.Now
                }, commandType: CommandType.StoredProcedure);

                return result;
            }
        }

        public IEnumerable<response> SelectUserByData(request data)
        {
            using (IDbConnection cn = Connect)
            {
                if (cn.State == ConnectionState.Closed)
                {
                    cn.Open();
                }
                var result = cn.Query<response>("SP_DC_GET_USER_BY_DATA", new
                {
                    @email = data.email,
                    @userName = data.nickName,
                }, commandType: CommandType.StoredProcedure);

                return result;
            }
        }

        public IEnumerable<string> SelectTokenBySkey(string token, string nameSystem)
        {
            using (IDbConnection cn = Connect)
            {
                if (cn.State == ConnectionState.Closed)
                {
                    cn.Open();
                }
                var result = cn.Query<string>("SP_DC_GET_TOKEN_BY_SKEY", new
                {
                    @token = token, 
                    @nameSystem = nameSystem
                }, commandType: CommandType.StoredProcedure);

                return result;
            }
        }
        public IEnumerable<AuthenticationResponseDTO> SelectUserByEmailPass(UserRequestDTO data)
        {
            using (IDbConnection cn = Connect)
            {
                if (cn.State == ConnectionState.Closed)
                {
                    cn.Open();
                }
                var result = cn.Query<AuthenticationResponseDTO>("SP_DC_GET_USER_BY_EMAIL_PASS", new
                {
                    @email = data.email,
                    @password = data.codeIn,
                }, commandType: CommandType.StoredProcedure);

                return result;
            }
        }

        public IEnumerable<response> SelectUserByEmail(request data)
        {
            using (IDbConnection cn = Connect)
            {
                if (cn.State == ConnectionState.Closed)
                {
                    cn.Open();
                }
                var result = cn.Query<response>("SP_DC_GET_USER_BY_EMAIL", new
                {
                    @email = data.email,
                }, commandType: CommandType.StoredProcedure);

                return result;
            }
        }
    }
}
