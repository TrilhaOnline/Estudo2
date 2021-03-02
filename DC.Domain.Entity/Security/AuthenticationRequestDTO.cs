using System;

namespace DC.Domain.Entity.Security
{
    public class AuthenticationRequestDTO
    {
        public string userName { get; set; }
        public string codeIn { get; set; }
        public string nameSystem { get; set; }
        public string email { get; set; }
        public string sKey { get; set; }
    }
}
