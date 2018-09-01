using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XForms.GramaticaIrony
{
    class TError
    {
        public String Tipo  { get; }
        public String Mensaje { get; }
        public int Linea { get; }
        public int Columna { get; }
        public bool esAdvertencia { get; set; }
        public List<String> esperados { get; }

        public TError(String tipo, String mensaje, int linea, int columna)
        {
            this.Tipo = tipo;
            this.Mensaje = mensaje;
            this.Linea = linea;
            this.Columna = columna;
            this.esperados = new List<string>();
            esAdvertencia = false;
        }

        public TError(String tipo, String mensaje, int linea, int columna, Boolean esAdverencia)
        {
            this.Tipo = tipo;
            this.Mensaje = mensaje;
            this.Linea = linea;
            this.Columna = columna;
            this.esperados = new List<string>();
            this.esAdvertencia = esAdverencia;
        }
        public void AddEsperado(String esperado)
        {
            this.esperados.Add(esperado);
        }

        
    }
}
