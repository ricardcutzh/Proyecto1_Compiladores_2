using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.Objs;
using XForms.ASTTree.Interfaces;
using XForms.ASTTree.Valores;
using XForms.GramaticaIrony;
namespace XForms.Simbolos
{
    class Funcion : NodoAST, Instruccion
    {

        /*VA A TENER UNA LISTA DE PARAMETROS*/
        /*VA A TENER UNA LISTA DE INSTRUCCIONES QUE VA A AJECUTAR*/
        /*VA A HEREDAR DE LA CLASE INSTRUCCION PARA PODER IMPLEMENTAR LA EJECUCCION*/

        public String idFuncion { get; }
        public String Tipo { get; }
        public Estatico.Vibililidad Vibililidad { get; }
        List<NodoParametro> parametros;
        List<Object> instrucciones;
        public Funcion(List<Object> instrucciones, List<NodoParametro> parametros, String idFuncion, String Tipo, Estatico.Vibililidad Visibilidad, int linea, int col, String clase) : base(linea, col, clase)
        {
            this.idFuncion = idFuncion;
            this.Tipo = Tipo;
            this.Vibililidad = Visibilidad;
            this.parametros = parametros;
            this.instrucciones = instrucciones;
        }

        public Ambito seteaParametrosLocales(Ambito m, List<Object> valores)
        {
            try
            {
                if (valores.Count == this.parametros.Count)
                {
                    for (int x = 0; x < valores.Count; x++)
                    {
                        String tipo = this.parametros.ElementAt(x).tipo.ToLower();
                        String id = this.parametros.ElementAt(x).idparam.ToLower();
                        Variable v = new Variable(id, tipo, Estatico.Vibililidad.LOCAL, valores.ElementAt(x));
                        m.agregarVariableAlAmbito(id, v);
                    }
                }
            }
            catch
            {
                TError error = new TError("Ejecucion", "Error al ejecutar los parametros en clase: " + this.clase, this.linea, this.columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            return m;
        }

        public object Ejecutar(Ambito ambito)
        {
            try
            {
                foreach(Object ob in this.instrucciones)
                {
                    if(ob is Instruccion)
                    {
                        Instruccion aux = (Instruccion)ob;
                        Object res = aux.Ejecutar(ambito);
                        if(res is NodoReturn)
                        {
                            return res;
                        }
                    }
                    else if(ob is Expresion)
                    {
                        Expresion exp = (Expresion)ob;
                        Object res = exp.getValor(ambito);
                        if(res is NodoReturn)
                        {
                            return res;
                        }
                    }
                }
            }
            catch(Exception e)
            {
                TError error = new TError("Ejecucion", "Error al Ejecutar la funcion: "+idFuncion+" | Error: " + e.Message, this.linea, this.columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            return new NodoReturn(new Vacio(), "vacio");
        }


        public override string ToString()
        {
            String cad = "";
            cad += "| Funcion: " + this.idFuncion + " | Tipo: " + this.Tipo + " | Visibilidad: " + this.Vibililidad + " | Params: " + this.parametros.Count;
            return cad;
        }
    }
}
