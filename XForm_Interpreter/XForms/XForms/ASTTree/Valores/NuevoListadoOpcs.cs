using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.GramaticaIrony;
using XForms.ASTTree.Interfaces;
using XForms.Simbolos;
using XForms.Objs;
using XForms.ASTTree.Sentencias;

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

                Ambito ambitoOpcs = new Ambito(null, this.clase, ambito.archivo); //CREO EL AMBITO DEL OBJETO

                Opciones listado = new Opciones("opciones", null); // CREO EL LISTADO QUE VA A MANEJAR LOS VALORES

                Variable v = new Variable("cutz", "opciones", Estatico.Vibililidad.LOCAL, listado); // SETEO LA VARIABLE QUE ME VA A GUARDAR EL LISTADO

                ambitoOpcs.agregarVariableAlAmbito("cutz", v); /// LO AGREGO AL AMBITO

                Objeto opciones = new Objeto("opciones", ambitoOpcs); /// CREO EL OBJETO OPCIONES


                return opciones; //LO RETORNO
            }
            catch(Exception e)
            {

            }
            return new Nulo();
        }
    }
}
