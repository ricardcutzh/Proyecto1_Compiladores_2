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

        public int getNumeroListas()
        {
            return this.listados.Count;
        }

        public List<NodoSelecciona> getSelecciones()
        {
            List<NodoSelecciona> listado = new List<NodoSelecciona>();
            foreach(List<Object> l in this.listados)
            {
                if(l.Count == 3)
                {
                    Object val1 = l.ElementAt(0);
                    Object val2 = l.ElementAt(1);
                    Object val3 = l.ElementAt(2);
                    if(val1 is String && val2 is String && val3 is String)
                    {
                        listado.Add(new NodoSelecciona((String)val1, (String)val2, (String)val3));
                    }
                }
                else if(l.Count == 2)
                {
                    Object val1 = l.ElementAt(0);
                    Object val2 = l.ElementAt(1);
                    if(val1 is String && val2 is String)
                    {
                        listado.Add(new NodoSelecciona((String)val1, (String)val2, ""));
                    }
                }
            }

            return listado;
        }
    }

    public class NodoSelecciona
    {
        public String etiqueta;
        public String valor;
        public String ruta;

        public NodoSelecciona(String etiqueta, String valor, String ruta)
        {
            this.etiqueta = etiqueta;
            this.valor = valor;
            this.ruta = ruta;
        }

        
    }
}
