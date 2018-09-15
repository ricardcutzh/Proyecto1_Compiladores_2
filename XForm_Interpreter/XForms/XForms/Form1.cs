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
using XForms.Simbolos;
using FastColoredTextBoxNS;
using System.Text.RegularExpressions;

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
                //crearNuevaPestania(nombre, "// NUEVA PESTANIA");
                crearNuevaPestaniaColor(nombre, "$$ARCHIVO NUEVO\nclase "+nombre+" \n{\n\tPrincipal()\n\t{\n\t\tmensajes(\"hola mundo!\");\n\t}\n}");
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
                    crearNuevaPestaniaColor(abrir.SafeFileName.Replace(".xform",""), texto);
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
                    //crearNuevaPestania(abrir.SafeFileName.Replace(".xform", ""), texto);
                    crearNuevaPestaniaColor(abrir.SafeFileName.Replace(".xform", ""), texto);
                }
            }
        }

        //PERMITE LA GENERACION DEL FORMULARIO
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            StatusControl.Text = "Estatus";
            Estatico.setUp(Consola);
            Hashtable clasesPreanalizadas = new Hashtable();
            if(Editor.TabCount > 0)
            {
                //RichTextBox principal = (RichTextBox)Editor.TabPages[Editor.SelectedIndex].Controls[0].Controls[1];
                FastColoredTextBox principal = (FastColoredTextBox)Editor.TabPages[Editor.SelectedIndex].Controls[0].Controls[0];
                String cadena = principal.Text.ToLower();//PARA QUE TODO ESTE EN MINUSCULAS Y NO TENGA CLAVOS CON LA COMPROBACION DE NOMBRES
                Progreso.Value = 40;
                StatusControl.Text = "Iniciando Proceso...";
                System.Threading.Thread.Sleep(200);
                Analizador an = new Analizador(cadena, this.ProyectoPath, archivo);
                if(an.analizar())//SI SE ANALIZA LA CADENA...
                {
                    // PROCEDE A INTENTAR CAPTURAR LA INFO...
                    if(Estatico.NumeroErroes()> 0) //SO EXISTEN ERRORES ANTES DE CAPTURAR LA INFO...
                    {
                        //REVISAR...
                        Progreso.Value = 0;
                        StatusControl.Text = "Proceso Interrumpido!";
                        Estatico.consolaSalida.AppendText("\n>> Proceso detenido, Errores detectados...");
                        MessageBox.Show("Existen: "+Estatico.NumeroErroes()+" en La cadena! Revisalos en el reporte", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else//SINO PROCEDER A CAPTURA LA INFORMACION
                    {
                        CapturarInformacion(an, clasesPreanalizadas);
                        ///////////////////////////////////////////////////////
                        Progreso.Value = 100;
                        StatusControl.Text = "Proceso Terminado!";
                        System.Threading.Thread.Sleep(200);
                        Progreso.Value = 0;
                        ///////////////////////////////////////////////////////
                        //AQUI DEBO DE PREGUNTAR SI EN CASO HAY UN PROBLEMA CON LAS ADVERTENCIAS
                    }
                }
                else//SI NO SE PUEDE, ENTONCES EL ERROR ES FATAL... REVISAR
                {
                    MessageBox.Show("No se logro Analizar la cadena de entrada", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                errorLabel.Text = Convert.ToString(Estatico.NumeroErroes());
                warningsLabel.Text = Convert.ToString(Estatico.NumeroAdvertencias());
            }
        }

        //PERMITE VER LOS FORMULARIOS
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            /*String mihora = "12/10/96 12:34:00";
            DateTime p = DateTime.ParseExact(mihora, "dd/MM/yy hh:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            MessageBox.Show(p.Day.ToString());
            TError errorp = new TError("Prueba", "Este es un error de prueba", 1, 1, false);
            Estatico.ColocaError(errorp);
            crearNuevaPestaniaColor("prueba", "hola mundo");*/
            Object ob = "hola mundo!";
            Object ob1 = "hola mundo!";

            if(ob.Equals(ob1))
            {
                MessageBox.Show("iguales");
            }
        }

        #endregion

        #region PROCESO
        private void CapturarInformacion(Analizador an, Hashtable clasesPreanalizadas)
        {
            List<ClasePreAnalizada> clases = an.clases;
            Progreso.Value = 80;
            System.Threading.Thread.Sleep(200);
            foreach (ClasePreAnalizada a in clases)
            {
                if (!clasesPreanalizadas.Contains(a.id))
                {
                    Clase aux = a.obtenerClase();
                    clasesPreanalizadas.Add(a.id, a);//METO LAS CLASES AL HASHTABLE PARA LUEGO LAS PREANALIZADAS LLEVARLAS A ANALIZARLAS
                    Estatico.clasesDisponibles.addClass(aux);
                }
                else
                {
                    Estatico.errores.Add(new TError("Advertencia", "Se encontro una nueva definicion de la clase: " + a.id + " En el archivo: " + a.archivoOringen + ", Por lo que Se descarto", 0, 0, true));
                }
            }
            if (Estatico.NumeroErroes() > 0)//SI HAY ERRORES REVISAR
            {
                MessageBox.Show("Existen: " + Estatico.NumeroErroes() + " en La cadena! Revisalos en el reporte", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                //EN ESTE PUNTO YA TENGO LAS CLASES CON SU AST..... AHORA DEBERIA DE EMPEZAR LA EJECUCION DEL PROGRAMA
                Clase inicio = Estatico.clasesDisponibles.getFirstClassWithMain();
                if(inicio!=null)
                {
                    Ambito am = new Ambito(null, inicio.idClase.ToLower(), inicio.ArchivoOrigen);
                    
                    am = (Ambito)inicio.Ejecutar(am);
                    inicio.ejecutaMain(am);

                    //am.ImprimeAmbito();
                    
                    /*List<NodoParametro> parametros = new List<NodoParametro>();
                    NodoParametro m = new NodoParametro("m", "booleano", false);
                    parametros.Add(m);
                    ClaveFuncion n = new ClaveFuncion("funcion1", "vacio", parametros);
                    Funcion ej = am.GetFuncion(n);*/
                    //MessageBox.Show("FUNCION RECUPERADA: " + ej.ToString());
                    //inicio.ejecutaMain(am);
                }
            }
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

        private void crearNuevaPestaniaColor(String nombre, String contenido)
        {
            //CREANDO UNA NUEVA TAB
            TabPage nueva = new TabPage(nombre + ".xform");

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
            //RichTextBox textEditor = new RichTextBox(); //EDITOR DE TEXTO
            //RichTextBox numl = new RichTextBox(); //CONTADOR DE LINEAS

            FastColoredTextBox editor = new FastColoredTextBox();

            editor.SetBounds(10, 0, Editor.Width - 20, Editor.Height - 30);
            editor.AcceptsTab = true;
            editor.Font = new Font("Consolas", 9);
            editor.WordWrap = false;
            editor.TextChanged += new EventHandler<TextChangedEventArgs>(fastColoredTextBox1_TextChanged);
            editor.Text = contenido.ToLower();

            pan.Controls.Add(editor);
            nueva.Controls.Add(pan);// posicion 0 del tabpage

            //AGREGANDO AL TAB CONTROL
            Editor.TabPages.Add(nueva);
        }


        Style GreenStyle = new TextStyle(Brushes.Green, null, FontStyle.Italic);
        Style BlueStyle = new TextStyle(Brushes.Blue, null, FontStyle.Bold);
        Style LightBlue = new TextStyle(Brushes.DarkViolet, null, FontStyle.Bold);
        Style StringStyle = new TextStyle(Brushes.Brown, null, FontStyle.Regular);
        Style StringStyle1 = new TextStyle(Brushes.DarkOrange, null, FontStyle.Regular);
        Style functionStyle = new TextStyle(Brushes.DarkOliveGreen, null, FontStyle.Underline);
        private void fastColoredTextBox1_TextChanged(Object sender, TextChangedEventArgs e)
        {
           
            e.ChangedRange.ClearStyle(BlueStyle);
            e.ChangedRange.ClearStyle(LightBlue);
            e.ChangedRange.ClearStyle(StringStyle);
            e.ChangedRange.ClearFoldingMarkers();
            //set folding markers
            e.ChangedRange.SetFoldingMarkers("{", "}");
            e.ChangedRange.SetStyle(StringStyle, "\"((\\.)|[^\\\\\"])*\"");
            e.ChangedRange.SetStyle(StringStyle1, "\'((\\.)|[^\\\\\"])*\'");

            e.ChangedRange.SetStyle(GreenStyle, @"\$\$.*$", RegexOptions.Multiline);

            e.ChangedRange.SetStyle(BlueStyle, @"\b(publico|privado|protegido|clase|padre|principal|imprimir|nuevo|este|retorno|romper|nulo|importar|sino|si|verdadero|falso)\b", RegexOptions.IgnoreCase);
            e.ChangedRange.SetStyle(BlueStyle, @"\b(Publico|Privado|Protegido|Clase|Padre|Principal|Imprimir|Nuevo|Este|Retorno|Romper|Nulo|Importar|SiNo|Si|Verdadero|Falso)\b", RegexOptions.IgnoreCase);

            e.ChangedRange.SetStyle(BlueStyle, @"\b(super|Mientras|continuar|hacer|Repetir|Hasta|para|caso|defecto|pregunta|formulario|grupo|mensajes|)\b", RegexOptions.IgnoreCase);

            e.ChangedRange.SetStyle(functionStyle, @"\b(imagen|video|audio|subcad|poscad|tam|random|max|min|pow|Log|Log10|abs|sin|cos|tan|sqrt|pi|hoy|ahora|buscar|insertar|obtener)\b", RegexOptions.IgnoreCase);

            e.ChangedRange.SetStyle(LightBlue, @"\b(cadena|entero|decimal|booleano|fecha|hora|fechahora|vacio|respuestas|opciones)\b", RegexOptions.IgnoreCase);


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

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if(Estatico.NumeroErroes() > 0 || Estatico.NumeroAdvertencias() > 0)
            {
                ErrorReport errorRep = new ErrorReport(Estatico.errores);
                bool aux = errorRep.writeReport();
                if(aux)
                {
                    errorRep.openReport();
                }
                else
                {
                    MessageBox.Show("No se Pudo generar el Reporte!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("No Existen Errores o Advertencias", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void erroresSemanticosPermitidosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String cantidadErrores = Microsoft.VisualBasic.Interaction.InputBox("Tolerancia de Errores Semanticos Permitidos: ", "Configuracion Errores Semanticos", "", 100, 100);
            int cantidad = 1;
            try
            {
                cantidad = Convert.ToInt32(cantidadErrores);
            }
            catch
            {
                cantidad = 5;
            }

            Estatico.tolerancia = cantidad;
        }



    }
}
