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
using Homework05.DTOs;
using System.Text;
using System.IO;
using Hangfire;
using Newtonsoft.Json;
using Homework05.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Homework05.API_Controllers
{
    //Get stored surveys
    //Get surveys for study coordinator
    //Get surveys for study group
    //Get surveys for user
    [Authorize]
    [RoutePrefix("api/Surveys")]
    public class SurveysController : ApiController
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
        //Get stored surveys
        // GET: api/Surveys
        public IList<SurveyDTO> GetSurveys()
        {
            List<SurveyDTO> surveysSaved = new List<SurveyDTO>();

            var surveysInDb = db.Surveys.ToList();
            foreach (var s in surveysInDb)
            {
                var questionsInSurvey = db.X_Survey_Questions
                                            .Include(q => q.Question)
                                            .Where(q => q.SurveyId == s.Id)
                                            .Select(q => new QuestionDTO
                                            {
                                                Id = q.Id,
                                                QuestionText = q.Question.QuestionText,
                                                QuestionType = q.Question.QuestionType,
                                                Maximum = q.Question.Maximum,
                                                Minimum = q.Question.Minimum,
                                                StepSize = q.Question.StepSize,
                                                Options = q.Question.Options
                                            });
                                            
                var survey = new SurveyDTO
                {
                    SurveyId = s.Id,
                    SurveyName = s.SurveyName,
                    SurveyType = s.SurveyType,
                    Questions = questionsInSurvey.ToList()
                };

                surveysSaved.Add(survey);
            }

            return surveysSaved;




            /*return db.Surveys.Include(s => s.StudyGroup).Include(s => s.Question).Select(s => new SurveyDTO
            {
                SurveyId = s.SurveyId,
                SurveyCreatedTime = s.SurveyCreatedTime,
                QuestionText = s.Question.QuestionText,
                StudyGroupId = s.StudyGroupId,
                StudyGroupName = s.StudyGroup.StudyName,
                QuestionId = s.QuestionId,
                QuestionType = s.Question.QuestionType,
                Options = s.Question.Options,
                QuestionFrequency = ((Frequency)s.FrequencyOfNotifications).ToString(),
                Time1 = s.Time1,
                Time2 = s.Time2

            }).ToList();*/


        }

        [Route("CoordinatorSurveys")]
        //Get surveys for study coordinator
        public IList<SurveyDTO> GetSurveysForStudyCoordinator(string coordinatorId)
        {
            List<SurveyDTO> surveys = new List<SurveyDTO>();

            var surveysForCoordinator = from survey_group in db.X_Survey_Groups.Include("Survey")
                                      join coordinator_group in db.X_Coordinator_Groups.Include("Coordinator") on survey_group.StudyGroupId equals coordinator_group.StudyGroupId
                                       join studyGroup in db.StudyGroups on survey_group.StudyGroupId equals studyGroup.Id
                                        where coordinator_group.CoordinatorId.Equals(coordinatorId)
                                      select new { survey_group, coordinator_group, studyGroup, Survey = survey_group.Survey, Coordinator = coordinator_group.Coordinator };


          /*  var surveysForCoordinator = db.X_Survey_Groups
                                            .Include(s => s.Survey)
                                            .Join(db.X_Coordinator_Groups
                                                    .Include(g => g.Coordinator)
                                                    .Where(g => g.CoordinatorId.Equals(coordinatorId)),
                                                  survey_group => survey_group.StudyGroupId,
                                                  group => group.Id,
                                                  (survey_group, group) => new { survey_group, group });*/

            foreach (var s in surveysForCoordinator.ToList())
            {
                var questionsInSurvey = db.X_Survey_Questions
                                            .Include(q => q.Question)
                                            .Where(q => q.SurveyId == s.survey_group.SurveyId)
                                            .Select(q => new QuestionDTO
                                            {
                                                Id = q.QuestionId,
                                                QuestionText = q.Question.QuestionText,
                                                QuestionType = q.Question.QuestionType,
                                                Maximum = q.Question.Maximum,
                                                Minimum = q.Question.Minimum,
                                                StepSize = q.Question.StepSize,
                                                Options = q.Question.Options
                                            })
                                            .ToList();
                var survey = new SurveyDTO
                {
                    SurveyId = s.survey_group.Id,
                    StudyGroupId = s.survey_group.StudyGroupId,
                    StudyGroupName = s.studyGroup.StudyGroupName,
                    SurveyName = s.Survey.SurveyName,
                    StudyCoordinatorId = s.Coordinator.Id,
                    StudyCoordinatorName = s.Coordinator.UserName,
                    SurveyType = s.survey_group.Survey.SurveyType,
                    Questions = questionsInSurvey.ToList()
                };

                surveys.Add(survey);
            }

            return surveys;
        }

        [Route("StudyGroupSurveys")]
        //Get surveys for study group
        public IList<SurveyDTO> GetSurveysForStudyGroup(int studyGroupId)
        {
            List<SurveyDTO> surveysSaved = new List<SurveyDTO>();

           var surveysInStudyGroup =   from survey_group in db.X_Survey_Groups.Include("Survey")
                                       join coordinator_group in db.X_Coordinator_Groups.Include("Coordinator") on survey_group.StudyGroupId equals coordinator_group.StudyGroupId
                                       where survey_group.StudyGroupId == studyGroupId
                                       select new {survey_group, coordinator_group, Survey = survey_group.Survey, Coordinator = coordinator_group.Coordinator };
           
            /* var surveysInStudyGroup = db.X_Survey_Groups
                                            .Include(s => s.Survey)
                                            .Where(s => s.StudyGroupId == studyGroupId)
                                            .Join(db.X_Coordinator_Groups.Include(g => g.Coordinator),
                                                    survey_group => survey_group.StudyGroupId,
                                                    group => group.Id,
                                                    (survey_group, group) => new { survey_group, group })
                                                    .ToList();*/

            foreach (var s in surveysInStudyGroup.ToList())
            {
                var questionsInSurvey = db.X_Survey_Questions
                                            .Include(q => q.Question)
                                            .Where(q => q.SurveyId == s.survey_group.SurveyId)
                                            .Select(q => new QuestionDTO
                                            {
                                                Id = q.Question.Id,
                                                QuestionText = q.Question.QuestionText,
                                                QuestionType = q.Question.QuestionType,
                                                Maximum = q.Question.Maximum,
                                                Minimum = q.Question.Minimum,
                                                StepSize = q.Question.StepSize,
                                                Options = q.Question.Options
                                            })
                                            .ToList();
                var survey = new SurveyDTO
                {
                    SurveyId = s.survey_group.Id,
                    StudyGroupId = s.survey_group.StudyGroupId,
                    SurveyName = s.Survey.SurveyName,
                    StudyCoordinatorId = s.Coordinator.Id,
                    StudyCoordinatorName = s.Coordinator.UserName,
                    SurveyType = s.survey_group.Survey.SurveyType,
                    Questions = questionsInSurvey.ToList()
                };

                surveysSaved.Add(survey);
            }

            return surveysSaved;
            /*var surveys = db.Surveys.Where(s => s.StudyGroupId == studyGroupId).Include(s => s.Question).Include(s => s.StudyGroup).Select(s => new SurveyDTO
            {
                SurveyId = s.SurveyId,
                SurveyCreatedTime = s.SurveyCreatedTime,
                QuestionText = s.Question.QuestionText,
                StudyGroupId = s.StudyGroupId,
                StudyGroupName = s.StudyGroup.StudyName,
                QuestionId = s.QuestionId,
                QuestionType = s.Question.QuestionType,
                Options = s.Question.Options,
                QuestionFrequency = ((Frequency)s.FrequencyOfNotifications).ToString(),
                Time1 = s.Time1,
                Time2 = s.Time2

            });

            return surveys.ToList();*/

        }

        //Get surveys for user
        [Route("UserSurveys")]
        public IHttpActionResult GetSurveysForUser(string userId)
        {
            List<ResponseDTO> responses = new List<ResponseDTO>();
            List<SurveyDTO> surveys = new List<SurveyDTO>();

            var userRole = AppRoleManager.Roles.Single(r => r.Name == "User");
            var user = db.Users.Where(u => u.Roles.Any(r => r.RoleId == userRole.Id) && u.Id.Equals(userId)).FirstOrDefault();
            

            if(user == null)
            {
                return NotFound();
            }

            var idsOfSurveysTaken = db.SurveyResponses
                                        .Where(s => s.UserId.Equals(userId))
                                        .Select(s => s.SurveyId)
                                        .Distinct()                                        
                                        .ToList();
                                        //.GroupBy(s => s.SurveyId, s => s.SurveyId ,((k,v) => v);//.Select((key,value) => value.ToSurveyId).ToList();

            var userStudyGroupId = db.X_User_Groups.Where(u => u.UserId.Equals(userId)).FirstOrDefault();

            foreach (var id in idsOfSurveysTaken)
            {
                var responsesForQuestionsInSurvey = db.SurveyResponses
                                                        .Where(r => r.SurveyId == id)
                                                        .Include(r => r.Question)
                                                        .Select(r => new QuestionResponseDTO
                                                        {
                                                            ResponseReceivedTime = r.ResponseReceivedTime,
                                                            ResponseText = r.ResponseText,
                                                            QuestionText = r.Question.QuestionText,
                                                            QuestionId = r.Question.Id,
                                                            QuestionType = r.Question.QuestionType,
                                                            Options = r.Question.Options
                                                        })
                                                        .ToList();
                var survey = db.X_Survey_Groups.Where(s => s.Id == id).Include(s => s.Survey).FirstOrDefault();
                var responseDTO = new ResponseDTO
                {
                    SurveyId = id,
                    UserName = user.UserName,
                    QuestionResponses = responsesForQuestionsInSurvey,
                    SurveyType = survey.Survey.SurveyType,
                    StudyGroupId = survey.StudyGroupId
                };

                responses.Add(responseDTO);
            }


           // db.X_Survey_Groups.Include(s => s.Survey).Where(s => s.StudyGroupId == userStudyGroupId.StudyGroupId).
            var surveysNotResponsed = (from survey_group in db.X_Survey_Groups.Include("Survey")
                                      join coordinator_group in db.X_Coordinator_Groups.Include("Coordinator") on survey_group.StudyGroupId equals coordinator_group.StudyGroupId
                                       where survey_group.StudyGroupId.Equals(userStudyGroupId.StudyGroupId)
                                      // where !idsOfSurveysTaken.Contains(survey_group.SurveyId)
                                       select new 
                                       {
                                           X_Survey_Id = survey_group.SurveyId,
                                           SurveyId = survey_group.Id,
                                           StudyGroupId = survey_group.StudyGroupId,
                                           SurveyName = survey_group.Survey.SurveyName,
                                           SurveyType = survey_group.Survey.SurveyType,
                                           Coordinator = coordinator_group.Coordinator
                                       }).ToList();

            foreach (var s in surveysNotResponsed)
            {
                var questionsInSurvey = db.X_Survey_Questions
                                            .Include(q => q.Question)
                                            .Where(q => q.SurveyId == s.X_Survey_Id)
                                            .Select(q => new QuestionDTO
                                            {
                                                Id = q.Question.Id,
                                                QuestionText = q.Question.QuestionText,
                                                QuestionType = q.Question.QuestionType,
                                                Maximum = q.Question.Maximum,
                                                Minimum = q.Question.Minimum,
                                                StepSize = q.Question.StepSize,
                                                Options = q.Question.Options
                                            })
                                            .ToList();
                var survey = new SurveyDTO
                {
                    SurveyId = s.SurveyId,
                    StudyGroupId = s.StudyGroupId,
                    SurveyName = s.SurveyName,
                    SurveyType = s.SurveyType,
                    Questions = questionsInSurvey.ToList(),
                    StudyCoordinatorId = s.Coordinator.Id,
                    StudyCoordinatorName = s.Coordinator.UserName,
                };

                if(!idsOfSurveysTaken.Contains(survey.SurveyId))surveys.Add(survey);
            }

            /* var surveysNotResponsed = (from r in db.Surveys.Include("Question")
                              where r.StudyGroupId.Equals(user.StudyGroupId)
                             where !idsOfSurveysTaken.Contains(r.SurveyId)                          
                             select new SurveyDTO
                             {
                               SurveyId = r.SurveyId,
                               SurveyCreatedTime = r.SurveyCreatedTime,
                               QuestionText = r.Question.QuestionText,
                               StudyGroupId = r.StudyGroupId,
                               StudyGroupName = r.StudyGroup.StudyName,
                               Options =r.Question.Options,
                               QuestionId = r.QuestionId,
                               QuestionType = r.Question.QuestionType,
                               QuestionFrequency = ((Frequency)r.FrequencyOfNotifications).ToString(),
                               Time1 = r.Time1,
                               Time2 = r.Time2
                             }).ToList();*/

            return Ok(new SurveysForUser { Surveys = surveys, SurveysResponded = responses });
        }


        // GET: api/Surveys/5
        [Route("GetSurvey")]
        [ResponseType(typeof(Survey))]
        public IHttpActionResult GetSurvey(int id)
        {
            Survey survey = db.Surveys.Find(id);
            if (survey == null)
            {
                return NotFound();
            }

            List<SurveyDTO> surveysSaved = new List<SurveyDTO>();

            var surveysInStudyGroup = from survey_group in db.X_Survey_Groups.Include("Survey")
                                      join coordinator_group in db.X_Coordinator_Groups.Include("Coordinator") on survey_group.StudyGroupId equals coordinator_group.StudyGroupId
                                      where survey_group.SurveyId == id
                                      select new { survey_group, coordinator_group, Survey = survey_group.Survey, Coordinator = coordinator_group.Coordinator };
            foreach (var s in surveysInStudyGroup.ToList())
            {
                var questionsInSurvey = db.X_Survey_Questions
                                            .Include(q => q.Question)
                                            .Where(q => q.SurveyId == s.survey_group.SurveyId)
                                            .Select(q => new QuestionDTO
                                            {
                                                Id = q.Question.Id,
                                                QuestionText = q.Question.QuestionText,
                                                QuestionType = q.Question.QuestionType,
                                                Maximum = q.Question.Maximum,
                                                Minimum = q.Question.Minimum,
                                                StepSize = q.Question.StepSize,
                                                Options = q.Question.Options
                                            })
                                            .ToList();
                var surveyDTO = new SurveyDTO
                {
                    SurveyId = s.survey_group.Id,
                    StudyGroupId = s.survey_group.StudyGroupId,
                    SurveyName = s.Survey.SurveyName,
                    StudyCoordinatorId = s.Coordinator.Id,
                    StudyCoordinatorName = s.Coordinator.UserName,
                    SurveyType = s.survey_group.Survey.SurveyType,
                    Questions = questionsInSurvey.ToList()
                };

                surveysSaved.Add(surveyDTO);
            }

           
            return Ok(surveysSaved[0]);
        }

        // PUT: api/Surveys/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSurvey(int id, Survey survey)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != survey.Id)
            {
                return BadRequest();
            }

            db.Entry(survey).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SurveyExists(id))
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

        [Route("Publish")]
        public IHttpActionResult PublishSurvey(PublishSurveyViewModel survey)
        {
            var surveyDetails = db.Surveys.Find(survey.SurveyId);
            if (surveyDetails != null)
            {
                if (surveyDetails.SurveyType == SurveyType.Message)
                {
                    //based on recurrence Add to X_Survey_Group

                    try
                    {
                        switch (survey.FrequencyOfNotifications)
                        {
                            case Frequency.Daily:
                                {
                                    String[] times = survey.Time1.Split(':');
                                    String cornExpression = times[1] + " " + times[0] + " * * * ";
                                    RecurringJob.AddOrUpdate(survey.SurveyId + "", () => SendNotification(survey), cornExpression, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
                                    break;
                                }
                            case Frequency.Hourly:
                                {
                                    SendNotification(survey);
                                    RecurringJob.AddOrUpdate(survey.SurveyId + "", () => SendNotification(survey), Cron.Hourly, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
                                    break;
                                }
                            case Frequency.TwiceDaily:
                                {
                                    String[] times = survey.Time1.Split(':');
                                    String cornExpression = times[1] + " " + times[0] + " * * *";
                                    String[] times2 = survey.Time2.Split(':');
                                    String cornExpression2 = times2[1] + " " + times2[0] + " * * *";
                                    RecurringJob.AddOrUpdate(survey.SurveyId + "First", () => SendNotification(survey), cornExpression, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
                                    RecurringJob.AddOrUpdate(survey.SurveyId + "Second", () => SendNotification(survey), cornExpression2, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
                                    break;
                                }
                        }
                    }
                    catch (DbUpdateException)
                    {
                        if (SurveyExists(survey.SurveyId.Value))
                        {
                            return Conflict();
                        }
                        else
                        {
                            throw;
                        }
                    }

                }
                else
                {
                    //add to X_Survey_Group and send right now
                    SendNotification(survey);
                }
            }
            else
            {
                return NotFound();
            }
            return Ok();
        }

        //Add survey to db -- not publishing
        // POST: api/Surveys
        [Route("Post")]
        [ResponseType(typeof(Survey))]
        public IHttpActionResult PostSurvey(SurveyViewModel survey)
        {
            var surveyToSave = new Survey { SurveyName = survey.SurveyName, SurveyType = survey.SurveyType };

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Surveys.Add(surveyToSave);
            try
            {
                db.SaveChanges();
            }
            catch (Exception oExcep)
            {
                Console.Write(oExcep.Message);
                throw;
            }

            var id = surveyToSave.Id;
            foreach (var question in survey.QuestionIds)
            {
                // add to X_Survey_Question
                db.X_Survey_Questions.Add(new X_Survey_Question { SurveyId = id, QuestionId = question });
            }

            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                throw;
            }

            return Ok(surveyToSave);

            /*
           
            return Ok(survey);*/


        }

        public void SendNotification(PublishSurveyViewModel surveyToSend)
        {
            try
            {
                var surveyToPublish = new X_Survey_Group
                {
                    SurveyId = surveyToSend.SurveyId.Value,
                    StudyGroupId = surveyToSend.StudyGroupId.Value,
                    SurveyCreatedTime = DateTime.Now.ToString(),
                    FrequencyOfNotifications = surveyToSend.FrequencyOfNotifications,
                    Time1 = surveyToSend.Time1,
                    Time2 = surveyToSend.Time2
                };


                db.X_Survey_Groups.Add(surveyToPublish);

                db.SaveChanges();

                List<string> deviceIds = new List<string>();
                var ids = from user_group in db.X_User_Groups where user_group.StudyGroupId == surveyToSend.StudyGroupId select new { UserId = user_group.UserId };
                //var userIdsInGroup = db.X_User_Groups.Where(u => u.StudyGroupId.Equals(surveyToSend.StudyGroupId)).Select(u =>  u.UserId ).ToList();
                foreach (var userId in ids.ToList())
                {
                    var devicesForOneUser = db.Devices.Where(d => d.UserId.Equals(userId.UserId));
                    foreach (var device in devicesForOneUser)
                    {
                        deviceIds.Add(device.DeviceId);
                    }
                }

                var survey = db.Surveys.Find(surveyToSend.SurveyId);

                String messageToDisplay = "";
                if (survey != null)
                {
                    if (survey.SurveyType == SurveyType.Message)
                    {
                        var question = db.X_Survey_Questions.Where(q => q.SurveyId == survey.Id).Include(q => q.Question).FirstOrDefault();
                        messageToDisplay = question.Question.QuestionText;
                    }
                    else
                    {
                        messageToDisplay = survey.SurveyName + " published!!";
                    }

                }
                SurveyPushNotification notification = new SurveyPushNotification
                {
                    RegisteredDeviceIds = deviceIds,
                    Data = new PushNotificationData
                    {
                        Message = messageToDisplay,
                        Time = DateTime.Now.ToString()
                    }

                };
                if (deviceIds.Count > 0)
                {
                    var applicationID = "AIzaSyC0Ian0Yr7JK9tZEi7-dZ3GcO-2dzomG1M";
                    // applicationID means google Api key 
                    var SENDER_ID = "283278634859";
                    // SENDER_ID is nothing but your ProjectID (from API Console- google code)  

                    string serializedNotification = JsonConvert.SerializeObject(notification);

                    WebRequest tRequest;

                    tRequest = WebRequest.Create("https://android.googleapis.com/gcm/send");

                    tRequest.Method = "post";

                    tRequest.ContentType = " application/json";

                    tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));

                    tRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));

                    Byte[] byteArray = Encoding.UTF8.GetBytes(serializedNotification);
                    tRequest.ContentLength = byteArray.Length;
                    Stream dataStream = tRequest.GetRequestStream();
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();
                    WebResponse tResponse = tRequest.GetResponse();
                    dataStream = tResponse.GetResponseStream();
                    StreamReader tReader = new StreamReader(dataStream);
                    String sResponseFromServer = tReader.ReadToEnd();   //Get response from GCM server.
                    Console.WriteLine(sResponseFromServer);
                    tReader.Close();
                    dataStream.Close();
                    tResponse.Close();
                }
            }
            catch (Exception e)
            {

            }




        }

        // DELETE: api/Surveys/5
        [ResponseType(typeof(Survey))]
        public IHttpActionResult DeleteSurvey(string id)
        {
            Survey survey = db.Surveys.Find(id);
            if (survey == null)
            {
                return NotFound();
            }

            db.Surveys.Remove(survey);
            db.SaveChanges();

            return Ok(survey);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SurveyExists(int id)
        {
            return db.Surveys.Count(e => e.Id == id) > 0;
        }
    }
}