using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.ASTTree.Interfaces;
using XForms.Simbolos;

namespace XForms.ASTTree.Sentencias
{
    class Continuar : Instruccion
    {
        public object Ejecutar(Ambito ambito)
        {
            return new Continuar();
        }
    }
}
