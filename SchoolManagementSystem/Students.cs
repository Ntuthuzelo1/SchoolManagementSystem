using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace SchoolManagementSystem
{
    public partial class Students : Form
    {
        public Students()
        {
            InitializeComponent();
            DispalyStudent();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Ntuthuzelo\Documents\SchoolDb.mdf;Integrated Security=True;Connect Timeout=30");
        
        private void DispalyStudent()
        {
            Con.Open();
            string Query = "Select * from StudentTbl";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            StudentDGV.DataSource = ds.Tables[0];
            Con.Close();
        }
        private void Students_Load(object sender, EventArgs e)
        {

        }
        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (StNameTb.Text == "" || FeesTb.Text == "" || AddressTb.Text == "" || StGenCb.SelectedIndex == -1 || ClassCb.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("insert into StudentTbl(StName,StGen,StDOB,StClass,StFees,StAdd) values (@Sname,@SGen,@SDob,@SClass,@SFees,@SAdd)", Con);
                    cmd.Parameters.AddWithValue("@Sname", StNameTb.Text);
                    cmd.Parameters.AddWithValue("@SGen", StGenCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@SDob", DOBPicker.Value.Date);
                    cmd.Parameters.AddWithValue("@SClass", ClassCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@SFees", FeesTb.Text);
                    cmd.Parameters.AddWithValue("@SAdd", AddressTb.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Student Added");
                    Con.Close();
                    DispalyStudent();
                    Reset();

                }
                catch(Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
                
            }

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void Reset()
        {
            Key = 0;
            StNameTb.Text = "";
            FeesTb.Text = "";
            AddressTb.Text = "";
            StGenCb.SelectedIndex = 0;
            ClassCb.SelectedIndex = 0;
        }
        int Key = 0;
        private void StudentDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            StNameTb.Text = StudentDGV.SelectedRows[0].Cells[1].Value.ToString();
            StGenCb.SelectedItem = StudentDGV.SelectedRows[0].Cells[2].Value.ToString();
            DOBPicker.Text = StudentDGV.SelectedRows[0].Cells[3].Value.ToString();
            ClassCb.SelectedItem = StudentDGV.SelectedRows[0].Cells[4].Value.ToString();
            FeesTb.Text = StudentDGV.SelectedRows[0].Cells[5].Value.ToString();
            AddressTb.Text = StudentDGV.SelectedRows[0].Cells[6].Value.ToString();
            if (StNameTb.Text == "")
            {
                Key = 0;
            }else
            {
                Key = Convert.ToInt32(StudentDGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            if(Key == 0)
            {
                MessageBox.Show("Select Student");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("delete from StudentTbl where StId=@StKey",Con);
                    cmd.Parameters.AddWithValue("@StKey", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Student Deleted");
                    Con.Close();
                    DispalyStudent();
                    Reset();
                }catch(Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void StNameTb_TextChanged(object sender, EventArgs e)
        {

        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            if (StNameTb.Text == "" || FeesTb.Text == "" || AddressTb.Text == "" || StGenCb.SelectedIndex == -1 || ClassCb.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("Update StudentTbl set StName=@Sname,StGen=@SGen,StDOB=@SDob,StClass=@SClass,StFees=@SFees,StAdd=@SAdd where StId=@SID", Con);
                    cmd.Parameters.AddWithValue("@Sname", StNameTb.Text);
                    cmd.Parameters.AddWithValue("@SGen", StGenCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@SDob", DOBPicker.Value.Date);
                    cmd.Parameters.AddWithValue("@SClass", ClassCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@SFees", FeesTb.Text);
                    cmd.Parameters.AddWithValue("@SAdd", AddressTb.Text);
                    cmd.Parameters.AddWithValue("@SID", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Student Updated");
                    Con.Close();
                    DispalyStudent();
                    Reset();

                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }

            }
        }

        private void BackBtn_Click(object sender, EventArgs e)
        {
            MainMenu Obj = new MainMenu();
            Obj.Show();
            this.Hide();
        }
    }
}
