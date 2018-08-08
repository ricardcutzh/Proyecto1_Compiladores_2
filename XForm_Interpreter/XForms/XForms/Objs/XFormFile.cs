using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XForms.Objs
{
    class XFormFile
    {
        //OBJETO QUE VA PODER ALMACENAR LAS RUTAS Y LOS ARCHIVOS EN MEMORIA
        String ruta;
        String FileName;
        bool guardado;

        public XFormFile(String ruta, String FileName, bool guardado)
        {
            this.ruta = ruta;
            this.FileName = FileName;
            this.guardado = guardado;
        }

        // SE GUARDO EL ARCHIVO?
        public bool isItSaved()
        {
            return this.guardado;
        }

        // CAMBIA EL ESTADO DE GUARDAR
        public void setSaved(bool val)
        {
            this.guardado = val;
        }

        // OBTIENE EL NOMBRE DEL ARCHIVO
        public String getXFileName()
        {
            return this.FileName;
        }

        // OBTIENE LA RUTA DEL ARCHIVO
        public String getXFilePath()
        {
            return this.ruta;
        }

        //CAMBIA EL NOMBRE DEL ARCHIVO
        public void setXFileName(String name)
        {
            this.FileName = name;
        }

        //CAMBIA LA RUTA DEL ARCHIVO
        public void setXFilePath(String path)
        {
            this.ruta = path;
        }




    }
}
