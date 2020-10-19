using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ParkyWeb.StaticData
{
    public static class SD
    {
        public static string APIBaseUrl = "https://localhost:44322/";
        public static string NationalParkAPIPath  = "https://localhost:44322/api/NationalParks/";
        public static string  TrailAPIPath  = "https://localhost:44322/api/Trails/";

        public static void DefualtHeadreConfigurration()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
