using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XForms.ASTTree.Interfaces
{
    class NodoAST
    {
        public int linea { get; } //SIRVE PARA REPORTAR EL ERROR
        public int columna { get; } //SIRVE PARA REPORTAR EL ERROR
        public String clase { get; } //PARA UBICAR EN QUE CLASE OCURRIO EL ERROR
        public String archivoOrigen { get; set; }

        public NodoAST(int linea, int columna, String clase)
        {
            this.linea = linea;
            this.columna = columna;
            this.clase = clase;
        }

        public void SetArchivoOrigen(String archivo)
        {
            this.archivoOrigen = archivo;
        }
    }
}
