using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace XForms.StrParse
{
    class StrAnalizador
    {
        String cadenaAnalizar;

        public StrAnalizador(String cadena)
        {
            this.cadenaAnalizar = cadena;
        }

        public ParseTreeNode getRoot()
        {
            try
            {
                GramaticaStr str = new GramaticaStr();
                LanguageData l = new LanguageData(str);
                Parser p = new Parser(l);
                ParseTree arbol = p.Parse(this.cadenaAnalizar);
                if(arbol.Root!=null)
                {
                    return arbol.Root;
                }
                return null;
            }
            catch
            {

            }
            return null;
        }
    }
}
