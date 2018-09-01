using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.Objs;
namespace XForms.Simbolos
{
    class Funcion
    {

        /*VA A TENER UNA LISTA DE PARAMETROS*/
        /*VA A TENER UNA LISTA DE INSTRUCCIONES QUE VA A AJECUTAR*/
        /*VA A HEREDAR DE LA CLASE INSTRUCCION PARA PODER IMPLEMENTAR LA EJECUCCION*/

        public String idFuncion { get; }
        public String Tipo { get; }
        public Estatico.Vibililidad Vibililidad { get; }

        public Funcion(String idFuncion, String Tipo, Estatico.Vibililidad Visibilidad)
        {
            this.idFuncion = idFuncion;
            this.Tipo = Tipo;
            this.Vibililidad = Visibilidad;
        }
    }
}
