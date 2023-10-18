using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace CRUDAppPostgresql
{
    public partial class Form1 : Form
    {
        private NpgsqlConnection conn;
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)//  save
        {
            try
            {
                //create
                string commandText = "INSERT INTO t_siswa (nis,nama_siswa,alamat,handphone) " +
                    "VALUES (@nis, @nama_siswa, @alamat, @handphone)";
                using (var cmd = new NpgsqlCommand(commandText, conn))
                {
                    cmd.Parameters.AddWithValue("nis", txtNIS.Text);
                    cmd.Parameters.AddWithValue("nama_siswa", txtName.Text);
                    cmd.Parameters.AddWithValue("alamat", txtAlamat.Text);
                    cmd.Parameters.AddWithValue("handphone", txtHp.Text);

                    await cmd.ExecuteNonQueryAsync();
                    await getData();

                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //connection to postgressql
            NpgsqlBoardGameRepository npb = new NpgsqlBoardGameRepository();
            conn = npb.connection;
            getData();
        }

        private async Task getData()
        {
            try // view table
            { 
                DGV1.ColumnCount = 4;
                DGV1.Columns[0].Name = "NIS";
                DGV1.Columns[1].Name = "Nama Siswa";
                DGV1.Columns[2].Name = "Alamat";
                DGV1.Columns[3].Name = "Handphone";
                DGV1.Rows.Clear();
                string commandText = "SELECT * FROM t_siswa";
                using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, conn))
                {
                    using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                string[] rows = new string[] { reader["nis"].ToString(), reader["nama_siswa"].ToString(), reader["alamat"].ToString(), reader["handphone"].ToString() };
                                DGV1.Rows.Add(rows); //read tabel
                            }
                        }
                    }
                }
            } catch (Exception e)
            {
                MessageBox.Show("Error "+ e.Message);//message
            }
        }

        private async void button2_Click(object sender, EventArgs e) // update
        {
            try
            {
                var commandText = @"UPDATE t_siswa
                SET nama_siswa = @nama_siswa, alamat = @alamat, handphone = @handphone
                WHERE nis = @nis";

                using (var cmd = new NpgsqlCommand(commandText, conn))//new create
                {
                    cmd.Parameters.AddWithValue("nama_siswa", txtName.Text);
                    cmd.Parameters.AddWithValue("alamat", txtAlamat.Text);
                    cmd.Parameters.AddWithValue("handphone", txtHp.Text);
                    cmd.Parameters.AddWithValue("nis", txtNIS.Text);

                    await cmd.ExecuteNonQueryAsync();
                    await getData();
                }//--
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex.Message);
            }
        }

        private async void button3_Click(object sender, EventArgs e) // delete
        {
            try
            {

                string commandText = "DELETE FROM t_siswa WHERE nis=@nis";
                using (var cmd = new NpgsqlCommand(commandText, conn))
                {
                    cmd.Parameters.AddWithValue("nis", txtNIS.Text);
                    await cmd.ExecuteNonQueryAsync();
                    await getData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex.Message);
            }
        }

        private void DGV1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;// get the Row Index
            DataGridViewRow selectedRow = DGV1.Rows[index];
            txtNIS.Text = selectedRow.Cells[0].Value.ToString();
            txtName.Text = selectedRow.Cells[1].Value.ToString();
            txtAlamat.Text = selectedRow.Cells[2].Value.ToString();
            txtHp.Text = selectedRow.Cells[3].Value.ToString();
        }

        private void btnclear_Click(object sender, EventArgs e)
        {
            txtNIS.Text = "";
            txtName.Text = "";
            txtHp.Text = "";
            txtAlamat.Text = "";
          
        }

        
    }
}
//storeprocedure