using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XForms.GramaticaIrony;

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

        public static void setUp(RichTextBox cons)
        {
            consolaSalida = cons;
            errores = new List<TError>();
        }

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
    }
}
