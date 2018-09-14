using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.ASTTree.Interfaces;
using XForms.Simbolos;
using XForms.Objs;
using XForms.GramaticaIrony;
namespace XForms.ASTTree.Sentencias
{
    class llamadaConstructor:NodoAST, Instruccion
    {

        List<Expresion> expresiones;

        public llamadaConstructor(List<Expresion> expresiones, String clase, int linea, int col) : base(linea, col, clase)
        {
            this.expresiones = expresiones;
        }

        public object Ejecutar(Ambito ambito)
        {
            try
            {
                List<Object> valores = getValoresParam(ambito);
                ClaveFuncion clave = new ClaveFuncion("padre", "vacio", getNodoParametros(ambito));
                Ambito aux = buscaElGlobal(ambito);
                Constructor c = aux.getConstructor(clave);
                if(c!=null)
                {
                    Ambito local = new Ambito(ambito, "padre", ambito.archivo);
                    local = c.seteaParametrosLocales(local, valores);
                    c.Ejecutar(local);
                    return new Nulo();
                }
                else if(c == null && this.expresiones.Count == 0)
                {
                    return new Nulo();
                }
                else
                {
                    TError error = new TError("Ejecucion", "No existe un construcotr de la clase padre que contenga los parametros indicados en Clase: " + this.clase + " | Archivo: " + ambito.archivo , this.linea, this.columna, false);
                    Estatico.errores.Add(error);
                    Estatico.ColocaError(error);
                }
            }
            catch(Exception e)
            {
                TError error = new TError("Ejecucion", "Error al intentar ejecutar Super en Clase: " + this.clase + " | Archivo: " + ambito.archivo + " | Error: " + e.Message, this.linea, this.columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            return new Nulo();
        }


        private List<NodoParametro> getNodoParametros(Ambito ambito)
        {
            List<NodoParametro> parametros = new List<NodoParametro>();
            foreach (Expresion e in this.expresiones)
            {
                //String tipo = e.getTipo(ambito);
                String tipo = e.getTipo(ambito);
                NodoParametro p = new NodoParametro("aux", tipo.ToLower(), false);
                parametros.Add(p);
            }
            return parametros;
        }

        private List<Object> getValoresParam(Ambito ambito)
        {
            List<Object> valores = new List<object>();
            foreach (Expresion e in this.expresiones)
            {
                //object val = e.getValor(ambito);
                object val = e.getValor(ambito);
                if (val != null)
                {
                    valores.Add(val);
                }
            }
            return valores;
        }

        private Ambito buscaElGlobal(Ambito am)
        {
            Ambito aux = am;
            while(aux.Anterior!=null)
            {
                aux = aux.Anterior;
            }

            return aux;
        }
    }
}
