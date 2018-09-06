using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.ASTTree.Interfaces;
using XForms.GramaticaIrony;
using XForms.Simbolos;
using XForms.Objs;

namespace XForms.ASTTree.Sentencias
{
    class AsignacionPropiedad : NodoAST, Instruccion
    {
        Expresion objetoAsignar;
        String propiedad;
        Expresion valorAsignar;

        public AsignacionPropiedad(Expresion objetoAsignar, String propieda, Expresion valorAsignar, int linea, int col, String clase):base(linea, col, clase)
        {
            this.objetoAsignar = objetoAsignar;
            this.propiedad = propieda;
            this.valorAsignar = valorAsignar;
        }

        public object Ejecutar(Ambito ambito)
        {
            try
            {
                
            }
            catch(Exception e)
            {
                TError error = new TError("Ejecuccion", "Error al ejecutar la asignacion de Propiedad: \"" + this.propiedad + "\" | Clase: " + this.clase + " | Archivo: " + ambito.archivo + " | " + e.Message, linea, columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            return null;
        }
    }
}
