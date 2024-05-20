using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using MongoDB.Bson;

using MongoDbPractice.DataAccess.Models;

namespace CQRS.Kit.MethodInterceptor
{
    public class App
    {
        private readonly IServiceProvider serviceProvider;
        private readonly int loopCount = 10;

        public App()
        {
            string connectionString = "mongodb+srv://abir:Password12@mongodbcluster.3hvxclb.mongodb.net/";
            string databaseName = "MongoDbPractice";

            serviceProvider = new ServiceCollection()
                                    .AddMongoDB<MyDbContext>(connectionString, databaseName)
                                    //.AddDbContext<MyDbContext>(options => options.UseMongoDB(connectionString, databaseName))method linq
                                    .BuildServiceProvider();
        }

        public async Task RunAsync()
        {
            MyDbContext dbContext = serviceProvider.GetService<MyDbContext>()!;


            Func<User, ObjectId> outerKeySelector = x => x.Id;
            Func<Customer, ObjectId> innerKeySelector = x => x.UserId;
            Func<User, Customer, User> resultSelector = ((user, customer) =>
            new User
            {
                Id = user.Id,
                Name = user.Name,
                Customer = customer
            });
            ////Currently Does not support in 8.0.0
            //List<User> users = dbContext.Users.Join(dbContext.Customers, outerKeySelector, innerKeySelector, resultSelector).ToList();

            List<User> users = await dbContext.Users.ToListAsync();
            List<Customer> customers = await dbContext.Customers.Where(x => users.Select(x => x.Id).Contains(x.UserId)).ToListAsync();

            users = users.Join(customers, outerKeySelector, innerKeySelector, resultSelector).ToList();

            if (users.Count() == 0)
            {
                List<User> userList = new List<User>()
                {
                    new User{ Name = "Abir Mahmud", Customer = new Customer{ Type = CustomerType.Special } },
                    new User{ Name = "Sadiya Islam", Customer = new Customer{ Type = CustomerType.Special } },
                    new User{ Name = "Swapnil Mahmuda", Customer = new Customer{ Type = CustomerType.Special } },
                    new User{ Name = "Md. Saiful Alam", Customer = new Customer{ Type = CustomerType.Special } },
                    new User{ Name = "Makamam Mahmuda", Customer = new Customer{ Type = CustomerType.Special } },
                    new User{ Name = "Md. Abdus Salam", Customer = new Customer{ Type = CustomerType.Basic } },
                    new User{ Name = "Monir Hayat Siddik", Customer = new Customer{ Type = CustomerType.Basic } },
                    new User{ Name = "Touhid Jihad", Customer = new Customer{ Type = CustomerType.Basic } },
                    new User{ Name = "Raiyan Rafi", Customer = new Customer{ Type = CustomerType.Banned } },
                    new User{ Name = "Sabbir Hossain", Customer = new Customer{ Type = CustomerType.Banned } },
                    new User{ Name = "Alamin Islam", Customer = new Customer{ Type = CustomerType.Banned } },
                };

                await dbContext.AddRangeAsync(userList);
                await dbContext.SaveChangesAsync();
            }

            await Parallel.ForEachAsync(users, async (x, ct) =>
            {
                Console.WriteLine(x.Name +" - "+ x.Customer.Type.ToString());
            });
            Console.WriteLine();
            users.ForEach(x => Console.WriteLine(x.Name + " - " + x.Customer.Type.ToString()));


        }
    }
}