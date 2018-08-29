using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace XForms.GramaticaIrony
{
    class Recorrido
    {
        ParseTreeNode raiz;

        //INICIO DE RECORRIDO PARA CONSTRUCCION DEL LAS CLASES CON SUS SIMBOLOS
        public void iniciaRecorrido()
        {

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
                            recorre(raiz.ChildNodes.ElementAt(0));//ME MUEVO A LAS IMPORTACIONES
                            recorre(raiz.ChildNodes.ElementAt(1));//ME MUEVO A LAS CLASES
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

                        }
                        if(raiz.ChildNodes.Count == 5)//SI EXACTAMENTE TIENE 5 HIJOS
                        {

                        }
                        if(raiz.ChildNodes.Count == 4)//SI EXACTAMENTE TIENE 4 HIJOS
                        {

                        }
                        if(raiz.ChildNodes.Count == 3)//SI EXACTAMENTE TIENE 3 HIJOS
                        {

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

                        }
                        break;
                    }
            }
            return null;
        }
        #endregion
    }
}
