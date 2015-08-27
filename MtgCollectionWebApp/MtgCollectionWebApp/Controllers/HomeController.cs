﻿using MtgCollectionWebApp.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MtgCollectionWebApp.Controllers
{
    public class HomeController : Controller
    {
        private MtgCollectionDB db = new MtgCollectionDB();
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (db.Collections.Where(a=> a.CollectionOwner.Equals(User.Identity.Name)).Count() == 0){
                    db.Collections.Add(new Collection { CollectionOwner = User.Identity.Name });
                    db.SaveChanges();
                }
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}