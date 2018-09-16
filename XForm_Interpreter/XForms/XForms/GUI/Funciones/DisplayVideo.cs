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
    public partial class DisplayVideo : Form
    {
        public DisplayVideo(String rutaVideo, Boolean reproducir)
        {
            InitializeComponent();
            if(System.IO.File.Exists(rutaVideo))
            {
                axWindowsMediaPlayer1.settings.autoStart = reproducir;
                axWindowsMediaPlayer1.URL = rutaVideo;
                
            }
            else
            {
                MessageBox.Show("Error al reproducir el Video");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.close();   
            this.Close();
        }
    }
}
