using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.GramaticaIrony;
using XForms.Objs;
using XForms.ASTTree.Interfaces;
using XForms.Simbolos;

namespace XForms.ASTTree.Sentencias
{
    class LLamadaFuncion:NodoAST, Instruccion
    {
        Expresion call;

        public LLamadaFuncion(String clase, int linea, int col, Expresion llamada):base(linea, col, clase)
        {
            this.call = llamada;
        }

        public object Ejecutar(Ambito ambito)
        {
            try
            {
                call.getValor(ambito);

                return new Nulo();
            }
            catch
            {
                TError error = new TError("", "Error en la ejecucion de la llamada a funcion | Clase: " + this.clase + " | Archivo: " + ambito.archivo, linea, columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            return new Nulo();
        }
    }
}
