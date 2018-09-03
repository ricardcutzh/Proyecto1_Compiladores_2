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
        List<int> dimensiones;
        List<Object> elementos;

        public Arreglo(List<int> dimensiones, string idSimbolo, bool esVector, Estatico.Vibililidad visibilidad, string Tipo) : base(idSimbolo, esVector, visibilidad, Tipo)
        {
            this.dimensiones = dimensiones;
            elementos = new List<object>();
        }

        public Arreglo(List<Object> elementos, List<int> dimensiones, string idSimbolo, bool esVector, Estatico.Vibililidad visibilidad, String Tipo): base(idSimbolo, esVector, visibilidad, Tipo)
        {
            this.elementos = elementos;
            this.dimensiones = dimensiones;
        }

        public void AddElemento(Object elemento)
        {
            this.elementos.Add(elemento);
        }

        public int calcularIndiceReal(List<int> coordenada)
        {
            int aux = 0;
            for(int x = 0; x < coordenada.Count; x++)
            {
                aux = calcularAuxiliar(x, aux, coordenada.ElementAt(x));
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
            if(coordenada.Count != this.dimensiones.Count) { return false; }
            for(int x = 0; x < this.dimensiones.Count; x++)
            {
                if(coordenada.ElementAt(x)>= this.dimensiones.ElementAt(x))
                {
                    return false;
                }
            }
            return true;
        }


    }
}
