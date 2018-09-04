using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
using XForms.ASTTree.ASTConstructor;
using XForms.ASTTree.Instrucciones;
using System.Windows.Forms;
using XForms.Simbolos;
namespace XForms.Objs
{
    class ClasePreAnalizada
    {
        public String id { get; } //ID DE LA CLASE QUE SE IMPORTA
        public Estatico.Vibililidad vibililidad { get; } //VISIBILIDAD DE LA CLASE
        String padre { get; } /*SI ESTA CLASE HEREDA NECESITO SABER SU PADRE*/
        Boolean Hereda { get; } /*BANDERA QUE ME INDICA SI ESTA CLASE HEREDA DE ALGUNA OTRA*/
        ParseTreeNode cuerpoClase { get; } /*EL PARSE TREE NODE DE LA CASE EN SI, CONTIENE EL CUERPO DE LA CLASE*/
        public String archivoOringen { get; }

        ASTTreeConstructor constructor;
        CuerpoClase arbolClase;


        /*
         * Primer constructor de una clase que no hereda
         */

        public ClasePreAnalizada(String id, Estatico.Vibililidad vibililidad, ParseTreeNode cuerpo, String origen)
        {
            this.id = id.ToLower();//LO DEJO EN MINUSCULAS PARA EVITARME CLAVOS
            this.vibililidad = vibililidad;
            this.cuerpoClase = cuerpo;
            this.Hereda = false;
            this.padre = "";
            this.archivoOringen = origen;
        }

       

        /*
         * Segundo Constructor que define una clase que si hereda de otra
         * 
         */

        public ClasePreAnalizada(String id, Estatico.Vibililidad vibililidad, ParseTreeNode cuerpo, String padre, String origen)
        {
            this.id = id.ToLower();
            this.vibililidad = vibililidad;
            this.cuerpoClase = cuerpo;
            this.padre = padre.ToLower();
            this.Hereda = true;
            this.archivoOringen = origen;
        }

        /*
         * EL METODO SIGUIENTE ME VA  A SERVIR PARA CONSTRUIR EL AST PERSONALIZADO Y DEVOLVERME UNA CLASE EN SI
         */
        public void metodoAuxiliar()
        {
            this.constructor = new ASTTreeConstructor(this.cuerpoClase, id, archivoOringen);
            arbolClase = (CuerpoClase)constructor.ConstruyerAST();
            MessageBox.Show(Convert.ToString(arbolClase.numeroInstrucciones()));
        }

        public Clase obtenerClase()
        {
            ///ESTE SE ECARGA DE CONSTRUIR EL AST PARA EJECUTAR
            this.constructor = new ASTTreeConstructor(this.cuerpoClase, id, archivoOringen);
            arbolClase = (CuerpoClase)constructor.ConstruyerAST();

            /// CREO UN NUEVO AMBITO
            Ambito ambito = new Ambito(null, this.id, this.archivoOringen);

            ///MANDO A EJECUTAR LAS INSTRUCCIONES DEL ARBOL PARA COLOCAR TODO
            ///
            arbolClase.Ejecutar(ambito);

            ///CREO LA NUEVA CLASE PARA RETORNARLA
            Clase cls = new Clase(id, this.padre, this.Hereda, ambito, this.archivoOringen);
            return cls;
        }
        
        public String toString()
        {
            String cad = "Clase: " + this.id + " | Visibilidad: " + this.vibililidad.ToString();
            if(Hereda)
            {
                cad += " | Padre: " + this.padre;
            }
            return cad;
        }

    }
}
