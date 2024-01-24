using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
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
            string QoH = "";
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
                QoH = e.NewValues["QuantityOnHand"]?.ToString();

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
        protected void UpdateRow(int WID, string InvCode, string Desc, string QoH, string Reorder)
        {
            bool isNumeric = false;
            int iQoH;
            int iReorder;
            try
            {
                
                _connection.Open();

                //-----Create Command-----
                SqlCommand cmd = new SqlCommand("UpdateWidget", _connection);
                cmd.CommandType = CommandType.StoredProcedure;


                //-----Add parameters-----

                //@WID
                cmd.Parameters.AddWithValue("@WID", WID);

                //@ICode                
                if (InvCode.Length <= 50)  //cant be more than 50 chars long
                {
                    cmd.Parameters.AddWithValue("@ICode", InvCode);
                }
                else
                {
                    Response.Write("Inventory Code cannot be more than 50 characters.");
                    _connection.Close();
                    return;
                }

                //@Desc                
                if (string.IsNullOrEmpty(Desc)) 
                    cmd.Parameters.AddWithValue("@Desc", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@Desc", Desc);

                //@QoH
                isNumeric = int.TryParse(QoH, out iQoH);
                if (isNumeric)
                {
                    cmd.Parameters.AddWithValue("@QoH", iQoH);
                    isNumeric = false;
                }
                else
                {
                    Response.Write("Quantity on Hand must be a number");
                    _connection.Close();
                    return;

                }

                //@ReorderQ
                if (string.IsNullOrEmpty(Reorder))
                    cmd.Parameters.AddWithValue("@ReorderQ", DBNull.Value);
                else
                {
                    isNumeric = int.TryParse(Reorder, out iReorder);
                    if (isNumeric)
                    {
                        cmd.Parameters.AddWithValue("@ReorderQ", (iReorder));
                    }
                    else
                    {
                        Response.Write("Reorder Quantity must be a number");
                        _connection.Close();
                        return;
                    }
                }



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
                WidgetID = WidgetTableGridView.Rows[RowIndex].Cells[2].Text;                

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
                _connection.Close();
            }
        }

        /// <summary>
        /// Show the Modal Window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCreate_Click(object sender, EventArgs e)
        {
            ModalWindow.Style["display"] = "block";
        }

        /// <summary>
        /// Insert a row into SQL using stored procedure "NewWidget"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnInsert_Click(object sender, EventArgs e)
        {

            string InvCode = txtInvCode.Text;
            string Desc = txtDescription.Text;
            string QoH = (txtQuantity.Text);
            int iQoH;
            string Reorder = txtReorder.Text;
            int iReorder;
            bool isNumeric = false;


            try
            {
                //Format:
                //Exec NewWidget @ICode='adsf', @Desc='fdaddd', @QoH=23, @ReorderQ=45

                _connection.Open();

                //-----Create Command-----
                SqlCommand cmd = new SqlCommand("NewWidget", _connection);
                cmd.CommandType = CommandType.StoredProcedure;


                //-----Add parameters-----

                //@ICode
                if (InvCode.Length <= 50 )  //cant be more than 50 chars long
                {
                    cmd.Parameters.AddWithValue("@ICode", InvCode);
                }
                else
                {
                    Response.Write("Inventory Code cannot be more than 50 characters.");
                    _connection.Close();
                    return;
                }
                

                //@Desc                
                if (string.IsNullOrEmpty(Desc))
                    cmd.Parameters.AddWithValue("@Desc", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@Desc", Desc);

                //@QoH
                isNumeric = int.TryParse(QoH, out iQoH);
                if (isNumeric)
                {
                    cmd.Parameters.AddWithValue("@QoH", iQoH);
                    isNumeric = false;
                }
                else
                {
                    Response.Write("Quantity on Hand must be a number");
                    _connection.Close();
                    return;
                }
            

                //@ReorderQ
                if (string.IsNullOrEmpty(Reorder))
                    cmd.Parameters.AddWithValue("@ReorderQ", DBNull.Value);
                else
                {                    
                    isNumeric = int.TryParse(Reorder, out iReorder);
                    if (isNumeric)
                    {
                        cmd.Parameters.AddWithValue("@ReorderQ", (iReorder));
                    }
                    else
                    {
                        Response.Write("Reorder Quantity must be a number");
                        _connection.Close();
                        return;
                    }
                }

                cmd.ExecuteNonQuery();
                _connection.Close();

                //Clear Fields
                txtInvCode.Text = "";
                txtDescription.Text = "";
                txtQuantity.Text = "";
                txtReorder.Text = "";

                //close window
                ModalWindow.Style["display"] = "none";

                //Refresh Table               
                LoadTable();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
                _connection.Close();
            }
        }

        /// <summary>
        /// Exit out of modal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //close window
            ModalWindow.Style["display"] = "none";
        }
    }
}