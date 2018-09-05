using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.Objs;

namespace XForms.Simbolos
{
    class Variable:Simbolo
    {
        public Object valor { get; set; }//VALOR QUE TIENE LA VARIABLE

        public Variable(String idVariable, String Tipo, Estatico.Vibililidad Visibilidad, Object Valor):base(idVariable, false, Visibilidad, Tipo)
        {
            this.valor = Valor;
        }

    }
}
