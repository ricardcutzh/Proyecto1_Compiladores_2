using System;
using System.Linq;
using Irony.Parsing;
using XForms.Objs;
using XForms.ASTTree.Interfaces;
using XForms.ASTTree.Instrucciones;
using System.Collections.Generic;
using XForms.ASTTree.Valores;
using XForms.Simbolos;
using XForms.GramaticaIrony;
using XForms.ASTTree.Sentencias;

namespace XForms.ASTTree.ASTConstructor
{
    class ASTTreeConstructor
    {
        ParseTreeNode raiz; /*RAIZ DEL CUERPO DE LA CLASE*/
        String clase; /*SOLO POR SI OCURRE UN ERROR DURANTE EL PARSER*/
        String archivo; /*SOLO SI OCURRE UN ERROR LO UTILIZARE*/
        public Principal main { get; set; }

        /*YA QUE EL LENGUAJE ES CASE INSENSITIVE ENTONCES VOY A CONVERTIR TODO A MINUSCULAS*/
        public ASTTreeConstructor(ParseTreeNode raiz, String clase, String Archivo)
        {
            this.raiz = raiz;
            this.clase = clase;
            this.archivo = Archivo;
            this.main = null;
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
                case "PRINCIPAL":
                    {
                        if (raiz.ChildNodes.Count == 2)
                        {
                            int linea = raiz.ChildNodes.ElementAt(0).Token.Location.Line;
                            int col = raiz.ChildNodes.ElementAt(0).Token.Location.Column;
                            List<Instruccion> instrucciones = new List<Instruccion>();
                            /// FALTA TOMAR LAS INSTRUCCIONES DEL HIJO EN 1
                                foreach(ParseTreeNode nodo in raiz.ChildNodes.ElementAt(1).ChildNodes)
                                {
                                    Instruccion ins = (Instruccion)construyeSentencias(nodo);
                                    if(ins!=null)
                                    {
                                        instrucciones.Add(ins);
                                    }
                                }
                            /// 
                            Principal principal = new Principal(instrucciones, linea, col, clase);
                            DeclaracionMain main = new DeclaracionMain(principal, linea, col, clase);
                            main.SetArchivoOrigen(archivo);
                            if(this.main==null)
                            {
                                this.main = principal;
                            }
                            else
                            {
                                TError error = new TError("Semantico", "Se produjo una definicion multiple de metodo Principal en Clase: "+this.clase +" | Se produjo en: " + this.archivo , linea, col);
                                Estatico.errores.Add(error);
                            }
                            break;
                        }
                        break;
                    }
                case "CONSTRUCTOR":
                    {
                        if(raiz.ChildNodes.Count == 3)
                        {
                            String idCons = raiz.ChildNodes.ElementAt(0).Token.Text.ToLower();
                            int linea = raiz.ChildNodes.ElementAt(0).Token.Location.Line;
                            int col = raiz.ChildNodes.ElementAt(0).Token.Location.Column;
                            if (this.clase.ToLower().Equals(idCons))
                            {
                                //SI TIENEN EL MISMO NOMBRE ENTONCES LO ANADO A LA LISTA DE INSTRUCCIONES QUE SE VAN A EJECUTAR
                                List<NodoParametro> parametros = (List<NodoParametro>)getParametros(raiz.ChildNodes.ElementAt(1));
                                //FALTA TOMAR LAS INSTRUCCIONES
                                List<Instruccion> instrucciones = new List<Instruccion>();//ESTA LAS TOMO DEL HIJO EN 2
                                ///
                                    foreach(ParseTreeNode nodo in raiz.ChildNodes.ElementAt(2).ChildNodes)
                                    {
                                        Instruccion ins = (Instruccion)construyeSentencias(nodo);
                                        if (ins != null) { instrucciones.Add(ins); }
                                    }
                                ///
                                DeclaracionConstructor cons = new DeclaracionConstructor(instrucciones, parametros, linea, col, this.clase);
                                cons.SetArchivoOrigen(archivo);
                                return cons;
                            }
                            else
                            {
                                TError error = new TError("Semantico", "Constructor no definido con el nombre de Clase, Clase: \"" + this.clase + "\" Constructor definido: \"" + idCons+"\" | Se produjo en: "+this.archivo, linea, col);
                                Estatico.errores.Add(error);
                            }
                        }
                        break;
                    }
                case "FUNCIONES":
                    {
                        if(raiz.ChildNodes.Count == 5)
                        {
                            Estatico.Vibililidad visibilidad = (Estatico.Vibililidad)dameVisibilidad(raiz.ChildNodes.ElementAt(0));
                            String tipo = (String)dameTipo(raiz.ChildNodes.ElementAt(1));
                            int linea = raiz.ChildNodes.ElementAt(2).Token.Location.Line;
                            int col = raiz.ChildNodes.ElementAt(2).Token.Location.Column;
                            String idfun = raiz.ChildNodes.ElementAt(2).Token.Text.ToLower();
                            List<NodoParametro> parametros = (List<NodoParametro>)getParametros(raiz.ChildNodes.ElementAt(3));
                            List<Object> instrucciones = new List<Object>();//AQUI DEBO TRAER LAS INTRUCCIONES HIJO EN 4
                            ///
                                foreach(ParseTreeNode nodo in raiz.ChildNodes.ElementAt(4).ChildNodes)
                                {
                                    Object ins = (Object)construyeSentencias(nodo);
                                    if(ins!=null)
                                    {
                                        instrucciones.Add(ins);
                                    }
                                }
                            ///
                            if(tipo!=null)
                            {
                                DeclaracionFuncion declaracion = new DeclaracionFuncion(instrucciones, tipo.ToLower(), parametros, visibilidad, idfun, linea, col, this.clase);
                                declaracion.SetArchivoOrigen(archivo);
                                return declaracion;
                            }
                        }
                        if(raiz.ChildNodes.Count == 4)
                        {
                            String tipo = (String)dameTipo(raiz.ChildNodes.ElementAt(0));
                            int linea = raiz.ChildNodes.ElementAt(1).Token.Location.Line;
                            int col = raiz.ChildNodes.ElementAt(1).Token.Location.Column;
                            String idfun = raiz.ChildNodes.ElementAt(1).Token.Text.ToLower();
                            List<NodoParametro> parametros = (List<NodoParametro>)getParametros(raiz.ChildNodes.ElementAt(2));
                            List<Object> instrucciones = new List<Object>();//AQUI DEBO DE TRAER LAS INSTRUCCIONES EN HIJO 3
                            ///
                                foreach (ParseTreeNode nodo in raiz.ChildNodes.ElementAt(3).ChildNodes)
                                {
                                    Object ins = (Object)construyeSentencias(nodo);
                                    if (ins != null)
                                    {
                                        instrucciones.Add(ins);
                                    }
                                }
                            ///
                            if (tipo!=null)
                            {
                                DeclaracionFuncion declaracion = new DeclaracionFuncion(instrucciones, tipo, parametros, Estatico.Vibililidad.PRIVADO, idfun, linea, col, this.clase);
                                declaracion.SetArchivoOrigen(archivo);
                                return declaracion;
                            }
                        }
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
                                    declaracion.SetArchivoOrigen(archivo);
                                    return declaracion;
                                }
                            }
                            else /// PARA ARREGLOS VACIOS
                            {
                                /// TIPO + VISIBILIDAD + identificador + EMPTYDIM + ";"
                                String tipo = (String)dameTipo(raiz.ChildNodes.ElementAt(0));//OBTENGO EL TIPO

                                Estatico.Vibililidad visibilidad = (Estatico.Vibililidad)dameVisibilidad(raiz.ChildNodes.ElementAt(1)); //OBTENGO LA VISIBILIDAD

                                String idArr = raiz.ChildNodes.ElementAt(2).Token.Text.ToLower();//OBTENGO EL ID DEL ARREGLO
                                int linea = raiz.ChildNodes.ElementAt(2).Token.Location.Line;
                                int col = raiz.ChildNodes.ElementAt(2).Token.Location.Column;

                                int numDim = (int)dameNumeroDimensiones(raiz.ChildNodes.ElementAt(3));//OBTENGO EL NUMERO DE DIMENSIONES

                                if(tipo!=null)
                                {
                                    DeclaracionArreglo declaracion = new DeclaracionArreglo(tipo, visibilidad, idArr, numDim, linea, col, this.clase);
                                    return declaracion;
                                }
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
                                declaracion.SetArchivoOrigen(archivo);
                                return declaracion;
                            }
                        }
                        if(raiz.ChildNodes.Count == 5)
                        {
                            ///PARA LOS ARRAYS                          
                            if(raiz.ChildNodes.ElementAt(4).ToString().Equals("EXP"))
                            {
                                ///TIPO + VISIBILIDAD + identificador + EMPTYDIM + "=" + EXP + ";"
                                String tipo = (String)dameTipo(raiz.ChildNodes.ElementAt(0));//OBTENGO EL TIPO

                                Estatico.Vibililidad visibilidad = (Estatico.Vibililidad)dameVisibilidad(raiz.ChildNodes.ElementAt(1));//OBTENGO LA VISIBILIDAD

                                String idArr = raiz.ChildNodes.ElementAt(2).Token.Text.ToLower();//OBTENGO EL ID DEL ARREGLO
                                int linea = raiz.ChildNodes.ElementAt(2).Token.Location.Line;
                                int col = raiz.ChildNodes.ElementAt(2).Token.Location.Column;

                                int numDim = (int)dameNumeroDimensiones(raiz.ChildNodes.ElementAt(3));//OBTENGO EL NUMERO DE DIMENSIONES

                                ASTTreeExpresion arbolExp = new ASTTreeExpresion(raiz.ChildNodes.ElementAt(4), this.clase, this.archivo);
                                Expresion exp = (Expresion)arbolExp.ConstruyeASTExpresion();

                                if (tipo!=null && exp!=null)
                                {

                                    DeclaracionArreglo decla = new DeclaracionArreglo(tipo, visibilidad, idArr, numDim, exp, linea, col, this.clase);
                                    return decla;
                                }
                            }
                            else
                            {
                                ///TIPO + VISIBILIDAD + identificador + EMPTYDIM + "=" + ARRAYDEF + ";"
                                String tipo = (String)dameTipo(raiz.ChildNodes.ElementAt(0));//OBTENGO EL TIPO

                                Estatico.Vibililidad visibilidad = (Estatico.Vibililidad)dameVisibilidad(raiz.ChildNodes.ElementAt(1));//OBTENGO LA VISIBILIDAD

                                String idArr = raiz.ChildNodes.ElementAt(2).Token.Text.ToLower();//OBTENGO EL ID DEL ARREGLO
                                int linea = raiz.ChildNodes.ElementAt(2).Token.Location.Line;
                                int col = raiz.ChildNodes.ElementAt(2).Token.Location.Column;

                                int numDim = (int)dameNumeroDimensiones(raiz.ChildNodes.ElementAt(3));//OBTENGO EL NUMERO DE DIMENSIONES

                                List<Object> arbolArreglo = (List<Object>)obtenerArbolArreglo(raiz.ChildNodes.ElementAt(4));// OBTENGO EL ARBOL QUE DESCRIBE EL ARREGLO

                                if(tipo!=null && arbolArreglo!=null)
                                {
                                    DeclaracionArreglo declaracion = new DeclaracionArreglo(tipo, visibilidad, idArr, numDim, arbolArreglo, linea, col, clase);
                                    return declaracion;
                                }
                            }
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
                            else /// ARREGLOS SIN INICIALIZAR
                            {
                                /// TIPO  identificador + EMPTYDIM + ";"
                                String tipo = (String)dameTipo(raiz.ChildNodes.ElementAt(0)); //OBTENGO EL TIPO

                                String idArr = raiz.ChildNodes.ElementAt(1).Token.Text.ToLower();//OBTENGO EL ID DEL ARREGLO
                                int linea = raiz.ChildNodes.ElementAt(1).Token.Location.Line;
                                int col = raiz.ChildNodes.ElementAt(1).Token.Location.Column;

                                int numDim = (int)dameNumeroDimensiones(raiz.ChildNodes.ElementAt(2));//OBTENGO EL NUMERO DE DIMENSIONES

                                if(tipo!=null)
                                {
                                    DeclaracionArreglo decla = new DeclaracionArreglo(tipo, Estatico.Vibililidad.LOCAL, idArr, numDim, linea, col, clase);
                                    return decla;
                                }
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
                                declaracion.SetArchivoOrigen(archivo);
                                return declaracion;
                            }
                        }
                        if(raiz.ChildNodes.Count == 4)
                        {
                            ///ESTE ES PARA LOS ARRAYS
                            if(raiz.ChildNodes.ElementAt(3).ToString().Equals("EXP"))
                            {
                                ///TIPO + identificador + EMPTYDIM + "=" + EXP + ";"
                                String tipo = (String)dameTipo(raiz.ChildNodes.ElementAt(0)); //OBTENGO EL TIPO

                                String idArr = raiz.ChildNodes.ElementAt(1).Token.Text.ToLower();//OBTENGO EL ID DEL ARREGLO
                                int linea = raiz.ChildNodes.ElementAt(1).Token.Location.Line;
                                int col = raiz.ChildNodes.ElementAt(1).Token.Location.Column;

                                int numDim = (int)dameNumeroDimensiones(raiz.ChildNodes.ElementAt(2));//OBTENGO EL NUMERO DE DIMENSIONES

                                ASTTreeExpresion arbolExp = new ASTTreeExpresion(raiz.ChildNodes.ElementAt(3), this.clase, this.archivo);
                                Expresion exp = (Expresion)arbolExp.ConstruyeASTExpresion();

                                if (tipo!=null && exp!=null)
                                {
                                    DeclaracionArreglo de = new DeclaracionArreglo(tipo, Estatico.Vibililidad.LOCAL, idArr, numDim, exp, linea, col, clase);
                                    return de;
                                }
                            }
                            else
                            {
                                ///TIPO + identificador + EMPTYDIM + "=" + ARRAYDEF + ";"
                                String tipo = (String)dameTipo(raiz.ChildNodes.ElementAt(0)); //OBTENGO EL TIPO

                                String idArr = raiz.ChildNodes.ElementAt(1).Token.Text.ToLower();//OBTENGO EL ID DEL ARREGLO
                                int linea = raiz.ChildNodes.ElementAt(1).Token.Location.Line;
                                int col = raiz.ChildNodes.ElementAt(1).Token.Location.Column;

                                int numDim = (int)dameNumeroDimensiones(raiz.ChildNodes.ElementAt(2));//OBTENGO EL NUMERO DE DIMENSIONES

                                List<Object> arbolArreglo = (List<Object>)obtenerArbolArreglo(raiz.ChildNodes.ElementAt(3));//OBTENGO LA DEFINICION DEL ARREGLO

                                if(tipo!=null && arbolArreglo!=null)
                                {
                                    DeclaracionArreglo de = new DeclaracionArreglo(tipo, Estatico.Vibililidad.LOCAL, idArr, numDim, arbolArreglo, linea, col, clase);
                                    return de;
                                }
                                
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

        #region DIMENSIONES
        public Object getDimensiones(ParseTreeNode raiz)
        {
            String etiqueta = raiz.ToString();
            switch(etiqueta)
            {
                case "DIMS":
                    {
                        List<Expresion> dimensiones = new List<Expresion>();
                        foreach (ParseTreeNode nodo in raiz.ChildNodes)
                        {
                            Expresion exp = (Expresion)getDimensiones(nodo);
                            if (exp != null)
                            {
                                dimensiones.Add(exp);
                            }
                        }
                        return dimensiones;
                    }
                case "REALDIM":
                    {
                        if (raiz.ChildNodes.Count == 1)
                        {
                            ASTTreeExpresion arbolExp = new ASTTreeExpresion(raiz.ChildNodes.ElementAt(0), this.clase, this.archivo);
                            Expresion exp = (Expresion)arbolExp.ConstruyeASTExpresion();
                            return exp;
                        }
                        break;
                    }
            }
            return null;
        }
        #endregion

        #region PARAMETROS
        private Object getParametros(ParseTreeNode raiz)
        {
            String eitqueta = raiz.ToString();
            switch(eitqueta)
            {
                case "PARAMETROS":
                    {
                        List<NodoParametro> parametros = new List<NodoParametro>();
                        if(raiz.ChildNodes.Count > 0)
                        {
                            
                            foreach(ParseTreeNode nodo in raiz.ChildNodes)
                            {
                                NodoParametro aux = (NodoParametro)getParametros(nodo);
                                if(aux!=null)
                                {
                                    parametros.Add(aux);
                                }
                            }
                        }
                        return parametros;
                    }
                case "PARAMETRO":
                    {
                        if(raiz.ChildNodes.Count == 2)
                        {
                            String tipo = (String)dameTipo(raiz.ChildNodes.ElementAt(0));
                            String idvar = raiz.ChildNodes.ElementAt(1).Token.Text.ToLower();
                            NodoParametro param = new NodoParametro(idvar, tipo, false);
                            return param;
                        }
                        if(raiz.ChildNodes.Count == 3)
                        {
                            String tipo = (String)dameTipo(raiz.ChildNodes.ElementAt(0));
                            String idvar = raiz.ChildNodes.ElementAt(1).Token.Text.ToLower();
                            List<int> dimensiones = new List<int>();
                            foreach(ParseTreeNode nodo in raiz.ChildNodes.ElementAt(2).ChildNodes)
                            {
                                if(nodo.ChildNodes.Count == 1)
                                {
                                    int val = Convert.ToInt32(nodo.ChildNodes.ElementAt(0).Token.Text);
                                    dimensiones.Add(val);
                                }
                            }
                            NodoParametro parametro = new NodoParametro(idvar, tipo, true, dimensiones);
                            return parametro;
                        }
                        break;
                    }
            }
            return new List<NodoParametro>();
        }
        #endregion

        #region SENTENCIAS
        private Object construyeSentencias(ParseTreeNode raiz)
        {
            String etiqueta = raiz.ToString();
            switch(etiqueta)
            {
                case "DECLARACION_LOCAL":
                    {
                        return recorreDeclaraciones(raiz);
                    }
                case "IMPRIMIR":
                    {
                        if(raiz.ChildNodes.Count == 2)
                        {
                            int linea = raiz.ChildNodes.ElementAt(0).Token.Location.Line;
                            int col = raiz.ChildNodes.ElementAt(0).Token.Location.Column;
                            ASTTreeExpresion exp = new ASTTreeExpresion(raiz.ChildNodes.ElementAt(1), this.clase, this.archivo);
                            Expresion res = (Expresion)exp.ConstruyeASTExpresion();
                            if(res!=null)
                            {
                                Imprimir imp = new Imprimir(this.clase, linea, col, res);
                                return imp;
                            }
                        }
                        break;
                    }
                case "ASIGNACION":
                    {
                        if(raiz.ChildNodes.Count == 2) 
                        {
                            /// identificador + "=" + EXP + ";" //2
                            
                            /// DEL HIJO EN 0 OBTENGO ID A QUIEN VOY A ASIGNAR
                                String id = raiz.ChildNodes.ElementAt(0).Token.Text.ToLower();
                                int linea = raiz.ChildNodes.ElementAt(0).Token.Location.Line;
                                int col = raiz.ChildNodes.ElementAt(0).Token.Location.Column;
                            /// DEL HIJO EN 1 OBTENGO EL VALOR A ASIGNAR
                                ASTTreeExpresion arbol = new ASTTreeExpresion(raiz.ChildNodes.ElementAt(1), this.clase, this.archivo);
                                Expresion exp = (Expresion)arbol.ConstruyeASTExpresion();
                            if(exp!=null)
                            {
                                AsignacionSimple asignar = new AsignacionSimple(exp, id, linea, col, this.clase);
                                return asignar;
                            }
                        }
                        if(raiz.ChildNodes.Count == 3)
                        {
                            if(raiz.ChildNodes.ElementAt(1).ToString().Contains("identificador"))
                            {
                                /// LLAMADAID_OBJ + "." + identificador + "=" + EXP + ";"
                                
                                /// DEL HIJO 0 TENGO EL VALOR DE TIPO OBJETO QUE ESPERO SIEMPRE
                                ASTTreeExpresion arbol = new ASTTreeExpresion(raiz.ChildNodes.ElementAt(0), this.clase, this.archivo);
                                Expresion exp1 = (Expresion)arbol.traeLlamadas(raiz.ChildNodes.ElementAt(0));
                                /// DEL HIJO 1 OBTENGO LA PROPIEDAD A LA QUE SE LE VA ASIGNAR EL VALOR
                                String propiedad = raiz.ChildNodes.ElementAt(1).Token.Text.ToLower();
                                int linea = raiz.ChildNodes.ElementAt(1).Token.Location.Line;
                                int col = raiz.ChildNodes.ElementAt(1).Token.Location.Column;
                                /// DEL HIJO 2 OBTENGO EL VALOR A ASIGNAR EN LA PROPIEDAD
                                ASTTreeExpresion arbol2 = new ASTTreeExpresion(raiz.ChildNodes.ElementAt(2), this.clase, this.archivo);
                                Expresion exp2 = (Expresion)arbol2.ConstruyeASTExpresion();

                                if (exp1 != null && exp2 != null)
                                {
                                    AsignacionPropiedad asig = new AsignacionPropiedad(exp1, propiedad, exp2, linea, col, this.clase);
                                    return asig;
                                }
                            }
                            else if(raiz.ChildNodes.ElementAt(1).ToString().Contains("DIMS"))
                            {
                                /// identificador + DIMS + "=" + EXP + ";";
                                /// 
                                String identificadorArr = raiz.ChildNodes.ElementAt(0).Token.Text.ToLower();/// obtengo el id
                                int linea = raiz.ChildNodes.ElementAt(0).Token.Location.Line;
                                int col = raiz.ChildNodes.ElementAt(0).Token.Location.Column;

                                List<Expresion> dimensiones = (List<Expresion>)getDimensiones(raiz.ChildNodes.ElementAt(1)); //OBTENGO LAS DIMENSIONES

                                ASTTreeExpresion arbolExp = new ASTTreeExpresion(raiz.ChildNodes.ElementAt(2), this.clase, this.archivo); //OBTENTO LA EXPRESION QUE SE ASIGNA
                                Expresion exp = (Expresion)arbolExp.ConstruyeASTExpresion();

                                if(exp!=null && exp!=null)
                                {
                                    AsignacionSimpleArreglo asig = new AsignacionSimpleArreglo(identificadorArr, dimensiones, exp, linea, col, clase);
                                    return asig;
                                }
                                
                            }
                        }
                        if(raiz.ChildNodes.Count == 4)
                        {
                            /// LLAMADAID_OBJ + "." + identificador + DIMS + "=" + EXP + ";"
                            ASTTreeExpresion arbol = new ASTTreeExpresion(raiz.ChildNodes.ElementAt(0), this.clase, this.archivo);
                            Expresion exp1 = (Expresion)arbol.traeLlamadas(raiz.ChildNodes.ElementAt(0));

                            String identificadorArr = raiz.ChildNodes.ElementAt(1).Token.Text.ToLower();/// obtengo el id
                            int linea = raiz.ChildNodes.ElementAt(1).Token.Location.Line;
                            int col = raiz.ChildNodes.ElementAt(1).Token.Location.Column;

                            List<Expresion> dimensiones = (List<Expresion>)getDimensiones(raiz.ChildNodes.ElementAt(2)); //OBTENGO LAS DIMENSIONES

                            ASTTreeExpresion arbolExp = new ASTTreeExpresion(raiz.ChildNodes.ElementAt(3), this.clase, this.archivo); //OBTENTO LA EXPRESION QUE SE ASIGNA
                            Expresion exp = (Expresion)arbolExp.ConstruyeASTExpresion();

                            if(exp!=null && dimensiones!=null)
                            {
                                AsignacionArregloComp asig = new AsignacionArregloComp(exp1, new AsignacionSimpleArreglo(identificadorArr, dimensiones, exp, linea,col, clase), linea, col, clase);
                                return asig;
                            }
                        }
                        break;
                    }
                case "RETORNO":
                    {
                        if(raiz.ChildNodes.Count == 2)
                        {
                            //RETORNO VACIO
                            ASTTreeExpresion arbol = new ASTTreeExpresion(raiz.ChildNodes.ElementAt(1), this.clase, this.archivo);
                            Expresion exp = (Expresion)arbol.ConstruyeASTExpresion();
                            int linea = raiz.ChildNodes.ElementAt(0).Token.Location.Line;
                            int col = raiz.ChildNodes.ElementAt(0).Token.Location.Column;
                            if(exp!=null)
                            {
                                Retorno r = new Retorno(exp,linea, col, this.clase);
                                return r;
                            }
                        }
                        else if(raiz.ChildNodes.Count == 1)
                        {
                            int linea = raiz.ChildNodes.ElementAt(0).Token.Location.Line;
                            int col = raiz.ChildNodes.ElementAt(0).Token.Location.Column;
                            return new Retorno(linea, col, this.clase);
                        }
                        break;
                    }
                case "CONDICIONAL_SI":
                    {
                        if(raiz.ChildNodes.Count == 4)
                        {
                            int linea = raiz.ChildNodes.ElementAt(0).Token.Location.Line;
                            int col = raiz.ChildNodes.ElementAt(0).Token.Location.Column;

                            ASTTreeExpresion expr = new ASTTreeExpresion(raiz.ChildNodes.ElementAt(1), this.clase, this.archivo);
                            Expresion exp = (Expresion)expr.ConstruyeASTExpresion();

                            List<object> instruccionesVerd = new List<object>();

                            foreach (ParseTreeNode nodo in raiz.ChildNodes.ElementAt(2).ChildNodes)
                            {
                                Object instruccion = (Object)construyeSentencias(nodo);
                                if (instruccion != null)
                                {
                                    instruccionesVerd.Add(instruccion);
                                }
                            }

                            List<object> instrucFalsas = (List<object>)construyeSentencias(raiz.ChildNodes.ElementAt(3));

                            if(instruccionesVerd!=null && instrucFalsas!=null && exp!=null)
                            {
                                SentenciaSi s = new SentenciaSi(exp, instruccionesVerd, instrucFalsas, linea, col, this.clase);
                                return s;
                            }
                        }
                        if(raiz.ChildNodes.Count == 3)
                        {
                            int linea = raiz.ChildNodes.ElementAt(0).Token.Location.Line;
                            int col = raiz.ChildNodes.ElementAt(0).Token.Location.Column;

                            ASTTreeExpresion expr = new ASTTreeExpresion(raiz.ChildNodes.ElementAt(1), this.clase, this.archivo);
                            Expresion exp = (Expresion)expr.ConstruyeASTExpresion();

                            List<object> instruccionesVerd = new List<object>();

                            foreach(ParseTreeNode nodo in raiz.ChildNodes.ElementAt(2).ChildNodes)
                            {
                                Object instruccion = (Object)construyeSentencias(nodo);
                                if(instruccion!=null)
                                {
                                    instruccionesVerd.Add(instruccion);
                                }
                            }

                            if(exp!=null)
                            {
                                SentenciaSi si = new SentenciaSi(exp, instruccionesVerd, this.clase, linea, col);
                                return si;
                            }

                        }
                        break;
                    }
                case "SINO":
                    {
                        if(raiz.ChildNodes.Count == 2)
                        {

                            List<Object> sentFalsa = new List<object>();
                            if(raiz.ChildNodes.ElementAt(1).ToString().Equals("SENTENCIAS"))
                            {
                                foreach(ParseTreeNode nodo in raiz.ChildNodes.ElementAt(1).ChildNodes)
                                {
                                    Object ins = construyeSentencias(nodo);
                                    if(ins!=null)
                                    {
                                        sentFalsa.Add(ins);
                                    }
                                }
                            }
                            else if(raiz.ChildNodes.ElementAt(1).ToString().Equals("CONDICIONAL_SI"))
                            {
                                Instruccion ins = (Instruccion)construyeSentencias(raiz.ChildNodes.ElementAt(1));
                                if(ins!=null)
                                {
                                    sentFalsa.Add(ins);
                                }
                            }

                            return sentFalsa;
                        }
                        break;
                    }
                case "MIENTRAS":
                    {
                        if(raiz.ChildNodes.Count == 3)
                        {
                            int linea = raiz.ChildNodes.ElementAt(0).Token.Location.Line;
                            int col = raiz.ChildNodes.ElementAt(0).Token.Location.Column;

                            ASTTreeExpresion arbolExpre = new ASTTreeExpresion(raiz.ChildNodes.ElementAt(1), this.clase, this.archivo);
                            Expresion condicion = (Expresion)arbolExpre.ConstruyeASTExpresion();

                            List<Object> sentencias = new List<object>();

                            foreach(ParseTreeNode nodo in raiz.ChildNodes.ElementAt(2).ChildNodes)
                            {
                                Object ins = construyeSentencias(nodo);
                                if(ins!=null)
                                {
                                    sentencias.Add(ins);
                                }
                            }

                            if(condicion!=null)
                            {
                                SentenciaMientras sente = new SentenciaMientras(condicion, sentencias, this.clase, linea, col);
                                return sente;
                            }
                            
                        }
                        break;
                    }
                case "ROMPER":
                    {
                        return new Romper();
                    }
                case "CONTINUAR":
                    {
                        return new Continuar();
                    }
                case "LLAMADAFUN":
                    {
                        if(raiz.ChildNodes.Count == 1)
                        {
                            ASTTreeExpresion aux = new ASTTreeExpresion(this.raiz.ChildNodes.ElementAt(0), this.clase, this.archivo);

                            Expresion exp = (Expresion)aux.traeLlamadas(raiz.ChildNodes.ElementAt(0));

                            if(exp!=null)
                            {
                                LLamadaFuncion f = new LLamadaFuncion(this.clase, 0, 0, exp);
                                return f;
                            }
                        }
                        break;
                    }
                case "HACERMIENTRAS":
                    {
                        if(raiz.ChildNodes.Count == 4)
                        {
                            ASTTreeExpresion arbol = new ASTTreeExpresion(raiz.ChildNodes.ElementAt(3), this.clase, this.archivo);
                            Expresion condicion = (Expresion)arbol.ConstruyeASTExpresion();
                            List<Object> intrucciones = new List<object>();
                            foreach(ParseTreeNode nodo in raiz.ChildNodes.ElementAt(1).ChildNodes)
                            {
                                Object instruccion = construyeSentencias(nodo);
                                if(instruccion!=null)
                                {
                                    intrucciones.Add(instruccion);
                                }
                            }

                            if(condicion!=null)
                            {
                                int linea = raiz.ChildNodes.ElementAt(0).Token.Location.Line;
                                int col = raiz.ChildNodes.ElementAt(0).Token.Location.Column;
                                HacerMientras ciclo = new HacerMientras(condicion, intrucciones, this.clase, linea, col);
                                return ciclo;
                            }
                        }
                        break;
                    }
                case "REPETIRHASTA":
                    {
                        if(raiz.ChildNodes.Count == 4)
                        {
                            int linea = raiz.ChildNodes.ElementAt(0).Token.Location.Line;
                            int col = raiz.ChildNodes.ElementAt(0).Token.Location.Column;

                            List<Object> instrucciones = new List<object>();
                            foreach(ParseTreeNode nodo in raiz.ChildNodes.ElementAt(1).ChildNodes)
                            {
                                object instruccion = construyeSentencias(nodo);
                                if(instruccion!=null)
                                {
                                    instrucciones.Add(instruccion);
                                }
                            }

                            ASTTreeExpresion arbol = new ASTTreeExpresion(raiz.ChildNodes.ElementAt(3), this.clase, this.archivo);
                            Expresion exp = (Expresion)arbol.ConstruyeASTExpresion();

                            if(exp!=null)
                            {
                                RepetirHasta rep = new RepetirHasta(instrucciones, exp, this.clase, linea, col);
                                return rep;
                            }
                        }
                        break;
                    }
                case "INCREMENTO":
                    {
                        if(raiz.ChildNodes.Count == 2)
                        {
                            String id = raiz.ChildNodes.ElementAt(0).Token.Text.ToLower();
                            int linea = raiz.ChildNodes.ElementAt(0).Token.Location.Line;
                            int col = raiz.ChildNodes.ElementAt(0).Token.Location.Column;

                            Incremento inc = new Incremento(id, linea, col, this.clase);
                            return inc;
                        }
                        break;
                    }
                case "DECREMENTO":
                    {
                        if(raiz.ChildNodes.Count == 2)
                        {
                            String id = raiz.ChildNodes.ElementAt(0).Token.Text.ToLower();
                            int linea = raiz.ChildNodes.ElementAt(0).Token.Location.Line;
                            int col = raiz.ChildNodes.ElementAt(0).Token.Location.Column;

                            Decremento dec = new Decremento(id.ToLower(), this.clase, linea, col);
                            return dec;
                        }
                        break;
                    }
                case "FOR":
                    {
                        if(raiz.ChildNodes.Count == 5)
                        {
                            int linea = raiz.ChildNodes.ElementAt(0).Token.Location.Line;
                            int col = raiz.ChildNodes.ElementAt(0).Token.Location.Column;

                            Instruccion varControl = (Instruccion)construyeSentencias(raiz.ChildNodes.ElementAt(1)); //TOMO LA ASIGNACION O LA DELCARACION

                            ASTTreeExpresion arbol = new ASTTreeExpresion(raiz.ChildNodes.ElementAt(2), clase, archivo);
                            Expresion condicion = (Expresion)arbol.ConstruyeASTExpresion();

                            Instruccion operacion = (Instruccion)construyeSentencias(raiz.ChildNodes.ElementAt(3));// OPERACION DEL CICLO

                            List<object> instrucciones = new List<object>();

                            foreach(ParseTreeNode nodo in raiz.ChildNodes.ElementAt(4).ChildNodes)
                            {
                                object ins = construyeSentencias(nodo);
                                if(ins!=null)
                                {
                                    instrucciones.Add(ins);
                                }
                            }
                            
                            if(varControl!=null && condicion!=null && operacion!=null)
                            {
                                CicloPara para = new CicloPara(varControl, condicion, operacion, instrucciones, clase, linea, col);
                                return para;
                            }

                        }
                        break;
                    }
                case "VARCONTROL":
                    {
                        if(raiz.ChildNodes.Count == 2)
                        {
                            int linea = raiz.ChildNodes.ElementAt(0).Token.Location.Line;
                            int col = raiz.ChildNodes.ElementAt(0).Token.Location.Column;
                            String id = raiz.ChildNodes.ElementAt(0).Token.Text.ToLower();

                            ASTTreeExpresion arbol = new ASTTreeExpresion(raiz.ChildNodes.ElementAt(1), this.clase, this.archivo);
                            Expresion exp = (Expresion)arbol.ConstruyeASTExpresion();

                            if(exp!=null)
                            {
                                AsignacionSimple asig = new AsignacionSimple(exp, id.ToLower(), linea, col, clase);
                                return asig;
                            }
                        }
                        else if(raiz.ChildNodes.Count == 3)
                        {
                            String tipo = (String)dameTipo(raiz.ChildNodes.ElementAt(0));

                            int linea = raiz.ChildNodes.ElementAt(1).Token.Location.Line;
                            int col = raiz.ChildNodes.ElementAt(1).Token.Location.Column;
                            String id = raiz.ChildNodes.ElementAt(1).Token.Text.ToLower();

                            ASTTreeExpresion arbol = new ASTTreeExpresion(raiz.ChildNodes.ElementAt(2), this.clase, this.archivo);
                            Expresion exp = (Expresion)arbol.ConstruyeASTExpresion();

                            if(tipo!=null && exp!=null)
                            {
                                DeclaracionVar dec = new DeclaracionVar(id.ToLower(), tipo, Estatico.Vibililidad.LOCAL, linea, col, this.clase);
                                return dec;
                            }
                        }
                        break;
                    }
                case "OPERACION":
                    {
                        if(raiz.ChildNodes.Count ==2)
                        {
                            int linea = raiz.ChildNodes.ElementAt(0).Token.Location.Line;
                            int col = raiz.ChildNodes.ElementAt(0).Token.Location.Column;
                            String identificador = raiz.ChildNodes.ElementAt(0).Token.Text.ToLower();

                            if (raiz.ChildNodes.ElementAt(1).ToString().Contains("++"))
                            {
                                Incremento inc = new Incremento(identificador.ToLower(), linea, col, clase);
                                return inc;
                            }
                            else if(raiz.ChildNodes.ElementAt(1).ToString().Contains("--"))
                            {
                                Decremento dec = new Decremento(identificador.ToLower(),clase, linea, col);
                                return dec;
                            }
                            else if(raiz.ChildNodes.ElementAt(1).ToString().Contains("EXP"))
                            {
                                ASTTreeExpresion arbol = new ASTTreeExpresion(raiz.ChildNodes.ElementAt(1), clase, archivo);
                                Expresion exp = (Expresion)arbol.ConstruyeASTExpresion();

                                AsignacionSimple sig = new AsignacionSimple(exp, identificador.ToLower(), linea, col, clase);
                                return sig;
                            }
                        }
                        break;
                    }
            }
            return null;
        }
        #endregion

        #region DIMVACIAS
        private object dameNumeroDimensiones(ParseTreeNode raiz)
        {
            String etiqueta = raiz.ToString();
            switch(etiqueta)
            {
                case "EMPTYDIM":
                    {
                        if(raiz.ChildNodes.Count > 0)
                        {
                            int d = 0;
                            foreach(ParseTreeNode n in raiz.ChildNodes)
                            {
                                d++;
                            }
                            return d;
                        }
                        break;
                    }
            }
            return 1;
        }
        #endregion

        #region DEFINICIONARRAY
        private Object obtenerArbolArreglo(ParseTreeNode raiz)
        {
            String etiqueta = raiz.ToString();
            switch(etiqueta)
            {
                case "ARRAYDEF":
                    {
                        if(raiz.ChildNodes.Count == 1)
                        {
                            return obtenerArbolArreglo(raiz.ChildNodes.ElementAt(0));
                        }
                        break;
                    }
                case "ARRELEMENTS":
                    {
                        if(raiz.ChildNodes.Count > 0)
                        {
                            List<Object> elementos = new List<object>();
                            foreach(ParseTreeNode n in raiz.ChildNodes)
                            {
                                Object elem = obtenerArbolArreglo(n);
                                if(elem!=null)
                                {
                                    elementos.Add(elem);
                                }
                            }
                            return elementos;
                        }
                        break;
                    }
                case "EXP":
                    {
                        if(raiz.ChildNodes.Count == 1)
                        {
                            ASTTreeExpresion exp = new ASTTreeExpresion(raiz, this.clase, this.archivo);
                            Expresion expre = (Expresion)exp.ConstruyeASTExpresion();
                            return expre;
                        }
                        break;
                    }
            }
            return null;
        }
        #endregion
    }
}
