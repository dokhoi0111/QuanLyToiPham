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

namespace QuanLyHoSoToiPham
{
    public partial class Form1 : Form
    {
        SqlConnection conn =new SqlConnection("Data Source=DESKTOP-KTQ1FE9\\DUCLONG;Initial Catalog=QuanLyToiPham;Integrated Security=True");
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn thoát không ?", "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                Close();
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            tbTaiKhoan.Text = "";
            tbMatKhau.Text = "";
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {

            string taiKhoan = tbTaiKhoan.Text;
            string matKhau = tbMatKhau.Text;

            // Kết nối đến cơ sở dữ liệu
            SqlConnection conn = new SqlConnection("Data Source=DESKTOP-KTQ1FE9\\DUCLONG;Initial Catalog=QuanLyToiPham;Integrated Security=True");

            try
            {
                conn.Open();

                // Tạo câu truy vấn SQL để kiểm tra thông tin đăng nhập
                string query = "SELECT COUNT(*) FROM DangNhap WHERE TenTaiKhoan=@TaiKhoan AND MatKhau=@MatKhau";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TaiKhoan", taiKhoan);
                cmd.Parameters.AddWithValue("@MatKhau", matKhau);

                int result = (int)cmd.ExecuteScalar(); // Lấy kết quả trả về (số hàng thỏa mãn điều kiện)

                if (result > 0)
                {
                    // Đăng nhập thành công, mở form chính
                    MessageBox.Show("Đăng nhập thành công!");
                    Main main = new Main();
                    this.Hide();
                    main.Show();
                }
                else
                {
                    // Đăng nhập không thành công
                    MessageBox.Show("Đăng nhập không thành công. Vui lòng kiểm tra lại tên đăng nhập và mật khẩu!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message);
            }
            finally
            {
                conn.Close(); // Đóng kết nối sau khi thực hiện xong
            }
        }
    }
}
