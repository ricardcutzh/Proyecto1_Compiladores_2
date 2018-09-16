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
            OPERACION = new NonTerminal("OPERACION"),
            SWITCH = new NonTerminal("SWITCH"),
            CASO = new NonTerminal("CASO"),
            DEFECTO = new NonTerminal("DEFECTO"),
            CUERPOSWITCH = new NonTerminal("CUERPOSWITCH"),
            LLAMADAFORMULARIO = new NonTerminal("LLAMADAFORM"),
            ESTILOS = new NonTerminal("ESTILOS"),
            CUERPO_PREGUNTA = new NonTerminal("CUERPO_PREGUNTA"),
            CALL_Q = new NonTerminal("CALL_Q"),
            CASTEO_PREGUNTA = new NonTerminal("CASTEO_PREGUNTA"),
            ESTILO_RESP = new NonTerminal("ESTILO_RESP"),
            MENSAJES = new NonTerminal("MENSAJES"),
            OPCS_NUEVO = new NonTerminal("OPCS_NUEVO"),
            FUN_CADENA = new NonTerminal("FUN_CADENA"),
            FUN_SUBCAD = new NonTerminal("FUN_SUBCAD"),
            FUN_POSCAD = new NonTerminal("FUN_POSCAD"),
            FUN_BOOLEAN = new NonTerminal("FUN_BOOLEAN"),
            FUN_ENTERO = new NonTerminal("FUN_ENTERO"),
            FUN_TAM = new NonTerminal("FUN_TAM"),
            FUN_RANDOM = new NonTerminal("FUN_RANDOM"),
            FUN_MIN = new NonTerminal("FUN_MIN"),
            FUN_MAX = new NonTerminal("FUN_MAX"),
            FUN_POW = new NonTerminal("FUN_POW"),
            FUN_LOG = new NonTerminal("FUN_LOG"),
            FUN_LOG10 = new NonTerminal("FUN_LOG10"),
            FUN_ABS = new NonTerminal("FUN_ABS"),
            FUN_SIN = new NonTerminal("FUN_SIN"),
            FUN_COS = new NonTerminal("FUN_COS"),
            FUN_TAN = new NonTerminal("FUN_TAN"),
            FUN_SQRT = new NonTerminal("FUN_SQRT"),
            FUN_PI = new NonTerminal("FUN_PI"),
            FUN_HOY = new NonTerminal("FUN_HOY"),
            FUN_AHORA = new NonTerminal("FUN_AHORA"),
            FUN_AFECHA = new NonTerminal("FUN_AFECHA"),
            FUN_TOHORA = new NonTerminal("FUN_TOHORA"),
            FUN_TOFECHAHORA = new NonTerminal("FUN_TOFECHAHORA"),
            OPCS_AGREGAR = new NonTerminal("OPCS_AGREGAR"),
            SUPER = new NonTerminal("SUPER"),
            EST_CADENA = new NonTerminal("EST_CADENA"),
            EST_ENTERO = new NonTerminal("EST_ENTERO"),
            EST_SELECC = new NonTerminal("EST_SELECC"),
            EST_DECIMAL = new NonTerminal("EST_DECIMAL"),
            EST_CONDICION = new NonTerminal("EST_CONDICION"),
            EST_FECHA = new NonTerminal("EST_FECHA"),
            EST_HORA = new NonTerminal("EST_HORA"),
            EST_FECHAHORA = new NonTerminal("EST_FECHAHORA"),
            FUNC_MULTIMEDIA = new NonTerminal("FUN_MULTIMEDIA");
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
                       | ToTerm("vacio")
                       | ToTerm("Respuestas");
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

            //-------------------------------------------------------------------------------------------
            CUERPO_PREGUNTA.Rule = MakeStarRule(CUERPO_PREGUNTA, DECLARACION_LOCAL)
                                | MakeStarRule(CUERPO_PREGUNTA, FUNCIONES);
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
            PREGUNTA.Rule = ToTerm("pregunta") + identificador + "(" + PARAMETROS + ")" + "{" + CUERPO_PREGUNTA + "}";
            PREGUNTA.ErrorRule = SyntaxError + "}";
            //-------------------------------------------------------------------------------------------

            //-------------------------------------------------------------------------------------------
            GRUPO.Rule = ToTerm("grupo") + identificador + "{" + SENTENCIAS + "}";
            GRUPO.ErrorRule = SyntaxError + "}";
            //-------------------------------------------------------------------------------------------

            //-------------------------------------------------------------------------------------------
            FORMULARIO.Rule = ToTerm("formulario") + identificador + "{" + SENTENCIAS + "}";
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
                     | FUN_TOFECHAHORA
                     | FUN_TOHORA
                     | FUN_AFECHA
                     | FUN_AHORA
                     | FUN_HOY
                     | FUN_PI
                     | FUN_SIN
                     | FUN_COS
                     | FUN_TAN
                     | FUN_SQRT
                     | FUN_ABS
                     | FUN_LOG10
                     | FUN_LOG
                     | FUN_POW
                     | FUN_MIN
                     | FUN_MAX
                     | FUN_RANDOM
                     | FUN_ENTERO
                     | FUN_TAM
                     | FUN_BOOLEAN
                     | FUN_SUBCAD
                     | FUN_POSCAD
                     | LLAMADAID_OBJ
                     | DECLARACION_OBJ
                     | DECLARACION_ARR
                     | OPCS_NUEVO
                     | FUN_CADENA
                     | nulo
                     ;
            //-------------------------------------------------------------------------------------------

            //-------------------------------------------------------------------------------------------
            FUN_CADENA.Rule = ToTerm("Cadena") + "(" + EXP + ")";
            FUN_CADENA.ErrorRule = SyntaxError + ";";

            FUN_SUBCAD.Rule = ToTerm("subCad") + "(" + EXP + "," + EXP + "," + EXP + ")";
            FUN_SUBCAD.ErrorRule = SyntaxError + ";";

            FUN_POSCAD.Rule = ToTerm("posCad") + "(" + EXP + "," + EXP + ")";
            FUN_POSCAD.ErrorRule = SyntaxError + ";";

            FUN_BOOLEAN.Rule = ToTerm("booleano") + "(" + EXP + ")";
            FUN_BOOLEAN.ErrorRule = SyntaxError + ";";

            FUN_ENTERO.Rule = ToTerm("entero") + "(" + EXP + ")";
            FUN_ENTERO.ErrorRule = SyntaxError + ";";

            FUN_TAM.Rule = ToTerm("tam") + "(" + EXP + ")";
            FUN_TAM.ErrorRule = SyntaxError + ";";

            FUN_RANDOM.Rule = ToTerm("random") + "(" + L_EXPRE + ")"
                            | ToTerm("random") + "(" + ")";
            FUN_RANDOM.ErrorRule = SyntaxError + ";";

            FUN_MIN.Rule = ToTerm("min") + "(" + L_EXPRE + ")" // POR SI VIENEN MUCHAS EXPRESIONES
                        | ToTerm("min") + "(" + identificador + ")";//ESTO ES POR SI ES ARREGLO
            FUN_MIN.ErrorRule = SyntaxError + ";";

            FUN_MAX.Rule = ToTerm("max") + "(" + L_EXPRE + ")" //ESTO ES POR SI VIENEN MUCHAS EXPRESIONES
                        | ToTerm("max") + "(" + identificador + ")"; // POR SI ES ARREGLO
            FUN_MAX.ErrorRule = SyntaxError + ";";

            FUN_POW.Rule = ToTerm("pow") + "(" + EXP + "," + EXP + ")";
            FUN_POW.ErrorRule = SyntaxError + ";";

            FUN_LOG.Rule = ToTerm("log") + "(" + EXP + ")";
            FUN_LOG.ErrorRule = SyntaxError + ";";

            FUN_LOG10.Rule = ToTerm("log10") + "(" + EXP + ")";
            FUN_LOG10.ErrorRule = SyntaxError + ";";

            FUN_ABS.Rule = ToTerm("abs") + "(" + EXP + ")";
            FUN_ABS.ErrorRule = SyntaxError + ";";

            FUN_SIN.Rule = ToTerm("sin") + "(" + EXP + ")";
            FUN_SIN.ErrorRule = SyntaxError + ";";

            FUN_COS.Rule = ToTerm("cos") + "(" + EXP + ")";
            FUN_COS.ErrorRule = SyntaxError + ";";

            FUN_TAN.Rule = ToTerm("tan") + "(" + EXP + ")";
            FUN_TAN.ErrorRule = SyntaxError + ";";

            FUN_SQRT.Rule = ToTerm("sqrt") + "(" + EXP + ")";
            FUN_SQRT.ErrorRule = SyntaxError + ";";

            FUN_PI.Rule = ToTerm("pi") + "(" + ")";
            FUN_PI.ErrorRule = SyntaxError + ";";

            FUN_HOY.Rule = ToTerm("hoy") + "(" + ")";
            FUN_HOY.ErrorRule = SyntaxError + ";";

            FUN_AHORA.Rule = ToTerm("ahora") + "(" + ")";
            FUN_AHORA.ErrorRule = SyntaxError + ";";

            FUN_AFECHA.Rule = ToTerm("fecha") + "(" + EXP + ")";
            FUN_AFECHA.ErrorRule = SyntaxError + ";";

            FUN_TOHORA.Rule = ToTerm("hora") + "(" + EXP + ")";
            FUN_TOHORA.ErrorRule = SyntaxError + ";";

            FUN_TOFECHAHORA.Rule = ToTerm("fechahora") + "(" + EXP +")";
            FUN_TOFECHAHORA.ErrorRule = SyntaxError + ";";
            //-------------------------------------------------------------------------------------------
            #endregion

            #region OBJETOS_ARRS
            //-------------------------------------------------------------------------------------------
            LLAMADAID_OBJ.Rule = MakePlusRule(LLAMADAID_OBJ, ToTerm("."), identificador)
                               | MakePlusRule(LLAMADAID_OBJ, ToTerm("."), LLAMADA);

            LLAMADA.Rule = identificador + "(" + L_EXPRE + ")"
                         | identificador + DIMS
                         | ToTerm("buscar") + "(" + EXP + ")" + "[" + EXP + "]"
                         | ToTerm("obtener") + "(" + EXP + ")" + "[" + EXP + "]";

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

            //-------------------------------------------------------------------------------------------
            OPCS_NUEVO.Rule = ToTerm("nuevo") + ToTerm("Opciones") + "(" + ")";
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
                            | MakeStarRule(SENTENCIAS, FOR)
                            | MakeStarRule(SENTENCIAS, SWITCH)
                            | MakeStarRule(SENTENCIAS, LLAMADAFORMULARIO)
                            | MakeStarRule(SENTENCIAS, CALL_Q)
                            | MakeStarRule(SENTENCIAS, MENSAJES)
                            | MakeStarRule(SENTENCIAS, FUNC_MULTIMEDIA);

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
                                 | MakeStarRule(SENTENCIAS_CONS, FOR)
                                 | MakeStarRule(SENTENCIAS_CONS, SWITCH)
                                 | MakeStarRule(SENTENCIAS_CONS, LLAMADAFORMULARIO)
                                 | MakeStarRule(SENTENCIAS_CONS, CALL_Q)
                                 | MakeStarRule(SENTENCIAS_CONS, MENSAJES)
                                 | MakeStarRule(SENTENCIAS_CONS, SUPER)
                                 | MakeStarRule(SENTENCIAS_CONS, FUNC_MULTIMEDIA);
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

            //-------------------------------------------------------------------------------------------
            CASO.Rule = EXP + ":" + "{" + SENTENCIAS + "}";

            CASO.ErrorRule = SyntaxError + "}";

            DEFECTO.Rule = ToTerm("Defecto") + ":" + "{" + SENTENCIAS + "}";

            DEFECTO.ErrorRule = SyntaxError + "}";

            SWITCH.Rule = ToTerm("Caso") + "(" + EXP + ")" + "{" + CUERPOSWITCH + "}";
            SWITCH.ErrorRule = SyntaxError + "}";

            CUERPOSWITCH.Rule = MakeStarRule(CUERPOSWITCH, CASO)
                            | MakeStarRule(CUERPOSWITCH, DEFECTO);
            //-------------------------------------------------------------------------------------------

            //-------------------------------------------------------------------------------------------
            FUNC_MULTIMEDIA.Rule = ToTerm("Imagen") + "(" + EXP + "," + EXP + ")" + ";"
                                 | ToTerm("Video") + "(" + EXP + "," + EXP + ")" + ";"
                                 | ToTerm("Audio") + "(" + EXP + "," + EXP + ")" + ";";

            FUNC_MULTIMEDIA.ErrorRule = SyntaxError + ";";
            //-------------------------------------------------------------------------------------------

            //-------------------------------------------------------------------------------------------
            LLAMADAFORMULARIO.Rule = ToTerm("nuevo") + identificador + "(" + ")" + ";"
                                   | ToTerm("nuevo") + identificador + "(" + ")" + "." +  ESTILOS + ";";

            ESTILOS.Rule = ToTerm("Todo")
                         | ToTerm("Cuadricula")
                         | ToTerm("Pagina");
            //-------------------------------------------------------------------------------------------

            //-------------------------------------------------------------------------------------------
            CALL_Q.Rule = identificador + "(" + L_EXPRE + ")" + "." + ToTerm("nota") + "(" + ")" + ";" /// pregunta(params).nota();
                        | identificador + "(" + L_EXPRE + ")" + "." + ToTerm("nota") + "(" + ")" + "." + ToTerm("mostrar") + "(" + ")" + ";" /// pregunta(params).nota().mostrar();
                        | identificador + "(" + L_EXPRE + ")" + "." + ToTerm("fichero") + "(" + EXP + ")" + ";"
                        | identificador + "(" + L_EXPRE + ")" + "." + ToTerm("fichero") + "(" + ")" + ";"
                        | identificador + "(" + L_EXPRE + ")" + "." + ToTerm("respuesta") + "(" + CASTEO_PREGUNTA + ")" + "." + ESTILO_RESP + ";"
                        | identificador + "(" + L_EXPRE + ")" + "." + ToTerm("respuesta") + "(" + CASTEO_PREGUNTA + ")" + "." + ToTerm("Apariencia") + "(" + ")" + "." + ESTILO_RESP + ";";

            CALL_Q.ErrorRule = SyntaxError + ";";

            CASTEO_PREGUNTA.Rule = ToTerm("resp.esCadena")
                                | ToTerm("resp.esBooleano")
                                | ToTerm("resp.esEntero")
                                | ToTerm("resp.esDecimal")
                                | ToTerm("resp.esFecha")
                                | ToTerm("resp.esHora")
                                | ToTerm("resp.esFechaHora");

            ESTILO_RESP.Rule = EST_CADENA
                            | EST_ENTERO
                            | EST_SELECC
                            | EST_DECIMAL
                            | EST_CONDICION
                            | EST_FECHA
                            | EST_HORA
                            | EST_FECHAHORA;

            EST_CADENA.Rule = ToTerm("Cadena") + "(" + EXP + "," + EXP + "," + EXP + ")" /// SI VIENE PARAMETROS EN CADENA
                            | ToTerm("Cadena") + "(" + ")"; /// CADENA SIN PARAMETROS

            EST_ENTERO.Rule = ToTerm("Entero") + "(" + ")" /// ENTEROS
                            | ToTerm("Entero") + "(" + EXP + "," + EXP + ")"; /// CUANDO VIENE RANGO INICIAL O FINAL

            EST_SELECC.Rule = ToTerm("seleccionar_1") + "(" + EXP + ")" /// SELECCIONAR 1
                            | ToTerm("seleccionar") + "(" + EXP + ")"; /// SELECCIONAR

            EST_DECIMAL.Rule = ToTerm("Decimal") + "(" + ")"; /// DECIMAL

            EST_CONDICION.Rule = ToTerm("Condicion") + "(" + EXP + "," + EXP + ")"; /// SI VIENE SI O NO Y ES PARA CONDICION

            EST_FECHA.Rule = ToTerm("Fecha") + "(" + ")"; /// FECHA

            EST_HORA.Rule = ToTerm("Hora") + "(" + ")";/// HORA

            EST_FECHAHORA.Rule = ToTerm("FechaHora") + "(" + ")";
            //-------------------------------------------------------------------------------------------

            //-------------------------------------------------------------------------------------------
            MENSAJES.Rule = ToTerm("Mensajes") + "(" + EXP + ")" + ";";
            //-------------------------------------------------------------------------------------------

            //-------------------------------------------------------------------------------------------
            SUPER.Rule = ToTerm("super") + "(" + L_EXPRE + ")" + ";";
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

            this.MarkPunctuation("{", "}", "(", ")", ";",".xform","{","}", "=", "[", "]",".",",");
            NonGrammarTerminals.Add(LineComment);
            NonGrammarTerminals.Add(MultiLineComment);
            this.Root = INICIO;
            #endregion
        }
        #endregion

        
    }
}
