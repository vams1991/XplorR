using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Security.Cryptography;
using System.Runtime.Serialization.Json;
using System.Web.Script.Serialization;

namespace ElectiveServices
{

    public class ElecServices : ElectiveServices
    {
        private const string oauth_consumer_key = "rpImM5giYKdQGP8XwmH1vgrLM";
        private const string oauth_consumer_secret = "r4aFKbZx0INno3HMnjJ9pO6fwpHOyT5DaUQWKopWecVDZ1ahAh";
        private const string oauth_token = "32807725-9ZKLri1oMM6f6ehRc2ASnT0oDBWlNDetDJRyaA6rQ";
        private const string oauth_token_secret = "3ZeNMUwwIPWIC6rHzOkNatU6xD9fp1rILHCpIjKyESqFw";
        private const string oauth_version = "1.0";
        private const string oauth_signature_method = "HMAC-SHA1";
        public string getWoeid(string city_name)
        {
            string baseString = "{0}('{1}')?appid={2}"; 
            string appId = "dj0yJmk9Q013OHFyUmJiU1FPJmQ9WVdrOVFtZDFXVWxvTm04bWNHbzlNQS0tJnM9Y29uc3VtZXJzZWNyZXQmeD00ZA--";
            string yahooUrl = @"http://where.yahooapis.com/v1/places.q";
            string url = String.Format(baseString, yahooUrl, city_name, appId);
            
            // Create a request object
            WebRequest req = WebRequest.Create(url);
            
            try
            {
                // Get response from the endpoint
                WebResponse res = (WebResponse)req.GetResponse();
                Stream dataStream = res.GetResponseStream();
                // Open a stream
                StreamReader reader = new StreamReader(dataStream);
                // Read the content from the response stream
                string responseFromServer = reader.ReadToEnd();
                
                // Load the document and extract WOEID
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(responseFromServer);
                XmlNode woeid = doc.GetElementsByTagName("woeid")[0];
                return woeid.InnerText;
            }
            catch (Exception ex)
            {
                // Return error message if any
                return ex.Message;
            }
        }
        // Referenced stackoverflow: http://stackoverflow.com/questions/814001/convert-json-string-to-xml-or-xml-to-json-string
        private XmlDocument ConvertJsonToXml(string json)
        {
            XmlDocument doc = new XmlDocument();
            XmlDictionaryReader reader = JsonReaderWriterFactory.CreateJsonReader(Encoding.UTF8.GetBytes(json), XmlDictionaryReaderQuotas.Max);
            XElement ele = XElement.Load(reader);
            doc.LoadXml(ele.ToString());
            return doc;
        }
        public TwitterTrending fetchTrending(string city_name)
        {
            // Get WOEID
            string woeid = getWoeid(city_name);
            string url = "https://api.twitter.com/1.1/trends/place.json?id=" + woeid;
            
            // Create OAuth parameters for twitter API
            string oauth_nonce = Convert.ToBase64String(
              new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()));
            TimeSpan time_span = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            string oauthtimestamp = Convert.ToInt64(time_span.TotalSeconds).ToString();
            SortedDictionary<string, string> basestringParameters = new SortedDictionary<string, string>();
            basestringParameters.Add("id", woeid);
            basestringParameters.Add("oauth_version", oauth_version);
            basestringParameters.Add("oauth_consumer_key", oauth_consumer_key);
            basestringParameters.Add("oauth_nonce", oauth_nonce);
            basestringParameters.Add("oauth_signature_method", oauth_signature_method);
            basestringParameters.Add("oauth_timestamp", oauthtimestamp);
            basestringParameters.Add("oauth_token", oauth_token);
            //Build the signature string
            string baseString = String.Empty;
            baseString += "GET" + "&";
            baseString += Uri.EscapeDataString(url.Split('?')[0]) + "&";
            foreach (KeyValuePair<string, string> entry in basestringParameters)
            {
                baseString += Uri.EscapeDataString(entry.Key + "=" + entry.Value + "&");
            }

            //Remove the trailing ambersand char last 3 chars - %26
            baseString = baseString.Substring(0, baseString.Length - 3);

            //Build the signing key
            string signingKey = Uri.EscapeDataString(oauth_consumer_secret) + "&" + Uri.EscapeDataString(oauth_token_secret);

            //Sign the request
            HMACSHA1 hasher = new HMACSHA1(new ASCIIEncoding().GetBytes(signingKey));
            string oauthsignature = Convert.ToBase64String(hasher.ComputeHash(new ASCIIEncoding().GetBytes(baseString)));

            //Tell Twitter we don't do the 100 continue thing
            ServicePointManager.Expect100Continue = false;

            //authorization header
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(@url);
            string authorizationHeaderParams = String.Empty;
            authorizationHeaderParams += "OAuth ";
            authorizationHeaderParams += "oauth_nonce=\"" + Uri.EscapeDataString(oauth_nonce) + "\",";
            authorizationHeaderParams += "oauth_signature_method=\"" + Uri.EscapeDataString(oauth_signature_method) + "\",";
            authorizationHeaderParams += "oauth_timestamp=\"" + Uri.EscapeDataString(oauthtimestamp) + "\",";
            authorizationHeaderParams += "oauth_consumer_key=\"" + Uri.EscapeDataString(oauth_consumer_key) + "\",";
            authorizationHeaderParams += "oauth_token=\"" + Uri.EscapeDataString(oauth_token) + "\",";
            authorizationHeaderParams += "oauth_signature=\"" + Uri.EscapeDataString(oauthsignature) + "\",";
            authorizationHeaderParams += "oauth_version=\"" + Uri.EscapeDataString(oauth_version) + "\"";
            webRequest.Headers.Add("Authorization", authorizationHeaderParams);

            webRequest.Method = "GET";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            TwitterTrending tw = new TwitterTrending();
            //Allow us a reasonable timeout in case Twitter's busy
            webRequest.Timeout = 3 * 60 * 1000;
            try
            {
                //Proxy settings
                HttpWebResponse webResponse = webRequest.GetResponse() as HttpWebResponse;
                Stream dataStream = webResponse.GetResponseStream();
                
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                
                XmlDocument doc = ConvertJsonToXml(responseFromServer);

                XmlNodeList names = doc.GetElementsByTagName("name");
                tw.TrendList = new List<Trends>();
                foreach (XmlNode n in names)
                {
                    Trends t = new Trends();
                    t.Name = n.InnerText;
                    tw.TrendList.Add(t);
                }
                tw.Location = city_name;
                
            }
            catch (Exception ex)
            {
                tw.Error = ex.Message;
            }
            return tw;
        }
        
        
        public TweetCount fetchCount(string topic, string city_name)
        {

            LatLng l = new LatLng();
            l.getLatLng(city_name);
            string geocode = l.lat + "," + l.lng + ",10km";
            string param_string = topic; // +"&geocode" + geocode;
            string url = "https://api.twitter.com/1.1/search/tweets.json?q=" + param_string;

            // Create OAuth parameters for twitter API
            string oauth_nonce = Convert.ToBase64String(new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()));
            TimeSpan time_span = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            string oauthtimestamp = Convert.ToInt64(time_span.TotalSeconds).ToString();
            SortedDictionary<string, string> basestringParameters = new SortedDictionary<string, string>();
            basestringParameters.Add("q", topic);
            //basestringParameters.Add("geocode", geocode);
            basestringParameters.Add("oauth_version", oauth_version);
            basestringParameters.Add("oauth_consumer_key", oauth_consumer_key);
            basestringParameters.Add("oauth_nonce", oauth_nonce);
            basestringParameters.Add("oauth_signature_method", oauth_signature_method);
            basestringParameters.Add("oauth_timestamp", oauthtimestamp);
            basestringParameters.Add("oauth_token", oauth_token);
            //Build the signature string
            string baseString = String.Empty;
            baseString += "GET" + "&";
            baseString += Uri.EscapeDataString(url.Split('?')[0]) + "&";
            foreach (KeyValuePair<string, string> entry in basestringParameters)
            {
                baseString += Uri.EscapeDataString(entry.Key + "=" + entry.Value + "&");
            }

            //Remove the trailing ambersand char last 3 chars - %26
            baseString = baseString.Substring(0, baseString.Length - 3);

            //Build the signing key
            string signingKey = Uri.EscapeDataString(oauth_consumer_secret) + "&" + Uri.EscapeDataString(oauth_token_secret);

            //Sign the request
            HMACSHA1 hasher = new HMACSHA1(new ASCIIEncoding().GetBytes(signingKey));
            string oauthsignature = Convert.ToBase64String(hasher.ComputeHash(new ASCIIEncoding().GetBytes(baseString)));

            //Tell Twitter we don't do the 100 continue thing
            ServicePointManager.Expect100Continue = false;

            //authorization header
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(@url);
            string authorizationHeaderParams = String.Empty;
            authorizationHeaderParams += "OAuth ";
            authorizationHeaderParams += "oauth_nonce=\"" + Uri.EscapeDataString(oauth_nonce) + "\",";
            authorizationHeaderParams += "oauth_signature_method=\"" + Uri.EscapeDataString(oauth_signature_method) + "\",";
            authorizationHeaderParams += "oauth_timestamp=\"" + Uri.EscapeDataString(oauthtimestamp) + "\",";
            authorizationHeaderParams += "oauth_consumer_key=\"" + Uri.EscapeDataString(oauth_consumer_key) + "\",";
            authorizationHeaderParams += "oauth_token=\"" + Uri.EscapeDataString(oauth_token) + "\",";
            authorizationHeaderParams += "oauth_signature=\"" + Uri.EscapeDataString(oauthsignature) + "\",";
            authorizationHeaderParams += "oauth_version=\"" + Uri.EscapeDataString(oauth_version) + "\"";
            webRequest.Headers.Add("Authorization", authorizationHeaderParams);

            webRequest.Method = "GET";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            
            TweetCount tc = new TweetCount();
            //Allow us a reasonable timeout in case Twitter's busy
            webRequest.Timeout = 3 * 60 * 1000;
            
            try
            {
                //Proxy settings
                HttpWebResponse webResponse = webRequest.GetResponse() as HttpWebResponse;
                Stream dataStream = webResponse.GetResponseStream();

                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();

                XmlDocument doc = ConvertJsonToXml(responseFromServer);
                XmlNodeList items = doc.GetElementsByTagName("item");
                tc.Count = Convert.ToString(items.Count);
                tc.Tweets = new List<Tweets>();
                XmlNodeList tweets = doc.GetElementsByTagName("text");
                foreach (XmlNode t in tweets)
                {
                    Tweets tt = new Tweets();
                    tt.Text = t.InnerText;
                    tc.Tweets.Add(tt);
                }
            }
            catch (Exception ex)
            {
                tc.Error = ex.Message;
            }
            return tc;
        }

    }

    public class LatLng
    {
        public string lat;
        public string lng;
        public void getLatLng(string city_name)
        {
            XmlDocument doc = new XmlDocument();
            string apiKey = "AIzaSyDYsII60Y0V31K96xyq2lwGrNDnOrAf-8E";
            string url = @"https://maps.googleapis.com/maps/api/geocode/xml?address=" + city_name + "&key=" + apiKey;
            doc.Load(url);

            if (doc.GetElementsByTagName("status")[0].ChildNodes[0].InnerText == "OK")
            {
                XmlNodeList location = doc.GetElementsByTagName("location");
                Console.WriteLine(location.Count);

                for (int i = 0; i < location.Count; i++)
                {
                    lat = location[i].SelectSingleNode("lat").InnerText;
                    lng = location[i].SelectSingleNode("lng").InnerText;
                }
            }
        }
    }
}
