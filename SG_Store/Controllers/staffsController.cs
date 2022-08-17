using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SG_Store.Models;

namespace SG_Store.Controllers
{
    public class staffsController : Controller
    {
        private SG_StoreEntities db = new SG_StoreEntities();

        // GET: staffs
        public ActionResult Index()
        {
            var staffs = db.staffs.Include(s => s.staff1).Include(s => s.store);
            return View(staffs.ToList());
        }

        // GET: staffs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            staff staff = db.staffs.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            return View(staff);
        }

        // GET: staffs/Create
        public ActionResult Create()
        {
            ViewBag.manager_id = new SelectList(db.staffs, "staff_id", "first_name");
            ViewBag.store_id = new SelectList(db.stores, "store_id", "store_name");
            return View();
        }

        // POST: staffs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "staff_id,first_name,last_name,email,phone,active,store_id,manager_id")] staff staff)
        {
            if (ModelState.IsValid)
            {
                db.staffs.Add(staff);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.manager_id = new SelectList(db.staffs, "staff_id", "first_name", staff.manager_id);
            ViewBag.store_id = new SelectList(db.stores, "store_id", "store_name", staff.store_id);
            return View(staff);
        }

        // GET: staffs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            staff staff = db.staffs.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            ViewBag.manager_id = new SelectList(db.staffs, "staff_id", "first_name", staff.manager_id);
            ViewBag.store_id = new SelectList(db.stores, "store_id", "store_name", staff.store_id);
            return View(staff);
        }

        // POST: staffs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "staff_id,first_name,last_name,email,phone,active,store_id,manager_id")] staff staff)
        {
            if (ModelState.IsValid)
            {
                db.Entry(staff).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.manager_id = new SelectList(db.staffs, "staff_id", "first_name", staff.manager_id);
            ViewBag.store_id = new SelectList(db.stores, "store_id", "store_name", staff.store_id);
            return View(staff);
        }

        // GET: staffs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            staff staff = db.staffs.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            return View(staff);
        }

        // POST: staffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            staff staff = db.staffs.Find(id);
            db.staffs.Remove(staff);
            db.SaveChanges();
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
