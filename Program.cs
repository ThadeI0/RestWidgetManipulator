using System;
using YouTrackHubExchanger.ConnectorClass;

namespace YouTrackHubExchanger
{
    class Program
    {
        static void Main(string[] args)
        {
            Connector connector = new Connector();
            connector.YouTrackRestParams();
            connector.YouTrackConnect();
            connector.MarkdownDeserializer();
            connector.MarkdownSerializer();
            Console.WriteLine("All operations done");
            Console.ReadKey();
        }
    }
}
