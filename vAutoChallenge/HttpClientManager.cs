using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;


namespace vAutoChallenge
{
    /// <summary>
    /// Generic endpoints for async calls using HttpClient - 
    /// </summary>
    class HttpClientManager
    {
        public static async Task<T> GetDataAsync<T>(string uri) where T : class
        {
            Console.WriteLine("Running Get URI: {0}", uri);

            HttpClient httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            var response = httpClient.GetStreamAsync(uri);

            T responseObject = new DataContractJsonSerializer(typeof(T)).ReadObject(await response) as T;

            return responseObject;
        }

        public static async Task<IEnumerable<T>> GetDataAsync<T>(IEnumerable<string> uris) where T : class
        {
            List<T> results = new List<T>();

            List<Task<T>> tasks = new List<Task<T>>();

            foreach (var uri in uris)
            {
                tasks.Add(GetDataAsync<T>(uri));
            }

            foreach( var task in tasks)
            {
                results.Add( await task);
            }
            
            return results;
        }



        public static async Task<T> PostDataAsync<T>(string uri, object postObject) where T : class
        {
            Console.WriteLine("Running Post URI: {0}", uri);

            HttpClient httpClient = new HttpClient();
            
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            //httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");
            
            string data = JsonConvert.SerializeObject(postObject);

            var response = await httpClient.PostAsync(uri, new StringContent(data, Encoding.UTF8, "application/json"));

            T ds = new DataContractJsonSerializer(typeof(T)).ReadObject( response.Content.ReadAsStreamAsync().Result ) as T;

            return ds;
        }

      
    }
}
