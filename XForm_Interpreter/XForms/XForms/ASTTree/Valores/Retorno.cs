using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.ASTTree.Interfaces;
using XForms.GramaticaIrony;
using XForms.Simbolos;
using XForms.Objs;
namespace XForms.ASTTree.Valores
{
    class Retorno : NodoAST, Expresion
    {
        Expresion expresion;
        Boolean retornaVacio;

        public Retorno(Expresion exp, int linea, int col, String clase):base(linea, col, clase)
        {
            this.expresion = exp;
            this.retornaVacio = false;
        }

        public Retorno(int linea, int col, String clase):base(linea, col, clase)
        {
            this.expresion = null;
            retornaVacio = true;
        }

        Object ValorAux = null;
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
            else if (val is Vacio)
            {
                return "vacio";
            }
            else if(val is Arreglo)
            {
                return "Arreglo";
            }
            //AQUI FALTA EL TIPO OBJETO
            return "vacio";
        }

        public object getValor(Ambito ambito)
        {
            try
            {
                if(retornaVacio)
                {
                    this.ValorAux = new Vacio();
                    NodoReturn n = new NodoReturn(this.ValorAux, "vacio");
                    return n;
                }
                else
                {
                    this.ValorAux = this.expresion.getValor(ambito);
                    NodoReturn n = new NodoReturn(this.ValorAux, getTipo(ambito).ToLower());
                    return n;
                }
            }
            catch(Exception e)
            {
                TError error = new TError("Ejecucion", "No se ejecuto de buena forma el retorno: Clase: " + this.clase + " | Archivo: " + ambito.archivo+" | "+e.Message, this.linea, this.columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            return new Vacio();
        }
    }
}
