using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeTa.Models
{
    public class monan
    {
        BunContextDataContext data = new BunContextDataContext();
        public int mamon { get; set; }
        public string tenmon { get; set; }
        public double gia { get; set; }
        public string tinhtrang { get; set; }
        public string anh { get; set; }
        public monan(int ma)
        {
            mamon = ma;
            BunBo bun = data.BunBos.Single(n => n.Mamon == mamon);
            tenmon = bun.Tenmon;
            gia = (double)bun.GiaTien;
            anh = bun.Img;
        }
    }
}