using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.Objs;

namespace XForms.Simbolos
{
    class Arreglo : Simbolo
    {
        public List<Object> arbolArreglo;

        public List<Object> linealizacion;

        public List<int> dimensiones;

        public int numDimensiones;

        public Boolean tienaArbol;

        public Arreglo(List<Object>arbol, List<Object> linealizacion, List<int> dimensiones, int dimen, string idSimbolo, bool esVector, Estatico.Vibililidad visibilidad, string Tipo) : base(idSimbolo, esVector, visibilidad, Tipo)
        {
            this.arbolArreglo = arbol;
            this.linealizacion = linealizacion;
            this.dimensiones = dimensiones;
            this.numDimensiones = dimen;
            tienaArbol = true;
        }

        public Arreglo(List<Object> linealizacion, List<int> dimensiones, int dimen, String idSimbolo, bool esVector, Estatico.Vibililidad visibilidad, String tipo):base(idSimbolo, esVector, visibilidad, tipo)
        {
            this.arbolArreglo = null;
            this.linealizacion = linealizacion;
            this.dimensiones = dimensiones;
            this.numDimensiones = dimen;
            tienaArbol = false;
        }

        public void setVisibilidad(Estatico.Vibililidad visibilidad)
        {
            this.Visibilidad = visibilidad;
        }

        public void setID(String id)
        {
            this.idSimbolo = id;
        }

        public void setTipo(String tipo)
        {
            this.Tipo = tipo;
        }

        public override string ToString()
        {
            String cad = "| Dimensiones " + this.numDimensiones + "Linealizado: "+this.linealizacion.Count+" | DefincionDim: ";
            String aux = "";
            foreach(int i in this.dimensiones)
            {
                aux += "[" + i + "]";
            }
            cad += aux;
            return cad;
        }

        public int calcularPosicion(List<int>coordenada)
        {
            int aux = 0;
            int index = 0;
            foreach(int i in coordenada)
            {
                aux = calcularAuxiliar(index, aux, i);
                index++;
            }
            return aux;
        }

        private int calcularAuxiliar(int iteracion, int anterior, int coordenada)
        {
            if(iteracion == 0)
            {
                return coordenada;
            }
            else
            {
                return (anterior * this.dimensiones.ElementAt(iteracion)) + coordenada;
            }
        }

        public Boolean esCoordenadaValida(List<int> coordenada)
        {
            if (coordenada.Count != dimensiones.Count) { return false; }
            for(int x = 0; x< dimensiones.Count; x++)
            {
                if(coordenada.ElementAt(x)>=dimensiones.ElementAt(x))
                {
                    return false;
                }
            }
            return true;
        }

        public object getElementFromArray(int index)
        {
            return this.linealizacion.ElementAt(index);
        }

        public void setValueAtPosition(int index, Object elemento)
        {
            this.linealizacion.RemoveAt(index);
            this.linealizacion.Insert(index, elemento);
        }
    }
}
