using System;
using System.Linq;
using Irony.Parsing;
using XForms.Objs;
using XForms.ASTTree.Interfaces;
using XForms.ASTTree.Instrucciones;

namespace XForms.ASTTree.ASTConstructor
{
    class ASTTreeConstructor
    {
        ParseTreeNode raiz; /*RAIZ DEL CUERPO DE LA CLASE*/
        String clase; /*SOLO POR SI OCURRE UN ERROR DURANTE EL PARSER*/
        String archivo; /*SOLO SI OCURRE UN ERROR LO UTILIZARE*/

        /*YA QUE EL LENGUAJE ES CASE INSENSITIVE ENTONCES VOY A CONVERTIR TODO A MINUSCULAS*/
        public ASTTreeConstructor(ParseTreeNode raiz, String clase, String Archivo)
        {
            this.raiz = raiz;
            this.clase = clase;
            this.archivo = Archivo;
        }

        public Object ConstruyerAST()
        {

            return null;
        }

        #region INICIO
        private Object recorreCuerpo(ParseTreeNode raiz)
        {
            String etiqueta = raiz.ToString();
            switch(etiqueta)
            {
                case "DECLARACION_GLOBAL":
                    {
                        break;
                    }
            }
            return null;
        }
        #endregion

        #region DECLARACIONES
        private Object recorreDeclaraciones(ParseTreeNode raiz)
        {
            String etiqueta = raiz.ToString();
            switch(etiqueta)
            {
                case "DECLARACION_GLOBAL":
                    {
                        if(raiz.ChildNodes.Count == 4)
                        {
                            String tipo = (String)dameTipo(raiz.ChildNodes.ElementAt(0));
                            Estatico.Vibililidad visibilidad = (Estatico.Vibililidad)dameVisibilidad(raiz.ChildNodes.ElementAt(1));
                            String idvar = raiz.ChildNodes.ElementAt(2).Token.Text.ToLower();
                            //FALTA LA EXPRESION
                            if(tipo!=null)
                            {
                                //FALTA EL RETORNO
                            }
                        }   
                        if(raiz.ChildNodes.Count == 3)
                        {
                            String tipo = (String)dameTipo(raiz.ChildNodes.ElementAt(0));
                            Estatico.Vibililidad visibilidad = (Estatico.Vibililidad)dameVisibilidad(raiz.ChildNodes.ElementAt(1));
                            String idvar = raiz.ChildNodes.ElementAt(2).Token.Text.ToLower();
                            if(tipo!=null)
                            {
                                int linea = raiz.ChildNodes.ElementAt(2).Token.Location.Line;
                                int colum = raiz.ChildNodes.ElementAt(2).Token.Location.Column;
                                DeclaracionVar declaracion = new DeclaracionVar(idvar, tipo, visibilidad, linea, colum, this.clase);
                                return declaracion;
                            }
                        }
                        if(raiz.ChildNodes.Count == 1)
                        {
                            return recorreDeclaraciones(raiz.ChildNodes.ElementAt(0));
                        }
                        break;
                    }
                case "DECLARACION_LOCAL":
                    {
                        if(raiz.ChildNodes.Count == 3)
                        {
                            String tipo = (String)dameTipo(raiz.ChildNodes.ElementAt(0));
                            String idvar = raiz.ChildNodes.ElementAt(1).Token.Text.ToLower();
                            //FALTA LA EXPRESION
                            if(tipo!=null)
                            {
                                int linea = raiz.ChildNodes.ElementAt(1).Token.Location.Line;
                                int colum = raiz.ChildNodes.ElementAt(1).Token.Location.Column;
                                //RETORNO
                            }
                        }
                        if(raiz.ChildNodes.Count == 2)
                        {
                            String tipo = (String)dameTipo(raiz.ChildNodes.ElementAt(0));
                            String idvar = raiz.ChildNodes.ElementAt(1).Token.Text.ToLower();
                            if(tipo!=null)
                            {
                                int linea = raiz.ChildNodes.ElementAt(1).Token.Location.Line;
                                int colum = raiz.ChildNodes.ElementAt(1).Token.Location.Column;
                                DeclaracionVar declaracion = new DeclaracionVar(idvar, tipo, Estatico.Vibililidad.LOCAL, linea, colum, this.clase);
                                return declaracion;
                            }
                        }
                        break;
                    }
            }
            return null;
        }
        #endregion

        #region TIPOS
        private Object dameTipo(ParseTreeNode raiz)
        {
            String etiqueta = raiz.ToString();
            switch(etiqueta)
            {
                case "TIPO":
                    {
                        if(raiz.ChildNodes.Count == 1)
                        {
                            return raiz.ChildNodes.ElementAt(0).Token.Text.ToLower();
                        }
                        break;
                    }
            }
            return null;
        }
        #endregion

        #region VISIBILIDAD
        private Object dameVisibilidad(ParseTreeNode raiz)
        {
            String etiqueta = raiz.ToString();
            switch(etiqueta)
            {
                case "VISIBILIDAD":
                    {
                        if(raiz.ChildNodes.Count == 1)
                        {
                            switch(raiz.ChildNodes.ElementAt(0).Token.Text)
                            {
                                case "publico":
                                    {
                                        return Estatico.Vibililidad.PUBLICO;
                                    }
                                case "privado":
                                    {
                                        return Estatico.Vibililidad.PRIVADO;
                                    }
                                case "protegido":
                                    {
                                        return Estatico.Vibililidad.PROTEGIDO;
                                    }
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
