using UXComex_challenge.Application.Interfaces;
using UXComex_challenge.Domain.Entities;

namespace UXComex_challenge.Application.Services
{
    public class OrderNotificationServices : IService<OrderNotification>
    {
        private readonly IRepository<OrderNotification> _repository;

        public OrderNotificationServices(IRepository<OrderNotification> repository)
        {
            _repository = repository;
        }

        public List<OrderNotification> list()
        {
            return _repository.GetAll();
        }

        public OrderNotification GetById(int id)
        {
            var notifications = _repository.GetAll();
            return notifications.FirstOrDefault(n => n.Id == id);
        }

        public void Insert(ref OrderNotification notification)
        {
            int id = _repository.insert(notification);
            notification.Id = id;
        }

        public void UpdateData(OrderNotification notification)
        {
            _repository.Update(notification);
        }

        public bool Delete(int id)
        {
            return _repository.Delete(id);
        }
    }
}
