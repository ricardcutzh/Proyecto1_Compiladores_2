using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.GramaticaIrony;
using XForms.Objs;
using XForms.ASTTree.Interfaces;
using XForms.Simbolos;

namespace XForms.ASTTree.Valores.Func_Cadenas
{
    class SubCade : NodoAST, Expresion
    {
        Expresion cadena;
        Expresion num1;
        Expresion num2;

        public SubCade(Expresion cadena, Expresion num1, Expresion num2, int linea, int col, String clase) : base(linea, col, clase)
        {
            this.cadena = cadena;
            this.num1 = num1;
            this.num2 = num2;
        }

        String tipo = "";
        public string getTipo(Ambito ambito)
        {
            if(tipo.Equals(""))
            {
                return "nulo";
            }
            else
            {
                return "cadena";
            }
        }

        public object getValor(Ambito ambito)
        {
            try
            {
                Object cad = cadena.getValor(ambito);
                String tipocad = cadena.getTipo(ambito);

                Object inf = num1.getValor(ambito);
                String tipoInf = num1.getTipo(ambito);

                Object sup = num2.getValor(ambito);
                String tipoSup = num2.getTipo(ambito);

                if(cad is String && inf is int && sup is int)
                {
                    String cadena = (String)cad;
                    int inferior = (int)inf;
                    int superior = (int)sup;

                    if(inferior >= 0 && superior>=0)
                    {
                        if(superior < cadena.Length)
                        {
                            tipo = cadena.Substring(inferior, superior);
                            return tipo;
                        }
                        else
                        {
                            TError error = new TError("Semantico", "El limite superior no debe ser menor al largo de la cadena! | Clase: " + this.clase + " | Archivo: " + ambito.archivo, linea, columna, false);
                            Estatico.errores.Add(error);
                            Estatico.ColocaError(error);
                        }
                    }
                    else
                    {
                        TError error = new TError("Semantico", "Parametros de Funcion SubCadena deben ser mayores a 0 | Clase: " + this.clase + " | Archivo: " + ambito.archivo, linea, columna, false);
                        Estatico.errores.Add(error);
                        Estatico.ColocaError(error);
                    }
                }
                else
                {
                    TError error = new TError("Semantico", "Funcion SubCade requiere parametros: (cadena, entero, entero) y se encontro: ("+tipocad+","+tipoInf+","+tipoSup+") | Clase: "+this.clase+" | Archivo: "+ambito.archivo, linea, columna, false);
                    Estatico.errores.Add(error);
                    Estatico.ColocaError(error);
                }
            }
            catch(Exception e)
            {
                TError error = new TError("Ejecucion", "Error al ejecutar la funcion SubCadena | Clase: "+clase+" | Archivo: "+ambito.archivo+" | Mensaje: "+e.Message, linea, columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            return new Nulo();
        }
    }
}
