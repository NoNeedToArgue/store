namespace Store
{
    public interface IRepository
    {
        Product[] GetAllByName(string name);
        Product GetById(int id);
    }
}
