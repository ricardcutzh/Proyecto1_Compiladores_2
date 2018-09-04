using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.ASTTree.Interfaces;
using XForms.Objs;
using XForms.Simbolos;
using System.Windows.Forms;

namespace XForms.ASTTree.Instrucciones
{
    class DeclaracionVar: NodoAST, Instruccion
    {
        Expresion expresion;
        String idVar;
        String tipo;
        Estatico.Vibililidad visibilidad;
        Boolean asignaValor;

        /*CONSRUCTOR DE UNA DECLARACION DE UNA VARIABLE*/
        public DeclaracionVar(Expresion expresio, String idvar, String tipo, Estatico.Vibililidad visibilidad, int linea, int col, String clase) : base(linea, col, clase)
        {
            this.expresion = expresio;
            this.idVar = idvar;
            this.tipo = tipo;
            this.visibilidad = visibilidad;
            this.asignaValor = true;
        }

        public DeclaracionVar(String idvar, String tipo, Estatico.Vibililidad visibilidad, int linea, int columna, String clase) : base(linea, columna, clase)
        {
            this.expresion = null;
            this.tipo = tipo;
            this.visibilidad = visibilidad;
            this.idVar = idvar;
            this.asignaValor = false;
        }

        public object Ejecutar(Ambito ambito)
        {
            try
            {
                if (this.asignaValor)
                {
                    //OBT EL VALOR
                    Object valor = this.expresion.getValor(ambito);//LLAMADA AL VALOR
                    MessageBox.Show(valor.ToString());


                }
                else
                {

                }
            }
            catch
            {
                MessageBox.Show("Error en La Ejecucion de una declaracion de Variable: \""+this.idVar+"\" en: Linea: "+this.linea+" y Columna: "+this.columna+" | En Clase: "+this.clase+" | Archivo: "+this.archivoOrigen, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;
        }
    }
}
