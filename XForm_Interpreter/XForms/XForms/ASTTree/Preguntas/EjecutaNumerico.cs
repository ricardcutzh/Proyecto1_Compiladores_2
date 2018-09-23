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
using XForms.GUI.Numericos;

namespace XForms.ASTTree.Preguntas
{
    class EjecutaNumerico:NodoAST, Instruccion
    {

        int numero;
        String identificador;
        List<Expresion> parametros;
        NodoEstiloRespuesta estilo;
        Type casteo;

        public EjecutaNumerico(Type casteo, String identificador, List<Expresion> parametros, NodoEstiloRespuesta estilo, int numero, String clase, int linea, int col):base(linea, col, clase)
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
                DamePregunta pr = new DamePregunta(this.identificador, parametros, clase, linea, columna, "Entero, Rango, Decimal", "", this.numero);
                Pregunta p = pr.getPregunta(ambito);
                if(p!=null)
                {
                    Objeto ob = pr.ob;
                    Ambito auxiliar = pr.ambPregu;
                    if(estilo.tipo.ToLower().Equals("entero") || estilo.tipo.ToLower().Equals("decimal"))
                    {
                        MuestraForm1:
                        Numerico n = new Numerico(p, linea, columna, 0, 0, false, estilo.tipo.Equals("decimal") ? true : false);
                        n.ShowDialog();
                        String respuesta = n.respuesta;
                        if(respuesta.Equals(""))
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
                            goto MuestraForm1;
                        }

                        if(!llamaARespuesta(auxiliar, valorResp))
                        {
                            goto MuestraForm1;
                        }

                        PreguntaAlmacenada nueva = new PreguntaAlmacenada(this.identificador, p.etiqueta, this.numero);
                        nueva.addAnswer(this.valorResp.ToString());
                        Estatico.resps.Add(nueva);

                        ob.ambito = pr.ambPregu;

                        if(n.salir!=null)
                        {
                            return n.salir;
                        }
                    }
                    else if(estilo.tipo.ToLower().Equals("rango"))
                    {
                        Expresion p1 = this.estilo.parametros.ElementAt(0);//INFERIOR
                        Expresion p2 = this.estilo.parametros.ElementAt(1);//SUPERIOR
                        Object inf = p1.getValor(ambito);
                        Object sup = p2.getValor(ambito);
                        if(inf is int && sup is int)
                        {
                            int superior = (int)sup;
                            int inferior = (int)inf;
                            MuestraForm2:
                            Numerico n = new Numerico(p, linea, columna, inferior, superior, true, false);
                            n.ShowDialog();
                            String respuesta = n.respuesta;
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
                                goto MuestraForm2;
                            }

                            if (!llamaARespuesta(auxiliar, valorResp))
                            {
                                goto MuestraForm2;
                            }

                            PreguntaAlmacenada nueva = new PreguntaAlmacenada(this.identificador, p.etiqueta, this.numero);
                            nueva.addAnswer(this.valorResp.ToString());
                            Estatico.resps.Add(nueva);

                            ob.ambito = pr.ambPregu;

                            if (n.salir != null)
                            {
                                return n.salir;
                            }
                        }
                        else
                        {
                            TError error = new TError("Semantico", "Para la visualizacion de pregunta de Rango recibe parametros: Rango(entero, entero) | Clase: "+clase+" | Archivo: "+ambito.archivo, linea, columna, false);
                            Estatico.errores.Add(error);
                            Estatico.ColocaError(error);
                        }
                    }
                }
            }
            catch(Exception e)
            {
                TError error = new TError("Ejecucion", "Se produjo un error de Ejecucion en la ejecucion de Pregunta numerica | Clase: "+clase+ " | Archivo: "+ambito.archivo+" | Mensaje: "+e.Message, linea, columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            return new Nulo();
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
