using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.ASTTree.Interfaces;
using XForms.Objs;
using XForms.Simbolos;

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
            throw new NotImplementedException();
        }
    }
}
