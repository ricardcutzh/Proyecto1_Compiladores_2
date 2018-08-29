using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
using Irony.Interpreter;
using Irony.Ast;
using System.Windows.Forms;

namespace XForms.GramaticaIrony
{
    class Analizador
    {
        public List<TError> errores { get; set; }//LISTA DE LOS ERRORES EN IRONY
        public ParseTreeNode raiz { get; set; } //RAIZ DEL ARBOL
        String cadena;

        public Analizador(String cadena)
        {
            this.cadena = cadena;
            this.errores = new List<TError>();
        }

        public bool analizar()
        {
            try
            {
                Gramatica gramatica = new Gramatica();
                LanguageData languaje = new LanguageData(gramatica);
                Parser p = new Parser(languaje);
                ParseTree arbol = p.Parse(this.cadena);
                this.raiz = arbol.Root;
                obteErroes(arbol);
                if (raiz == null)
                {
                    
                    return false;
                }
                else
                {
                    ASTGraph g = new ASTGraph();
                    g.graficarAST(raiz);
                    return true;
                }
            }
            catch(Exception e)
            {
                return false;
            }
        }

        private void obteErroes(ParseTree raiz)
        {
            for(int x = 0; x < raiz.ParserMessages.Count(); x++)
            {
                String men = "Mensaje: " + raiz.ParserMessages.ElementAt(x).Message + " | Linea: " + raiz.ParserMessages.ElementAt(x).Location.Line + " | Columna: " + raiz.ParserMessages.ElementAt(x).Location.Column + " | Esperaba: ";
                for(int y = 0; y < raiz.ParserMessages.ElementAt(x).ParserState.ExpectedTerminals.Count(); y ++)
                {
                    men += "* -" + raiz.ParserMessages.ElementAt(x).ParserState.ExpectedTerminals.ElementAt(y).ErrorAlias + "\n";
                }
                MessageBox.Show(men);
            }
        }

    }
}
