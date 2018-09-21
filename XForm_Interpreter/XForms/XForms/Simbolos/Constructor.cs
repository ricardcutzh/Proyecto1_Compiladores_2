using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.ASTTree.Interfaces;
using XForms.Objs;
using XForms.GramaticaIrony;
using XForms.ASTTree.Sentencias;
namespace XForms.Simbolos
{
    class Constructor : NodoAST, Instruccion
    {
        public List<NodoParametro> parametros;
        List<Instruccion> instrucciones;

        public Constructor(List<NodoParametro> parametros, List<Instruccion>instrucciones, int linea, int col, String clase):base(linea, col, clase)
        {
            this.parametros = parametros;
            this.instrucciones = instrucciones;
        }

        public Ambito seteaParametrosLocales(Ambito m, List<Object> valores)
        {
            try
            {
                if(valores.Count == this.parametros.Count)
                {
                    for(int x = 0; x < valores.Count; x++)
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
                TError error = new TError("Ejecucion", "Error al ejecutar los parametros en clase: "+this.clase, this.linea, this.columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            return m;
        }

        public object Ejecutar(Ambito ambito)
        {
            try
            {
                foreach(Instruccion i in this.instrucciones)
                {
                    i.Ejecutar(ambito);
                }
            }
            catch(Exception e)
            {
                TError error = new TError("Ejecucion", "Ejecutando Constructor: " + this.clase + " | Archivo: " + ambito.archivo+" | Error: "+e.Message, this.linea, this.columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            return null;
        }

        public override string ToString()
        {
            String cad = "";
            cad += "| Constructor | Numero de Parametros: " + this.parametros.Count;
            return cad;
        }
    }
}
