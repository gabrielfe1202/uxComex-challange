using UXComex_challenge.Application.Interfaces;
using UXComex_challenge.Application.Repository;
using UXComex_challenge.Domain.Entities;

namespace UXComex_challenge.Application.Services
{
    public class ClientServices : IService<Client>
    {
        private readonly IRepository<Client> _repository;

        public ClientServices(IRepository<Client> repository)
        {
            _repository = repository;
        }

        public List<Client> list()
        {
            return _repository.GetAll();
        }
        public Client GetById(int id)
        {
            var clients = _repository.GetAll();
            return clients.FirstOrDefault(c => c.Id == id);
        }
        public void Insert(ref Client client)
        {
            client.RegisteredAt = DateTime.Now;

            int id = _repository.insert(client);

            client.Id = id;
        }
        public void UpdateData(Client client) 
        {
            _repository.Update(client);
        }
        public bool Delete(int id)
        {
            return _repository.Delete(id);
        }
    }
}

