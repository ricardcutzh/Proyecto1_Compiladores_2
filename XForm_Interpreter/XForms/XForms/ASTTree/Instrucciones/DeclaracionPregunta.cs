using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.ASTTree.Interfaces;
using XForms.Simbolos;
using XForms.Objs;
using XForms.GramaticaIrony;

namespace XForms.ASTTree.Instrucciones
{
    class DeclaracionPregunta:NodoAST, Instruccion
    {

        List<Instruccion> declaraciones;
        String identificador;
        List<NodoParametro> parametros;

        public DeclaracionPregunta(List<Instruccion> declaraciones, String identificador, List<NodoParametro> parametros,String clase, int linea, int col):base(linea, col, clase)
        {
            this.declaraciones = declaraciones;
            this.identificador = identificador;
            this.parametros = parametros;
        }

        public object Ejecutar(Ambito ambito)
        {

            /// YA QUE LO VOY A TRATAR COMO UN OBJETO:
            /// 1) DEBO DE COLOCARLE UN CONSTRUCTOR AL CUAL ME VA A SERVIR PARA PODER MANDARLE LOS PARAMETROS CUANDO ESTA SE LLAME
            /// 2) AL MOMENTO DE LLAMAR LA PREGUNTA DEBO DE MANDARLE LOS MISMOS PARAMETROS PARA INSTANCIAR LA PREGUNTA QUE EN TEORIA DEBERIAN SER LOS MISMOS
            /// 3) LA PREGUNTA LA VOY A GUARDAR COMO UN OBJETO DENTRO DEL AMBITO DE LA CLASE LA CUAL VA A GUARDAR COMO UNA VARIABLE QUE A GUARDAR COMO VALOR OBJETO DE TIPO
            /// PREGUNTA
            try
            {
                if(!ambito.existeVariable(this.identificador.ToLower()))
                {
                    Ambito amPregunta = new Ambito(ambito, this.identificador.ToLower(), ambito.archivo);

                    /*CREO EL CONSTRUCTOR DE LA PREGUNTA QUE ME VA A SERVIR PARA PODER DECLARAR EN EL AMBITO LAS VARIABLES*/
                    Constructor c = new Constructor(this.parametros, new List<Instruccion>(), linea, columna, clase);

                    /*AGREGO EL CONSTRUCTOR DE LA PREGUNTA AL AMBITO*/
                    amPregunta.agregarConstructor(new ClaveFuncion(this.identificador.ToLower(), "vacio", parametros), c);

                    /*VOY A A GREGAR UNA VARIABLE TEMPORAL PARA QUE ENTONCES PUEDA GUARDAR LAS INSTRUCCIONES :) */
                    Variable v = new Variable("instr", "vale", Estatico.Vibililidad.PRIVADO, this.declaraciones);

                    /*LA AGREGO AL AMBITO DE ESTA PREGUNTA :)*/
                    amPregunta.agregarVariableAlAmbito("instr", v);

                    /*CREO EL OBJETO DE TIPO PREGUNTA*/
                    Objeto pregunta = new Objeto("pregunta", amPregunta);

                    /*AGREGO LA VARIABLE AL AMBITO ACTUAL PARA PODER ACCERDER A LA PREGUNTA EN UN LLAMADO A LA MISMA*/
                    Variable p = new Variable(this.identificador.ToLower(), "pregunta", Estatico.Vibililidad.PRIVADO, pregunta);

                    /*AGREGO LA VARIABLE EN EL AMBITO Y TERMINA*/
                    ambito.agregarVariableAlAmbito(this.identificador.ToLower(), p);

                    return new Nulo();
                    
                }
                else
                {
                    TError error = new TError("Semantico", "Ya existe una definicion de un simbolo: \""+identificador+"\", las preguntas se tratan como objetos por lo que no pueden haber multiples definiciones de un objeto con el mismo nombre | Clase"
                        +clase + " | Archivo: "+ambito.archivo, linea, columna, false);
                    Estatico.ColocaError(error);
                    Estatico.errores.Add(error);
                }
            }
            catch(Exception e)
            {
                TError erro = new TError("Ejecucion", "Error al intentar ejecutar la declaracion de una pregunta | Clase: " + clase + " | Archivo: " + ambito.archivo + " | Mensaje: " + e.Message, linea, columna, false);
                Estatico.errores.Add(erro);
                Estatico.ColocaError(erro);
            }
            return new Nulo();
        }
    }
}
