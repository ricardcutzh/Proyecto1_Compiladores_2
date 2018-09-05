using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XForms.Simbolos
{
    class Objeto
    {
        Ambito ambito;
        String idClase;

        public Objeto(String idClase, Ambito ambito)
        {
            this.idClase = idClase;
            this.ambito = ambito;
        }


    }
}
