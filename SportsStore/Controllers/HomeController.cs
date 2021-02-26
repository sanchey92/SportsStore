using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Controllers
{
    public class HomeController : Controller
    {
        private IStoreRepository _repository;

        public HomeController(IStoreRepository repo)
        {
            _repository = repo;
        }
        public IActionResult Index() => View(_repository.Products);
    }
}