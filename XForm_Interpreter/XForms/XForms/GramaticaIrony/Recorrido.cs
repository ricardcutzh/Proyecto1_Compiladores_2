using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
using System.Windows.Forms;
using XForms.Objs;
using System.IO;

namespace XForms.GramaticaIrony
{
    class Recorrido
    {
        ParseTreeNode raiz;

        String archivo;
        String ProyectoPath;

        public List<ClasePreAnalizada> clases { get; }

        public Recorrido(ParseTreeNode raiz)
        {
            this.raiz = raiz;
            this.clases = new List<ClasePreAnalizada>();
        }

        public Recorrido(ParseTreeNode raiz, String archivo, String path)
        {
            this.raiz = raiz;
            this.archivo = archivo;
            this.ProyectoPath = path;
            this.clases = new List<ClasePreAnalizada>();
        }

        //INICIO DE RECORRIDO PARA CONSTRUCCION DEL LAS CLASES CON SUS SIMBOLOS
        public void iniciaRecorrido()
        {
            recorre(this.raiz);
        }

        #region GENERAL
        //SE ENCARGA DEL RECORRIDO GENERAL DEL ARCHIVO
        //INICIO := IMPORTACIONES CLASES
        //       | CLASES
        private Object recorre(ParseTreeNode raiz)
        {
            String etiqueta = raiz.ToString();
            switch(etiqueta)
            {
                case "INICIO":
                    {
                        if(raiz.ChildNodes.Count == 1)
                        {
                            recorre(raiz.ChildNodes.ElementAt(0));//ME MUEVO LAS CLASES!
                            return null;
                        }
                        if(raiz.ChildNodes.Count == 2)
                        {
                            recorre(raiz.ChildNodes.ElementAt(1));//ME MUEVO A LAS CLASES
                            recorre(raiz.ChildNodes.ElementAt(0));//ME MUEVO A LAS IMPORTACIONES
                            return null;
                        }
                        break;
                    }
                case "IMPORTACIONES":
                    {
                        if(raiz.ChildNodes.Count > 0)
                        {
                            foreach(ParseTreeNode n in raiz.ChildNodes)//POR CADA HIJO VOY REALIZAR LA IMPORTACION DE OTROS ARCHIVOS
                            {
                                List<ClasePreAnalizada> aux = (List<ClasePreAnalizada>)recorreImportaciones(n);
                                if(aux!=null)
                                {
                                    foreach(ClasePreAnalizada c in aux)
                                    {
                                        //IMPORTANTO SOLO LAS CLASES PUBLICAS
                                        if(c.vibililidad == Estatico.Vibililidad.PUBLICO)
                                        {
                                            this.clases.Add(c);
                                        }  
                                    }
                                }
                            }
                        }
                        break;
                    }
                case "CLASES":
                    {
                        if(raiz.ChildNodes.Count > 0)
                        {
                            foreach(ParseTreeNode n in raiz.ChildNodes)//POR CADA HIJO VOY A REALIZAR EL ANALISIS DE SUS CLASES CON SU RESPECTIVA TS
                            {
                                ClasePreAnalizada c = (ClasePreAnalizada)recorreClases(n);
                                if(c!=null)
                                {
                                    
                                    if(c!=null)
                                    {
                                        this.clases.Add(c);
                                    }
                                }
                            }
                        }
                        break;
                    }
            }
            return null;
        }
        #endregion

        #region CLASES
        // CLASES
        private Object recorreClases(ParseTreeNode raiz)//DEBE RETORNAR UNA CLASE
        {
            String etiqueta = raiz.ToString();
            switch(etiqueta)
            {
                case "CLASE"://DEBO DE RETORNAR EL OBJETO CLASE, EL CUERPO DE LA CLASE ME VA A RETORNAR LA LISTA DE ATRIBUTOS QUE TIENE
                    {
                        if(raiz.ChildNodes.Count == 6)//SI EXACTAMENTE TIENE 6 HIJOS
                        {
                            String idclase = raiz.ChildNodes.ElementAt(1).Token.Text.ToLower();
                            Estatico.Vibililidad visi = (Estatico.Vibililidad)obtenerVisibilidad(raiz.ChildNodes.ElementAt(2));
                            String padre = raiz.ChildNodes.ElementAt(4).Token.Text;
                            ClasePreAnalizada clase = new ClasePreAnalizada(idclase, visi, raiz.ChildNodes.ElementAt(5), padre, this.archivo);
                            return clase;
                        }
                        if(raiz.ChildNodes.Count == 5)//SI EXACTAMENTE TIENE 5 HIJOS
                        {
                            String idClase = raiz.ChildNodes.ElementAt(1).Token.Text.ToLower();
                            String padre = raiz.ChildNodes.ElementAt(3).Token.Text;
                            ClasePreAnalizada clase = new ClasePreAnalizada(idClase, Estatico.Vibililidad.PUBLICO, raiz.ChildNodes.ElementAt(4),padre, this.archivo);
                            return clase;
                        }
                        if(raiz.ChildNodes.Count == 4)//SI EXACTAMENTE TIENE 4 HIJOS
                        {
                            String idclase = raiz.ChildNodes.ElementAt(1).Token.Text.ToLower();
                            Estatico.Vibililidad visi = (Estatico.Vibililidad)obtenerVisibilidad(raiz.ChildNodes.ElementAt(2));
                            ClasePreAnalizada clase = new ClasePreAnalizada(idclase, visi, raiz.ChildNodes.ElementAt(3), this.archivo);
                            return clase;
                        }
                        if(raiz.ChildNodes.Count == 3)//SI EXACTAMENTE TIENE 3 HIJOS
                        {
                            /*SI NO TIENE VISIBILIDAD LA CLASE SERA PRIVADA*/
                            String idclase = raiz.ChildNodes.ElementAt(1).Token.Text.ToLower();
                            ClasePreAnalizada clase = new ClasePreAnalizada(idclase, Estatico.Vibililidad.PUBLICO, raiz.ChildNodes.ElementAt(2), this.archivo);
                            return clase;
                        }
                        break;
                    }
            }
            return null;
        }
        #endregion

        #region IMPORTACIONES
        //IMPORACIONES
        private Object recorreImportaciones(ParseTreeNode raiz) //DEBE DE RETORNAR UNA LISTA DE CLASES
        {
            String etiqueta = raiz.ToString();
            switch(etiqueta)
            {
                case "IMPORTA"://AQUI DEBO DE TOMAR EL ARCHIVO Y PARSEARLO, ESTE ME DEBE DE RETORNAR UNA LISTA DE CLASES QUE SE DECLARARON EN LAS IMPORTACIONES
                    {
                        if(raiz.ChildNodes.Count == 2)//SI EXTACTAMENTE TIENE 2 HIJOS
                        {
                            String importacion = raiz.ChildNodes.ElementAt(1).Token.Text+".xform";
                            try
                            {
                                String cad = File.ReadAllText(this.ProyectoPath + "\\" + importacion);
                                Analizador a = new Analizador(cad, ProyectoPath, importacion);
                                if(a.analizar())
                                {
                                    return a.clases;
                                }
                            }
                            catch
                            {
                                Estatico.errores.Add(new TError("Semantico", "Error Al importar: "+importacion+" | En: "+this.archivo, raiz.ChildNodes.ElementAt(1).Token.Location.Line, raiz.ChildNodes.ElementAt(1).Token.Location.Column));
                            }
                        }
                        break;
                    }
            }
            return null;
        }
        #endregion

        #region VISIBILIDAD
        private Object obtenerVisibilidad(ParseTreeNode raiz)
        {
            String visi = raiz.ToString();
            switch(visi)
            {
                case "VISIBILIDAD":
                    {
                        if(raiz.ChildNodes.Count ==1)
                        {
                            String vi = raiz.ChildNodes.ElementAt(0).Token.Text;
                            if(vi.Equals("privado"))
                            {
                                return Estatico.Vibililidad.PRIVADO;
                            }
                            else if(vi.Equals("publico"))
                            {
                                return Estatico.Vibililidad.PUBLICO;
                            }
                            else if(vi.Equals("protegido"))
                            {
                                return Estatico.Vibililidad.PROTEGIDO;
                            }
                        }
                        break;
                    }
            }

            return Estatico.Vibililidad.PRIVADO;
        }
        #endregion
    }
}
