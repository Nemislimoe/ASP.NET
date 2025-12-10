using Microsoft.AspNetCore.Mvc;
using Lab13.Models;
using Lab13.Services;
using System.Linq;

namespace Lab13.Controllers
{
    public class CalculatorController : Controller
    {
        private readonly ICalculatorService _calc;

        public CalculatorController(ICalculatorService calc)
        {
            _calc = calc;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new CalculatorModel());
        }

        [HttpPost]
        public IActionResult Index(CalculatorModel model)
        {
            // Діагностика: що реально прийшло
            ViewBag.ReceivedA = model?.A;
            ViewBag.ReceivedB = model?.B;
            ViewBag.ModelStateValid = ModelState.IsValid;
            ViewBag.ModelErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.Sum = _calc.Add(model.A, model.B);

            var div = _calc.Divide(model.A, model.B);
            model.DivisionSuccess = div.Success;
            model.DivisionMessage = div.Message;
            model.DivisionResult = div.Result;

            return View(model);
        }
    }
}
