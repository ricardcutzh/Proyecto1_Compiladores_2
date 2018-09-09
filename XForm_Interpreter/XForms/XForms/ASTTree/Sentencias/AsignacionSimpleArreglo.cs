using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.ASTTree.Interfaces;
using XForms.Objs;
using XForms.GramaticaIrony;
using XForms.Simbolos;

namespace XForms.ASTTree.Sentencias
{
    class AsignacionSimpleArreglo : NodoAST, Instruccion
    {
        String idArr;
        List<Expresion> dimensiones;
        Expresion val;

        public AsignacionSimpleArreglo(String idArr, List<Expresion>dimensiones, Expresion valor, int linea, int col, String clase):base(linea, col, clase)
        {
            this.idArr = idArr;
            this.dimensiones = dimensiones;
            this.val = valor;
        }

        public object Ejecutar(Ambito ambito)
        {
            try
            {
                Simbolo s = (Simbolo)ambito.getSimbolo(this.idArr.ToLower());
                if(s!=null)
                {
                    if(s is Arreglo)
                    {
                        Arreglo arr = (Arreglo)s;
                        /// DIMENSIONES DEL ARREGLO
                        List<int> dimensiones = getDimensiones(ambito);
                        if(dimensiones!=null)
                        {
                            Object valorAsignado = val.getValor(ambito);
                            String tipoVal = val.getTipo(ambito);
                            if(tipoVal.ToLower().Equals(arr.Tipo.ToLower()))
                            {
                                //ASIGNO EL VALOR
                                if(arr.esCoordenadaValida(dimensiones))
                                {
                                    int realIndex = arr.calcularPosicion(dimensiones);
                                    arr.setValueAtPosition(realIndex, valorAsignado);
                                }
                                else
                                {
                                    TError error = new TError("Semantico", "Asignacion a Arreglo: \""+this.idArr+"\" no se puede realizar ya que la posicion no es valida! | Clase: " + this.clase + " | Archivo: " + ambito.archivo, this.linea, this.columna, false);
                                    Estatico.errores.Add(error);
                                    Estatico.ColocaError(error);
                                }
                            }
                            else
                            {
                                TError error = new TError("Semantico", "El valor asignar al arreglo: \""+this.idArr+"\" no coincide con el tipo esperado. Se esperaba: \""+arr.Tipo+"\" y se encontro:\""+tipoVal+"\" | Clase: " + this.clase + " | Archivo: " + ambito.archivo, this.linea, this.columna, false);
                                Estatico.errores.Add(error);
                                Estatico.ColocaError(error);
                            }
                        }
                        else
                        {
                            TError error = new TError("Semantico", "Las dimensiones no contienen valores valodos, solo enteros son admitidos: \""+this.idArr+"\" | Clase: " + this.clase + " | Archivo: " + ambito.archivo, this.linea, this.columna, false);
                            Estatico.errores.Add(error);
                            Estatico.ColocaError(error);
                        }
                    }
                    else
                    {
                        TError error = new TError("Semantico", "El simbolo: \""+this.idArr+"\" No es un arreglo para poder realizara la asignacion | Clase: "+this.clase+" | Archivo: "+ambito.archivo, this.linea, this.columna, false);
                        Estatico.errores.Add(error);
                        Estatico.ColocaError(error);
                    }
                }
                else
                {
                    TError error = new TError("Semantico", "El arreglo: \""+idArr+"\" no existe en este ambito! | Clase: " + this.clase + " | Archivo: " + ambito.archivo, this.linea, this.columna, false);
                    Estatico.errores.Add(error);
                    Estatico.ColocaError(error);
                }
            }
            catch(Exception e)
            {
                TError error = new TError("Ejecucion", "Error de Ejecucion al intentar asignar posicion de un Arreglo | Clase: " + this.clase + " | Archivo: " + ambito.archivo+" | Mensaje: "+e.Message, this.linea, this.columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            return null;
        }


        private List<int> getDimensiones(Ambito ambito)
        {
            List<int> dims = new List<int>();
            foreach (Expresion e in this.dimensiones)
            {
                Object valor = e.getValor(ambito);
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
