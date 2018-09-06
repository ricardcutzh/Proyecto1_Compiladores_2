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
        Hashtable funciones;

        public TablaFunciones()
        {
            
            this.funciones = new Hashtable();
        }

        public Boolean existeFuncion(ClaveFuncion clave)
        {
            return this.funciones.ContainsKey(clave);
        }

        public void agregarFuncion(ClaveFuncion clave, Funcion fun)
        {
            this.funciones.Add(clave, fun);
        }

        public Funcion getFuncion(ClaveFuncion clave)
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
