using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.Objs;
using XForms.ASTTree.Interfaces;
using XForms.ASTTree.Valores;
namespace XForms.Simbolos
{
    class Funcion : NodoAST, Instruccion
    {

        /*VA A TENER UNA LISTA DE PARAMETROS*/
        /*VA A TENER UNA LISTA DE INSTRUCCIONES QUE VA A AJECUTAR*/
        /*VA A HEREDAR DE LA CLASE INSTRUCCION PARA PODER IMPLEMENTAR LA EJECUCCION*/

        public String idFuncion { get; }
        public String Tipo { get; }
        public Estatico.Vibililidad Vibililidad { get; }
        List<Expresion> parametros;

        public Funcion(List<Expresion> parametros, String idFuncion, String Tipo, Estatico.Vibililidad Visibilidad, int linea, int col, String clase) : base(linea, col, clase)
        {
            this.idFuncion = idFuncion;
            this.Tipo = Tipo;
            this.Vibililidad = Visibilidad;
            this.parametros = parametros;
        }

        public object Ejecutar(Ambito ambito)
        {
            throw new NotImplementedException();
        }
    }
}
