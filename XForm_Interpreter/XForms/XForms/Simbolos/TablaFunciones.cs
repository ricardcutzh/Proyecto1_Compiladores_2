using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.Objs;
using System.Windows.Forms;

namespace XForms.Simbolos
{
    class TablaFunciones
    {
        public Hashtable funciones;

        public TablaFunciones()
        {
            
            this.funciones = new Hashtable();
        }

        public Boolean existeFuncion(Clave clave)
        {
            return this.funciones.ContainsKey(clave);
        }

        public void agregarFuncion(Clave clave, Funcion fun)
        {
            this.funciones.Add(clave, fun);
        }

        public Funcion getFuncion(Clave clave)
        {
            if(existeFuncion(clave))
            {
                return (Funcion)this.funciones[clave];
            }
            else
            {
                return null;
            }
        }

        public String ImprimirTabla()
        {
            String mensaje = "-------------------TABLA FUNCIONES---------------------\n";
            foreach(DictionaryEntry d in this.funciones)
            {
                Funcion aux = (Funcion)d.Value;
                mensaje += aux.ToString()+"\n";
            }
            mensaje += "-----------------------------------------------------------\n";
            return mensaje;
        }
        
    }
}
