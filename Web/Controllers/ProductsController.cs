using Microsoft.AspNetCore.Mvc;
using UXComex_challenge.Application.Interfaces;
using UXComex_challenge.Domain.Entities;

namespace UXComex_challenge.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IService<Product> _productServices;

        public ProductsController(IService<Product> productServices)
        {
            _productServices = productServices;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult List(int current = 1, int rowCount = 10, string searchPhrase = "")
        {
            var products = _productServices.list();

            if (!string.IsNullOrEmpty(searchPhrase))
            {
                products = products.Where(u => u.Name.Contains(searchPhrase, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            var total = products.Count;
            var paginedData = products
                .Skip((current - 1) * rowCount)
                .Take(rowCount)
                .ToList();

            return Json(new
            {
                current,
                rowCount,
                rows = paginedData,
                total
            });
        }
        public IActionResult Upsert(int id)
        {
            Product response = new Product();
            if (id != 0)
            {
                response = _productServices.GetById(id);
            }

            return View(response);
        }

        [HttpPost]
        public IActionResult Save(Product product)
        {
            if (product.Id == 0)
            {
                _productServices.Insert(ref product);
            }
            else
            {
                _productServices.UpdateData(product);
            }

            return RedirectToAction("Upsert", "Products", new { id = product.Id });
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                _productServices.Delete(id);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    msg = ex.Message
                });
            }

            return Json(new
            {
                success = true,
                msg = "Excluido com sucesso"
            });
        }
    }
}
