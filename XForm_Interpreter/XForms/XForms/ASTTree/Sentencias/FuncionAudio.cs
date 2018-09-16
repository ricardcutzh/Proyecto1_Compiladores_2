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
    class FuncionAudio:NodoAST, Instruccion
    {
        Expresion ruta;
        Expresion condicion;
        public FuncionAudio(Expresion ruta,Expresion condicion,String clase, int linea, int col):base(linea, col, clase)
        {
            this.ruta = ruta;
            this.condicion = condicion;
        }

        public object Ejecutar(Ambito ambito)
        {
            try
            {
                object path = ruta.getValor(ambito);
                object condi = condicion.getValor(ambito);
                if(path is String && condi is Boolean)
                {
                    //String currentDir = System.IO.Directory.GetCurrentDirectory();
                    String currentDir = Estatico.PROYECT_PATH + "\\";
                    String cad = (String)path;
                    Boolean cond = (Boolean)condi;
                    if(System.IO.File.Exists(cad))
                    {
                        XForms.GUI.Funciones.DisplayAudio audio = new GUI.Funciones.DisplayAudio(cad, cond);
                        audio.ShowDialog();
                    }
                    else if(System.IO.File.Exists(currentDir + cad))
                    {
                        XForms.GUI.Funciones.DisplayAudio audio = new GUI.Funciones.DisplayAudio(currentDir + cad, cond);
                        audio.ShowDialog();
                    }
                    else
                    {
                        TError error = new TError("Semantico", "La ruta de el Audio a desplegar no existe, Ruta: \"" + cad + "\" | Tampoco se encontro en el directorio raiz: \"" + currentDir + "\" | Clase: " + clase + " | Archivo: " + ambito.archivo, linea, columna, false);
                        Estatico.ColocaError(error);
                        Estatico.errores.Add(error);
                    }
                    
                }
                else
                {
                    TError error = new TError("Semantico", "La funcion Audio() recibe como parametros unicamente: (Cadena, Booleano) | Clase: " + clase + " | Archivo: " + ambito.archivo, linea, columna, false);
                    Estatico.ColocaError(error);
                    Estatico.errores.Add(error);
                }
            }
            catch(Exception e)
            {
                TError error = new TError("Ejecucion", "Error al ejecutar la funcion Audio() | Clase: " + clase + " | Archivo: " + ambito.archivo + " | Mensaje: " + e.Message, linea, columna, false);
                Estatico.ColocaError(error);
                Estatico.errores.Add(error);
            }
            return new Nulo();
        }
    }
}
