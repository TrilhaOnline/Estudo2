using System;
using System.Net;

namespace DC.Domain.Entity.Security
{
    public class AuthenticationResponseDTO
    {
        public AuthenticationResponseDTO()
        {
            this.hasAccess = false;
        }
        public string guid { get; set; }
        public string email { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public bool generateToken { get; set; }
        public bool hasAccess { get; set; }
        public string message { get; set; }
        public string token { get; set; }
        public string haskKeyUser { get; set; }
        public DateTime processDate { get; set; }
        public string nameSystem { get; set; }
        public DateTimeOffset? expires { get; set; }
        public HttpStatusCode statusCode { get; set; }
    }
}
