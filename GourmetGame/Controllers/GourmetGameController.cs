using GourmetGame.Business;
using GourmetGame.Data;
using GourmetGame.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices.Marshalling;

namespace GourmetGame.Controllers
{
    public class GourmetGameController : Controller
    {
        private readonly ILogger<GourmetGameController> _logger;
        private readonly GameBusiness _gameBusiness;
        public GourmetGameController(ILogger<GourmetGameController> logger, GameBusiness gameBusiness)
        {
            _logger = logger;
            _gameBusiness = gameBusiness;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult IsPasta()
        {
            return PartialView();
        }
        [HttpPost]
        public IActionResult IspastaResponse(bool isPasta)
        {
            if (isPasta)
            {
                return PartialView("IsLasanha");
            }
            else
            {
                return Redirect("OptionsFood");
            }
        }
        public IActionResult IsLasanha(bool isLasanha)
        {
            if (isLasanha)
                return PartialView("Success");
            return Redirect("OptionsFood");
        }
        public IActionResult PredictionFood(string selectedHint, bool IsDish)
        {

            if (selectedHint.ToUpper() == "NENHUMA DAS OPCOES")
                return PartialView("InsertFood");

            if (IsDish)
            {
                return PartialView("Success");
            }
            else
            {
                var selectedDish = _gameBusiness.PredictionFoodResponse(selectedHint);

                if (selectedDish == null)
                {
                    return PartialView("InsertFood");
                }

                return PartialView(selectedDish);
            }
        }

        public IActionResult ListFood()
        {
            return PartialView();
        }
        public IActionResult OptionsFood()
        {
            var options = _gameBusiness.OPtionsDishesResponse();
            if (options.Count > 0)
                return PartialView(options);
            return PartialView("InsertFood");
        }

        public IActionResult InsertFood(InsertDishesModel dish)
        {
            _gameBusiness.InsertDishResponse(dish);

            return PartialView("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
