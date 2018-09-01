using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XForms.Simbolos
{
    class Ambito
    {
        public String idAmbito { get; } /*VA A SERVIR POR SI NECESITO REPORTAR UN ERROR EN UN AMBITO ESPECIFICO*/
        public Ambito Anterior { get; } /*VA A SERVIR PARA MOVERME DENTRO DE LOS AMBITOS BUSCANDO UNA VARIABLE*/
        TablaVariables tablaVars;

        public Ambito(Ambito anterior, String idAmbito)
        {
            this.Anterior = anterior;
            this.idAmbito = idAmbito;
            this.tablaVars = new TablaVariables();
        }

        /*METODO PARA BUSCAR UNA VARIABLE DENTRO DEL AMBITO*/

        public Simbolo getSimbolo(String id)
        {
            Ambito auxiliar = this;

            while(auxiliar!=null)
            {
                if(this.tablaVars.ExisteVariable(id))
                {
                    return this.tablaVars.getVariable(id);
                }

                auxiliar = auxiliar.Anterior; //ME MUEVO AL ANTERIOR
            }

            return null;//EN CASO DE NO ENCONTRAR LA VARIABLE MARCARIA UN ERROR
        }

        /*AQUI FALTA EL METODO PARA MANEJAR LAS FUNCIONES QUE EXISTEN EN UN AMBITO*/

    }
}
