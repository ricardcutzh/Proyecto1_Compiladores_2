using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.ASTTree.Interfaces;
using XForms.Simbolos;
using XForms.Objs;
using XForms.GramaticaIrony;

namespace XForms.ASTTree.Valores
{
    class IfSimple : NodoAST, Expresion
    {
        Expresion condicion;
        Expresion verdadero;
        Expresion falso;

        public IfSimple(Expresion condicion, Expresion verdadero, Expresion falso,String clase, int linea, int col):base(linea, col, clase)
        {
            this.condicion = condicion;
            this.verdadero = verdadero;
            this.falso = falso;
        }

        String tipo = "";
        public string getTipo(Ambito ambito)
        {
            return tipo;
        }

        public object getValor(Ambito ambito)
        {
            try
            {
                Object condi = condicion.getValor(ambito);
                String tipoCond = condicion.getTipo(ambito);
                tipo = tipoCond;
                if(condi is Boolean)
                {
                    Boolean val = (Boolean)condi;
                    if(val)
                    {
                        Object valor = verdadero.getValor(ambito);
                        tipo = verdadero.getTipo(ambito);
                        return valor;
                    }
                    else
                    {
                        Object valor = falso.getValor(ambito);
                        tipo = falso.getTipo(ambito);
                        return valor;
                    }
                }
                else
                {
                    TError error = new TError("Semantico", "Para la condicion simplificada se necesita un valor booelano y se encontro: \""+tipoCond+"\" | Clase: "+this.clase+" | Archivo: "+ambito.archivo, linea, columna, false);
                    Estatico.errores.Add(error);
                    Estatico.ColocaError(error);
                }
            }
            catch(Exception e)
            {
                TError error = new TError("Ejecucion", "Error al ejecutar la condicion Simplificada | Clase: "+this.clase+" | Archivo: "+ambito.archivo+" | Mensaje: "+e.Message, linea, columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            return new Nulo();
        }
    }
}
