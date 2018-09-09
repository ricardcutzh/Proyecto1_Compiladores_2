using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.ASTTree.Interfaces;
using XForms.Simbolos;
using XForms.Objs;
using XForms.GramaticaIrony;

namespace XForms.ASTTree.Valores
{
    class Identificador : NodoAST, Expresion
    {
        public String identificador;

        public Identificador(String id, int col, int linea, String clase):base(linea, col, clase)
        {
            this.identificador = id;
        }

        private object ValorAux = null;
        public string getTipo(Ambito ambito)
        {
            Object val = null;
            if(ValorAux==null)
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
            else if(val is Arreglo)
            {
                return "Arreglo";
            }
            //AQUI FALTA EL TIPO OBJETO
            return "Objeto";

        }

        public object getValor(Ambito ambito)
        {
            if(this.identificador.ToLower().Equals("este"))
            {
                return new Este();
            }
            Simbolo aux = ambito.getSimbolo(this.identificador.ToLower());
            if(aux!=null)
            {
                if(aux is Variable)
                {
                    Variable v = (Variable)aux;
                    this.ValorAux = v.valor;
                    return v.valor;
                }
                else if(aux is Arreglo)
                {
                    //RETORNO SI ES OTRO TIPO
                    Arreglo arr = (Arreglo)aux;
                    this.ValorAux = arr;
                    return arr;
                }
            }
            else
            {
                TError erro = new TError("Semantico", "Se hacer referencia a: " + this.identificador + ", La cual no Existe en este Contexto: Clase: " + this.clase + " | Archivo: " + ambito.archivo, this.linea, this.columna, false);
                Estatico.errores.Add(erro);
                Estatico.ColocaError(erro);
            }
            return new Nulo(); 
        }
    }
}
