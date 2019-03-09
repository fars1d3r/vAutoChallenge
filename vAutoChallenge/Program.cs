using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace vAutoChallenge
{
    class Program
    {
        static void Main(string[] args)
        {
            List<vehicle> vehicles = new List<vehicle>();
            List<dealer> dealers = new List<dealer>();
            dealerPost dealerPost = new dealerPost();

            //ServicePointManager.DefaultConnectionLimit = 100;

            string uriRoot = "http://vautointerview.azurewebsites.net/api";

            //Get Dataset - datasetId is required for all subsequent calls
            DataSet ds = HttpClientManager.GetDataAsync<DataSet>(string.Format("{0}/datasetId", uriRoot)).Result;

            //Get Vehicles list for dataset -  
            VehiclesResponse vehiclesResponse = HttpClientManager.GetDataAsync<VehiclesResponse>(string.Format("{0}/{1}/vehicles", uriRoot ,ds.datasetId)).Result;
            
            //Build array of uris to get individual vehicle information
            List<string> uris = vehiclesResponse.vehicleIds.Select(item => 
                               string.Format("{0}/{1}/vehicles/{2}", uriRoot, ds.datasetId, item)).ToList();

            //Async batch gets for the vehicles
            vehicles.AddRange(HttpClientManager.GetDataAsync<vehicle>(uris).Result);
            
            //Get distinct uri to select dealers based on vehicles returned
            List<string> dealerUris = vehicles.Select(item => item.dealerId).Distinct()
                       .Select(dealerId => string.Format("{0}/{1}/dealers/{2}", uriRoot, ds.datasetId, dealerId)).ToList();

            //Async Batch gets for dealers
            dealers.AddRange(HttpClientManager.GetDataAsync<dealer>(dealerUris).Result);

            //Associate vehicles with dealers
            foreach (var dealer in dealers)
            {
                dealer.vehicles.AddRange(vehicles.Where(item => item.dealerId == dealer.dealerId));
            }

            //Populate Answer Object for posting
            dealerPost.dealers = dealers;

            //post answer to dataset endpoint
            DatasetModelResponse dsReply = HttpClientManager.PostDataAsync<DatasetModelResponse>(string.Format("{0}/{1}/answer", uriRoot, ds.datasetId), dealerPost).Result;            
            
            Console.WriteLine("Reply: Message: {0}   Success: {1}  TotalMs: {2} ", dsReply.message, dsReply.success, dsReply.totalMilliseconds);
            Console.WriteLine("Press enter to end.");
            Console.Read();
        }
    }
}
