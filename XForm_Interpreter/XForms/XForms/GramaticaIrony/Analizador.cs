using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
using Irony.Interpreter;
using Irony.Ast;
using System.Windows.Forms;
using XForms.Objs; 
namespace XForms.GramaticaIrony
{
    class Analizador
    {
        public List<TError> errores { get; set; }//LISTA DE LOS ERRORES EN IRONY
        public ParseTreeNode raiz { get; set; } //RAIZ DEL ARBOL
        String cadena;
        String archivo;
        String pathProyecto;
        public List<ClasePreAnalizada> clases { get; set; }

        public Analizador(String cadena)
        {
            this.cadena = cadena;
            this.errores = new List<TError>();
            this.clases = new List<ClasePreAnalizada>();
        }

        public Analizador(String cadena, String PathProyecto, String archivo)
        {
            this.cadena = cadena;
            this.archivo = archivo;
            this.pathProyecto = PathProyecto;
            this.clases = new List<ClasePreAnalizada>();
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
                    Recorrido r = new Recorrido(this.raiz, this.archivo, this.pathProyecto);
                    r.iniciaRecorrido();
                    //OBTENGO LAS CLASES DEL ANALIZADOR
                    this.clases = r.clases;
                    return true;
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
        }

        private void obteErroes(ParseTree raiz)
        {
            for(int x = 0; x < raiz.ParserMessages.Count(); x++)
            {
                String mensajeEsperados = "";
                for (int y = 0; y < raiz.ParserMessages.ElementAt(x).ParserState.ExpectedTerminals.Count(); y++)
                {
                    mensajeEsperados += " Simbolo: \""+(String)raiz.ParserMessages.ElementAt(x).ParserState.ExpectedTerminals.ElementAt(y).ErrorAlias+"\"\n";
                }
                TError error = new TError("Sintatico", "Error Sintactico"+ " En Archivo: " + this.archivo+" | Se esperaba: \n"+mensajeEsperados, raiz.ParserMessages.ElementAt(x).Location.Line, raiz.ParserMessages.ElementAt(x).Location.Column);
                Estatico.errores.Add(error);
            }
        }

    }
}
