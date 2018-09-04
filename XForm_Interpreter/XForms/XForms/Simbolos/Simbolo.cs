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

        public override string ToString()
        {
            String cad = "";
            cad += "Nombre: " + this.idSimbolo;
            if(this.esVector)
            {
                cad += " | Vector";
            }
            else
            {
                cad += " | Variable";
            }
            cad += " | TIPO: " + this.Tipo;
            cad += " | Visibilidad: " + this.Visibilidad.ToString()+"\n";
            return cad;
        }
    }
}
