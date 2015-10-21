using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using System.Web.Caching;

public partial class GetSolarEnergy : System.Web.UI.Page
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
        if (Cache["CacheItem"] != null)
        {
            TextArea1.InnerText = "Data retrieved from Cache"+"\n"+Cache["CacheItem"].ToString();
        }
       

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
       
    }
protected void Button1_Click1(object sender, EventArgs e)
{
        decimal latitude = Convert.ToDecimal(TextBox1.Text);
        decimal longitude = Convert.ToDecimal(TextBox2.Text);
        ServiceReference1.ServiceClient serviceref = new ServiceReference1.ServiceClient();
        TextArea1.InnerText = serviceref.getSolarIntensity(latitude, longitude);
        String str="latitude: "+latitude.ToString()+"\n"+"longitude:"+longitude.ToString()+"\n"+serviceref.getSolarIntensity(latitude, longitude);
        Cache.Insert("CacheItem",str,null,DateTime.Now.AddMinutes(10), TimeSpan.Zero);
        

}
}

    
