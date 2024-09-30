using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using QLCHThoiTrang.Models;
using static System.Net.Mime.MediaTypeNames;

namespace QLCHThoiTrang.Controllers
{
    public class ProductController : Controller
    {
        QuanLyCuaHangThoiTrangEntities db = new QuanLyCuaHangThoiTrangEntities();
        

        // GET: Product

        public ActionResult Index()
        {
            
            return View(db.SanPhams.ToList());
        }

        public ActionResult Create()
        {
            ViewBag.MaDM = new SelectList(db.DanhMucs, "MaDM", "TenDanhMuc");
            return View();
        }

        [HttpPost]
        public ActionResult Create(SanPham sp, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Kiểm tra file có tồn tại không
                    if (file != null && file.ContentLength > 0)
                    {
                        // Chuyển đổi file thành mảng byte
                        using (var binaryReader = new BinaryReader(file.InputStream))
                        {
                            sp.HinhAnh = binaryReader.ReadBytes(file.ContentLength);
                        }
                        // Tạo tên file duy nhất
                        var fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/Content/Images/"), fileName);

                        // Lưu file vào thư mục
                        file.SaveAs(path);

                        ////Lưu tên file vào thuộc tính HinhAnh của sản phẩm
                        ////sp.HinhAnh = fileName;

                        return RedirectToAction("Index");
                        
                    }

                    // Lưu vào cơ sở dữ liệu
                    db.SanPhams.Add(sp);
                    db.SaveChanges();
                    ViewBag.Message = "Sản phẩm đã được thêm thành công!";
                }
                catch (DbEntityValidationException ex)
                {
                    // Kiểm tra các lỗi xác thực
                    foreach (var validationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            // Ghi lại hoặc hiển thị lỗi xác thực
                            Console.WriteLine($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                        }
                    }
                    ModelState.AddModelError("", "Có lỗi xảy ra khi lưu sản phẩm. Vui lòng kiểm tra lại thông tin.");
                }
                catch (Exception ex)
                {
                    // Xử lý ngoại lệ chung
                    ModelState.AddModelError("", "Có lỗi xảy ra: " + ex.Message);
                }
            }
            ViewBag.MaDM = new SelectList(db.DanhMucs, "MaDM", "TenDanhMuc", sp.MaDM);
            return View(sp);
        }
        public ActionResult GetImage(string imageName)
        {
            var imagePath = Path.Combine(Server.MapPath("~/Content/Images/"), imageName);

            if (System.IO.File.Exists(imagePath))
            {
                return File(imagePath, "image/jpeg"); // Hoặc "image/png" tùy thuộc vào định dạng
            }
            return HttpNotFound();
        }





        //[HttpPost]
        //public ActionResult Create(SanPham sp, HttpPostedFileBase file)
        //{
        //    //if (ModelState.IsValid)
        //    //{
        //    //    Chuyển đổi file hình ảnh thành mảng byte
        //    //    byte[] imageData = null;
        //    //    if (file != null && file.ContentLength > 0)
        //    //    {
        //    //        using (var binaryReader = new BinaryReader(file.InputStream))
        //    //        {
        //    //            imageData = binaryReader.ReadBytes(file.ContentLength);
        //    //        }
        //    //    }

        //    //    Tạo đối tượng SanPham và gán các giá trị từ ViewModel
        //    //   var sanPham = new SanPham
        //    //   {
        //    //       TenSP = sp.TenSP,
        //    //       GiaBan = sp.GiaBan,
        //    //       DanhMuc = sp.DanhMuc,
        //    //       ChiTiet = sp.ChiTiet,
        //    //       HinhAnh = imageData,  // Lưu ảnh dưới dạng byte[]

        //    //   };

        //    //    Thêm sản phẩm vào cơ sở dữ liệu
        //    //    using (var db = new QuanLyCuaHangThoiTrangEntities())
        //    //    {
        //    //        db.SanPhams.Add(sanPham);
        //    //        db.SaveChanges();
        //    //    }

        //    //    return RedirectToAction("Index");  // Chuyển hướng sau khi lưu thành công
        //    //}

        //    //return View(sp);  // Nếu không hợp lệ, quay lại trang nhập liệu
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            // Lưu vào cơ sở dữ liệu
        //            db.SanPhams.Add(sp);
        //            db.SaveChanges();
        //            ViewBag.Message = "Sản phẩm đã được thêm thành công!";
        //        }
        //        catch (DbEntityValidationException ex)
        //        {
        //            // Kiểm tra các lỗi xác thực
        //            foreach (var validationErrors in ex.EntityValidationErrors)
        //            {
        //                foreach (var validationError in validationErrors.ValidationErrors)
        //                {
        //                    // Ghi lại hoặc hiển thị lỗi xác thực
        //                    Console.WriteLine($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
        //                }
        //            }
        //            ModelState.AddModelError("", "Có lỗi xảy ra khi lưu sản phẩm. Vui lòng kiểm tra lại thông tin.");
        //        }
        //    }
        //    return View(sp);
        //}
        //    [HttpPost]
        //    public ActionResult Upload(HttpPostedFileBase file)
        //    {
        //        if (file != null && file.ContentLength > 0)
        //        {
        //            Kiểm tra loại file(chỉ cho phép hình ảnh)
        //            var fileExtension = Path.GetExtension(file.FileName);
        //            if (fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == ".png")
        //            {
        //                Chuyển file thành byte[]
        //                byte[] imageData = null;
        //                using (var binaryReader = new BinaryReader(file.InputStream))
        //                {
        //                    imageData = binaryReader.ReadBytes(file.ContentLength);
        //                }

        //                Tạo đối tượng SanPham và gán dữ liệu ảnh dưới dạng byte[]
        //               var sanPham = new SanPham
        //               {
        //                   HinhAnh = imageData,
        //               };

        //                Lưu vào cơ sở dữ liệu
        //                db.SanPhams.Add(sanPham);
        //                db.SaveChanges();

        //                ViewBag.Message = "Tải lên thành công và lưu vào cơ sở dữ liệu!";
        //            }
        //            else
        //            {
        //                ViewBag.Message = "Chỉ chấp nhận file ảnh định dạng JPG, JPEG, hoặc PNG.";
        //            }
        //        }
        //        else
        //        {
        //            ViewBag.Message = "Vui lòng chọn một file để tải lên.";
        //        }

        //        return View();
        //    }
    }

}