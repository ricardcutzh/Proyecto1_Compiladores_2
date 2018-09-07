using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XForms.Objs
{
    class Clave
    {
        public String idFuncion;
        public List<NodoParametro> parametros;

        public Clave(String idFuncion, List<NodoParametro>parametros, String Tipo)
        {
            this.idFuncion = idFuncion;
            this.parametros = parametros;
        }

        public override bool Equals(object obj)
        {
            if(obj is Clave)
            {
                Clave aux = (Clave)obj;
                if(this.idFuncion == aux.idFuncion && this.parametros.Count == aux.parametros.Count)
                {
                    for(int x = 0; x < this.parametros.Count; x++)
                    {
                        if(!this.parametros.ElementAt(x).Equals(aux.parametros.ElementAt(x)))
                        {
                            return false;
                        }
                    }
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            var hashCode = -1951830441;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(idFuncion);
            return hashCode;
        }
    }
}
