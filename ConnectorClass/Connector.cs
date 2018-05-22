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

            var client = new RestClient((string)jInput["YTurl"] + "/" + (string)jInput["YTdashboard"]);
            client.Authenticator = new JwtAuthenticator((string)jInput["YTtoken"]);
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
            dynamic exchangeList = new JArray();
            dynamic tempProduct = null;
            string widgetMessage = YouTrackConnect();
            Regex regex = new Regex(@"### (?<vendor>\w+)\n\n(?<models>(?:^\+.*$\n?)+)", RegexOptions.Multiline);
            MatchCollection matches = regex.Matches(widgetMessage);
            Regex regex2 = new Regex(@"^\+ \[(?<model>\S+)\]\((?<url>\S+)\)(?: - (?<fw>.+))?$", RegexOptions.Multiline);

            foreach (Match m in matches)
            {
                dynamic products = new JObject();
                dynamic modelList = new JArray();
                products.Add("Vendor", m.Groups["vendor"].Value);
                MatchCollection matches2 = regex2.Matches(m.Groups["models"].Value);
                foreach (Match m2 in matches2)
                {
                    tempProduct = new JObject();
                    tempProduct.Model = m2.Groups["model"].ToString();
                    tempProduct.Url = m2.Groups["url"].ToString();
                    tempProduct.FW = m2.Groups["fw"].ToString();
                    modelList.Add(tempProduct);
                }
                products.Add("Models", modelList);
                exchangeList.Add(products);
            }
            //Console.WriteLine(exchangeList);
            MarkdownSerializer(exchangeList); //убрать после отладки

        }

        public void MarkdownSerializer(JArray exchangeList)
        {
            StringBuilder markdownContent = new StringBuilder();
            foreach (JObject disassemb0 in exchangeList)
            {
                markdownContent.AppendLine("### " + disassemb0["Vendor"] + "\n");
                foreach (JObject disassemb1 in disassemb0["Models"])
                {
                    markdownContent.Append(string.Format(@"+ [{0}]({1})", disassemb1["Model"], disassemb1["Url"]));
                    if (disassemb1["FW"].ToString() != "") markdownContent.AppendLine(@" - " + disassemb1["FW"]);
                    else if (!(disassemb1 == disassemb0["Models"].Last)) markdownContent.AppendLine();
                    if ((disassemb1 == disassemb0["Models"].Last) && (!(disassemb0 == exchangeList.Last))) markdownContent.AppendLine();     
                }
                if (!(disassemb0 == exchangeList.Last)) markdownContent.AppendLine();
                
            }
            Console.Write(markdownContent);
        }

        public void YoutrackConnectPost()
        {

        }
    }
}
