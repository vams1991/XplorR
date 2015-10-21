<%@ Page Title="Login Page" Language="C#" MasterPageFile="~/Master1.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    UserName:&nbsp;&nbsp;&nbsp;
<asp:TextBox ID="TextBox1" runat="server" OnTextChanged="TextBox1_TextChanged"></asp:TextBox>
<br />
<br />
ASU ID:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
<asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
<br />
<br />
<asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Submit" />
<br />
<br />
<asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
    <br />
    <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>
</asp:Content>

