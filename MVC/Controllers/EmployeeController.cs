using MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace MVC.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult Index()
        {
            IEnumerable<MVCEmployeeModel> mVCEmployeeModels;           
            HttpResponseMessage httpResponseMessage = GlobalVariables.httpClient.GetAsync("Employees").Result;
            mVCEmployeeModels = httpResponseMessage.Content.ReadAsAsync<IEnumerable<MVCEmployeeModel>>().Result;
            return View(mVCEmployeeModels);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new MVCEmployeeModel());

        }

        [HttpPost]
        [ActionName("Create")]
        public ActionResult CreatePost(MVCEmployeeModel mVCEmployeeModel)
        {
            HttpResponseMessage httpResponseMessage = GlobalVariables.httpClient.PostAsJsonAsync("Employees", mVCEmployeeModel).Result;
            TempData["SuccessMessage"] = "Saved Successfully";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Employee employee = db.Employees.Find(id);
            HttpResponseMessage httpResponseMessage = GlobalVariables.httpClient.GetAsync("Employees/" + id.ToString()).Result;
            if (httpResponseMessage == null)
            {
                return HttpNotFound();
            }
            return View(httpResponseMessage.Content.ReadAsAsync<MVCEmployeeModel>().Result);
        }


        [HttpPost]
        [ActionName("Edit")]
        public ActionResult EditPost(MVCEmployeeModel mVCEmployeeModel)
        {
            if (ModelState.IsValid)
            {
                HttpResponseMessage httpResponseMessage = GlobalVariables.httpClient.PutAsJsonAsync("Employees/" + mVCEmployeeModel.EmployeeID, mVCEmployeeModel).Result;
                TempData["SuccessMessage"] = "Updated Successfully";
                return RedirectToAction("Index");
            }

                return View(mVCEmployeeModel);
        }
               
        public ActionResult Delete(int id)
        {
            HttpResponseMessage httpResponseMessage = GlobalVariables.httpClient.DeleteAsync("Employees/" + id.ToString()).Result;
            TempData["SuccessMessage"] = "Deleted Successfully";
            return RedirectToAction("Index");
        }
    }
}