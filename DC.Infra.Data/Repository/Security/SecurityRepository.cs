using Dapper;
using DC.Domain.Entity.Security;
using DC.Domain.Interface.Security;
using DC.Infra.Data.Context;
using System.Collections.Generic;
using System.Data;

namespace DC.Infra.Data.Repository.Security
{
    public class SecurityRepository<response, request> : ISecurity<response, request> where response : AuthenticationResponseDTO where request : AuthenticationRequestDTO
    {
        private response _response;

        public SecurityRepository() : base()
        {
            _response = (response)new AuthenticationResponseDTO();
        }

        internal IDbConnection Connect
        {
            get => BaseContext.Conn();
        }

        public int InsertToken(response data)
        {
            using (IDbConnection cn = Connect)
            {
                if (cn.State == ConnectionState.Closed)
                {
                    cn.Open();
                }
                var result = cn.Execute("SP_DC_INSERT_USER_TOKEN", new
                {
                    @email = data.email,
                    @token = data.token,                   
                    @processDate = data.processDate,
                    @nameSystem =data.nameSystem,
                    @expires = data.expires,
                }, commandType: CommandType.StoredProcedure);

                return result;
            }
        }

        public IEnumerable<response> ValidateToken(request data)
        {
            using (IDbConnection cn = Connect)
            {
                if (cn.State == ConnectionState.Closed)
                {
                    cn.Open();
                }
                var result = cn.Query<response>("SP_DC_GET_USER_TOKEN", new
                {
                    @nameSystem = data.nameSystem,
                    @email = data.email
                }, commandType: CommandType.StoredProcedure);

                return result;
            }
        }
    }
}
