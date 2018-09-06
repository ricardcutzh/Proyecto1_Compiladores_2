using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XForms.Objs
{
    class ClaveFuncion
    {
        public String idFuncion;
        public String Tipo;
        public List<NodoParametro> parametros;

        public String mensajeError;

        public ClaveFuncion(String idFuncion, String Tipo, List<NodoParametro> parametros)
        {
            this.idFuncion = idFuncion;
            this.Tipo = Tipo;
            this.parametros = parametros;
            this.mensajeError = "";
        }

        public override bool Equals(object obj)
        {
            if(obj is ClaveFuncion)
            {
                ClaveFuncion aux = (ClaveFuncion)obj;
                //ES IGUAL EL NOMNBRE DE LA FUNCION, TIPO Y NUMERO DE PARAMETROS?
                if(this.idFuncion == aux.idFuncion && this.Tipo == aux.Tipo && this.parametros.Count == aux.parametros.Count)
                {
                    for(int x = 0; x < this.parametros.Count; x++)
                    {
                        if(!this.parametros.ElementAt(x).Equals(aux.parametros.ElementAt(x)))
                        {
                            this.mensajeError = "Parametro: " + this.parametros.ElementAt(x).idparam + ", no concuerda en: " + this.parametros.ElementAt(x).mensajeError;
                            return false;
                        }
                    }
                    return true;
                }
                mensajeError = "No se encontro una funcion: \"" + this.idFuncion + "\" de tipo: \"" + this.Tipo + "\" que reciba: \"" + this.parametros.Count + "\" cantidad de parametros";
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
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Tipo);
            return hashCode;
        }
    }
}
