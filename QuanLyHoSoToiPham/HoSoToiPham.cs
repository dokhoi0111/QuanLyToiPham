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
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace QuanLyHoSoToiPham
{
    public partial class HoSoToiPham : Form
    {
        public HoSoToiPham()
        {
            InitializeComponent();
        }
        SqlConnection conn;
        private void HoSoToiPham_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection("Data Source=DESKTOP-KTQ1FE9\\DUCLONG;Initial Catalog=QuanLyToiPham;Integrated Security=True");
            conn.Open();
            hienthi();

            SqlCommand sqlCommand = new SqlCommand(@"select *from QuanLyVuAn", conn);
            SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            cbVA.DisplayMember = "MaVuAn";
            cbVA.ValueMember = "MaVuAn";
            cbVA.DataSource = dt;
            cbVA.SelectedIndexChanged += cbVA_SelectedIndexChanged;

            SqlCommand sqlCommand1 = new SqlCommand(@"select *from PhongGiam", conn);
            SqlDataAdapter adapter1 = new SqlDataAdapter(sqlCommand1);
            DataTable dt1 = new DataTable();
            adapter1.Fill(dt1);
            cbPG.DisplayMember = "MaPhongGiam";
            cbPG.ValueMember = "MaPhongGiam";
            cbPG.DataSource = dt1;
            cbPG.SelectedIndexChanged += cbPG_SelectedIndexChanged;

            SqlCommand sqlCommand2 = new SqlCommand(@"select *from ThongTinCanBo", conn);
            SqlDataAdapter adapter2 = new SqlDataAdapter(sqlCommand2);
            DataTable dt2 = new DataTable();
            adapter2.Fill(dt2);
            cbCB.DisplayMember = "MaCanBo";
            cbCB.ValueMember = "MaCanBo";
            cbCB.DataSource = dt2;
            cbCB.SelectedIndexChanged += cbCB_SelectedIndexChanged;

            SqlCommand sqlCommand3 = new SqlCommand(@"select *from QuanLyThongTinToiPham", conn);
            SqlDataAdapter adapter3 = new SqlDataAdapter(sqlCommand3);
            DataTable dt3 = new DataTable();
            adapter3.Fill(dt3);
            cbTP.DisplayMember = "MaToiPham";
            cbTP.ValueMember = "MaToiPham";
            cbTP.DataSource = dt3;
            cbTP.SelectedIndexChanged += cbTP_SelectedIndexChanged;
        }
        private void cbTP_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Kiểm tra xem có mã cán bộ được chọn không
            if (cbTP.SelectedValue != null)
            {
                // Lấy mã cán bộ từ combobox
                string maToiPham= cbTP.SelectedValue.ToString();

                // Truy vấn cơ sở dữ liệu để lấy tên và cấp bậc của cán bộ tương ứng
                string sqlSelect = "SELECT HoTen,NoioHienTai,QueQuan,GioiTinh,NgaySinh,CanCuoc,HoTenNN,TinhTrangSucKhoeTP FROM QuanLyThongTinToiPham WHERE MaToiPham = @MaToiPham";
                SqlCommand cmd = new SqlCommand(sqlSelect, conn);
                cmd.Parameters.AddWithValue("@MaToiPham", maToiPham);

                // Thực hiện truy vấn và đọc dữ liệu
                SqlDataReader reader = cmd.ExecuteReader();

                // Kiểm tra xem có dữ liệu được trả về không
                if (reader.Read())
                {
                    // Lấy tên và cấp bậc của cán bộ
                    string tentp = reader["HoTen"].ToString();
                    string noioht = reader["NoioHienTai"].ToString();
                    string quequan = reader["QueQuan"].ToString();
                    string gioitinh = reader["GioiTinh"].ToString();
                    string ngaysinh = reader["NgaySinh"].ToString();
                    string caccuoc = reader["CanCuoc"].ToString();
                    string hotennn = reader["HoTenNN"].ToString();
                    string tinhtrangsuckhoe = reader["TinhTrangSucKhoeTP"].ToString();

                    // Hiển thị tên và cấp bậc của cán bộ trong các textbox tương ứng
                    txtTenTP.Text = tentp;
                    txtNOHTTP.Text = noioht;
                    txtQQTP.Text = quequan;
                    txtGTTP.Text = gioitinh;
                    dateNgaySinhTP.Text = ngaysinh;
                    txtCCTP.Text = caccuoc;
                    txtTenBoMe.Text = hotennn;
                    txtTinhTrangSK.Text = tinhtrangsuckhoe;
                  
                }
                else
                {
                    // Xử lý trường hợp không tìm thấy cán bộ
                    // Ví dụ: Xóa nội dung hiển thị trong các textbox
                    txtTenTP.Text = "";
                    txtNOHTTP.Text = "";
                    txtQQTP.Text = "";
                    txtGTTP.Text = "";
                    dateNgaySinhTP.Text = "";
                    txtCCTP.Text = "";
                    txtTenBoMe.Text = "";
                    txtTinhTrangSK.Text = "";
                }

                // Đóng đối tượng SqlDataReader
                reader.Close();
            }
        }
        private void cbVA_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Kiểm tra xem có mã cán bộ được chọn không
            if (cbVA.SelectedValue != null)
            {
                // Lấy mã cán bộ từ combobox
                string maVuAn = cbVA.SelectedValue.ToString();

                // Truy vấn cơ sở dữ liệu để lấy tên và cấp bậc của cán bộ tương ứng
                string sqlSelect = "SELECT TenVuAn, MoTaVuAn,NgayKhoiTo,NgayXetXu FROM QuanLyVuAn WHERE MaVuAn = @MaVuAn";
                SqlCommand cmd = new SqlCommand(sqlSelect, conn);
                cmd.Parameters.AddWithValue("@MaVuAn", maVuAn);

                // Thực hiện truy vấn và đọc dữ liệu
                SqlDataReader reader = cmd.ExecuteReader();

                // Kiểm tra xem có dữ liệu được trả về không
                if (reader.Read())
                {
                    // Lấy tên và cấp bậc của cán bộ
                    string tenVuAn = reader["TenVuAn"].ToString();
                    string motavuan = reader["MoTaVuAn"].ToString();
                    string ngaykhoito = reader["NgayKhoiTo"].ToString();
                    string ngayxetxu = reader["NgayXetXu"].ToString();

                    // Hiển thị tên và cấp bậc của cán bộ trong các textbox tương ứng
                    txtTenVA.Text = tenVuAn;
                    txtMoTaVA.Text = motavuan;
                    dateKhoito.Text = ngaykhoito;
                    dateXetxu.Text = ngayxetxu;
                }
                else
                {
                    // Xử lý trường hợp không tìm thấy cán bộ
                    // Ví dụ: Xóa nội dung hiển thị trong các textbox
                    txtTenVA.Text = "";
                    txtMoTaVA.Text = "";
                    dateKhoito.Text = "";
                    dateXetxu.Text = "";
                }

                // Đóng đối tượng SqlDataReader
                reader.Close();
            }
        }
        private void cbPG_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Kiểm tra xem có mã cán bộ được chọn không
            if (cbPG.SelectedValue != null)
            {
                // Lấy mã cán bộ từ combobox
                string maPG = cbPG.SelectedValue.ToString();

                // Truy vấn cơ sở dữ liệu để lấy tên và cấp bậc của cán bộ tương ứng
                string sqlSelect = "SELECT TenPG FROM PhongGiam WHERE MaPhongGiam = @MaPhongGiam";
                SqlCommand cmd = new SqlCommand(sqlSelect, conn);
                cmd.Parameters.AddWithValue("@MaPhongGiam",maPG);

                // Thực hiện truy vấn và đọc dữ liệu
                SqlDataReader reader = cmd.ExecuteReader();

                // Kiểm tra xem có dữ liệu được trả về không
                if (reader.Read())
                {
                    // Lấy tên và cấp bậc của cán bộ
                    string tenPG = reader["TenPG"].ToString();


                    // Hiển thị tên và cấp bậc của cán bộ trong các textbox tương ứng
                    txtPG.Text = tenPG;
                    
                }
                else
                {
                    // Xử lý trường hợp không tìm thấy cán bộ
                    // Ví dụ: Xóa nội dung hiển thị trong các textbox
                    txtPG.Text = "";
                }

                // Đóng đối tượng SqlDataReader
                reader.Close();
            }
        }
        private void cbCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Kiểm tra xem có mã cán bộ được chọn không
            if (cbVA.SelectedValue != null)
            {
                // Lấy mã cán bộ từ combobox
                string maCanBo = cbCB.SelectedValue.ToString();

                // Truy vấn cơ sở dữ liệu để lấy tên và cấp bậc của cán bộ tương ứng
                string sqlSelect = "SELECT TenCanBo, TheCC, NgaySinh,QueQuan,GioiTinh,HienTai,SoDT,CapBac FROM ThongTinCanBo WHERE MaCanBo = @MaCanBo";
                SqlCommand cmd = new SqlCommand(sqlSelect, conn);
                cmd.Parameters.AddWithValue("@MaCanBo", maCanBo);

                // Thực hiện truy vấn và đọc dữ liệu
                SqlDataReader reader = cmd.ExecuteReader();

                // Kiểm tra xem có dữ liệu được trả về không
                if (reader.Read())
                {
                    // Lấy tên và cấp bậc của cán bộ
                    string tenCanBo = reader["TenCanBo"].ToString();
                    string thecc = reader["TheCC"].ToString();
                    string ngaysinh = reader["NgaySinh"].ToString();
                    string quequan = reader["QueQuan"].ToString();
                    string gioitinh = reader["GioiTinh"].ToString();
                    string hientai = reader["HienTai"].ToString();
                    string SoDT = reader["SoDT"].ToString();
                    string capbac = reader["CapBac"].ToString();

                    // Hiển thị tên và cấp bậc của cán bộ trong các textbox tương ứng
                    txtTenCB.Text = tenCanBo;
                    txtCCCB.Text = thecc;
                    dateNgaySCB.Text = ngaysinh;
                    txtQQCB.Text = quequan;
                    txtGTCB.Text = gioitinh;
                    txtNOHTCB.Text = hientai;
                    txtSDTCB.Text = SoDT;
                    txtCBCB.Text = capbac;
                }
                else
                {
                    // Xử lý trường hợp không tìm thấy cán bộ
                    // Ví dụ: Xóa nội dung hiển thị trong các textbox
                    txtTenCB.Text = "";
                    txtCCCB.Text = "";
                    dateNgaySCB.Text = "";
                    txtQQCB.Text = "";
                    txtGTCB.Text = "";
                    txtNOHTCB.Text = "";
                    txtSDTCB.Text = "";
                    txtCBCB.Text = "";
                }

                // Đóng đối tượng SqlDataReader
                reader.Close();
            }
        }
        void hienthi()
        {
            string sqlSelect = "select * From QuanLyHoSoToiPham ";
            SqlCommand cmd = new SqlCommand(sqlSelect, conn);
            SqlDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dataGridView1.DataSource = dt;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem các trường thông tin có được điền đầy đủ không
            if (txtMaHoSo.Text == "" || txtBanAn.Text == "" || dateBatDau.Text == "" || dateKetThuc.Text == "" || txtGhiChu.Text == "" )

            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin.");
                return;
            }

            try
            {
                // Kiểm tra xem MaTheNguoiDung đã tồn tại trong cơ sở dữ liệu chưa
                string sqlCheckExist = "SELECT COUNT(*) FROM QuanLyHoSoToiPham WHERE MaHoSo=@MaHoSo";
                SqlCommand cmdCheckExist = new SqlCommand(sqlCheckExist, conn);
                cmdCheckExist.Parameters.AddWithValue("@MaHoSo", txtMaHoSo.Text);
                int count = Convert.ToInt32(cmdCheckExist.ExecuteScalar());

                if (count > 0)
                {
                    MessageBox.Show("Mã hồ sơ đã tồn tại trong cơ sở dữ liệu.");
                    return;
                }

                // Nếu không có trùng lặp, tiến hành thêm dữ liệu
                string sqlInsert = "INSERT INTO QuanLyHoSoToiPham (MaHoSo,MaToiPham,MaCanBo,MaVuAn,MaPhongGiam,BanAn,NgayBatDauThiHanh,NgayKetThuc,GhiChu) VALUES " +
                    "(@MaHoSo,@MaToiPham,@MaCanBo,@MaVuAn,@MaPhongGiam,@BanAn,@NgayBatDauThiHanh,@NgayKetThuc,@GhiChu)";
                SqlCommand cmdInsert = new SqlCommand(sqlInsert, conn);
                cmdInsert.Parameters.AddWithValue("@MaHoSo", txtMaHoSo.Text);
                cmdInsert.Parameters.AddWithValue("@MaToiPham", cbTP.Text);
                cmdInsert.Parameters.AddWithValue("@MaCanBo", cbCB.Text);
                cmdInsert.Parameters.AddWithValue("@MaVuAn", cbVA.Text);
                cmdInsert.Parameters.AddWithValue("@MaPhongGiam", cbPG.Text);
                cmdInsert.Parameters.AddWithValue("@BanAn", txtBanAn.Text);
                cmdInsert.Parameters.AddWithValue("@NgayBatDauThiHanh", dateBatDau.Text);
                cmdInsert.Parameters.AddWithValue("@NgayKetThuc", dateKetThuc.Text);
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

        private void button2_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Bạn Chắc chắn muốn sửa?", "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                string sqlsua = "UPDATE QuanLyVuAn SET MaToiPham=@MaToiPham,MaCanBo=@MaCanBo,MaVuAn=@MaVuAn,MaPhongGiam=@MaPhongGiam,BanAn=@BanAn,NgayBatDauThiHanh=@NgayBatDauThiHanh,NgayKetThuc=@NgayKetThuc,GhiChu=@GhiChu Where MaHoSo=@MaHoSo  ";
                SqlCommand cmd = new SqlCommand(sqlsua, conn);
                cmd.Parameters.AddWithValue("@MaHoSo", txtMaHoSo.Text);
                cmd.Parameters.AddWithValue("@MaToiPham", cbTP.Text);
                cmd.Parameters.AddWithValue("@MaCanBo", cbCB.Text);
                cmd.Parameters.AddWithValue("@MaVuAn", cbVA.Text);
                cmd.Parameters.AddWithValue("@MaPhongGiam", cbPG.Text);
                cmd.Parameters.AddWithValue("@BanAn", txtBanAn.Text);
                cmd.Parameters.AddWithValue("@NgayBatDauThiHanh", dateBatDau.Text);
                cmd.Parameters.AddWithValue("@NgayKetThuc", dateKetThuc.Text);
                cmd.Parameters.AddWithValue("@GhiChu", txtGhiChu.Text);
                cmd.ExecuteNonQuery();
                hienthi();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i;
            i = dataGridView1.CurrentRow.Index;
            txtMaHoSo.Text = dataGridView1.Rows[i].Cells[0].Value.ToString();
            cbTP.Text = dataGridView1.Rows[i].Cells[1].Value.ToString();
            cbCB.Text = dataGridView1.Rows[i].Cells[2].Value.ToString();
            cbVA.Text = dataGridView1.Rows[i].Cells[3].Value.ToString();
            cbPG.Text = dataGridView1.Rows[i].Cells[4].Value.ToString();
            txtBanAn.Text = dataGridView1.Rows[i].Cells[5].Value.ToString();
            dateBatDau.Text = dataGridView1.Rows[i].Cells[6].Value.ToString();
            dateKetThuc.Text = dataGridView1.Rows[i].Cells[7].Value.ToString();
            txtGhiChu.Text = dataGridView1.Rows[i].Cells[8].Value.ToString();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
            Main homeForm = new Main();
            homeForm.Show();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtMaHoSo.Text = "";
            cbTP.Text = "";
            cbCB.Text = "";
            cbVA.Text = "";
            cbPG.Text = ""; 
            txtBanAn.Text = "";
            dateBatDau.Text = "";
            dateKetThuc.Text = "";
            txtGhiChu.Text = "";
            txtTenTP.Text = "";
            txtNOHTTP.Text = "";
            txtQQTP.Text = "";
            txtGTTP.Text = "";
            dateNgaySinhTP.Text = "";
            txtCCTP.Text = "";
            txtTenBoMe.Text = "";
            txtTinhTrangSK.Text = "";
            txtTenVA.Text = "";
            txtMoTaVA.Text = "";
            dateKhoito.Text = "";
            dateXetxu.Text = "";
            txtTenCB.Text = "";
            txtCCCB.Text = "";
            dateNgaySCB.Text = "";
            txtQQCB.Text = "";
            txtGTCB.Text = "";
            txtNOHTCB.Text = "";
            txtSDTCB.Text = "";
            txtCBCB.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string searchText = txtTim.Text.Trim();

            if (!string.IsNullOrEmpty(searchText))
            {
                try
                {
                    string sqlSelect = "SELECT * FROM QuanLyHoSoToiPham WHERE  MaHoSo LIKE @searchText";
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
