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
    public partial class ThongTinPG : Form
    {
        public ThongTinPG()
        {
            InitializeComponent();
        }
        SqlConnection conn;


        private void ThongTinPG_Load(object sender, EventArgs e)
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
                string sqlSelect = "SELECT TenCanBo, CapBac FROM ThongTinCanBo WHERE MaCanBo = @MaCanBo";
                SqlCommand cmd = new SqlCommand(sqlSelect, conn);
                cmd.Parameters.AddWithValue("@MaCanBo", maCanBo);

                // Thực hiện truy vấn và đọc dữ liệu
                SqlDataReader reader = cmd.ExecuteReader();

                // Kiểm tra xem có dữ liệu được trả về không
                if (reader.Read())
                {
                    // Lấy tên và cấp bậc của cán bộ
                    string tenCanBo = reader["TenCanBo"].ToString();
                    string capBac = reader["CapBac"].ToString();

                    // Hiển thị tên và cấp bậc của cán bộ trong các textbox tương ứng
                    txtTenCanBo.Text = tenCanBo;
                    txtCapBac.Text = capBac;
                }
                else
                {
                    // Xử lý trường hợp không tìm thấy cán bộ
                    // Ví dụ: Xóa nội dung hiển thị trong các textbox
                    txtTenCanBo.Text = "";
                    txtCapBac.Text = "";
                }

                // Đóng đối tượng SqlDataReader
                reader.Close();
            }
        }
        void hienthi()
        {
            string sqlSelect = "select * From PhongGiam ";
            SqlCommand cmd = new SqlCommand(sqlSelect, conn);
            SqlDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dataGridView1.DataSource = dt;
        }
   

        private void btnThem_Click(object sender, EventArgs e)
        {

            // Kiểm tra xem các trường thông tin có được điền đầy đủ không
            if (tbMaPG.Text == "" || tbTenPG.Text == "" || cbCanBo.Text == "" )

            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin.");
                return;
            }

            try
            {
                // Kiểm tra xem MaTheNguoiDung đã tồn tại trong cơ sở dữ liệu chưa
                string sqlCheckExist = "SELECT COUNT(*) FROM PhongGiam WHERE MaPhongGiam = @MaPhongGiam";
                SqlCommand cmdCheckExist = new SqlCommand(sqlCheckExist, conn);
                cmdCheckExist.Parameters.AddWithValue("@MaPhongGiam",tbMaPG.Text);
                int count = Convert.ToInt32(cmdCheckExist.ExecuteScalar());

                if (count > 0)
                {
                    MessageBox.Show("Mã phòng giam đã tồn tại trong cơ sở dữ liệu.");
                    return;
                }

                // Nếu không có trùng lặp, tiến hành thêm dữ liệu
                string sqlInsert = "INSERT INTO PhongGiam (MaPhongGiam,TenPG,MaCanBo) VALUES (@MaPhongGiam,@TenPG,@MaCanBo)";
                SqlCommand cmdInsert = new SqlCommand(sqlInsert, conn);
                cmdInsert.Parameters.AddWithValue("@MaPhongGiam", tbMaPG.Text);
                cmdInsert.Parameters.AddWithValue("@TenPG", tbTenPG.Text);
                cmdInsert.Parameters.AddWithValue("@MaCanBo", cbCanBo.Text);
             

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
            tbMaPG.Text = dataGridView1.Rows[i].Cells[0].Value.ToString();
            tbTenPG.Text = dataGridView1.Rows[i].Cells[1].Value.ToString();
            cbCanBo.Text = dataGridView1.Rows[i].Cells[2].Value.ToString();
            
        }

        private void btSua_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn Chắc chắn muốn sửa?", "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                string sqlsua = "update PhongGiam Set TenPG=@TenPG,MaCanBo=@MaCanBo where MaPhongGiam=@MaPhongGiam";
                SqlCommand cmd = new SqlCommand(sqlsua, conn);
                cmd.Parameters.AddWithValue("MaPhongGiam", tbMaPG.Text);
                cmd.Parameters.AddWithValue("TenPG", tbTenPG.Text);
                cmd.Parameters.AddWithValue("MaCanBo", cbCanBo.Text);
               
                cmd.ExecuteNonQuery();
                hienthi();
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            tbMaPG.Text = "";
            tbTenPG.Text = "";
            cbCanBo.Text = "";
            txtTenCanBo.Text = "";
            txtCapBac.Text = "";
        }

        private void btxoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn Chắc chắn muốn xóa?", "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                string sqlxoa = "delete from PhongGiam where MaPhongGiam=@MaPhongGiam ";
                SqlCommand cmd = new SqlCommand(sqlxoa, conn);
                cmd.Parameters.AddWithValue("MaPhongGiam", tbMaPG.Text);
                cmd.Parameters.AddWithValue("TenPG", tbTenPG.Text);
                cmd.Parameters.AddWithValue("MaCanBo", cbCanBo.Text);

                cmd.ExecuteNonQuery();
                hienthi();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
            Main homeForm = new Main();
            homeForm.Show();
        }
    }
    }

