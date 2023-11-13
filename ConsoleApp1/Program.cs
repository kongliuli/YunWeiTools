using StackExchange.Redis;

internal class Program
{
    private static void Main(string[] args)
    {

        Connection();



        Console.ReadLine();
    }

    public static void Connection()
    {

        // 创建Redis客户端
        ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("hssc16.3hvkim.clustercfg.use1.cache.amazonaws.com:6379,abortConnect=false");

        // 获取数据库
        IDatabase db = redis.GetDatabase(0);
        Console.WriteLine(db.KeyExists("1"));

        var a = redis.GetDatabase();



        Console.WriteLine(a);

    }
}