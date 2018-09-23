using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XForms.GramaticaIrony;
using System.Collections;
using XForms.Simbolos;
using System.Drawing;


namespace XForms.Objs
{
    class Estatico
    {
        /*
         * @class   Estatico
         * 
         * @brief   Va a manejar los elementos estaticos de la ejecuccion del programa
         * 
         * @author  Ricardo Antonio Cutz Hernandez
         * 
         */

        /*CONTADOR DE PREGUNTAS*/

        public static int contador = 1;

        public static int numPregunta = 1;

        /*RUTA DEL PROYECTO EN CUESTION*/
        public static String PROYECT_PATH = "";
        
        public static int tolerancia = 5;

        public static ListaClases clasesDisponibles;

        public static List<PreguntaAlmacenada> resps;

        public static void setUp(RichTextBox cons)
        {
            consolaSalida = cons;
            consolaSalida.Text = ">> XForm Console | Compiladores 2 | 2018";
            errores = new List<TError>();
            clasesDisponibles = new ListaClases();
            Estatico.contador = 1;
            resps = new List<PreguntaAlmacenada>();
            numPregunta = 1;
        }
        /*AMBITO ESTATICO PAR ACCEDER A LOS PARAMETROS*/
        public static Ambito temporal;

        /*CONSOLA DE SALIDA DEL PROGRAMA */
        public static RichTextBox consolaSalida;

        /*LISTADO DE ERRORES ENCONTRADOS DURANTE LA EJECUCION*/
        public static List<TError> errores;

        /*VISISBILIDAD DE LAS PROPIEDADES*/
        public enum Vibililidad
        {
            PUBLICO,
            PRIVADO,
            PROTEGIDO,
            LOCAL /*SOLO SI LA VARIABLE ES INICIALIZADA LOCALMENTE*/
        };

        public enum Operandos
        {
            SUMA,
            RESTA,
            MULT,
            DIVI,
            POT,
            MOD,
            INC,
            DEC,
            MENOR,
            MAYOR,
            MENORIGUAL,
            MAYORIGUAL,
            IGUAL,
            DIFERENTE,
            AND,
            OR,
            NOT
        }

        public static int NumeroErroes()
        {
            int x = 0;
            foreach(TError e in errores)
            {
                if (!e.esAdvertencia)
                {
                    x++;
                }
            }
            return x;
        }

        public static int NumeroAdvertencias()
        {
            int x = 0;
            foreach(TError e in errores)
            {
                if(e.esAdvertencia)
                {
                    x++;
                }
            }
            return x;
        }

        public static Boolean paraEjecucionPorCantidadErrores()
        {
            if(NumeroErroes() > tolerancia)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void ColocaError(TError error)
        {
            String salida = "\n>> {{ERROR! : Tipo: "+error.Tipo+" | "+error.Mensaje + " | Linea: " + (error.Linea+1) + " , Columna: " + error.Columna+"}}";
            consolaSalida.AppendText(salida);
            int index = consolaSalida.Text.IndexOf(salida);
            int tam = salida.Length;
            consolaSalida.Select(index, tam);
            consolaSalida.SelectionColor = Color.Red;
        }

        public static void imprimeConsola(String mensaje)
        {
            mensaje = "\n>> " + mensaje;
            consolaSalida.AppendText(mensaje);
            int index = consolaSalida.Text.IndexOf(mensaje);
            int tam = mensaje.Length;
            consolaSalida.Select(index, tam);
            consolaSalida.SelectionColor = Color.White;
        }



        public static String header()
        {
            return "<html lang='en'><head> <meta charset='utf-8'><meta name='viewport' content='width=device-width, initial-scale=1, shrink-to-fit=no'><link rel='stylesheet' href='https://stackpath.bootstrapcdn.com/bootstrap/4.1.3/css/bootstrap.min.css' integrity='sha384-MCw98/SFnGE8fJT3GXwEOngsV7Zt27NXFoaoApmYm81iuXoPkFOJwJ8ERdknLPMO' crossorigin='anonymous'><title>Hello, world!</title></head> <body style='background-color:#80bfff;'> <div class='container'>";
        }

        public static String footer()
        {
            return "</div></body></ html >";
        }

        public static String toHTMLCard(String mensaje)
        {
            String cad = "<br><div class='card'> <div class='card-body'> <center>" + mensaje + "</center> </div> </div>";
            return cad; 
        }

        public static String toHTMLTitle(String mensaje)
        {
            return "<center> <h1> " + mensaje + "</h1> </center>";
        }

        public static String toHTMLAlert(String mensaje)
        {
            return "<div class='alert alert-danger' role='alert'>" + mensaje + "</div>";
        }

        public static Object casteaRespuestaA(String resp, Object valorResp, Type tipo)
        {
            try
            {
                if(tipo.Equals(typeof(int)))
                {
                    int aux = Convert.ToInt32(resp);
                    valorResp = aux;
                    return valorResp;
                }
                else if(tipo.Equals(typeof(String)))
                {
                    valorResp = resp;
                    return valorResp;
                }
                else if(tipo.Equals(typeof(double)))
                {
                    double aux = Convert.ToDouble(resp);
                    valorResp = aux;
                    return valorResp;
                }
                else if(tipo.Equals(typeof(Boolean)))
                {
                    Boolean aux = Convert.ToBoolean(resp);
                    valorResp = aux;
                    return valorResp;
                }
                else if(tipo.Equals(typeof(DateTime)))
                {
                    DateTime val = DateTime.ParseExact(resp, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                    valorResp = val;
                    return valorResp;
                }
                else if(tipo.Equals(typeof(Date)))
                {
                    DateTime val = DateTime.ParseExact(resp, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                    Date fecha = new Date(val);
                    valorResp = fecha;
                    return valorResp;
                }
                else if(tipo.Equals(typeof(Hour)))
                {
                    DateTime val = DateTime.ParseExact(resp, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                    Hour hora = new Hour(val);
                    valorResp = hora;
                    return valorResp;
                }
            }
            catch
            {

            }
            return null;
        }

        public static Object respuestaPorDefecto(Type tipo)
        {
            try
            {
                if (tipo.Equals(typeof(int)))
                {
                    return 0;
                }
                else if (tipo.Equals(typeof(String)))
                {
                    return "";
                }
                else if (tipo.Equals(typeof(double)))
                {
                    return 0.0;
                }
                else if (tipo.Equals(typeof(Boolean)))
                {
                    return true;
                }
                else if (tipo.Equals(typeof(DateTime)))
                {
                    return new DateTime();
                }
                else if (tipo.Equals(typeof(Date)))
                {
                    return new Date(new DateTime());   
                }
                else if (tipo.Equals(typeof(Hour)))
                {
                    return new Hour(new DateTime());
                }
            }
            catch
            {

            }
            return new Nulo();
        }
    }
}
