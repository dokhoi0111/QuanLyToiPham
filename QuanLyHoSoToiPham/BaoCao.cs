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

namespace QuanLyHoSoToiPham
{
    public partial class BaoCao : Form
    {
        public BaoCao()
        {
            InitializeComponent();
        }
        SqlConnection conn;
    

        private void BaoCao_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection("Data Source=DESKTOP-KTQ1FE9\\DUCLONG;Initial Catalog=QuanLyToiPham;Integrated Security=True");
            conn.Open();
            UpdateSoLuongToiPham();
        }
        private void UpdateSoLuongToiPham()
        {
            string query = "SELECT COUNT(*) FROM QuanLyHoSoToiPham"; // Truy vấn để đếm số lượng hồ sơ tội phạm.

            SqlCommand command = new SqlCommand(query, conn);
            int count = (int)command.ExecuteScalar(); // Thực thi truy vấn và lấy số lượng.

            btSs.Text = count.ToString(); // Hiển thị số lượng trên TextBox.
        }

   

        private void btnThongKe_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtNam.Text, out int nam))
            {
                string query = @"
            SELECT 
                YEAR(NgayKetThuc) AS Năm, 
                COUNT(*) AS [Số lượng tội phạm], 
                STRING_AGG(MaHoSo, ', ') AS [Danh sách mã hồ sơ tội phạm]
            FROM 
                QuanLyHoSoToiPham 
            WHERE 
                YEAR(NgayKetThuc) = @Nam 
            GROUP BY 
                YEAR(NgayKetThuc)
        ";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@Nam", nam);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable table = new DataTable();
                adapter.Fill(table);

                dataGridView1.DataSource = table;
            }
            else
            {
                MessageBox.Show("Vui lòng nhập một năm hợp lệ.");
            }
        }
    }
}
