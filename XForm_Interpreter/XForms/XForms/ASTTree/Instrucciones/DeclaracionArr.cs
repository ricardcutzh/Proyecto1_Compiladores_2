using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.ASTTree.Interfaces;
using XForms.Simbolos;
using XForms.ASTTree.Valores;
namespace XForms.ASTTree.Instrucciones
{
    class DeclaracionArr : NodoAST, Instruccion
    {
        Dimensiones dimensiones;
        Expresion ValorAsignado;
        String tipo;


        /*SI TUVIESE UN VALOR ASIGNADO*/
        public DeclaracionArr(Dimensiones dimensiones, String tipo, Expresion valorAsignado, int linea, int col, String clase):base(linea, col, clase)
        {
            this.dimensiones = dimensiones;
            this.ValorAsignado = valorAsignado;
            this.tipo = tipo;
        }

        /*SIN NO LO TIENE*/
        public DeclaracionArr(Dimensiones dimensiones, String tipo, int linea, int col, String clase):base(linea, col, clase)
        {
            this.dimensiones = dimensiones;
            this.ValorAsignado = null;
            this.tipo = tipo;
        }


        public object Ejecutar(Ambito ambito)
        {
            throw new NotImplementedException();
        }
    }
}
