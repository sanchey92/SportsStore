using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers
{
    public class HomeController : Controller
    {
        private IStoreRepository _repository;
        public int PageSize = 4;

        public HomeController(IStoreRepository repo)
        {
            _repository = repo;
        }

        public ViewResult Index(string category, int productPage = 1)
            => View(
                new ProductsListViewModel
                {
                    Products = _repository.Products
                        .Where(p => category == null || p.Category == category)
                        .OrderBy(product => product.ProductId)
                        .Skip((productPage - 1) * PageSize)
                        .Take(PageSize),
                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = productPage,
                        ItemPerPage = PageSize,
                        TotalItem = category == null
                            ? _repository.Products.Count()
                            : _repository.Products.Where(product =>
                                product.Category == category).Count()
                    },
                    CurrentCategory = category
                }
            );
    }
}