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

        public IActionResult Index()
        {
            LidModel lidModel = new LidModel(_configuration);
            List<LidModel> lids = lidModel.GetLids();
            return View(lids);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(LidModel lid)
        {
            LidModel lidModel = new LidModel(_configuration);
            lidModel.AddLid(lid);

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            LidModel lidModel = new LidModel(_configuration);
            LidModel lid = lidModel.GetLid(id);

            return View(lid);
        }

        [HttpPost]
        public IActionResult Edit(LidModel lid)
        {
            LidModel lidModel = new LidModel(_configuration);
            lidModel.UpdateLid(lid);

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            LidModel lidModel = new LidModel(_configuration);
            lidModel.DeleteLid(id);

            return RedirectToAction("Index");
        }
    }
}
