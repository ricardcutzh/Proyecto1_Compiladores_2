﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.GramaticaIrony;
using XForms.Objs;
using XForms.ASTTree.Interfaces;
using XForms.Simbolos;

namespace XForms.ASTTree.Valores.Func_Numericas
{
    class FunMax : NodoAST, Expresion
    {
        List<Expresion> expresiones;
        String identificador;
        int tipo;

        public FunMax(List<Expresion> expresiones, String clase, int linea, int col):base(linea, col, clase)
        {
            this.expresiones = expresiones;
            this.tipo = 1;
        }

        public FunMax(String identificador, String clase, int linea, int col):base(linea, col, clase)
        {
            this.identificador = identificador;
            this.tipo = 2;
        }

        Object valor = new Nulo();

        public string getTipo(Ambito ambito)
        {
            if(valor is double)
            {
                return "decimal";
            }
            else
            {
                return "nulo";
            }
        }

        public object getValor(Ambito ambito)
        {
            try
            {
                if(tipo == 1)/// ESTE ES PARA VALORES 
                {
                    double v = 0;
                    foreach(Expresion e in this.expresiones)
                    {
                        Object val = e.getValor(ambito);
                        double auxiliar = 0;
                        if(val is int)
                        {
                            int aux = (int)val;
                            auxiliar = Convert.ToDouble(aux);
                        }
                        else if(val is double)
                        {
                            double aux = (double)val;
                            auxiliar = aux;
                        }
                        else if(val is String)
                        {
                            String aux = (String)val;
                            double x = 0;
                            foreach(char c in aux)
                            {
                                x = x + Convert.ToInt32(c);
                            }
                            auxiliar = x;
                        }
                        else if(val is Objeto || val is Nulo)
                        {
                            this.valor = 0;
                            return 0;
                        }

                        if(auxiliar > v)
                        {
                            v = auxiliar;
                        }
                    }
                    this.valor = v;
                    return v;
                }
                else
                {
                    Simbolo s = (Simbolo)ambito.getSimbolo(this.identificador.ToLower());
                    if(s!=null)
                    {
                        if(s is Arreglo)
                        {
                            Arreglo aux = (Arreglo)s;
                            List<object> linealizado = aux.linealizacion;
                            double auxiliar = 0;
                            foreach(object ob in linealizado)
                            {
                                double v = 0;
                                if(ob is int)
                                {
                                    int x = (int)ob;
                                    v = Convert.ToDouble(x);
                                }
                                else if(ob is double)
                                {
                                    double x = (double)ob;
                                    v = x;
                                }
                                else if(ob is String)
                                {
                                    String x = (String)ob;
                                    double y = 0;
                                    foreach(char c in x)
                                    {
                                        y = y + Convert.ToInt32(c);
                                    }
                                    v = y;
                                }
                                else if(ob is Objeto || ob is Nulo)
                                {
                                    this.valor = 0;
                                    return 0.0;
                                }

                                if(v > auxiliar)
                                {
                                    auxiliar = v;
                                }
                            }
                            this.valor = auxiliar;
                            return auxiliar;
                        }
                        else
                        {
                            TError error = new TError("Semantico", "El simbolo: \""+identificador+"\" no es un arreglo para poder aplicar la funcion Max() | Clase: "+this.clase+" | Archivo: "+ambito.archivo, linea, columna, false);
                            Estatico.errores.Add(error);
                            Estatico.ColocaError(error);
                        }
                    }
                    else
                    {
                        TError error = new TError("Semantico", "En funcion Max se hace referencia a Arreglo: \""+s.idSimbolo+"\" el cual no existe | Clase: "+this.clase+" | Archivo: "+ambito.archivo, linea, columna, false);
                        Estatico.errores.Add(error);
                        Estatico.ColocaError(error);
                    }
                }
            }
            catch(Exception e)
            {
                TError error = new TError("Ejecucion", "Error al ejecutar la funcion Max() | Clase: "+this.clase+" | Archivo: "+ambito.archivo+" | Mensaje: "+e.Message, linea, columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            return 0.0;
        }
    }
}
