using System;
using System.Security.Claims;
using System.Threading.Tasks;
using FindTech.Entities.Models.Enums;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FindTech.Entities.Models
{
    public class FindTechUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public DateTime? DayOfBirth { get; set; }
        public Gender Gender { get; set; }
        public Level Level { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<FindTechUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}