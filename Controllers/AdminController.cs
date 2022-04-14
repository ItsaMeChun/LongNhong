using BeTa.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BeTa.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        BunBo buntemp;
        public ActionResult Index()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            return View();
        }
        BunContextDataContext db = new BunContextDataContext();
        // GET: Account      
        mahoa md5 = new mahoa();
        /*-----------------------------------ĐĂNG NHẬP------------------------------------------*/
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Login(FormCollection collection)
        {
            var id = collection["Tài khoản"];
            var pass = collection["Mật khẩu"];
            ViewBag.Thongbao = null;
            if (String.IsNullOrEmpty(id)) { ViewData["loi1"] = "Không thể để trống"; }
            else if (String.IsNullOrEmpty(pass)) { ViewData["loi2"] = "Không thể để trống"; }
            else
            {
                Manager admin = db.Managers.SingleOrDefault(n => n.TaikhoanM == md5.MD5Hash(id) && n.MatkhauM == md5.MD5Hash(pass));
                if (admin != null)
                {
                    Session["admin"] = admin;
                    Session["tendangnhapadmin"] = admin.HoTenM;
                    Session["idadmin"] = admin.IDM;
                    return RedirectToAction("Index", "Admin");
                }
                else ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
        /*-----------------------------------ĐĂNG KÝ------------------------------------------*/
        [HttpGet]
        public ActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Signup(FormCollection collection, Manager admin)
        {
            var name = collection["Hotenadmin"];
            var id = collection["ID"];
            var password = collection["Password"];
            var conpass = collection["Confirm password"];
            var address = collection["Address"];
            var Pnum = collection["Phone Number"];
            var email = collection["Email"];
            var bd = String.Format("{0:MM/dd/yyyy}", collection["DateofBirth"]);

            admin.HoTenM = name;
            admin.TaikhoanM = md5.MD5Hash(id);
            admin.MatkhauM = md5.MD5Hash(password);
            admin.EmailM = email;
            admin.DienthoaiM = Pnum;
            admin.NgaysinhM = DateTime.Parse(bd);
            admin.DiachiM = address;
            db.Managers.InsertOnSubmit(admin);
            db.SubmitChanges();
            return RedirectToAction("Login", "Admin");
        }
        /*-----------------------------------ĐĂNG XUẤT------------------------------------------*/
        public ActionResult SignOut()
        {
            Session.Clear();
            return RedirectToAction("Login", "Admin");
        }

        public ActionResult MonAn()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            return View(db.BunBos.ToList());
        }
        [HttpGet]
        public ActionResult ThemMoi()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemMoi(BunBo bunbo, HttpPostedFileBase fileupload)
        {

            if (fileupload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var filename = Path.GetFileName(fileupload.FileName);
                    var path = Path.Combine(Server.MapPath("~/images"), filename);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.ThongBao = "Hình ảnh đã được sử dụng";
                    }
                    else
                    {
                        fileupload.SaveAs(path);
                    }
                    bunbo.Img = filename;
                    db.BunBos.InsertOnSubmit(bunbo);
                    db.SubmitChanges();
                }
                return RedirectToAction("MonAn");
            }

        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            BunBo bun = db.BunBos.SingleOrDefault(n => n.Mamon == id);
            ViewBag.MaMon = bun.Mamon;
            if (bun == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(bun);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult ConfirmDelete(int id)
        {
            BunBo bun = db.BunBos.SingleOrDefault(n => n.Mamon == id);
            ViewBag.MaMon = bun.Mamon;
            if (bun == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.BunBos.DeleteOnSubmit(bun);
            db.SubmitChanges();
            return RedirectToAction("MonAn");
        }
      
        [HttpGet]
        public ActionResult Edit1(int id)
        {
            BunBo bun = db.BunBos.SingleOrDefault(n => n.Mamon == id);
            if (bun == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.BunBos.DeleteOnSubmit(bun);
            db.SubmitChanges();
            return View(bun);           
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit1(BunBo bunbo, HttpPostedFileBase fileupload)
        {                      
            if (fileupload == null)
            {
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var filename = Path.GetFileName(fileupload.FileName);
                    var path = Path.Combine(Server.MapPath("~/images"), filename);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.ThongBao = "Hình ảnh đã được sử dụng";
                    }
                    else
                    {
                        fileupload.SaveAs(path);
                    }
                    bunbo.Img = filename;
                    db.BunBos.InsertOnSubmit(bunbo);
                    db.SubmitChanges();
                }
                return RedirectToAction("MonAn");
            }
           }
    }
}