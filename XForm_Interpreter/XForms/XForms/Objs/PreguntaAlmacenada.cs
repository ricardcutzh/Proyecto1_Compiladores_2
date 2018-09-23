using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XForms.Objs
{
    class PreguntaAlmacenada
    {

        public String idPregunta;
        public String etiqueta;

        List<Object> respuestas;
        int numero;

        public PreguntaAlmacenada(String idPregunta, String etiqueta, int numero)
        {
            this.idPregunta = idPregunta;
            this.etiqueta = etiqueta;
            this.respuestas = new List<object>();
            this.numero = numero;
        }

        public void addAnswer(Object valor)
        {
            this.respuestas.Add(valor);
        }

        
        public String toHTML(int x)
        {

            String cad = "<div class='card'> <div class='card-body'> <h5 class='card-title'>"+this.numero+") "+this.etiqueta+"</h5>";

            foreach(Object o in this.respuestas)
            {
                if(o is String)
                {
                    cad = cad + "<p class='card-text'> <strong> R//</strong>"+ (String)o + "</p>";
                }
            }

            cad = cad + "</div> </div> <br>";
            return cad;
        }
    }
}
