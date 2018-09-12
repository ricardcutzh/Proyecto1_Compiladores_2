using System;
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
    class AEntero : NodoAST, Expresion
    {
        Expresion exp;

        public AEntero(Expresion exp, String clase, int linea, int col):base(linea, col, clase)
        {
            this.exp = exp;
        }

        String val = "";
        public string getTipo(Ambito ambito)
        {
            if(val.Equals(""))
            {
                return "nulo";
            }
            else
            {
                return "entero";
            }
        }

        public object getValor(Ambito ambito)
        {
            try
            {
                Object valorReal = exp.getValor(ambito);
                if(valorReal is String)
                {
                    String cad = (String)valorReal;
                    int col = 0;
                    foreach(char c in cad)
                    {
                        col = col + Convert.ToInt32(c);
                    }
                    this.val = "entero";
                    return col;
                }
                else if(valorReal is Boolean)
                {
                    Boolean valor = (Boolean)valorReal;
                    if(valor)
                    {
                        this.val = "entero";
                        return 1;
                    }
                    else
                    {
                        this.val = "entero";
                        return 0;
                    }
                }
                else if(valorReal is int)
                {
                    int v = (int)valorReal;
                    this.val = "entero";
                    return v;
                }
                else if(valorReal is double)
                {
                    double deci = (double)valorReal;
                    this.val = "entero";
                    return Convert.ToInt32(deci);
                }
                else if(valorReal is DateTime)
                {
                    DateTime valor = (DateTime)valorReal;
                    DateTime fechaBase = DateTime.ParseExact("01/01/2000 00:00:00", "dd/MM/yyyy hh:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                    int dias = (valor - fechaBase).Days;
                    this.val = "entero";
                    return dias;
                }
                else if(valorReal is Date)
                {
                    Date fecha = (Date)valorReal;
                    DateTime fechaBase = DateTime.ParseExact("01/01/2000 00:00:00", "dd/MM/yyyy hh:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                    int dias = (fecha.fecha - fechaBase).Days;
                    this.val = "entero";
                    return dias;
                }
                else if(valorReal is Hour)
                {
                    Hour hora = (Hour)valorReal;
                    DateTime fechaBase = DateTime.ParseExact("01/01/2000 00:00:00", "dd/MM/yyyy hh:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                    int dias = (hora.fecha - fechaBase).Days;
                    this.val = "entero";
                    return dias;
                }
                else
                {
                    return 0;
                }
            }
            catch(Exception e)
            {
                TError error = new TError("Ejecucion", "Error al ejecutar la funcion de Entero() | Clase: "+this.clase+" | Archivo: "+ambito.archivo+" | Mensaje: "+e.Message, linea, columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            return 0;
        }
    }
}
