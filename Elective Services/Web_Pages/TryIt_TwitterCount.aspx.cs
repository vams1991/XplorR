using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Xml;
using System.IO;

namespace ElectiveServices.Web_Pages
{
    public partial class TryIt_TwitterCount : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void CountButton_Click(object sender, EventArgs e)
        {
            // Sending web request to our web service
            if (PlaceName.Text == "" || PlaceName.Text == null)
                PlaceName.Text = "Phoenix";
            WebRequest req = WebRequest.Create("http://localhost:1337/ElecServices.svc/count/q=" + TopicName.Text + "," + PlaceName.Text);
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
            tb_CountData.Text = "Data: \nTweet Count - ";

            XmlNodeList nodes = xmldoc.GetElementsByTagName("Count");
            foreach (XmlNode n in nodes)
            {
                tb_CountData.Text += n.InnerText + "\n";
            }
            tb_CountData.Text += "\nRaw Data:\n";
            tb_CountData.Text += responseFromServer;
        }
    }
}