using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.ASTTree.Interfaces;

namespace XForms.Objs
{
    class NodoEstiloRespuesta
    {
        public String tipo;
        public List<Expresion> parametros;

        public int linea, col;

        public NodoEstiloRespuesta(String tipo, int linea, int col)
        {
            this.tipo = tipo;
            this.parametros = new List<Expresion>();
            this.linea = linea;
            this.col = col;
        }

        public void addParametro(Expresion exp)
        {
            this.parametros.Add(exp);
        }
    }
}
