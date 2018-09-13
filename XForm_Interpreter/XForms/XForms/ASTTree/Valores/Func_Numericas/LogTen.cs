﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.GramaticaIrony;
using XForms.Objs;
using XForms.ASTTree.Interfaces;
using XForms.Simbolos;

namespace XForms.ASTTree.Valores.Func_Numericas
{
    class LogTen : NodoAST, Expresion
    {

        Expresion exp;

        public LogTen(Expresion exp, String clase, int linea, int col):base(linea,col, clase)
        {
            this.exp = exp;
        }

        object valor = new Nulo();
        public string getTipo(Ambito ambito)
        {
            if (valor is Nulo)
            {
                return "nulo";
            }
            else
            {
                return "decimal";
            }
        }

        public object getValor(Ambito ambito)
        {
            try
            {
                Object valor = exp.getValor(ambito);
                if (valor is int)
                {
                    int v = (int)valor;
                    if (v < 0) { this.valor = 0.0; return 0.0; }
                    this.valor = Math.Log10(v);
                    return this.valor;
                }
                else if (valor is double)
                {
                    double v = (double)valor;
                    if (v < 0) { this.valor = 0.0; return 0.0; }
                    this.valor = Math.Log10(v);
                    return this.valor;
                }
                else
                {
                    TError error = new TError("Semantico", "El parametro que reciba la Funcion Log10() puede ser Entero o Decimal | Clase: " + this.clase + " | Archivo: " + ambito.archivo, linea, columna, false);
                    Estatico.errores.Add(error);
                    Estatico.ColocaError(error);
                }
            }
            catch (Exception e)
            {
                TError error = new TError("Ejecucion", "Error al ejecutar la funcion Log10() | Clase: " + this.clase + " | Archivo: " + ambito.archivo + " | Mensaje: " + e.Message, linea, columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            this.valor = 0.0;
            return 0.0;
        }
    }
}
