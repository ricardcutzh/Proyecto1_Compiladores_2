using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.GramaticaIrony;
using XForms.ASTTree.Interfaces;
using XForms.Simbolos;
using XForms.Objs;

namespace XForms.ASTTree.Valores
{
    class NuevoListadoOpcs : NodoAST, Expresion
    {


        public NuevoListadoOpcs(String clase, int linea, int col):base(linea, col, clase)
        {
            ///SOLO ME VA A SERVIR PARA HACER REFERENCIA AL LISTADO
        }

        public string getTipo(Ambito ambito)
        {
            return "opciones";
        }

        public object getValor(Ambito ambito)
        {
            try
            {
                Ambito ambitoOpcs = new Ambito(null, this.clase, ambito.archivo);

                Opciones ops = new Opciones(this.clase.ToLower(), ambitoOpcs);

                return ops;
            }
            catch(Exception e)
            {

            }
            return new Nulo();
        }
    }
}
