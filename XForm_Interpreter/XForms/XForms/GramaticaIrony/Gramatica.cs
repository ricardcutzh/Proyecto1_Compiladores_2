using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;
using Irony.Interpreter;

namespace XForms.GramaticaIrony
{
    class Gramatica : Grammar
    {
        //GRAMATICA PARA XFORM

        #region Principal
        public Gramatica():base(caseSensitive:false)
        {
            #region EXREG
            //REGIONES DE EXPRESIONES REGULARES
            // --> COMENTARIOS
            CommentTerminal LineComment = new CommentTerminal("comentarioLinea", "$$", "\n");
            CommentTerminal MultiLineComment = new CommentTerminal("multilinea", "$#", "#$");

            //EXPRESIONES REGULARES

            //----->TIPOS DE DATOS
            StringLiteral cadena = new StringLiteral("cadena", "\"");
            StringLiteral cadena2 = new StringLiteral("cadena2", "\'");
            RegexBasedTerminal verdadero = new RegexBasedTerminal("verdadero", "verdadero");
            RegexBasedTerminal falso = new RegexBasedTerminal("falso", "falso");
            RegexBasedTerminal entero = new RegexBasedTerminal("entero", "[0-9]+");
            RegexBasedTerminal deci = new RegexBasedTerminal("decimal", "[0-9]+\\.+[0-9]+");

            //----->OTROS
            IdentifierTerminal identificador = new IdentifierTerminal("identificador");
            #endregion

            #region Terminales

            //REGION DE TERMINALES

            //----->RESERVADAS
            var privado = ToTerm("privado");
            var publico = ToTerm("publico");
            var protegido = ToTerm("protegido");

            #endregion

            #region NoTerminales
            //REGION DE LOS NO TERMINALES
            NonTerminal INICIO = new NonTerminal("INICIO"),
            IMPORTACIONES = new NonTerminal("IMPORTACIONES"),
            CLASES = new NonTerminal("CLASES"),
            CLASE = new NonTerminal("CLASE"),
            VISIBILIDAD = new NonTerminal("VISIBILIDAD"),
            CUERPOCLASE = new NonTerminal("CUERPOCLASE"),
            PRINCIPAL = new NonTerminal("PRINCIPAL");
            #endregion

            #region Reglas
            //AQUI VA LA GRAMATICA DE XFORM

            //-----> INICIO DE GRAMATICA

            //-------------------------------------------------------------------------------------------
            INICIO.Rule = IMPORTACIONES + CLASES
                        | CLASES;
            //-------------------------------------------------------------------------------------------

            //-------------------------------------------------------------------------------------------
            IMPORTACIONES.Rule = IMPORTACIONES + "importar" + "(" + identificador + ".xform" + ")" + ";"
                              | "importar" + "(" + identificador + ".xform" + ")" + ";";

            IMPORTACIONES.ErrorRule = SyntaxError + ";";
            //-------------------------------------------------------------------------------------------

            //-------------------------------------------------------------------------------------------
            CLASES.Rule = CLASES + CLASE
                        | CLASE;
            //-------------------------------------------------------------------------------------------

            //-------------------------------------------------------------------------------------------
            CLASE.Rule = "clase" + identificador + VISIBILIDAD + "padre" + identificador + "{" + "}"
                        | "clase" + identificador + "padre" + identificador + "{" + "}"
                        | "clase" + identificador + VISIBILIDAD + "{" + "}"
                        | "clase" + identificador + "{" + "}"
                        //--------------------------------------------------------------------------------
                        | "clase" + identificador + VISIBILIDAD + "padre" + identificador + "{" + CUERPOCLASE + "}"
                        | "clase" + identificador + "padre" + identificador + "{" + CUERPOCLASE + "}"
                        | "clase" + identificador + VISIBILIDAD + "{" + CUERPOCLASE + "}"
                        | "clase" + identificador + "{" + CUERPOCLASE + "}";

            CLASE.ErrorRule = SyntaxError + "}";
            //-------------------------------------------------------------------------------------------

            //-------------------------------------------------------------------------------------------
            VISIBILIDAD.Rule = privado
                             | publico
                             | protegido;
            //-------------------------------------------------------------------------------------------

            //-------------------------------------------------------------------------------------------
            CUERPOCLASE.Rule = CUERPOCLASE + PRINCIPAL
                            | PRINCIPAL;

            PRINCIPAL.Rule = "principal" + "(" + ")" + "{" + Empty + "}";
            PRINCIPAL.ErrorRule = SyntaxError + "}";
            //-------------------------------------------------------------------------------------------
            #endregion

            #region Preferencias
            this.MarkPunctuation("{", "}", "(", ")", ";",".xform","{","}");
            NonGrammarTerminals.Add(LineComment);
            NonGrammarTerminals.Add(MultiLineComment);
            this.Root = INICIO;
            #endregion
        }
        #endregion

        
    }
}
