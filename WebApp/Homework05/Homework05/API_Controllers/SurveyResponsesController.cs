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
using System.Data.Entity.Validation;

namespace Homework05.API_Controllers
{
    [Authorize]
    [RoutePrefix("api/SurveyResponses")]
    public class SurveyResponsesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //Get responses for a study group
        //Get responses for a study coordinator
        //Get responses for a user
        //Get responses of a survey

        // GET: api/SurveyResponses
        public IList<ResponseDTO> GetSurveyResponses()
        {
            var ques = db.SurveyResponses.GroupBy(r => r.SurveyId);
            /* var result = db.SurveyResponses.Include(r => r.StudyGroup)
                                            .Include(r => r.Survey)
                                            .Include(r => r.Survey.Question)
                                            .Include(r => r.User)
                                            .Select(r => new ResponseDTO
                                            {
                                                StudyGroupName = r.StudyGroup.StudyName,
                                                SurveyId = r.SurveyId,
                                                UserName = r.User.UserName,
                                                ResponseReceivedTime = r.SurveyResponseReceivedTime,
                                                ResponseText = r.UserResponseText,
                                                QuestionFrequency = ((Frequency)r.Survey.FrequencyOfNotifications).ToString(),
                                                QuestionText = r.Survey.Question.QuestionText,
                                                QuestionId = r.Survey.Question.QuestionId,
                                                QuestionType = r.Survey.Question.QuestionType,
                                                Options = r.Survey.Question.Options,
                                                ResponseId = r.SurveyResponseId

                                                 // SurveyComments = r.SurveyComments
                                             });
             return result.ToList();*/

            return null;
        }

        [Route("StudyResponses")]
        //Get responses for a study group
        [ResponseType(typeof(SurveyResponse))]
        public IList<ResponseDTO> GetSurveyResponseForStudy(int studyGroupId)
        {
            List<ResponseDTO> responses = new List<ResponseDTO>();

            var idsOfSurveysInGroup = db.X_Survey_Groups.Where(g => g.StudyGroupId == studyGroupId).Select(g => g.Id).ToList();

            foreach (var surveyId in idsOfSurveysInGroup)
            {
                var responseList = from response in db.SurveyResponses.Include("Survey").Include("Question").Include("User")
                                   join question in db.Questions on response.QuestionId equals question.Id
                                   join survey in db.X_Survey_Groups.Include("Survey").Include("StudyGroup") on response.SurveyId equals survey.Id
                                   join study in db.StudyGroups on survey.StudyGroupId equals study.Id
                                   join user in db.Users on response.UserId equals user.Id
                                   where response.SurveyId == surveyId
                                   select new { response, question, survey, surveyObj = survey.Survey, study, user }
                                       ;
                var groupedResponses = responseList.GroupBy(p => p.response.SurveyId, p => new { p.response, p.question, p.survey, p.surveyObj, p.study, p.user }, (key, g) => new { SurveyId = key, Value = g.ToList() }).ToList();
                List<QuestionResponseDTO> questions = new List<QuestionResponseDTO>();
                foreach (var r in groupedResponses)
                {
                    questions.Clear();
                    foreach (var p in r.Value.ToList())
                    {

                        var questionDTO = new QuestionResponseDTO
                        {
                            ResponseReceivedTime = p.response.ResponseReceivedTime,
                            ResponseText = p.response.ResponseText,
                            QuestionText = p.question.QuestionText,
                            QuestionId = p.question.Id,
                            QuestionType = p.question.QuestionType,
                            QuestionFrequency = p.survey.FrequencyOfNotifications + "",
                            Options = p.question.Options,

                        };
                        questions.Add(questionDTO);

                    }

                    var responseDTO = new ResponseDTO
                    {
                        SurveyId = r.Value.ElementAt(0).response.SurveyId,
                        SurveyName = r.Value.ElementAt(0).surveyObj.SurveyName,
                        UserName = r.Value.ElementAt(0).user.UserName,
                        QuestionResponses = questions,
                        StudyGroupName = r.Value.ElementAt(0).study.StudyGroupName

                    };

                    responses.Add(responseDTO);
                }
            }

            //  var surveysForStudyGroup = db.X_Survey_Groups.Where(r => r.StudyGroupId == studyGroupId);

            /*var surveysForStudyGroup = from u in db.X_Survey_Groups
                                join r in db.X_User_Groups.Include("User")
                                on u.StudyGroupId equals r.StudyGroupId
                                where u.StudyGroupId == studyGroupId
                                select new { r, u };

            foreach (var survey in surveysForStudyGroup.ToList())
            {
                var responsesForQuestionsInSurvey = db.SurveyResponses
                                                       .Where(r => r.SurveyId == survey.u.SurveyId)
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

                var responseDTO = new ResponseDTO
                {
                    SurveyId = survey.u.SurveyId,
                    UserName = survey.r.User.UserName,
                    QuestionResponses = responsesForQuestionsInSurvey
                };

                responses.Add(responseDTO);
            }*/

            return responses;
            /* var result = db.SurveyResponses.Include(r => r.StudyGroup)
                                             .Include(r => r.Survey)
                                             .Include(r => r.Survey.Question)
                                             .Include(r => r.User).Where(r => r.StudyGroupId == studyGroupId)
                                             .Select(r => new ResponseDTO
                                             {
                                                 StudyGroupName = r.StudyGroup.StudyName,
                                                 SurveyId = r.SurveyId,
                                                 UserName = r.User.UserName,
                                                 ResponseReceivedTime = r.SurveyResponseReceivedTime,
                                                 ResponseText = r.UserResponseText,
                                                 QuestionFrequency = ((Frequency)r.Survey.FrequencyOfNotifications).ToString(),
                                                 QuestionText = r.Survey.Question.QuestionText,
                                                 QuestionId = r.Survey.Question.QuestionId,
                                                 QuestionType = r.Survey.Question.QuestionType,
                                                 Options = r.Survey.Question.Options,
                                                 ResponseId = r.SurveyResponseId

                                                 // SurveyComments = r.SurveyComments
                                             });
             return result.ToList();*/           


        }

        [Route("CoordinatorSurveyResponses")]
        //Get responses for a coordinator
        [ResponseType(typeof(SurveyResponse))]
        public IList<ResponseDTO> GetResponsesForStudyCoordinator(int coordinatorId)
        {
            List<ResponseDTO> responses = new List<ResponseDTO>();

            var idsOfSurveysForCoordinator = from s in db.X_Coordinator_Groups

                                             join g in db.X_Survey_Groups.Include("Survey") on s.Id equals g.StudyGroupId

                                             where s.CoordinatorId.Equals(coordinatorId)

                                             select g.Id;

            foreach (var surveyId in idsOfSurveysForCoordinator.ToList())
            {
                var responseList = from response in db.SurveyResponses.Include("Survey").Include("Question").Include("User")
                                   join question in db.Questions on response.QuestionId equals question.Id
                                   join survey in db.X_Survey_Groups.Include("Survey").Include("StudyGroup") on response.SurveyId equals survey.Id
                                   join study in db.StudyGroups on survey.StudyGroupId equals study.Id
                                   join user in db.Users on response.UserId equals user.Id
                                   where response.SurveyId == surveyId
                                   select new { response, question, survey, surveyObj = survey.Survey, study, user }
                                       ;
                var groupedResponses = responseList.GroupBy(p => p.response.SurveyId, p => new { p.response, p.question, p.survey, p.surveyObj, p.study, p.user }, (key, g) => new { SurveyId = key, Value = g.ToList() }).ToList();
                List<QuestionResponseDTO> questions = new List<QuestionResponseDTO>();
                foreach (var r in groupedResponses)
                {
                    questions.Clear();
                    foreach (var p in r.Value.ToList())
                    {

                        var questionDTO = new QuestionResponseDTO
                        {
                            ResponseReceivedTime = p.response.ResponseReceivedTime,
                            ResponseText = p.response.ResponseText,
                            QuestionText = p.question.QuestionText,
                            QuestionId = p.question.Id,
                            QuestionType = p.question.QuestionType,
                            QuestionFrequency = p.survey.FrequencyOfNotifications + "",
                            Options = p.question.Options,

                        };
                        questions.Add(questionDTO);

                    }

                    var responseDTO = new ResponseDTO
                    {
                        SurveyId = r.Value.ElementAt(0).response.SurveyId,
                        SurveyName = r.Value.ElementAt(0).surveyObj.SurveyName,
                        UserName = r.Value.ElementAt(0).user.UserName,
                        QuestionResponses = questions,
                        StudyGroupName = r.Value.ElementAt(0).study.StudyGroupName

                    };

                    responses.Add(responseDTO);
                }
            }
            return responses;
        }

        [Route("SurveyResponses")]
        //Get responses for a survey
        [ResponseType(typeof(SurveyResponse))]
        public IList<ResponseDTO> GetResponsesForSurvey(int surveyId)
        {
            List<ResponseDTO> responses = new List<ResponseDTO>();

            var usersInSurvey = from u in db.X_Survey_Groups
                                join r in db.X_User_Groups.Include("User") on u.StudyGroupId equals r.StudyGroupId
                                where u.SurveyId == surveyId
                                select  new { r, u };

            foreach(var user in usersInSurvey.ToList())
            {
                var responsesForQuestionsInSurvey = db.SurveyResponses
                                                         .Where(r => r.SurveyId == surveyId)
                                                         .Where(r => r.UserId.Equals(user.r.UserId))
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

                var responseDTO = new ResponseDTO
                {
                    SurveyId = user.u.SurveyId,
                    UserName = user.r.User.UserName,
                    QuestionResponses = responsesForQuestionsInSurvey
                };

                responses.Add(responseDTO);
            }
            
            return responses;
        }

        [Route("UserResponses")]
        //Get responses for a user
        [ResponseType(typeof(SurveyResponse))]
        public IList<ResponseDTO> GetSurveyResponseOfUser(string userId)
        {
            List<ResponseDTO> responses = new List<ResponseDTO>();

            var user = db.Users.Where(u => u.Id.Equals(userId)).FirstOrDefault();

            var responseList = from response in db.SurveyResponses.Include("Survey").Include("Question")
                       join question in db.Questions on response.QuestionId equals question.Id
                       join survey in db.X_Survey_Groups.Include("Survey").Include("StudyGroup") on response.SurveyId equals survey.Id
                       join study in  db.StudyGroups on survey.StudyGroupId equals study.Id
                       //group response by response.SurveyId into g
                       select new { response, question, survey, surveyObj =  survey.Survey, study }
                       ;
            var groupedResponses = responseList.GroupBy(p => p.response.SurveyId, p => new { p.response, p.question,p.survey, p.surveyObj, p.study }, (key, g) => new { SurveyId = key, Value = g.ToList()} ).ToList();
            List<QuestionResponseDTO> questions = new List<QuestionResponseDTO>();
            foreach (var r in groupedResponses)
            {
                questions.Clear();
                foreach(var p in r.Value.ToList())
                {

                    var questionDTO = new QuestionResponseDTO
                    {
                        ResponseReceivedTime = p.response.ResponseReceivedTime,
                        ResponseText = p.response.ResponseText,
                        QuestionText = p.question.QuestionText,
                        QuestionId = p.question.Id,
                        QuestionType = p.question.QuestionType,
                        QuestionFrequency = p.survey.FrequencyOfNotifications + "",
                        Options = p.question.Options,
                        
                    };
                    questions.Add(questionDTO);
                    
                }

                var responseDTO = new ResponseDTO
                {
                    SurveyId = r.Value.ElementAt(0).response.SurveyId,
                    SurveyName = r.Value.ElementAt(0).surveyObj.SurveyName,
                    UserName = r.Value.ElementAt(0).response.User.UserName,
                    QuestionResponses = questions,
                    StudyGroupName = r.Value.ElementAt(0).study.StudyGroupName

                };

                responses.Add(responseDTO);  
            }
           /* var surveys = db.SurveyResponses
                            .Where(r => r.UserId.Equals(userId))
                            .Include("Question")
                            .Include("Survey")
                            .Select(s => new { s, Question = s.Question, survey = s.Survey})
                            .ToList()
                            ;
            
            //var test = surveys.GroupBy(r => r.SurveyId).ToList();

            //var groupsAssociated = db.X_User_Groups.Where(u => u.UserId.Equals(userId));


           /* var surveysOfUser = from u in db.X_Survey_Groups
                                join r in db.X_User_Groups.Include("User")
                                on u.StudyGroupId equals r.StudyGroupId
                                where r.UserId.Equals(userId)
                                select new { r, u };

            foreach (var survey in surveysOfUser.ToList())
            {
                var responsesForQuestionsInSurvey = db.SurveyResponses
                                                         .Where(r => r.SurveyId == survey.u.SurveyId)
                                                         .Where(r => r.UserId.Equals(survey.r.UserId))
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

                var responseDTO = new ResponseDTO
                {
                    SurveyId = survey.u.SurveyId,
                    UserName = survey.r.User.UserName,
                    QuestionResponses = responsesForQuestionsInSurvey
                };

                responses.Add(responseDTO);
            }*/

           /* foreach (var group in groupsAssociated)
            {
                var surveysForStudyGroup = db.X_Survey_Groups.Where(r => r.StudyGroupId == group.StudyGroupId);

                foreach (var s in surveysForStudyGroup)
                {
                    var responsesForQuestionsInSurvey = db.SurveyResponses
                                                           .Where(r => r.SurveyId == s.SurveyId)
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

                    var responseDTO = new ResponseDTO
                    {
                        SurveyId = s.SurveyId,
                        //UserName = user.UserName,
                        QuestionResponses = responsesForQuestionsInSurvey
                    };

                    responses.Add(responseDTO);
                }
            }*/

            

            return responses;
            /*
            var result = db.SurveyResponses.Include(r => r.StudyGroup)
                                            .Include(r => r.Survey)
                                            .Include(r => r.Survey.Question)
                                            .Include(r => r.User).Where(r => r.UserId == userId)
                                            .Select(r => new ResponseDTO
                                            {
                                                ResponseId = r.SurveyResponseId,
                                                StudyGroupName = r.StudyGroup.StudyName,
                                                SurveyId = r.SurveyId,
                                                UserName = r.User.UserName,
                                                ResponseReceivedTime = r.SurveyResponseReceivedTime,
                                                ResponseText = r.UserResponseText,
                                                QuestionFrequency = ((Frequency)r.Survey.FrequencyOfNotifications).ToString(),
                                                QuestionText = r.Survey.Question.QuestionText,
                                                QuestionId = r.Survey.Question.QuestionId,
                                                QuestionType = r.Survey.Question.QuestionType,
                                                Options = r.Survey.Question.Options                                            
                                            });
            return result.ToList();*/
           
        }

        // GET: api/SurveyResponses/5
        [ResponseType(typeof(SurveyResponse))]
        public IHttpActionResult GetSurveyResponse(string id)
        {
            SurveyResponse surveyResponse = db.SurveyResponses.Find(id);
            if (surveyResponse == null)
            {
                return NotFound();
            }

            return Ok(surveyResponse);
        }

        // PUT: api/SurveyResponses/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSurveyResponse(int id, SurveyResponse surveyResponse)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != surveyResponse.Id)
            {
                return BadRequest();
            }

            db.Entry(surveyResponse).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SurveyResponseExists(id))
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

        // POST: api/SurveyResponses
        [ResponseType(typeof(SurveyResponse))]
        public IHttpActionResult PostSurveyResponse(SurveyResponseViewModel surveyResponse)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //surveyResponse.Id = System.Guid.NewGuid();
            foreach(var resp in surveyResponse.Responses)
            {
                var response = new SurveyResponse {
                    SurveyId = surveyResponse.SurveyId,
                    UserId = surveyResponse.UserId,
                    QuestionId = resp.QuestionId,                    
                    ResponseText = resp.ResponseText,
                    ResponseReceivedTime = resp.ResponseReceivedTime
                };
                db.SurveyResponses.Add(response);
            }
            
            //Save responses
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (SurveyResponseExists(surveyResponse.Id))
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

            return CreatedAtRoute("DefaultApi", new { id = surveyResponse.Id }, surveyResponse);
        }

        // DELETE: api/SurveyResponses/5
        [ResponseType(typeof(SurveyResponse))]
        public IHttpActionResult DeleteSurveyResponse(string id)
        {
            SurveyResponse surveyResponse = db.SurveyResponses.Find(id);
            if (surveyResponse == null)
            {
                return NotFound();
            }

            db.SurveyResponses.Remove(surveyResponse);
            db.SaveChanges();

            return Ok(surveyResponse);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SurveyResponseExists(int id)
        {
            return db.SurveyResponses.Count(e => e.Id == id) > 0;
        }
    }
}