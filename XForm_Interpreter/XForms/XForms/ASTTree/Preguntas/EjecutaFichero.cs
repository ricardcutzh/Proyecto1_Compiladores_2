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
using XForms.GUI.Ficheros;
using XForms.ASTTree.Sentencias;
using XForms.ASTTree.Valores;

namespace XForms.ASTTree.Preguntas
{
    class EjecutaFichero:NodoAST, Instruccion
    {

        int numero;
        String identificador;
        List<Expresion> expresiones;

        Expresion extensiones;


        public EjecutaFichero(String identificador, List<Expresion> expresiones, int numero, String clase, int linea, int col):base(linea, col, clase)
        {
            this.identificador = identificador;
            this.expresiones = expresiones;
            this.numero = numero;
            this.extensiones = null;
        }

        public EjecutaFichero(String identificador, List<Expresion> expresiones, Expresion extensiones, int numero, String clase, int linea, int col):base(linea, col, clase)
        {
            this.identificador = identificador;
            this.expresiones = expresiones;
            this.numero = numero;
            this.extensiones = extensiones;
        }

        public object Ejecutar(Ambito ambito)
        {
            try
            {
                Simbolo s = (Simbolo)ambito.getSimbolo(this.identificador.ToLower());
                if(s!=null)
                {
                    if(s is Variable)
                    {
                        Variable v = (Variable)s;
                        Object valor = v.valor;
                        if(valor is Objeto)
                        {
                            Objeto ob = (Objeto)valor;

                            List<Object> valores = getValoresParams(ambito);

                            if (ob.idClase.ToLower().Equals("pregunta"))
                            {
                                Ambito ambitoPregunta = ob.ambito; /*oteniendo el ambito de la pregunta*/
                                /*obtendre el constructor de la pregunta para poder setear los parametros que hagan falta*/
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

                                    if(this.extensiones!=null)
                                    {
                                        /*AQUI CUANDO ADMITEN SOLO CIERTAS EXTENSIONES*/
                                        Object ext = this.extensiones.getValor(ambito);
                                        if(ext is String)
                                        {
                                            Fichero f = new Fichero(p, (String)ext, linea, columna, clase,ambito.archivo);
                                            f.ShowDialog();
                                            String resp = f.rutaArchivo;
                                            if (resp.Equals(""))
                                            {
                                                resp = "No se subio ningun Archivo!";
                                            }

                                            PreguntaAlmacenada pr = new PreguntaAlmacenada(p.idPregunta, p.etiqueta, this.numero);
                                            pr.addAnswer(" Archivo Almacenado en: " + resp);
                                            Estatico.resps.Add(pr);

                                            ob.ambito = ambitoPregunta;/*ASIGNO EL NUEVO AMBITO A OB*/

                                            if(f.salir!=null)
                                            {
                                                return f.salir;
                                            }
                                        }
                                        else
                                        {
                                            TError error = new TError("Semantico", "Para el simbolo: \"" + identificador + "\", Se espera una cadena especificando el formato de las Extensiones de Archivos | Clase: " + clase + " | Archivo: " + ambito.archivo, linea, columna, false);
                                            Estatico.ColocaError(error);
                                            Estatico.errores.Add(error);
                                        }
                                    }
                                    else
                                    {
                                        /*AQUI CUANDO ADMITEN CUALQUIER EXTENSION*/
                                        Fichero f = new Fichero(p, "", linea, columna, clase, ambito.archivo);
                                        f.ShowDialog();
                                        String resp = f.rutaArchivo;
                                        if(resp.Equals(""))
                                        {
                                            resp = "No se subio ningun Archivo!";
                                        }

                                        PreguntaAlmacenada pr = new PreguntaAlmacenada(p.idPregunta, p.etiqueta, this.numero);
                                        pr.addAnswer(" Archivo Almacenado en: "+resp);
                                        Estatico.resps.Add(pr);

                                        ob.ambito = ambitoPregunta;/*ASIGNO EL NUEVO AMBITO A OB*/

                                        if (f.salir != null)
                                        {
                                            return f.salir;
                                        }
                                    }
                                    
                                }
                                else
                                {
                                    TError error = new TError("Semantico", "Para el simbolo: \"" + identificador + "\" No existe una pregunta de tipo Fichero que rebiba los parametros especificados | Clase: " + clase + " | Archivo: " + ambito.archivo, linea, columna, false);
                                    Estatico.ColocaError(error);
                                    Estatico.errores.Add(error);
                                }
                            }
                            else
                            {
                                TError error = new TError("Semantico", "El simbolo: \"" + this.identificador + "\"  no es de tipo Pregunta | Clase: " + clase + " | Archivo: " + ambito.archivo , linea, columna, false);
                                Estatico.ColocaError(error);
                                Estatico.errores.Add(error);
                            }
                        }
                        else
                        {
                            TError error = new TError("Semantico", "El simbolo: \"" + this.identificador + "\"  no es de tipo Pregunta | Clase: " + clase + " | Archivo: " + ambito.archivo , linea, columna, false);
                            Estatico.ColocaError(error);
                            Estatico.errores.Add(error);
                        }
                    }
                    else
                    {
                        TError error = new TError("Semantico", "El simbolo: \"" + this.identificador + "\"  no es de tipo Pregunta | Clase: " + clase + " | Archivo: " + ambito.archivo , linea, columna, false);
                        Estatico.ColocaError(error);
                        Estatico.errores.Add(error);
                    }
                }
                else
                {
                    TError error = new TError("Semantico", "No existe la pregunta de tipo Fichero: \""+this.identificador+"\" | Clase: " + clase + " | Archivo: " + ambito.archivo, linea, columna, false);
                    Estatico.ColocaError(error);
                    Estatico.errores.Add(error);
                }
            }
            catch(Exception e)
            {
                TError error = new TError("Ejecucion", "Error al ejecutar el guardar un Fichero | Clase: "+clase+" | Archivo: "+ambito.archivo+" | Mensaje: "+e.Message, linea, columna, false);
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
