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
using XForms.ASTTree.Valores.Func_Cadenas;
using XForms.ASTTree.Valores.Func_Booleanas;
using XForms.ASTTree.Valores.Func_Numericas;
using XForms.ASTTree.Valores.Fun_Fechas;
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
                case "IF_SIMPLE":
                    {
                        if (raiz.ChildNodes.Count == 5)
                        {
                            ASTTreeExpresion arbolExpresion = new ASTTreeExpresion(raiz.ChildNodes.ElementAt(0), this.clase, this.archivo);
                            Expresion condicion = (Expresion)arbolExpresion.ConstruyeASTExpresion();

                            arbolExpresion = new ASTTreeExpresion(raiz.ChildNodes.ElementAt(2), this.clase, this.archivo);
                            Expresion verdadero = (Expresion)arbolExpresion.ConstruyeASTExpresion();

                            arbolExpresion = new ASTTreeExpresion(raiz.ChildNodes.ElementAt(4), this.clase, this.archivo);
                            Expresion falso = (Expresion)arbolExpresion.ConstruyeASTExpresion();

                            if (condicion != null && verdadero != null && falso != null)
                            {
                                int linea = raiz.ChildNodes.ElementAt(1).Token.Location.Line;
                                int col = raiz.ChildNodes.ElementAt(1).Token.Location.Column;
                                IfSimple simple = new IfSimple(condicion, verdadero, falso, this.clase, linea, col);
                                return simple;
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


        public Object traeLlamadas(ParseTreeNode raiz)
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
                            if(raiz.ChildNodes.ElementAt(1).ToString().Equals("L_EXPRE"))
                            {
                                String id = raiz.ChildNodes.ElementAt(0).Token.Text.ToLower();//MINUSCULAS
                                int linea = raiz.ChildNodes.ElementAt(0).Token.Location.Line;
                                int col = raiz.ChildNodes.ElementAt(0).Token.Location.Column;
                                Llamada llam = new Llamada(id, linea, col, this.clase);
                                llam.SetArchivoOrigen(archivo);
                                if (raiz.ChildNodes.ElementAt(1).ChildNodes.Count > 0)//SI EL HIJO DERECHO TIENE MAS DE UN HIJO ENTONCES LO RECORRO COMO EXP
                                {
                                    foreach (ParseTreeNode n in raiz.ChildNodes.ElementAt(1).ChildNodes)
                                    {
                                        Expresion aux = (Expresion)recorreExpresion(n);//LOS TOMO...
                                        if (aux != null)//SI NO ES NULL...
                                        {
                                            llam.AddExpresion(aux);//LO ANADO
                                        }
                                    }
                                }
                                return llam;
                            }
                            else if(raiz.ChildNodes.ElementAt(1).ToString().Equals("DIMS"))
                            {
                                //NECESITO HACER LA EXPRESION PARA ACCESO A ARREGLO
                                // id + DIMS
                                String id = raiz.ChildNodes.ElementAt(0).Token.Text.ToLower();
                                int linea = raiz.ChildNodes.ElementAt(0).Token.Location.Line;
                                int col = raiz.ChildNodes.ElementAt(0).Token.Location.Column;

                                List<Expresion> expresiones = (List<Expresion>)getDimensiones(raiz.ChildNodes.ElementAt(1));

                                if(expresiones!=null)
                                {
                                    ValorArreglo val = new ValorArreglo(id, expresiones, linea, col, clase);
                                    return val;
                                }
                            }    
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
                case "DECLARACION_ARR":
                    {
                        if(raiz.ChildNodes.Count == 3)
                        {
                            String tipo = (String)dameTipo(raiz.ChildNodes.ElementAt(1));//OBTENGO EL TIPO DEL ARREGLO
                            int linea = raiz.ChildNodes.ElementAt(0).Token.Location.Line;
                            int col = raiz.ChildNodes.ElementAt(0).Token.Location.Column;
                            List<Expresion> dimensiones = (List<Expresion>)getDimensiones(raiz.ChildNodes.ElementAt(2));//OBTENGO LAS DIMENSIONES

                            if(tipo!=null)
                            {
                                NuevoArreglo nuev = new NuevoArreglo(tipo, dimensiones, linea, col, clase);
                                return nuev;
                            }
                        }
                        break;
                    }
                case "OPCS_NUEVO":
                    {
                        if(raiz.ChildNodes.Count == 2)
                        {
                            int linea = raiz.ChildNodes.ElementAt(0).Token.Location.Line;
                            int colum = raiz.ChildNodes.ElementAt(0).Token.Location.Column;

                            NuevoListadoOpcs opcs = new NuevoListadoOpcs(this.clase.ToLower(), linea, colum);
                            return opcs;
                        }
                        break;
                    }
                case "FUN_CADENA":
                    {
                        if(raiz.ChildNodes.Count == 2)
                        {
                            int linea = raiz.ChildNodes.ElementAt(0).Token.Location.Line;
                            int col = raiz.ChildNodes.ElementAt(0).Token.Location.Column;

                            ASTTreeExpresion arbol = new ASTTreeExpresion(raiz.ChildNodes.ElementAt(1), clase, archivo);
                            Expresion exp = (Expresion)arbol.ConstruyeASTExpresion();

                            if(exp!=null)
                            {
                                ACadena cad = new ACadena(exp, clase, linea, col);
                                return cad;
                            }
                        }
                        break;
                    }
                case "FUN_SUBCAD":
                    {
                        if(raiz.ChildNodes.Count == 4)
                        {
                            int linea = raiz.ChildNodes.ElementAt(0).Token.Location.Line;
                            int col = raiz.ChildNodes.ElementAt(0).Token.Location.Column;

                            ASTTreeExpresion arbol = new ASTTreeExpresion(raiz.ChildNodes.ElementAt(1), clase, archivo);
                            Expresion cadena = (Expresion)arbol.ConstruyeASTExpresion();

                            arbol = new ASTTreeExpresion(raiz.ChildNodes.ElementAt(2), clase, archivo);
                            Expresion num1 = (Expresion)arbol.ConstruyeASTExpresion();

                            arbol = new ASTTreeExpresion(raiz.ChildNodes.ElementAt(3), clase, archivo);
                            Expresion num2 = (Expresion)arbol.ConstruyeASTExpresion();

                            if(cadena!=null && num1!=null && num2!=null)
                            {
                                SubCade s = new SubCade(cadena, num1, num2, linea, col, clase);
                                return s;
                            }
                        }
                        break;
                    }
                case "FUN_POSCAD":
                    {
                        if(raiz.ChildNodes.Count == 3)
                        {
                            int linea = raiz.ChildNodes.ElementAt(0).Token.Location.Line;
                            int col = raiz.ChildNodes.ElementAt(0).Token.Location.Column;

                            ASTTreeExpresion arbol = new ASTTreeExpresion(raiz.ChildNodes.ElementAt(1), clase, archivo);
                            Expresion cadena = (Expresion)arbol.ConstruyeASTExpresion();

                            arbol = new ASTTreeExpresion(raiz.ChildNodes.ElementAt(2), clase, archivo);
                            Expresion posicion = (Expresion)arbol.ConstruyeASTExpresion();

                            if(cadena!=null && posicion!=null)
                            {
                                PosCade posca = new PosCade(cadena, posicion, clase, linea, col);
                                return posca;
                            }
                        }
                        break;
                    }
                case "FUN_BOOLEAN":
                    {
                        if(raiz.ChildNodes.Count == 2)
                        {
                            int linea = raiz.ChildNodes.ElementAt(0).Token.Location.Line;
                            int col = raiz.ChildNodes.ElementAt(0).Token.Location.Column;

                            ASTTreeExpresion expr = new ASTTreeExpresion(raiz.ChildNodes.ElementAt(1), clase, archivo);
                            Expresion expresion = (Expresion)expr.ConstruyeASTExpresion();

                            if(expresion!=null)
                            {
                                return new ABooleano(expresion, clase, linea, col);
                            }
                        }
                        break;
                    }
                case "FUN_ENTERO":
                    {
                        if(raiz.ChildNodes.Count == 2)
                        {
                            int linea = raiz.ChildNodes.ElementAt(0).Token.Location.Line;
                            int col = raiz.ChildNodes.ElementAt(0).Token.Location.Column;

                            ASTTreeExpresion expr = new ASTTreeExpresion(raiz.ChildNodes.ElementAt(1), clase, archivo);
                            Expresion expresion = (Expresion)expr.ConstruyeASTExpresion();

                            if(expresion !=null)
                            {
                                return new AEntero(expresion, clase, linea, col);
                            }
                        }
                        break;
                    }
                case "FUN_TAM":
                    {
                        if(raiz.ChildNodes.Count == 2)
                        {
                            int linea = raiz.ChildNodes.ElementAt(0).Token.Location.Line;
                            int col = raiz.ChildNodes.ElementAt(0).Token.Location.Column;

                            ASTTreeExpresion expr = new ASTTreeExpresion(raiz.ChildNodes.ElementAt(1), clase, archivo);
                            Expresion expresion = (Expresion)expr.ConstruyeASTExpresion();

                            if (expresion != null)
                            {
                                FunTam f = new FunTam(expresion, clase, linea, col);
                                return f;
                            }
                        }
                        break;
                    }
                case "FUN_RANDOM":
                    {
                        if(raiz.ChildNodes.Count == 1)
                        {
                            int linea = raiz.ChildNodes.ElementAt(0).Token.Location.Line;
                            int col = raiz.ChildNodes.ElementAt(0).Token.Location.Column;

                            FunRandom r = new FunRandom(clase, linea, col);
                            return r;
                        }
                        else if(raiz.ChildNodes.Count == 2)
                        {
                            int linea = raiz.ChildNodes.ElementAt(0).Token.Location.Line;
                            int col = raiz.ChildNodes.ElementAt(0).Token.Location.Column;

                            List<Expresion> exp = new List<Expresion>();

                            foreach(ParseTreeNode nodo in raiz.ChildNodes.ElementAt(1).ChildNodes)
                            {
                                Expresion aux = (Expresion)recorreExpresion(nodo);
                                if(aux!=null)
                                {
                                    exp.Add(aux);
                                }
                            }

                            FunRandom r = new FunRandom(exp, clase, linea, col);
                            return r;

                        }
                        break;
                    }
                case "FUN_MIN":
                    {
                        if(raiz.ChildNodes.Count == 2)
                        {
                            int linea = raiz.ChildNodes.ElementAt(0).Token.Location.Line;
                            int col = raiz.ChildNodes.ElementAt(0).Token.Location.Column;
                            if(raiz.ChildNodes.ElementAt(1).ToString().Equals("L_EXPRE"))
                            {
                                List<Expresion> expresiones = new List<Expresion>();
                                foreach(ParseTreeNode nodo in raiz.ChildNodes.ElementAt(1).ChildNodes)
                                {
                                    Expresion exp = (Expresion)recorreExpresion(nodo);
                                    if(exp!=null)
                                    {
                                        expresiones.Add(exp);
                                    }
                                }

                                ///AQUI RETORNO
                                FunMin f = new FunMin(expresiones, clase, linea, col);
                                return f;
                            }
                            else
                            {
                                String idArreglo = raiz.ChildNodes.ElementAt(1).Token.Text.ToLower();
                                FunMin f = new FunMin(idArreglo.ToLower(), clase, linea, col);
                                return f;
                            }
                        }
                        break;
                    }
                case "FUN_MAX":
                    {
                        if(raiz.ChildNodes.Count == 2)
                        {
                            int linea = raiz.ChildNodes.ElementAt(0).Token.Location.Line;
                            int col = raiz.ChildNodes.ElementAt(0).Token.Location.Column;
                            if (raiz.ChildNodes.ElementAt(1).ToString().Equals("L_EXPRE"))
                            {
                                List<Expresion> expresiones = new List<Expresion>();
                                foreach (ParseTreeNode nodo in raiz.ChildNodes.ElementAt(1).ChildNodes)
                                {
                                    Expresion exp = (Expresion)recorreExpresion(nodo);
                                    if (exp != null)
                                    {
                                        expresiones.Add(exp);
                                    }
                                }

                                /// AQUI RETORNO
                                FunMax f = new FunMax(expresiones, clase, linea, col);
                                return f;
                            }
                            else
                            {
                                String idArreglo = raiz.ChildNodes.ElementAt(1).Token.Text.ToLower();
                                FunMax f = new FunMax(idArreglo.ToLower(), clase, linea, col);
                                return f;
                            }
                        }
                        break;
                    }
                case "FUN_POW":
                    {
                        if(raiz.ChildNodes.Count == 3)
                        {
                            int linea = raiz.ChildNodes.ElementAt(0).Token.Location.Line;
                            int col = raiz.ChildNodes.ElementAt(0).Token.Location.Column;

                            Expresion bas = (Expresion)recorreExpresion(raiz.ChildNodes.ElementAt(1));
                            Expresion pot = (Expresion)recorreExpresion(raiz.ChildNodes.ElementAt(2));

                            if(bas!=null && pot!=null)
                            {
                                FunPow po = new FunPow(bas, pot, clase, linea, col);
                                return po;
                            }
                        }
                        break;
                    }
                case "FUN_LOG":
                    {
                        if(raiz.ChildNodes.Count == 2)
                        {
                            int linea = raiz.ChildNodes.ElementAt(0).Token.Location.Line;
                            int col = raiz.ChildNodes.ElementAt(0).Token.Location.Column;

                            Expresion exp = (Expresion)recorreExpresion(raiz.ChildNodes.ElementAt(1));

                            if(exp!=null)
                            {
                                Logaritmo l = new Logaritmo(exp, clase, linea, col);
                                return l;
                            }
                        }
                        break;
                    }
                case "FUN_LOG10":
                    {
                        if(raiz.ChildNodes.Count == 2)
                        {
                            int linea = raiz.ChildNodes.ElementAt(0).Token.Location.Line;
                            int col = raiz.ChildNodes.ElementAt(0).Token.Location.Column;

                            Expresion exp = (Expresion)recorreExpresion(raiz.ChildNodes.ElementAt(1));

                            if(exp!=null)
                            {
                                LogTen l = new LogTen(exp, clase, linea, col);
                                return l;
                            }
                        }
                        break;
                    }
                case "FUN_ABS":
                    {
                        if(raiz.ChildNodes.Count == 2)
                        {
                            int linea = raiz.ChildNodes.ElementAt(0).Token.Location.Line;
                            int col = raiz.ChildNodes.ElementAt(0).Token.Location.Column;

                            Expresion exp = (Expresion)recorreExpresion(raiz.ChildNodes.ElementAt(1));

                            if(exp!=null)
                            {
                                FunAbs abs = new FunAbs(exp, clase, linea, col);
                                return abs;
                            }
                        }
                        break;
                    }
                case "FUN_SIN":
                    {
                        if(raiz.ChildNodes.Count == 2)
                        {
                            int linea = raiz.ChildNodes.ElementAt(0).Token.Location.Line;
                            int col = raiz.ChildNodes.ElementAt(0).Token.Location.Column;

                            Expresion exp = (Expresion)recorreExpresion(raiz.ChildNodes.ElementAt(1));

                            if(exp!=null)
                            {
                                Seno s = new Seno(exp, clase, linea, col);
                                return s;
                            }
                        }
                        break;
                    }
                case "FUN_COS":
                    {
                        if(raiz.ChildNodes.Count == 2)
                        {
                            int linea = raiz.ChildNodes.ElementAt(0).Token.Location.Line;
                            int col = raiz.ChildNodes.ElementAt(0).Token.Location.Column;

                            Expresion exp = (Expresion)recorreExpresion(raiz.ChildNodes.ElementAt(1));

                            if(exp!=null)
                            {
                                Coseno c = new Coseno(exp, clase, linea, col);
                                return c;
                            }
                        }
                        break;
                    }
                case "FUN_TAN":
                    {
                        if(raiz.ChildNodes.Count == 2)
                        {
                            int linea = raiz.ChildNodes.ElementAt(0).Token.Location.Line;
                            int col = raiz.ChildNodes.ElementAt(0).Token.Location.Column;

                            Expresion exp = (Expresion)recorreExpresion(raiz.ChildNodes.ElementAt(1));

                            if(exp!=null)
                            {
                                Tangente t = new Tangente(exp, clase, linea, col);
                                return t;
                            }
                        }
                        break;
                    }
                case "FUN_SQRT":
                    {
                        if(raiz.ChildNodes.Count == 2)
                        {
                            int linea = raiz.ChildNodes.ElementAt(0).Token.Location.Line;
                            int col = raiz.ChildNodes.ElementAt(0).Token.Location.Column;

                            Expresion exp = (Expresion)recorreExpresion(raiz.ChildNodes.ElementAt(1));

                            if(exp!=null)
                            {
                                Raiz r = new Raiz(exp, clase, linea, col);
                                return r;
                            }
                        }
                        break;
                    }
                case "FUN_PI":
                    {
                        if(raiz.ChildNodes.Count == 1)
                        {
                            int linea = raiz.ChildNodes.ElementAt(0).Token.Location.Line;
                            int col = raiz.ChildNodes.ElementAt(0).Token.Location.Column;
                            Pi p = new Pi(clase, linea, col);
                            return p;
                        }
                        break;
                    }
                case "FUN_HOY":
                    {
                        if(raiz.ChildNodes.Count == 1)
                        {
                            int linea = raiz.ChildNodes.ElementAt(0).Token.Location.Line;
                            int col = raiz.ChildNodes.ElementAt(0).Token.Location.Column;

                            FunHoy hoy = new FunHoy(clase, linea, col);
                            return hoy;
                        }
                        break;
                    }
                case "FUN_AHORA":
                    {
                        if(raiz.ChildNodes.Count == 1)
                        {
                            int linea = raiz.ChildNodes.ElementAt(0).Token.Location.Line;
                            int col = raiz.ChildNodes.ElementAt(0).Token.Location.Column;

                            FunAhora ahora = new FunAhora(clase, linea, col);
                            return ahora;
                        }
                        break;
                    }
                case "FUN_AFECHA":
                    {
                        if(raiz.ChildNodes.Count == 2)
                        {
                            int linea = raiz.ChildNodes.ElementAt(0).Token.Location.Line;
                            int col = raiz.ChildNodes.ElementAt(0).Token.Location.Column;

                            Expresion exp = (Expresion)recorreExpresion(raiz.ChildNodes.ElementAt(1));

                            if (exp != null)
                            {
                                FunAFecha r = new FunAFecha(exp, clase, linea, col);
                                return r;
                            }
                        }
                        break;
                    }
                case "FUN_TOHORA":
                    {
                        if(raiz.ChildNodes.Count == 2)
                        {
                            int linea = raiz.ChildNodes.ElementAt(0).Token.Location.Line;
                            int col = raiz.ChildNodes.ElementAt(0).Token.Location.Column;

                            Expresion exp = (Expresion)recorreExpresion(raiz.ChildNodes.ElementAt(1));

                            if(exp!=null)
                            {
                                FunToHora f = new FunToHora(exp, clase, linea, col);
                                return f;
                            }
                        }
                        break;
                    }
                case "FUN_TOFECHAHORA":
                    {
                        if(raiz.ChildNodes.Count == 2)
                        {
                            int linea = raiz.ChildNodes.ElementAt(0).Token.Location.Line;
                            int col = raiz.ChildNodes.ElementAt(0).Token.Location.Column;

                            Expresion exp = (Expresion)recorreExpresion(raiz.ChildNodes.ElementAt(1));

                            if (exp != null)
                            {
                                FunFechaHora f = new FunFechaHora(exp, clase, linea, col);
                                return f;
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
                case "DIMS":
                    {
                        List<Expresion> dimensiones = new List<Expresion>();
                        foreach(ParseTreeNode nodo in raiz.ChildNodes)
                        {
                            Expresion exp = (Expresion)getDimensiones(nodo);
                            if(exp!=null)
                            {
                                dimensiones.Add(exp);
                            }
                        }
                        return dimensiones;
                    }
                case "REALDIM":
                    {
                        if(raiz.ChildNodes.Count == 1)
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
    }
}
