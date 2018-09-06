using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Windows.Forms;
namespace XForms.Simbolos
{
    class TablaVariables
    {
        public Hashtable variables;

        /*CONSTRUCTOR DE LA TABLA DE VARIABLES QUE VA A MANEJAR LAS VARIABLES Y ARREGLOS*/
        public TablaVariables()
        {
            this.variables = new Hashtable();
        }

        /*METODO DE PODER AVERIGUAR SI EXISTE LA VARIABLES*/
        public Boolean ExisteVariable(String id)
        {
            return this.variables.ContainsKey(id);
        }

        /*PARA PODER INSERTAR UNA VARIABLE EN LA TABLA*/
        public void agregaVariable(String id, Simbolo sim)
        {
            this.variables.Add(id, sim);
        }

        /*PARA BUSCAR SI EXISTE UNA VARIABLE EN LA TABLA*/
        public Simbolo getVariable(String id)
        {
            if(ExisteVariable(id))
            {
                return (Simbolo)this.variables[id];
            }
            else
            {
                return null;
            }
        }

        public String imprimeTabla()
        {
            String mensaje = "----------- TABLA DE VARS -----------------\n";
            foreach(DictionaryEntry d in this.variables)
            {
                Simbolo aux = (Simbolo)d.Value;
                mensaje += aux.ToString();
                if(aux is Variable)
                {
                    Variable v = (Variable)aux;
                    mensaje += " | "+v.valor.ToString()+"\n";
                }
            }
            mensaje += "-------------------------------------------------\n";
            return mensaje;
        }

    }
}
