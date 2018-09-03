using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.ASTTree.Interfaces;
using XForms.Simbolos;

namespace XForms.ASTTree.Valores
{
    class Dimensiones : NodoAST, Expresion
    {
        List<Expresion> dimensiones;

        public Dimensiones(List<Expresion> dimensiones, int linea, int col, String clase):base(linea, col, clase)
        {
            this.dimensiones = dimensiones;
        }

        public Dimensiones(int linea, int col, String clase):base(linea, col, clase)
        {
            this.dimensiones = new List<Expresion>();
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
