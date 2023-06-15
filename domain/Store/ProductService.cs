namespace Store
{
    public class ProductService
    {
        private readonly IRepository repository;

        public ProductService(IRepository repository)
        {
            this.repository = repository;
        }
        public Product[] GetAllByQuery(string query)
        {
            return repository.GetAllByName(query);
        }
    }
}
