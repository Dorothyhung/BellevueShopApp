using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using PROG123.DAL;
using PROG123.Models;


namespace PROG123.Controllers
{
    public class HungController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<HungController> _logger;

        public HungController(ILogger<HungController> logger, IConfiguration configuration)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Home()
        {
            return View();
        }

        public IActionResult Login(LogInCredentialsModel logInCredentialsModel)
        {
            DALPerson dp = new DALPerson(_configuration);
            PersonModel pm = dp.CheckLogInCredentials(logInCredentialsModel);

            if (dp.CheckLogInCredentials(logInCredentialsModel) == null)
            {
                ViewBag.LoginMessage = "Login is incorrect";
            }
            else
            {           
                HttpContext.Session.SetString("personID", pm.PersonID);
                HttpContext.Session.SetString("UserName", pm.FName);
                ViewBag.UserName = pm.FName;
            }

            return View("Index");

        }

        public IActionResult EnterNewProduct()
        {
            string uID = "0";
            uID = HttpContext.Session.GetString("personID");
            if (uID == null)
            {
                ViewBag.ErrorMessage = "User is not logged in";
                return View("Index");
            }
            
            return View();
        }

        public IActionResult AddProductToDB(ProductModel productModel)
        {
            DALProducts dp = new DALProducts(_configuration);
            string productID = dp.AddNewProduct(productModel);

            HttpContext.Session.SetString("PID", productID);
            productModel.PID = productID;

            return View(productModel);

        }

        public IActionResult ListAllProducts()
        {
            string uID = "0";
            uID = HttpContext.Session.GetString("personID");
            if (uID == null)
            {
                ViewBag.ErrorMessage = "User is not logged in";
                return View("Index");
            }

            DALProducts dp = new DALProducts(_configuration);
            LinkedList<ProductModel> productList = dp.GetAllProducts();

            return View(productList);
        }

        public IActionResult OneClickBuy(string PID)
        {
            string uID = "0";
            uID = HttpContext.Session.GetString("personID");
            if (uID == null)
            {
                ViewBag.ErrorMessage = "User is not logged in";
                return View("Index");
            }

            DALSalesTransaction dst = new DALSalesTransaction(_configuration);
            PurchaseModel pm = dst.OneClickBuy(PID, uID, 1);

            return View(pm);
        }
    }
}
