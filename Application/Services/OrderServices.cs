using UXComex_challenge.Application.Interfaces;
using UXComex_challenge.Domain.Entities;

namespace UXComex_challenge.Application.Services
{
    public class OrderServices : IService<Order>
    {
        private readonly IRepository<Order> _repository;

        public OrderServices(IRepository<Order> repository)
        {
            _repository = repository;
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
