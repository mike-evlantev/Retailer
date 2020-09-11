using System;

namespace Retailer.Core.Models
{
    public interface IUserModel
    {
        DateTime CreatedDate { get; set; }
        string Email { get; set; }
        string FirstName { get; set; }
        string Id { get; set; }
        string LastName { get; set; }
        string Token { get; set; }

        void ClearUser();
    }
}