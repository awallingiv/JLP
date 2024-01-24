<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="JLP._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <header>        
            <style type="text/css">            
            .widget-table-grid-view > tbody > tr > th, 
            .widget-table-grid-view > tbody > tr > td { 
                border: 3px ridge black; 
                padding: 5px; 
                text-align: center;                
            }            
            .modal {
                display: none;
                padding-top: 5px;                
                margin:auto;                
                border: 2px;
                border-radius: 5px;
                padding: 10px;
                background-color: rgb(128 128 128);
                position:fixed;
                top:50%;
                left:50%;
                height:50%;
                width:50%;
                transform: translate(-50%, -50%);
                z-index: 1000;
            }
            .modal-header{
                text-align: center;
                width: 100%;
            }
            .modal-footer{
                text-align: center;
                width: 100%;
            }            
            .field-row label{
                text-align: right;
            }
            .field-row input {
                width: 100%;
            }
            .field-row {
                /*display:grid;
                grid-template-columns: 1fr 2fr;                
                gap: 10px;*/
                display:flex;
                flex-direction:column;
                align-items:center;
                margin-bottom: 10px;
            }
            .cancel-button{
                display:table-row;
                align-content:flex-start;
                align-items:flex-end;
            }
            </style>
    </header>

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

        <!--Table-->
        <div class="row">            
            <center>                      
            <asp:GridView ID="WidgetTableGridView" runat="server" AutoGenerateColumns="true" CssClass="widget-table-grid-view" OnRowEditing="WidgetTableGridView_RowEditing" OnRowDeleting="WidgetTableGridView_RowDeleting" OnRowUpdating="WidgetTableGridView_RowUpdating" OnRowCancelingEdit="WidgetTableGridView_RowCancelingEdit" DataKeyNames="WidgetID" GridLines="both">
                <Columns>                    
                    <asp:CommandField ShowEditButton="true" />                    
                    <asp:CommandField ShowDeleteButton="true" />
                </Columns>
            </asp:GridView>                
            </center>
        </div>
        <!--/Table-->
        
        <!--Spacer-->
        <div style="height: 50px;"></div>
        <!--/Spacer-->

        <!--Create Button-->       
        <div class="row">
             
            <center>
                <asp:Button ID="btnCreate" runat="server" Text="New Row" OnClick ="btnCreate_Click" />
                <asp:Panel ID ="ModalWindow" runat="server" CssClass="modal">
                    <div class="modal-header"  style="display: block;">
                        <h4>Insert Row into Widget Table</h4>
                    </div>
                    <div class="modal-body">
                        <div class="field-row">
                            <asp:Label ID="lblInvCode" Text="Inventory Code:" runat="server"/>
                            <asp:TextBox ID = "txtInvCode" runat="server" />
                        </div>
                        <div class="field-row">
                            <asp:Label ID="lblDescription" Text="Description:   " runat="server"/>
                            <asp:TextBox ID = "txtDescription" runat="server" />
                        </div>
                        <div class="field-row">
                            <asp:Label ID="lblQuantity" Text="Quantity on Hand:" runat="server"/>
                            <asp:TextBox ID = "txtQuantity" runat="server" />
                        </div>                        
                        <div class="field-row">
                            <asp:Label ID="lblReorder" Text="Reorder Quantity:" runat="server"/>
                            <asp:TextBox ID = "txtReorder" runat="server" />
                        </div>                                                   
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="Cancel" runat="server" Text="Cancel" CssClass="cancel-button" OnClick="btnCancel_Click"/>
                        <asp:Button ID="btnInsert" runat="server" Text="Insert" OnClick="btnInsert_Click"/>                            
                    </div>
                </asp:Panel>                    
            </center>
        </div>
        <!--/Create Button-->

        <!--Spacer-->
        <div style="height: 240px;"></div>
        <!--/Spacer-->
    </main>

</asp:Content>
