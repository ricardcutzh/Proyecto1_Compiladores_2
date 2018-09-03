using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.ASTTree.Interfaces;
using XForms.Simbolos;

namespace XForms.ASTTree.Valores
{
    class ValorArreglo : NodoAST, Expresion
    {

        List<Expresion> expresiones;

        public ValorArreglo(List<Expresion> expresiones, int linea, int col, String clase):base(linea, col, clase)
        {
            this.expresiones = expresiones;
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
