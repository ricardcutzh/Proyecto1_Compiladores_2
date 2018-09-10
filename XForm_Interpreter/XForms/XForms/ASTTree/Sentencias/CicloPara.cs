using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.GramaticaIrony;
using XForms.ASTTree.Interfaces;
using XForms.Simbolos;
using XForms.Objs;


namespace XForms.ASTTree.Sentencias
{
    class CicloPara : NodoAST, Instruccion
    {

        Instruccion varControl;
        Expresion condicional;
        Instruccion operacion;
        List<Object> instrucciones;

        public CicloPara(Instruccion varcontrol, Expresion condicional, Instruccion operacion, List<Object> instrucciones, String clase, int linea, int col):base(linea, col, clase)
        {
            this.varControl = varcontrol;
            this.condicional = condicional;
            this.operacion = operacion;
            this.instrucciones = instrucciones;
        }

        public object Ejecutar(Ambito ambito)
        {
            try
            {
                Object condicion = condicional.getValor(ambito);
                if(condicion is Boolean)
                {
                    //
                }
                else
                {
                    TError error = new TError("Semantico", "En la condicional del ciclo Para se espera un valor booleano | Clase: "+this.clase+" | Archivo: "+ambito.archivo, linea, columna, false);
                    Estatico.errores.Add(error);
                    Estatico.ColocaError(error);
                }
            }
            catch(Exception e)
            {
                TError error = new TError("Ejecucion", "Error al ejecutar el ciclo For | Clase: "+clase+" | Archivo: "+ambito.archivo+" | Mensaje: "+e.Message, linea, columna);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            return new Nulo();
        }
    }
}
