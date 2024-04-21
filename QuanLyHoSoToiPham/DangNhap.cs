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
    public partial class DangNhap : Form
    {
        public DangNhap()
        {
            InitializeComponent();
        }
        SqlConnection conn;
        private void DangNhap_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection("Data Source=DESKTOP-KTQ1FE9\\DUCLONG;Initial Catalog=QuanLyToiPham;Integrated Security=True");
            conn.Open();
            hienthi();
        }
        void hienthi()
        {
            string sqlSelect = "select * From DangNhap ";
            SqlCommand cmd = new SqlCommand(sqlSelect, conn);
            SqlDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dataLogin.DataSource = dt;
        }

        private void DangNhap_FormClosing(object sender, FormClosingEventArgs e)
        {
            conn.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
            Main homeForm = new Main();
            homeForm.Show();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem các trường thông tin có được điền đầy đủ không
            if (txtMThe.Text == "" || txtHoTen.Text == "" || dateSinh.Text =="" || txtCV.Text == "" || txtLTK.Text == "" || txtTenTk.Text == "" || txtMatKhau.Text == "")

            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin.");
                return;
            }

            try
            {
                // Kiểm tra xem MaTheNguoiDung đã tồn tại trong cơ sở dữ liệu chưa
                string sqlCheckExist = "SELECT COUNT(*) FROM DangNhap WHERE MaTheNguoiDung = @MaTheNguoiDung";
                SqlCommand cmdCheckExist = new SqlCommand(sqlCheckExist, conn);
                cmdCheckExist.Parameters.AddWithValue("@MaTheNguoiDung", txtMThe.Text);
                int count = Convert.ToInt32(cmdCheckExist.ExecuteScalar());

                if (count > 0)
                {
                    MessageBox.Show("Mã người dùng đã tồn tại trong cơ sở dữ liệu.");
                    return;
                }

                // Nếu không có trùng lặp, tiến hành thêm dữ liệu
                string sqlInsert = "INSERT INTO DangNhap (MaTheNguoiDung, HoTenNguoiDung, NgaySinh, ChucVu, LoaiTK, TenTaiKhoan, MatKhau) VALUES (@MaTheNguoiDung, @HoTenNguoiDung, @NgaySinh, @ChucVu, @LoaiTK, @TenTaiKhoan, @MatKhau)";
                SqlCommand cmdInsert = new SqlCommand(sqlInsert, conn);
                cmdInsert.Parameters.AddWithValue("@MaTheNguoiDung", txtMThe.Text);
                cmdInsert.Parameters.AddWithValue("@HoTenNguoiDung", txtHoTen.Text);
                cmdInsert.Parameters.AddWithValue("@NgaySinh", dateSinh.Text);
                cmdInsert.Parameters.AddWithValue("@ChucVu", txtCV.Text);
                cmdInsert.Parameters.AddWithValue("@LoaiTK", txtLTK.Text);
                cmdInsert.Parameters.AddWithValue("@TenTaiKhoan", txtTenTk.Text);
                cmdInsert.Parameters.AddWithValue("@MatKhau", txtMatKhau.Text);

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

        private void button1_Click(object sender, EventArgs e)
        {
          
                if (MessageBox.Show("Bạn Chắc chắn muốn sửa?", "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    string sqlsua = "UPDATE DangNhap SET HoTenNguoiDung=@HoTenNguoiDung, NgaySinh=@NgaySinh, ChucVu=@ChucVu, LoaiTK=@LoaiTK, TenTaiKhoan=@TenTaiKhoan, MatKhau=@MatKhau WHERE MaTheNguoiDung=@MaTheNguoiDung";
                    SqlCommand cmd = new SqlCommand(sqlsua, conn);
                    cmd.Parameters.AddWithValue("@MaTheNguoiDung", txtMThe.Text);
                    cmd.Parameters.AddWithValue("@HoTenNguoiDung", txtHoTen.Text);
                    cmd.Parameters.AddWithValue("@NgaySinh", dateSinh.Text);
                    cmd.Parameters.AddWithValue("@ChucVu", txtCV.Text);
                    cmd.Parameters.AddWithValue("@LoaiTK", txtLTK.Text);
                    cmd.Parameters.AddWithValue("@TenTaiKhoan", txtTenTk.Text);
                    cmd.Parameters.AddWithValue("@MatKhau", txtMatKhau.Text);
                    cmd.ExecuteNonQuery();
                    hienthi();
                }
          
        }

        private void dataLogin_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i;
            i = dataLogin.CurrentRow.Index;
            txtMThe.Text = dataLogin.Rows[i].Cells[0].Value.ToString();
            txtHoTen.Text = dataLogin.Rows[i].Cells[1].Value.ToString();
            dateSinh.Text = dataLogin.Rows[i].Cells[2].Value.ToString();
            txtCV.Text = dataLogin.Rows[i].Cells[3].Value.ToString();
            txtLTK.Text = dataLogin.Rows[i].Cells[4].Value.ToString();
            txtTenTk.Text = dataLogin.Rows[i].Cells[5].Value.ToString();
            txtMatKhau.Text = dataLogin.Rows[i].Cells[6].Value.ToString();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
             txtMThe.Text="";
             txtHoTen.Text = "" ;
             dateSinh.Text = "";
             txtCV.Text = "";
             txtLTK.Text = "";
             txtTenTk.Text = "";
             txtMatKhau.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn Chắc chắn muốn xóa?", "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                string sqlxoa = "delete from DangNhap where MaTheNguoiDung=@MaTheNguoiDung ";
                SqlCommand cmd = new SqlCommand(sqlxoa, conn);
                cmd.Parameters.AddWithValue("@MaTheNguoiDung", txtMThe.Text);
                cmd.Parameters.AddWithValue("@HoTenNguoiDung", txtHoTen.Text);
                cmd.Parameters.AddWithValue("@NgaySinh", dateSinh.Text);
                cmd.Parameters.AddWithValue("@ChucVu", txtCV.Text);
                cmd.Parameters.AddWithValue("@LoaiTK", txtLTK.Text);
                cmd.Parameters.AddWithValue("@TenTaiKhoan", txtTenTk.Text);
                cmd.Parameters.AddWithValue("@MatKhau", txtMatKhau.Text);
                cmd.ExecuteNonQuery();
                hienthi();
            }
        }
    }



    }

