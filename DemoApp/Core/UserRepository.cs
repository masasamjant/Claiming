namespace DemoApp.Core
{
    public class UserRepository : Repository<User>
    {
        public UserRepository() 
        { }

        public override void Initialize(string rootFolder)
        {
            var filePath = Path.Combine(rootFolder, "files", "Users.txt");

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
                    var user = new User(values[0])
                    {
                        FirstName = values[1],
                        LastName = values[2]
                    };
                    Add(user);
                }
            }
        }
    }
}
