using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PROG123.DAL;
using PROG123.Models;
using PROG123.utils;

namespace PROG123.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public IActionResult Index()
        {
            // this is for testing purpuse only.
            /*DatabaseHelper dbh = new DatabaseHelper(_configuration);
            ConnStatusModel status = dbh.GetConnectionStringAndConnectionStatus();
            ViewBag.ConnStr = status.ConnStr;
            ViewBag.DBStatus = status.DBConnectionStatus;
            ViewBag.Exception = status.Exception;*/



            
            return View();
        }

        // add your actions here 
        
        public IActionResult Page2(PersonModel personModel)
        {
            //Send the personModel to the DB
            //Instantiate a DALPerson object
            //Call the DALPerson AddPerson method - save the person id
            //Save the PersonID in to the Session

            DALPerson dp = new DALPerson(_configuration);
            string personID = dp.AddPerson(personModel);
            personModel.PersonID = personID;

            HttpContext.Session.SetString("personID", personID);
            personModel.PersonID = personID;

            return View(personModel);
        }

        public IActionResult EditMyInfo(PersonModel pm)
        {
            string personID = HttpContext.Session.GetString("personID");
            DALPerson dp = new DALPerson(_configuration);
            pm = dp.getPerson(personID);
            return View(pm);
        }

        public IActionResult UpdatePersonTable(PersonModel personModel)
        {
            string personID = HttpContext.Session.GetString("personID");
            personModel.PersonID = personID;
            DALPerson dp = new DALPerson(_configuration);
            dp.UpdatePerson(personModel);
            return View("page2", personModel);
        }

        public IActionResult DeletePerson(PersonModel personModel)
        {
            string personID = HttpContext.Session.GetString("personID");
            DALPerson dp = new DALPerson(_configuration);
            dp.DeletePerson(personID);
            return View(personModel);
        }


    }
}
