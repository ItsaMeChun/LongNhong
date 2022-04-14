using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BeTa.Models;

namespace BeTa.Controllers
{
    public class MonAnController : Controller
    {
        BunContextDataContext db = new BunContextDataContext();
        // GET: MonAn
        public ActionResult Index()
        {
            var allfoodstuff = bunbo();
            return View(allfoodstuff);
        }
        private List<BunBo> bunbo()
        {
            return db.BunBos.ToList();
        }
    }
}