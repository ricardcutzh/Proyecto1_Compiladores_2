using System;
using XForms.ASTTree.Interfaces;
using XForms.Simbolos;
using XForms.Objs;

namespace XForms.ASTTree.Valores
{
    class Operacion : NodoAST, Expresion
    {

        Object operando1;
        Object operando2;
        Estatico.Operandos operador;
        Boolean unoSolo;

        //OPERACIONES CON DOS EXPRESIONES
        public Operacion(Object operando1, Object operando2, Estatico.Operandos operador, int linea, int columna, String clase):base(linea,columna, clase)
        {
            this.operando1 = operando1;
            this.operando2 = operando2;
            this.operador = operador;
            unoSolo = false;
        }

        /*OPERANDOS CON UNA SOLA EXPRESION*/
        public Operacion(Object operando1, Estatico.Operandos operador, int linea, int columna, String clase):base(linea, columna, clase)
        {
            this.operando1 = operando1;
            this.operador = operador;
            this.unoSolo = true;
        }

        public string getTipo(Ambito ambito)
        {
            throw new NotImplementedException();
        }

        public object getValor(Ambito ambito)
        {
            throw new NotImplementedException();
        }
    }
}
