using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.Objs;
using System.Windows.Forms;
using System.Collections;
namespace XForms.Simbolos
{
    class Ambito
    {
        public String idAmbito { get; } /*VA A SERVIR POR SI NECESITO REPORTAR UN ERROR EN UN AMBITO ESPECIFICO*/
        public Ambito Anterior { get; set; } /*VA A SERVIR PARA MOVERME DENTRO DE LOS AMBITOS BUSCANDO UNA VARIABLE*/
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

        public void removerArreglo(String id)
        {
            this.tablaVars.removerArreglo(id);
        }
        /*AQUI FALTA EL METODO PARA MANEJAR LAS FUNCIONES QUE EXISTEN EN UN AMBITO*/
        #endregion

        #region MANEJOFUNCIONES

        public Funcion GetFuncion(Clave clave)
        {
            if (this.tablaFuns.existeFuncion(clave))
            {
                return this.tablaFuns.getFuncion(clave);
            }
            return null;
        }

        public Boolean existeFuncion(Clave clave)
        {
            return this.tablaFuns.existeFuncion(clave);
        }

        public void agregarFuncionAlAmbito(Clave clave, Funcion fun)
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

        #region HEREDARAMBITO
        public void tomaValoresDelAmbito(Ambito padre, bool ignoravisibilidad)
        {
            tomaVariables(padre.tablaVars, ignoravisibilidad);
            tomaFunciones(padre.tablaFuns, ignoravisibilidad);
            tomarConstructores(padre.tablaConst);
        }

        public void tomaVariables(TablaVariables tablaPadre, bool ignoraVisibilidad)
        {
            Hashtable auxiliar = tablaPadre.variables;
            foreach(DictionaryEntry data in auxiliar)
            {
                if(data.Value is Variable)
                {
                    Variable aux = (Variable)data.Value;
                    if (this.existeVariable(aux.idSimbolo.ToLower())) { return; }
                    if(aux.Visibilidad == Estatico.Vibililidad.PUBLICO || ignoraVisibilidad)
                    {
                        this.agregarVariableAlAmbito(aux.idSimbolo.ToLower(), aux);
                    }
                }
                else if(data.Value is Arreglo)
                {
                    Arreglo aux = (Arreglo)data.Value;
                    if (this.existeVariable(aux.idSimbolo.ToLower())) { return; }
                    if(aux.Visibilidad == Estatico.Vibililidad.PUBLICO || ignoraVisibilidad)
                    {
                        this.agregarVariableAlAmbito(aux.idSimbolo, aux);
                    }
                }
            }

        }

        public void tomaFunciones(TablaFunciones tablapadre, bool ignoraVisibilidad)
        {
            Hashtable auxiliar = tablapadre.funciones;
            foreach(DictionaryEntry data in auxiliar)
            {
                if(data.Value is Funcion)
                {
                    Funcion f = (Funcion)data.Value;
                    Clave c = (Clave)data.Key;
                    if (this.existeFuncion(c)) { return; }
                    if(f.Vibililidad == Estatico.Vibililidad.PUBLICO || ignoraVisibilidad)
                    {
                        this.tablaFuns.agregarFuncion(c, f);
                    }
                }
            }
        }

        public void tomarConstructores(TablaConstructores tablaPadre)
        {
            Hashtable auxiliar = tablaPadre.constructores;
            foreach(DictionaryEntry data in auxiliar)
            {
                if(data.Value is Constructor)
                {
                    Constructor cons = (Constructor)data.Value;
                    ClaveFuncion c = (ClaveFuncion)data.Key;
                    this.tablaConst.AgregaConstrucor(c, cons);
                }
            }
        }

        
        public void HeredaAmbito(Ambito padre)
        {
            heredaAtributos(padre.tablaVars);
            heredaMetodosFunciones(padre.tablaFuns);
            heredaConstructores(padre.tablaConst);
        }

        public void heredaAtributos(TablaVariables tabla)
        {
            Hashtable auxiliar = tabla.variables;
            foreach (DictionaryEntry data in auxiliar)
            {
                if (data.Value is Variable)
                {
                    Variable aux = (Variable)data.Value;
                    if (!this.existeVariable(aux.idSimbolo.ToLower()) && (aux.Visibilidad == Estatico.Vibililidad.PUBLICO || aux.Visibilidad == Estatico.Vibililidad.PROTEGIDO) )
                    {
                        this.agregarVariableAlAmbito(aux.idSimbolo.ToLower(), aux);
                    }
                }
                else if (data.Value is Arreglo)
                {
                    Arreglo aux = (Arreglo)data.Value;
                    if (!this.existeVariable(aux.idSimbolo.ToLower()) && (aux.Visibilidad == Estatico.Vibililidad.PUBLICO || aux.Visibilidad == Estatico.Vibililidad.PROTEGIDO))
                    {
                        this.agregarVariableAlAmbito(aux.idSimbolo, aux);
                    }
                }
            }
        }

        public void heredaMetodosFunciones(TablaFunciones tabla)
        {
            Hashtable auxiliar = tabla.funciones;
            foreach(DictionaryEntry data in auxiliar)
            {
                if(data.Value is Funcion)
                {
                    Funcion f = (Funcion)data.Value;
                    Clave c = (Clave)data.Key;
                    if(!this.existeFuncion(c) && (f.Vibililidad == Estatico.Vibililidad.PUBLICO || f.Vibililidad == Estatico.Vibililidad.PROTEGIDO))
                    {
                        this.tablaFuns.agregarFuncion(c, f);
                    }
                }
            }
        }

        public void heredaConstructores(TablaConstructores tabla)
        {
            Hashtable auxiliar = tabla.constructores;
            foreach(DictionaryEntry data in auxiliar)
            {
                if(data.Value is Constructor)
                {
                    Constructor cons = (Constructor)data.Value;
                    ClaveFuncion c = (ClaveFuncion)data.Key;
                    c.idFuncion = "padre";
                    c.Tipo = "vacio";
                    this.tablaConst.AgregaConstrucor(c, cons);
                }
            }
        }
        #endregion
    }
}
