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

namespace XForms.GUI.Cadenas
{
    partial class Cadenas : Form
    {
        Pregunta p;
        int linea, col;
        String clase, archivo;

        public String respuesta = "";
        public object salir = null;

        int min;
        int fil;
        int max;

        Boolean tomaEnCuentaParams;

        public Cadenas(Pregunta p, int linea, int col, String clase, String archivo, int max, int min, int fil, Boolean tomaEnCuentaParams)
        {

            this.p = p;

            this.linea = linea;
            this.col = col;
            this.clase = clase;
            this.archivo = archivo;

            this.min = min;
            this.fil = fil;
            this.max = max;

            if(this.min < 0)
            {
                this.min = 0;
            }
            if(this.fil<0)
            {
                this.fil = 10;
            }
            

            this.tomaEnCuentaParams = tomaEnCuentaParams;

            InitializeComponent();

            if(this.p.lectura == true)
            {
                this.button1.Enabled = false;
            }

            String salida = Estatico.toHTMLTitle("(" + p.numeroPregunta + ") Texto: " + p.idPregunta);
            if (!p.etiqueta.Equals(""))
            {
                salida = salida + Estatico.toHTMLCard("Etiquerta: " + p.etiqueta);
            }
            if(!p.sugerencia.Equals(""))
            {
                salida = salida + Estatico.toHTMLCard("Sugerencia: " + p.sugerencia);
            }
            if (!p.requeridoMsn.Equals(""))
            {
                salida = salida + Estatico.toHTMLCard(Estatico.toHTMLAlert(p.requeridoMsn));
            }

            if (max > 0) { this.Input.MaxLength = max; }

            Salida.DocumentText = Estatico.header() + salida + Estatico.footer();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //CONTESTAR
            this.respuesta = this.Input.Text;

            if(respuestaValida(this.respuesta))
            {
                compruebaRequerido(null);
            }
            else
            {
                MessageBox.Show("La respuesta necesita que lleve al como minimo: "+this.min+" caracteres, maximo: "+this.max+" caracteres y solo: "+this.fil+" lineas", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //CONTINUAR
            if (respuestaValida(this.respuesta))
            {
                compruebaRequerido(null);
            }
            else
            {
                MessageBox.Show("La respuesta necesita que lleve al como minimo: " + this.min + " caracteres, maximo: " + this.max + " caracteres y solo: " + this.fil + " lineas", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Input_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //SALIR
            if (respuestaValida(this.respuesta))
            {
                compruebaRequerido(new Romper());
            }
            else
            {
                MessageBox.Show("La respuesta necesita que lleve al como minimo: " + this.min + " caracteres, maximo: " + this.max + " caracteres y solo: " + this.fil + " lineas", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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


        private Boolean respuestaValida(String resp)
        {
            if(tomaEnCuentaParams)
            {
                if(resp.Length >= this.min && numLineas(resp)<= this.fil)
                {
                    return true;
                }
                return false;
            }
            else
            {
                return true;
            }
        }

        private int numLineas(String resp)
        {
            String[] arr = resp.Split('\n');

            return arr.Length;
        }
    }
}
