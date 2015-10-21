using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Population : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Cache["CacheItem7"] != null)
        {
            TextArea1.InnerText = "Data retrieved from Cache" + "\n" + Cache["CacheItem7"].ToString();
        }
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

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        string placeName = TextBox1.Text;
        ServiceReference4.Service1Client proxy = new ServiceReference4.Service1Client();
        TextArea1.InnerText = proxy.GetPopulation(placeName);
        String str = "Place Name: " + placeName + "\n" + TextArea1.InnerText;
        Cache.Insert("CacheItem7", str, null, DateTime.Now.AddMinutes(10), TimeSpan.Zero);
    }
}