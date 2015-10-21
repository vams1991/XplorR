using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ElectiveServices
{
    [ServiceContract]
    public interface ElectiveServices
    {

        [OperationContract]
        [WebGet(UriTemplate = "/trending/place={value}")]
        TwitterTrending fetchTrending(string value);

        [OperationContract]
        [WebGet(UriTemplate = "/count/q={topic},{city_name}")]
        TweetCount fetchCount(string topic, string city_name);
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class TwitterTrending
    {
        [DataMember]
        public List<Trends> TrendList { get; set; }
        [DataMember]
        public string Location { get; set; }
        [DataMember]
        public string Error { get; set; }
    }
    
    [DataContract]
    public class Trends
    {
        [DataMember]
        public string Name { get; set; }
    }
    [DataContract]
    public class TweetCount
    {
        [DataMember]
        public string Count { get; set; }
        [DataMember]
        public string Error { get; set; }
        [DataMember]
        public List<Tweets> Tweets { get; set; }
    }
    [DataContract]
    public class Tweets
    {
        [DataMember]
        public string Text { get; set; }
    }

}
