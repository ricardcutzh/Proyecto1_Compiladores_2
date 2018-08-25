using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XForms.GramaticaIrony
{
    class TError
    {
        String Tipo  { get; }
        String Mensaje { get; }
        int Linea { get; }
        int Columna { get; }

        public TError(String tipo, String mensaje, int linea, int columna)
        {
            this.Tipo = tipo;
            this.Mensaje = mensaje;
            this.Linea = linea;
            this.Columna = columna;
        }


    }
}
