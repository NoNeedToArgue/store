namespace Store
{
    public class Order
    {
        public int Id { get; }

        private List<OrderItem> _items;

        public IReadOnlyCollection<OrderItem> Items
        {
            get { return _items; }
        }

        public int TotalCount
        {
            get { return _items.Sum(item => item.Count); }
        }

        public decimal TotalPrice
        {
            get { return _items.Sum(item => item.Price * item.Count); }
        }

        public Order(int id, IEnumerable<OrderItem> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));

            Id = id;
            _items = new List<OrderItem>(items);
        }

        public void AddItem(Product product, int count)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));

            var item = _items.SingleOrDefault(i => i.ProductId == product.Id);

            if (item == null)
            {
                _items.Add(new OrderItem(product.Id, count, product.Price));
            }
            else
            {
                _items.Remove(item);
                _items.Add(new OrderItem(product.Id, item.Count + count, product.Price));
            }    
        }

        public void RemoveItem(Product product, int count)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));

            var item = _items.SingleOrDefault(i => i.ProductId == product.Id);

            if (item == null)
            {
                return;
            }
            else
            {
                _items.Remove(item);
            }
        }
    }
}
