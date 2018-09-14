using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using XForms.Objs;

namespace XForms.Simbolos
{
    class Opciones : Objeto
    {

        List<List<object>> listados;

        public Opciones(string idClase, Ambito ambito) : base(idClase, ambito)
        {
            listados = new List<List<object>>();
            //EL AMBITO NI LO VOY A USAR

        }


        public void agregarElementos(List<Object> elementos)
        {
            this.listados.Add(elementos);
        }


        public Object obtenerDeLista(int index1, int index2)
        {
            if(index1 < listados.Count)
            {
                List<Object> sublistado = this.listados[index1];
                if (index2 < sublistado.Count)
                {
                    Object ob = sublistado[index2];
                    return ob;
                }
                else
                {
                    return new Nulo();
                }
            }
            else
            {
                return new Nulo();
            }
        }

        public Object obtenerDeLista(Object indice, int index)
        {
            List<Object> aux = null;
            foreach(List<Object> sub in this.listados)
            {
                Object ob = sub[0];
                if(indice.Equals(ob))
                {
                    aux = sub;
                    break;
                }
            }

            if(aux!=null)
            {
                if(index < aux.Count)
                {
                    Object ret = aux[index];
                    return ret;
                }
                else
                {
                    return new Nulo();
                }
            }
            return new Nulo();
        }
    }       
}
