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

namespace XForms.GUI.Numericos
{
    partial class Numerico : Form
    {
        int col, linea, superior, inferior;
        Pregunta p;
        Boolean tomaParams;

        public String respuesta = "";
        public object salir = null;

        public Numerico(Pregunta p, int linea, int col, int inferior, int superior, Boolean tomaParams, Boolean esDecimal)
        {
            this.col = col;
            this.linea = linea;
            this.inferior = inferior;
            this.superior = superior;

            this.p = p;
            this.tomaParams = tomaParams;

            InitializeComponent();

            if(tomaParams)
            {
                this.Input.Minimum = inferior;
                this.Input.Maximum = superior;
            }

            if(esDecimal)
            {
                this.Input.DecimalPlaces = 4;
                this.Input.Increment = 0.1m;
            }

            if(this.p.lectura == true)
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
            //CONTESTAR
            this.respuesta = this.Input.Value.ToString();
            compruebaRequerido(null);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // CONTINUAR
            compruebaRequerido(null);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // SALIR
            compruebaRequerido(new Romper());
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
    }
}
