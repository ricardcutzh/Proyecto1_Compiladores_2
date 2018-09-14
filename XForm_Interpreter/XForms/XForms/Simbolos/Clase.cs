using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.ASTTree.Interfaces;
using XForms.ASTTree.Instrucciones;
using XForms.Objs;
namespace XForms.Simbolos
{
    class Clase : Instruccion
    {
        public String idClase { get; set; }
        public String Padre { get; set; }
        public Boolean Hereda { get; set; }
        public CuerpoClase AST;
        Principal Main;
        public String ArchivoOrigen;

        public Boolean tieneMain;

        public Estatico.Vibililidad visibilidad { get; set; }

        public Clase(String idclase, String padre, Boolean hereda, CuerpoClase AST, Principal main, String Archivo)
        {
            this.idClase = idclase;
            this.Padre = padre;
            this.Hereda = hereda;
            this.ArchivoOrigen = Archivo;
            this.AST = AST;
            tieneMain = false;
            if(main!=null)
            {
                this.Main = main;
                tieneMain = true;
            }
        }

        public object Ejecutar(Ambito ambito)
        {
            AST.Ejecutar(ambito);
            return ambito;
        }


        //AQUI INICIO LA EJECUCION DEL MAIN
        public void ejecutaMain(Ambito ambito)
        {
            if(this.Main!=null)
            {
                Main.Ejecutar(ambito);
            }
        }

        public override string ToString()
        {
            String clase = "Clase: " + this.idClase;
            if(Hereda)
            {
                clase += " | Padre: " + this.Padre;
            }
            if(tieneMain)
            {
                clase += " | Main: Si";
            }
            else
            {
                clase += " | Main: No";
            }
            clase += " | Archivo Origen: " + this.ArchivoOrigen;
            return clase;
        }

        public override bool Equals(object obj)
        {
            var clase = obj as Clase;
            return clase != null &&
                   idClase == clase.idClase;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
