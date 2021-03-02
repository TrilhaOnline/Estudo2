using System;

namespace DC.Domain.Entity.User
{
    public class UserRequestDTO
    {
        //public string userName { get; set; }
        //public string password { get; set; }
        public string email { get; set; }
        public string customerCode { get; set; }
        public string ip { get; set; }
        public string nickName { get; set; }        
        public string codeIn { get; set; }
        public string repeatCodeIn { get; set; }
    }
    public class BaseEntity : UserRequestDTO
    {
        public string hashKeyUser { get; set; }
        public Guid? guid { get; set; }
    }
}
