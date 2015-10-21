using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HttpCookie myCookies = Request.Cookies["myCookieId"];
        if((myCookies==null)||(myCookies["Name"]==""))
        {
            Label1.Text = "Welcome, new user";
        }
        else
        {
            Label1.Text = "Welcome, " + myCookies["Name"] ;
            Label2.Text = "ASU ID: " + myCookies["ASU ID"];
        }
    }
    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        HttpCookie myCookies = new HttpCookie("myCookieId");
        myCookies["Name"] = TextBox1.Text;
        myCookies["ASU ID"] = TextBox2.Text;
        myCookies.Expires = DateTime.Now.AddYears(2);
        Response.Cookies.Add(myCookies);
        Label1.Text = "Name Stored in Cookies: " + myCookies["Name"];
        Label2.Text = "ASU ID stored in Cookies: " + myCookies["ASU ID"];
    }
}