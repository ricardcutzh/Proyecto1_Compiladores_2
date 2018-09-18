using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
namespace XForms.StrParse
{
    class GramaticaStr:Grammar
    {
        public GramaticaStr():base(caseSensitive:false)
        {
            RegexBasedTerminal color = new RegexBasedTerminal("color", "#[0-9A-Fa-f]+");
            RegexBasedTerminal entero = new RegexBasedTerminal("ent", "[0-9]+");



            NonTerminal INICIO = new NonTerminal("INICIO");
            NonTerminal ESTILO = new NonTerminal("ESTILO");
            NonTerminal LISTA = new NonTerminal("LISTA");

            INICIO.Rule = "{" + LISTA + "}";

            LISTA.Rule = MakePlusRule(LISTA, ToTerm(","), ESTILO);

            ESTILO.Rule = ToTerm("negrilla")
                        | ToTerm("cursiva")
                        | ToTerm("subrayado")
                        | ToTerm("color") + ":" + color
                        | ToTerm("tam") + ":" + entero;

            this.Root = INICIO;

            this.MarkPunctuation(",", "@", ":", "}", "{");
        }
    }
}
