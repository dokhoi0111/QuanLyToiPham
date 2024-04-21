using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyHoSoToiPham
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void thôngTinPhòngGiamToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ThongTinPG thongTinPG = new ThongTinPG();
            this.Hide();
            thongTinPG.Show();
        }

        private void thôngTinTộiPhạmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ThongTinTP thongTinTP = new ThongTinTP();
            this.Hide();
            thongTinTP.Show();
        }

      

        private void thôngTinCánBộToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TTCanBo tTCanBo = new TTCanBo();
            this.Hide();
            tTCanBo.Show(); 
        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 homeForm = new Form1();
            homeForm.Show();
        }

        private void chỉnhSửaTàiKhoảnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DangNhap dangNhap = new DangNhap();
            this.Hide();
            dangNhap.Show();
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }

        private void quảnLýVụÁnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QuanLyVuAn quanLyVu = new QuanLyVuAn();
            this.Hide();
            quanLyVu.Show();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void hồSơTộiPhạmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HoSoToiPham hoSoToiPham = new HoSoToiPham();
            this.Hide();
            hoSoToiPham.Show();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            BaoCao baoCao= new BaoCao();    
            this.Hide();
            baoCao.Show();
        }
    }
}
