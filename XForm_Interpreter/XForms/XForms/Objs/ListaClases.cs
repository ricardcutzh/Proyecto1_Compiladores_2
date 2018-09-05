using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.Simbolos;

namespace XForms.Objs
{
    class ListaClases
    {
        List<Clase> clases;

        public ListaClases()
        {
            clases = new List<Clase>();
        }

        public Boolean existeClase(String idclase)
        {
            idclase = idclase.ToLower();
            Clase aux = new Clase(idclase, "", false, null, null, null);
            if(this.clases.Contains(aux))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void addClass(Clase c)
        {
            this.clases.Add(c);
        }

        public Clase getClase(String id)
        {
            foreach(Clase c in this.clases)
            {
                if(c.idClase.ToLower().Equals(id.ToLower()))
                {
                    return c;
                }
            }
            return null;
        }

        public Clase getFirstClassWithMain()
        {
            foreach(Clase c in this.clases)
            {
                if(c.tieneMain)
                {
                    return c;
                }
            }
            return null;
        }

        public String printClassList()
        {
            String cad = "***********************************************************\n";
            cad += "*                  CLASES DISPONIBLES                     *\n";
            cad += "***********************************************************\n";
            foreach(Clase c in this.clases)
            {
                cad += "-----------------------------------------------------------\n";
                cad += c.ToString() + "\n";
                cad += "-----------------------------------------------------------\n";
            }

            cad += "***********************************************************\n";
            return cad;
        }
    }
}
