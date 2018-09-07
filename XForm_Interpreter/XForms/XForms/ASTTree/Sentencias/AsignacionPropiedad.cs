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
                Object ObjetoAsignar = this.objetoAsignar.getValor(ambito);

                Object valor = valorAsignar.getValor(ambito);
                String tipoAsignado = valorAsignar.getTipo(ambito).ToLower();

                if(ObjetoAsignar is Este)
                {
                    Ambito aux = ambito;
                    while(aux.Anterior!=null)
                    {
                        aux = aux.Anterior;
                    }
                    Simbolo s = (Simbolo)aux.getSimbolo(this.propiedad.ToLower());
                    
                    if(s!=null)
                    {
                        if (s is Variable)
                        {
                            Variable vari = (Variable)s;
                            String tipoEsperado = vari.Tipo.ToLower();
                            if (tipoEsperado.Equals(tipoAsignado))
                            {
                                vari.valor = valor;//ASIGNO EL VALOR AL FIN....
                            }
                            else
                            {
                                TError error = new TError("Semantico", "Tipos Incompatibles No se puede asignar: \"" + tipoAsignado + "\", a una Variable de tipo: \"" + tipoEsperado + "\" | Clase: " + this.clase + " | Archivo: " + ambito.archivo, linea, columna, false);
                                Estatico.errores.Add(error);
                                Estatico.ColocaError(error);
                            }
                        }
                        else
                        {
                            //SI NO ES VARIABLE
                        }
                    }
                    else
                    {
                        TError error = new TError("Semantico", "Objeto: \""+this.clase+"\" no tiene Propiedad: \"" + this.propiedad + "\", Error Al acceder | Clase: " + this.clase + " | Archivo: " + ambito.archivo, linea, columna, false);
                        Estatico.errores.Add(error);
                        Estatico.ColocaError(error);
                    }
                }
                /// ES LO QUE BUSCABA UN OBJETO?
                else if(ObjetoAsignar is Objeto)
                {
                    Ambito auxiliar = ((Objeto)ObjetoAsignar).ambito;
                    Simbolo s = (Simbolo)auxiliar.getSimbolo(this.propiedad.ToLower());
                    if(s!=null)
                    {
                        if(s is Variable)
                        {
                            Variable vari = (Variable)s;
                            String tipoEsperado = vari.Tipo.ToLower();
                            if(tipoEsperado.Equals(tipoAsignado))
                            {
                                vari.valor = valor;//ASIGNO EL VALOR AL FIN....
                            }
                            else
                            {
                                TError error = new TError("Semantico", "Tipos Incompatibles No se puede asignar: \"" + tipoAsignado + "\", a una Variable de tipo: \""+tipoEsperado+"\" | Clase: " + this.clase + " | Archivo: " + ambito.archivo, linea, columna, false);
                                Estatico.errores.Add(error);
                                Estatico.ColocaError(error);
                            }
                        }
                        else
                        {
                            //SI NO ES VARIABLE....
                        }
                    }
                    else
                    {
                        TError error = new TError("Semantico", "Objeto no tiene Propiedad: \"" + this.propiedad + "\", Error Al acceder | Clase: " + this.clase + " | Archivo: " + ambito.archivo, linea, columna, false);
                        Estatico.errores.Add(error);
                        Estatico.ColocaError(error);
                    }
                }
                else
                {
                    TError error = new TError("Semantico", "Se esperaba un Objeto para Asignar La propiedad: \"" + this.propiedad + "\" al Mismo | Clase: " + this.clase + " | Archivo: " + ambito.archivo, linea, columna, false);
                    Estatico.errores.Add(error);
                    Estatico.ColocaError(error);
                }
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
