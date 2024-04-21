using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QuanLyHoSoToiPham
{
    public partial class TTCanBo : Form
    {
        public TTCanBo()
        {
            InitializeComponent();
        }
        SqlConnection conn; 
        private void TTCanBo_Load(object sender, EventArgs e)
        {
            conn= new SqlConnection("Data Source=DESKTOP-KTQ1FE9\\DUCLONG;Initial Catalog=QuanLyToiPham;Integrated Security=True");
            conn.Open();
            hienthi();
        }
        void hienthi()
        {
            string sqlSelect = "select * From ThongTinCanBo";
            SqlCommand cmd = new SqlCommand(sqlSelect, conn);
            SqlDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dataTTCB.DataSource = dt;
        }
        // Câu lệnh này giúp ta không cần phải dùng đến conn đóng mở
        private void TTCanBo_FormClosing(object sender, FormClosingEventArgs e)
        {
            conn.Close();
        }
        private void pictureexit_Click(object sender, EventArgs e)
        {
            this.Close();
            Main homeForm = new Main(); 
            homeForm.Show();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TexMaCB.Text) || string.IsNullOrWhiteSpace(TexMaCB.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Thoát khỏi phương thức mà không thực hiện thêm dữ liệu
            }
            if (MaCanBo(TexMaCB.Text))
            {
                MessageBox.Show("Mã chức vụ đã tồn tại. Vui lòng nhập lại mã khác!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("Bạn Chắc chắn muốn thêm không?", "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                string sqlthem = "insert into ThongTinCanBo values(@MaCanBo,@TenCanBo,@TheCC,@NgaySinh,@QueQuan,@GioiTinh,@HienTai,@SoDT,@CapBac)";
                SqlCommand cmd = new SqlCommand(sqlthem, conn);
                cmd.Parameters.AddWithValue("MaCanBo",TexMaCB.Text);
                cmd.Parameters.AddWithValue("TenCanBo", texTenCB.Text);
                cmd.Parameters.AddWithValue("TheCC", txtCCCD.Text);
                cmd.Parameters.AddWithValue("NgaySinh",dateNgaySinh.Text);
                cmd.Parameters.AddWithValue("QueQuan", txtQQ.Text);
                cmd.Parameters.AddWithValue("GioiTinh", cbGioiTinh.Text);
                cmd.Parameters.AddWithValue("HienTai", txtHt.Text);
                cmd.Parameters.AddWithValue("SoDT",txtSdt.Text);
                cmd.Parameters.AddWithValue("CapBac", txtCB.Text);
                cmd.ExecuteNonQuery();
                hienthi();
                MessageBox.Show("Đã thêm thông tin cán bộ!");
            }
        }
        private bool MaCanBo(string macb)
        {
            // Thực hiện truy vấn kiểm tra tên đăng nhập trong cơ sở dữ liệu
            SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM ThongTinCanBo WHERE MaCanBo = @MaCanBo", conn);
            checkCmd.Parameters.AddWithValue("@MaCanBo", macb);

            int count = (int)checkCmd.ExecuteScalar();

            // Trả về true nếu có tồn tại, ngược lại trả về false
            return count > 0;
        }

        private void btnreset_Click(object sender, EventArgs e)
        {
             TexMaCB.Text = "";
             texTenCB.Text = "";
             txtCCCD.Text = "";
             dateNgaySinh.Text = "";
             txtQQ.Text = "";
             cbGioiTinh.Text = "";
             txtHt.Text = "";
             txtSdt.Text = "";
             txtCB.Text = "";
        }

        private void dataTTCB_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i;
            i = dataTTCB.CurrentRow.Index;
            TexMaCB.Text = dataTTCB.Rows[i].Cells[0].Value.ToString();
            texTenCB.Text = dataTTCB.Rows[i].Cells[1].Value.ToString();
            txtCCCD.Text = dataTTCB.Rows[i].Cells[2].Value.ToString();
            dateNgaySinh.Text = dataTTCB.Rows[i].Cells[3].Value.ToString();
            txtQQ.Text = dataTTCB.Rows[i].Cells[4].Value.ToString();
            cbGioiTinh.Text = dataTTCB.Rows[i].Cells[5].Value.ToString();
            txtHt.Text = dataTTCB.Rows[i].Cells[6].Value.ToString();
            txtSdt.Text = dataTTCB.Rows[i].Cells[7].Value.ToString();
            txtCB.Text = dataTTCB.Rows[i].Cells[8].Value.ToString();
        }

        private void btnsua_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn Chắc chắn muốn sửa?", "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                string sqlsua = "update ThongTinCanBo Set TenCanBo=@TenCanBo,TheCC=@TheCC,NgaySinh=@NgaySinh,QueQuan=@QueQuan,GioiTinh=@GioiTinh,HienTai=@HienTai,SoDT=@SoDT,CapBac=@CapBac where MaCanBo=@MaCanBo";
                SqlCommand cmd = new SqlCommand(sqlsua, conn);
                cmd.Parameters.AddWithValue("MaCanBo", TexMaCB.Text);
                cmd.Parameters.AddWithValue("TenCanBo", texTenCB.Text);
                cmd.Parameters.AddWithValue("TheCC", txtCCCD.Text);
                cmd.Parameters.AddWithValue("NgaySinh", dateNgaySinh.Text);
                cmd.Parameters.AddWithValue("QueQuan", txtQQ.Text);
                cmd.Parameters.AddWithValue("GioiTinh", cbGioiTinh.Text);
                cmd.Parameters.AddWithValue("HIenTai", txtHt.Text);
                cmd.Parameters.AddWithValue("SoDT", txtSdt.Text);
                cmd.Parameters.AddWithValue("CapBac", txtCB.Text);
                cmd.ExecuteNonQuery();
                hienthi();
            }
        }

        private void btnxoa_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Bạn Chắc chắn muốn xóa?", "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                string sqlxoa = "delete from ThongTinCanBo where MaCanBo=@MaCanBo ";
                SqlCommand cmd = new SqlCommand(sqlxoa, conn);
                cmd.Parameters.AddWithValue("MaCanBo", TexMaCB.Text);
                cmd.Parameters.AddWithValue("TenCanBo", texTenCB.Text);
                cmd.Parameters.AddWithValue("TheCC", txtCCCD.Text);
                cmd.Parameters.AddWithValue("NgaySinh", dateNgaySinh.Text);
                cmd.Parameters.AddWithValue("QueQuan", txtQQ.Text);
                cmd.Parameters.AddWithValue("GioiTinh", cbGioiTinh.Text);
                cmd.Parameters.AddWithValue("HIenTai", txtHt.Text);
                cmd.Parameters.AddWithValue("SoDT", txtSdt.Text);
                cmd.Parameters.AddWithValue("CapBac", txtCB.Text);
                cmd.ExecuteNonQuery();
                hienthi();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string searchText = textBox8.Text.Trim();

            if (!string.IsNullOrEmpty(searchText))
            {
                try
                {
                    string sqlSelect = "SELECT * FROM ThongTinCanBo WHERE TenCanBo LIKE @searchText OR MaCanBo LIKE @searchText";
                    SqlCommand cmd = new SqlCommand(sqlSelect, conn);
                    cmd.Parameters.AddWithValue("@searchText", "%" + searchText + "%");

                    SqlDataReader dr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(dr);

                    if (dt.Rows.Count > 0)
                    {
                        dataTTCB.DataSource = dt;
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
