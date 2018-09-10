﻿using System;
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
            var nulo = ToTerm("nulo");

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
            DECLARACION_LOCAL = new NonTerminal("DECLARACION_LOCAL"),
            LLAMADAID_OBJ = new NonTerminal("LLAMADAID_OBJ"),
            LLAMADA = new NonTerminal("LLAMADA"),
            L_EXPRE = new NonTerminal("L_EXPRE"),
            DECLARACION_OBJ = new NonTerminal("DECLARACION_OBJ"),
            SENTENCIAS = new NonTerminal("SENTENCIAS"),
            SENTENCIAS_CONS = new NonTerminal("SENTENCIAS_CONS"),
            L_DIMPARAM = new NonTerminal("L_DIMPARAM"),
            DIM_DEF = new NonTerminal("DIM_DEF"),
            IMPRIMIR = new NonTerminal("IMPRIMIR"),
            ASIGNACION = new NonTerminal("ASIGNACION"),
            RETORNO = new NonTerminal("RETORNO"),
            EMPTYDIM = new NonTerminal("EMPTYDIM"),
            REALDIM = new NonTerminal("REALDIM"),
            DIMS = new NonTerminal("DIMS"),
            ARRAYDEF = new NonTerminal("ARRAYDEF"),
            ARRELEMENTS = new NonTerminal("ARRELEMENTS"),
            AUXDIMS = new NonTerminal("AUXDIMS"),
            DECLARACION_ARR = new NonTerminal("DECLARACION_ARR"),
            ACCESOARRAY = new NonTerminal("ACCESOARRAY"),
            CONDICIONAL_SI = new NonTerminal("CONDICIONAL_SI"),
            SINO = new NonTerminal("SINO"),
            LISTA_SINO = new NonTerminal("LISTA_SINO"),
            IF_SIMPLE = new NonTerminal("IF_SIMPLE"),
            MIENTRAS = new NonTerminal("MIENTRAS"),
            ROMPER = new NonTerminal("ROMPER"),
            CONTINUAR = new NonTerminal("CONTINUAR"),
            LLAMADAFUNCION = new NonTerminal("LLAMADAFUN"),
            HACERMIENTRAS = new NonTerminal("HACERMIENTRAS"),
            REPETIRHASTA = new NonTerminal("REPETIRHASTA"),
            INCREMENTO = new NonTerminal("INCREMENTO"),
            DECREMENTO = new NonTerminal("DECREMENTO"),
            FOR = new NonTerminal("FOR"),
            VARCONTROL = new NonTerminal("VARCONTROL"),
            OPERACION = new NonTerminal("OPERACION");
            #endregion

            #region Reglas
            //AQUI VA LA GRAMATICA DE XFORM

            //-----> INICIO DE GRAMATICA

            #region INIT
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
                             | MakeStarRule(CUERPOCLASE, DECLARACION_LOCAL)
                             | MakeStarRule(CUERPOCLASE, PRINCIPAL)
                             | MakeStarRule(CUERPOCLASE, FUNCIONES)
                             | MakeStarRule(CUERPOCLASE, CONSTRUCTOR)
                             | MakeStarRule(CUERPOCLASE, PREGUNTA)
                             | MakeStarRule(CUERPOCLASE, GRUPO)
                             | MakeStarRule(CUERPOCLASE, FORMULARIO);
            //-------------------------------------------------------------------------------------------
            #endregion

            #region FUNCTIONS
            //-------------------------------------------------------------------------------------------
            FUNCIONES.Rule = VISIBILIDAD + TIPO + identificador + "(" + PARAMETROS + ")" + "{" + SENTENCIAS + "}"
                            | TIPO + identificador + "(" + PARAMETROS + ")" + "{" + SENTENCIAS + "}";

            FUNCIONES.ErrorRule = SyntaxError + "}";

            PARAMETROS.Rule = MakeStarRule(PARAMETROS, ToTerm(","), PARAMETRO);

            PARAMETRO.Rule = TIPO + identificador
                           | TIPO + identificador + L_DIMPARAM;

            L_DIMPARAM.Rule = MakePlusRule(L_DIMPARAM, DIM_DEF);

            DIM_DEF.Rule = "[" + entero + "]";

            //-------------------------------------------------------------------------------------------
            #endregion

            #region PRIN_CONS
            //-------------------------------------------------------------------------------------------
            PRINCIPAL.Rule = ToTerm("principal") + "(" + ")" + "{" + SENTENCIAS_CONS + "}";
            PRINCIPAL.ErrorRule = SyntaxError + "}";
            //-------------------------------------------------------------------------------------------

            //-------------------------------------------------------------------------------------------
            CONSTRUCTOR.Rule = identificador + "(" + PARAMETROS + ")" + "{" + SENTENCIAS_CONS + "}";

            CONSTRUCTOR.ErrorRule = SyntaxError + "}";
            //-------------------------------------------------------------------------------------------
            #endregion

            #region FORMULARIOS
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
            #endregion

            #region DECLARACIONES
            //-------------------------------------------------------------------------------------------
            DECLARACION_GLOBAL.Rule = TIPO + VISIBILIDAD + identificador + "=" + EXP + ";"
                                   | TIPO + VISIBILIDAD + identificador + ";"
                                   | TIPO + VISIBILIDAD + identificador + EMPTYDIM + ";" //INICIANDO UN ARREGLO VACIO
                                   | TIPO + VISIBILIDAD + identificador + EMPTYDIM + "=" + EXP + ";" //EN CASO QUE VENGA UN NUEVO ARREGLO
                                   | TIPO + VISIBILIDAD + identificador + EMPTYDIM + "=" + ARRAYDEF + ";"; // EN CASO QUE EL ARREGLO SE DEFINA DE UNA VEZ

            DECLARACION_GLOBAL.ErrorRule = SyntaxError + ";";

            DECLARACION_LOCAL.Rule = TIPO + identificador + "=" + EXP + ";"
                                   | TIPO + identificador + ";"
                                   | TIPO + identificador + EMPTYDIM + ";"
                                   | TIPO + identificador + EMPTYDIM + "=" + EXP + ";" //EN CASO QUE VENGA UN NUEVO ARREGLO
                                   | TIPO + identificador + EMPTYDIM + "=" + ARRAYDEF + ";"; // EN CASO QUE EL ARREGLO SE DEFINA DE UNA

            DECLARACION_LOCAL.ErrorRule = SyntaxError + ";";
            //-------------------------------------------------------------------------------------------
            #endregion

            #region EXPRESIONES
            //-------------------------------------------------------------------------------------------
            EXP.Rule = E
                    | IF_SIMPLE;

            E.Rule = E + and + E
                     | E + or + E
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
                     | E + pot + E
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
                     | LLAMADAID_OBJ
                     | DECLARACION_OBJ
                     | DECLARACION_ARR
                     | nulo
                     ;
            //-------------------------------------------------------------------------------------------
            #endregion

            #region OBJETOS_ARRS
            //-------------------------------------------------------------------------------------------
            LLAMADAID_OBJ.Rule = MakePlusRule(LLAMADAID_OBJ, ToTerm("."), identificador)
                               | MakePlusRule(LLAMADAID_OBJ, ToTerm("."), LLAMADA);

            LLAMADA.Rule = identificador + "(" + L_EXPRE + ")"
                         | identificador + DIMS;

            L_EXPRE.Rule = MakeStarRule(L_EXPRE, ToTerm(","), EXP);
            //-------------------------------------------------------------------------------------------

            //-------------------------------------------------------------------------------------------
            LLAMADAFUNCION.Rule = LLAMADAID_OBJ + ";";
            //-------------------------------------------------------------------------------------------

            //-------------------------------------------------------------------------------------------
            EMPTYDIM.Rule = MakePlusRule(EMPTYDIM, AUXDIMS);

            AUXDIMS.Rule = "[" + Empty + "]";

            DIMS.Rule = MakePlusRule(DIMS, REALDIM);

            REALDIM.Rule = "[" + EXP + "]";
            REALDIM.ErrorRule = SyntaxError + ";";

            ARRAYDEF.Rule = "{" + ARRELEMENTS + "}";
            ARRAYDEF.ErrorRule = SyntaxError + ";";

            ARRELEMENTS.Rule = MakePlusRule(ARRELEMENTS, ToTerm(","),  EXP)
                            | MakePlusRule(ARRELEMENTS, ToTerm(","), ARRAYDEF);

            DECLARACION_ARR.Rule = "nuevo" + TIPO + DIMS;

            ACCESOARRAY.Rule = identificador + DIMS;

            //-------------------------------------------------------------------------------------------
            #endregion

            #region OTROS
            //-------------------------------------------------------------------------------------------
            DECLARACION_OBJ.Rule = "nuevo" + TIPO + "(" + L_EXPRE + ")";

            DECLARACION_OBJ.ErrorRule = SyntaxError + ";";
            //-------------------------------------------------------------------------------------------

            //-------------------------------------------------------------------------------------------
            SENTENCIAS.Rule = MakeStarRule(SENTENCIAS, DECLARACION_LOCAL)
                            | MakeStarRule(SENTENCIAS, IMPRIMIR)
                            | MakeStarRule(SENTENCIAS, ASIGNACION)
                            | MakeStarRule(SENTENCIAS, RETORNO)
                            | MakeStarRule(SENTENCIAS, CONDICIONAL_SI)
                            | MakeStarRule(SENTENCIAS, MIENTRAS)
                            | MakeStarRule(SENTENCIAS, ROMPER)
                            | MakeStarRule(SENTENCIAS, CONTINUAR)
                            | MakeStarRule(SENTENCIAS, LLAMADAFUNCION)
                            | MakeStarRule(SENTENCIAS, HACERMIENTRAS)
                            | MakeStarRule(SENTENCIAS, REPETIRHASTA)
                            | MakeStarRule(SENTENCIAS, INCREMENTO)
                            | MakeStarRule(SENTENCIAS, DECREMENTO)
                            | MakeStarRule(SENTENCIAS, FOR);

            SENTENCIAS_CONS.Rule = MakeStarRule(SENTENCIAS_CONS, DECLARACION_LOCAL)
                                 | MakeStarRule(SENTENCIAS_CONS, IMPRIMIR)
                                 | MakeStarRule(SENTENCIAS_CONS, ASIGNACION)
                                 | MakeStarRule(SENTENCIAS_CONS, CONDICIONAL_SI)
                                 | MakeStarRule(SENTENCIAS_CONS, MIENTRAS)
                                 | MakeStarRule(SENTENCIAS_CONS, ROMPER)
                                 | MakeStarRule(SENTENCIAS_CONS, CONTINUAR)
                                 | MakeStarRule(SENTENCIAS_CONS, LLAMADAFUNCION)
                                 | MakeStarRule(SENTENCIAS_CONS, HACERMIENTRAS)
                                 | MakeStarRule(SENTENCIAS_CONS, REPETIRHASTA)
                                 | MakeStarRule(SENTENCIAS_CONS, INCREMENTO)
                                 | MakeStarRule(SENTENCIAS_CONS, DECREMENTO)
                                 | MakeStarRule(SENTENCIAS_CONS, FOR);
            //-------------------------------------------------------------------------------------------
            #endregion


            #region SENTENCIAS
            //-------------------------------------------------------------------------------------------
            IMPRIMIR.Rule = ToTerm("imprimir") + "(" + EXP + ")" + ";";
            IMPRIMIR.ErrorRule = SyntaxError + ";";
            //-------------------------------------------------------------------------------------------

            //-------------------------------------------------------------------------------------------
            ASIGNACION.Rule = LLAMADAID_OBJ + "." + identificador + "=" + EXP + ";" // 3
                            | identificador + "=" + EXP + ";" //2
                            | LLAMADAID_OBJ + "." + identificador + DIMS + "=" + EXP + ";" //4
                            | identificador + DIMS + "=" + EXP + ";"; //3
            //-------------------------------------------------------------------------------------------

            //-------------------------------------------------------------------------------------------
            RETORNO.Rule = ToTerm("retorno") + EXP + ";"
                        | ToTerm("retorno") + ";";
            //-------------------------------------------------------------------------------------------

            //-------------------------------------------------------------------------------------------
            CONDICIONAL_SI.Rule = ToTerm("Si") + "(" + EXP + ")" + "{" + SENTENCIAS + "}"
                                | ToTerm("Si") + "(" + EXP + ")" + "{" + SENTENCIAS + "}" + SINO;

            SINO.Rule = ToTerm("SiNo") + "{" + SENTENCIAS + "}"
                      | ToTerm("SiNo") + CONDICIONAL_SI;
            //-------------------------------------------------------------------------------------------

            //-------------------------------------------------------------------------------------------
            IF_SIMPLE.Rule = E + ToTerm("?") + EXP + ToTerm(":") + EXP;
            //-------------------------------------------------------------------------------------------

            //-------------------------------------------------------------------------------------------
            MIENTRAS.Rule = ToTerm("Mientras") + "(" + EXP + ")" + "{" + SENTENCIAS + "}";
            MIENTRAS.ErrorRule = SyntaxError + "}";
            //-------------------------------------------------------------------------------------------

            //-------------------------------------------------------------------------------------------
            HACERMIENTRAS.Rule = ToTerm("Hacer") + "{" + SENTENCIAS + "}" + ToTerm("Mientras") + "(" + EXP + ")" + ";";
            HACERMIENTRAS.ErrorRule = SyntaxError + "}";
            //-------------------------------------------------------------------------------------------

            //-------------------------------------------------------------------------------------------
            REPETIRHASTA.Rule = ToTerm("Repetir") + "{" + SENTENCIAS + "}" + ToTerm("Hasta") + "(" + EXP + ")" + ";";
            REPETIRHASTA.ErrorRule = SyntaxError + "}";
            //-------------------------------------------------------------------------------------------

            //-------------------------------------------------------------------------------------------
            ROMPER.Rule = ToTerm("Romper") + ";";
            CONTINUAR.Rule = ToTerm("Continuar") + ";";
            //-------------------------------------------------------------------------------------------

            //-------------------------------------------------------------------------------------------
            INCREMENTO.Rule = identificador + inc + ";";
            INCREMENTO.ErrorRule = SyntaxError + ";";

            DECREMENTO.Rule = identificador + dec + ";";
            DECREMENTO.ErrorRule = SyntaxError + ";";
            //-------------------------------------------------------------------------------------------

            //-------------------------------------------------------------------------------------------
            FOR.Rule = ToTerm("Para") + "(" + VARCONTROL + ";"  + EXP + ";" + OPERACION + ")" + "{" + SENTENCIAS + "}";

            VARCONTROL.Rule = TIPO + identificador + "=" + EXP
                          | identificador + "=" + EXP;

            OPERACION.Rule = identificador + inc
                           | identificador + dec
                           | identificador + "=" + EXP;
            //-------------------------------------------------------------------------------------------
            #endregion


            #endregion

            #region Preferencias
            RegisterOperators(1, Associativity.Left, or);
            RegisterOperators(2, Associativity.Left, and);
            RegisterOperators(3, Associativity.Left, nequal, equal);
            RegisterOperators(4, Associativity.Left, grater, lower, gequal, lequal);
            RegisterOperators(5, Associativity.Left, res, sum);
            RegisterOperators(6, Associativity.Left, divi, mult, mod);
            RegisterOperators(7, Associativity.Right, pot);
            RegisterOperators(9, Associativity.Right, inc, dec, not);
            RegisterOperators(10, Associativity.Neutral, ToTerm("("), ToTerm(")"));

            this.MarkPunctuation("{", "}", "(", ")", ";",".xform","{","}", "=", "[", "]",".");
            NonGrammarTerminals.Add(LineComment);
            NonGrammarTerminals.Add(MultiLineComment);
            this.Root = INICIO;
            #endregion
        }
        #endregion

        
    }
}
