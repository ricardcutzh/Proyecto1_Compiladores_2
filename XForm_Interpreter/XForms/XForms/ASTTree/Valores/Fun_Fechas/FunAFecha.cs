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
    class FunAFecha:NodoAST, Expresion
    {
        Expresion exp;
        public FunAFecha(Expresion exp, String clase, int linea, int col):base(linea, col, clase)
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
                return "fecha";
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
                        DateTime aux = DateTime.ParseExact(real + " 00:00:00", "dd/MM/yyyy hh:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                        Date fecha = new Date(aux);
                        this.val = fecha.ToString();
                        return fecha;
                    }
                    catch
                    {
                        TError error = new TError("Semantico", "No se encontro una cadena que cumpla con el formato: dd/mm/yyyy para la funcion Fecha() / o se ingreso valor invalido | Clase: " + this.clase + " | Archivo: " + ambito.archivo, linea, columna, false);
                        Estatico.errores.Add(error);
                        Estatico.ColocaError(error);
                    }
                }
                else if(valor is Date)
                {
                    this.val = valor.ToString();
                    return valor;
                }
                else
                {
                    TError error = new TError("Semantico", "La funcion Fecha() recibe como parametros solo una cadena | Clase: " + this.clase + " | Archivo: " + ambito.archivo, linea, columna, false);
                    Estatico.errores.Add(error);
                    Estatico.ColocaError(error);
                }
            }
            catch (Exception e)
            {
                TError error = new TError("Ejecucion", "Error al intentar convertir cadena a Fecha: Funcion: Fecha() | Clase: " + this.clase + " | Archivo: " + ambito.archivo + " | Mensaje: " + e.Message, linea, columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            return new Nulo();
        }
    }
}
