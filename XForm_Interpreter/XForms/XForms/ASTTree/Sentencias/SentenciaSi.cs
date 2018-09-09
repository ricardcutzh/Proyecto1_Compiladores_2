using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.ASTTree.Interfaces;
using XForms.GramaticaIrony;
using XForms.Objs;
using XForms.Simbolos;

namespace XForms.ASTTree.Sentencias
{
    class SentenciaSi : NodoAST, Instruccion
    {

        Expresion condicional;

        List<Object> sentenciasVerdaderas;

        List<Object> sentenciasFalso;

        int tipo;
        
        public SentenciaSi(Expresion condicional, List<Object> senVerd, String clase, int linea, int col):base(linea, col, clase)
        {
            this.condicional = condicional;
            this.sentenciasVerdaderas = senVerd;
            this.sentenciasFalso = null;

            tipo = 1;
        }

        public SentenciaSi(Expresion condicional, List<object> senVerd, List<object> senFal, int linea, int col, String clase) : base(linea, col, clase)
        {
            this.condicional = condicional;
            this.sentenciasVerdaderas = senVerd;
            this.sentenciasFalso = senFal;

            tipo = 2;
        }



        public object Ejecutar(Ambito ambito)
        {
            try
            {
                //SIMPLE IF SIN SENTENCIAS
                /// TOMO LA CONDICIONAL
                Object condicional = this.condicional.getValor(ambito);
                String tipoaux = this.condicional.getTipo(ambito);
                if(condicional is Boolean)
                {
                    Boolean res = (Boolean)condicional;
                    if(tipo == 1)
                    {
                        //UN SI SIMPLE
                        if(res)
                        {
                            foreach(Object o in this.sentenciasVerdaderas)
                            {
                                if(o is Instruccion)
                                {
                                    Instruccion aux = (Instruccion)o;
                                    Object s = aux.Ejecutar(ambito);
                                    if (s is NodoReturn)
                                    {
                                        return s;
                                    }
                                }
                                else if(o is Expresion)
                                {
                                    Expresion exp = (Expresion)o;
                                    Object s = exp.getValor(ambito);
                                    if (s is NodoReturn)
                                    {
                                        return s;
                                    }
                                }
                            }
                            return new Nulo();
                        }
                    }
                    else
                    {
                        if(res)
                        {
                            foreach (Object o in this.sentenciasVerdaderas)
                            {
                                if (o is Instruccion)
                                {
                                    Instruccion aux = (Instruccion)o;
                                    Object s = aux.Ejecutar(ambito);
                                    if (s is NodoReturn)
                                    {
                                        return s;
                                    }
                                }
                                else if (o is Expresion)
                                {
                                    Expresion exp = (Expresion)o;
                                    Object s = exp.getValor(ambito);
                                    if (s is NodoReturn)
                                    {
                                        return s;
                                    }
                                }
                            }
                            return new Nulo();
                        }
                        else
                        {
                            foreach(Object o in this.sentenciasFalso)
                            {
                                if(o is Instruccion)
                                {
                                    Instruccion aux = (Instruccion)o;
                                    Object s = aux.Ejecutar(ambito);
                                    if (s is NodoReturn)
                                    {
                                        return s;
                                    }
                                }
                                else if (o is Expresion)
                                {
                                    Expresion exp = (Expresion)o;
                                    Object s = exp.getValor(ambito);
                                    if (s is NodoReturn)
                                    {
                                        return s;
                                    }
                                }
                            }
                            return new Nulo();
                        }
                    }
                }
                else
                {
                    TError error = new TError("Semantico", "La condicional se espera un valor Booleano y se encontro: \""+tipoaux+"\" | Clase: " + this.clase + " | Archivo: " + ambito.archivo , linea, columna, false);
                }
            }
            catch(Exception e)
            {
                TError error = new TError("Ejecucion", "Error al Ejecutar condicional Si | Clase: " + this.clase + " | Archivo: " + ambito.archivo + " | Mensaje: " + e.Message, linea, columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            return null;
        }
    }
}
