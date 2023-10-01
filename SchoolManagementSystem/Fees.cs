using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SchoolManagementSystem
{
    public partial class Fees : Form
    {
        public Fees()
        {
            InitializeComponent();
            DisplayFees();
            FillStudId();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Ntuthuzelo\Documents\SchoolDb.mdf;Integrated Security=True;Connect Timeout=30");

        private void DisplayFees()
        {
            Con.Open();
            string Query = "Select * from FeesTbl";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            FeesDGV.DataSource = ds.Tables[0];
            Con.Close();
        }
        private void Students_Load(object sender, EventArgs e)
        {

        }
        private void FillStudId()
        {
            Con.Open();
            SqlCommand cmd = new SqlCommand("select StId from StudentTbl", Con);
            SqlDataReader rdr;
            rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("StId", typeof(int));
            dt.Load(rdr);
            StdIdCb.ValueMember = "StId";
            StdIdCb.DataSource = dt;
            Con.Close();
        }
        private void GetStudName()
        {
            Con.Open();
            SqlCommand Cmd = new SqlCommand("select * from StudentTbl where StId=@SID", Con);
            Cmd.Parameters.AddWithValue("@SID", StdIdCb.SelectedValue.ToString());
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(Cmd);
            sda.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                StNameTb.Text = dr["StName"].ToString();
            }
            Con.Close();
        }
        private void Reset()
        {
            AmountTb.Text = "";
            StNameTb.Text = "";
            StdIdCb.SelectedIndex = -1;
        }
        private void AddBtn_Click(object sender, EventArgs e)
        {
            if (StNameTb.Text == "" || AmountTb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                string paymentperiode;
                paymentperiode = PeriodDate.Value.Month.ToString() +"/"+ PeriodDate.Value.Year.ToString();
                Con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("select COUNT(*) from FeesTbl where StId = '" + StdIdCb.SelectedValue.ToString() + "' and Month = '" + paymentperiode.ToString() + "'", Con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows[0][0].ToString() == "1")
                {
                    MessageBox.Show("There is no due for this Month");
                }
                else
                {
                    //Con.Open();
                    SqlCommand cmd = new SqlCommand("insert into FeesTbl(StId,StName,Month,Amt) values (@SId,@SName,@SMonth,@SAmt)", Con);
                    cmd.Parameters.AddWithValue("@SId", StdIdCb.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@SName", StNameTb.Text);
                    cmd.Parameters.AddWithValue("@SMonth", paymentperiode);
                    cmd.Parameters.AddWithValue("@SAmt", AmountTb.Text);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Fees Successfully Paid");
                }
                Con.Close();
                DisplayFees();
                Reset();
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void StdIdCb_SelectionChangeCommitted(object sender, EventArgs e)
        {
            GetStudName();
        }

        private void BackBtn_Click(object sender, EventArgs e)
        {
            MainMenu Obj = new MainMenu();
            Obj.Show();
            this.Hide();
        }
    }
}
