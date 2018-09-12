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
    class RepetirHasta : NodoAST, Instruccion
    {
        List<Object> instrucciones;
        Expresion condicion;

        public RepetirHasta(List<Object> instrucciones, Expresion condicion, String clase, int linea, int col):base(linea,col, clase)
        {
            this.instrucciones = instrucciones;
            this.condicion = condicion;
        }

        public object Ejecutar(Ambito ambito)
        {
            try
            {
                Object condi = condicion.getValor(ambito);
                String tipo = condicion.getTipo(ambito);
                if(condi is Boolean)
                {
                    Boolean val = (Boolean)condi;
                    /// NUEVO AMBITO
                    Ambito ambitoRepetir = new Ambito(ambito, this.clase, ambito.archivo);
                    ambitoRepetir.tomaValoresDelAmbito(ambito, true);
                    /// 
                    do
                    {
                        foreach (Object o in this.instrucciones)
                        {
                            if (o is Instruccion)
                            {
                                Instruccion aux = (Instruccion)o;
                                Object res = aux.Ejecutar(ambitoRepetir);
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
                                Object res = exp.getValor(ambitoRepetir);
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
                        condi = this.condicion.getValor(ambito);//OJO CON POSIBLE ERROR
                        val = (Boolean)condi;
                    } while (!val);

                    HacerBreak:
                    return new Nulo();
                }
                else
                {
                    TError error = new TError("Semantico", "Para la condicion del ciclo Repetir se espera un valor Booleano y se encontro: \"" + tipo + "\" | Clase: " + this.clase + " | Archivo: " + ambito.archivo, linea, columna, false);
                    Estatico.ColocaError(error);
                    Estatico.errores.Add(error);
                }
            }
            catch(Exception e)
            {
                TError error = new TError("Ejecucion", "Error al ejecutar el ciclo Repetir-Hasta | Clase: " + this.clase + " | Archivo: " + ambito.archivo + " | Mensaje: " + e.Message, linea, columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            return new Nulo();
        }
    }
}
