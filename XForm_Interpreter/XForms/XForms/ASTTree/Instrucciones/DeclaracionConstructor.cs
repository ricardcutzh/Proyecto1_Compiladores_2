using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.ASTTree.Interfaces;
using XForms.Objs;
using XForms.Simbolos;

namespace XForms.ASTTree.Instrucciones
{
    class DeclaracionConstructor : NodoAST, Instruccion
    {
        List<Instruccion> instrucciones;

        public List<NodoParametro> parametros;

        public DeclaracionConstructor(List<Instruccion> instrucciones, List<NodoParametro> parametros,  int linea, int col, String clase):base(linea,col, clase)
        {
            this.instrucciones = instrucciones;
            this.parametros = parametros;
        }

        public object Ejecutar(Ambito ambito)
        {
            throw new NotImplementedException();
        }
    }
}
