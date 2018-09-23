using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.ASTTree.Sentencias;
using XForms.ASTTree.Valores;
using XForms.ASTTree.Interfaces;
using XForms.Simbolos;
using XForms.Objs;
using XForms.GramaticaIrony;
using XForms.GUI.Select;

namespace XForms.ASTTree.Preguntas
{
    class EjecutaSeleccion:NodoAST, Instruccion
    {
        int numero;
        String identificador;
        List<Expresion> parametros;
        NodoEstiloRespuesta estilo;
        Type casteo;

        public EjecutaSeleccion(Type casteo, String identificador, List<Expresion> parametros, NodoEstiloRespuesta estilo, int numero, String clase, int linea, int col) :base(linea, col, clase)
        {
            this.casteo = casteo;
            this.identificador = identificador;
            this.parametros = parametros;
            this.estilo = estilo;
            this.numero = numero;
        }
        Object valorResp = null;
        public object Ejecutar(Ambito ambito)
        {
            try
            {
                this.numero = Estatico.numPregunta;
                Estatico.numPregunta++;
                DamePregunta dame = new DamePregunta(identificador, parametros, clase, linea, columna, estilo.tipo, "", this.numero);
                Pregunta p = dame.getPregunta(ambito);
                if(p!=null)
                {
                    object l = estilo.parametros.ElementAt(0).getValor(ambito);
                    if(l is Objeto)
                    {
                        Objeto o = (Objeto)l;
                        if(o.idClase.ToLower().Equals("opciones"))
                        {
                            Objeto ob = dame.ob;
                            Ambito auxiliar = dame.ambPregu;

                            Opciones listado = dameOpciones(o.ambito);

                            int tipo = 0;

                            if(estilo.tipo.Equals("seleccionar"))
                            {
                                tipo = 0;
                            }
                            else
                            {
                                tipo = 1;
                            }

                            /*MUESTRA EL FORM*/
                            MuestraForm:
                            Seleccionar s = new Seleccionar(p, tipo, listado, linea, columna, clase, ambito.archivo);
                            s.ShowDialog();
                            String respuesta = s.respuesta;
                            if (respuesta.Equals(""))
                            {
                                this.valorResp = Estatico.respuestaPorDefecto(this.casteo);
                            }
                            else
                            {
                                this.valorResp = Estatico.casteaRespuestaA(respuesta, valorResp, this.casteo);
                            }
                            if (valorResp is null)
                            {
                                TError error = new TError("Semantico", "No se logro Castear la respuesta a tipo: " + this.casteo.ToString() + " | Clase: " + clase + " | Archivo: " + ambito.archivo, linea, columna, false);
                                Estatico.errores.Add(error);
                                Estatico.ColocaError(error);
                                goto MuestraForm;
                            }
                            if (!llamaARespuesta(auxiliar, valorResp))
                            {
                                goto MuestraForm;
                            }
                            PreguntaAlmacenada nueva = new PreguntaAlmacenada(this.identificador, p.etiqueta, this.numero);
                            nueva.addAnswer(this.valorResp.ToString());
                            Estatico.resps.Add(nueva);

                            ob.ambito = dame.ambPregu;

                            if (s.salir != null)
                            {
                                return s.salir;
                            }

                        }
                        else
                        {
                            TError error = new TError("Semantico", "El parametro recibido para la pregunta: " + estilo.tipo + " requiere un objeto de tipo Opciones | Clase: " + clase + " | Archivo: " + ambito.archivo, linea, columna, false);
                            Estatico.errores.Add(error);
                            Estatico.ColocaError(error);
                        }
                    }
                    else
                    {
                        TError error = new TError("Semantico", "El parametro recibido para la pregunta: " + estilo.tipo + " requiere un objeto de tipo Opciones | Clase: " + clase + " | Archivo: " + ambito.archivo, linea, columna, false);
                        Estatico.errores.Add(error);
                        Estatico.ColocaError(error);
                    }
                }
            }
            catch(Exception e)
            {
                TError error = new TError("Ejecucion", "Error al ejecutar pregunta:" + this.identificador + " de tipo: "+estilo.tipo+" | Clase: " + clase + " | Archivo: " + ambito.archivo + " | Mensaje: " + e.Message, linea, columna, false);
                Estatico.ColocaError(error);
                Estatico.errores.Add(error);
            }
            return new Nulo();
        }

        private Opciones dameOpciones(Ambito ambitoObjet)
        {
            try
            {
                Simbolo s = (Simbolo)ambitoObjet.getSimbolo("cutz");
                if (s != null)
                {
                    Variable v = (Variable)s;

                    Opciones op = (Opciones)v.valor;

                    return op;
                }
                return new Opciones("nada", new Ambito(null, "nada"));
            }
            catch
            {
                return new Opciones("nada", new Ambito(null, "nada"));
            }
        }

        private Boolean llamaARespuesta(Ambito ambitoPregunta, Object nuevoVal)
        {
            Simbolo s = (Simbolo)ambitoPregunta.getSimbolo("respuesta");
            if (s != null)
            {
                Variable v = (Variable)s;

                String primerVal = (v.valor.ToString());

                Ambito aux = ambitoPregunta.Anterior;
                ambitoPregunta.Anterior = null;

                Llamada l = new Llamada("respuesta", linea, columna, clase);
                l.AddExpresion(new ValorPrimitivo(nuevoVal, linea, columna, clase));
                LLamadaFuncion ll = new LLamadaFuncion(clase, linea, columna, l);

                ll.Ejecutar(ambitoPregunta);

                ambitoPregunta.Anterior = aux;

                if (v.valor.ToString().Equals(primerVal))
                {
                    /*SI EL VALOR NO CAMBIO!*/
                    return false;
                }

                return true;

            }
            return false;
        }

       
    }
}
