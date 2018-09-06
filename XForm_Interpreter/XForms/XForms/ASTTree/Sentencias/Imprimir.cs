using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.ASTTree.Interfaces;
using XForms.Simbolos;
using XForms.GramaticaIrony;
using XForms.Objs;

namespace XForms.ASTTree.Sentencias
{
    class Imprimir : NodoAST, Instruccion
    {
        Expresion expresion;
        public Imprimir(String clase, int linea, int col, Expresion exp):base(linea,col, clase)
        {
            this.expresion = exp;
        }


        object Instruccion.Ejecutar(Ambito ambito)
        {
            try
            {
                Object valor = expresion.getValor(ambito);
                Estatico.imprimeConsola(valor.ToString());
            }
            catch(Exception ex)
            {
                TError error = new TError("Ejecucion", "Error al Imprimir en Clase: " + this.clase + " | Archivo: " + ambito.archivo+ " | "+ex.Message, this.linea, this.columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            return null;
        }
    }
}
