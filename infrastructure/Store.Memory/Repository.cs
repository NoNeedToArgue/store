namespace Store.Memory
{
    public class Repository : IRepository
    {
        private readonly Product[] products = new[]
        {
            new Product(1, "Юкио Мисима: Дом Кёко", "", 1000m),
            new Product(2, "Джессика Олсон: Спой мне о забытом", "", 1000m),
            new Product(3, "Дженнифер Хей: Улица милосердия", "", 1000m),
            new Product(4, "Аврора Вентурини: Кузины", "", 1000m),
            new Product(5, "Джессамин Чан: Школа хороших матерей", "", 1000m),
            new Product(6, "Гуйюань Тянься: Восхождение фениксов", "", 1000m),
            new Product(7, "Ли Бардуго: Штурм и буря", "", 1000m),
            new Product(8, "Дженнифер Арментроут: Тень и искры", "", 1000m),
            new Product(9, "Лорен Грофф: Аббатиса", "", 1000m),
            new Product(10, "Али Хейзелвуд: Гипотеза любви", "", 1000m),
        };

        public Product[] GetAllByName(string name)
        {
            if (name == null) return null;
            return products.Where(i => i.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToArray();
        }

        public Product GetById(int id)
        {
            return products.Single(i => i.Id == id);
        }
    }
}
