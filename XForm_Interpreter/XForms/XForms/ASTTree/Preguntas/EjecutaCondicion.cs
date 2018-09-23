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
using XForms.GUI.Condicion;

namespace XForms.ASTTree.Preguntas
{
    class EjecutaCondicion:NodoAST, Instruccion
    {
        int numero;
        String identificador;
        List<Expresion> parametros;
        NodoEstiloRespuesta estilo;
        Type casteo;

        public EjecutaCondicion(Type casteo, String identificador, List<Expresion> parametros, NodoEstiloRespuesta estilo, int numero, String clase, int linea, int col):base(linea, col, clase)
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

                    Expresion p1 = estilo.parametros.ElementAt(0);
                    Expresion p2 = estilo.parametros.ElementAt(1);

                    Object val1 = p1.getValor(ambito);
                    Object val2 = p2.getValor(ambito);

                    Objeto ob = dame.ob;
                    Ambito auxiliar = dame.ambPregu;

                    String etiquetaV = "Verdadero";
                    String etiquetaF = "Falso";

                    if (val1 is String && val2 is String)
                    {
                        etiquetaV = (String)val1;
                        etiquetaF = (String)val2;
                    }

                    MuestraForm:
                    Condicional c = new Condicional(p, etiquetaV, etiquetaF, linea, columna, clase, ambito.archivo);
                    c.ShowDialog();
                    String respuesta = c.respuesta;
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

                    if (c.salir != null)
                    {
                        return c.salir;
                    }
                }
            }
            catch(Exception e)
            {
                TError error = new TError("Ejecucion", "Se produjo un error de Ejecucion en la ejecucion de Pregunta De tipo: " + estilo.tipo + " | Clase: " + clase + " | Archivo: " + ambito.archivo + " | Mensaje: " + e.Message, linea, columna, false);
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
