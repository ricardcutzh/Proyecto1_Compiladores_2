using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.ASTTree.Interfaces;
using XForms.Simbolos;
using XForms.Objs;
using XForms.GramaticaIrony;

namespace XForms.ASTTree.Valores
{
    class ValorArreglo : NodoAST, Expresion
    {
        public String id;
        List<Expresion> expresions;

        public ValorArreglo(String id, List<Expresion> expresiones, int linea, int col, String clase):base(linea, col, clase)
        {
            this.id = id;
            this.expresions = expresiones;
        }

        private object ValorAux = null;
        public string getTipo(Ambito ambito)
        {
            Object val = null;
            if (ValorAux == null)
            {
                val = getValor(ambito);
            }
            else
            {
                val = this.ValorAux;
            }
            if (val is bool)
            {
                return "Booleano";
            }
            else if (val is string)
            {
                return "Cadena";
            }
            else if (val is int)
            {
                return "Entero";
            }
            else if (val is double)
            {
                return "Decimal";
            }
            else if (val is System.DateTime)
            {
                return "FechaHora";
            }
            else if (val is Date)
            {
                return "Fecha";
            }
            else if (val is Hour)
            {
                return "Hora";
            }
            else if (val is Nulo)
            {
                return "Nulo";
            }
            else if (val is Objeto)
            {
                return ((Objeto)val).idClase.ToLower();
            }
            else if (val is Arreglo)
            {
                return "Arreglo";
            }
            //AQUI FALTA EL TIPO OBJETO
            return "Objeto";
        }

        public object getValor(Ambito ambito)
        {
            try
            {
                Simbolo s = (Simbolo)ambito.getSimbolo(this.id.ToLower());
                if (s != null)
                {
                    if(s is Arreglo)
                    {
                        Arreglo aux = (Arreglo)s;
                        List<int> coordenada = getDimensiones(ambito);
                        if(coordenada!=null)
                        {
                            if(aux.esCoordenadaValida(coordenada))
                            {
                                int realIndex = aux.calcularPosicion(coordenada);
                                Object valor = aux.getElementFromArray(realIndex);
                                this.ValorAux = valor;
                                return valor;
                            }
                            else
                            {
                                TError error = new TError("Semantico", "Para el arreglo: \"" + this.id + "\" La Dimension no se encuentra dentro de los limites! | Clase: " + this.clase + " | Archivo: " + ambito.archivo, this.linea, this.columna, false);
                                Estatico.errores.Add(error);
                                Estatico.ColocaError(error);
                            }
                        }
                        else
                        {
                            TError error = new TError("Semantico", "Para el arreglo: \"" + this.id + "\" No se ha proporcionado un Entero como Dimension del Mismo | Clase: " + this.clase + " | Archivo: " + ambito.archivo, this.linea, this.columna, false);
                            Estatico.errores.Add(error);
                            Estatico.ColocaError(error);
                        }
                    }
                    else
                    {
                        TError error = new TError("Semantico", "El simbolo: \"" + this.id + "\" No es un arreglo por lo que no se puede acceder al valor indicado | Clase: " + this.clase + " | Archivo: " + ambito.archivo, this.linea, this.columna, false);
                        Estatico.errores.Add(error);
                        Estatico.ColocaError(error);
                    }
                }
                else
                {
                    TError error = new TError("Semantico", "No existe el Arreglo: \""+this.id+"\" en este Ambito | Clase: "+this.clase+" | Archivo: "+ambito.archivo, this.linea, this.columna, false);
                    Estatico.errores.Add(error);
                    Estatico.ColocaError(error);
                }
            }
            catch(Exception e)
            {
                TError error = new TError("Ejecucion", "Error al acceder al valor de arreglo: \""+id+"\" | Clase: "+this.clase+" | Archivo: "+ambito.archivo+" | Mensaje: "+e.Message, this.linea, this.columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            this.ValorAux = new Nulo();
            return ValorAux;
        }


        private List<int> getDimensiones(Ambito ambito)
        {
            List<int> dims = new List<int>();
            foreach (Expresion e in this.expresions)
            {
                //Object valor = e.getValor(ambito);
                Object valor = e.getValor(Estatico.temporal);
                if (valor is int)
                {
                    dims.Add((int)valor);
                }
                else
                {
                    return null;
                }
            }
            return dims;
        }
    }
}
