using System;
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
    class FunTam : NodoAST, Expresion
    {

        Expresion exp;

        public FunTam(Expresion exp,String clase, int linea, int col):base(linea, col, clase)
        {
            this.exp = exp;
        }

        String val = "";
        public string getTipo(Ambito ambito)
        {
            if (val.Equals(""))
            {
                return "nulo";
            }
            else
            {
                return "entero";
            }
        }

        public object getValor(Ambito ambito)
        {
            try
            {
                //ESTA FUNCION AUN HAY QYE MEJORARLA
                object valorReal = this.exp.getValor(ambito);
                String tam = valorReal.ToString();
                if(valorReal is Nulo)
                {
                    return 0;
                }
                else
                {
                    val = "entero";
                    return tam.Length;
                }
            }
            catch(Exception e)
            {
                TError error = new TError("Ejecucion", "Error al ejecutar la funcion: Tam() | Clase: "+this.clase+" | Archivo: "+ambito.archivo+" | Mensaje: "+e.Message, linea, columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            return 0;
        }
    }
}
