using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XForms.Objs;

namespace XForms.GUI.Notas
{
    partial class Nota : Form
    {
        public Nota()
        {
            InitializeComponent();

        }

        public Nota(Pregunta p)
        {
            InitializeComponent();

            this.Etiqueta.Text = "<span style='Color:Blue'>" + " The color is " + "</span>" + "<span style='Color:Red'>"+"red"+"</span>";
            this.Sugerencia.Text = p.sugerencia;
        }

        private void Next_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
