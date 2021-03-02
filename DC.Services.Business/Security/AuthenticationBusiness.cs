using DC.Domain.Entity.Security;
using DC.Infra.Data.Repository.Security;
using System;

namespace DC.Services.Business.Security
{
    public class AuthenticationBusiness
    {
        private SecurityRepository<AuthenticationResponseDTO, AuthenticationRequestDTO> _securityRepository;
        private AuthenticationResponseDTO _response;

        public AuthenticationBusiness()
        {
            _securityRepository = new SecurityRepository<AuthenticationResponseDTO, AuthenticationRequestDTO>();
            _response = new AuthenticationResponseDTO();
        }

        public bool InsertToken(AuthenticationResponseDTO data)
        {
            var blnReturn = false;
            try
            {
                if (_securityRepository.InsertToken(data) == 1)
                    blnReturn = true;
            }
            catch (Exception ex)
            {
                return blnReturn;
            }
            return blnReturn;
        }
    }
}
