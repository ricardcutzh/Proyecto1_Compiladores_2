using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.ASTTree.Interfaces;
using XForms.Simbolos;
using XForms.Objs;

namespace XForms.ASTTree.Instrucciones
{
    class DeclaracionFuncion : NodoAST, Instruccion
    {
        List<Instruccion> instrucciones;
        List<NodoParametro> parametros;

        public DeclaracionFuncion(List<Instruccion> instrucciones, List<NodoParametro>parametros, Estatico.Vibililidad vibililidad, String idfuncion, int linea, int col, String clase):base(linea, col, clase)
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
