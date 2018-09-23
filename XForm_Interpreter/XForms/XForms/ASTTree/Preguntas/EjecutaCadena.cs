using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.ASTTree.Interfaces;
using XForms.Simbolos;
using XForms.Objs;
using XForms.GramaticaIrony;
using XForms.ASTTree.Sentencias;
using XForms.ASTTree.Valores;
using XForms.GUI.Cadenas;

namespace XForms.ASTTree.Preguntas
{
    class EjecutaCadena : NodoAST, Instruccion
    {

        int numero;
        String identificador;
        List<Expresion> parametros;

        NodoEstiloRespuesta estilo;

        Type casteo;

        public EjecutaCadena(Type casteo,String identificador, List<Expresion> parametros, NodoEstiloRespuesta estilo, int numero,String clase, int linea, int col):base(linea, col, clase)
        {
            this.identificador = identificador;
            this.parametros = parametros;
            this.estilo = estilo;
            this.numero = numero;
            this.casteo = casteo;
        }


        Object valorResp = null;

        public object Ejecutar(Ambito ambito)
        {
            try
            {
                if(estilo.parametros.Count == 3)
                {
                    Expresion p1 = estilo.parametros.ElementAt(0);
                    Expresion p2 = estilo.parametros.ElementAt(1);
                    Expresion p3 = estilo.parametros.ElementAt(2);

                    Object min = p1.getValor(ambito);
                    Object max = p2.getValor(ambito);
                    Object fil = p3.getValor(ambito);

                    if(min is int && max is int && fil is int)
                    {
                        int cad_min = (int)min;
                        int cad_max = (int)max;
                        int cad_fil = (int)fil;

                        if(cad_max <= cad_min)
                        {
                            cad_max = cad_min + 2;
                        }
                        this.numero = Estatico.numPregunta;
                        Estatico.numPregunta++;
                        DamePregunta dame = new DamePregunta(identificador, parametros, clase, linea, columna, "Texto/Cadena", "(min, max, fil)", this.numero);

                        Pregunta p = dame.getPregunta(ambito);

                        if(p!=null)
                        {
                            Objeto ob = dame.ob;

                            Ambito auxiliar = dame.ambPregu;


                            MuestraForm:
                            /*AQUI YA CREO EL FORMULARIO*/
                            Cadenas c = new Cadenas(p, linea, columna, clase, ambito.archivo, cad_max, cad_min, cad_fil, true);
                            c.ShowDialog();

                            String resp = c.respuesta;

                            if(resp.Equals(""))
                            {
                                this.valorResp = Estatico.respuestaPorDefecto(this.casteo);
                            }
                            else
                            {
                                this.valorResp = Estatico.casteaRespuestaA(resp, valorResp, this.casteo);
                            }
                            if(valorResp is null)
                            {
                                TError error = new TError("Semantico", "No se logro Castear la respuesta a tipo: "+this.casteo.ToString()+" | Clase: " + clase + " | Archivo: " + ambito.archivo, linea, columna, false);
                                Estatico.errores.Add(error);
                                Estatico.ColocaError(error);
                                goto MuestraForm;
                            }

                            // LLAMO A RESPUESTA
                            if (!llamaARespuesta(auxiliar, valorResp))
                            {
                                goto MuestraForm;
                            }

                            PreguntaAlmacenada pr = new PreguntaAlmacenada(this.identificador, p.etiqueta, this.numero);
                            pr.addAnswer(this.valorResp.ToString());
                            Estatico.resps.Add(pr);

                            ob.ambito = dame.ambPregu;

                            if(c.salir!=null)
                            {
                                return c.salir;
                            }
                        }
                    }
                    else
                    {
                        TError error = new TError("Semantico", "Error Al Mostrar Pregunta: \""+this.identificador+"\" ya que se espera que sus parametros sean enteros: (min, max, fila) | Clase: "+clase+ " | Archivo: "+ambito.archivo, linea, columna, false);
                        Estatico.errores.Add(error);
                        Estatico.ColocaError(error);
                    }
                }
                else
                {
                    this.numero = Estatico.numPregunta;
                    Estatico.numPregunta++;
                    DamePregunta dame = new DamePregunta(identificador, parametros, clase, linea, columna, "Texto/Cadena", "()", this.numero);

                    Pregunta p = dame.getPregunta(ambito);

                    if (p != null)
                    {
                        Objeto ob = dame.ob;

                        Ambito auxiliar = dame.ambPregu;

                        MuestraForm1:
                        /*AQUI YA CREO EL FORMULARIO*/
                        Cadenas c = new Cadenas(p, linea, columna, clase, ambito.archivo, -1, -2, -1, false);
                        c.ShowDialog();

                        String resp = c.respuesta;
                        if (resp.Equals(""))
                        {
                            this.valorResp = Estatico.respuestaPorDefecto(this.casteo);
                        }
                        else
                        {
                            this.valorResp = Estatico.casteaRespuestaA(resp, valorResp, this.casteo);
                        }
                        if (valorResp is null)
                        {
                            TError error = new TError("Semantico", "No se logro Castear la respuesta a tipo: " + this.casteo.ToString() + " que es lo que se espera! | Clase: " + clase + " | Archivo: " + ambito.archivo, linea, columna, false);
                            Estatico.errores.Add(error);
                            Estatico.ColocaError(error);
                            goto MuestraForm1;
                        }

                        // LLAMO A RESPUESTA
                        if(!llamaARespuesta(auxiliar, valorResp))
                        {
                            goto MuestraForm1;
                        }

                        PreguntaAlmacenada pr = new PreguntaAlmacenada(this.identificador, p.etiqueta, this.numero);
                        pr.addAnswer(this.valorResp.ToString());
                        Estatico.resps.Add(pr);

                        ob.ambito = dame.ambPregu;

                        if (c.salir != null)
                        {
                            return c.salir;
                        }
                    }
                }
            }
            catch(Exception e)
            {
                TError error = new TError("Ejecucion", "Error al ejecutar Pregunta: " +this.identificador+ " | Clase: " + clase + " | Archivo: " + ambito.archivo + " | Mensaje: " + e.Message, linea, columna, false);
                Estatico.ColocaError(error);
                Estatico.errores.Add(error);
            }
            return new Nulo();
        }


        private Boolean llamaARespuesta(Ambito ambitoPregunta, Object nuevoVal)
        {
            Simbolo s = (Simbolo)ambitoPregunta.getSimbolo("respuesta");
            if(s!=null)
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

                if(v.valor.ToString().Equals(primerVal))
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
