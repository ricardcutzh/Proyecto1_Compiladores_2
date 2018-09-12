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
    class AsignacionSimple : NodoAST, Instruccion
    {

        Expresion valor;
        String identificador;

        public AsignacionSimple(Expresion valor, String id, int linea, int col, String clase):base(linea, col, clase)
        {
            this.valor = valor;
            this.identificador = id;
        }

        public object Ejecutar(Ambito ambito)
        {
            try
            {
                Simbolo s = (Simbolo)ambito.getSimbolo(this.identificador.ToLower());
                if(s!=null)
                {
                    if(s is Variable)
                    {
                        Variable vari = (Variable)s;
                        String tipoEsperado = vari.Tipo;
                        Object val = this.valor.getValor(ambito);
                        String tipoEncontrado = this.valor.getTipo(ambito);
                        if(tipoEncontrado.ToLower().Equals(tipoEsperado.ToLower()) || tipoEncontrado.ToLower().Equals("nulo"))
                        {
                            vari.valor = val;//ASIGNO EL NUEVO VALOR
                        }
                        ////////////////////////////////////////////////////////////////
                        else if (tipoEsperado.ToLower().Equals("entero") && val is double)
                        {
                            Double valor = (Double)val;
                            int real = Convert.ToInt32(valor);
                            vari.valor = real;
                        }
                        else if (tipoEsperado.ToLower().Equals("decimal") && val is int)
                        {
                            int valor = (int)val;
                            double real = Convert.ToDouble(valor);
                            vari.valor = real;
                        }
                        else if (tipoEsperado.ToLower().Equals("entero") && val is bool)
                        {
                            Boolean valor = (Boolean)val;
                            int real = Convert.ToInt32(valor);
                            vari.valor = real;
                        }
                        else if (tipoEsperado.ToLower().Equals("booleano") && val is int)
                        {
                            int valor = (int)val;
                            Boolean real = false;
                            if (valor == 1) { real = true; }
                            vari.valor = real;
                        }
                        ////////////////////////////////////////////////////////////////
                        else
                        {
                            TError error = new TError("Semantico", "Tipos no Coinciden al Asignar, Esperado: \"" + tipoEsperado + "\", Encontrado: " + tipoEncontrado + "\" | Clase: " + this.clase + " | Archivo: " + ambito.archivo, linea, columna, false);
                            Estatico.errores.Add(error);
                            Estatico.ColocaError(error);
                        }
                    }
                    else if(s is Arreglo)
                    {
                        Arreglo a = (Arreglo)s;
                        Object v = this.valor.getValor(ambito);
                        String tipoEncontrado = this.valor.getTipo(ambito);
                        if (v is Arreglo)
                        {
                            Arreglo asig = (Arreglo)v;

                            if(a.numDimensiones == asig.numDimensiones && a.Tipo.ToLower().Equals(asig.Tipo.ToLower()))
                            {
                                Ambito aux = buscarAmbitoDondeEsta(this.identificador.ToLower(), ambito);
                                aux.removerArreglo(this.identificador.ToLower());
                                aux.agregarVariableAlAmbito(this.identificador.ToLower(), new Arreglo(asig.linealizacion, asig.dimensiones, asig.numDimensiones, a.idSimbolo, true, a.Visibilidad, a.Tipo));
                            }
                            else
                            {
                                TError error = new TError("Semantico", " Arreglos no tienen la misma caracteristicas, Se esperaba arreglo: Dimensiones: "+a.numDimensiones+" y Tipo: "+a.Tipo+", Se econtro: Dimensiones: "+asig.numDimensiones+" y Tipo: "+asig.Tipo+"| Clase: " + this.clase + " | Archivo: " + ambito.archivo, linea, columna, false);
                                Estatico.errores.Add(error);
                                Estatico.ColocaError(error);
                            }
                        }
                        else
                        {
                            TError error = new TError("Semantico", "Tipos no Coinciden al Asignar, Esperado: \"" + "Arreglo" + "\", Encontrado: " + tipoEncontrado + "\" | Clase: " + this.clase + " | Archivo: " + ambito.archivo, linea, columna, false);
                            Estatico.errores.Add(error);
                            Estatico.ColocaError(error);
                        }
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
                else
                {
                    TError error = new TError("Semantico", "Referencia a Simbolo: \"" + this.identificador + "\" inexistente | Clase: " + this.clase + " | Archivo: " + ambito.archivo, linea, columna, false);
                    Estatico.errores.Add(error);
                    Estatico.ColocaError(error);
                }
            }
            catch(Exception e)
            {
                TError error = new TError("Ejecuccion", "Error al ejecutar la asignacion a Var: \"" + this.identificador + "\" | Clase: " + this.clase + " | Archivo: " + ambito.archivo+" | "+e.Message, linea, columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            return null;
        }


        private Ambito buscarAmbitoDondeEsta(String id, Ambito ambito)
        {
            Ambito aux = ambito;
            while(aux.Anterior!=null)
            {
                if(aux.existeVariable(id.ToLower()))
                {
                    return aux;
                }
                aux = aux.Anterior; 
            }
            return aux;
        }
    }
}
