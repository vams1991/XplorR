<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TryIt_TwitterCount.aspx.cs" Inherits="ElectiveServices.Web_Pages.TryIt_TwitterCount" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Smart Coders | TryIt Page | Tweet Counter Service</title>
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
                <h2>Twitter Tweet Count Service</h2>
                <p>Fetches the count of tweets from Twitter using topic name</p>
                <p>Web Service URL : <a href="http://localhost:1337/ElecServices.svc/count/count/q=ASU,Tempe">http://localhost:1337/ElecServices.svc/count/count/q=ASU,Tempe</a><br />
                    NOTE: For demo purpose, Topic name has been added to the URL. Replace topic name with name of target location.</p>
                <p>Method Name: FetchCount<br />
                    Input Param: Topic Name & Place Name - As part of the invoking URL</p>
                <p>Topic Name:&nbsp;
                    <asp:TextBox ID="TopicName" runat="server"></asp:TextBox>
&nbsp;</p>
                <p>Place Name:
                    <asp:TextBox ID="PlaceName" runat="server"></asp:TextBox>
                </p>
                <p>
                    <asp:Button ID="CountButton" runat="server" OnClick="CountButton_Click" Text="Fetch Data" Width="191px" />
                </p>
                <p>
                    <asp:Label ID="Label1" runat="server" Text="Fetched Tweet Data:"></asp:Label>
                </p>
                <p>
                    <asp:TextBox ID="tb_CountData" runat="server" Enabled="False" Height="217px" TextMode="MultiLine" Width="914px"></asp:TextBox>
                </p>
            </form>
        </div>
        <div class="clear"></div>
    </div>
</body>
</html>
