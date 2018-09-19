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
namespace XForms.GUI.Ficheros
{
    partial class Fichero : Form
    {
        String salida = "";

        Pregunta p;

        public String rutaArchivo = "";

        String filtros;

        int linea, col;

        String clase, Archivo;

        public Fichero(Pregunta p, String filtros, int linea, int col, String clase, String archivo)
        {
            this.linea = linea;
            this.col = col;
            this.clase = clase;
            this.Archivo = archivo;

            this.filtros = filtros;
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

            Salida.DocumentText = Estatico.header() +  salida + Estatico.footer();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog abrir = new OpenFileDialog();
            String destino = System.IO.Directory.GetCurrentDirectory() + "\\FILES\\";

            if(!this.filtros.Equals(""))
            {
                try
                {
                    abrir.Filter = getFilters();
                }
                catch(Exception ex)
                {
                    TError error = new TError("Semantico", "La definicion de Extensiones no es correcta: \'" + getFilters() + "\' | Clase: "+clase+" | Archivo: "+Archivo, linea, col, false);
                    Estatico.errores.Add(error);
                    Estatico.ColocaError(error);
                }
            }
            if(abrir.ShowDialog()==DialogResult.OK)
            {
                String source = abrir.FileName;

                System.IO.File.Move(source, destino+abrir.SafeFileName);
                this.rutaArchivo = destino + abrir.SafeFileName;

                if (this.rutaArchivo.Equals("") && p.requerido)
                {
                    MessageBox.Show("Campo requerido", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    /*AQUI TENGO QUE PONER LA RUTA DEL ARCHIVO!!!*/
                    MessageBox.Show("Archivo Cargado a: " + this.rutaArchivo + ", con exito!","Archivo Cargado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
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


        private String getFilters()
        {
            this.filtros = this.filtros.Replace(" ", "");
            String[] arr = this.filtros.Split('.');
           
            String filter = "";
            int x = 0;
            foreach(String s in arr)
            {
                if(!s.Equals(""))
                {
                    String aux = "";
                    if(x==0)
                    {
                        aux = s + " Files (*." + s + ")|*." + s;
                    }
                    else
                    {
                        aux = "| "+ s + " Files (*." + s + ")|*." + s;
                    }
                    
                    filter = filter + aux;
                    x++;
                }
                
            }
            return filter;
        }
    }
}
