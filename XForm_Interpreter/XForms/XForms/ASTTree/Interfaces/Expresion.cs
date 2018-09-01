using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.Simbolos;

namespace XForms.ASTTree.Interfaces
{
    interface Expresion
    {
        /*VA A AYUDAR PARA EL MANEJO DE TIPOS*/
        String getTipo(Ambito ambito);

        /*VA AYUDAR AL MANEJO DE VALORES DE LA EXPRESION QUE SE MANDE A LLAMAR*/
        Object getValor(Ambito ambito);
    }
}
