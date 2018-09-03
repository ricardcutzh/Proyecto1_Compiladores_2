using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.ASTTree.Interfaces;
using XForms.Simbolos;

namespace XForms.ASTTree.Valores
{
    class NuevoArreglo : NodoAST, Expresion
    {
        Dimensiones dimensiones;
        String tipo;

        public NuevoArreglo(Dimensiones dimensiones, String tipo, int linea, int col, String clase):base(linea, col, clase)
        {
            this.dimensiones = dimensiones;
            this.tipo = tipo;
        }

        public string getTipo(Ambito ambito)
        {
            throw new NotImplementedException();
        }

        public object getValor(Ambito ambito)
        {
            throw new NotImplementedException();
        }
    }
}
