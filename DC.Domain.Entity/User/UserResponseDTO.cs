using System;
using System.Net;

namespace DC.Domain.Entity.User
{
    public class UserResponseDTO
    {
        public UserResponseDTO() 
        {
            this.logged = false;
            this.hasSuccess = false;
            this.hasExist = false;
        }
        public string guid { get; set; }
        public string message { get; set; }
        public bool logged { get; set; }
        public bool hasSuccess { get; set; }
        public bool hasExist { get; set; }
        public HttpStatusCode statusCode { get; set; }
    }
}
