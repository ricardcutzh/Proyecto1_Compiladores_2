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
                        if(raiz.ChildNodes.Count == 3)
                        {
                            Object operando1 = recorreExpresion(raiz.ChildNodes.ElementAt(0));
                            Object operando2 = recorreExpresion(raiz.ChildNodes.ElementAt(2));
                            Estatico.Operandos oper = (Estatico.Operandos)dameOperador(raiz.ChildNodes.ElementAt(1));
                            int linea = raiz.ChildNodes.ElementAt(1).Token.Location.Line;
                            int colum = raiz.ChildNodes.ElementAt(1).Token.Location.Column;
                            return new Operacion(operando1, operando2, oper, linea, colum, this.clase);
                        }
                        if(raiz.ChildNodes.Count == 2)
                        {
                            if(raiz.ChildNodes.ElementAt(0).ToString().Equals("E"))
                            {
                                Object operando = recorreExpresion(raiz.ChildNodes.ElementAt(0));
                                Estatico.Operandos oper = (Estatico.Operandos)dameOperador(raiz.ChildNodes.ElementAt(1));
                                int linea = raiz.ChildNodes.ElementAt(1).Token.Location.Line;
                                int colum = raiz.ChildNodes.ElementAt(1).Token.Location.Column;
                                return new Operacion(operando, oper, linea, colum, this.clase);
                            }
                            else
                            {
                                Object operando = recorreExpresion(raiz.ChildNodes.ElementAt(1));
                                Estatico.Operandos oper = (Estatico.Operandos)dameOperador(raiz.ChildNodes.ElementAt(0));
                                int linea = raiz.ChildNodes.ElementAt(1).Token.Location.Line;
                                int colum = raiz.ChildNodes.ElementAt(1).Token.Location.Column;
                                return new Operacion(operando, oper, linea, colum, this.clase);
                            }
                        }
                        if(raiz.ChildNodes.Count == 1)
                        {
                            return recorreExpresion(raiz.ChildNodes.ElementAt(0));
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
    }
}
