using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.ASTTree.Interfaces;
using XForms.ASTTree.Instrucciones;
using XForms.Objs;
using XForms.GramaticaIrony;
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
            if(this.Hereda)
            {
                Clase padre = Estatico.clasesDisponibles.getClase(this.Padre.ToLower());
                if (padre != null)
                {
                    Ambito tata = new Ambito(null, this.Padre.ToLower()+" "+this.idClase, ambito.archivo);
                    tata = (Ambito)padre.Ejecutar(tata);
                    ambito.HeredaAmbito(tata);
                }
                else
                {
                    TError error = new TError("Semantico", "Se intenta heredar: \"" + padre + "\" la cual no existe, desde clase: \"" + this.idClase + "\" | Clase: " + this.idClase + " | Archivo: " + ambito.archivo, 0, 0, false);
                    Estatico.ColocaError(error);
                    Estatico.errores.Add(error);
                }
            }            
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
