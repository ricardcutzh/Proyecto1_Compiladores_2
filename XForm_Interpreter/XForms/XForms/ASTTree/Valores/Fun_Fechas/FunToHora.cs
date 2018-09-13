using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.GramaticaIrony;
using XForms.Objs;
using XForms.ASTTree.Interfaces;
using XForms.Simbolos;

namespace XForms.ASTTree.Valores.Fun_Fechas
{
    class FunToHora:NodoAST, Expresion
    {
        Expresion exp;

        public FunToHora(Expresion exp, String clase, int linea, int col):base(linea, col, clase)
        {
            this.exp = exp;
        }

        String val = "";
        public string getTipo(Ambito ambito)
        {
            if (val.Equals(""))
            {
                return "nulo";
            }
            else
            {
                return "hora";
            }
        }

        public object getValor(Ambito ambito)
        {
            try
            {
                Object valor = this.exp.getValor(ambito);
                if(valor is String)
                {
                    try
                    {
                        String aux = (String)valor;
                        DateTime real = DateTime.ParseExact("12/10/1996 " +aux, "dd/MM/yyyy hh:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                        Hour hora = new Hour(real);
                        this.val = hora.ToString();
                        return hora;
                    }
                    catch
                    {
                        TError error = new TError("Semantico", "El formato de la cadena para la funcion Hora() no cumple el formato: hh:mm:ss / o se ingreso valor invalido | Clase: " + clase + " | Archivo: " + ambito.archivo, linea, columna, false);
                        Estatico.ColocaError(error);
                        Estatico.errores.Add(error);
                    }
                }
                else if( valor is Hour)
                {
                    this.val = valor.ToString();
                    return valor;
                }
                else
                {
                    TError error = new TError("Semantico", "La funcion Hora() unicamente puede recibir una cadena como parametro una cadena | Clase: "+clase+" | Archivo: "+ambito.archivo, linea, columna, false);
                    Estatico.ColocaError(error);
                    Estatico.errores.Add(error);
                }
            }
            catch(Exception e)
            {
                TError error = new TError("Ejecucion", "Error al intentar convertir cedena a hora, en funcion Hora() | Clase: "+this.clase+" | Archivo: "+ambito.archivo+" | Mensaje: "+e.Message, linea, columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            return new Nulo();
        }
    }
}
