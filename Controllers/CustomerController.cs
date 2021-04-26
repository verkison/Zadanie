using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Zadanie.Models;

namespace Zadanie.Controllers
{
    public class CustomerController : Controller
    {
        private DBContext db = new DBContext();

        public async Task<ActionResult> Index(string searchBy, string search)
        {
            var customers = db.Customers.Include(c => c.CustomerStatus);
            var emails = db.Emails.Include(e => e.Customer);
            var phones = db.Phones.Include(p => p.Customer);

            switch (searchBy)
            {
                case "FirstName":
                    customers = customers.Where(c => c.FirstName.Contains(search));
                    break;
                case "LastName":
                    customers = customers.Where(c => c.LastName.Contains(search));
                    break;
                case "Address":
                    customers = customers.Where(c => c.Address.Contains(search));
                    break;

                case "Status1":
                    customers = customers.Where(c => c.CustomerStatus.Status == "Potencjalny");
                    break;
                case "Status2":
                    customers = customers.Where(c => c.CustomerStatus.Status == "Obecny");
                    break;

                case "Opcja1":
                    customers = customers.Where(c => c.EmailQuantity == 0 && c.PhoneQuantity == 0);
                    break;
                case "Opcja2":
                    customers = customers.Where(c => c.EmailQuantity > 1);
                    break;

                default:
                    break;
            }

            return View(await customers.ToListAsync());
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = await db.Customers.FindAsync(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        public ActionResult Create()
        {
            ViewBag.StatusID = new SelectList(db.CustomerStatus, "ID", "Status");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,FirstName,LastName,Address,StatusID")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.StatusID = new SelectList(db.CustomerStatus, "ID", "Status", customer.StatusID);
            return View(customer);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = await db.Customers.FindAsync(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            ViewBag.StatusID = new SelectList(db.CustomerStatus, "ID", "Status", customer.StatusID);
            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,FirstName,LastName,Address,StatusID")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.StatusID = new SelectList(db.CustomerStatus, "ID", "Status", customer.StatusID);
            return View(customer);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = await db.Customers.FindAsync(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Customer customer = await db.Customers.FindAsync(id);
            var emails = db.Emails.Include(e => e.Customer);
            var phones = db.Phones.Include(p => p.CustomerID);

            foreach (var e in emails)
            {
                if (e.CustomerID == id)
                    db.Emails.Remove(e);
            }

            foreach (var p in phones)
            {
                if (p.CustomerID == id)
                    db.Phones.Remove(p);
            }

            db.Customers.Remove(customer);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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