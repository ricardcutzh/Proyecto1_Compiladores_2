using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XForms.GUI.Visor
{
    public partial class VisualizadorForm : Form
    {
        public VisualizadorForm()
        {
            InitializeComponent();
        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {
            /// NO HACE NADA   
        }

        private void VisualizadorForm_Load(object sender, EventArgs e)
        {
            DirectoryInfo d = new DirectoryInfo(Directory.GetCurrentDirectory()+"\\FORMS\\");
            FileInfo[] files = d.GetFiles("*.html");
            foreach(FileInfo f in files)
            {
                this.comboBox1.Items.Add(f.Name);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                String filename = comboBox1.SelectedItem.ToString();
                System.Diagnostics.Process.Start(Directory.GetCurrentDirectory()+"\\FORMS\\"+filename);
            }
            catch
            {

            }
        }
    }
}
