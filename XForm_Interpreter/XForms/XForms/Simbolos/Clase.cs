using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.ASTTree.Interfaces;

namespace XForms.Simbolos
{
    class Clase : Instruccion
    {
        public String idClase { get; set; }
        public String Padre { get; set; }
        public Boolean Hereda { get; set; }
        public Ambito AmbitoLocal { get; set; }
        String ArchivoOrigen;

        public Clase(String idclase, String padre, Boolean hereda, Ambito ambitolocal, String Archivo)
        {
            this.idClase = idClase;
            this.Padre = padre;
            this.Hereda = hereda;
            this.AmbitoLocal = ambitolocal;
            this.ArchivoOrigen = Archivo;
        }

        public object Ejecutar(Ambito ambito)
        {
            throw new NotImplementedException();
        }
    }
}
