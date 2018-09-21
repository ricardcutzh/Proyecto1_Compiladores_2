using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.Simbolos;
using XForms.StrParse;
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

        public Boolean lectura = false;

        public int numeroPregunta;

        public Pregunta(Ambito ambito, String idpregunta, int numeroP)
        {
            this.ambitoPregunta = ambito;
            this.idPregunta = idpregunta;
            this.numeroPregunta = numeroP;
            tomaEtiqueta();
            tomaSugerencia();
            tomaRequerido();
            tomaRequeridoMSN();
            tomaLectura();
        }

        private void tomaEtiqueta()
        {
            Variable v = (Variable)ambitoPregunta.getSimbolo("etiqueta");
            if(v!=null)
            {
                if(v.valor is String)
                {
                    this.etiqueta = (String)v.valor;
                    ParserStr p = new ParserStr(this.etiqueta);
                    String n = p.reemplazaCadena();
                    if(!this.etiqueta.Equals(n))
                    {
                        this.etiqueta = n;
                    }
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
                    ParserStr p = new ParserStr(this.sugerencia);
                    String n = p.reemplazaCadena();
                    if (!this.sugerencia.Equals(n))
                    {
                        this.sugerencia = n;
                    }
                }
            }
        }

        private void tomaRequerido()
        {
            Variable v = (Variable)ambitoPregunta.getSimbolo("requerido");
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
    
        private void tomaLectura()
        {
            foreach(DictionaryEntry d in this.ambitoPregunta.tablaFuns.funciones)
            {
                Funcion f = (Funcion)d.Value;
                if(f.idFuncion.ToLower().Equals("respuesta"))
                {
                    if(f.Vibililidad.Equals(Estatico.Vibililidad.PRIVADO))
                    {
                        this.lectura = true;
                        return;
                    }
                }
            }
        }

    }
}
