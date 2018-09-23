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

namespace XForms.GUI.FechasHoras
{
    partial class FechasHoras : Form
    {
        Pregunta p;
        int tipo;

        public String respuesta = "";
        public object salir = null;

        public FechasHoras(Pregunta p, int tipo)
        {
            this.p = p;
            this.tipo = tipo;
            InitializeComponent();
            if(tipo == 0)
            {
                this.Input.Format = DateTimePickerFormat.Custom;
                this.Input.CustomFormat = "dd/MM/yyyy HH:mm:ss";
                this.Input.ShowUpDown = true;
            }
            else if(tipo==2)
            {
                this.Input.Format = DateTimePickerFormat.Custom;
                this.Input.CustomFormat = "HH:mm:ss";
                this.Input.ShowUpDown = true;
            }
            else if(tipo==1)
            {
                this.Input.Format = DateTimePickerFormat.Custom;
                this.Input.CustomFormat = "dd/MM/yyyy";
            }
            this.Input.Value = DateTime.Now;

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
            //CONTESTAR
            tomaRespuesta();
            compruebaRequerido(null);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //CONTINUAR
            compruebaRequerido(null);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //SALIR
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

        private void tomaRespuesta()
        {
            this.respuesta = this.Input.Value.ToString();
            if(respuesta.Length == 18)
            {
                this.respuesta = "0" + this.respuesta;
            }
            //this.respuesta = aux.ToString();
        }
    }
}
