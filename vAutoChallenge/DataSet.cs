using System;
using System.Collections.Generic;
using System.Text;

namespace vAutoChallenge
{
    public class DataSet
    {
        public string datasetId { get; set; }
    }

    public class DatasetModelResponse
    {
        public bool success { get; set;}
        public string message { get; set; }
        public int totalMilliseconds { get; set; }
    }

    public class VehiclesResponse
    {
        public int[] vehicleIds { get; set; }
    }

    public class vehicle
    {
        public int vehicleId { get; set; }
        public int year { get; set; }
        public string make { get; set; }
        public string model { get; set; }
        public int dealerId { get; set; }

    }

    public class dealer
    {
        public int dealerId { get; set; }
        public string name { get; set; }
        public List<vehicle> vehicles { get; set; }

        public dealer()
        {
            vehicles = new List<vehicle>();
        }
    }

    public class dealerPost
    {
        public List<dealer> dealers { get; set; }

        public dealerPost()
        {
            dealers = new List<dealer>();
        }
    }

}
