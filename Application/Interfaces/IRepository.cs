using UXComex_challenge.Domain.Entities;

namespace UXComex_challenge.Application.Interfaces
{
    public interface IRepository<T>
    {
        public List<T> GetAll();
        public T GetById(int id);
        public int insert(T data);
        public bool Update(T data);
        public bool Delete(int id);
    }
}
