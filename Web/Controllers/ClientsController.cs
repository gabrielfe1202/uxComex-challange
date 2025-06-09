using Microsoft.AspNetCore.Mvc;
using UXComex_challenge.Application.Interfaces;
using UXComex_challenge.Domain.Entities;

namespace UXComex_challenge.Web.Controllers
{
    public class ClientsController : Controller
    {
        private readonly IService<Client> _clientService;

        public ClientsController(IService<Client> clientService)
        {
            _clientService = clientService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult List(int current = 1, int rowCount = 10, string searchPhrase = "")
        {
            var usuarios = _clientService.list();

            if (!string.IsNullOrEmpty(searchPhrase))
            {
                usuarios = usuarios.Where(u => u.Name.Contains(searchPhrase, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            var total = usuarios.Count;
            var dadosPaginados = usuarios
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
            Client respose = new Client();
            if(id != 0)
            {
                respose = _clientService.GetById(id);
            }

            return View(respose);
        }

        [HttpPost]
        public IActionResult Save(Client client)
        {
            if(client.Id == 0)
            {
                _clientService.Insert(ref client);
            }
            else
            {
                _clientService.UpdateData(client);
            }

            return RedirectToAction("Upsert", "Clients", new { id = client.Id });
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                _clientService.Delete(id);
            }catch (Exception ex)
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
