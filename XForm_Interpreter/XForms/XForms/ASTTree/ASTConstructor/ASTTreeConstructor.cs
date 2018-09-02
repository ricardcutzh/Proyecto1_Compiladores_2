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
            //int linea = this.raiz.Token.Location.Line;
            //int col = this.raiz.Token.Location.Column;
            CuerpoClase nodoCuerpo = new CuerpoClase(0, 0, this.clase);
            foreach(ParseTreeNode nodo in raiz.ChildNodes)
            {
                Instruccion aux = (Instruccion)recorreCuerpo(nodo);
                if(aux!=null)
                {
                    nodoCuerpo.addInstruccion(aux);
                }
            }
            return nodoCuerpo;
        }

        #region INICIO
        private Object recorreCuerpo(ParseTreeNode raiz)
        {
            String etiqueta = raiz.ToString();
            switch(etiqueta)
            {
                case "DECLARACION_GLOBAL":
                    {
                        return recorreDeclaraciones(raiz);
                    }
                case "DECLARACION_LOCAL":
                    {
                        return recorreDeclaraciones(raiz);
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
                            if(raiz.ChildNodes.ElementAt(3).ToString().Equals("EXP"))
                            {
                                String tipo = (String)dameTipo(raiz.ChildNodes.ElementAt(0));
                                Estatico.Vibililidad visibilidad = (Estatico.Vibililidad)dameVisibilidad(raiz.ChildNodes.ElementAt(1));
                                String idvar = raiz.ChildNodes.ElementAt(2).Token.Text.ToLower();
                                ///////////////////////////////////////////////////////////////////////////////////////////////////
                                ASTTreeExpresion aux = new ASTTreeExpresion(raiz.ChildNodes.ElementAt(3), this.clase, this.archivo);
                                Expresion exp = (Expresion)aux.ConstruyeASTExpresion();
                                ///////////////////////////////////////////////////////////////////////////////////////////////////
                                if (tipo != null && exp != null)
                                {
                                    int linea = raiz.ChildNodes.ElementAt(2).Token.Location.Line;
                                    int colum = raiz.ChildNodes.ElementAt(2).Token.Location.Column;
                                    DeclaracionVar declaracion = new DeclaracionVar(exp, idvar, tipo, visibilidad, linea, colum, this.clase);
                                    return declaracion;
                                }
                            }
                            else if(raiz.ChildNodes.ElementAt(3).ToString().Equals("DIMENSIONES"))
                            {
                                //AQUI MANEJO LAS DIMENSIONES DEL ARRAY
                                String tipo = (String)dameTipo(raiz.ChildNodes.ElementAt(0));
                                Estatico.Vibililidad vibililidad = (Estatico.Vibililidad)dameVisibilidad(raiz.ChildNodes.ElementAt(1));
                                String idva = raiz.ChildNodes.ElementAt(2).Token.Text.ToLower();
                                /////////////////////////////////////////////////////////////////
                                // AQUI PIDO LAS DIMENSIONES DEL ARREGLO

                                /////////////////////////////////////////////////////////////////
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
                        if(raiz.ChildNodes.Count == 5)
                        {
                            //TIPO + VISIBILIDAD + identificador + DIMENSIONES + "=" + EXP + ";";
                        }
                        break;
                    }
                case "DECLARACION_LOCAL":
                    {
                        if(raiz.ChildNodes.Count == 3)
                        {
                            if (raiz.ChildNodes.ElementAt(2).ToString().Equals("EXP"))
                            {
                                String tipo = (String)dameTipo(raiz.ChildNodes.ElementAt(0));
                                String idvar = raiz.ChildNodes.ElementAt(1).Token.Text.ToLower();
                                ///////////////////////////////////////////////////////////////////////////////////////////////////
                                ASTTreeExpresion aux = new ASTTreeExpresion(raiz.ChildNodes.ElementAt(2), this.clase, this.archivo);
                                Expresion exp = (Expresion)aux.ConstruyeASTExpresion();
                                ///////////////////////////////////////////////////////////////////////////////////////////////////
                                if (tipo != null && exp != null)
                                {
                                    int linea = raiz.ChildNodes.ElementAt(1).Token.Location.Line;
                                    int colum = raiz.ChildNodes.ElementAt(1).Token.Location.Column;
                                    DeclaracionVar declaracion = new DeclaracionVar(exp, idvar, tipo, Estatico.Vibililidad.PRIVADO, linea, colum, this.clase);
                                    return declaracion;
                                }
                            }
                            else if(raiz.ChildNodes.ElementAt(2).ToString().Equals("DIMENSIONES"))
                            {
                                //MANEJO LAS DIMENSIONES
                                String tipo = (String)dameTipo(raiz.ChildNodes.ElementAt(0));
                                String idvar = raiz.ChildNodes.ElementAt(1).Token.Text.ToLower();
                                /////////////////////////////////////////////////////////////////
                                // AQUI PIDO LAS DIMENSIONES DEL ARREGLO

                                /////////////////////////////////////////////////////////////////
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
                                DeclaracionVar declaracion = new DeclaracionVar(idvar, tipo, Estatico.Vibililidad.PRIVADO, linea, colum, this.clase);
                                return declaracion;
                            }
                        }
                        if(raiz.ChildNodes.Count == 4)
                        {
                            //TIPO + identificador + DIMENSIONES + "=" + EXP + ";";
                            String tipo = (String)dameTipo(raiz.ChildNodes.ElementAt(0));
                            String idvar = raiz.ChildNodes.ElementAt(1).Token.Text.ToLower();
                            ///////////////////////////////////////////////////////////////////////////////////////////////////
                            /// DIMENSIONES
                            ///////////////////////////////////////////////////////////////////////////////////////////////////
                            ASTTreeExpresion aux = new ASTTreeExpresion(raiz.ChildNodes.ElementAt(3), this.clase, this.archivo);
                            Expresion exp = (Expresion)aux.ConstruyeASTExpresion();
                            ///////////////////////////////////////////////////////////////////////////////////////////////////
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
