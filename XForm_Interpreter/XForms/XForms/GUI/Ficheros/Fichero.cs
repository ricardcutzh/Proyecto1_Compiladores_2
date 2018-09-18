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

namespace XForms.GUI.Ficheros
{
    partial class Fichero : Form
    {
        String salida = "";

        Pregunta p;

        String rutaArchivo = "";

        public Fichero(Pregunta p, String filtros)
        {
            this.p = p;
            InitializeComponent();
            salida = Estatico.toHTMLTitle("("+p.numeroPregunta+") Fichero: "+p.idPregunta);
            if (!p.etiqueta.Equals("") || !p.etiqueta.Equals(" "))
            {
                salida = salida + Estatico.toHTMLCard("Etiqueta: " + p.etiqueta);
            }
            if (!p.sugerencia.Equals("") || !p.sugerencia.Equals(" "))
            {
                salida = salida + Estatico.toHTMLCard("Sugerencia: " + p.sugerencia);
            }
            if (!p.requeridoMsn.Equals(""))
            {
                salida = salida + Estatico.toHTMLCard(Estatico.toHTMLAlert(p.requeridoMsn));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.rutaArchivo.Equals("") && p.requerido)
            {
                MessageBox.Show("Campo requerido", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                /*AQUI TENGO QUE PONER LA RUTA DEL ARCHIVO!!!*/
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(this.rutaArchivo.Equals("") && p.requerido)
            {
                MessageBox.Show("Campo requerido", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                this.Close();
            }
        }
    }
}
