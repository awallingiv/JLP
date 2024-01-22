<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="JLP._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <!--Title-->
        <section class="row" aria-labelledby="aspnetTitle">
            <Center><h1 id="aspnetTitle">Skill Assessment</h1></Center>
            <center><p class="lead">Landing Page for .Net Position at Jivasoft/Extra Duty Solutions</p></center>            
        </section>
        <!--/Title-->

        <!--Spacer-->
        <div style="height: 120px;"></div>
        <!--/Spacer-->

        <!--Table Title-->
        <div class="row">
            <center><h2>Widget Table</h2></center>
        </div>
        <!--/Table Title-->

        <!--Table Description-->
        <div class="row">
            <center><h5>Widget Table from DotNetDevSample Database</h5></center>
        </div>
        <!--/Table Description-->

        <!--Create Button-->
        <div class="row">
            <center><h5></h5></center>
        </div>
        <!--/Create Button-->

        <!--Table-->
        <div class="row">
            <!-- Widget Table Grid View CSS -->
            <style type="text/css">            
            .WidgetTableGridView > tbody > tr > th, 
            .WidgetTableGridView > tbody > tr > td { border: 3px ridge black; padding: 5px; text-align: center }            
            </style>
            <!-- /Widget Table Grid View CSS -->
           
            <center>                      
            <asp:GridView ID="WidgetTableGridView" runat="server" AutoGenerateColumns="true" CssClass="WidgetTableGridView" OnRowEditing="WidgetTableGridView_RowEditing" OnRowDeleting="WidgetTableGridView_RowDeleting" OnRowUpdating="WidgetTableGridView_RowUpdating" OnRowCancelingEdit="WidgetTableGridView_RowCancelingEdit" DataKeyNames="WidgetID" GridLines="both">
                <Columns>                    
                    <asp:CommandField ShowEditButton="true" />                    
                    <asp:CommandField ShowDeleteButton="true" />
                </Columns>
            </asp:GridView>                
            </center>
        </div>
        <!--/Table-->

        <!--Spacer-->
        <div style="height: 240px;"></div>
        <!--/Spacer-->
    </main>

</asp:Content>
