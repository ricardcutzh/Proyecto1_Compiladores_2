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
    class FunRandom:NodoAST, Expresion
    {
        List<Expresion> expresiones;

        int tipo;

        public FunRandom(List<Expresion> expresiones, String clase, int linea, int col):base(linea, col, clase)
        {
            this.expresiones = expresiones;
            this.tipo = 1;
        }

        public FunRandom(String clase, int linea, int col):base(linea, col, clase)
        {
            this.tipo = 2;
        }

        object valor = new Nulo();

        public string getTipo(Ambito ambito)
        {
            if(valor is int)
            {
                return "entero";
            }
            if(valor is String)
            {
                return "cadena";
            }
            if(valor is double)
            {
                return "decimal";
            }
            return "nulo";
        }

        public object getValor(Ambito ambito)
        {
            try
            {
                if(tipo==1)
                {
                    int maximo = this.expresiones.Count-1;/// TOMO EL VALOR MAXIMO
                    Random r = new Random();/// FUNCION RANDOM
                    int index = r.Next(0, maximo);/// VALOR DEL INDEX DE LA EXPRESION

                    Expresion v = this.expresiones.ElementAt(index);/// ELEMENTO

                    this.valor = v.getValor(ambito);

                    if(valor is string || valor is int || valor is double)
                    {
                        return valor;
                    }
                    else
                    {
                        TError error = new TError("Semantico", "La Funcion Random() solo permite retornar valores: Cadena, entero o Decimal | Clase: "+clase+" | Archivo: "+ambito.archivo, linea, columna, false);
                        Estatico.errores.Add(error);
                        Estatico.ColocaError(error);
                    }
                    return new Nulo();
                }
                else
                {
                    Random r = new Random();
                    this.valor = r.Next(0, 1);
                    return this.valor;
                }
            }
            catch(Exception e)
            {
                TError error = new TError("Ejecucion", "Error al ejecutar la funcion: Random() | Clase: "+this.clase+" | Archivo: "+ambito.archivo+" | Mensaje: "+e.Message, linea, columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            return new Nulo();
        }
    }
}
