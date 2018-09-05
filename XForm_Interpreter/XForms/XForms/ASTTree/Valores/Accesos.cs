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
    class Accesos : NodoAST, Expresion
    {
        List<Expresion> expresiones;

        public Accesos(int linea, int col, String clase):base(linea, col, clase)
        {
            this.expresiones = new List<Expresion>();
        }

        public void AddExpresion(Expresion exp)
        {
            this.expresiones.Add(exp);
        }

        Object valorAux = null;
        public string getTipo(Ambito ambito)
        {
            Object val = null;
            if(valorAux!=null)
            {
                val = valorAux;
            }
            else
            {
                val = getValor(ambito);
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
            //AQUI FALTA EL TIPO OBJETO
            return "Objeto";
        }

        public object getValor(Ambito ambito)
        {
            try
            {
                if(this.expresiones.Count == 1)//SI SOLO SE HACE REFERENCIA A UN ID O UNA LLAMADA A UNA FUNCION
                {
                    this.valorAux = expresiones.ElementAt(0).getValor(ambito);
                    if(valorAux is Este)
                    {
                        return new Nulo();
                    }
                    return valorAux;
                }
                else if(this.expresiones.Count > 1)//AQUI ES DONDE YA SE VA A HACER REFERENCIA A OBJETOS
                {
                    //AQUI SI ES ACCESO A OBJETO
                }
            }
            catch(Exception e)
            {
                TError error = new TError("Ejecucion", "Ocurrio un Error al acceder a Objeto en Clase: " + this.clase + " | Archivo: " + this.archivoOrigen, this.linea, this.columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            return new Nulo();
        }

        

        private object buscaAtributoDeClase(Ambito am)
        {
            Ambito aux = am;
            while(aux.Anterior!=null)
            {
                aux = aux.Anterior;
            }

            return new Nulo();
        }

    }
}
