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
using XForms.Simbolos;

namespace XForms.GUI.Select
{
    partial class Seleccionar : Form
    {
        Pregunta p;

        public String respuesta = "";
        public object salir = null;

        int tipo;

        int linea, col;
        String clase, archivo;

        List<NodoSelecciona> opcs;

        List<String> seleccion;

        public Seleccionar(Pregunta p, int tipo, Opciones listado, int linea, int col, String clase, String archivo)
        {
            this.p = p;

            this.tipo = tipo;

            this.linea = linea;
            this.col = col;
            this.clase = clase;
            this.archivo = archivo;
            this.seleccion = new List<string>();

            InitializeComponent();

            if (this.p.lectura == true)
            {
                this.button1.Enabled = false;
            }

            opcs = listado.getSelecciones();

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

            this.Options.View = View.Details;
            this.Options.Columns.Add("Selecciona de Las Opciones", 400);

            llenaOpciones();
        }

        private void llenaOpciones()
        {
            ImageList imgs = new ImageList();
            imgs.ImageSize = new Size(75, 75);
            foreach(NodoSelecciona n in this.opcs)
            {
                try
                {
                    imgs.Images.Add(Image.FromFile(getRealPath(n.ruta)));
                }
                catch
                {
                    imgs.Images.Add(Properties.Resources.if_shield_error_299056);
                }
            }

            this.Options.SmallImageList = imgs;
            int x = 0;
            foreach(NodoSelecciona n in this.opcs)
            {
                this.Options.Items.Add(n.etiqueta, x);
                x++;
            }
        }


        private String getRealPath(String path)
        {
            if (path.Equals("")) { return ""; }
            String currentDir = Estatico.PROYECT_PATH + "\\";
            if (System.IO.File.Exists(path))
            {
                return path;
            }
            if(System.IO.File.Exists(currentDir + path))
            {
                return currentDir + path;
            }
            else
            {
                 
                TError error = new TError("Semantico", "Para listado de Opciones, No existe la imagen: " + path + " | Clase: " + clase + " | Archivo: " + archivo, linea, col, false);
                Estatico.ColocaError(error);
                Estatico.errores.Add(error);
            }
            return "";
        }




        private void button1_Click(object sender, EventArgs e)
        {
            //CONTESTAR
            String s = getSeleccion();
            if(!s.Equals("Sin Seleccionar")) { this.respuesta = s; }
            compruebaRequerido(null);
        }

        private void label1_Click(object sender, EventArgs e)
        {

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

        private void Options_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(this.Options.SelectedItems.Count == 0) { return; }
            String i = this.Options.SelectedItems[0].SubItems[0].Text;
            agregarRespuesta(i);
            this.selecionadas.Text = getSeleccion();
        }

        private String getSeleccion()
        {

            if(this.seleccion.Count == 0) { return "Sin Seleccionar"; }
            String cad = "";
            foreach(String s in this.seleccion)
            {
                cad = cad + "{" + s + "}";
            }

            return cad;
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


        private void agregarRespuesta(String val)
        {
            if(tipo == 1)
            {
                this.seleccion.Clear();
                this.seleccion.Add(val);
            }
            else if(tipo == 0)
            {
                Boolean remueve = false;
                int x = 0;
                foreach(String s in this.seleccion)
                {
                    if(s.Equals(val))
                    {
                        remueve = true;
                        break;
                    }
                    x++;
                }

                if(remueve)
                {
                    this.seleccion.RemoveAt(x);
                }
                else
                {
                    this.seleccion.Add(val);
                }

            }
        }
    }
}
