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
using XForms.GUI.FechasHoras;

namespace XForms.ASTTree.Preguntas
{
    class EjecutaFechaHora:NodoAST, Instruccion
    {
        int numero;
        String identificador;
        List<Expresion> parametros;
        NodoEstiloRespuesta estilo;
        Type casteo;

        public EjecutaFechaHora(Type casteo, String identificador, List<Expresion> parametros, NodoEstiloRespuesta estilo, int numero, String clase, int linea, int col):base(linea, col, clase)
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
                DamePregunta dame = new DamePregunta(identificador, parametros, clase, linea, columna, estilo.tipo, "", this.numero);
                Pregunta p = dame.getPregunta(ambito);
                if(p!=null)
                {
                    Objeto ob = dame.ob;
                    Ambito auxiliar = dame.ambPregu;
                    int tipo = 0;
                    if(estilo.tipo.Equals("fechahora"))
                    {
                        tipo = 0;
                    }
                    else if(estilo.tipo.Equals("hora"))
                    {
                        tipo = 2;
                    }
                    else if(estilo.tipo.Equals("fecha"))
                    {
                        tipo = 1;
                    }

                    MuestraForm1:
                    FechasHoras f = new FechasHoras(p, tipo);
                    f.ShowDialog();
                    String respuesta = f.respuesta;
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
                        goto MuestraForm1;
                    }
                    if (!llamaARespuesta(auxiliar, valorResp))
                    {
                        goto MuestraForm1;
                    }
                    PreguntaAlmacenada nueva = new PreguntaAlmacenada(this.identificador, p.etiqueta, this.numero);
                    nueva.addAnswer(this.valorResp.ToString());
                    Estatico.resps.Add(nueva);

                    ob.ambito = dame.ambPregu;

                    if (f.salir != null)
                    {
                        return f.salir;
                    }
                }
            }
            catch(Exception e)
            {
                TError error = new TError("Ejecucion", "Se produjo un error de Ejecucion en la ejecucion de Pregunta De tipo: "+estilo.tipo+" | Clase: " + clase + " | Archivo: " + ambito.archivo + " | Mensaje: " + e.Message, linea, columna, false);
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
