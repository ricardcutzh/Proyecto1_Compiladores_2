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
    class FunPow : NodoAST, Expresion
    {

        Expresion expresion;
        Expresion potencia;

        public FunPow(Expresion exp, Expresion potencia, String clase, int linea, int col):base(linea, col, clase)
        {
            this.expresion = exp;
            this.potencia = potencia;
        }

        object valor = new Nulo();

        public string getTipo(Ambito ambito)
        {
            if(valor is Nulo)
            {
                return "nulo";
            }
            else
            {
                return "decimal";
            }
        }

        public object getValor(Ambito ambito)
        {
            try
            {
                object basse = expresion.getValor(ambito);
                object pot = potencia.getValor(ambito);

                if(basse is int && pot is int)
                {
                    int b = (int)basse;
                    int p = (int)pot;
                    this.valor = Math.Pow(b, p);
                    return this.valor;
                }
                else if( basse is int && pot is double)
                {
                    int b = (int)basse;
                    double p = (double)pot;
                    this.valor = Math.Pow(b, p);
                    return this.valor;
                }
                else if(basse is double && pot is int)
                {
                    double b = (double)basse;
                    int p = (int)pot;
                    this.valor = Math.Pow(b, p);
                    return this.valor;
                }
                else if(basse is double && pot is double)
                {
                    double b = (double)basse;
                    double p = (double)pot;
                    this.valor = Math.Pow(b, p);
                    return this.valor;
                }
                else
                {
                    TError error = new TError("Semantico", "La funcion Pow() no se encontro parametros validos, se esperan: Pow(entero/decimal, entero/decimal) | Clase: " + this.clase + " | Archivo: " + ambito.archivo, linea, columna, false);
                    Estatico.errores.Add(error);
                    Estatico.ColocaError(error);
                }
            }
            catch(Exception e)
            {
                TError error = new TError("Ejecucion", "Error al ejecutar la funcion Pow | Clase: " + this.clase + " | Archivo: " + ambito.archivo+" | Mensaje: "+e.Message, linea, columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            return 0.0;
        }
    }
}
