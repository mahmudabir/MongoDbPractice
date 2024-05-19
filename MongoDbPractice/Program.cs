using CQRS.Kit.MethodInterceptor;

namespace MongoDbPractice
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await new Test().RunAsync();
        }
    }
}
