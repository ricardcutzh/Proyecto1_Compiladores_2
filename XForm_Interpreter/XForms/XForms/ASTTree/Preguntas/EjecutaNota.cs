using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.ASTTree.Interfaces;
using XForms.Simbolos;
using XForms.Objs;
using XForms.GramaticaIrony;
using XForms.ASTTree.Instrucciones;
using XForms.GUI.Notas;
namespace XForms.ASTTree.Preguntas
{
    class EjecutaNota:NodoAST, Instruccion
    {

        String identificador;
        List<Expresion> expresiones;

        Boolean llamaMostar;

        public EjecutaNota(String identificador,List<Expresion>expresiones,  String clase, int linea, int col, Boolean mostrar):base(linea, col, clase)
        {
            this.expresiones = expresiones;
            this.identificador = identificador;
            this.llamaMostar = mostrar;
        }

        

        public object Ejecutar(Ambito ambito)
        {
            try
            {
                Simbolo s = (Simbolo)ambito.getSimbolo(this.identificador.ToLower());
                if (s != null)
                {
                    if(s is Variable)
                    {
                        Variable v = (Variable)s;
                        Object valor = v.valor;
                        if (valor is Objeto)
                        {
                            Objeto ob = (Objeto)valor;

                            List<Object> valores = getValoresParams(ambito);

                            if (ob.idClase.ToLower().Equals("pregunta"))
                            {
                                Ambito ambitoPregunta = ob.ambito; // obtengo el ambito de la pregunta

                                /*obtendre el constructor de la pregunta para poder setear los parametros que hagan falta*/
                                ClaveFuncion clave = new ClaveFuncion(this.identificador.ToLower(), "vacio", getNodosParametros(ambito));
                                Constructor constructor = (Constructor)ambitoPregunta.getConstructor(clave);
                                if(constructor!=null)
                                {
                                    /*aqui ya setee los parametros que venian en la pregunta en el ambito global ahora voy a ejecutar las declaraciones sobre este ambito*/
                                    ambitoPregunta = constructor.seteaParametrosLocales(ambitoPregunta, valores);

                                    Variable instruc = (Variable)ambitoPregunta.getSimbolo("instr");
                                    List<Instruccion> declaraciones = (List<Instruccion>)instruc.valor;/*ya tengo las instrucciones que hacen la ejecucion de delcaraciones*/
                                    ejecutaLasDeclaracionesPregunta(ambitoPregunta, declaraciones);/*carga todo lo de la pregunta*/

                                    Pregunta p = new Pregunta(ambitoPregunta, this.identificador.ToLower());// formo la pregunta

                                    Nota n = new Nota(p);

                                    n.ShowDialog();

                                }
                                else
                                {
                                    TError error = new TError("Semantico", "Para el simbolo: \"" + identificador + "\" No existe una pregunta de tipo Nota que rebiba los parametros especificados | Clase: " + clase + " | Archivo: " + ambito.archivo, linea, columna, false);
                                    Estatico.ColocaError(error);
                                    Estatico.errores.Add(error);
                                }
                            }
                            else
                            {
                                TError error = new TError("Semantico", "El simbolo: \"" + identificador + "\" No es un objeto de tipo Pregunta, por lo que no es aplicable la instrucccion | Clase: " + clase + " | Archivo: " + ambito.archivo, linea, columna, false);
                                Estatico.ColocaError(error);
                                Estatico.errores.Add(error);
                            }
                        }
                        else
                        {
                            TError error = new TError("Semantico", "El simbolo: \"" + identificador + "\" No es un objeto de tipo Nota, por lo que no es aplicable la instrucccion | Clase: " + clase + " | Archivo: " + ambito.archivo, linea, columna, false);
                            Estatico.ColocaError(error);
                            Estatico.errores.Add(error);
                        }
                    }
                    else
                    {
                        TError error = new TError("Semantico", "El simbolo: \"" + identificador + "\" No es un objeto de tipo Nota, por lo que no es aplicable la instrucccion | Clase: " + clase + " | Archivo: " + ambito.archivo, linea, columna, false);
                        Estatico.ColocaError(error);
                        Estatico.errores.Add(error);
                    }
                }
                else
                {
                    TError error = new TError("Semantico", "La referencia a la pregunta de tipo Nota: \""+identificador+"\" es Nulo, no existe en este ambito | Clase: "+clase+" | Archivo: "+ambito.archivo, linea, columna, false);
                    Estatico.ColocaError(error);
                    Estatico.errores.Add(error);
                }
            }
            catch(Exception e)
            {
                TError error = new TError("Ejecucion", "Error al ejecutar la Instrccion Nota() | Clase: "+clase+" | Archivo: "+ambito.archivo+" | Mensaje: "+e.Message, linea, columna, false);
                Estatico.ColocaError(error);
                Estatico.errores.Add(error);
            }
            return new Nulo();
        }


        private List<NodoParametro> getNodosParametros(Ambito ambito)
        {
            List<NodoParametro> parametros = new List<NodoParametro>();
            foreach (Expresion e in this.expresiones)
            {
                String tipo = e.getTipo(ambito);
                //PUEDE SER QUE NECESITE METER AQUI LO DE LOS ARREGLOS
                NodoParametro p = new NodoParametro("aux", tipo.ToLower(), false);
                parametros.Add(p);
            }
            return parametros;
        }

        private List<Object> getValoresParams(Ambito ambito)
        {
            List<Object> valores = new List<object>();
            foreach (Expresion e in this.expresiones)
            {
                object val = e.getValor(ambito);
                if (val != null)
                {
                    valores.Add(val);
                }
            }
            return valores;
        }


        private void ejecutaLasDeclaracionesPregunta(Ambito ambito, List<Instruccion> declaraciones)
        {
            foreach(Instruccion instruccion in declaraciones)
            {
                if(instruccion is DeclaracionFuncion)
                {
                    instruccion.Ejecutar(ambito);
                }
            }
            foreach(Instruccion instruccion in declaraciones)
            {
                if(instruccion is DeclaracionVar || instruccion is DeclaracionArreglo)
                {
                    instruccion.Ejecutar(ambito);
                }
            }
        }
    }
}
