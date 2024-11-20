namespace DemoApp.Core
{
    public class ProductRepository : Repository<Product>
    {
        public ProductRepository()
        { }

        public override void Initialize(string rootFolder)
        {
            var filePath = Path.Combine(rootFolder, "files", "Products.txt");

            if (!File.Exists(filePath))
                return;

            using (var stream = File.OpenRead(filePath))
            using (var reader = new StreamReader(stream))
            {
                string? line;

                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith('#'))
                        continue;
                    var values = line.Split(';');
                    var product = new Product(Guid.NewGuid(), values[0])
                    {
                        Description = values[1]
                    };
                    Add(product);
                }
            }
        }
    }
}
