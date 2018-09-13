using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.GramaticaIrony;
using XForms.Objs;
using XForms.ASTTree.Interfaces;
using XForms.Simbolos;

namespace XForms.ASTTree.Valores.Func_Numericas
{
    class Pi:NodoAST, Expresion
    {
        public Pi(String clase, int linea, int col) : base(linea, col, clase)
        {

        }

        public string getTipo(Ambito ambito)
        {
            return "decimal";
        }

        public object getValor(Ambito ambito)
        {
            try
            {
                return Math.Round(Math.PI, 5);
            }
            catch(Exception e)
            {
                TError error = new TError("Ejecucion", "Error al ejecutar la funcion Pi() | Clase: " + this.clase + " | Archivo: " + ambito.archivo + " | Mensaje: " + e.Message, linea, columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            return 0.0;
        }
    }
}
