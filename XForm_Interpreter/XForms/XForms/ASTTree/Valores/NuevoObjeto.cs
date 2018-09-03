using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.ASTTree.Interfaces;
using XForms.Simbolos;

namespace XForms.ASTTree.Valores
{
    class NuevoObjeto : NodoAST, Expresion
    {
        String tipo;
        List<Expresion> parametros;
        public NuevoObjeto(List<Expresion> parametros, String tipo, int linea, int col, String clase):base(linea, col, clase)
        {
            this.tipo = tipo;
            this.parametros = parametros;
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
