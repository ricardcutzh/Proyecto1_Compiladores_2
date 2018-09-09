using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.Simbolos;


namespace XForms.Objs
{
    class GeneradorArreglo
    {
        List<object> arbolArreglo;

        public List<int> dimensiones;
        public List<Object> linealizacion;

        public String  mensajesError;
        public int numDimensiones;

        public bool huboError;

        public GeneradorArreglo(List<Object> arbol)
        {
            this.arbolArreglo = arbol;
            this.dimensiones = new List<int>();
            this.linealizacion = new List<object>();
            this.numDimensiones = 0;

            huboError = false;

            ObtenerDimensiones(this.arbolArreglo, 0);
            dimensiones.Insert(0, arbolArreglo.Count);

            this.numDimensiones = dimensiones.Count;

            compruebaElementos();

            compruebaDimensiones();

            
        }


        

        private void ObtenerDimensiones(List<Object> lista, int nivel)
        {
            List<int> tams = new List<int>();
            foreach(Object o in lista)
            {
                if(o is List<Object>)
                {
                    if(this.dimensiones.Count == nivel)
                    {
                        this.dimensiones.Add(0);
                    }
                    List<Object> aux = (List<Object>)o;
                    ObtenerDimensiones(aux, nivel + 1);
                    tams.Add(aux.Count);
                }
                else if(o is Object)
                {
                    linealizacion.Add(o);
                }
            }

            compruebaTams(tams, nivel);
        }


        private void compruebaTams(List<int> tams, int nivel)
        {
            if(tams.Count>0)
            {
                int x = tams.ElementAt(0);
                foreach(int i in tams)
                {
                    if(i!=x)
                    {
                        dimensiones.RemoveAt(nivel);
                        dimensiones.Insert(nivel, 0);
                        return;
                    }
                }
                dimensiones.RemoveAt(nivel);
                dimensiones.Insert(nivel, x);
            }
        }

        private void compruebaDimensiones()
        {
            for(int x = 0; x < this.dimensiones.Count; x++)
            {
                if(this.dimensiones.ElementAt(x).Equals(0))
                {
                    mensajesError = "Los Elementos en la dimension: \"" + x + "\" no concuerdan con los esperados";
                    huboError = true;
                }
            }
        }

        private void compruebaElementos()
        {
            int esperados = 1;
            for(int x = 0; x < this.dimensiones.Count; x++)
            {
                esperados = esperados * dimensiones.ElementAt(x);
            }

            int obtenidos = this.linealizacion.Count;
            if(esperados!=obtenidos)
            {
                this.huboError = true;
                mensajesError = "Inconsistencia en la definicion del Arreglo con los elementos, se esperban: "+esperados+" elementos y se encontraron: "+obtenidos+" elementos";
            }
        }
    }
}
