using Microsoft.AspNetCore.Mvc;
using pots.Models;

namespace pots.Controllers
{
    public class LidController : Controller
    {
        private readonly IConfiguration _configuration;

        public LidController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Returns a view with a list of all lids
        public IActionResult Index()
        {
            LidModel lidModel = new LidModel(_configuration);
            List<LidModel> lids = lidModel.GetLids();
            return View(lids);
        }

        // Returns a view for creating a new lid
        public IActionResult Create()
        {
            return View();
        }

        // Adds a new lid and redirects to the index view
        [HttpPost]
        public IActionResult Create(LidModel lid)
        {
            LidModel lidModel = new LidModel(_configuration);
            lidModel.AddLid(lid);

            return RedirectToAction("Index");
        }

        // Updates a lid and redirects to the index view
        [HttpPost]
        public IActionResult Edit(LidModel lid)
        {
            LidModel lidModel = new LidModel(_configuration);
            lidModel.UpdateLid(lid);

            return RedirectToAction("Index");
        }

        // Returns a view for editing a lid with a given ID
        public IActionResult Edit(int id)
        {
            LidModel lidModel = new LidModel(_configuration);
            LidModel lid = lidModel.GetLid(id);

            return View(lid);
        }

        // Deletes a lid with a given ID and redirects to the index view
        public IActionResult Delete(int id)
        {
            LidModel lidModel = new LidModel(_configuration);
            lidModel.DeleteLid(id);

            return RedirectToAction("Index");
        }
    }
}
