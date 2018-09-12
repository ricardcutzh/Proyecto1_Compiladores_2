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
    class SentenciaMientras : NodoAST, Instruccion
    {
        Expresion condicion;
        List<Object> instrucciones;

        public SentenciaMientras(Expresion condicion, List<Object> instrucciones, String clase, int linea, int col):base(linea, col, clase)
        {
            this.condicion = condicion;
            this.instrucciones = instrucciones;
        }

        public object Ejecutar(Ambito ambito)
        {
            try
            {
                Object comprobacion = condicion.getValor(ambito);
                if(comprobacion is Boolean)
                {
                    Boolean val = (Boolean)comprobacion;
                    /// NUEVO AMBITO
                    Ambito ambitoWhile = new Ambito(ambito, this.clase, ambito.archivo);
                    ambitoWhile.tomaValoresDelAmbito(ambito, true);
                    /// 
                    while(val)
                    {
                        foreach(Object o in this.instrucciones)
                        {
                            if(o is Instruccion)
                            {
                                Instruccion aux = (Instruccion)o;
                                Object res = aux.Ejecutar(ambitoWhile);
                                if(res is NodoReturn)
                                {
                                    return res;
                                }
                                else if (res is Romper)
                                {
                                    goto HaceBreak;
                                }
                                else if (res is Continuar)
                                {
                                    goto HacerContinue;
                                }
                            }
                            else if(o is Expresion)
                            {
                                Expresion exp = (Expresion)o;
                                Object res = exp.getValor(ambitoWhile);
                                if (res is NodoReturn)
                                {
                                    return res;
                                }
                                else if (res is Romper)
                                {
                                    goto HaceBreak;
                                }
                                else if (res is Continuar)
                                {
                                    goto HacerContinue;
                                }
                            }
                            
                        }

                        HacerContinue:
                        comprobacion = condicion.getValor(ambito);//OJO POSIBLE ERROR
                        val = (Boolean)comprobacion;
                    }
                    HaceBreak:
                    return new Nulo();
                }
                else
                {
                    TError error = new TError("Semantico", "Para una Sentencias Mientras se necesita un valor Booelano en la condicion | Clase: " + this.clase + " | Archivo: " + ambito.archivo, linea, columna, false);
                    Estatico.errores.Add(error);
                    Estatico.ColocaError(error);
                }
            }
            catch(Exception e)
            {
                TError error = new TError("Ejecucion", "Ocurrio un error al Ejecutar la sentencia Mientras | Clase: " + this.clase + " | Archivo: " + ambito.archivo+" | Mensaje: "+e.Message, linea, columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            return new Nulo();
        }
    }
}
