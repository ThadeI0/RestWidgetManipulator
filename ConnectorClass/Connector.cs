using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using RestSharp;
using RestSharp.Authenticators;
using Newtonsoft.Json;

namespace YouTrackHubExchanger.ConnectorClass
{
    class Connector
    {
        public StreamReader YouTrackRestParams()
        {
            StreamReader youtrackParams = new StreamReader(@"..\..\..\inputdata\YouTrackInput.json");
            
            return youtrackParams;
        }

        public void YouTrackConnect()
        {
            
            var YTparams = YouTrackRestParams();
            

            //while ((line = YTparams.ReadLine()) != null)
            //{
                
            //}

            var client = new RestClient();
            // client.Authenticator = new HttpBasicAuthenticator(username, password);

            var request = new RestRequest("resource/{id}", Method.POST);
            request.AddParameter("name", "value"); // adds to POST or URL querystring based on Method
            request.AddUrlSegment("id", "123"); // replaces matching token in request.Resource

            // easily add HTTP Headers
            request.AddHeader("header", "value");

            // add files to upload (works with compatible verbs)
            //request.AddFile(path);

            // execute the request
            IRestResponse response = client.Execute(request);
            var content = response.Content;
        }
    }
}
