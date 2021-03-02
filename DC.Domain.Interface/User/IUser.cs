using DC.Domain.Entity.Security;
using DC.Domain.Entity.User;
using System;
using System.Collections.Generic;

namespace DC.Domain.Interface.User
{
    public interface IUser<response, request> where response : UserResponseDTO where request : UserRequestDTO
    {
        int InsertUser(request data, string guid, string hashKeyUser);
        IEnumerable<response> SelectUserByData(request data);
        IEnumerable<AuthenticationResponseDTO> SelectUserByEmailPass(UserRequestDTO data);
        IEnumerable<response> SelectUserByEmail(request data);
    }
}
