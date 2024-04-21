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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QuanLyHoSoToiPham
{
    public partial class QuanLyVuAn : Form
    {
        public QuanLyVuAn()
        {
            InitializeComponent();
        }
        SqlConnection conn;
        private void QuanLyVuAn_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection("Data Source=DESKTOP-KTQ1FE9\\DUCLONG;Initial Catalog=QuanLyToiPham;Integrated Security=True");
            conn.Open();
            hienthi();

            SqlCommand sqlCommand = new SqlCommand(@"select *from ThongTinCanBo", conn);
            SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            cbCanBo.DisplayMember = "MaCanBo";
            cbCanBo.ValueMember = "MaCanBo";
            cbCanBo.DataSource = dt;

            cbCanBo.SelectedIndexChanged += cbCanBo_SelectedIndexChanged;
        }

        private void cbCanBo_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Kiểm tra xem có mã cán bộ được chọn không
            if (cbCanBo.SelectedValue != null)
            {
                // Lấy mã cán bộ từ combobox
                string maCanBo = cbCanBo.SelectedValue.ToString();

                // Truy vấn cơ sở dữ liệu để lấy tên và cấp bậc của cán bộ tương ứng
                string sqlSelect = "SELECT TenCanBo FROM ThongTinCanBo WHERE MaCanBo = @MaCanBo";
                SqlCommand cmd = new SqlCommand(sqlSelect, conn);
                cmd.Parameters.AddWithValue("@MaCanBo", maCanBo);

                // Thực hiện truy vấn và đọc dữ liệu
                SqlDataReader reader = cmd.ExecuteReader();

                // Kiểm tra xem có dữ liệu được trả về không
                if (reader.Read())
                {
                    // Lấy tên và cấp bậc của cán bộ
                    string tenCanBo = reader["TenCanBo"].ToString();
                    

                    // Hiển thị tên và cấp bậc của cán bộ trong các textbox tương ứng
                    txtTenCB.Text = tenCanBo;
                    
                }
                else
                {
                    // Xử lý trường hợp không tìm thấy cán bộ
                    // Ví dụ: Xóa nội dung hiển thị trong các textbox
                    txtTenCB.Text = "";
                    
                }

                // Đóng đối tượng SqlDataReader
                reader.Close();
            }
        }
        void hienthi()
        {
            string sqlSelect = "select * From QuanLyVuAn ";
            SqlCommand cmd = new SqlCommand(sqlSelect, conn);
            SqlDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dataGridView1.DataSource = dt;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem các trường thông tin có được điền đầy đủ không
            if (txtMaVA.Text == "" || txtTenVA.Text == "" || txtMTaVuAn.Text == "" || cbCanBo.Text == "" || txtGhichu.Text == "" ) 

            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin.");
                return;
            }

            try
            {
                // Kiểm tra xem MaTheNguoiDung đã tồn tại trong cơ sở dữ liệu chưa
                string sqlCheckExist = "SELECT COUNT(*) FROM QuanLyVuAn WHERE MaVuAn= @MaVuAn";
                SqlCommand cmdCheckExist = new SqlCommand(sqlCheckExist, conn);
                cmdCheckExist.Parameters.AddWithValue("@MaVuAn", txtMaVA.Text);
                int count = Convert.ToInt32(cmdCheckExist.ExecuteScalar());

                if (count > 0)
                {
                    MessageBox.Show("Mã vụ án đã tồn tại trong cơ sở dữ liệu.");
                    return;
                }

                // Nếu không có trùng lặp, tiến hành thêm dữ liệu
                string sqlInsert = "INSERT INTO QuanLyVuAn (MaVuAn,TenVuAn,MoTaVuAn,NgayKhoiTo,NgayXetXu,MaCanBo,GhiChu) VALUES (@MaVuAn,@TenVuAn,@MoTaVuAn,@NgayKhoiTo,@NgayXetXu,@MaCanBo,@GhiChu)";
                SqlCommand cmdInsert = new SqlCommand(sqlInsert, conn);
                cmdInsert.Parameters.AddWithValue("@MaVuAn",txtMaVA.Text);
                cmdInsert.Parameters.AddWithValue("@TenVuAn", txtTenVA.Text);
                cmdInsert.Parameters.AddWithValue("@MoTaVuAn", txtMTaVuAn.Text);
                cmdInsert.Parameters.AddWithValue("@NgayKhoiTo", DateNgayKT.Text);
                cmdInsert.Parameters.AddWithValue("@NgayXetXu", DateNgayXS.Text);
                cmdInsert.Parameters.AddWithValue("@MaCanBo", cbCanBo.Text);
                cmdInsert.Parameters.AddWithValue("@GhiChu", txtGhichu.Text);

                int rowsAffected = cmdInsert.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Thêm dữ liệu thành công.");
                    hienthi(); // Cập nhật lại hiển thị danh sách sau khi thêm dữ liệu
                }
                else
                {
                    MessageBox.Show("Thêm dữ liệu thất bại.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btSua_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Bạn Chắc chắn muốn sửa?", "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                string sqlsua = "UPDATE QuanLyVuAn SET TenVuAn=@TenVuAn,MoTaVuAn=@MoTaVuAn,NgayKhoiTo=@NgayKhoiTo,NgayXetXu=@NgayXetXu,MaCanBo=@MaCanBo,GhiChu=@GhiChu WHERE MaVuAn=@MaVuAn ";
                SqlCommand cmd = new SqlCommand(sqlsua, conn);
                cmd.Parameters.AddWithValue("@MaVuAn", txtMaVA.Text);
                cmd.Parameters.AddWithValue("@TenVuAn", txtTenVA.Text);
                cmd.Parameters.AddWithValue("@MoTaVuAn", txtMTaVuAn.Text);
                cmd.Parameters.AddWithValue("@NgayKhoiTo", DateNgayKT.Text);
                cmd.Parameters.AddWithValue("@NgayXetXu", DateNgayXS.Text);
                cmd.Parameters.AddWithValue("@MaCanBo", cbCanBo.Text);
                cmd.Parameters.AddWithValue("@GhiChu", txtGhichu.Text);
                cmd.ExecuteNonQuery();
                hienthi();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i;
            i = dataGridView1.CurrentRow.Index;
            txtMaVA.Text = dataGridView1.Rows[i].Cells[0].Value.ToString();
            txtTenVA.Text = dataGridView1.Rows[i].Cells[1].Value.ToString();
            txtMTaVuAn.Text = dataGridView1.Rows[i].Cells[2].Value.ToString();
            DateNgayKT.Text = dataGridView1.Rows[i].Cells[3].Value.ToString();
            DateNgayXS.Text = dataGridView1.Rows[i].Cells[4].Value.ToString();
            cbCanBo.Text = dataGridView1.Rows[i].Cells[5].Value.ToString();
            txtGhichu.Text = dataGridView1.Rows[i].Cells[6].Value.ToString();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
            Main homeForm = new Main();
            homeForm.Show();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtMaVA.Text = " ";
            txtTenVA.Text = " ";
            txtMTaVuAn.Text = " ";
            cbCanBo.Text = " ";
            txtGhichu.Text = " ";
            txtTenCB.Text = " ";
            DateNgayKT.Text = "";
            DateNgayXS.Text = "";
        }

        private void btxoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn Chắc chắn muốn xóa?", "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                string sqlxoa = "delete from QuanLyVuAn where MaVuAn=@MaVuAn ";
                SqlCommand cmd = new SqlCommand(sqlxoa, conn);
                cmd.Parameters.AddWithValue("@MaVuAn", txtMaVA.Text);
                cmd.Parameters.AddWithValue("@TenVuAn", txtTenVA.Text);
                cmd.Parameters.AddWithValue("@MoTaVuAn", txtMTaVuAn.Text);
                cmd.Parameters.AddWithValue("@NgayKhoiTo", DateNgayKT.Text);
                cmd.Parameters.AddWithValue("@NgayXetXu", DateNgayXS.Text);
                cmd.Parameters.AddWithValue("@MaCanBo", cbCanBo.Text);
                cmd.Parameters.AddWithValue("@GhiChu", txtGhichu.Text);
                cmd.ExecuteNonQuery();
                hienthi();
            }
        }

        private void btTim_Click(object sender, EventArgs e)
        {
            string searchText = txtTim.Text.Trim();

            if (!string.IsNullOrEmpty(searchText))
            {
                try
                {
                    string sqlSelect = "SELECT * FROM QuanLyVuAn WHERE TenVuAn LIKE @searchText OR MaVuAn LIKE @searchText";
                    SqlCommand cmd = new SqlCommand(sqlSelect, conn);
                    cmd.Parameters.AddWithValue("@searchText", "%" + searchText + "%");

                    SqlDataReader dr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(dr);

                    if (dt.Rows.Count > 0)
                    {
                        dataGridView1.DataSource = dt;
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy kết quả nào.");
                    }

                    dr.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đã xảy ra lỗi: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập từ khóa tìm kiếm.");
            }
        }
    }
}
