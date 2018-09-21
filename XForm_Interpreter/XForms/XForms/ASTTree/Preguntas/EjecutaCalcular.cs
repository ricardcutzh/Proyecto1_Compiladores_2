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

namespace XForms.ASTTree.Preguntas
{
    class EjecutaCalcular:NodoAST, Instruccion
    {
        int numero;
        String identificador;
        List<Expresion> parametros;
        NodoEstiloRespuesta estilo;
        Type casteo;

        public EjecutaCalcular(Type casteo, String identificador, List<Expresion> parametros, NodoEstiloRespuesta estilo, int numero, String clase, int linea, int col):base(linea, col, clase)
        {
            this.casteo = casteo;
            this.identificador = identificador;
            this.parametros = parametros;
            this.estilo = estilo;
        }

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

                    llamadaACalcular(auxiliar, null);

                    llamadaACalcular(auxiliar, null);

                    ob.ambito = dame.ambPregu;
                }
            }
            catch(Exception e)
            {
                TError error = new TError("Ejecucion", "Error al ejecutar el metodo Calcular: "+this.identificador+" | Clase: "+clase+" | Archivo: "+ambito.archivo+" | Mensaje: "+e.Message, linea, columna, false);
                Estatico.ColocaError(error);
                Estatico.errores.Add(error);
            }
            return new Nulo();
        }

        private Boolean llamadaACalcular(Ambito ambitoPregunta, Object nuevoVal)
        {
            Simbolo s = (Simbolo)ambitoPregunta.getSimbolo("respuesta");
            if(s!=null)
            {
                Variable v = (Variable)s;

                String primerVal = (v.valor.ToString());

                Ambito aux = ambitoPregunta.Anterior;
                ambitoPregunta.Anterior = null;

                Llamada l = new Llamada("calcular", linea, columna, clase);
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


        private Type dameTipo(Ambito ambitoPregunta)
        {
            Simbolo s = (Simbolo)ambitoPregunta.getSimbolo("respuesta");
            if(s!=null)
            {
                String tipo = s.Tipo.ToLower();
                switch(tipo)
                {
                    case "booleano":
                        {
                            return typeof(Boolean);
                        }
                    case "entero":
                        {
                            return typeof(int);
                        }
                    case "decimal":
                        {
                            return typeof(double);
                        }
                    case "cadena":
                        {
                            return typeof(String);
                        }
                    case "fecha":
                        {
                            return typeof(Date);
                        }
                    case "hora":
                        {
                            return typeof(Hour);
                        }
                    case "fechahora":
                        {
                            return typeof(DateTime);
                        }
                }
            }
            return null;
        }

    }
}
