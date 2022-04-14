using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BeTa.Models;
namespace BeTa.Models
{
    public class GioHang
    {
        BunContextDataContext data  = new BunContextDataContext();
        public int Mamon { get; set; }
        public string Tenmon { get; set; }
        public string IMG { get; set; }
        public Double Dongia { get; set; }
        public string Mota { get; set; }
        public int Soluong { get; set; }
        public Double ThanhTien
        {
            get { return Soluong * Dongia; }
        }

        public GioHang(int ma)
        {
            Mamon = ma;
            BunBo bun = data.BunBos.Single(n => n.Mamon == Mamon);
            Tenmon = bun.Tenmon;
            IMG = bun.Img;
            Dongia = double.Parse(bun.GiaTien.ToString());
            Soluong = 1;
            Mota = bun.Mota;
        }       
    }
}