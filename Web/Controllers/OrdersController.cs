using Microsoft.AspNetCore.Mvc;
using UXComex_challenge.Application.Interfaces;
using UXComex_challenge.Application.Services;
using UXComex_challenge.Domain.Entities;

namespace UXComex_challenge.Web.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IService<Order> _orderServices;
        private readonly IService<Product> _productServices;
        private readonly IService<Client> _clientServices;
        private readonly IService<OrderNotification> _orderNotificartionServices;

        public OrdersController(IService<Order> orderServices, IService<Product> productServices, IService<Client> clientServices, IService<OrderNotification> orderNotificartionServices)
        {
            _orderServices = orderServices;
            _productServices = productServices;
            _clientServices = clientServices;
            _orderNotificartionServices = orderNotificartionServices;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult List(int current = 1, int rowCount = 10, string searchPhrase = "")
        {
            var orders = _orderServices.list();

            if (!string.IsNullOrEmpty(searchPhrase))
            {
                orders = orders.Where(u => u.ClientName.Contains(searchPhrase, StringComparison.OrdinalIgnoreCase) || u.Status.Contains(searchPhrase, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            var total = orders.Count;
            var dadosPaginados = orders
                .Skip((current - 1) * rowCount)
                .Take(rowCount)
                .ToList();

            return Json(new
            {
                current,
                rowCount,
                rows = dadosPaginados,
                total
            });
        }
        public IActionResult Upsert(int id)
        {
            Order respose = new Order();
            if (id != 0)
            {
                respose = _orderServices.GetById(id);
            }

            var clients = _clientServices.list();
            ViewBag.clients = clients;

            var products = _productServices.list();
            ViewBag.products = products;

            return View(respose);
        }

        [HttpPost]
        public IActionResult AlterarStatus(int id,[FromBody] string status)
        {
            try
            {
                var order = _orderServices.GetById(id);
                var not = new OrderNotification()
                {
                    ChangedAt = DateTime.Now,
                    NewStatus = status,
                    OldStatus = order.Status,
                    OrderId = order.Id
                };
                _orderNotificartionServices.Insert(ref not);
                order.Status = status;
                _orderServices.UpdateData(order);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao alterar status: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult Save([FromBody]Order order)
        {
            if (order.Id == 0)
            {
                _orderServices.Insert(ref order);
            }
            else
            {
                _orderServices.UpdateData(order);
            }

            return RedirectToAction("Upsert", "Orders", new { id = order.Id });
        }
        

        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                _orderServices.Delete(id);
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
