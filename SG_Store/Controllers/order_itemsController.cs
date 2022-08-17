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
    public class order_itemsController : Controller
    {
        private SG_StoreEntities db = new SG_StoreEntities();

        // GET: order_items
        public ActionResult Index()
        {
            var order_items = db.order_items.Include(o => o.order).Include(o => o.stock).Include(o => o.stock.product);
            return View(order_items.ToList());
        }

        // GET: order_items/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            order_items order_items = db.order_items.Find(id);
            if (order_items == null)
            {
                return HttpNotFound();
            }
            return View(order_items);
        }

        // GET: order_items/Create
        public ActionResult Create()
        {
            var stockItemDDL = db.stocks.Select(m => new { product_name = m.product.product_name + " (" + m.product.brand.brand_name + ") - " + m.stock_id, m.stock_id }).ToList();
            stockItemDDL.Insert(0, new { product_name = "Select Product", stock_id = 0 });
            ViewBag.order_id = new SelectList(db.orders, "order_id", "order_id");
            ViewBag.stock_id = new SelectList(stockItemDDL, "stock_id", "product_name");
            return View();
        }

        // POST: order_items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "order_id,stock_id,quantity,quantity_kg,list_price,discount")] order_items order_items)
        {
            if (ModelState.IsValid)
            {

                if (order_items.quantity > 0) { order_items.total_amount = (order_items.list_price * order_items.quantity) - order_items.discount; }
                else { order_items.total_amount = (order_items.list_price * order_items.quantity_kg) - order_items.discount; }
                
                db.order_items.Add(order_items);
                db.SaveChanges();

                //Manage Stock Sold Quantity
                if (order_items.quantity > 0) {
                    var stock = db.stocks.Find(order_items.stock_id);
                    if (!stock.sold_quantity.HasValue) { stock.sold_quantity = 0; }
                    stock.sold_quantity = stock.sold_quantity.Value + order_items.quantity;
                    //db.Entry(stock).State = EntityState.Modified;
                }
                else
                {
                    var stock = db.stocks.Find(order_items.stock_id);
                    if (!stock.sold_quantity_kg.HasValue) { stock.sold_quantity_kg = 0; }
                    stock.sold_quantity_kg = stock.sold_quantity_kg.Value + order_items.quantity_kg;
                    //db.Entry(stock).State = EntityState.Modified;
                }
                db.SaveChanges();

                ordersController objOrdersCtrl = new ordersController();
                objOrdersCtrl.UpdateOrderTotal(order_items.order_id);
                return RedirectToAction("Index");
            }

            var stockItemDDL = db.stocks.Select(m => new { product_name = m.product.product_name + " (" + m.product.brand.brand_name + ") - " + m.stock_id, m.stock_id }).ToList();
            stockItemDDL.Insert(0, new { product_name = "Select Product", stock_id = 0 });
            ViewBag.order_id = new SelectList(db.orders, "order_id", "order_id", order_items.order_id);
            ViewBag.stock_id = new SelectList(stockItemDDL, "stock_id", "product_name", order_items.stock_id);
            return View(order_items);
        }

        // GET: order_items/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            order_items order_items = db.order_items.Find(id);
            if (order_items == null)
            {
                return HttpNotFound();
            }
            var stockItemDDL = db.stocks.Select(m => new { product_name = m.product.product_name + " (" + m.product.brand.brand_name + ") - " + m.stock_id, m.stock_id }).ToList();
            stockItemDDL.Insert(0, new { product_name = "Select Product", stock_id = 0 });
            ViewBag.order_id = new SelectList(db.orders, "order_id", "order_id", order_items.order_id);
            ViewBag.stock_id = new SelectList(stockItemDDL, "stock_id", "product_name", order_items.stock_id);
            return View(order_items);
        }

        // POST: order_items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "order_id,order_item_id,stock_id,quantity,quantity_kg,list_price,discount")] order_items order_items)
        {
            if (ModelState.IsValid)
            {

                var old_orderline = db.order_items.Find(order_items.order_item_id);

                if (order_items.quantity > 0) { order_items.total_amount = (order_items.list_price * order_items.quantity) - order_items.discount; }
                else { order_items.total_amount = (order_items.list_price * order_items.quantity_kg) - order_items.discount; }

                
                //db.Entry(order_items).State = EntityState.Modified;
                db.SaveChanges();


                //Manage Stock Sold Quantity
                if (old_orderline.quantity != order_items.quantity || old_orderline.quantity_kg != order_items.quantity_kg)
                {
                    var stock = db.stocks.Find(order_items.stock_id);
                    if (old_orderline.quantity != order_items.quantity)
                    {
                        if (!stock.sold_quantity.HasValue) { stock.sold_quantity = 0; }
                        stock.sold_quantity = stock.sold_quantity.Value - old_orderline.quantity + order_items.quantity;
                    }
                    if (old_orderline.quantity_kg != order_items.quantity_kg)
                    {
                        if (!stock.sold_quantity_kg.HasValue) { stock.sold_quantity_kg = 0; }
                        stock.sold_quantity_kg = stock.sold_quantity_kg.Value - old_orderline.quantity_kg + order_items.quantity_kg;
                    }
                    //db.Entry(stock).State = EntityState.Modified;
                    db.SaveChanges();
                }

                ordersController objOrdersCtrl = new ordersController();
                objOrdersCtrl.UpdateOrderTotal(order_items.order_id);
                return RedirectToAction("Index");
            }
            var stockItemDDL = db.stocks.Select(m => new { product_name = m.product.product_name + " (" + m.product.brand.brand_name + ") - " + m.stock_id, m.stock_id }).ToList();
            stockItemDDL.Insert(0, new { product_name = "Select Product", stock_id = 0 });
            ViewBag.order_id = new SelectList(db.orders, "order_id", "order_id", order_items.order_id);
            ViewBag.stock_id = new SelectList(stockItemDDL, "stock_id", "product_name", order_items.stock_id);
            return View(order_items);
        }

        // GET: order_items/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            order_items order_items = db.order_items.Find(id);
            if (order_items == null)
            {
                return HttpNotFound();
            }
            return View(order_items);
        }

        // POST: order_items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var old_orderline = db.order_items.Find(id);
            order_items order_items = db.order_items.Find(id);


            //Manage Stock Sold Quantity
            {
                var stock = db.stocks.Find(order_items.stock_id);

                if (!stock.sold_quantity.HasValue) { stock.sold_quantity = 0; }
                stock.sold_quantity = stock.sold_quantity.Value - old_orderline.quantity;
                if (!stock.sold_quantity_kg.HasValue) { stock.sold_quantity_kg = 0; }
                stock.sold_quantity_kg = stock.sold_quantity_kg.Value - old_orderline.quantity_kg;

                //db.Entry(stock).State = EntityState.Modified;
                db.SaveChanges();
            }


            db.order_items.Remove(order_items);
            db.SaveChanges();


            ordersController objOrdersCtrl = new ordersController();
            objOrdersCtrl.UpdateOrderTotal(order_items.order_id);
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
