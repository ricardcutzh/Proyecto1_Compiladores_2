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
    class PosCade : NodoAST, Expresion
    {

        Expresion cadena;
        Expresion posicion;

        public PosCade(Expresion cadena, Expresion posicion, String clase, int linea, int col):base(linea, col, clase)
        {
            this.cadena = cadena;
            this.posicion = posicion;
        }

        String tipo = "";
        public string getTipo(Ambito ambito)
        {
            if (tipo.Equals(""))
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
                Object cad = cadena.getValor(ambito);
                String tipoCad = cadena.getTipo(ambito);

                Object pos = posicion.getValor(ambito);
                String tipoPos = posicion.getTipo(ambito);

                if(cad is String && pos is int)
                {
                    String cadenaAux = (String)cad;
                    int posi = (int)pos;
                    if(posi > 0 && posi < cadenaAux.Length)
                    {
                        char aux = cadenaAux[posi];

                        tipo = aux.ToString();

                        return tipo;
                    }
                    else
                    {
                        TError error = new TError("Semantico", "La funcion PosCad, el parametro entero debe estar dentro de los limites: 0 y "+cadenaAux.Length+" | Clase: " + this.clase + " | Archivo: " + ambito.archivo, linea, columna, false);
                        Estatico.errores.Add(error);
                        Estatico.ColocaError(error);
                    }
                }
                else
                {
                    TError error = new TError("Semantico", "Funcion PosCad requiere parametros: (cadena, entero) y se encontro: (" + tipoCad + "," + tipoPos + ") | Clase: " + this.clase + " | Archivo: " + ambito.archivo, linea, columna, false);
                    Estatico.errores.Add(error);
                    Estatico.ColocaError(error);
                }
            }
            catch(Exception e)
            {
                TError error = new TError("Ejecucion", "Error al ejecutar la funcion Poscade | Clase: " + clase + " | Archivo: " + ambito.archivo + " | Mensaje: " + e.Message, linea, columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            return new Nulo();
        }
    }
}
