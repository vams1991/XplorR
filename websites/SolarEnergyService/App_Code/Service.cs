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

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service" in code, svc and config file together.
public class Service : IService
{
	//Method takes latitude and longitude as parameters and gives Solar intensity values as output
     public string getSolarIntensity(decimal latitude, decimal longitude)
	{
        string API_KEY = "i1V91qZsIZKcy1uufdmcAu5bWViwkrpyjwd2Htdo";
        string url = "http://developer.nrel.gov/api/solar/solar_resource/v1.xml?api_key=" + API_KEY + "&lat=" + latitude + "&lon=" + longitude;
        //request to the web service using the url
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        WebResponse response1 = request.GetResponse();
        Stream dataStream = response1.GetResponseStream();
        StreamReader sreader = new StreamReader(dataStream);
        string response2 = sreader.ReadToEnd();
        XmlDocument xdoc= new XmlDocument();
         //loading the web response as an xml
        xdoc.LoadXml(string.Format(response2));
         //parsing the fetched values
        XmlNodeList elemList = xdoc.GetElementsByTagName("annual");
        String output = "";
        int i=0;
         //displaying the reqired values from the parsed values
        foreach (XmlNode node in elemList)
        {

            if (i==0)
                output += "Annual Average Direct Normal Irradiance: " + node.InnerText + "\n";
            if (i == 1)
                output += "Average Global Horizontal Irradiance: " + node.InnerText + "\n";
            if (i == 2)
                output += "Average Tilt at Latitude: " + node.InnerText + "\n";
            i++;

        }

            return output;   
	}
}
