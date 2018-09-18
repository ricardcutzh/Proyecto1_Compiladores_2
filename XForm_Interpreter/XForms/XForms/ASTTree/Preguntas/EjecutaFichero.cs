using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.ASTTree.Interfaces;
using XForms.Simbolos;
using XForms.Objs;
using XForms.GramaticaIrony;
using XForms.ASTTree.Instrucciones;
using XForms.GUI.Notas;
using XForms.ASTTree.Sentencias;
using XForms.ASTTree.Valores;

namespace XForms.ASTTree.Preguntas
{
    class EjecutaFichero:NodoAST, Instruccion
    {

        int numero;
        String identificador;
        List<Expresion> expresiones;


        public EjecutaFichero(String identificador, List<Expresion> expresiones, int numero, String clase, int linea, int col):base(linea, col, clase)
        {
            this.identificador = identificador;
            this.expresiones = expresiones;
            this.numero = numero;
        }

        public object Ejecutar(Ambito ambito)
        {
            try
            {

            }
            catch(Exception e)
            {
                TError error = new TError("Ejecucion", "", linea, columna, false);
                Estatico.ColocaError(error);
                Estatico.errores.Add(error;)
            }
            return new Nulo();
        }
    }
}
