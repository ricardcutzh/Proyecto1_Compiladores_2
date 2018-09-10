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
    class Incremento : NodoAST, Instruccion
    {
        String id;

        public Incremento(String identificador, int linea, int col, String clase):base(linea, col, clase)
        {
            this.id = identificador;
        }

        public object Ejecutar(Ambito ambito)
        {
            try
            {
                Simbolo s = (Simbolo)ambito.getSimbolo(this.id.ToLower());
                if(s!=null)
                {
                    if(s is Variable && (s.Tipo.ToLower().Equals("entero")||s.Tipo.ToLower().Equals("decimal")))
                    {
                        Variable v = (Variable)s;
                        if(v.valor is int)
                        {
                            int aux = (int)v.valor;
                            v.valor = aux + 1;
                        }
                        else if(v.valor is Double)
                        {
                            double aux = (double)v.valor;
                            v.valor = aux + 1;
                        }
                        else
                        {
                            TError error = new TError("Semantico", "El Simbolo: \"" + this.id.ToLower() + "\" Posiblemente no ha sido inicializada! | Clase: " + this.clase + " | Archivo: " + ambito.archivo, linea, columna, false);
                            Estatico.errores.Add(error);
                            Estatico.ColocaError(error);
                        }
                    }
                    else
                    {
                        TError error = new TError("Semantico", "El Simbolo: \"" + this.id.ToLower() + "\" No hace referencia a una variable que se pueda aplicar: \""+s.Tipo+"\"| Clase: " + this.clase + " | Archivo: " + ambito.archivo, linea, columna, false);
                        Estatico.errores.Add(error);
                        Estatico.ColocaError(error);
                    }
                }
                else
                {
                    TError error = new TError("Semantico", "El Simbolo: \"" + this.id.ToLower() + "\" al cual se hace referencia no existe en este contexto | Clase: " + this.clase + " | Archivo: " + ambito.archivo, linea, columna, false);
                    Estatico.errores.Add(error);
                    Estatico.ColocaError(error);
                }
            }
            catch(Exception e)
            {
                TError error = new TError("Ejecucion", "Error al intentar ejecutar un incremento | Clase: " + this.clase + " | Archivo: " + ambito.archivo + " | Mensaje: " + e.Message, linea, columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            return new Nulo();
        }
    }
}
