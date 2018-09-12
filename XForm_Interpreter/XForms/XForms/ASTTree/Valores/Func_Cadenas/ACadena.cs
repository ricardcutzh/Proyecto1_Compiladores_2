using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.GramaticaIrony;
using XForms.Objs;
using XForms.ASTTree.Interfaces;
using XForms.Simbolos;

namespace XForms.ASTTree.Valores.Func_Cadenas
{
    class ACadena : NodoAST, Expresion
    {

        Expresion valor;

        public ACadena(Expresion valor, String clase, int linea, int col):base(linea, col, clase)
        {
            this.valor = valor;
        }

        String val = "";
        public string getTipo(Ambito ambito)
        {
            if(val.Equals(""))
            {
                return "nulo";
            }
            else
            {
                return "cadena";
            }
        }

        public object getValor(Ambito ambito)
        {
            try
            {
                Object val = this.valor.getValor(ambito);
                this.val = val.ToString();
                return this.val;
            }
            catch(Exception e)
            {
                TError error = new TError("Ejecucion", "Error al intentar comventir en cadena: Funcion: Cadena() | Clase: " + this.clase + " | Archivo: " + ambito.archivo + " | Mensaje: " + e.Message, linea, columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            return new Nulo();
        }
    }
}
