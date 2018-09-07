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
                            else if(raiz.ChildNodes.ElementAt(3).ToString().Equals("DIMENSIONES"))
                            {
                                //AQUI MANEJO LAS DIMENSIONES DEL ARRAY
                                String tipo = (String)dameTipo(raiz.ChildNodes.ElementAt(0));//1
                                Estatico.Vibililidad vibililidad = (Estatico.Vibililidad)dameVisibilidad(raiz.ChildNodes.ElementAt(1));//2
                                String idva = raiz.ChildNodes.ElementAt(2).Token.Text.ToLower();//3
                                /////////////////////////////////////////////////////////////////
                                // AQUI PIDO LAS DIMENSIONES DEL ARREGLO
                                Dimensiones dim = (Dimensiones)getDimensiones(raiz.ChildNodes.ElementAt(3));//4
                                if(tipo!=null && dim!=null)
                                {
                                    int linea = raiz.ChildNodes.ElementAt(2).Token.Location.Line;
                                    int col = raiz.ChildNodes.ElementAt(2).Token.Location.Column;
                                    DeclaracionArr declaracion = new DeclaracionArr(dim,tipo, linea, col, this.clase);
                                    declaracion.SetArchivoOrigen(archivo);
                                    return declaracion;
                                }
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
                                declaracion.SetArchivoOrigen(archivo);
                                return declaracion;
                            }
                        }
                        if(raiz.ChildNodes.Count == 5)
                        {
                            //TIPO + VISIBILIDAD + identificador + DIMENSIONES + "=" + EXP + ";";
                            String tipo = (String)dameTipo(raiz.ChildNodes.ElementAt(0));//1
                            Estatico.Vibililidad vibililidad = (Estatico.Vibililidad)dameVisibilidad(raiz.ChildNodes.ElementAt(1));//2
                            String idva = raiz.ChildNodes.ElementAt(2).Token.Text.ToLower();//3
                            Dimensiones dim = (Dimensiones)getDimensiones(raiz.ChildNodes.ElementAt(3));//4
                            ASTTreeExpresion arbolExpre = new ASTTreeExpresion(raiz.ChildNodes.ElementAt(4), this.clase, this.archivo);
                            Expresion exp = (Expresion)arbolExpre.ConstruyeASTExpresion();
                            if(tipo!=null && dim!=null && exp!=null)
                            {
                                int linea = raiz.ChildNodes.ElementAt(2).Token.Location.Line;
                                int col = raiz.ChildNodes.ElementAt(2).Token.Location.Column;
                                DeclaracionArr declaracion = new DeclaracionArr(dim, tipo, exp, linea, col, clase);
                                declaracion.SetArchivoOrigen(archivo);
                                return declaracion;
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
                            else if(raiz.ChildNodes.ElementAt(2).ToString().Equals("DIMENSIONES"))
                            {
                                //MANEJO LAS DIMENSIONES
                                String tipo = (String)dameTipo(raiz.ChildNodes.ElementAt(0));
                                String idvar = raiz.ChildNodes.ElementAt(1).Token.Text.ToLower();
                                /////////////////////////////////////////////////////////////////
                                // AQUI PIDO LAS DIMENSIONES DEL ARREGLO
                                Dimensiones dim = (Dimensiones)getDimensiones(raiz.ChildNodes.ElementAt(2));
                                if(tipo!=null && dim!=null)
                                {
                                    int linea = raiz.ChildNodes.ElementAt(1).Token.Location.Line;
                                    int col = raiz.ChildNodes.ElementAt(1).Token.Location.Column;
                                    DeclaracionArr arr = new DeclaracionArr(dim, tipo,  linea, col, clase);
                                    arr.SetArchivoOrigen(archivo);
                                    return arr;
                                }
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
                                declaracion.SetArchivoOrigen(archivo);
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
                            Dimensiones dim = (Dimensiones)getDimensiones(raiz.ChildNodes.ElementAt(2));
                            ///////////////////////////////////////////////////////////////////////////////////////////////////
                            ASTTreeExpresion aux = new ASTTreeExpresion(raiz.ChildNodes.ElementAt(3), this.clase, this.archivo);
                            Expresion exp = (Expresion)aux.ConstruyeASTExpresion();
                            ///////////////////////////////////////////////////////////////////////////////////////////////////
                            if(tipo!=null && dim!=null && exp!=null)
                            {
                                int linea = raiz.ChildNodes.ElementAt(1).Token.Location.Line;
                                int col = raiz.ChildNodes.ElementAt(1).Token.Location.Column;
                                DeclaracionArr declaracion = new DeclaracionArr(dim, tipo,  exp, linea, col, clase);
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
                case "DIMENSIONES":
                    {
                        List<Expresion> expr = new List<Expresion>();
                        foreach(ParseTreeNode nodo in raiz.ChildNodes)
                        {
                            Expresion aux = (Expresion)getDimensiones(nodo);
                            if(aux!=null)
                            {
                                expr.Add(aux);
                            }
                        }
                        Dimensiones dim = new Dimensiones(expr, 0, 0, this.clase);
                        return dim;
                    }
                case "DIMENSION":
                    {
                        if(raiz.ChildNodes.Count==1)
                        {
                            ASTTreeExpresion arbolExpre = new ASTTreeExpresion(raiz.ChildNodes.ElementAt(0), this.clase, this.archivo);
                            return arbolExpre.ConstruyeASTExpresion();
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

                            if(exp1!=null && exp2!=null)
                            {
                                AsignacionPropiedad asig = new AsignacionPropiedad(exp1, propiedad, exp2, linea, col, this.clase);
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
            }
            return null;
        }
        #endregion
    }
}
