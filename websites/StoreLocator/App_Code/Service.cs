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
    //method takes place or zip and storename as parameters and returns the store addresses
    public string storeLocate(string placeorzip, string storename)
    {

        int version = 20130815;
        string CLIENT_ID = "AQHJXGJUFQMK2SKU4SMHIJ2ZDSHQEFPPH4QCI3VTLE5ZU50C", CLIENT_SECRET = "DMAFU5COI3G2VJM2QQ2HMPV4UHFRUGDTPHNK3O3NU2TEFMR0";
        string url = "https://api.foursquare.com/v2/venues/search?client_id=" + CLIENT_ID + "&client_secret=" + CLIENT_SECRET + "&v=" + version + "&near=" + placeorzip + "&query=" + storename;
        ////request to the web service using the url
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        WebResponse response1 = request.GetResponse();
        Stream dataStream = response1.GetResponseStream();
        StreamReader sreader = new StreamReader(dataStream);
        string response2 = sreader.ReadToEnd();
        ////loading the web response as an xml
        XmlDocument xdoc = ConvertJsonToXML(response2);
        //parsing the fetched values
        XmlNodeList elemList = xdoc.GetElementsByTagName("address");
        String output = "";
        //diaplaying the parsed values
        foreach (XmlNode node in elemList)
        {
            output += "address: "+node.OuterXml;
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
        








       

