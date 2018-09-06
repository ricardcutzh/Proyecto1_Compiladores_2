using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.Objs;
using System.Windows.Forms;
namespace XForms.Simbolos
{
    class Ambito
    {
        public String idAmbito { get; } /*VA A SERVIR POR SI NECESITO REPORTAR UN ERROR EN UN AMBITO ESPECIFICO*/
        public Ambito Anterior { get; } /*VA A SERVIR PARA MOVERME DENTRO DE LOS AMBITOS BUSCANDO UNA VARIABLE*/
        TablaVariables tablaVars;
        TablaFunciones tablaFuns;
        TablaConstructores tablaConst;
        public String archivo { get; }

        public Ambito(Ambito anterior, String idAmbito)
        {
            this.Anterior = anterior;
            this.idAmbito = idAmbito;
            this.tablaVars = new TablaVariables();
            this.tablaFuns = new TablaFunciones();
            this.tablaConst = new TablaConstructores();
        }

        public Ambito(Ambito anterior, String idAmbito, String archivo)
        {
            this.Anterior = anterior;
            this.idAmbito = idAmbito;
            this.archivo = archivo;
            this.tablaVars = new TablaVariables();
            this.tablaFuns = new TablaFunciones();
            this.tablaConst = new TablaConstructores();
        }


        #region MANEJOVARIABLES
        /*METODO PARA BUSCAR UNA VARIABLE DENTRO DEL AMBITO*/

        public Simbolo getSimbolo(String id)
        {
            Ambito auxiliar = this;

            while (auxiliar != null)
            {
                if (auxiliar.tablaVars.ExisteVariable(id))
                {
                    return auxiliar.tablaVars.getVariable(id);
                }

                auxiliar = auxiliar.Anterior; //ME MUEVO AL ANTERIOR
            }

            return null;//EN CASO DE NO ENCONTRAR LA VARIABLE MARCARIA UN ERROR
        }

        public void ImprimeAmbito()
        {
            String s = this.tablaVars.imprimeTabla();
            String s2 = this.tablaFuns.ImprimirTabla();
            String s3 = this.tablaConst.ImprimirTabla();
            MessageBox.Show(s + "\n" + s2 + "\n" + s3);
        }

        public Boolean existeVariable(String id)
        {
            id = id.ToLower();
            return this.tablaVars.ExisteVariable(id);
        }

        public void agregarVariableAlAmbito(String id, Simbolo vari)
        {
            this.tablaVars.agregaVariable(id, vari);
        }

        /*AQUI FALTA EL METODO PARA MANEJAR LAS FUNCIONES QUE EXISTEN EN UN AMBITO*/
        #endregion

        #region MANEJOFUNCIONES

        public Funcion GetFuncion(ClaveFuncion clave)
        {
            if (this.tablaFuns.existeFuncion(clave))
            {
                return this.tablaFuns.getFuncion(clave);
            }
            return null;
        }

        public Boolean existeFuncion(ClaveFuncion clave)
        {
            return this.tablaFuns.existeFuncion(clave);
        }

        public void agregarFuncionAlAmbito(ClaveFuncion clave, Funcion fun)
        {
            this.tablaFuns.agregarFuncion(clave, fun);
        }
        #endregion

        #region MANEJOCONSTRUCTORES
        public Constructor getConstructor(ClaveFuncion clave)
        {
            if (this.tablaConst.existeConstructor(clave))
            {
                return this.tablaConst.getConstructro(clave);
            }
            return null;
        }
        public Boolean existeConstructor(ClaveFuncion clave)
        {
            return this.tablaConst.existeConstructor(clave);
        }

        public void agregarConstructor(ClaveFuncion clave, Constructor constr)
        {
            this.tablaConst.AgregaConstrucor(clave, constr);
        }
        #endregion
    }
}
