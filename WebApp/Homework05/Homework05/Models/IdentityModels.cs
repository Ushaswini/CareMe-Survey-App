using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace Homework05.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        

       

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            :base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<Survey> Surveys { get; set; }

        public System.Data.Entity.DbSet<StudyGroup> StudyGroups { get; set; }

        public System.Data.Entity.DbSet<SurveyResponse> SurveyResponses { get; set; }

        public System.Data.Entity.DbSet<Question> Questions { get; set; }

        public System.Data.Entity.DbSet<Device> Devices { get; set; }

        public System.Data.Entity.DbSet<X_Survey_Group> X_Survey_Groups { get; set; }
        public System.Data.Entity.DbSet<X_Survey_Question> X_Survey_Questions { get; set; }
        public System.Data.Entity.DbSet<X_User_Group> X_User_Groups { get; set; }
        public System.Data.Entity.DbSet<X_Coordinator_Group> X_Coordinator_Groups { get; set; }



    }
}