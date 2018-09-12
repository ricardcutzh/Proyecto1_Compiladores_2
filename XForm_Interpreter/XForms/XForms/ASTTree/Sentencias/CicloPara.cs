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
                Ambito ambitoFor = new Ambito(ambito, this.clase, ambito.archivo);
                ambitoFor.tomaValoresDelAmbito(ambito, true);

                varControl.Ejecutar(ambitoFor);
                Object condicion = condicional.getValor(ambitoFor);
                if(condicion is Boolean)
                {
                    Boolean val = (Boolean)condicion;
                    ///
                    
                    while(val)
                    {
                        foreach (Object o in this.instrucciones)
                        {
                            if (o is Instruccion)
                            {
                                Instruccion aux = (Instruccion)o;
                                Object res = aux.Ejecutar(ambitoFor);
                                if (res is NodoReturn)
                                {
                                    return res;
                                }
                                else if (res is Romper)
                                {
                                    goto HacerBreak;
                                }
                                else if (res is Continuar)
                                {
                                    goto HacerContinue;
                                }
                            }
                            else if (o is Expresion)
                            {
                                Expresion exp = (Expresion)o;
                                Object res = exp.getValor(ambitoFor);
                                if (res is NodoReturn)
                                {
                                    return res;
                                }
                                else if (res is Romper)
                                {
                                    goto HacerBreak;
                                }
                                else if (res is Continuar)
                                {
                                    goto HacerContinue;
                                }
                            }

                        }
                        HacerContinue:
                        operacion.Ejecutar(ambitoFor); //EJECTUO EL IMCREMENTO
                        condicion = condicional.getValor(ambitoFor); // EVALUO LA CONDICION, OJO QUE LA TOMO DEL PADRE
                        val = (Boolean)condicion; /// LA PASO A BOOLEANO
                    }

                    HacerBreak:
                    return new Nulo();
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
