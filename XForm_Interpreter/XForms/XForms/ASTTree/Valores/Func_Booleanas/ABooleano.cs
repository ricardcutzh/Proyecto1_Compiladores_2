using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.GramaticaIrony;
using XForms.Objs;
using XForms.ASTTree.Interfaces;
using XForms.Simbolos;

namespace XForms.ASTTree.Valores.Func_Booleanas
{
    class ABooleano : NodoAST, Expresion
    {

        Expresion expr;

        public ABooleano(Expresion expr, String clase, int linea, int col):base(linea, col, clase)
        {
            this.expr = expr;
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
                return "booleano";
            }
        }

        public object getValor(Ambito ambito)
        {
            try
            {
                Object ex = expr.getValor(ambito);
                if(ex is int)
                {
                    int aux = (int)ex;
                    if (aux == 1) { val = "verdadero"; return true; }
                    if (aux == 0) { val = "falso"; return false; }
                    else if(aux > 0) { val = "verdadero"; return true; }
                    else
                    {
                        val = "false";
                        return false;
                    }
                }
                else if(ex is double)
                {
                    double aux = (double)ex;
                    if(aux == 1) { val = "verdadero"; return true;}
                    if(aux == 0) { val = "falso"; return false; }
                    else if(aux > 0) { val = "verdadero"; return true; }
                    else
                    {
                        val = "false";
                        return false;
                    }
                }
                else if(ex is Objeto)
                {
                    val = "ob";
                    return true;
                }
                else if(ex is String)
                {
                    String aux = (String)ex;
                    if (aux.ToLower().Equals("falso")) { val = "false"; return false; }
                    if (aux.ToLower().Equals("verdadero")) { val = "true"; return true; }
                    else if(aux.Length > 0)
                    {
                        val = "true";
                        return true;
                    }
                    else
                    {
                        val = "false";
                        return false;
                    }
                }
                else if(ex is Boolean)
                {
                    val = "boolean";
                    return (Boolean)ex;
                }
                else 
                {
                    val = "false";
                    return false;
                }
            }
            catch(Exception e)
            {
                TError error = new TError("Ejecucion", "Error al intentar utilizar la funcion Booleano | Clase: " + this.clase + " | Archivo: " + ambito.archivo + " | Mensaje: " + e.Message, linea, columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            return new Nulo();
        }
    }
}
