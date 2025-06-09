using UXComex_challenge.Application.Interfaces;
using UXComex_challenge.Domain.Entities;
using UXComex_challenge.Domain.ObjectValues;

namespace UXComex_challenge.Application.Services
{
    public class OrderServices : IService<Order>
    {
        private readonly IRepository<Order> _repository;
        private readonly IService<Product> _productServices;

        public OrderServices(IRepository<Order> repository, IService<Product> service)
        {
            _repository = repository;
            _productServices = service;
        }

        public List<Order> list()
        {
            return _repository.GetAll();
        }

        public Order GetById(int id)
        {
            return _repository.GetById(id);
        }

        public void Insert(ref Order order)
        {
            if (order == null || order.Items.Count() == 0) {
                throw new Exception("Pedido invalido");
            }

            order.CreatedAt = DateTime.Now;
            order.Status = "Novo";
            decimal total = 0;

            var products = _productServices.list();
            foreach(OrderItem item in order.Items)
            {
                var product = products.FirstOrDefault(p => p.Id == item.ProductId);
                if (product == null)
                {
                    throw new Exception($"Produto com ID {item.ProductId} não encontrado.");
                }
                if (item.Quantity <= 0)
                {
                    throw new Exception("Quantidade deve ser maior que zero.");
                }
                if (item.Quantity > product.Quantity)
                {
                    throw new Exception($"Estoque insuficiente para o produto {product.Name}. Estoque atual: {product.Quantity}, Quantidade solicitada: {item.Quantity}.");
                }
                item.UnitPrice = product.Price;
                total += item.Quantity * item.UnitPrice;
            }

            order.Total = total;

            int id = _repository.insert(order);
            order.Id = id;
        }

        public void UpdateData(Order order)
        {
            _repository.Update(order);
        }

        public bool Delete(int id)
        {
            return _repository.Delete(id);
        }
    }
}
