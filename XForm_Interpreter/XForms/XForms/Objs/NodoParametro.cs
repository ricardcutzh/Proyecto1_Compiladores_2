using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XForms.Objs
{
    class NodoParametro
    {
        public String idparam; //IDENTIFICADOR DEL PARAMETRO

        public String tipo; //EL TIPO DEL PARAMETRO

        public List<int> dimensiones; //SI ES ARREGLO ENTONCES VA A TENER DIMENSIONES

        public Boolean soyVector;

        public String mensajeError;

        public NodoParametro(String id, String tipo, Boolean soyVector)
        {
            this.tipo = tipo;
            this.idparam = id;
            this.soyVector = soyVector;
            this.mensajeError = "";
        }

        public NodoParametro(String id, String tipo, Boolean soyVector, List<int> dimensiones)
        {
            this.tipo = tipo;
            this.idparam = id;
            this.soyVector = soyVector;
            this.dimensiones = dimensiones;
            this.mensajeError = "";
        }

        public override bool Equals(object obj)
        {
            if(obj is NodoParametro)
            {
                NodoParametro aux = (NodoParametro)obj;
                if(this.tipo != aux.tipo && !aux.tipo.Equals("nulo")) { mensajeError = "El Tipo No Coincide en Parametro "+this.idparam+", Se esperaba: " + this.tipo + " y se Encontro: " + aux.tipo; return false; }
                if(this.soyVector != aux.soyVector) { if (this.soyVector) { mensajeError = "Se esperaba un Parametro de Tipo Arreglor"; }
                else { mensajeError = "No se esperaba un Parametro de Tipo Arreglo"; }
                ; return false; }
                if(this.soyVector)
                {
                    if(this.dimensiones.Count != aux.dimensiones.Count) { mensajeError = "El Numero de Dimensiones del Parametro Arreglo no Concuerdan"; return false; }
                    for(int x = 0; x < aux.dimensiones.Count; x++)
                    {
                        if(aux.dimensiones.ElementAt(x)!= this.dimensiones.ElementAt(x)) { mensajeError = "El Tamano de Dimensiones no Concuerda"; return false; }
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
