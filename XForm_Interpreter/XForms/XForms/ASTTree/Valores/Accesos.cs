using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.ASTTree.Interfaces;
using XForms.Simbolos;
using XForms.Objs;
using XForms.GramaticaIrony;
namespace XForms.ASTTree.Valores
{
    class Accesos : NodoAST, Expresion
    {
        List<Expresion> expresiones;

        public Accesos(int linea, int col, String clase):base(linea, col, clase)
        {
            this.expresiones = new List<Expresion>();
        }

        public void AddExpresion(Expresion exp)
        {
            this.expresiones.Add(exp);
        }

        Object valorAux = null;
        public string getTipo(Ambito ambito)
        {
            Object val = null;
            if(valorAux!=null)
            {
                val = valorAux;
            }
            else
            {
                val = getValor(ambito);
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
            //AQUI FALTA EL TIPO OBJETO
            return "Objeto";
        }

        
        public object getValor(Ambito ambito)
        {
            try
            {
                if(this.expresiones.Count == 1)//SI SOLO SE HACE REFERENCIA A UN ID O UNA LLAMADA A UNA FUNCION
                {
                    this.valorAux = expresiones.ElementAt(0).getValor(ambito);
                    if(valorAux is Este)
                    {
                        //return new Nulo();
                        return new Este();
                    }
                    return valorAux;
                }
                else if(this.expresiones.Count > 1)//AQUI ES DONDE YA SE VA A HACER REFERENCIA A OBJETOS
                {
                    Ambito ambitoAux = null;
                    Object auxiliar = this.expresiones.ElementAt(0).getValor(ambito);
                    if(auxiliar is Este)
                    {
                        ambitoAux = (Ambito)buscaAtributoDeClase(ambito);
                        return recorreExpresiones(ambitoAux, auxiliar);
                    }
                    else if(auxiliar is Objeto)
                    {
                        Objeto ob = (Objeto)auxiliar;
                        ambitoAux = ob.ambito;
                        return recorreExpresiones(ambitoAux, ob);
                    }
                    return new Nulo();
                }
            }
            catch(Exception e)
            {
                TError error = new TError("Ejecucion", "Ocurrio un Error al acceder a Objeto en Clase: " + this.clase + " | Archivo: " +ambito.archivo+" | Error: "+e.Message, this.linea, this.columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            return new Nulo();
        }

        private Object recorreExpresiones(Ambito ambito, Object Actual)
        {
            for(int x = 1; x < this.expresiones.Count; x++)
            {
                if(ambito.idAmbito.ToLower().Contains(this.clase.ToLower()))
                {
                    //TOMA SIN VISIBILIDAD //AQUI MANDA VERDADERO
                    Actual = tomaConVisibilidad(ambito, this.expresiones.ElementAt(x), true);
                }
                else
                {
                    //TOMA CON VISIBILIDAD // AQUI MANDA FALSO
                    Actual = tomaConVisibilidad(ambito, this.expresiones.ElementAt(x), false);
                }

                if(Actual is Objeto)
                {
                    Objeto aux = (Objeto)Actual;
                    ambito = aux.ambito;
                    this.valorAux = aux;
                }
                else
                {
                    valorAux = Actual;
                    return Actual;
                }
            }
            return Actual;
        }


        private object tomaConVisibilidad(Ambito ambito, Expresion iterador, bool ignoraVisibilidad)
        {
            if(iterador is Identificador)
            {
                String idaux = ((Identificador)iterador).identificador.ToLower();
                Simbolo s = (Simbolo)ambito.getSimbolo(idaux);
                if(s!=null)
                {
                    if (s.Visibilidad == Estatico.Vibililidad.PUBLICO || ignoraVisibilidad)
                    {
                        Object val = iterador.getValor(ambito);//OBTENGO EL VALOR SI ES PUBLICO
                        return val;
                    }
                    else
                    {
                        //ERROR PORQUE SE INTENTA ACCEDER A UNA PROPIEDAD QUE NO ES PUBLICA
                        TError error = new TError("Semantico", "No es posible Acceder a la propiedad: \"" + s.idSimbolo + "\" ya que no es PUBLICO | Clase: " + this.clase + " | Archivo: " + ambito.archivo, this.linea, this.columna, false);
                        Estatico.errores.Add(error);
                        Estatico.ColocaError(error);
                        return null;
                    }
                }
                else
                {
                    //ERROR LA PROPIEDAD A BUSCAR NO EXISTE Y MUESTRO IDAUX
                    TError error = new TError("Semantico", "No existe la propiedad: "+idaux+" | Clase: "+this.clase+" | "+ambito.archivo, this.linea, this.columna, false);
                    Estatico.errores.Add(error);
                    Estatico.ColocaError(error);
                    return null;
                }
            }
            else if(iterador is Llamada)
            {
                String idaux = ((Llamada)iterador).id.ToLower();
                Object valor = ((Llamada)iterador).getValor(ambito);
                if(((Llamada)iterador).vibililidad == Estatico.Vibililidad.PUBLICO || ignoraVisibilidad)
                {
                    return valor;
                }
                else
                {
                    //ERROR PORQUE SE INTENCA ACCEDER A UNA PROPIEDAD QUE NO ES PUBLICA
                    TError error = new TError("Semantico", "No es posible Acceder a la propiedad: \"" + idaux + "\" ya que no es PUBLICO | Clase: " + this.clase + " | Archivo: " + ambito.archivo, this.linea, this.columna, false);
                    Estatico.errores.Add(error);
                    Estatico.ColocaError(error);
                    return new Nulo();
                }
                //AQUI PREGUNTO SI EXISTE LA FUNCION A LA QUE VOY A HACER REFERENCIA 
            }
            return null;
        }

        private object buscaAtributoDeClase(Ambito am)
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
