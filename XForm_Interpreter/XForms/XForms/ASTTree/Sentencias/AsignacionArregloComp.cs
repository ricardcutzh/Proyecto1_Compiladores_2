using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.Objs;
using XForms.GramaticaIrony;
using XForms.Simbolos;
using XForms.ASTTree.Interfaces;

namespace XForms.ASTTree.Sentencias
{
    class AsignacionArregloComp:NodoAST, Instruccion
    {
        Expresion ObjetoAsignar;
        AsignacionSimpleArreglo asignacionValor;


        public AsignacionArregloComp(Expresion ObjetoAsingar, AsignacionSimpleArreglo asignaicon, int linea, int col, String clase):base(linea, col, clase)
        {
            this.ObjetoAsignar = ObjetoAsingar;
            this.asignacionValor = asignaicon;
        }

        public object Ejecutar(Ambito ambito)
        {
            try
            {
                Object objeto = ObjetoAsignar.getValor(ambito);
                if(objeto is Objeto)
                {
                    Objeto aux = (Objeto)objeto;
                    Ambito ambOb = aux.ambito;
                    asignacionValor.Ejecutar(ambOb);
                }
                else //EN CASO DE QUE NO FUERA OBJETO
                {
                    TError error = new TError("Semantico", "Se esta intentando asignar a un Tipo de dato que no es Objeto! | Clase: " + this.clase + " | Archivo: " + ambito.archivo, this.linea, this.columna, false);
                    Estatico.errores.Add(error);
                    Estatico.ColocaError(error);
                }
            }
            catch(Exception e)
            {
                TError error = new TError("Ejecucion", "Error de Ejecucion al intentar asignar posicion de un Arreglo | " + this.clase + " | Archivo: " + ambito.archivo+" | Mensaje: "+e.Message, this.linea, this.columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            return null;
        }
    }
}
