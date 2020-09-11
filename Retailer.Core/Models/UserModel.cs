using System;

namespace Retailer.Core.Models
{
    public class UserModel : IUserModel
    {
        public string Id { get; set; }
        public string Token { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }

        public void ClearUser()
        {
            Id = "";
            Token = "";
            FirstName = "";
            LastName = "";
            Email = "";
            CreatedDate = DateTime.MinValue;
        }
    }
}
