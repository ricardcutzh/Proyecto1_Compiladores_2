﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.ASTTree.Interfaces;
using XForms.GramaticaIrony;
using XForms.Simbolos;
using XForms.Objs;
namespace XForms.ASTTree.Sentencias
{
    class AsignacionSimple : NodoAST, Instruccion
    {

        Expresion valor;
        String identificador;

        public AsignacionSimple(Expresion valor, String id, int linea, int col, String clase):base(linea, col, clase)
        {
            this.valor = valor;
            this.identificador = id;
        }

        public object Ejecutar(Ambito ambito)
        {
            try
            {
                Simbolo s = (Simbolo)ambito.getSimbolo(this.identificador.ToLower());
                if(s!=null)
                {
                    if(s is Variable)
                    {
                        Variable vari = (Variable)s;
                        String tipoEsperado = vari.Tipo;
                        Object val = this.valor.getValor(ambito);
                        String tipoEncontrado = this.valor.getTipo(ambito);
                        if(tipoEncontrado.ToLower().Equals(tipoEsperado.ToLower()))
                        {
                            vari.valor = val;//ASIGNO EL NUEVO VALOR
                        }
                        else
                        {
                            TError error = new TError("Semantico", "Tipos no Coinciden al Asignar, Esperado: \"" + tipoEsperado + "\", Encontrado: " + tipoEncontrado + "\" | Clase: " + this.clase + " | Archivo: " + ambito.archivo, linea, columna, false);
                            Estatico.errores.Add(error);
                            Estatico.ColocaError(error);
                        }
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
                else
                {
                    TError error = new TError("Semantico", "Referencia a Simbolo: \"" + this.identificador + "\" inexistente | Clase: " + this.clase + " | Archivo: " + ambito.archivo, linea, columna, false);
                    Estatico.errores.Add(error);
                    Estatico.ColocaError(error);
                }
            }
            catch(Exception e)
            {
                TError error = new TError("Ejecuccion", "Error al ejecutar la asignacion a Var: \"" + this.identificador + "\" | Clase: " + this.clase + " | Archivo: " + ambito.archivo+" | "+e.Message, linea, columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            return null;
        }
    }
}
