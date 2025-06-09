using UXComex_challenge.Application.Interfaces;
using UXComex_challenge.Domain.Entities;

namespace UXComex_challenge.Application.Services
{
    public class ProductServices : IService<Product>
    {
        private readonly IRepository<Product> _repository;

        public ProductServices(IRepository<Product> repository)
        {
            _repository = repository;
        }

        public List<Product> list()
        {
            return _repository.GetAll();
        }
        public Product GetById(int id)
        {
            var clients = _repository.GetAll();
            return clients.FirstOrDefault(c => c.Id == id);
        }
        public void Insert(ref Product product)
        {
            int id = _repository.insert(product);

            product.Id = id;
        }
        public void UpdateData(Product product)
        {
            _repository.Update(product);
        }
        public bool Delete(int id)
        {
            return _repository.Delete(id);
        }
    }
}
