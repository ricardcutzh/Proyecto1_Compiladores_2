using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.ASTTree.Interfaces;

namespace XForms.Simbolos
{
    class Principal:NodoAST, Instruccion
    {
        List<Instruccion> instrucciones;

        public Principal(List<Instruccion> instrucciones, int linea, int col, String clase):base(linea, col, clase)
        {
            this.instrucciones = instrucciones;
        }

        public object Ejecutar(Ambito ambito)
        {
            throw new NotImplementedException();
        }
    }
}
