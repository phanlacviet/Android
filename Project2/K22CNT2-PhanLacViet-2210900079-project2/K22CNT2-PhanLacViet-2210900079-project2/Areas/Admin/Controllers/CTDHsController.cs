using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using K22CNT2_PhanLacViet_2210900079_project2.Models;

namespace K22CNT2_PhanLacViet_2210900079_project2.Areas.Admin.Controllers
{
    public class CTDHsController : Controller
    {
        private PhanLacViet_K22CNT2_2210900079_project2Entities db = new PhanLacViet_K22CNT2_2210900079_project2Entities();

        // GET: Admin/CTDHs
        public ActionResult Index()
        {
            var cTDHs = db.CTDHs.Include(c => c.DonHang).Include(c => c.SanPham);
            return View(cTDHs.ToList());
        }

        // GET: Admin/CTDHs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CTDH cTDH = db.CTDHs.Find(id);
            if (cTDH == null)
            {
                return HttpNotFound();
            }
            return View(cTDH);
        }

        // GET: Admin/CTDHs/Create
        public ActionResult Create()
        {
            ViewBag.MaDH = new SelectList(db.DonHangs, "MaDH", "MaKH");
            ViewBag.MaSp = new SelectList(db.SanPhams, "MaSP", "TenSP");
            return View();
        }

        // POST: Admin/CTDHs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "STT,MaDH,MaSp,TenSP,GiaCa")] CTDH cTDH)
        {
            if (ModelState.IsValid)
            {
                db.CTDHs.Add(cTDH);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaDH = new SelectList(db.DonHangs, "MaDH", "MaKH", cTDH.MaDH);
            ViewBag.MaSp = new SelectList(db.SanPhams, "MaSP", "TenSP", cTDH.MaSp);
            return View(cTDH);
        }

        // GET: Admin/CTDHs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CTDH cTDH = db.CTDHs.Find(id);
            if (cTDH == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaDH = new SelectList(db.DonHangs, "MaDH", "MaKH", cTDH.MaDH);
            ViewBag.MaSp = new SelectList(db.SanPhams, "MaSP", "TenSP", cTDH.MaSp);
            return View(cTDH);
        }

        // POST: Admin/CTDHs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "STT,MaDH,MaSp,TenSP,GiaCa")] CTDH cTDH)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cTDH).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaDH = new SelectList(db.DonHangs, "MaDH", "MaKH", cTDH.MaDH);
            ViewBag.MaSp = new SelectList(db.SanPhams, "MaSP", "TenSP", cTDH.MaSp);
            return View(cTDH);
        }

        // GET: Admin/CTDHs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CTDH cTDH = db.CTDHs.Find(id);
            if (cTDH == null)
            {
                return HttpNotFound();
            }
            return View(cTDH);
        }

        // POST: Admin/CTDHs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CTDH cTDH = db.CTDHs.Find(id);
            db.CTDHs.Remove(cTDH);
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
