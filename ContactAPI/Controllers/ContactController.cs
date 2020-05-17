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
using System.Web.Http.Results;
using ContactAPI;

namespace ContactAPI.Controllers
{
    public class ContactController : ApiController
    {

        private tempdbEntities db = new tempdbEntities();

        // GET: api/Contact
        /// <summary>
        /// Get all Contact.
        /// </summary>
        /// <returns>
        /// List of Contact.
        /// </returns>
        /// <param></param>
        [HttpGet]
        public System.Web.Http.IHttpActionResult GetContacts()
        {
            return Json(new { data = db.Contacts });
        }

        // GET: api/Contact/5
        /// <summary>
        /// Get Contact by ID.
        /// </summary>
        /// <returns>
        /// Contact.
        /// </returns>
        /// <param name="id">ID of Contact</param>
        [ResponseType(typeof(Contact))]
        [HttpGet]
        public IHttpActionResult GetContact(int id)
        {
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return NotFound();
            }

            return Ok(contact);
        }

        // PUT: api/Contact/5
        /// <summary>
        /// Update Contact.
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        /// <param name="id"> ID of Contact.</param>
        /// <param name="changedContact"> Updated Contact.</param>
        [ResponseType(typeof(void))]
        [HttpPut]
        public IHttpActionResult PutContact(int id, Contact changedContact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != changedContact.ContactId)
            {
                return BadRequest("Contact not found.");
            }

            db.Entry(changedContact).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactExists(id))
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

        // POST: api/Contact
        /// <summary>
        /// Save Contact.
        /// </summary>
        /// <returns>
        /// Contact.
        /// </returns>
        /// <param name="newContact"> Contact.</param>
        [ResponseType(typeof(Contact))]
        [HttpPost]
        public IHttpActionResult PostContact([FromBody] Contact newContact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // refactor to save ContactId from identity
            newContact.ContactId = db.Contacts.OrderByDescending(i => i.ContactId).FirstOrDefault().ContactId + 1;

            Contact cc = new Contact()
            {
                ContactId = newContact.ContactId,
                FirstName = newContact.FirstName,
                LastName = newContact.LastName,
                Email = newContact.Email,
                PhoneNo = newContact.PhoneNo,
                Status = false
            };


            try
            {
                using (var transaction = db.Database.BeginTransaction())
                {

                    db.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Contact] ON");

                    db.Contacts.Add(cc);
                    db.SaveChanges();

                    db.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Contact] OFF");

                    transaction.Commit();

                }

            }
            catch (DbUpdateException ex)
            {
                if (ContactExists(newContact.ContactId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = newContact.ContactId }, newContact);
        }

        // DELETE: api/Contact/5
        /// <summary>
        /// Delete Contact.
        /// </summary>
        /// <returns>
        /// Contact.
        /// </returns>
        /// <param name="id"> ID of Contact.</param>
        [ResponseType(typeof(Contact))]
        [HttpDelete]
        public IHttpActionResult DeleteContact(int id)
        {
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return NotFound();
            }

            db.Contacts.Remove(contact);
            db.SaveChanges();

            return Ok(contact);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ContactExists(int id)
        {
            return db.Contacts.Count(e => e.ContactId == id) > 0;
        }
    }
}