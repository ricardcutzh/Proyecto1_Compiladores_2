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

namespace XForms.Objs
{
    class DamePregunta
    {
        String identificador;
        List<Expresion> parametros;

        String clase;
        int linea, col;
        String tipoPregunta;
        String esperados;

        public Objeto ob;

        public Ambito ambPregu;

        int numero;

        public DamePregunta(String identificador, List<Expresion> parametros, String clase, int linea, int col, String tipoPregunta, String Esperados, int numero)
        {
            this.identificador = identificador;
            this.parametros = parametros;
            this.clase = clase;
            this.linea = linea;
            this.col = col;
            this.tipoPregunta = tipoPregunta;
            this.esperados = Esperados;
            this.ob = null;
            this.numero = numero;
            this.ambPregu = null;
        }


        public Pregunta getPregunta(Ambito ambito)
        {
            try
            {
                Simbolo s = (Simbolo)ambito.getSimbolo(this.identificador.ToLower());
                if (s != null)
                {
                    if (s is Variable)
                    {
                        Variable v = (Variable)s;
                        Object valor = v.valor;
                        if (valor is Objeto)
                        {
                            Objeto ob = (Objeto)valor;
                            List<Object> valores = getValoresParams(ambito);
                            this.ob = ob;
                            if (ob.idClase.ToLower().Equals("pregunta"))
                            {
                                Ambito ambitoPregunta = ob.ambito; /*oteniendo el ambito de la pregunta*/
                                ClaveFuncion clave = new ClaveFuncion(this.identificador.ToLower(), "vacio", getNodosParametros(ambito));
                                Constructor constructor = (Constructor)ambitoPregunta.getConstructor(clave);
                                if(constructor!=null)
                                {
                                    Variable instruc = (Variable)ambitoPregunta.getSimbolo("instr");
                                    List<Instruccion> declaraciones = (List<Instruccion>)instruc.valor;/*ya tengo las instrucciones que hacen la ejecucion de delcaraciones*/
                                    if (existePropiedad("etiqueta", ambitoPregunta))
                                    {
                                        /*EN CASO QUE LA PROPIEDAD YA EXISTA EN LA PREGUNTA: RESETEO EL AMBITO*/
                                        ambitoPregunta = new Ambito(ambitoPregunta.Anterior, ambitoPregunta.idAmbito, ambitoPregunta.archivo);
                                        ambitoPregunta.agregarConstructor(clave, constructor);
                                        ambitoPregunta.agregarVariableAlAmbito("instr", instruc);
                                    }

                                    ambitoPregunta = constructor.seteaParametrosLocales(ambitoPregunta, valores);
                                    ejecutaLasDeclaracionesPregunta(ambitoPregunta, declaraciones);/*carga todo lo de la pregunta*/

                                    Pregunta p = new Pregunta(ambitoPregunta, this.identificador.ToLower(), this.numero);// formo la pregunta

                                    this.ambPregu = ambitoPregunta;

                                    return p;
                                }
                                else
                                {
                                    TError error = new TError("Semantico", "Para el simbolo: \"" + identificador + "\" No existe una pregunta de tipo "+this.tipoPregunta+" que rebiba los parametros especificados | Clase: " + clase + " | Archivo: " + ambito.archivo, linea, col, false);
                                    Estatico.ColocaError(error);
                                    Estatico.errores.Add(error);
                                }
                            }
                            else
                            {
                                TError error = new TError("Semantico", "Error Al Mostrar Pregunta: \"" + this.identificador + "\" ya que El simbolo  no es de Tipo "+this.tipoPregunta+" | Clase: " + clase + " | Archivo: " + ambito.archivo, linea, col, false);
                                Estatico.errores.Add(error);
                                Estatico.ColocaError(error);
                            }
                        }
                        else
                        {
                            TError error = new TError("Semantico", "Error Al Mostrar Pregunta: \"" + this.identificador + "\" ya que El simbolo  no es de Tipo "+this.tipoPregunta+" | Clase: " + clase + " | Archivo: " + ambito.archivo, linea, col, false);
                            Estatico.errores.Add(error);
                            Estatico.ColocaError(error);
                        }
                    }
                    else
                    {
                        TError error = new TError("Semantico", "Error Al Mostrar Pregunta: \"" + this.identificador + "\" ya que El simbolo  no es de Tipo "+this.tipoPregunta+" | Clase: " + clase + " | Archivo: " + ambito.archivo, linea, col, false);
                        Estatico.errores.Add(error);
                        Estatico.ColocaError(error);
                    }
                }
                else
                {
                    TError error = new TError("Semantico", "Error Al Mostrar Pregunta: \"" + this.identificador + "\" ya que este Simbolo no Existe en este Contexto | Clase: " + clase + " | Archivo: " + ambito.archivo, linea, col, false);
                    Estatico.errores.Add(error);
                    Estatico.ColocaError(error);
                }
            }
            catch(Exception e)
            {
                TError error = new TError("Ejecucion", "Error al ejecutar Pregunta: " + this.identificador + " | Clase: " + clase + " | Archivo: " + ambito.archivo + " | Mensaje: " + e.Message, linea, col, false);
                Estatico.ColocaError(error);
                Estatico.errores.Add(error);
            }
            return null;
        }


        private List<Object> getValoresParams(Ambito ambito)
        {
            List<Object> valores = new List<object>();
            foreach (Expresion e in this.parametros)
            {
                object val = e.getValor(ambito);
                if (val != null)
                {
                    valores.Add(val);
                }
            }
            return valores;
        }

        private List<NodoParametro> getNodosParametros(Ambito ambito)
        {
            List<NodoParametro> parametros = new List<NodoParametro>();
            foreach (Expresion e in this.parametros)
            {
                String tipo = e.getTipo(ambito);
                //PUEDE SER QUE NECESITE METER AQUI LO DE LOS ARREGLOS
                NodoParametro p = new NodoParametro("aux", tipo.ToLower(), false);
                parametros.Add(p);
            }
            return parametros;
        }

        private void ejecutaLasDeclaracionesPregunta(Ambito ambito, List<Instruccion> declaraciones)
        {
            foreach (Instruccion instruccion in declaraciones)
            {
                if (instruccion is DeclaracionFuncion)
                {
                    instruccion.Ejecutar(ambito);
                }
            }
            foreach (Instruccion instruccion in declaraciones)
            {
                if (instruccion is DeclaracionVar || instruccion is DeclaracionArreglo)
                {
                    instruccion.Ejecutar(ambito);
                }
            }
        }

        private Boolean existePropiedad(String p, Ambito m)
        {
            if (m.existeVariable(p.ToLower()))
            {
                return true;
            }
            return false;
        }
    }
}
