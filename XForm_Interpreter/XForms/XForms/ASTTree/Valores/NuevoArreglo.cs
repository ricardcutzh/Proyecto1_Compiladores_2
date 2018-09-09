using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.Simbolos;
using XForms.Objs;
using XForms.GramaticaIrony;
using XForms.ASTTree.Interfaces;
namespace XForms.ASTTree.Valores
{
    class NuevoArreglo : NodoAST, Expresion
    {

        String tipo;
        List<Expresion> dimensiones;

        public NuevoArreglo(String tipo, List<Expresion> dimensiones, int linea, int col, String clase) : base(linea, col, clase)
        {
            this.tipo = tipo;
            this.dimensiones = dimensiones;
        }

        public string getTipo(Ambito ambito)
        {
            return "Arreglo";
        }

        public object getValor(Ambito ambito)
        {
            try
            {
                /// OBTENGO LAS DIMENSIONES VALORES REALES
                List<int> dimensiones = (List<int>)getDimensiones(ambito);
                if(dimensiones!=null)
                {
                    int num = numElementos(dimensiones);
                    if(num>0)
                    {
                        List<Object> valores = linealizacionVals(num);

                        //return new Arreglo(valores, dimensiones, dimensiones.Count, "pendiente", true, Estatico.Vibililidad.LOCAL, "nuevo");
                        return new Arreglo(valores, dimensiones, dimensiones.Count, "pendiente", true, Estatico.Vibililidad.LOCAL, this.tipo.ToLower());
                    }
                    else
                    {
                        TError error = new TError("Semantico", "Las dimensiones del arreglo no deben de ser 0, | Clase:" + this.clase + " | Archivo: " + ambito.archivo, this.linea, this.columna, false);
                        Estatico.errores.Add(error);
                        Estatico.ColocaError(error);
                    }
                }
                else
                {
                    TError error = new TError("Semantico", "Las dimensiones del arreglo deben ser Enteros | Clase:" + this.clase + " | Archivo: " + ambito.archivo, this.linea, this.columna, false);
                    Estatico.errores.Add(error);
                    Estatico.ColocaError(error);
                }
            }
            catch (Exception e)
            {
                TError error = new TError("Ejecucion", "Error al declarar el arreglo"+e.Message+" | Clase:" + this.clase + " | Archivo: " + ambito.archivo, this.linea, this.columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            return null;
        }

        public List<Object> linealizacionVals(int numeroValores)
        {
            List<Object> valores = new List<object>();

            for (int x = 0; x < numeroValores; x++)
            {
                valores.Add(new Nulo());
            }
            return valores;
        }
        
        private List<int> getDimensiones(Ambito ambito)
        {
            List<int> dims = new List<int>();
            foreach(Expresion e in this.dimensiones)
            {
                Object valor = e.getValor(ambito);
                if(valor is int)
                {
                    dims.Add((int)valor);
                }
                else
                {
                    return null;
                }
            }
            return dims;
        }

        private int numElementos(List<int> dimensiones)
        {
            int x = 0;
            if(dimensiones.Count>0)
            {
                x = 1;
                foreach(int s in dimensiones)
                {
                    x = x * s;
                }
                return x;
            }
            return x;
        }

    }
}
