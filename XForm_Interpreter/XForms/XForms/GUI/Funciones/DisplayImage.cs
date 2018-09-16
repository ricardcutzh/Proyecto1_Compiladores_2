using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XForms.GUI.Funciones
{
    public partial class DisplayImage : Form
    {
        public DisplayImage(String rutaImagen)
        {
            InitializeComponent();
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            if (System.IO.File.Exists(rutaImagen))
            {
                pictureBox1.Image = Image.FromFile(rutaImagen); 
            }
            else
            {
                pictureBox1.Image = Properties.Resources.if_shield_error_299056;
            }
        }

        private void DisplayImage_Load(object sender, EventArgs e)
        {
            
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
