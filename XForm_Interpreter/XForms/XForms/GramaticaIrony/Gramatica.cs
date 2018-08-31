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
        public Gramatica() : base(caseSensitive: false)
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
            var importar = ToTerm("importar");

            //------> OPERADORES
            var and = ToTerm("&&");
            var or = ToTerm("||");
            var not = ToTerm("!");
            var equal = ToTerm("==");
            var nequal = ToTerm("!=");
            var gequal = ToTerm(">=");
            var lequal = ToTerm("<=");
            var grater = ToTerm(">");
            var lower = ToTerm("<");
            var sum = ToTerm("+");
            var res = ToTerm("-");
            var mult = ToTerm("*");
            var divi = ToTerm("/");
            var pot = ToTerm("^");
            var mod = ToTerm("%");
            var inc = ToTerm("++");
            var dec = ToTerm("--");
            #endregion

            #region NoTerminales
            //REGION DE LOS NO TERMINALES
            NonTerminal INICIO = new NonTerminal("INICIO"),
            IMPORTACIONES = new NonTerminal("IMPORTACIONES"),
            IMPORTA = new NonTerminal("IMPORTA"),
            CLASES = new NonTerminal("CLASES"),
            CLASE = new NonTerminal("CLASE"),
            VISIBILIDAD = new NonTerminal("VISIBILIDAD"),
            CUERPOCLASE = new NonTerminal("CUERPOCLASE"),
            PRINCIPAL = new NonTerminal("PRINCIPAL"),
            FUNCIONES = new NonTerminal("FUNCIONES"),
            TIPO = new NonTerminal("TIPO"),
            PARAMETROS = new NonTerminal("PARAMETROS"),
            PARAMETRO = new NonTerminal("PARAMETRO"),
            CONSTRUCTOR = new NonTerminal("CONSTRUCTOR"),
            EXP = new NonTerminal("EXP"),
            E = new NonTerminal("E"),
            PREGUNTA = new NonTerminal("PREGUNTA"),
            GRUPO = new NonTerminal("GRUPO"),
            FORMULARIO = new NonTerminal("FORMULARIO"),
            DECLARACION_GLOBAL = new NonTerminal("DECLARACION_GLOBAL"),
            DECLARACION_LOCAL = new NonTerminal("DECLARACION_LOCAL");
            #endregion

            #region Reglas
            //AQUI VA LA GRAMATICA DE XFORM

            //-----> INICIO DE GRAMATICA

            //-------------------------------------------------------------------------------------------
            INICIO.Rule = IMPORTACIONES + CLASES
                        | CLASES;
            //-------------------------------------------------------------------------------------------

            //-------------------------------------------------------------------------------------------
            IMPORTACIONES.Rule = MakeStarRule(IMPORTACIONES, IMPORTA); //YA QUE PUEDEN O NO VENIR IMPORTACIONES

            IMPORTA.Rule = importar + "(" + identificador + ".xform" + ")" + ";"; // 2 HIJOS

            IMPORTA.ErrorRule = SyntaxError + ";";
            //-------------------------------------------------------------------------------------------

            //-------------------------------------------------------------------------------------------
            CLASES.Rule = MakePlusRule(CLASES, CLASE); //YA QUE AL MENOS UNA CLASSE VA A VENIR EN LA ENTRADA
            //-------------------------------------------------------------------------------------------

            //-------------------------------------------------------------------------------------------
            CLASE.Rule = "clase" + identificador + VISIBILIDAD + "padre" + identificador + "{" + CUERPOCLASE + "}"// 6 HIJOS
                        | "clase" + identificador + "padre" + identificador + "{" + CUERPOCLASE + "}" // 5 HIJOS
                        | "clase" + identificador + VISIBILIDAD + "{" + CUERPOCLASE + "}" // 4 HIJOS
                        | "clase" + identificador + "{" + CUERPOCLASE + "}"; //3 HIJOS

            CLASE.ErrorRule = SyntaxError + "}";
            //-------------------------------------------------------------------------------------------

            //-------------------------------------------------------------------------------------------
            VISIBILIDAD.Rule = privado
                             | publico
                             | protegido;
            //-------------------------------------------------------------------------------------------

            //-------------------------------------------------------------------------------------------
            TIPO.Rule = ToTerm("cadena")
                       | ToTerm("booleano")
                       | ToTerm("entero")
                       | ToTerm("decimal")
                       | ToTerm("hora")
                       | ToTerm("fecha")
                       | ToTerm("fechahora")
                       | ToTerm("respuestas")
                       | identificador //EN CASO QUE SEA UN TIPO DE OBJETO
                       | ToTerm("vacio");
            //-------------------------------------------------------------------------------------------

            //-------------------------------------------------------------------------------------------
            CUERPOCLASE.Rule = MakeStarRule(CUERPOCLASE, DECLARACION_GLOBAL)
                             | MakeStarRule(CUERPOCLASE, PRINCIPAL)
                             | MakeStarRule(CUERPOCLASE, FUNCIONES)
                             | MakeStarRule(CUERPOCLASE, CONSTRUCTOR)
                             | MakeStarRule(CUERPOCLASE, PREGUNTA)
                             | MakeStarRule(CUERPOCLASE, GRUPO)
                             | MakeStarRule(CUERPOCLASE, FORMULARIO);
            //-------------------------------------------------------------------------------------------

            //-------------------------------------------------------------------------------------------
            FUNCIONES.Rule = VISIBILIDAD + TIPO + identificador + "(" + PARAMETROS + ")" + "{" + "}"
                            | TIPO + identificador + "(" + PARAMETROS + ")" + "{" + "}";

            FUNCIONES.ErrorRule = SyntaxError + "}";

            PARAMETROS.Rule = MakeStarRule(PARAMETROS, ToTerm(","), PARAMETRO);

            PARAMETRO.Rule = TIPO + identificador;
            //-------------------------------------------------------------------------------------------

            //-------------------------------------------------------------------------------------------
            PRINCIPAL.Rule = ToTerm("principal") + "(" + ")" + "{" + "}";
            PRINCIPAL.ErrorRule = SyntaxError + "}";
            //-------------------------------------------------------------------------------------------

            //-------------------------------------------------------------------------------------------
            CONSTRUCTOR.Rule = identificador + "(" + PARAMETROS + ")" + "{" + "}";

            CONSTRUCTOR.ErrorRule = SyntaxError + "}";
            //-------------------------------------------------------------------------------------------

            //-------------------------------------------------------------------------------------------
            PREGUNTA.Rule = ToTerm("pregunta") + identificador + "(" + PARAMETROS + ")" + "{" + "}";
            PREGUNTA.ErrorRule = SyntaxError + "}";
            //-------------------------------------------------------------------------------------------

            //-------------------------------------------------------------------------------------------
            GRUPO.Rule = ToTerm("grupo") + identificador + "{" + "}";
            GRUPO.ErrorRule = SyntaxError + "}";
            //-------------------------------------------------------------------------------------------

            //-------------------------------------------------------------------------------------------
            FORMULARIO.Rule = ToTerm("formulario") + identificador + "{" + "}";
            FORMULARIO.ErrorRule = SyntaxError + "}";
            //-------------------------------------------------------------------------------------------

            //-------------------------------------------------------------------------------------------
            DECLARACION_GLOBAL.Rule = TIPO + VISIBILIDAD + identificador + "=" + EXP + ";"
                                   | TIPO + VISIBILIDAD + identificador + ";"
                                   | DECLARACION_LOCAL;

            DECLARACION_GLOBAL.ErrorRule = SyntaxError + ";";

            DECLARACION_LOCAL.Rule = TIPO + identificador + "=" + EXP + ";"
                                   | TIPO + identificador + ";";

            DECLARACION_LOCAL.ErrorRule = SyntaxError + ";";
            //-------------------------------------------------------------------------------------------

            //-------------------------------------------------------------------------------------------
            EXP.Rule = E;
            E.Rule =   E + and + E
                     | E + or  + E
                     | not + E
                     | E + equal + E
                     | E + nequal + E
                     | E + gequal + E
                     | E + lequal + E
                     | E + grater + E
                     | E + lower + E
                     | E + sum + E
                     | E + res + E
                     | E + mult + E
                     | E + divi + E
                     | E + pot  + E
                     | E + mod + E
                     | E + inc
                     | E + dec
                     | res + E
                     | ToTerm("(") + E + ToTerm(")") 
                     | cadena
                     | cadena2
                     | entero
                     | deci
                     | verdadero
                     | falso
                     | identificador
                     ;
            //-------------------------------------------------------------------------------------------
            #endregion

            #region Preferencias
            RegisterOperators(1, Associativity.Left, or);
            RegisterOperators(2, Associativity.Left, and);
            RegisterOperators(3, Associativity.Left, nequal, equal);
            RegisterOperators(4, Associativity.Left, grater, lower, gequal, lequal);
            RegisterOperators(5, Associativity.Left, sum, res);
            RegisterOperators(6, Associativity.Left, divi, mult, mod, pot);
            RegisterOperators(7, Associativity.Right, inc, dec, not, res);
            RegisterOperators(8, Associativity.Left, ToTerm("("));

            this.MarkPunctuation("{", "}", "(", ")", ";",".xform","{","}");
            NonGrammarTerminals.Add(LineComment);
            NonGrammarTerminals.Add(MultiLineComment);
            this.Root = INICIO;
            #endregion
        }
        #endregion

        
    }
}
