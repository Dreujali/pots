using Microsoft.AspNetCore.Mvc;
using pots.Models;

namespace pots.Controllers
{
    public class PotController : Controller
    {
        private readonly IConfiguration _configuration;

        public PotController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Returns a view with a list of all pots
        public IActionResult Index()
        {
            PotModel potModel = new PotModel(_configuration);
            List<PotModel> pots = potModel.GetPots();
            return View(pots);
        }

        // Returns a view for creating a new pot
        public IActionResult Create()
        {
            return View();
        }

        // Adds a new pot and redirects to the index view
        [HttpPost]
        public IActionResult Create(PotModel pot, IFormFile image)
        {
            // Check if the file is an image
            if (image != null && !image.ContentType.ToLower().StartsWith("image/"))
            {
                ModelState.AddModelError("", "Invalid file type. Please upload an image file.");
                return View(pot);
                
            }

            PotModel potModel = new PotModel(_configuration);
            potModel.AddPot(pot, image);
            return RedirectToAction("Index");
        }


        // Returns a view for editing a pot with a given ID
        public IActionResult Edit(int id)
        {
            PotModel potModel = new PotModel(_configuration);
            PotModel pot = potModel.GetPot(id);

            return View(pot);

        }

        // Updates a pot and redirects to the index view
        [HttpPost]
        public IActionResult Edit(PotModel pot, IFormFile image)
        {

            // Check if the file is an image
            if (image != null && !image.ContentType.ToLower().StartsWith("image/"))
            {
                ModelState.AddModelError("", "Invalid file type. Please upload an image file.");
                return View(pot);
            }

            PotModel potModel = new PotModel(_configuration);
            potModel.UpdatePot(pot, image);
            return RedirectToAction("Index");
        }


        // Deletes a pot with a given ID and redirects to the index view
        public IActionResult Delete(int id)
        {
            PotModel potModel = new PotModel(_configuration);
            potModel.DeletePot(id);

            return RedirectToAction("Index");
        }

        // Returns a view with the details of a pot and its compatible lids
        public IActionResult Details(int id)
        {
            PotModel potModel = new PotModel(_configuration);
            PotModel pot = potModel.GetPot(id);

            LidModel lidModel = new LidModel(_configuration);
            List<LidModel> compatibleLids = lidModel.GetCompatibleLids(pot.DIAMETER);

            PotDetailsViewModel viewModel = new PotDetailsViewModel
            {
                Pot = pot,
                CompatibleLids = compatibleLids
            };

            return View(viewModel);
        }

    }
}
