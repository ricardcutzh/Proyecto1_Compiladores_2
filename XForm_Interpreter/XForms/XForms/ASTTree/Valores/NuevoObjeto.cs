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
    class NuevoObjeto : NodoAST, Expresion
    {
        String tipo;
        List<Expresion> parametros;
        public NuevoObjeto(List<Expresion> parametros, String tipo, int linea, int col, String clase):base(linea, col, clase)
        {
            this.tipo = tipo;
            this.parametros = parametros;
        }

        String valorAux = "";
            
        public string getTipo(Ambito ambito)
        {
            if(!this.valorAux.Equals(""))
            {
                return valorAux;
            }
            else
            {
                return "Nulo";
            }
        }

        public object getValor(Ambito ambito)
        {
            try
            {
                Clase obj = Estatico.clasesDisponibles.getClase(this.tipo);
                if(obj!=null)
                {
                    //TENGO QUE COMPILAR LA CLASE PRIMERO
                    ///
                    List<Object> valores = getValoresParams(ambito);
                    ///
                    Ambito auxiliar = new Ambito(null, obj.idClase, obj.ArchivoOrigen);
                    auxiliar = (Ambito)obj.Ejecutar(auxiliar);
                    /////////////////////////////////////
                    ClaveFuncion clave = new ClaveFuncion(obj.idClase.ToLower(), "vacio", getNodosParametros(ambito));
                    ////////////////////////////////////////////////
                    /// TOMO EL CONSTRUCCTOR
                    Constructor c = auxiliar.getConstructor(clave);
                    /////////////////////////////////////////////////
                    if(c!=null)
                    {
                        /// SETEANDO LOS PARAMETROS
                        /// 1) CREO EL CONSTRUCTOR LOCAL
                            Ambito local = new Ambito(auxiliar, this.clase.ToLower(), ambito.archivo);
                        /// 2) SETEO SUS PARAMETROS
                        //local = c.seteaParametrosLocales(local, getValoresParams(ambito));
                            local = c.seteaParametrosLocales(local, valores);
                        /// EJECUCION DEL CONSTRUCTOR EN SI
                            c.Ejecutar(local);
                        /// 3) LE PASO EL AMBITO AUXILIAR DE LA CLASE AL OBJETO
                            Objeto objeto = new Objeto(obj.idClase.ToLower(), auxiliar);
                            this.valorAux = obj.idClase.ToLower();
                        /// 4) LO RETORNO
                        return objeto;
                    }
                    else if(c==null && this.parametros.Count == 0)//CLASE SIN CONSTRUCTOR
                    {
                        /// 1) CREO EL AMBITO LOCAL
                        Ambito local = new Ambito(auxiliar, this.clase.ToLower(), ambito.archivo);
                        Objeto objeto = new Objeto(obj.idClase.ToLower(), auxiliar);
                        this.valorAux = obj.idClase.ToLower();
                        return objeto;
                    }
                    else
                    {
                        TError error = new TError("Semantico","Error Constructor: "+clave.mensajeError+" | Clase: "+this.clase+" | Archivo: "+ambito.archivo, this.linea, this.columna, false);
                        Estatico.errores.Add(error);
                        Estatico.ColocaError(error);
                    }
                    return null;
                }
                else
                {
                    TError error = new TError("Semantico", "No existe una clase: \"" + this.tipo + "\" Para realizar una Instancia, En Clase: " + this.clase + " | Archivo: " + ambito.archivo, this.linea, this.columna, false);
                    Estatico.errores.Add(error);
                    Estatico.ColocaError(error);
                }
            }
            catch(Exception e)
            {
                TError error = new TError("Ejecucion", "Error al Crear un Objeto en Clase: "+this.clase+" | Archivo: "+ambito.archivo+" | Error: "+e.Message, this.linea, this.columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            return new Nulo();
        }


        private List<NodoParametro> getNodosParametros(Ambito ambito)
        {
            List<NodoParametro> parametros = new List<NodoParametro>();
            foreach(Expresion e in this.parametros)
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
            foreach(Expresion e in this.parametros)
            {
                object val = e.getValor(ambito);
                if(val!=null)
                {
                    valores.Add(val);
                }
            }
            return valores;
        }
    }
}
