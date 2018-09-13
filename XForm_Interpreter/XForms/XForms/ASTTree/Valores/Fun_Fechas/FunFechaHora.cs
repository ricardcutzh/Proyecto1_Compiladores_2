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
    class FunFechaHora:NodoAST, Expresion
    {
        Expresion exp;
        public FunFechaHora(Expresion exp, String clase, int linea, int col):base(linea, col, clase)
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
                return "fechahora";
            }
        }

        public object getValor(Ambito ambito)
        {
            try
            {
                Object valor = this.exp.getValor(ambito);
                if(valor is String)
                {
                    String real = (String)valor;
                    try
                    {
                        DateTime aux = DateTime.ParseExact(real, "dd/MM/yyyy hh:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                        this.val = aux.ToString();
                        return aux;
                    }
                    catch
                    {
                        TError error = new TError("Semantico", "No se encontro una cadena que cumpla con el formato: dd/mm/yyyy hh:mm:ss para la funcion FechaHora() / o se ingreso valor invalido | Clase: " + this.clase + " | Archivo: " + ambito.archivo, linea, columna, false);
                        Estatico.errores.Add(error);
                        Estatico.ColocaError(error);
                    }
                }
                else if(valor is DateTime)
                {
                    this.val = valor.ToString();
                    return valor;
                }
                else
                {
                    TError error = new TError("Semantico", "La funcion FechaHora() unicamente puede recibir una cadena como parametro una cadena | Clase: " + clase + " | Archivo: " + ambito.archivo, linea, columna, false);
                    Estatico.ColocaError(error);
                    Estatico.errores.Add(error);
                }
                
            }
            catch(Exception e)
            {
                TError error = new TError("Ejecucion", "Error al intentar convertir cedena a hora, en funcion FechaHora() | Clase: " + this.clase + " | Archivo: " + ambito.archivo + " | Mensaje: " + e.Message, linea, columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            return new Nulo();
        }
    }
}
