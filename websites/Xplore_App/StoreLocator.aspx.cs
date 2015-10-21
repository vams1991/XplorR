using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class StoreLocator : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HttpCookie myCookies = Request.Cookies["myCookieId"];
        if ((myCookies == null) || (myCookies["Name"] == ""))
        {
            Label2.Text = "Welcome, new user";
        }
        else
        {
            Label2.Text = "Welcome, " + myCookies["Name"];
            Label3.Text = "ASU ID: " + myCookies["ASU ID"];
        }
        if (Cache["CacheItem2"] != null)
        {
            Label1.Text = "Data retrieved from Cache" + "\n" + Cache["CacheItem2"].ToString();
        }

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        string placeorzip1 = TextBox1.Text;
        string storename1 = TextBox2.Text;
        ServiceReference2.ServiceClient myservice = new ServiceReference2.ServiceClient();
        Label1.Text = myservice.storeLocate(placeorzip1, storename1);
        String str = "place or zip: " + placeorzip1 + "\n" + " store:" + storename1 + "\n" + myservice.storeLocate(placeorzip1, storename1);
        Cache.Insert("CacheItem2", str, null, DateTime.Now.AddMinutes(10), TimeSpan.Zero);
    }
}