using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using XForms.Objs;
using System.Windows.Forms;

namespace XForms.Simbolos
{
    class TablaConstructores
    {
        Hashtable constructores;

        public TablaConstructores()
        {
            this.constructores = new Hashtable();
        }

        public Boolean existeConstructor(ClaveFuncion clave)
        {
            return this.constructores.ContainsKey(clave);
        }
    
        public void AgregaConstrucor(ClaveFuncion clave, Constructor constructor)
        {
            this.constructores.Add(clave, constructor);
        }

        public Constructor getConstructro(ClaveFuncion clave)
        {
            if(existeConstructor(clave))
            {
                return (Constructor)this.constructores[clave];
            }
            else
            {
                return null;
            }
        }

        public String ImprimirTabla()
        {
            String mensaje = "-------------------TABLA CONSTRUCTORES---------------------\n";
            foreach(DictionaryEntry d in this.constructores)
            {
                Constructor c = (Constructor)d.Value;
                mensaje += c.ToString()+"\n";
            }
            mensaje += "----------------------------------------------------------\n";
            return mensaje;
        }
    }
}
