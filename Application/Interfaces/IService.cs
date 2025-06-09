namespace UXComex_challenge.Application.Interfaces
{
    public interface IService<T> where T : class
    {
        public List<T> list();
        public T GetById(int id);
        public void Insert(ref T data);
        public void UpdateData(T data);
        public bool Delete(int id);
    }
}
