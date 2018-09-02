using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.ASTTree.Interfaces;
using XForms.Simbolos;

namespace XForms.ASTTree.Valores
{
    class Llamada :NodoAST, Expresion
    {
        String id;
        List<Expresion> expresiones;

        public Llamada(String id,int linea, int col, String clase):base(linea, col, clase)
        {
            this.id = id;
            this.expresiones = new List<Expresion>();
        }

        public void AddExpresion(Expresion exp)
        {
            this.expresiones.Add(exp);
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
