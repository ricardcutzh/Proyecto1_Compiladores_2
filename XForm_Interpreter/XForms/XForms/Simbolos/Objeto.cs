using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XForms.Simbolos
{
    class Objeto
    {
        public Ambito ambito;
        public String idClase;

        public Objeto(String idClase, Ambito ambito)
        {
            this.idClase = idClase;
            this.ambito = ambito;
            
        }

        public override string ToString()
        {
            String c = "Objeto: " + this.idClase;
            return c;
        }
    }
}
