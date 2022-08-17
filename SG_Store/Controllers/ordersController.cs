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
    public class ordersController : Controller
    {
        private SG_StoreEntities db = new SG_StoreEntities();

        public void UpdateOrderTotal(int orderid)
        {
            order order = db.orders.Find(orderid);
            if (order != null)
            {
                decimal sub_total = 0;
                var order_items = db.order_items.Where(x => x.order_id == order.order_id).ToList();
                foreach(var item in order_items)
                {
                    sub_total += (item.total_amount.HasValue) ? item.total_amount.Value : 0;
                }
                order.sub_total = sub_total;
                order.total_tax = 0;
                order.grand_total = sub_total;

                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        // GET: orders
        public ActionResult Index()
        {
            var orders = db.orders.Include(o => o.customer).Include(o => o.staff).Include(o => o.store);
            return View(orders.ToList());
        }

        // GET: orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            order order = db.orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: orders/Create
        public ActionResult Create()
        {
            ViewBag.customer_id = new SelectList(db.customers, "customer_id", "first_name");
            ViewBag.staff_id = new SelectList(db.staffs, "staff_id", "first_name");
            ViewBag.store_id = new SelectList(db.stores, "store_id", "store_name");
            return View();
        }

        // POST: orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "order_id,customer_id,order_status,order_date,required_date,shipped_date,store_id,staff_id")] order order)
        {
            if (ModelState.IsValid)
            {
                db.orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.customer_id = new SelectList(db.customers, "customer_id", "first_name", order.customer_id);
            ViewBag.staff_id = new SelectList(db.staffs, "staff_id", "first_name", order.staff_id);
            ViewBag.store_id = new SelectList(db.stores, "store_id", "store_name", order.store_id);
            return View(order);
        }

        // GET: orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            order order = db.orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.customer_id = new SelectList(db.customers, "customer_id", "first_name", order.customer_id);
            ViewBag.staff_id = new SelectList(db.staffs, "staff_id", "first_name", order.staff_id);
            ViewBag.store_id = new SelectList(db.stores, "store_id", "store_name", order.store_id);
            return View(order);
        }

        // POST: orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "order_id,customer_id,order_status,order_date,required_date,shipped_date,store_id,staff_id")] order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.customer_id = new SelectList(db.customers, "customer_id", "first_name", order.customer_id);
            ViewBag.staff_id = new SelectList(db.staffs, "staff_id", "first_name", order.staff_id);
            ViewBag.store_id = new SelectList(db.stores, "store_id", "store_name", order.store_id);
            return View(order);
        }

        // GET: orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            order order = db.orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            order order = db.orders.Find(id);
            db.orders.Remove(order);
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
