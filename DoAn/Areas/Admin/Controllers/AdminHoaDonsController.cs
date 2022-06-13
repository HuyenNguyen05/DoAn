using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BTL_Nhom13.Models;
using PagedList;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClosedXML.Excel;

namespace BTL_Nhom13.Areas.Admin.Controllers
{
    public class AdminHoaDonsController : Controller
    {
        private TinhDauDB db = new TinhDauDB();

        public ActionResult Receipt(int? page)
        {
            var hoaDons = db.HoaDons.Select(h => h);
            hoaDons = hoaDons.OrderByDescending(s => s.NgayDat);
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(hoaDons.ToPagedList(pageNumber, pageSize));
        }
        // GET: Admin/AdminHoaDons/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoaDon hoaDon = db.HoaDons.Find(id);
            if (hoaDon == null)
            {
                return HttpNotFound();
            }
            return View(hoaDon);
        }
        // GET: Admin/AdminHoaDons/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoaDon hoaDon = db.HoaDons.Find(id);
            if (hoaDon == null)
            {
                return HttpNotFound();
            }
            //ViewBag.MaGioHang = new SelectList(db.GioHangs, "MaGioHang", "TenTaiKhoan", hoaDon.MaGioHang);
            return View(hoaDon);
        }

        // POST: Admin/AdminHoaDons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaHoaDon,NgayDat,TinhTrang,PhiShip,GhiChu,MaGioHang")] HoaDon hoaDon)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(hoaDon).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Receipt");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi nhập dữ liệu! " + ex.Message;
                return View(hoaDon);
            }
        }

        // GET: Admin/AdminHoaDons/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoaDon hoaDon = db.HoaDons.Find(id);
            if (hoaDon == null)
            {
                return HttpNotFound();
            }
            return View(hoaDon);
        }

        // POST: Admin/AdminHoaDons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HoaDon hoaDon = db.HoaDons.Find(id);
            db.HoaDons.Remove(hoaDon);
            db.SaveChanges();
            return RedirectToAction("Receipt");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }




        public FileResult ExportToExel()
        {
            DataTable dt = new DataTable("Grid");

            var temp = db.HoaDons.OrderBy(x => x.NgayDat).ToList();

            dt.Columns.AddRange(new DataColumn[8] { new DataColumn("Tên khách hàng"),
                new DataColumn("Tên sản phẩm"),
                                                     new DataColumn("Ngày đặt"),
                                                     new DataColumn("Tình trạng"),
                                                    new DataColumn("Phí vận chuyển"),
                                                    new DataColumn("Tổng tiền hàng"),
                                                    new DataColumn("Thành tiền"),
                                                    new DataColumn("Địa chỉ vận chuyển")
                                                     });

            temp.ForEach(item =>
            {

                // Lấy thêm giá + địa chỉ giao hàng
                List<ChiTietGioHang> listDetail = db.ChiTietGioHangs.Where(g => g.MaGioHang == item.MaGioHang).ToList();
                decimal total = 0;
                string tensanpham = "";

                listDetail.ForEach(detail =>
                {
                    // laasy ten sp
                    var ten = db.SanPhams.Where(sp => sp.MaSP == detail.MaSP).FirstOrDefault();
                    tensanpham += ten.TenSP + " ;";
                    
                    total += detail.Gia * detail.SoLuongMua;
                });

                decimal totalPlus = total + item.PhiShip;


                dt.Rows.Add(item.GioHang.TaiKhoan.TenKhachHang, tensanpham, item.NgayDat, item.TinhTrang, item.PhiShip, total, totalPlus, item.DcNhanHang);
                

            });


            using (XLWorkbook wb = new XLWorkbook()) //Install ClosedXml from Nuget for XLWorkbook  
            {
                
                wb.Worksheets.Add(dt);
                wb.ColumnWidth = 100.0;

                using (MemoryStream stream = new MemoryStream()) //using System.IO;  
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "HoaDon.xlsx");
                }
            }
        }

    }
}
