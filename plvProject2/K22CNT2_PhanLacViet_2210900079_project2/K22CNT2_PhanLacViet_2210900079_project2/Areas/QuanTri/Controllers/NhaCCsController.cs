using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using K22CNT2_PhanLacViet_2210900079_project2.Models;

namespace K22CNT2_PhanLacViet_2210900079_project2.Areas.QuanTri.Controllers
{
    public class NhaCCsController : Controller
    {
        private PhanLacViet_K22CNT2_2210900079_project2Entities db = new PhanLacViet_K22CNT2_2210900079_project2Entities();

        // GET: QuanTri/NhaCCs
        public ActionResult Index()
        {
            return View(db.NhaCCs.ToList());
        }

        // GET: QuanTri/NhaCCs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhaCC nhaCC = db.NhaCCs.Find(id);
            if (nhaCC == null)
            {
                return HttpNotFound();
            }
            return View(nhaCC);
        }

        // GET: QuanTri/NhaCCs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: QuanTri/NhaCCs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaNCC,TenNCC,DiaChi,SDT")] NhaCC nhaCC)
        {
            if (ModelState.IsValid)
            {
                db.NhaCCs.Add(nhaCC);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nhaCC);
        }

        // GET: QuanTri/NhaCCs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhaCC nhaCC = db.NhaCCs.Find(id);
            if (nhaCC == null)
            {
                return HttpNotFound();
            }
            return View(nhaCC);
        }

        // POST: QuanTri/NhaCCs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaNCC,TenNCC,DiaChi,SDT")] NhaCC nhaCC)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nhaCC).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nhaCC);
        }

        // GET: QuanTri/NhaCCs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhaCC nhaCC = db.NhaCCs.Find(id);
            if (nhaCC == null)
            {
                return HttpNotFound();
            }
            return View(nhaCC);
        }

        // POST: QuanTri/NhaCCs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            NhaCC nhaCC = db.NhaCCs.Find(id);
            db.NhaCCs.Remove(nhaCC);
            bool hasOrders = db.SanPhams.Any(db => db.MaNCC == nhaCC.MaNCC);
            if (hasOrders)
            {
                TempData["ErrorMessage"] = "Không thể xóa nhà cung cấp vì vẫn còn sản phẩm liên kết với nhà cung cấp này";
                return RedirectToAction("Index");
            }
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
