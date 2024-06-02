using pots.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using FirebirdSql.Data.FirebirdClient;
using Microsoft.AspNetCore.Http;

namespace pots.Controllers
{
    public class PotController : Controller
    {
        private readonly IConfiguration _configuration;

        public PotController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            PotModel potModel = new PotModel(_configuration);
            List<PotModel> pots = potModel.GetPots();
            return View(pots);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(PotModel pot, IFormFile image)
        {
            PotModel potModel = new PotModel(_configuration);
            potModel.AddPot(pot, image);

            return RedirectToAction("Index");
        }


        public IActionResult Edit(int id)
        {
            PotModel potModel = new PotModel(_configuration);
            PotModel pot = potModel.GetPot(id);

            return View(pot);
        }

        [HttpPost]
        public IActionResult Edit(PotModel pot, IFormFile image)
        {
            PotModel potModel = new PotModel(_configuration);
            potModel.UpdatePot(pot, image);

            return RedirectToAction("Index");
        }


        public IActionResult Delete(int id)
        {
            PotModel potModel = new PotModel(_configuration);
            potModel.DeletePot(id);

            return RedirectToAction("Index");
        }


        public IActionResult Details(int id)
        {
            PotModel potModel = new PotModel(_configuration);
            PotModel pot = potModel.GetPot(id);

            LidModel lidModel = new LidModel(_configuration);
            List<LidModel> compatibleLids = lidModel.GetCompatibleLids(pot.DIAMETER);

            // Create a ViewModel to hold the pot and the compatible lids
            PotDetailsViewModel viewModel = new PotDetailsViewModel
            {
                Pot = pot,
                CompatibleLids = compatibleLids
            };

            return View(viewModel);
        }

    }
}
