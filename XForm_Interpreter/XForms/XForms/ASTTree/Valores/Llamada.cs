using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.ASTTree.Interfaces;
using XForms.Simbolos;
using XForms.GramaticaIrony;
using XForms.Objs;

namespace XForms.ASTTree.Valores
{
    class Llamada :NodoAST, Expresion//ME REFIERO AUNA LLAMADA DE UNA FUNCION
    {
        public String id;
        List<Expresion> expresiones;
        public Estatico.Vibililidad vibililidad;

        public Llamada(String id,int linea, int col, String clase):base(linea, col, clase)
        {
            this.id = id;
            this.expresiones = new List<Expresion>();
        }

        public void AddExpresion(Expresion exp)
        {
            this.expresiones.Add(exp);
        }

        Object ValorAux = null;
        public string getTipo(Ambito ambito)
        {
            Object val = null;
            if (ValorAux == null)
            {
                val = getValor(ambito);
            }
            else
            {
                val = this.ValorAux;
            }
            if (val is bool)
            {
                return "Booleano";
            }
            else if (val is string)
            {
                return "Cadena";
            }
            else if (val is int)
            {
                return "Entero";
            }
            else if (val is double)
            {
                return "Decimal";
            }
            else if (val is System.DateTime)
            {
                return "FechaHora";
            }
            else if (val is Date)
            {
                return "Fecha";
            }
            else if (val is Hour)
            {
                return "Hora";
            }
            else if (val is Nulo)
            {
                return "Nulo";
            }
            else if (val is Objeto)
            {
                return ((Objeto)val).idClase.ToLower();
            }
            else if (val is Vacio)
            {
                return "vacio";
            }
            else if(val is Arreglo)
            {
                return "Arreglo";
            }
            //AQUI FALTA EL TIPO OBJETO
            return "vacio";
        }

        public object getValor(Ambito ambito)
        {
            try
            {
                List<Object> valores = getValoresParam(ambito);
                /// GENERO MI CLAVE PARA PODER OBTENER LA FUNCION QUE DESEO
                    Clave clave = new Clave(this.id.ToLower(), getNodoParametros(ambito), "");
                /// LLAMO A LA FUNCION QUE DESEO EJECUTAR
                Ambito aux = AmbitoDeClase(ambito);
                    Funcion f = aux.GetFuncion(clave);
                if(f!=null)
                {
                    this.vibililidad = f.Vibililidad;
                    ///CREO EL AMBITO DE LA FUNCION
                    Ambito auxliar = new Ambito(aux, this.clase.ToLower(), ambito.archivo);
                    this.vibililidad = f.Vibililidad;
                    ///SETEO LOS PARAMETROS QUE RECIBE
                    auxliar = f.seteaParametrosLocales(auxliar, valores);

                    ///OBTENGO EL VALOR QUE DEFINE
                    Object valor = f.Ejecutar(auxliar);
                    if(valor is NodoReturn)
                    {
                        Object valorReal = ((NodoReturn)valor).valor;
                        String tipoEsperado = f.Tipo.ToLower();
                        if(tipoEsperado.Equals(((NodoReturn)valor).tipo.ToLower()))
                        {
                            
                            this.ValorAux = valorReal;
                            return valorReal;
                        }
                        else
                        {
                            TError error = new TError("Semantico", "El tipo de retorno no coincide: Tipo de Funcion: \"" + tipoEsperado + "\", encontrado: \"" + ((NodoReturn)valor).tipo.ToLower() + "\" | Clase: " + this.clase + " | " + ambito.archivo, this.linea, this.columna, false);
                            Estatico.errores.Add(error);
                            Estatico.ColocaError(error);
                            return new Nulo();
                        }
                    }
                }
                else
                {
                    TError error = new TError("Semantico", "No existe funcion: "+this.id+" que reciba parametro: "+getMensajeError(ambito)+" | Clase: "+this.clase+" | "+ambito.archivo, this.linea, this.columna, false);
                    Estatico.errores.Add(error);
                    Estatico.ColocaError(error);
                }
            }
            catch(Exception e)
            {
                TError error = new TError("Ejecucion", "Error al Buscar la funcion, en ejecucion | Erro: "+e.Message, this.linea, this.columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            return new Nulo();
        }

        private String getMensajeError(Ambito ambito)
        {
            String cad = "(";
            foreach(NodoParametro p in getNodoParametros(ambito))
            {
                cad += " \"" + p.tipo + "\" ";
            }
            return cad += ")";
        }

        private List<NodoParametro> getNodoParametros(Ambito ambito)
        {
            List<NodoParametro> parametros = new List<NodoParametro>();
            foreach(Expresion e in this.expresiones)
            {
                String tipo = e.getTipo(ambito);
                NodoParametro p = new NodoParametro("aux", tipo.ToLower(), false);
                parametros.Add(p);
            }
            return parametros;
        }

        private List<Object> getValoresParam(Ambito ambito)
        {
            List<Object> valores = new List<object>();
            foreach(Expresion e in this.expresiones)
            {
                object val = e.getValor(ambito);
                if(val!=null)
                {
                    valores.Add(val);
                }
            }
            return valores;
        }

        private Ambito AmbitoDeClase(Ambito am)
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
