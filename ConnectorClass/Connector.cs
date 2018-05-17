using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using RestSharp;
using RestSharp.Authenticators;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace YouTrackHubExchanger.ConnectorClass
{
    class Connector
    {
        public string YouTrackRestParams()
        {
            string jsonInput = File.ReadAllText(@"..\..\..\inputdata\YouTrackInput.json");            
            return jsonInput;
        }

        public string YouTrackConnect()
        {
            
            JObject jInput = JObject.Parse(YouTrackRestParams());

            var client = new RestClient((string) jInput["YTurl"] + "/" + (string) jInput["YTdashboard"]);
            client.Authenticator = new JwtAuthenticator((string) jInput["YTtoken"]);
            var request = new RestRequest(Method.GET);      
            request.AddHeader("Accept", "application/json");
            
            IRestResponse response = client.Execute(request);
            var content = response.Content;

            JObject contentGET = JObject.Parse(content);
            JToken widgetID = contentGET.SelectToken(string.Format(@"$.data.widgets[?(@.config.id=='{0}')].config.message", (string)jInput["YTwidget"]));

            return widgetID.ToString();

        }

        public void MarkdownDeserializer()
        {
            string widgetMessage = YouTrackConnect();
            Regex regex = new Regex(@"### (\w+)\n\n((?:^\+.*$\n?)+)", RegexOptions.Multiline);
            MatchCollection matches = regex.Matches(widgetMessage);
            Console.WriteLine(matches[0].Value);
        }
    }
}
