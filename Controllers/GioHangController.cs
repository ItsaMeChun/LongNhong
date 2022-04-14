using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BeTa.Models;
namespace BeTa.Controllers
{
    public class GioHangController : Controller
    {
        
        // GET: GioHang
        BunContextDataContext dbcontext = new BunContextDataContext();
        public ActionResult Index()
        {
            return View();
        }

        public List<GioHang> LayGH()
        {
            List<GioHang> listGH = Session["GioHang"] as List<GioHang>;
            if(listGH == null)
            {
                listGH = new List<GioHang>();
                Session["GioHang"] = listGH;
            }
            return listGH;
        }

        public ActionResult ThemGioHang(int mamon, string strURL)
        {
            List<GioHang> listGH = LayGH();
            GioHang sp = listGH.Find(n => n.Mamon == mamon);               
            if(sp == null)
            {
                sp = new GioHang(mamon);
                listGH.Add(sp);
                return Redirect(strURL);
            }
            else
            {
                sp.Soluong++;
                return Redirect(strURL);
            }
        }

        /*public ActionResult CapnhatSL(int mamon, string strURL, FormCollection collection)
        {
            List<GioHang> listGH = LayGH();
            GioHang sp = listGH.Find(n => n.Mamon == mamon);
            int Soluong = 0;
            if (Int32.Parse(collection["SoLuong"]) != sp.Soluong)
            {
                Soluong = Int32.Parse(collection["SoLuong"]);
            }           

                sp.Soluong = sp.Soluong + 1 + Soluong;
                return Redirect(strURL);

        }*/

        private int TongSL()
        {
            int TongSL = 0;
            List<GioHang> ListGH = Session["GioHang"] as List<GioHang>;
            if(ListGH!= null)
            {              
                TongSL = ListGH.Sum(n => n.Soluong);
            }
            return TongSL;
        }
       
        private double tongTien()
        {
            double tongtien = 0;
            List<GioHang> ListGH = Session["GioHang"] as List<GioHang>;
            if (ListGH != null)
            {
                tongtien = ListGH.Sum(n => n.ThanhTien);
            }
            return tongtien;
        }

        [HttpGet]
        public ActionResult GioHang()
        {
            List<GioHang> lstgiohang = LayGH();            
            ViewBag.Tongsoluong = TongSL();
            ViewBag.Tongtien = tongTien();
            return View(lstgiohang);
        }       
       

        public ActionResult GioHang(FormCollection collection)
        {
            if (Session["tk"] == null)
            {
                return RedirectToAction("Login", "Account");
            }         
            List<GioHang> ListThanhtoan = LayGH();
            DONDATHANG ddh = new DONDATHANG();
            KH kh = (KH) Session["tk"];
            ddh.IDKH = kh.IDKH;
            ddh.Ngaydat = DateTime.Now;                            
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["NgayGiao"]);
            ddh.Ngaygiao = DateTime.Parse(ngaygiao);
            ddh.Tinhtranggiaohang = false;
            ddh.Dathanhtoan = false;
            ddh.Tongtien =  (decimal) tongTien();
            dbcontext.DONDATHANGs.InsertOnSubmit(ddh);
            dbcontext.SubmitChanges();
            foreach(var item in ListThanhtoan)
            {
                CHITIETDONTHANG ct = new CHITIETDONTHANG();
                if (item.Soluong!= int.Parse(collection["Soluong"]))
                {
                    ct.MaDonHang = ddh.MaDonHang;
                    ct.Mamon = item.Mamon;
                    ct.Soluong = int.Parse(collection["Soluong"]);
                    ct.Dongia = (decimal)item.Dongia;
                    dbcontext.CHITIETDONTHANGs.InsertOnSubmit(ct);
                }
                else
                {
                    ct.MaDonHang = ddh.MaDonHang;
                    ct.Mamon = item.Mamon;
                    ct.Soluong = item.Soluong;
                    ct.Dongia = (decimal)item.Dongia;
                    dbcontext.CHITIETDONTHANGs.InsertOnSubmit(ct);
                }
                
            }
            dbcontext.SubmitChanges();
            Session["GioHang"] = null;
            return RedirectToAction("Xacnhandonhang","GioHang");
        }     

        public ActionResult Xacnhandonhang()
        {
            return View();
        }
    }
}