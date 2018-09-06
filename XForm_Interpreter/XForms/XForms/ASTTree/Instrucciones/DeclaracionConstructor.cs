using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.ASTTree.Interfaces;
using XForms.Objs;
using XForms.Simbolos;
using XForms.GramaticaIrony;

namespace XForms.ASTTree.Instrucciones
{
    class DeclaracionConstructor : NodoAST, Instruccion
    {
        List<Instruccion> instrucciones;

        public List<NodoParametro> parametros;

        public DeclaracionConstructor(List<Instruccion> instrucciones, List<NodoParametro> parametros,  int linea, int col, String clase):base(linea,col, clase)
        {
            this.instrucciones = instrucciones;
            this.parametros = parametros;
        }

        public object Ejecutar(Ambito ambito)
        {
            try
            {
                ClaveFuncion clave = new ClaveFuncion(this.clase.ToLower(), "vacio", this.parametros);
                if(!ambito.existeConstructor(clave))
                {
                    Constructor c = new Constructor(this.parametros, instrucciones, this.linea, this.columna, this.clase);
                    ambito.agregarConstructor(clave,c);
                }
                else
                {
                    TError error = new TError("Semantico", "Ya existe una definicion de Constructor: " + this.clase + " con la misma cantidad de parametros y tipo | Clase: " + this.clase + " | Archivo: " + ambito.archivo, this.linea, this.columna, false);
                    Estatico.errores.Add(error);
                    Estatico.ColocaError(error);
                }
            }
            catch(Exception e)
            {
                TError error = new TError("Ejecucion", "Error en la ejecucion de Declaracion de Constructor: " + this.clase + " | Clase: " + this.clase + " | Archivo: " + ambito.archivo+" | Error: "+e.Message, this.linea, this.columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            return null;
        }
    }
}
