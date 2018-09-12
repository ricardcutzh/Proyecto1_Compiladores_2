using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.GramaticaIrony;
using XForms.ASTTree.Interfaces;
using XForms.Objs;
using XForms.Simbolos;
using System.Windows.Forms;

namespace XForms.ASTTree.Sentencias
{
    class Mensajes : NodoAST, Instruccion
    {
        Expresion valor;

        public Mensajes(Expresion valor, String clase, int linea, int col):base(linea, col, clase)
        {
            this.valor = valor;
        }

        public object Ejecutar(Ambito ambito)
        {
            try
            {
                Object valor = this.valor.getValor(ambito);
                MessageBox.Show(valor.ToString(), "Mensajes");
            }
            catch(Exception e)
            {
                TError error = new TError("Ejecucion", "Error al ejecutar Mensajes: | Clase: "+this.clase+" | Archivo: "+ambito.archivo+" | Mensaje: "+e.Message, linea, columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            return new Nulo();
        }
    }
}
