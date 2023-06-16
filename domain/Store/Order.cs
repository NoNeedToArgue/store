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

        public int TotalCount => _items.Sum(item => item.Count);

        public decimal TotalPrice => _items.Sum(item => item.Price * item.Count);
        
        public Order(int id, IEnumerable<OrderItem> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));

            Id = id;
            _items = new List<OrderItem>(items);
        }

        public OrderItem GetItem(int productId)
        {
            int index = _items.FindIndex(item => item.ProductId == productId);

            if (index == -1) ThrowProductException("Product not found.", productId);

            return _items[index];
        }

        public void AddOrUpdateItem(Product product, int count)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));

            int index = _items.FindIndex(item => item.ProductId == product.Id);

            if (index == -1)
                _items.Add(new OrderItem(product.Id, count, product.Price));
            else
                _items[index].Count += count;
        }

        public void RemoveItem(int productId)
        {
            int index = _items.FindIndex(item => item.ProductId == productId);

            if (index == -1) ThrowProductException("Order does not contain specified item.", productId);

            _items.RemoveAt(index);
        }

        private void ThrowProductException(string message, int productId)
        {
            var exception = new InvalidOperationException(message);

            exception.Data["ProductId"] = productId;

            throw exception;
        }
    }
}
