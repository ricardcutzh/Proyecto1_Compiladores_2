using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.ASTTree.Interfaces;
using XForms.Simbolos;

namespace XForms.ASTTree.Instrucciones
{
    class DeclaracionMain : NodoAST, Instruccion
    {
        Principal principal;

        public DeclaracionMain(Principal principal, int linea, int columna, String clase):base(linea, columna, clase)
        {
            this.principal = principal;
        }

        public object Ejecutar(Ambito ambito)
        {
            throw new NotImplementedException();
        }
    }
}
