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
using XForms.GramaticaIrony;
using XForms.ASTTree.Sentencias;    

namespace XForms.GUI.Condicion
{
    partial class Condicional : Form
    {
        Pregunta p;
        int linea, col;
        String clase, archivo;

        public String respuesta = "";
        public object salir = null;

        String etiquetaV, etiquetaF;

        private void button3_Click(object sender, EventArgs e)
        {
            //SALIR
            compruebaRequerido(new Romper());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //CONTINUAR
            compruebaRequerido(null);
        }

        public Condicional(Pregunta p, String etiquetaV, String etiquetaF, int linea, int col, String clase, String archivo)
        {

            this.p = p;
            this.linea = linea;
            this.col = col;
            this.clase = clase;
            this.archivo = archivo;

            this.etiquetaF = etiquetaF;
            this.etiquetaV = etiquetaV;

            

            InitializeComponent();

            this.opcVerd.Text = this.etiquetaV;
            this.opcFalso.Text = this.etiquetaF;

            if(p.lectura == true)
            {
                this.button1.Enabled = false;
            }

            String salida = Estatico.toHTMLTitle("(" + p.numeroPregunta + ") Texto: " + p.idPregunta);
            if (!p.etiqueta.Equals(""))
            {
                salida = salida + Estatico.toHTMLCard("Etiquerta: " + p.etiqueta);
            }
            if (!p.sugerencia.Equals(""))
            {
                salida = salida + Estatico.toHTMLCard("Sugerencia: " + p.sugerencia);
            }
            if (!p.requeridoMsn.Equals(""))
            {
                salida = salida + Estatico.toHTMLCard(Estatico.toHTMLAlert(p.requeridoMsn));
            }

            Salida.DocumentText = Estatico.header() + salida + Estatico.footer();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // CONTESTAR
            contestar();
            compruebaRequerido(null);
        }

        private void compruebaRequerido(Object goOut)
        {
            if (this.respuesta.Equals("") && p.requerido)
            {
                MessageBox.Show("Campo requerido", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                this.salir = goOut;
                this.Close();
            }
        }

        public void contestar()
        {
            if(opcVerd.Checked == true)
            {
                this.respuesta = "true";
            }
            else if(opcFalso.Checked == true)
            {
                this.respuesta = "false";
            }
        }
    }
}
