using XForms.ASTTree.Interfaces;
using XForms.Simbolos;
using XForms.Objs;

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
            if(this.valor is bool)
            {
                return "Booleano";
            }
            else if(this.valor is string)
            {
                return "Cadena";
            }
            else if(this.valor is int)
            {
                return "Entero";
            }
            else if(this.valor is double)
            {
                return "Decimal";
            }
            else if(this.valor is System.DateTime)
            {
                return "FechaHora";
            }
            else if(this.valor is Date)
            {
                return "Fecha";
            }
            else if(this.valor is Hour)
            {
                return "Hora";
            }
            else if(this.valor is Nulo)
            {
                return "Nulo";
            }
            else if(this.valor is Objeto)
            {
                Objeto aux = (Objeto)this.valor;
                return aux.idClase;
            }
            //AQUI FALTA EL TIPO OBJETO
            return "Objeto";
        }

        public object getValor(Ambito ambito)
        {
            return this.valor;
        }
    }
}
