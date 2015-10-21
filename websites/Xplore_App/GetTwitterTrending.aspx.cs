using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Xml;
public partial class GetTwitterTrending : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HttpCookie myCookies = Request.Cookies["myCookieId"];
        if ((myCookies == null) || (myCookies["Name"] == ""))
        {
            Label1.Text = "Welcome, new user";
        }
        else
        {
            Label1.Text = "Welcome, " + myCookies["Name"];
            Label2.Text = "ASU ID: " + myCookies["ASU ID"];
        }
        if (Cache["CacheItem4"] != null)
        {
            tb_TrendingData.Text = "Data retrieved from Cache" + "\n" + Cache["CacheItem4"].ToString();
        }

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        // Sending web request to our web service
        WebRequest req = WebRequest.Create("http://localhost:1337/ElecServices.svc/trending/place=" + PlaceName.Text);
        WebResponse resp = req.GetResponse();

        // Reading and parsing the response data
        Stream dataStream = resp.GetResponseStream();
        StreamReader sReader = new StreamReader(dataStream);
        string responseFromServer = sReader.ReadToEnd();
        resp.Close();

        // Loading the string response as an XML
        XmlDocument xmldoc = new XmlDocument();
        xmldoc.LoadXml(responseFromServer);

        // Displaying our fetched values
        tb_TrendingData.Text = "Data - Trending Topics: \n";

        XmlNodeList nodes = xmldoc.GetElementsByTagName("Name");
        foreach (XmlNode n in nodes)
        {
            tb_TrendingData.Text += n.InnerText + "\n";
        }
        //tb_TrendingData.Text += "\nRaw Data:\n";
        //tb_TrendingData.Text += responseFromServer;
        String str = "PlaceName: " + PlaceName.Text + "\n" + tb_TrendingData.Text;
        Cache.Insert("CacheItem4", str, null, DateTime.Now.AddMinutes(10), TimeSpan.Zero);
    }
}