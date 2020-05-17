using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ContactAPI.Models;
using Newtonsoft.Json;

namespace ContactAPI.Controllers
{
    public class HomeController : Controller
    {


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {

            return View(new ContactViewModel());
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            HttpClient client = new HttpClient();
            Contact contact = null;
            HttpResponseMessage response = await client.GetAsync("http://"+ Request.Url.Host +":" + Request.Url.Port + "/api/Contact/" + id);
            if (response.IsSuccessStatusCode)
            {
                contact = await response.Content.ReadAsAsync<Contact>();
            }

            
            if (contact == null)
            {
                return HttpNotFound();
            }

            ContactViewModel cvm = new ContactViewModel() {
                ContactId = contact.ContactId,
                FirstName= contact.FirstName,
                LastName= contact.LastName,
                Email= contact.Email,
                PhoneNo= contact.PhoneNo,
                Status= contact.Status
            };
            client.Dispose();
            return View(cvm);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //

            HttpClient client = new HttpClient();
            Contact cc = null;
            HttpResponseMessage response = await client.GetAsync("https://localhost:44386/api/Contact/" + id);
            if (response.IsSuccessStatusCode)
            {
                cc = await response.Content.ReadAsAsync<Contact>();
            }

            //


            if (cc == null)
            {
                return HttpNotFound();
            }


            ContactViewModel ccc = new ContactViewModel()
            {
                ContactId = cc.ContactId,
                FirstName = cc.FirstName,
                LastName = cc.LastName,
                Email = cc.Email,
                PhoneNo = cc.PhoneNo,
                Status = cc.Status
            };
            return View(ccc);
        }
    }
}
