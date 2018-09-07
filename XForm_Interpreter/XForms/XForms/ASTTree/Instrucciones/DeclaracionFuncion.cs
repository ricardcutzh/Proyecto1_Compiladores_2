using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.ASTTree.Interfaces;
using XForms.Simbolos;
using XForms.Objs;
using XForms.GramaticaIrony;

namespace XForms.ASTTree.Instrucciones
{
    class DeclaracionFuncion : NodoAST, Instruccion
    {
        List<object> instrucciones;
        List<NodoParametro> parametros;
        String tipo;
        String idfuncion;
        Estatico.Vibililidad visibilidad;

        public DeclaracionFuncion(List<object> instrucciones, String tipo, List<NodoParametro>parametros, Estatico.Vibililidad vibililidad, String idfuncion, int linea, int col, String clase):base(linea, col, clase)
        {
            this.instrucciones = instrucciones;
            this.parametros = parametros;
            this.tipo = tipo;
            this.idfuncion = idfuncion;
            this.visibilidad = vibililidad;
        }

        public object Ejecutar(Ambito ambito)
        {
            try
            {
                Clave clave = new Clave(this.idfuncion.ToLower(), this.parametros, "");
                if(!ambito.existeFuncion(clave))
                {
                    Funcion f = new Funcion(this.instrucciones, this.parametros, this.idfuncion.ToLower(), this.tipo, this.visibilidad, this.linea, this.columna, this.clase);
                    ambito.agregarFuncionAlAmbito(clave, f);
                }
                else
                {
                    TError error = new TError("Semantico", "Ya existe una definicion de Funcion: "+this.idfuncion+" con la misma cantidad de parametros y tipo | Clase: "+this.clase+" | Archivo: "+ambito.archivo, this.linea, this.columna, false);
                    Estatico.errores.Add(error);
                    Estatico.ColocaError(error);
                }
            }
            catch(Exception e)
            {
                TError error = new TError("Ejecucion", "Error en la ejecucion de Declaracion de Funcion: "+this.idfuncion+" | Clase: "+this.clase+" | Archivo: "+ambito.archivo+" | Error: "+e.Message, this.linea, this.columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            return null;
        }
    }
}
