using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.Objs;

namespace XForms.Simbolos
{
    class Simbolo
    {
        public String idSimbolo { get; } //ID DEL SIMBOLO
        public Boolean esVector { get; } //ES VECTOR?
        public Estatico.Vibililidad Visibilidad { get; } //VISIBILIDAD
        public String Tipo { get; } //TIPO DE EL SIMBOLO

        public Simbolo(String idSimbolo, Boolean esVector, Estatico.Vibililidad visibilidad, String Tipo)//CONSTRUCTOR
        {
            this.idSimbolo = idSimbolo;
            this.esVector = esVector;
            this.Visibilidad = visibilidad;
            this.Tipo = Tipo;
        }

    }
}
