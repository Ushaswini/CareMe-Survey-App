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

namespace Homework05.API_Controllers
{
    [Authorize(Roles ="Admin, StudyCoordinator")]
    public class QuestionsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Questions
        public IQueryable<Question> GetQuestions()
        {
            return db.Questions;
        }

        // GET: api/Questions/5
        [ResponseType(typeof(Question))]
        public IHttpActionResult GetQuestion(string id)
        {
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return NotFound();
            }

            return Ok(question);
        }

        // PUT: api/Questions/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutQuestion(int id, Question question)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != question.Id)
            {
                return BadRequest();
            }

            db.Entry(question).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionExists(id))
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

        // POST: api/Questions
        [ResponseType(typeof(Question))]
        public IHttpActionResult PostQuestion(Question question)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (hasError(question))
            {
                return BadRequest(ModelState);
            }

            db.Questions.Add(question);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (QuestionExists(question.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = question.Id }, question);
        }

        private bool hasError(Question question) {

            bool hasError = false;

            switch (question.QuestionType.ToString()) {
                case "Scale":
                    if (question.Minimum < 0)
                    {
                        ModelState.AddModelError("Minimum", "Min should be greater than zero");
                        hasError = true;
                    }
                    if (question.Maximum < 0)
                    {
                        ModelState.AddModelError("Maximum", "Max should be greater than zero");
                        hasError = true;
                    }
                    if (question.Maximum < question.Minimum)
                    {
                        ModelState.AddModelError("StepSize", "Max should be greater than Min");
                        hasError = true;
                    }
                    if (question.StepSize < question.Minimum && question.StepSize > question.Maximum)
                    {
                        ModelState.AddModelError("StepSize", "Step Size should be in the range of Min and Max");
                        hasError = true;

                    }
                    if (((question.Maximum - question.Minimum) / question.StepSize) > 10)
                    {
                        ModelState.AddModelError("StepSize", "No of steps should be less than 10");
                        hasError = true;
                    }

                    break;
            }

            ModelState.AddModelError("","");


            return hasError;
        }

        // DELETE: api/Questions/5
        [ResponseType(typeof(Question))]
        public IHttpActionResult DeleteQuestion(string id)
        {
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return NotFound();
            }

            db.Questions.Remove(question);
            db.SaveChanges();

            return Ok(question);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool QuestionExists(int id)
        {
            return db.Questions.Count(e => e.Id == id) > 0;
        }
    }
}