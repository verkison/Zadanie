using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Zadanie.Models;

namespace Zadanie.Controllers
{
    public class EmailController : Controller
    {
        private DBContext db = new DBContext();

        public ActionResult Create(int? id)
        {
            Customer customer = db.Customers.Single(c => c.ID == id);

            ViewBag.CustomerData = customer.FirstName + " " + customer.LastName + ", " + customer.Address;
            ViewBag.id = customer.ID;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,EmailContent,CustomerID")] Email email, int id)
        {
            var customer = db.Customers.Single(c => c.ID == id);
            ViewBag.id = customer.ID;
            ViewBag.CustomerData = customer.FirstName + " " + customer.LastName + ", " + customer.Address;

            Email newEmail = new Email()
            {
                ID = email.ID,
                EmailContent = email.EmailContent,
                CustomerID = id,
            };

            if (ModelState.IsValid)
            {
                db.Emails.Add(newEmail);
                customer.EmailQuantity++;
                await db.SaveChangesAsync();
                return RedirectToAction("Details", "Customer", new { id = id });
            }

            return View(email);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            Email email = await db.Emails.FindAsync(id);
            Customer customer = db.Customers.Single(c => c.ID == email.CustomerID);

            ViewBag.CustomerData = customer.FirstName + " " + customer.LastName + ", " + customer.Address;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (email == null)
            {
                return HttpNotFound();
            }

            return View(email);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,EmailContent,CustomerID")] Email email)
        {
            if (ModelState.IsValid)
            {
                db.Entry(email).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Details", "Customer", new { id = email.CustomerID });
            }

            return View(email);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            Email email = await db.Emails.FindAsync(id);
            Customer customer = db.Customers.Single(c => c.ID == email.CustomerID);

            ViewBag.CustomerData = customer.FirstName + " " + customer.LastName + ", " + customer.Address;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (email == null)
            {
                return HttpNotFound();
            }
            return View(email);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Email email = await db.Emails.FindAsync(id);
            var customer = db.Customers.Single(c => c.ID == email.CustomerID);
            db.Emails.Remove(email);
            customer.EmailQuantity--;
            await db.SaveChangesAsync();
            return RedirectToAction("Details", "Customer", new { id = email.CustomerID });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}