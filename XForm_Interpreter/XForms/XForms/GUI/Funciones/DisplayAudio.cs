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
    public partial class DisplayAudio : Form
    {
        public DisplayAudio(String rutaAudio, Boolean reproduccion)
        {
            InitializeComponent();
            if (System.IO.File.Exists(rutaAudio))
            {
                axWindowsMediaPlayer1.settings.autoStart = reproduccion;
                axWindowsMediaPlayer1.URL = rutaAudio;

            }
            else
            {
                MessageBox.Show("Error al reproducir el Audio");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.close();
            this.Close();
        }
    }
}
