using System;
using YouTrackHubExchanger.ConnectorClass;

namespace YouTrackHubExchanger
{
    class Program
    {
        static void Main(string[] args)
        {
            Connector connector = new Connector();
            connector.YouTrackConnect();
            Console.WriteLine("Hello World!");
        }
    }
}
