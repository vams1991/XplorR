using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using System.IO;
using System.Xml;
using System.Runtime.Serialization.Json;
using System.Xml.Linq;

namespace WcfService1
{
    public class Service1 : IService1
    {
        //method takes placeName as parameter and returns population values as output
        public string GetPopulation(string placeName)
        {
            string API_KEY="bgaxcn4catrsed6w72q5emck";
            //accessing the API using the url of the service
            string fullURL= "http://api.usatoday.com/open/census/pop?keypat="+placeName+"&keyname=placename&sumlevid=2&api_key="+API_KEY;
            //Used Uri class to convert the URL in string format into Uri type
            Uri ServivrUri= new Uri(fullURL);
            WebClient proxy= new WebClient();
            //the remote service returns the data in a byte[] array
            byte[] abc=proxy.DownloadData(ServivrUri);
            //convert the array into a string
            string s= System.Text.UTF8Encoding.UTF8.GetString(abc);
            //convert string in json format into xml
            XmlDocument xdoc = ConvertJsonToXML(s);
            //parsing the fetched values
            XmlNodeList elemList1 = xdoc.GetElementsByTagName("Pop");
            XmlNodeList elemList2 = xdoc.GetElementsByTagName("PctChange");
            XmlNodeList elemList3 = xdoc.GetElementsByTagName("PopSqMi");
            XmlNodeList elemList4 = xdoc.GetElementsByTagName("StatePostal");
            String output = "";
            //passing the parsed values into a string
            for (int i = 0; i < elemList1.Count;i++ )
            {
                output += "Population: " + elemList1[i].InnerText+"\n";
                output += "Percentage change in population for an area from Census 2000 to Census 2010: " + elemList2[i].InnerText + "\n";
                output += "The population density of an area, represented by number of people per square mile of land area: " + elemList3[i].InnerText + "\n";
                output += "StatePostal: " + elemList4[i].InnerText+"\n";
            }
            return output;

        }
        //method to convert jsonstring to xml
        public XmlDocument ConvertJsonToXML(string strinjson)
        {
            XmlDocument doc = new XmlDocument();

            using (var value = JsonReaderWriterFactory.CreateJsonReader(Encoding.UTF8.GetBytes(strinjson), XmlDictionaryReaderQuotas.Max))
            {
                XElement elem = XElement.Load(value);
                doc.LoadXml(elem.ToString());
            }

            return doc;

        }

       
    }
}
