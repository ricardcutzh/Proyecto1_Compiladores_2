using XForms.ASTTree.Interfaces;
using XForms.Simbolos;

namespace XForms.ASTTree.Valores
{
    class ValorPrimitivo : NodoAST, Expresion
    {

        object valor;

        public ValorPrimitivo(object valor, int linea, int columna, string clase):base(linea, columna, clase)
        {
            this.valor = valor;
        }

        public string getTipo(Ambito ambito)
        {
            throw new System.NotImplementedException();
        }

        public object getValor(Ambito ambito)
        {
            throw new System.NotImplementedException();
        }
    }
}
