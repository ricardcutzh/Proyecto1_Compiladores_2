using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using XForms.GramaticaIrony;
using XForms.Objs;
using System.Collections;

namespace XForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        String ProyectoPath;
        String archivo;

        /*
         * REGION DE LOS BOTONES
         * DE LA INTERFAZ
         */

        #region Botones
        //METODO DE LA CONSOLA...
        private void Consola_TextChanged(object sender, EventArgs e)
        {
            //NO HACE NADA :)
        }

        //PERMITE CREAR UNA NUEVA PESTANIA PARA EL EDITOR DE TEXTO
        private void nuevaPestanaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //PEDIRE EL NOMBRE DE LA PESTANIA
            //AGREGUE LA REFERENCIA A MICROSOFT.VISUALBASIC
            String nombre = Microsoft.VisualBasic.Interaction.InputBox("Nombre Del Archivo: ", "Nueva Pestana", "", 100, 100);
            if(nombre != "")
            {
                //AGREGO LA PESTANIA NUEVA!
                crearNuevaPestania(nombre, "// NUEVA PESTANIA");
            }
        }

        //PERMITE CERRAR LAS PESTANIAS
        private void cerrarPestanaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(Editor.TabPages.Count>0) //HAY PESTANIAS PARA CERRAR?
            {
                //SI SI... CIERRALA!
                Editor.TabPages.RemoveAt(Editor.SelectedIndex);
            }
            else
            {
                //SI NO.... 
                MessageBox.Show("No hay Pestanas Abiertas!", "Ups", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //PERMITE ABRIR UN ARCHIVO
        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //SE ENCARGA DE CARGAR EL ARCHIVO PARA ABRIRLOS
            Stream str;
            OpenFileDialog abrir = new OpenFileDialog();
            abrir.Filter = "xForm Files | *.xform";
            if(abrir.ShowDialog()== System.Windows.Forms.DialogResult.OK)
            {
                if((str = abrir.OpenFile())!=null)
                {
                    String archivo = abrir.FileName;
                    String texto = File.ReadAllText(archivo);
                    crearNuevaPestania(abrir.SafeFileName.Replace(".xform",""), texto);
                }
            }
        }

        //PERMITE GUARDAR EL ARCHIVO
        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        //PERMITE GUARDAR COMO
        private void guardarComoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        //DESPLIEGA EL MANUAL TECNICO
        private void manualTecnicoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        //DESPLIEGA EL MANUAL DE USUARIO
        private void manualDeUsuarioToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        //DESPLIEGA LA GRAMATICA UTILIZADA
        private void gramaticaUtilizadaToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        //PERMITE CARGAR UN ARCHIVO .XFORM
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //SE ENCARGA DE CARGAR EL ARCHIVO PARA ABRIRLOS
            Stream str;
            OpenFileDialog abrir = new OpenFileDialog();
            abrir.Filter = "xForm Files | *.xform";
            if (abrir.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if ((str = abrir.OpenFile()) != null)
                {
                    String archivo = abrir.FileName;
                    ////////////////////////////////
                    // AQUI TOMO EL PATH DE DONDE SE
                    // ABRIO EL ARCHIVO
                    this.ProyectoPath = archivo.Replace(abrir.SafeFileName,"");
                    this.archivo = archivo;
                    ////////////////////////////////
                    String texto = File.ReadAllText(archivo);
                    crearNuevaPestania(abrir.SafeFileName.Replace(".xform", ""), texto);
                }
            }
        }

        //PERMITE LA GENERACION DEL FORMULARIO
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Estatico.setUp(Consola);
            Hashtable clasesPreanalizadas = new Hashtable();
            if(Editor.TabCount > 0)
            {
                RichTextBox principal = (RichTextBox)Editor.TabPages[Editor.SelectedIndex].Controls[0].Controls[1];
                String cadena = principal.Text;
                //Analizador an = new Analizador(cadena);
                Analizador an = new Analizador(cadena, this.ProyectoPath, archivo);
                if(an.analizar())
                {
                    //AQUI DEBERIA DE TRAER LAS CLASES QUE SE LOGRARON CAPTURAR
                    List<ClasePreAnalizada> c = an.clases;
                    foreach(ClasePreAnalizada a in c)
                    {
                        if(!clasesPreanalizadas.Contains(a.id))
                        {
                            clasesPreanalizadas.Add(a.id, a);//METO LAS CLASES AL HASHTABLE PARA LUEGO LAS PREANALIZADAS LLEVARLAS A ANALIZARLAS 
                            //CREAR SU TABLA DE SIMBOLOS CON SUS FUNCIONES CORRESPONDIENTES
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No se logro Analizar la cadena de entrada", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if(Estatico.errores.Count>0)
                {
                    MessageBox.Show("Existen: "+Estatico.errores.Count()+" En la cadena de Entrada!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        //PERMITE VER LOS FORMULARIOS
        private void toolStripButton3_Click(object sender, EventArgs e)
        {

        }

        #endregion
        
        /*
         * CODIGO PARA PODER
         * GENERAR LAS PESTANIAS DE FORMA DINAMICA
         * 
         * CODIGO PARA PODER IMPLEMENTAR EL CONTADOR DE LAS LINEAS
         * 
         * CODIGO PARA PODER LLEVAR EL CONTROL DE LAS ACTUALIZACIONES DEL TEXTO
         */

        //GENERA UNA NUEVA PESTANIA
        private void crearNuevaPestania(String Nombre, String contenido)
        {
            //CREANDO UNA NUEVA TAB
            TabPage nueva = new TabPage(Nombre + ".xform");

            //CREANDO EL PANEL
            Panel pan = new Panel();
            pan.AutoScroll = false;
            pan.HorizontalScroll.Enabled = false;
            pan.HorizontalScroll.Visible = false;
            pan.HorizontalScroll.Maximum = 0;
            pan.AutoScroll = true;
            pan.SetBounds(0, 0, Editor.Width - 10, Editor.Height - 10);

            //AGREGANDO EL PANEL A LA TAB
            nueva.Controls.Add(pan);

            //CREANDO LOS RICHTEXTBOX
            RichTextBox textEditor = new RichTextBox(); //EDITOR DE TEXTO
            RichTextBox numl = new RichTextBox(); //CONTADOR DE LINEAS

            //CONTADOR DE LINEAS
            numl.ReadOnly = true;
            numl.Font = new Font("Consolas", 8);
            numl.SetBounds(0, 0, 30, Editor.Height - 30);
            numl.Text = "0";
            numl.WordWrap = false;
            numl.ScrollBars = RichTextBoxScrollBars.None;

            //EDITOR DE TEXTO
            textEditor.SetBounds(30, 0, Editor.Width - 60, Editor.Height - 30);
            textEditor.AcceptsTab = true;
            textEditor.Font = new Font("Consolas", 8);
            textEditor.WordWrap = false;
            textEditor.Text = contenido;
            //textEditor.TextChanged += new EventHandler(eventocambio); ESTE SERIA PARA PINTAR AUN FALTA REVISAR...
            textEditor.ScrollBars = RichTextBoxScrollBars.None;
            textEditor.KeyDown += esEnter;

            //AGREGANDO AL PANEL
            pan.Controls.Add(numl);//posición 0 del panel
            pan.Controls.Add(textEditor);// posición 1 del panel
            nueva.Controls.Add(pan);// posicion 0 del tabpage
            
            //AGREGANDO AL TAB CONTROL
            Editor.TabPages.Add(nueva);
        }

        //ACTUALIZA EL NUMERO DE LINEAS
        private void actualizaNumeroDeLineas(int tipo)
        {
            //OBTENIENDO EL LOS DOS RICHS DEL EDITOR PARA PODER ACTUALIZAR LAS LINEAS
            RichTextBox principal = (RichTextBox)Editor.TabPages[Editor.SelectedIndex].Controls[0].Controls[1];
            RichTextBox lineas = (RichTextBox)Editor.TabPages[Editor.SelectedIndex].Controls[0].Controls[0];

            lineas.Text = "";

            if (principal.Lines.Count() > 0)
            {
                int linenumber = principal.Lines.Count() * 16 ;
                if ( linenumber > Editor.TabPages[Editor.SelectedIndex].Height)
                {
                    principal.Height = linenumber;
                    lineas.Height = linenumber;
                }
                else
                {
                    principal.Height = Editor.Height - 30;
                    lineas.Height = Editor.Height - 30;
                }
                if(tipo == 1)
                {
                    for (int i = 0; i <= (linenumber / 16); i++)
                    {
                        lineas.Text = lineas.Text + i + "\n";
                    }
                }
                else if(tipo == 2)
                {
                    for (int i = 0; i < (linenumber / 16); i++)
                    {
                        lineas.Text = lineas.Text + i + "\n";
                    }
                }
            }

            principal.Focus();
        }

        //METODO QUE SE LE AGREGA AL RICH TEXTBOX PARA ACTUALIZAR EL NUMERO DE LINEAS
        private void esEnter(object sender, KeyEventArgs e)
        {
            if(e.KeyData == Keys.Enter)
            {
                actualizaNumeroDeLineas(1);
            }
            else if(e.KeyData == Keys.Back)
            {
                actualizaNumeroDeLineas(2);
            }
        }
        
    }
}
