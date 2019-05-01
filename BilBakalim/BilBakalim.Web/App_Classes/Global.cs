using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace BilBakalim.Web.App_Classes
{
    public static class Global
    {
        public static HttpClient client = new HttpClient();

       static Global()
        {
            client.BaseAddress = new Uri("http://localhost:28831/");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

    }
}