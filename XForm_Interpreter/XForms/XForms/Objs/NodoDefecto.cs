using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.ASTTree.Interfaces;
namespace XForms.Objs
{
    class NodoDefecto
    {

        List<Object> sentencias;

        public NodoDefecto(List<Object> sentencias)
        {
            this.sentencias = sentencias;
        }


        public List<Object> getSentencias()
        {
            return sentencias;
        }
    }
}
