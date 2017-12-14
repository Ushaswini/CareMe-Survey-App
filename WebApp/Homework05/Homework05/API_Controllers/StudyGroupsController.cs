using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Homework05.Models;
using System.Data.Entity.Validation;

namespace Homework05.API_Controllers
{
    public class StudyGroupsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/StudyGroups
        public IQueryable<StudyGroup> GetStudyGroups()
        {
            return db.StudyGroups;
        }

        //TODO
        public IList<StudyGroup> GetStudyGroupsForCoordinator(string coordinatorId)
        {
           // var groups = db.StudyGroups.Where(s => s.StudyCoordinatorId.Equals(coordinatorId));
            return null;
        }

        // GET: api/StudyGroups/5
        [ResponseType(typeof(StudyGroup))]
        public IHttpActionResult GetStudyGroup(string id)
        {
            StudyGroup studyGroup = db.StudyGroups.Find(id);
            if (studyGroup == null)
            {
                return NotFound();
            }

            return Ok(studyGroup);
        }

        // PUT: api/StudyGroups/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutStudyGroup(int id, StudyGroup studyGroup)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != studyGroup.Id)
            {
                return BadRequest();
            }

            db.Entry(studyGroup).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudyGroupExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/StudyGroups
        [ResponseType(typeof(StudyGroup))]
        public IHttpActionResult PostStudyGroup(StudyGroupViewModel studyGroup)
        {
            //Add group to StudyGroup and X_Coordinator_Group tables
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var group = new StudyGroup { StudyGroupCreatedTime = studyGroup.StudyGroupCreadtedTime, StudyGroupName = studyGroup.StudyGroupName};
            db.StudyGroups.Add(group);
            db.X_Coordinator_Groups.Add(new X_Coordinator_Group { StudyGroupId = group.Id, CoordinatorId = studyGroup.StudyCoordinatorId});

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (StudyGroupExists(studyGroup.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
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

            return CreatedAtRoute("DefaultApi", new { id = studyGroup.Id }, group);
        }

        // DELETE: api/StudyGroups/5
        [ResponseType(typeof(StudyGroup))]
        public IHttpActionResult DeleteStudyGroup(string id)
        {
            StudyGroup studyGroup = db.StudyGroups.Find(id);
            if (studyGroup == null)
            {
                return NotFound();
            }

            db.StudyGroups.Remove(studyGroup);
            db.SaveChanges();

            return Ok(studyGroup);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StudyGroupExists(int id)
        {
            return db.StudyGroups.Count(e => e.Id == id) > 0;
        }
    }
}