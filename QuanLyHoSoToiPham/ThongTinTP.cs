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
    public partial class ThongTinTP : Form
    {
        public ThongTinTP()
        {
            InitializeComponent();
        }
        SqlConnection conn;
        private void ThongTinTP_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection("Data Source=DESKTOP-KTQ1FE9\\DUCLONG;Initial Catalog=QuanLyToiPham;Integrated Security=True");
            conn.Open();
            hienthi();

            SqlCommand sqlCommand = new SqlCommand(@"select *from QuanLyVuAn", conn);
            SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            cbVuAn.DisplayMember = "MaVuAn";
            cbVuAn.ValueMember = "MaVuAn";
            cbVuAn.DataSource = dt;

            cbVuAn.SelectedIndexChanged += cbVuAn_SelectedIndexChanged;

        }
        private void cbVuAn_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Kiểm tra xem có mã cán bộ được chọn không
            if (cbVuAn.SelectedValue != null)
            {
                // Lấy mã cán bộ từ combobox
                string maVuAn = cbVuAn.SelectedValue.ToString();

                // Truy vấn cơ sở dữ liệu để lấy tên và cấp bậc của cán bộ tương ứng
                string sqlSelect = "SELECT TenVuAn FROM QuanLyVuAn WHERE MaVuAn = @MaVuAn";
                SqlCommand cmd = new SqlCommand(sqlSelect, conn);
                cmd.Parameters.AddWithValue("@MaVuAn", maVuAn);

                // Thực hiện truy vấn và đọc dữ liệu
                SqlDataReader reader = cmd.ExecuteReader();

                // Kiểm tra xem có dữ liệu được trả về không
                if (reader.Read())
                {
                    // Lấy tên và cấp bậc của cán bộ
                    string tenVuAn = reader["TenVuAn"].ToString();


                    // Hiển thị tên và cấp bậc của cán bộ trong các textbox tương ứng
                    txtTenVuAn.Text = tenVuAn;

                }
                else
                {
                    // Xử lý trường hợp không tìm thấy cán bộ
                    // Ví dụ: Xóa nội dung hiển thị trong các textbox
                    txtTenVuAn.Text = "";

                }

                // Đóng đối tượng SqlDataReader
                reader.Close();
            }
        }
        void hienthi()
        {
            string sqlSelect = "select * From QuanLyThongTinToiPham ";
            SqlCommand cmd = new SqlCommand(sqlSelect, conn);
            SqlDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dataGridView1.DataSource = dt;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem các trường thông tin có được điền đầy đủ không
            if (txtMaTP.Text == "" || cbVuAn.Text == "" || txtNoiOHT.Text == "" || txtHoTen.Text == "" || txtQueQuan.Text == "" ||cbGioiTinh.Text == ""|| txtCanCuoc.Text==""||txtNguoiNha.Text==""||txtSDTNN.Text==""||txtTinhTrangSucKhoe.Text==""||txtGhiChu.Text=="")

            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin.");
                return;
            }

            try
            {
                // Kiểm tra xem MaTheNguoiDung đã tồn tại trong cơ sở dữ liệu chưa
                string sqlCheckExist = "SELECT COUNT(*) FROM QuanLyThongTinToiPham WHERE MaToiPham=@MaToiPham";
                SqlCommand cmdCheckExist = new SqlCommand(sqlCheckExist, conn);
                cmdCheckExist.Parameters.AddWithValue("@MaToiPham", txtMaTP.Text);
                int count = Convert.ToInt32(cmdCheckExist.ExecuteScalar());

                if (count > 0)
                {
                    MessageBox.Show("Mã tội phạm đã tồn tại trong cơ sở dữ liệu.");
                    return;
                }

                // Nếu không có trùng lặp, tiến hành thêm dữ liệu
                string sqlInsert = "INSERT INTO QuanLyThongTinToiPham (MaToiPham,MaVuAn,NoioHienTai,HoTen,QueQuan,GioiTinh,NgaySinh,CanCuoc,HoTenNN,SodtNN,TinhTrangSucKhoeTP,GhiChu) VALUES (@MaToiPham,@MaVuAn,@NoioHienTai,@HoTen,@QueQuan,@GioiTinh,@NgaySinh,@CanCuoc,@HoTenNN,@SodtNN,@TinhTrangSucKhoeTP,@GhiChu)";
                SqlCommand cmdInsert = new SqlCommand(sqlInsert, conn);
                cmdInsert.Parameters.AddWithValue("@MaToiPham", txtMaTP.Text);
                cmdInsert.Parameters.AddWithValue("@MaVuAn", cbVuAn.Text);
                cmdInsert.Parameters.AddWithValue("@NoioHienTai", txtNoiOHT.Text);
                cmdInsert.Parameters.AddWithValue("@HoTen", txtHoTen.Text);
                cmdInsert.Parameters.AddWithValue("@QueQuan", txtQueQuan.Text);
                cmdInsert.Parameters.AddWithValue("@GioiTinh", cbGioiTinh.Text);
                cmdInsert.Parameters.AddWithValue("@NgaySinh", dateNgáyinh.Text);
                cmdInsert.Parameters.AddWithValue("@CanCuoc", txtCanCuoc.Text);
                cmdInsert.Parameters.AddWithValue("@HoTenNN", txtNguoiNha.Text);
                cmdInsert.Parameters.AddWithValue("@SodtNN", txtSDTNN.Text);
                cmdInsert.Parameters.AddWithValue("@TinhTrangSucKhoeTP", txtTinhTrangSucKhoe.Text);
                cmdInsert.Parameters.AddWithValue("@GhiChu", txtGhiChu.Text);
                
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

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i;
            i = dataGridView1.CurrentRow.Index;
            txtMaTP.Text = dataGridView1.Rows[i].Cells[0].Value.ToString();
            cbVuAn.Text = dataGridView1.Rows[i].Cells[1].Value.ToString();
            txtNoiOHT.Text = dataGridView1.Rows[i].Cells[2].Value.ToString();
            txtHoTen.Text = dataGridView1.Rows[i].Cells[3].Value.ToString();
            txtQueQuan.Text = dataGridView1.Rows[i].Cells[4].Value.ToString();
            cbGioiTinh.Text = dataGridView1.Rows[i].Cells[5].Value.ToString();
            dateNgáyinh.Text = dataGridView1.Rows[i].Cells[6].Value.ToString();
            txtCanCuoc.Text = dataGridView1.Rows[i].Cells[7].Value.ToString();
            txtNguoiNha.Text = dataGridView1.Rows[i].Cells[8].Value.ToString();
            txtSDTNN.Text = dataGridView1.Rows[i].Cells[9].Value.ToString();
            txtTinhTrangSucKhoe.Text = dataGridView1.Rows[i].Cells[10].Value.ToString();
            txtGhiChu.Text = dataGridView1.Rows[i].Cells[11].Value.ToString();
           
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtMaTP.Text = "";
            cbVuAn.Text = "";
            txtNoiOHT.Text = "";
            txtHoTen.Text = "";
            txtQueQuan.Text = "";
            cbGioiTinh.Text = "";
            dateNgáyinh.Text = "";
            txtCanCuoc.Text = "";
            txtNguoiNha.Text = "";
            txtSDTNN.Text = "";
            txtTinhTrangSucKhoe.Text = "";
            txtGhiChu.Text = "";
            txtTenVuAn.Text = "";
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
            Main homeForm = new Main();
            homeForm.Show();
        }

        private void btSua_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn Chắc chắn muốn sửa?", "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                string sqlsua = "UPDATE QuanLyThongTinToiPham SET MaVuAn=@MaVuAn,NoioHienTai=@NoioHienTai,HoTen=@HoTen,QueQuan=@QueQuan,GioiTinh=@GioiTinh,NgaySinh=@NgaySinh,CanCuoc=@CanCuoc,HoTenNN=@HoTenNN,SodtNN=@SodtNN,TinhTrangSucKhoeTP=@TinhTrangSucKhoeTP,GhiChu=@GhiChu Where MaToiPham=@MaToiPham ";
                SqlCommand cmd = new SqlCommand(sqlsua, conn);
                cmd.Parameters.AddWithValue("@MaToiPham", txtMaTP.Text);
                cmd.Parameters.AddWithValue("@MaVuAn", cbVuAn.Text);
                cmd.Parameters.AddWithValue("@NoioHienTai", txtNoiOHT.Text);
                cmd.Parameters.AddWithValue("@HoTen", txtHoTen.Text);
                cmd.Parameters.AddWithValue("@QueQuan", txtQueQuan.Text);
                cmd.Parameters.AddWithValue("@GioiTinh", cbGioiTinh.Text);
                cmd.Parameters.AddWithValue("@NgaySinh", dateNgáyinh.Text);
                cmd.Parameters.AddWithValue("@CanCuoc", txtCanCuoc.Text);
                cmd.Parameters.AddWithValue("@HoTenNN", txtNguoiNha.Text);
                cmd.Parameters.AddWithValue("@SodtNN", txtSDTNN.Text);
                cmd.Parameters.AddWithValue("@TinhTrangSucKhoeTP", txtTinhTrangSucKhoe.Text);
                cmd.Parameters.AddWithValue("@GhiChu", txtGhiChu.Text);
                cmd.ExecuteNonQuery();
                hienthi();
            }
        }

        private void btxoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn Chắc chắn muốn xóa?", "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                string sqlxoa = "delete from QuanLyThongTinToiPham where MaToiPham=@MaToiPham ";
                SqlCommand cmd = new SqlCommand(sqlxoa, conn);
                cmd.Parameters.AddWithValue("@MaToiPham", txtMaTP.Text);
                cmd.Parameters.AddWithValue("@MaVuAn", cbVuAn.Text);
                cmd.Parameters.AddWithValue("@NoioHienTai", txtNoiOHT.Text);
                cmd.Parameters.AddWithValue("@HoTen", txtHoTen.Text);
                cmd.Parameters.AddWithValue("@QueQuan", txtQueQuan.Text);
                cmd.Parameters.AddWithValue("@GioiTinh", cbGioiTinh.Text);
                cmd.Parameters.AddWithValue("@NgaySinh", dateNgáyinh.Text);
                cmd.Parameters.AddWithValue("@CanCuoc", txtCanCuoc.Text);
                cmd.Parameters.AddWithValue("@HoTenNN", txtNguoiNha.Text);
                cmd.Parameters.AddWithValue("@SodtNN", txtSDTNN.Text);
                cmd.Parameters.AddWithValue("@TinhTrangSucKhoeTP", txtTinhTrangSucKhoe.Text);
                cmd.Parameters.AddWithValue("@GhiChu", txtGhiChu.Text);
                cmd.ExecuteNonQuery();
                hienthi();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string searchText = textBox13.Text.Trim();

            if (!string.IsNullOrEmpty(searchText))
            {
                try
                {
                    string sqlSelect = "SELECT * FROM QuanLyThongTinToiPham WHERE HoTen LIKE @searchText OR MaToiPham LIKE @searchText";
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

