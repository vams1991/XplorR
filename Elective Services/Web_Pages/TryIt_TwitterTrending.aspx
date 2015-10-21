<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TryIt_TwitterTrending.aspx.cs" Inherits="ElectiveServices.Web_Pages.TryIt_TwitterTrending" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Smart Coders | TryIt Page | Twitter Trending Service</title>
    <link type="text/css" rel="stylesheet" href= "TryIt.css" />
</head>
<body>
    <div class="page">
        <div class="header">
            <div class="title">
                <h1>
                    Smart Coders - ASP.NET Application
                </h1>
            </div>
        </div>
        <div class="main">
            <form id="form1" runat="server">
                <h2>Twitter Trending Service</h2>
                <p>Fetches the trending topic on Twitter using place name</p>
                <p>Web Service URL : <a href="http://localhost:1337/ElecServices.svc/trending/place=phoenix">http://localhost:1337/ElecServices.svc/trending/place=Phoenix</a><br />
                    NOTE: For demo purpose, Place name has been added to the URL. Replace place name with name of target location.</p>
                <p>Method Name: FetchTrending<br />
                    Input Param: Place Name - As part of the invoking URL</p>
                <p>Place Name:&nbsp;
                    <asp:TextBox ID="PlaceName" runat="server"></asp:TextBox>
&nbsp;</p>
                <p>
                    <asp:Button ID="TrendingButton" runat="server" OnClick="TrendingButton_Click" Text="Fetch Data" Width="191px" />
                </p>
                <p>
                    <asp:Label ID="Label1" runat="server" Text="Fetched Trending Data:"></asp:Label>
                </p>
                <p>
                    <asp:TextBox ID="tb_TrendingData" runat="server" Enabled="False" Height="217px" TextMode="MultiLine" Width="914px"></asp:TextBox>
                </p>
            </form>
        </div>
        <div class="clear"></div>
    </div>
</body>
</html>
