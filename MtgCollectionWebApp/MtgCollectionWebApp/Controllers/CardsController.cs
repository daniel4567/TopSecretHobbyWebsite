﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MtgCollectionWebApp.Models;
using System.Threading.Tasks;

namespace MtgCollectionWebApp.Controllers
{
    public class CardsController : Controller
    {
        private MtgCollectionDB db = new MtgCollectionDB();

        // GET: Cards
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var model = await GetCardsByName("");
            return View(model);
        }

        public List<Card> GetCardList(string q)
        {
            return (List<Card>)db.Cards.Where(a => a.CardName.Contains(q));
        }

        [HttpGet]
        public async Task<ActionResult> GetCardsByName(string q)
        {
            string b = q;
            var model = await GetFullAndPartialViewModel(b);
          
            return PartialView("_DisplayCards",model);
        }

        private async Task<IEnumerable<CardsViewModel>> GetFullAndPartialViewModel(string q)
        {
            var cardsAll = db.Cards.Where(a => a.CardName.Contains(q));
            var cardsViewModel = new List<CardsViewModel>();
            
            foreach (Card i in cardsAll)
            {
                var b = new CardsViewModel();
                b.Card = i;
                b.Quantity = getQuantityOrZero(i.CardId);
                cardsViewModel.Add(b);
                   
            }

            return cardsViewModel;
        }

        private Collection getUserCollection()
        {
            var collection = db.Collections.Find(User.Identity.Name);

            return collection;
        }

        private int getQuantityOrZero(int cardId)
        {
            int q = 0;
            var test = db.Collections.Where(a => a.CollectionOwner.Equals(User.Identity.Name));
            var collection = test.First();
            if (collection.CollectionEntries != null)
            {
                var entries = collection.CollectionEntries.Where(d => d.CollectionEntryCardId.Equals(cardId));
                if (entries.Count() > 0) {
                   q = entries.First().Quantity;
                }  
            }
            
            return q;
        }

        // GET: Cards/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Card card = db.Cards.Find(id);
            if (card == null)
            {
                return HttpNotFound();
            }
            return View(card);
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


