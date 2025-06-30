using System.Data;
using System.Data.Odbc;
using System.Text.RegularExpressions;

namespace CrudUsingExcel
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            
        }

        OdbcConnection con;
        OdbcCommand cmd;
        OdbcDataReader dr;
        
        private void Form1_Load(object sender, EventArgs e)
        {
            ConnectPro();
            ShowData();
        }

        private void ConnectPro()
        {
            if (con == null || con.State == ConnectionState.Closed)
            {
                con = new OdbcConnection("DSN=ExcelDSN;ReadOnly=0");
                cmd = new OdbcCommand();
                cmd.Connection = con;
                con.Open();
            }

        }

        private void ShowData()
        {
            if (con.State == ConnectionState.Closed)
            {
                ConnectPro();
            }
            if (dr == null || dr.IsClosed)
            {
                cmd.CommandText = "Select * from [Student$]";
                dr = cmd.ExecuteReader();
            }

            if (dr.Read())
            {
                textBox1.Text = dr["Sno"].ToString();
                textBox2.Text = dr["Sname"].ToString();
                textBox3.Text = dr["Class"].ToString();
                textBox4.Text = dr["Fees"].ToString();
                button1.Text = "Next";
            }
            else
            {
                MessageBox.Show("you are at last record of the table.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                button1.Text = "Refresh";
                con.Close();
                ConnectPro();
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {

            ShowData();
        }


        private void button6_Click_1(object sender, EventArgs e)
        {
            con.Close();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is TextBox)
                {
                    TextBox tb = (TextBox)ctrl;
                    tb.Clear();
                }

            }

            textBox1.Focus();
            button3.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (con.State == ConnectionState.Closed)
            {
                ConnectPro();
            }
            if (!dr.IsClosed)
            {
                dr.Close();
            }
            cmd.CommandText = $"Insert into [Student$] (Sno, Sname, Class, Fees) Values({textBox1.Text},'{textBox2.Text}','{textBox3.Text}',{textBox4.Text})";
            int rows = cmd.ExecuteNonQuery();
            if (rows > 0)
            {
                MessageBox.Show($"Record Inserted Successfully.{rows} rows affected", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Record Insertion Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox1_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Regex regex = new Regex(@"^\d+$");
            if (!regex.IsMatch(textBox1.Text))
            {
                MessageBox.Show("Sno must be a number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                textBox1.Focus();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (con.State == ConnectionState.Closed)
            {
                ConnectPro();
            }
            if (!dr.IsClosed) 
            { 
                dr.Close(); 
            }
            cmd.CommandText = $"Update [Student$] set Sname='{textBox2.Text}', Class='{textBox3.Text}', Fees={textBox4.Text} where Sno={textBox1.Text}";
            int rows = cmd.ExecuteNonQuery();
            if (rows > 0)
            {
                MessageBox.Show($"Record Updated Successfully.{rows} rows affected", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Record Update Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        
    }
}
