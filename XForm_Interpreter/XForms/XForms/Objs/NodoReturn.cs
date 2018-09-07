using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XForms.Objs
{
    class NodoReturn
    {
        public Object valor;
        public String tipo;

        public NodoReturn(Object valor, String tipo)
        {
            this.valor = valor;
            this.tipo = tipo;
        }
    }
}
