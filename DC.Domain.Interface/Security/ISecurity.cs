using DC.Domain.Entity.Security;
using System.Collections.Generic;

namespace DC.Domain.Interface.Security
{
    public interface ISecurity<response, request> where response : AuthenticationResponseDTO where request : AuthenticationRequestDTO
    {
        int InsertToken(response data);
        IEnumerable<response> ValidateToken(request data);
    }
}
