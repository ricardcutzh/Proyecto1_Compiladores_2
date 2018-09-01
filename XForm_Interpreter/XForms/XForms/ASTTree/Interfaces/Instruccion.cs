using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.Simbolos;
namespace XForms.ASTTree.Interfaces
{
    interface Instruccion
    {
        /*METODO QUE SE VA IMPLEMENTAR EN LAS CLASES PARA LLEVAR A CABO LA EJECUCION*/
        Object Ejecutar(Ambito ambito);
    }
}
