using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.ASTTree.Interfaces;
using XForms.GramaticaIrony;
using XForms.Objs;
using XForms.ASTTree.Sentencias;
namespace XForms.Simbolos
{
    class Principal:NodoAST, Instruccion
    {
        List<Instruccion> instrucciones;

        public Principal(List<Instruccion> instrucciones, int linea, int col, String clase):base(linea, col, clase)
        {
            this.instrucciones = instrucciones;
        }

        public object Ejecutar(Ambito ambito)
        {
            try
            {
                Ambito am = new Ambito(ambito, "Clase: " + this.clase, ambito.archivo);
                foreach(Instruccion i in this.instrucciones)
                {
                    i.Ejecutar(ambito);
                }
            }
            catch(Exception e)
            {
                TError error = new TError("Ejecucion", "Ejecutando Constructor: " + this.clase + " | Archivo: " + ambito.archivo+" | Error: "+e.Message, this.linea, this.columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            return null;
        }
    }
}
