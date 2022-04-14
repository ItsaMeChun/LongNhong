using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BeTa.Models;

namespace BeTa.Controllers
{
    public class AccountController : Controller
    {
        BunContextDataContext db = new BunContextDataContext();
        // GET: Account      
        mahoa md5 = new mahoa();
        public ActionResult Index()
        {
            return View();
        }
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
                KH kh = db.KHs.SingleOrDefault(n => n.TaikhoanKH == md5.MD5Hash(id) && n.MatkhauKH == md5.MD5Hash(pass));
                if (kh != null)
                {
                    Session["tk"] = kh;
                    Session["tendangnhap"] = kh.HoTenKH;
                    Session["id"] = kh.IDKH;                  
                    return RedirectToAction("Index", "Home");
                }
                else ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
        /*-----------------------------------ĐĂNG KÝ------------------------------------------*/
        public ActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Signup(FormCollection collection, KH Kh)
        {
            var name = collection["HotenKH"];
            var id = collection["ID"];
            var password = collection["Password"];
            var conpass = collection["Confirm password"];
            var address = collection["Address"];
            var Pnum = collection["Phone Number"];
            var email = collection["Email"];
            var bd = String.Format("{0:MM/dd/yyyy}", collection["DateofBirth"]);
           
                Kh.HoTenKH = name;
                Kh.TaikhoanKH = md5.MD5Hash(id);
                Kh.MatkhauKH = md5.MD5Hash(password);
                Kh.EmailKH = email;
                Kh.DienthoaiKH = Pnum;
                Kh.NgaysinhKH = DateTime.Parse(bd);
                Kh.DiachiKH = address;
                db.KHs.InsertOnSubmit(Kh);
                db.SubmitChanges();
                return RedirectToAction("Login","Home");
        }
        /*-----------------------------------ĐĂNG XUẤT------------------------------------------*/
        public ActionResult SignOut()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        /*-----------------------------------QUÊN MẬT KHẨU------------------------------------------*/
        [HttpGet]
        public ActionResult Quenmatkhau()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Quenmatkhau(FormCollection collection)
        {
            var name = collection["HotenKH"];
            var id = collection["ID"];
            var password = collection["Password"];
            var conpass = collection["Confirm password"];
            var address = collection["Address"];
            var Pnum = collection["Phone Number"];
            var email = collection["Email"];
            var bd = String.Format("{0:MM/dd/yyyy}", collection["DateofBirth"]);

            KH kh = db.KHs.SingleOrDefault(n => md5.MD5Hash(n.TaikhoanKH.Trim()) == md5.MD5Hash(id.Trim()));

            if (kh != null)
            {
                if (kh.DienthoaiKH.Trim() == Pnum.Trim() && kh.EmailKH.Trim() == email.Trim())
                {
                    if (password != conpass)
                    {
                        ViewBag.Thongbao = "Nhập lại mật khẩu không đúng";
                    }
                    else if (password.Trim() == conpass.Trim())
                    {
                        kh.TaikhoanKH = md5.MD5Hash(id);
                        kh.MatkhauKH = md5.MD5Hash(password);
                        kh.EmailKH = email;
                        kh.DienthoaiKH = Pnum;
                        db.SubmitChanges();
                        return RedirectToAction("Index", "Home");
                    }
                }
                ViewBag.Thongbao = "Nhập thông tin sai vui lòng nhập lại";
            }
            else
            {
                ViewBag.Thongbao = "Nhập thông tin không đúng";
            }
            return View();
        }
    }
}