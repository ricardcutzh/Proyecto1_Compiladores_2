using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.Simbolos;

namespace XForms.Objs
{
    class Pregunta
    {
        Ambito ambitoPregunta;

        /* CARACTERISTICAS DE PREGUNTAS */
        public String etiqueta = "";
        public String sugerencia = "";
        public Boolean requerido = false;
        public String requeridoMsn = "";
        public Object respuesta = null; /* LA RESPUESTA DE LA PREGUNTA */
        public String idPregunta = ""; /* EL IDENTIFICADOR DE LA PREGUNTA */

        public Pregunta(Ambito ambito, String idpregunta)
        {
            this.ambitoPregunta = ambito;
            this.idPregunta = idpregunta;
            tomaEtiqueta();
            tomaSugerencia();
            tomaRequerido();
            tomaRequeridoMSN();
        }

        private void tomaEtiqueta()
        {
            Variable v = (Variable)ambitoPregunta.getSimbolo("etiqueta");
            if(v!=null)
            {
                if(v.valor is String)
                {
                    this.etiqueta = (String)v.valor;
                }
            }
        }

        private void tomaSugerencia()
        {
            Variable v = (Variable)ambitoPregunta.getSimbolo("sugerir");
            if(v!=null)
            {
                if(v.valor is String)
                {
                    this.sugerencia = (String)v.valor;
                }
            }
        }

        private void tomaRequerido()
        {
            Variable v = (Variable)ambitoPregunta.getSimbolo("requerida");
            if(v!=null)
            {
                if(v.valor is Boolean)
                {
                    this.requerido = (Boolean)v.valor;
                }
            }
        }

        private void tomaRequeridoMSN()
        {
            Variable v = (Variable)ambitoPregunta.getSimbolo("requeridomsn");
            if(v !=null)
            {
                if(v.valor is String)
                {
                    this.requeridoMsn = (String)v.valor;
                }
            }
        }


    }
}
