using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JLP
{
    public partial class _Default : Page
    {
        //global connection 
        SqlConnection _connection;

        /// <summary>
        /// Page Load - set SQL connection. In this case connection is local
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Connection String (Which we would normally not hard-code) - amw
            Console.WriteLine("Setting Connection");
            _connection = new SqlConnection();
            _connection.ConnectionString = "Server=DESKTOP-29433VS\\SQLEXPRESS;Database=DotNetDevSample;User Id=JivaTester;Password=JivaTest;Trusted_Connection=true";

            if (!IsPostBack)
            {
                LoadTable();
            }            
        }

        /// <summary>
        /// Calls ReadTable procedure which selects all, ordered by WidgetID
        /// Binds to Table
        /// </summary>
        protected void LoadTable()
        {
            try
            {   //-----open connection-----
                _connection.Open();

                //Execute SQL Command and Bind to Table
                //SqlCommand cmd = new SqlCommand("EXEC ReadTable", _connection);
                //opting for commandtype.storedprocedure instead
                SqlCommand cmd = new SqlCommand("ReadTable", _connection);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable WidgetTable = new DataTable();
                WidgetTable.Load(reader);
                WidgetTableGridView.DataSource = WidgetTable;
                WidgetTableGridView.DataBind();


                //-----Close Connection-----
                _connection.Close();
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
                //Console.WriteLine(ex.Message);
                _connection.Close();
            }
        }

        /// <summary>
        /// For Editing a Row, brings up editable cells
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void WidgetTableGridView_RowEditing(object sender, GridViewEditEventArgs e)
        {            
            WidgetTableGridView.EditIndex = e.NewEditIndex;

            // Reload Table
            LoadTable();

        }
        /// <summary>
        /// For Canceling out of edit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void WidgetTableGridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            WidgetTableGridView.EditIndex = -1;
            LoadTable();
        }

        /// <summary>
        /// Once the user makes modifications, check data and push to SQL using stored procedure
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void WidgetTableGridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int WidgetID = 0;
            string InvCode = "";
            string Desc = "";
            int QoH = 0;
            string ReorderQuantity = "";    //will convert later

            try
            {
                //-----Widget ID-----
                WidgetID = Convert.ToInt32(WidgetTableGridView.DataKeys[e.RowIndex].Value);

                //-----Inventory Code-----
                //TempCellValue = WidgetTableGridView.Rows[e.RowIndex].Cells[3].Controls[0] as TextBox;
                //InvCode = TempCellValue.Text;
                InvCode = e.NewValues["InventoryCode"]?.ToString();

                //-----Description-----
                //TextBox TempCellValue = WidgetTableGridView.Rows[e.RowIndex].Cells[4].Controls[0] as TextBox;
                //Desc = TempCellValue.Text;
                Desc = e.NewValues["Description"]?.ToString();

                //-----Quantity On Hand-----
                //TempCellValue = WidgetTableGridView.Rows[e.RowIndex].Cells[5].Controls[0] as TextBox;
                //QoH = Convert.ToInt32(TempCellValue.Text);
                QoH = Convert.ToInt32(e.NewValues["QuantityOnHand"]?.ToString());

                //-----Reorder Quantity-----
                //TempCellValue = WidgetTableGridView.Rows[e.RowIndex].Cells[6].Controls[0] as TextBox;
                //ReorderQuantity = (TempCellValue.Text);
                ReorderQuantity = e.NewValues["ReorderQuantity"]?.ToString();


                //-----Update Row-----
                UpdateRow(WidgetID, InvCode, Desc, QoH, ReorderQuantity);

                //-----Reset EdtIndex-----
                WidgetTableGridView.EditIndex = -1;

                //-----Reload Table-----
                LoadTable();

            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }

            
        }

        /// <summary>
        /// Perform some data validation and then update/push row to table
        /// 
        /// EXEC UpdateWidget @WID = 6, @ICode = 'InvCode6', @Desc = 'Desc6', @QoH = 78, @ReorderQ = 77
        /// </summary>
        protected void UpdateRow(int wid, string invcode, string desc, int qoh, string reorder)
        {

            try
            {
                if (wid == 0)
                {
                    Response.Write("Invalid Widget ID");
                    return;
                }

                _connection.Open();

                //-----Create Command-----
                SqlCommand cmd = new SqlCommand("UpdateWidget", _connection);
                cmd.CommandType = CommandType.StoredProcedure;


                //-----Add parameters-----

                //@WID
                cmd.Parameters.AddWithValue("@WID", wid);

                //@ICode                
                cmd.Parameters.AddWithValue("@ICode", invcode);

                //@Desc                
                if (string.IsNullOrEmpty(invcode)) 
                    cmd.Parameters.AddWithValue("@Desc", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@Desc", desc);

                //@QoH                
                cmd.Parameters.AddWithValue("@QoH", qoh);

                //@ReorderQ
                if (string.IsNullOrEmpty(reorder))
                    cmd.Parameters.AddWithValue("@ReorderQ", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@ReorderQ", Convert.ToInt32(reorder));

                cmd.ExecuteNonQuery();
                _connection.Close();

                
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
                _connection.Close();
            }

        }

        /// <summary>
        /// Delete from table using stored SQL Procedure for deletion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void WidgetTableGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int RowIndex = e.RowIndex;  //grab row index
            string WidgetID;            //integer not needed       


            try
            {
                //Grab widget ID from 4th column
                WidgetID = WidgetTableGridView.Rows[RowIndex].Cells[3].Text;                

                //open connection
                _connection.Open();

                //Execute SQL Command and Bind to Table
                SqlCommand cmd = new SqlCommand($"EXEC DeleteWidget @WID = {WidgetID}", _connection);
                SqlDataReader reader = cmd.ExecuteReader();         
                             
                //Close Connection
                _connection.Close();

                //Refresh Table               
                LoadTable();
                
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
                //Console.WriteLine(ex.Message);
                _connection.Close();
            }
        }

        

       

    }
}