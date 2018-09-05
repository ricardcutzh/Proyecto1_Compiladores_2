﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.ASTTree.Interfaces;
using XForms.Simbolos;
using XForms.Objs;
using System.Windows.Forms;

namespace XForms.ASTTree.Instrucciones
{
    class CuerpoClase : NodoAST, Instruccion
    {
        List<Instruccion> instrucciones;


        public CuerpoClase(int linea, int col, String clase):base(linea, col, clase)
        {
            this.instrucciones = new List<Instruccion>();
        }

        public void addInstruccion(Instruccion instruccion)
        {
            this.instrucciones.Add(instruccion);
        }

        public int numeroInstrucciones()
        {
            return this.instrucciones.Count();
        }

        public object Ejecutar(Ambito ambito)
        {
            foreach(Instruccion instruccion in this.instrucciones)
            {
                instruccion.Ejecutar(ambito);
                if(Estatico.paraEjecucionPorCantidadErrores())
                {
                    MessageBox.Show("Se han encontrado demasiados errores Semanticos, Revisa el reporte para Corregirlos! Se Pauso en: "+this.clase, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }
            }
            return null;
        }
    }
}