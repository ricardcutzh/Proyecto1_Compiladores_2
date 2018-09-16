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

        /*RUTA DEL PROYECTO EN CUESTION*/
        public static String PROYECT_PATH = "";
        
        public static int tolerancia = 5;

        public static ListaClases clasesDisponibles;

        public static void setUp(RichTextBox cons)
        {
            consolaSalida = cons;
            consolaSalida.Text = ">> XForm Console | Compiladores 2 | 2018";
            errores = new List<TError>();
            clasesDisponibles = new ListaClases();
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
    }
}
