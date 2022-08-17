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
    public class stocksController : Controller
    {
        private SG_StoreEntities db = new SG_StoreEntities();

        // GET: stocks
        public ActionResult Index()
        {
            var stocks = db.stocks.Include(s => s.product).Include(s => s.store).Include(s => s.vendor);
            return View(stocks.ToList());
        }

        // GET: stocks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            stock stock = db.stocks.Find(id);
            if (stock == null)
            {
                return HttpNotFound();
            }
            return View(stock);
        }

        // GET: stocks/Create
        public ActionResult Create()
        {
            ViewBag.product_id = new SelectList(db.products, "product_id", "product_name");
            ViewBag.store_id = new SelectList(db.stores, "store_id", "store_name");
            ViewBag.vendor_id = new SelectList(db.vendors, "vendor_id", "vendor_name");
            return View();
        }

        // POST: stocks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "store_id,product_id,quantity,vendor_id,quantity_kg,MRP,purchase_price,is_per_kg_item,is_active,added_on,total_bill_amount,sale_discount_percentage,sale_price,item_code,item_barcode,sold_quantity,sold_quantity_kg,stock_id")] stock stock)
        {
            if (ModelState.IsValid)
            {
                db.stocks.Add(stock);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.product_id = new SelectList(db.products, "product_id", "product_name", stock.product_id);
            ViewBag.store_id = new SelectList(db.stores, "store_id", "store_name", stock.store_id);
            ViewBag.vendor_id = new SelectList(db.vendors, "vendor_id", "vendor_name", stock.vendor_id);
            return View(stock);
        }

        // GET: stocks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            stock stock = db.stocks.Find(id);
            if (stock == null)
            {
                return HttpNotFound();
            }
            ViewBag.product_id = new SelectList(db.products, "product_id", "product_name", stock.product_id);
            ViewBag.store_id = new SelectList(db.stores, "store_id", "store_name", stock.store_id);
            ViewBag.vendor_id = new SelectList(db.vendors, "vendor_id", "vendor_name", stock.vendor_id);
            return View(stock);
        }

        // POST: stocks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "store_id,product_id,quantity,vendor_id,quantity_kg,MRP,purchase_price,is_per_kg_item,is_active,added_on,total_bill_amount,sale_discount_percentage,sale_price,item_code,item_barcode,sold_quantity,sold_quantity_kg,stock_id")] stock stock)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stock).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.product_id = new SelectList(db.products, "product_id", "product_name", stock.product_id);
            ViewBag.store_id = new SelectList(db.stores, "store_id", "store_name", stock.store_id);
            ViewBag.vendor_id = new SelectList(db.vendors, "vendor_id", "vendor_name", stock.vendor_id);
            return View(stock);
        }

        // GET: stocks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            stock stock = db.stocks.Find(id);
            if (stock == null)
            {
                return HttpNotFound();
            }
            return View(stock);
        }

        // POST: stocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            stock stock = db.stocks.Find(id);
            db.stocks.Remove(stock);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: stocks/Delete/5
        [HttpPost, ActionName("GetStockPricing")]
        //[ValidateAntiForgeryToken]
        public JsonResult GetStockPricing(int id)
        {
            stock stock = db.stocks.Find(id);
            var result = new { stock.MRP, stock.sale_discount_percentage, stock.sale_price, stock.stock_id };
            return Json(result);
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
