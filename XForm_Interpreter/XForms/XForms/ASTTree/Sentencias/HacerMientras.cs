using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.ASTTree.Interfaces;
using XForms.Simbolos;
using XForms.Objs;
using XForms.GramaticaIrony;
namespace XForms.ASTTree.Sentencias
{
    class HacerMientras:NodoAST, Instruccion
    {
        Expresion condicion;
        List<Object> instrucciones;

        public HacerMientras(Expresion condicion, List<Object> instrucciones, String clase, int linea, int col):base(linea, col, clase)
        {
            this.condicion = condicion;
            this.instrucciones = instrucciones;
        }

        public object Ejecutar(Ambito ambito)
        {
            try
            {
                Object cond = this.condicion.getValor(ambito);
                String tipo = this.condicion.getTipo(ambito);
                if(cond is Boolean)
                {
                    Boolean val = (Boolean)cond;
                    do
                    {
                        foreach(Object o in this.instrucciones)
                        {
                            if (o is Instruccion)
                            {
                                Instruccion aux = (Instruccion)o;
                                Object res = aux.Ejecutar(ambito);
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
                                Object res = exp.getValor(ambito);
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
                        cond = this.condicion.getValor(ambito);
                        val = (Boolean)cond;
                    } while (val);

                    HacerBreak:
                    return new Nulo();
                }
                else
                {
                    TError error = new TError("Ejecucion", "La sentencias Hacer-Mientras recibe una condicion con resultado booleano, Se encontro tipo: \""+tipo+"\" | Clase: " + this.clase + " | Archivo: " + ambito.archivo , linea, columna, false);
                    Estatico.errores.Add(error);
                    Estatico.ColocaError(error);
                }
            }
            catch(Exception e)
            {
                TError error = new TError("Ejecucion", "Error al ejecutar el ciclo Hacer-Mientras | Clase: "+this.clase+" | Archivo: "+ambito.archivo+" | Mensaje: "+e.Message, linea, columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            return new Nulo();
        }
    }
}
