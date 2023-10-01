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
    public partial class Events : Form
    {
        public Events()
        {
            InitializeComponent();
            DispalyEvents();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Ntuthuzelo\Documents\SchoolDb.mdf;Integrated Security=True;Connect Timeout=30");

        private void DispalyEvents()
        {
            Con.Open();
            string Query = "Select * from EventsTbl";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            EventsDGV.DataSource = ds.Tables[0];
            Con.Close();
        }
        private void Reset()
        {
            EdurationTb.Text = "";
            EDescTb.Text = "";
        }
        private void AddBtn_Click(object sender, EventArgs e)
        {
            if (EDescTb.Text == "" || EdurationTb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("insert into EventsTbl(EDesc,EDate,EDuration) values (@EvDesc,@EvDate,@EvDur)", Con);
                    cmd.Parameters.AddWithValue("@EvDesc", EDescTb.Text);
                    cmd.Parameters.AddWithValue("@EvDate", EDate.Value.Date);
                    cmd.Parameters.AddWithValue("@EvDur", EdurationTb.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Event Added");
                    Con.Close();
                    DispalyEvents();
                    Reset();

                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }

            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BAckBtn_Click(object sender, EventArgs e)
        {
            MainMenu Obj = new MainMenu();
            Obj.Show();
            this.Hide();
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            if (Key == 0)
            {
                MessageBox.Show("Select Event");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("delete from EventsTbl where EId=@EKey", Con);
                    cmd.Parameters.AddWithValue("@EKey", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Event Deleted");
                    Con.Close();
                    DispalyEvents();
                    Reset();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }
        int Key = 0;
        private void EventsDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            EDescTb.Text = EventsDGV.SelectedRows[0].Cells[1].Value.ToString();
            EDate.Text = EventsDGV.SelectedRows[0].Cells[2].Value.ToString();
            EdurationTb.Text = EventsDGV.SelectedRows[0].Cells[3].Value.ToString();
            if (EDescTb.Text == "")
            {
                Key = 0;
            }
            else
            {
                Key = Convert.ToInt32(EventsDGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            if (EDescTb.Text == "" || EdurationTb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("Update EventsTbl set EDesc=@EvDesc,EDate=@EvDate,EDuration=@EvDuration where EId=@EvID", Con);
                    cmd.Parameters.AddWithValue("@EvDesc", EDescTb.Text);
                    cmd.Parameters.AddWithValue("@EvDate", EDate.Value.Date);
                    cmd.Parameters.AddWithValue("@EvDuration", EdurationTb.Text);
                    cmd.Parameters.AddWithValue("@EvID", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Event Updated");
                    Con.Close();
                    DispalyEvents();
                    Reset();

                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }

            }
        }

        private void Events_Load(object sender, EventArgs e)
        {

        }
    }
}
