using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.ASTTree.Interfaces;
using XForms.Simbolos;
using XForms.GramaticaIrony;
using XForms.Objs;


namespace XForms.ASTTree.Sentencias
{
    class FuncionImagen:NodoAST, Instruccion
    {

        Expresion ruta;
        Expresion condicional;

        public FuncionImagen(Expresion ruta, Expresion condicional, String clase, int linea, int col):base(linea, col, clase)
        {
            this.ruta = ruta;
            this.condicional = condicional;
        }

        public object Ejecutar(Ambito ambito)
        {
            try
            {
                
                object path = ruta.getValor(ambito);
                object condicion = condicional.getValor(ambito);
                if(path is String && condicion is Boolean)
                {
                    //String currentDir = System.IO.Directory.GetCurrentDirectory() + "\\";

                    String currentDir = Estatico.PROYECT_PATH + "\\";

                    String cad = (String)path;
                    Boolean cond = (Boolean)condicion;

                    if(System.IO.File.Exists(cad))
                    {
                        XForms.GUI.Funciones.DisplayImage im = new GUI.Funciones.DisplayImage(cad);
                        im.ShowDialog();
                    }
                    else if(System.IO.File.Exists(currentDir + cad))
                    {
                        XForms.GUI.Funciones.DisplayImage im = new GUI.Funciones.DisplayImage(currentDir + cad);
                        im.ShowDialog();
                    }
                    else
                    {
                        TError error = new TError("Semantico", "La ruta de la Imagen a desplegar no existe, Ruta: \""+cad+"\" | Tampoco se encontro en el directorio raiz: \""+currentDir+"\" | Clase: "+clase+" | Archivo: "+ambito.archivo, linea, columna, false);
                        Estatico.ColocaError(error);
                        Estatico.errores.Add(error);
                    }
                    return new Nulo();
                }
                else
                {
                    TError error = new TError("Semantico", "La funcion Imagen() recibe como parametros unicamente: (Cadena, Booleano) | Clase: "+clase+" | Archivo: "+ambito.archivo, linea, columna, false);
                    Estatico.ColocaError(error);
                    Estatico.errores.Add(error);
                }
            }
            catch(Exception e)
            {
                TError error = new TError("Ejecucion", "Error al ejecutar la funcion Imagen() | Clase: "+clase+" | Archivo: "+ambito.archivo+" | Mensaje: "+e.Message ,linea, columna, false);
                Estatico.ColocaError(error);
                Estatico.errores.Add(error);
            }
            return new Nulo();
        }
    }
}
