using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.ASTTree.Interfaces;
using XForms.Objs;
using XForms.Simbolos;
using System.Windows.Forms;
using XForms.GramaticaIrony;

namespace XForms.ASTTree.Instrucciones
{
    class DeclaracionVar: NodoAST, Instruccion
    {
        Expresion expresion;
        String idVar;
        String tipo;
        Estatico.Vibililidad visibilidad;
        Boolean asignaValor;

        /*CONSRUCTOR DE UNA DECLARACION DE UNA VARIABLE*/
        public DeclaracionVar(Expresion expresio, String idvar, String tipo, Estatico.Vibililidad visibilidad, int linea, int col, String clase) : base(linea, col, clase)
        {
            this.expresion = expresio;
            this.idVar = idvar;
            this.tipo = tipo;
            this.visibilidad = visibilidad;
            this.asignaValor = true;
        }

        public DeclaracionVar(String idvar, String tipo, Estatico.Vibililidad visibilidad, int linea, int columna, String clase) : base(linea, columna, clase)
        {
            this.expresion = null;
            this.tipo = tipo;
            this.visibilidad = visibilidad;
            this.idVar = idvar;
            this.asignaValor = false;
        }

        public object Ejecutar(Ambito ambito)
        {
            try
            {
                if (this.asignaValor)
                {
                    //OBT EL VALOR
                    Object valor = this.expresion.getValor(ambito);//LLAMADA AL VALOR
                    String tipoaux = this.expresion.getTipo(ambito);
                    if(this.tipo.ToLower().Equals(tipoaux.ToLower()) || tipoaux.ToLower().Equals("nulo")) //SON DEL MISMO TIPO? ESTO ME PERMITE ASIGNARLE NULO COMO TIPO VALIDO
                    {
                        //AQUI ASGINO LA VARIABLE
                        if(!ambito.existeVariable(this.idVar.ToLower())) //YA EXISTE?
                        {
                            Variable variable = new Variable(this.idVar.ToLower(), this.tipo.ToLower(), this.visibilidad, valor);
                            ambito.agregarVariableAlAmbito(this.idVar.ToLower(), variable);
                            //ambito.ImprimeAmbito();
                        }
                        else
                        {
                            TError error = new TError("Semantico", "Ya existe una declaracion de: \"" + this.idVar + "\" en este Ambito | Clase: "+this.clase+" | Archivo: "+ambito.archivo, this.linea, this.columna, false);
                            Estatico.errores.Add(error);
                            Estatico.ColocaError(error);
                        }
                    }
                    else
                    {
                        //AQUI MUESTRO EL ERROR QUE SUCEDA
                        TError error = new TError("Semantico", "El valor Asignado a \""+this.idVar+"\" no concuerda, Esperado: \""+this.tipo+"\", encontrado: \""+tipoaux+"\" | Clase: "+this.clase+" | Archivo: "+ambito.archivo, this.linea, this.columna, false);
                        Estatico.errores.Add(error);
                        Estatico.ColocaError(error);
                    }
                }
                else
                {
                    if(!ambito.existeVariable(this.idVar.ToLower()))// EXISTE YA UNA DEFINICION DE LA VARIABLE?
                    {
                        Variable variable = new Variable(this.idVar.ToLower(), this.tipo, this.visibilidad, new Nulo());
                        ambito.agregarVariableAlAmbito(this.idVar.ToLower(), variable);
                    }
                    else//SI SI EXISTE... ERROR..
                    {
                        TError error = new TError("Semantico", "Ya existe una declaracion de: \"" + this.idVar + "\" en este Ambito | Clase: " + this.clase + " | Archivo: " + ambito.archivo, this.linea, this.columna, false);
                        Estatico.errores.Add(error);
                        Estatico.ColocaError(error);
                    }
                }
            }
            catch(Exception e)
            {
                TError error = new TError("Ejecucion", "Error en la declaracion de variable: \"" + this.idVar + "\" en este Ambito | Clase: " + this.clase + " | Archivo: " + ambito.archivo+" | "+e.Message, this.linea, this.columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            return null;
        }
    }
}
