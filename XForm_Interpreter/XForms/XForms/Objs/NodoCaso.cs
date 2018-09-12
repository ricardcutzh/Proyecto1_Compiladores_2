using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.ASTTree.Interfaces;

namespace XForms.Objs
{
    class NodoCaso
    {
        Expresion expresion;

        List<Object> sentencias;

        public NodoCaso(Expresion exp, List<Object> sentencias)
        {
            this.expresion = exp;
            this.sentencias = sentencias;
        }


        public Expresion getExpresion()
        {
            return this.expresion;
        }

        public List<Object> getSentencias()
        {
            return this.sentencias;
        }
    }
}
