using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Xml;

public partial class GetWindEnergy : System.Web.UI.Page
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
        if (Cache["CacheItem3"] != null)
        {
            tb_WindData.Text = "Data retrieved from Cache" + "\n" + Cache["CacheItem3"].ToString();
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        // Sending request to our web service
        WebRequest req = WebRequest.Create("http://localhost:1337/ReqServices.svc/winddata/zipcode=" + ZipCode.Text);
        WebResponse resp = req.GetResponse();

        // Reading and parsing the response data
        Stream dataStream = resp.GetResponseStream();
        StreamReader sReader = new StreamReader(dataStream);
        string responseFromServer = sReader.ReadToEnd();
        resp.Close();

        // Loading the string response as an XML
        XmlDocument xmldoc = new XmlDocument();
        xmldoc.LoadXml(responseFromServer);

        // Displaying fetched values
        tb_WindData.Text = "Data(in Miles per hour): \n";
        tb_WindData.Text += "Annual Wind Speed: " + xmldoc.GetElementsByTagName("annualWindSpeed")[0].InnerText + "\n";
        tb_WindData.Text += "January Wind Speed: " + xmldoc.GetElementsByTagName("janWindSpeed")[0].InnerText + "\n";
        tb_WindData.Text += "February Wind Speed: " + xmldoc.GetElementsByTagName("febWindSpeed")[0].InnerText + "\n";
        tb_WindData.Text += "March Wind Speed: " + xmldoc.GetElementsByTagName("marWindSpeed")[0].InnerText + "\n";
        tb_WindData.Text += "April Wind Speed: " + xmldoc.GetElementsByTagName("aprWindSpeed")[0].InnerText + "\n";
        tb_WindData.Text += "May Wind Speed: " + xmldoc.GetElementsByTagName("mayWindSpeed")[0].InnerText + "\n";
        tb_WindData.Text += "June Wind Speed: " + xmldoc.GetElementsByTagName("junWindSpeed")[0].InnerText + "\n";
        tb_WindData.Text += "July Wind Speed: " + xmldoc.GetElementsByTagName("julWindSpeed")[0].InnerText + "\n";
        tb_WindData.Text += "August Wind Speed: " + xmldoc.GetElementsByTagName("augWindSpeed")[0].InnerText + "\n";
        tb_WindData.Text += "September Wind Speed: " + xmldoc.GetElementsByTagName("sepWindSpeed")[0].InnerText + "\n\n";
        tb_WindData.Text += "October Wind Speed: " + xmldoc.GetElementsByTagName("octWindSpeed")[0].InnerText + "\n\n";
        tb_WindData.Text += "November Wind Speed: " + xmldoc.GetElementsByTagName("novWindSpeed")[0].InnerText + "\n\n";
        tb_WindData.Text += "Decemeber Wind Speed: " + xmldoc.GetElementsByTagName("decWindSpeed")[0].InnerText + "\n\n";

        tb_WindData.Text += "Raw XML Data: \n";
        tb_WindData.Text += responseFromServer;
        String str = "ZipCode: " + ZipCode.Text + "\n" + tb_WindData.Text;
        Cache.Insert("CacheItem3", str, null, DateTime.Now.AddMinutes(10), TimeSpan.Zero);
    }
}