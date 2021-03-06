﻿using Matrix.Entities.MongoEntities;
using Matrix.Business.ViewModels;
using Matrix.Core.FrameworkCore;
using Matrix.Core.MongoCore;
using Matrix.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Matrix.DAL.MongoRepositoriesBase;

namespace Matrix.Web.Areas.Sales.Controllers
{
    public class ClientController : Controller
    {
        IMXBusinessMongoRepository _repository;

        public ClientController(IMXBusinessMongoRepository repository)
        {
            this._repository = repository;
        }

        public ActionResult Index()
        {
            MXTimer timing = new MXTimer();

            var model = _repository.GetMany<Client>();

            ViewBag.QueryTime = timing.Finish();

            return View(model);
        }

        public ActionResult CompleteDetails(string id) //id- ClientID
        {
            MXTimer timing = new MXTimer();
                        
            ViewBag.ParentItemName = _repository.GetNameById<Client>(id);

            ViewBag.QueryTime = timing.Finish();

            return View();
        }

        public ActionResult Create()
        {
            return View(new ClientViewModel());
        }

        [HttpPost]
        public ActionResult Create(ClientViewModel model)
        {
            if (ModelState.IsValid)
            {
                _repository.Insert<Client>(model.Client);

                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        public ActionResult Edit(string id)
        {
            MXTimer timing = new MXTimer();

            ClientViewModel model = new ClientViewModel 
            {
                Client = _repository.GetOne<Client>(id),
            };

            ViewBag.QueryTime = timing.Finish();

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(ClientViewModel model)
        {
            if (ModelState.IsValid)
            {//this one is a bit different because we have embedded Contacts that are not posted back from the same form.                
                _repository.Update(model.Client);

                return RedirectToAction("Edit", new { id = model.Client.Id });
            }
            else
            {
                return View(model.Client.Id);
            }
        }

        #region "async json loads"

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult LoadDataForAutoComplete(string term)
        {
            try
            {
                var predicate = MXPredicateBuilder.True<ClientType>();

                predicate = predicate.And(p => p.Name.ToLower().Contains(term));

                var results = _repository.GetOptionSet<ClientType, DenormalizedReference>(predicate);

                var myData = results.Select(a => new SelectListItem()
                {
                    Text = a.DenormalizedName,
                    Value = a.DenormalizedId,
                });

                return Json(myData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error Occurred: " + ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

    }
}
