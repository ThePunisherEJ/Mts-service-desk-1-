using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MTS.ServiceDesk.Shared.Models
{
    public class CurrentUser
    {
        public bool IsAuthenticated { get; set; }
        public string UserName { get; set; }
        //public Dictionary<string, string> Claims { get; set; }
        public List<string[]> Claims { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public static class SecurityRoles
    {
        public const string Admin = "7FCDA348-23FF-4B5D-BAD6-4ED662B7EF36";
        public const string Consultant = "A69FADF6-73A6-4CFF-BEBF-99292C091AD7";
        public const string User = "225487A7-5823-46C2-815A-025B2EBF360C";
    }

    public enum UserType
    {
        Administrator = 1,
        Consultant = 2,
        User = 3
    }

    public class UserCreateUpdateRequest
    {
        public Guid UserId { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }
        [Required]
        public int ClientId { get; set; }
        public UserType TypeOfUser { get; set; }
        
        public int UserStatus { get; set; }

    }

    public class UserDetails
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int ClientId { get; set; }
        public UserType TypeOfUser { get; set; }
        public int UserStatus { get; set; }

        public bool UserStatusBool
        {
            get
            {
                return UserStatus == 1;
            }
            set
            {
                if (value == true)
                {
                    UserStatus = 1;
                }
                else
                {
                    UserStatus = 2;
                }
            }
        }

    }



}
