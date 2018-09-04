using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XForms.Objs
{
    class NodoParametro
    {
        String idparam; //IDENTIFICADOR DEL PARAMETRO

        String tipo; //EL TIPO DEL PARAMETRO

        List<int> dimensiones; //SI ES ARREGLO ENTONCES VA A TENER DIMENSIONES

        Boolean soyVector;

        public NodoParametro(String id, String tipo, Boolean soyVector)
        {
            this.tipo = tipo;
            this.idparam = id;
            this.soyVector = soyVector;
        }

        public NodoParametro(String id, String tipo, Boolean soyVector, List<int> dimensiones)
        {
            this.tipo = tipo;
            this.idparam = id;
            this.soyVector = soyVector;
            this.dimensiones = dimensiones;
        }


    }
}
