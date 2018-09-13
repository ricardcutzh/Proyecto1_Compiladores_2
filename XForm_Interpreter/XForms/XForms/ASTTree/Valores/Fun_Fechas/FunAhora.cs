using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.GramaticaIrony;
using XForms.Objs;
using XForms.ASTTree.Interfaces;
using XForms.Simbolos;


namespace XForms.ASTTree.Valores.Fun_Fechas
{
    class FunAhora:NodoAST, Expresion
    {

        public FunAhora(String clase, int linea, int col):base(linea, col, clase)
        {

        }

        public string getTipo(Ambito ambito)
        {
            return "fechahora";
        }

        public object getValor(Ambito ambito)
        {
            try
            {
                DateTime ahora = DateTime.Now;
                return ahora;
            }
            catch (Exception e)
            {
                TError error = new TError("Ejecucion", "Error al ejecutar la funcion ahora() | Clase: " + this.clase + " | Archivo: " + ambito.archivo + " | Mensaje: " + e.Message, linea, columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            return new Nulo();
        }
    }
}
