using Homework05.DTOs;
using Homework05.Identity;
using Homework05.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Homework05.API_Controllers
{
    [Authorize]
    [RoutePrefix("api/Users")]
    public class UsersController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private ApplicationRoleManager _AppRoleManager = null;

        protected ApplicationRoleManager AppRoleManager
        {
            get
            {
                return _AppRoleManager ?? Request.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            set
            {
                _userManager = value;
            }
        }
        // GET: api/Users

        public List<UserDTO> GetAllUsers()
        {
            string roleName = "User";
            var role = AppRoleManager.Roles.Single(r => r.Name == roleName);
            /*var users = from user in UserManager.Users
            where user.Roles.Any(r => r.RoleId == role.Id)
            select user;
            var usersInRoles = users.Include("StudyGroup");
            var usersDTO = usersInRoles
                                       .Select(u => new UserDTO
                                       {
                                           Id = u.Id,
                                           UserName = u.UserName,
                                           Email = u.Email,
                                           StudyGroupId = u.StudyGroupId,
                                           StudyGroupName = u.StudyGroup.StudyName
                                       }).ToList();

            return usersDTO;*/

            // Find the users in that role
            var roleUsers = UserManager.Users
                //.Include(u => u.StudyGroup)
                .Where(u => u.Roles.Any(r => r.RoleId == role.Id))

                .Select(u => new UserDTO
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    //StudyGroupId = u.StudyGroupId,
                   // StudyGroupName = u.StudyGroup.StudyName
                }).ToList();

            return roleUsers;
        }

        [Route("Coordinator")]
        public IList<UserDTO>GetUsersForSC(string coordinatorId)
        {
            List<UserDTO> users = new List<UserDTO>();

            var list = from user in db.Users
                       join user_group in db.X_User_Groups on user.Id equals user_group.UserId
                       join study_group in db.StudyGroups on user_group.StudyGroupId equals study_group.Id
                       join sc_group in db.X_Coordinator_Groups on user_group.StudyGroupId equals sc_group.StudyGroupId
                       where sc_group.CoordinatorId.Equals(coordinatorId)
                       select new { user,user_group,sc_group, study_group};

           foreach(var u in list)
            {
                users.Add(new UserDTO {
                    Id = u.user.Id,
                    UserName = u.user.UserName,
                    Email = u.user.Email,
                    StudyGroupId = u.user_group.StudyGroupId,
                    StudyGroupName = u.study_group.StudyGroupName
                });
            }
            return users;
        }
        [Route("UpdateDeviceId")]
        [ResponseType(typeof(DeviceIdModel))]
        public IHttpActionResult PostDeviceId(Device device)
        {
            var userToModify = db.Users.Where(u => u.Id == device.UserId).FirstOrDefault();
            if (userToModify == null)
            {
                return NotFound();
            }

            var deviceAlreadyPresent = db.Devices.Where(d => d.DeviceId.Equals(device.DeviceId)).FirstOrDefault();

            if(deviceAlreadyPresent == null)
            {
                //device.Id = System.Guid.NewGuid().ToString();
                //string deviceId = "";
                db.Devices.Add(device);

                // userToModify.DeviceId = deviceId;
                // db.Entry(userToModify).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    // Retrieve the error messages as a list of strings.
                    var errorMessages = ex.EntityValidationErrors
                            .SelectMany(x => x.ValidationErrors)
                            .Select(x => x.ErrorMessage);

                    // Join the list to a single string.
                    var fullErrorMessage = string.Join("; ", errorMessages);

                    // Combine the original exception message with the new one.
                    var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                    // Throw a new DbEntityValidationException with the improved exception message.
                    throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(device.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            
           

            return Ok(device);
        }

        private bool UserExists(string id)
        {
            return db.Users.Count(e => e.Id == id) > 0;
        }
    }
}
