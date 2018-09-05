using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.ASTTree.Interfaces;
using XForms.ASTTree.Instrucciones;
using XForms.Objs;
using Irony.Parsing;
using XForms.ASTTree.Valores;
namespace XForms.ASTTree.ASTConstructor
{
    class ASTTreeExpresion
    {
        ParseTreeNode raiz; /*RAIZ DEL CUERPO DE LA CLASE*/
        String clase; /*SOLO POR SI OCURRE UN ERROR DURANTE EL PARSER*/
        String archivo; /*SOLO SI OCURRE UN ERROR LO UTILIZARE*/

        public ASTTreeExpresion(ParseTreeNode raiz, String clase, String Archivo)
        {
            this.raiz = raiz;
            this.clase = clase;
            this.archivo = Archivo;
        }

        public Object ConstruyeASTExpresion()
        {

            return recorreExpresion(this.raiz);
        }

        private Object recorreExpresion(ParseTreeNode raiz)
        {
            String etiqueta = raiz.ToString();
            switch(etiqueta)
            {
                case "EXP":
                    {
                        if(raiz.ChildNodes.Count == 1)
                        {
                            return recorreExpresion(raiz.ChildNodes.ElementAt(0));
                        }
                        break;
                    }
                case "E":
                    {
                        if(raiz.ChildNodes.Count == 3)//CUANDO SE REALIZA UNA SUMA
                        {
                            Expresion operando1 = (Expresion)recorreExpresion(raiz.ChildNodes.ElementAt(0));
                            Expresion operando2 = (Expresion)recorreExpresion(raiz.ChildNodes.ElementAt(2));
                            Estatico.Operandos oper = (Estatico.Operandos)dameOperador(raiz.ChildNodes.ElementAt(1));
                            int linea = raiz.ChildNodes.ElementAt(1).Token.Location.Line;
                            int colum = raiz.ChildNodes.ElementAt(1).Token.Location.Column;
                            Operacion op = new Operacion(operando1, operando2, oper, linea, colum, this.clase);
                            op.SetArchivoOrigen(this.archivo);
                            return op;
                        }
                        if(raiz.ChildNodes.Count == 2)
                        {
                            if(raiz.ChildNodes.ElementAt(0).ToString().Equals("E"))//AUMENTO O DECREMENTO
                            {
                                Expresion operando = (Expresion)recorreExpresion(raiz.ChildNodes.ElementAt(0));
                                Estatico.Operandos oper = (Estatico.Operandos)dameOperador(raiz.ChildNodes.ElementAt(1));
                                int linea = raiz.ChildNodes.ElementAt(1).Token.Location.Line;
                                int colum = raiz.ChildNodes.ElementAt(1).Token.Location.Column;
                                Operacion op = new Operacion(operando, oper, linea, colum, this.clase);
                                op.SetArchivoOrigen(archivo);
                                return op;
                            }
                            else//NEGATIVO O NOT
                            {
                                Expresion operando = (Expresion)recorreExpresion(raiz.ChildNodes.ElementAt(1));
                                Estatico.Operandos oper = (Estatico.Operandos)dameOperador(raiz.ChildNodes.ElementAt(0));
                                int linea = raiz.ChildNodes.ElementAt(0).Token.Location.Line;
                                int colum = raiz.ChildNodes.ElementAt(0).Token.Location.Column;
                                Operacion op = new Operacion(operando, oper, linea, colum, this.clase);
                                op.SetArchivoOrigen(archivo);
                                return op;
                            }
                        }
                        if(raiz.ChildNodes.Count == 1)
                        {
                            if(raiz.ChildNodes.ElementAt(0).ToString().Equals("E"))
                            {
                                return recorreExpresion(raiz.ChildNodes.ElementAt(0));
                            }
                            else
                            {
                                return recorreTerminales(raiz.ChildNodes.ElementAt(0));
                            }
                        }
                        break;
                    }
            }
            return null;
        }

        private Object dameOperador(ParseTreeNode raiz)
        {
            String op = raiz.Token.Text;
            switch(op)
            {
                case "+":
                    {
                        return Estatico.Operandos.SUMA;
                    }
                case "-":
                    {
                        return Estatico.Operandos.RESTA;
                    }
                case "*":
                    {
                        return Estatico.Operandos.MULT;
                    }
                case "/":
                    {
                        return Estatico.Operandos.DIVI;
                    }
                case "^":
                    {

                        return Estatico.Operandos.POT;
                    }
                case "%":
                    {
                        return Estatico.Operandos.MOD;
                    }
                case "++":
                    {
                        return Estatico.Operandos.INC;
                    }
                case "--":
                    {
                        return Estatico.Operandos.DEC;
                    }
                case "<":
                    {
                        return Estatico.Operandos.MENOR;
                    }
                case ">":
                    {
                        return Estatico.Operandos.MAYOR;
                    }
                case "<=":
                    {
                        return Estatico.Operandos.MENORIGUAL;
                    }
                case ">=":
                    {
                        return Estatico.Operandos.MAYORIGUAL;
                    }
                case "==":
                    {
                        return Estatico.Operandos.IGUAL;
                    }
                case "!=":
                    {
                        return Estatico.Operandos.DIFERENTE;
                    }
                case "&&":
                    {
                        return Estatico.Operandos.AND;
                    }
                case "||":
                    {
                        return Estatico.Operandos.OR;
                    }
                case "!":
                    {
                        return Estatico.Operandos.NOT;
                    }
            }
            return Estatico.Operandos.SUMA;
        }

        private Object recorreTerminales(ParseTreeNode raiz)
        {
            String tipoterminal = raiz.Term.Name;
            
            switch (tipoterminal)
            {
                case "cadena":
                    {
                        int linea = raiz.Token.Location.Line;
                        int col = raiz.Token.Location.Column;
                        String val = raiz.Token.Text;
                        val = val.Replace("\"", "");
                        ValorPrimitivo valor = new ValorPrimitivo(val, linea, col, this.clase);
                        valor.SetArchivoOrigen(archivo);
                        return valor;
                    }
                case "cadena2":
                    {
                        int linea = raiz.Token.Location.Line;
                        int col = raiz.Token.Location.Column;
                        String val = raiz.Token.Text;
                        object vallor = parsearCadena2(val);
                        ValorPrimitivo valor = new ValorPrimitivo(vallor, linea, col, this.clase);
                        valor.SetArchivoOrigen(archivo);
                        return valor;
                    }
                case "entero":
                    {
                        int linea = raiz.Token.Location.Line;
                        int col = raiz.Token.Location.Column;
                        int val = Convert.ToInt32(raiz.Token.Text);
                        ValorPrimitivo valor = new ValorPrimitivo(val, linea, col, this.clase);
                        valor.SetArchivoOrigen(archivo);
                        return valor;
                    }
                case "decimal":
                    {
                        int linea = raiz.Token.Location.Line;
                        int col = raiz.Token.Location.Column;
                        double val = Convert.ToDouble(raiz.Token.Text);
                        ValorPrimitivo valor = new ValorPrimitivo(val, linea, col, this.clase);
                        valor.SetArchivoOrigen(archivo);
                        return valor;
                    }
                case "verdadero":
                    {
                        int linea = raiz.Token.Location.Line;
                        int col = raiz.Token.Location.Column;
                        bool val = true;
                        ValorPrimitivo valor = new ValorPrimitivo(val, linea, col, this.clase);
                        return valor;
                    }
                case "falso":
                    {
                        int linea = raiz.Token.Location.Line;
                        int col = raiz.Token.Location.Column;
                        bool val = false;
                        ValorPrimitivo valor = new ValorPrimitivo(val, linea, col, this.clase);
                        valor.SetArchivoOrigen(archivo);
                        return valor;
                    }
                case "nulo":
                    {
                        int linea = raiz.Token.Location.Line;
                        int col = raiz.Token.Location.Column;
                        Nulo val = new Nulo();
                        ValorPrimitivo valor = new ValorPrimitivo(val, linea, col, this.clase);
                        valor.SetArchivoOrigen(archivo);
                        return valor;
                    }
                default:
                    {
                        return traeLlamadas(raiz);
                    }
            }
        }


        private Object traeLlamadas(ParseTreeNode raiz)
        {
            String eti = raiz.Term.Name;
            switch(eti)
            {
                case "LLAMADAID_OBJ":
                    {
                        if(raiz.ChildNodes.Count > 0)
                        {
                            Accesos acceso = new Accesos(0, 0, this.clase);
                            acceso.SetArchivoOrigen(archivo);
                            foreach(ParseTreeNode n in raiz.ChildNodes)
                            {
                                Expresion ex = (Expresion)traeLlamadas(n);
                                if(ex!=null)
                                {
                                    acceso.AddExpresion(ex);
                                }
                            }
                            return acceso;
                        }
                        break;
                    }
                case "LLAMADA":
                    {
                        if(raiz.ChildNodes.Count == 2)
                        {
                            String id = raiz.ChildNodes.ElementAt(0).Token.Text.ToLower();//MINUSCULAS
                            int linea = raiz.ChildNodes.ElementAt(0).Token.Location.Line;
                            int col = raiz.ChildNodes.ElementAt(0).Token.Location.Column;
                            Llamada llam = new Llamada(id, linea, col, this.clase);
                            llam.SetArchivoOrigen(archivo);
                            if(raiz.ChildNodes.ElementAt(1).ChildNodes.Count > 0)//SI EL HIJO DERECHO TIENE MAS DE UN HIJO ENTONCES LO RECORRO COMO EXP
                            {
                                foreach(ParseTreeNode n in raiz.ChildNodes.ElementAt(1).ChildNodes)
                                {
                                    Expresion aux = (Expresion)recorreExpresion(n);//LOS TOMO...
                                    if(aux!=null)//SI NO ES NULL...
                                    {
                                        llam.AddExpresion(aux);//LO ANADO
                                    }
                                }
                            }
                            return llam;
                        }
                        break;
                    }
                case "identificador":
                    {
                        int linea = raiz.Token.Location.Line;
                        int col = raiz.Token.Location.Column;
                        String id = raiz.Token.Text.ToLower();//MINUSCULAS
                        Identificador ide = new Identificador(id, col, linea, this.clase);
                        ide.SetArchivoOrigen(archivo);
                        return ide;
                    }
                case "ARRAY":
                    {
                        if(raiz.ChildNodes.Count ==1)
                        {
                            List<Expresion> expresionesArr = new List<Expresion>();
                            foreach(ParseTreeNode nodo in raiz.ChildNodes.ElementAt(0).ChildNodes)
                            {
                                Expresion exp = (Expresion)recorreExpresion(nodo);
                                if(exp!=null)
                                {
                                    expresionesArr.Add(exp);
                                }
                            }
                            ValorArreglo val = new ValorArreglo(expresionesArr, 0, 0, this.clase);
                            val.SetArchivoOrigen(archivo);
                            return val;
                        }
                        if(raiz.ChildNodes.Count == 3)
                        {
                            String tipo = (String)dameTipo(raiz.ChildNodes.ElementAt(1));
                            Dimensiones dim = (Dimensiones)getDimensiones(raiz.ChildNodes.ElementAt(2));
                            dim.SetArchivoOrigen(archivo);
                            ///AQUI CREO EL NODO NUEVOARREGLO QUE ME DEVOLVERA UN ARREGLO CON LAS CARACTERISTICAS INDICADAS
                            ///
                            if(tipo!=null & dim!=null)
                            {
                                int linea = raiz.ChildNodes.ElementAt(0).Token.Location.Line;
                                int col = raiz.ChildNodes.ElementAt(0).Token.Location.Column;
                                NuevoArreglo nuevo = new NuevoArreglo(dim, tipo, linea, col, clase);
                                nuevo.SetArchivoOrigen(archivo);
                                return nuevo;
                            }
                        }
                        break;
                    }
                case "DECLARACION_OBJ":
                    {
                        if(raiz.ChildNodes.Count == 3)
                        {
                            String tipo = (String)dameTipo(raiz.ChildNodes.ElementAt(1));
                            List<Expresion> parametros = new List<Expresion>();
                            foreach(ParseTreeNode nodo in raiz.ChildNodes.ElementAt(2).ChildNodes)
                            {
                                Expresion aux = (Expresion)recorreExpresion(nodo);
                                if(aux!=null)
                                {
                                    parametros.Add(aux);
                                }
                            }
                            if(tipo!=null)
                            {
                                int linea = raiz.ChildNodes.ElementAt(0).Token.Location.Line;
                                int col = raiz.ChildNodes.ElementAt(0).Token.Location.Column;
                                NuevoObjeto ob = new NuevoObjeto(parametros, tipo, linea, col, this.clase);
                                ob.SetArchivoOrigen(archivo);
                                return ob;
                            }
                        }
                        break;
                    }
            }
            return null;
        }

        private Object parsearCadena2(String valor)
        {
            valor = valor.Replace("'", "");
            if(valor.ToLower().Equals("verdadero"))
            {
                return true;
            }
            if(valor.ToLower().Equals("falso"))
            {
                return false;
            }
            try
            {
                DateTime val = DateTime.ParseExact(valor, "dd/MM/yyyy hh:mm:ss",System.Globalization.CultureInfo.InvariantCulture);
                return val;
            }
            catch
            {
                goto ParseDate;
            }
            ParseDate:
            try
            {
                DateTime val = DateTime.ParseExact(valor + " 00:00:00", "dd/MM/yyyy hh:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                Date fecha = new Date(val);
                return fecha;
            }
            catch
            {
                goto ParseHora;
            }
            ParseHora:
            try
            {
                DateTime val = DateTime.ParseExact("12/10/1996 "+valor, "dd/MM/yyyy hh:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                Hour hora = new Hour(val);
                return hora;
            }
            catch
            {
                goto DevuelveCadena;
            }
            DevuelveCadena:
            return valor;
        }

        #region TIPOS
        private Object dameTipo(ParseTreeNode raiz)
        {
            String etiqueta = raiz.ToString();
            switch (etiqueta)
            {
                case "TIPO":
                    {
                        if (raiz.ChildNodes.Count == 1)
                        {
                            return raiz.ChildNodes.ElementAt(0).Token.Text.ToLower();
                        }
                        break;
                    }
            }
            return null;
        }
        #endregion

        #region DIMENSIONES
        public Object getDimensiones(ParseTreeNode raiz)
        {
            String etiqueta = raiz.ToString();
            switch (etiqueta)
            {
                case "DIMENSIONES":
                    {
                        List<Expresion> expr = new List<Expresion>();
                        foreach (ParseTreeNode nodo in raiz.ChildNodes)
                        {
                            Expresion aux = (Expresion)getDimensiones(nodo);
                            if (aux != null)
                            {
                                expr.Add(aux);
                            }
                        }
                        Dimensiones dim = new Dimensiones(expr, 0, 0, this.clase);
                        dim.SetArchivoOrigen(this.archivo);
                        return dim;
                    }
                case "DIMENSION":
                    {
                        if (raiz.ChildNodes.Count == 1)
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
    }
}
