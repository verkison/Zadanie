using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Zadanie.Models;

namespace Zadanie.Controllers
{
    public class PhoneController : Controller
    {
        private DBContext db = new DBContext();

        public ActionResult Create(int? id)
        {
            Customer customer = db.Customers.Single(c => c.ID == id);

            ViewBag.CustomerData = customer.FirstName + " " + customer.LastName + ", " + customer.Address;
            ViewBag.id = id;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,PhoneContent,CustomerID")] Phone phone, int id)
        {
            var customer = db.Customers.Single(c => c.ID == id);
            ViewBag.CustomerData = customer.FirstName + " " + customer.LastName + ", " + customer.Address;

            Phone newPhone = new Phone()
            {
                ID = phone.ID,
                PhoneContent = phone.PhoneContent,
                CustomerID = id
            };

            if (ModelState.IsValid)
            {
                db.Phones.Add(newPhone);
                customer.PhoneQuantity++;
                await db.SaveChangesAsync();
                return RedirectToAction("Details", "Customer", new { id = id });
            }

            return View(phone);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            Phone phone = await db.Phones.FindAsync(id);
            Customer customer = db.Customers.Single(c => c.ID == phone.CustomerID);

            ViewBag.CustomerData = customer.FirstName + " " + customer.LastName + ", " + customer.Address;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (phone == null)
            {
                return HttpNotFound();
            }

            return View(phone);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,PhoneContent,CustomerID")] Phone phone)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phone).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Details", "Customer", new { id = phone.CustomerID });
            }

            return View(phone);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            Phone phone = await db.Phones.FindAsync(id);
            Customer customer = db.Customers.Single(c => c.ID == phone.CustomerID);

            ViewBag.CustomerData = customer.FirstName + " " + customer.LastName + ", " + customer.Address;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (phone == null)
            {
                return HttpNotFound();
            }
            return View(phone);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Phone phone = await db.Phones.FindAsync(id);
            var customer = db.Customers.Single(c => c.ID == phone.CustomerID);
            db.Phones.Remove(phone);
            customer.PhoneQuantity--;
            await db.SaveChangesAsync();
            return RedirectToAction("Details", "Customer", new { id = phone.CustomerID });
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