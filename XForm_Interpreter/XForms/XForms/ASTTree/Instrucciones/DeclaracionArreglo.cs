using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.ASTTree.Interfaces;
using XForms.Simbolos;
using XForms.Objs;
using XForms.GramaticaIrony;

namespace XForms.ASTTree.Instrucciones
{
    class DeclaracionArreglo : NodoAST, Instruccion
    {

        String tipo; //TIPO ARR
        Estatico.Vibililidad visibilidad; //VISIBILIDAD
        String idArr;//ID
        int NumDim; //NUMERO DE DIMENSIONES

        //TIPO 2
        Expresion exp; //SI SE LE HACE UNA ASIGNACION A UNA EXPRESION

        //TIPO 3
        List<Object> arbolArreglo; //SI SE LE DEFINE EL ARREGLO

        //BANDERA DEL TIPO
        int typeDefinition;


        public DeclaracionArreglo(String tipo, Estatico.Vibililidad visibilidad, String id, int NumDim, int linea, int col, String clase) : base(linea, col, clase)
        {
            this.typeDefinition = 0;
            this.tipo = tipo;
            this.visibilidad = visibilidad;
            this.idArr = id;
            this.NumDim = NumDim;
        }

        public DeclaracionArreglo(String tipo, Estatico.Vibililidad visibilidad, String id, int NumDim, Expresion exp, int linea, int col, String clase) : base(linea, col, clase)
        {
            this.typeDefinition = 1;
            this.tipo = tipo;
            this.visibilidad = visibilidad;
            this.idArr = id;
            this.NumDim = NumDim;
            this.exp = exp;
        }

        public DeclaracionArreglo(String tipo, Estatico.Vibililidad visibilidad, String id, int NumDim, List<Object> arbolArreglo, int linea, int col, String clase) : base(linea, col, clase)
        {
            this.typeDefinition = 2;
            this.tipo = tipo;
            this.visibilidad = visibilidad;
            this.idArr = id;
            this.NumDim = NumDim;
            this.arbolArreglo = arbolArreglo;
        }

        public object Ejecutar(Ambito ambito)
        {
            try
            {
                switch(this.typeDefinition)
                {
                    case 0:
                        {
                            declaracionVacia(ambito);
                            break;
                        }
                    case 1:
                        {
                            declaracionApartirExp(ambito);
                            //ARREGLO COMO NUEVO ARREGLO O DE UN RESULTADO DE EXPRESION
                            break;
                        }
                    case 2:
                        {
                            declaracionApartirDeArbol(ambito);
                            //ARREGLO A PARTIR DE UN ARBOL DE OBJETOS
                            break;
                        }
                }
            }
            catch
            {

            }
            return null;
        }


        private void declaracionVacia(Ambito ambito)
        {
            if(!ambito.existeVariable(this.idArr.ToLower()))
            {
                Arreglo arr = new Arreglo(new List<object>(), new List<object>(), new List<int>(), this.NumDim, this.idArr.ToLower(), true, this.visibilidad, this.tipo);
                ambito.agregarVariableAlAmbito(this.idArr.ToLower(), arr);
            }
            else
            {
                TError error = new TError("Semantico", "Ya existe una declaracion de: \"" + this.idArr + "\" en este Ambito | Clase: " + this.clase + " | Archivo: " + ambito.archivo, this.linea, this.columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
        }

        private void declaracionApartirExp(Ambito ambito)
        {
            if (!ambito.existeVariable(this.idArr.ToLower()))
            {
                //TENGO QUE VER SI LO QUE ME RETORNA LA EXPRESION ES UN ARREGLO Y SI HACE MATCH CON LOS DATOS QUE TENGO
                Object arr = this.exp.getValor(ambito);
                String tipoaux = this.exp.getTipo(ambito);
                if(arr is Arreglo)
                {
                    Arreglo arreglo = (Arreglo)arr;
                    
                    if(this.NumDim == arreglo.numDimensiones)
                    {
                        if(arreglo.Tipo.Equals("nuevo"))
                        {
                            arreglo.setID(this.idArr);
                            arreglo.setTipo(this.tipo.ToLower());
                            arreglo.setVisibilidad(this.visibilidad);

                            ambito.agregarVariableAlAmbito(this.idArr.ToLower(), arreglo);
                        }
                        else if(arreglo.Tipo.ToLower().Equals(this.tipo.ToLower()))
                        {
                            ambito.agregarVariableAlAmbito(this.idArr.ToLower(), arreglo);
                        }
                        else
                        {
                            TError error = new TError("Semantico", "Los tipos del arreglo no concuerdan, se esperaba: \"" + this.tipo + "\" y se econtro: \""+arreglo.Tipo.ToLower()+"\""+"  | Clase: " + this.clase + " | Archivo: " + ambito.archivo, this.linea, this.columna, false);
                            Estatico.errores.Add(error);
                            Estatico.ColocaError(error);
                        }
                    }
                    else
                    {
                        TError error = new TError("Semantico", "Para \""+this.idArr.ToLower()+"\", Las dimensiones no concuerdan se esperaba: "+this.NumDim+", dimensiones y se encontraron: "+arreglo.numDimensiones+"  | Clase: " + this.clase + " | Archivo: " + ambito.archivo, this.linea, this.columna, false);
                        Estatico.errores.Add(error);
                        Estatico.ColocaError(error);
                    }
                }
                else if(arr is Nulo)
                {
                    ambito.agregarVariableAlAmbito(this.idArr.ToLower(), new Arreglo(new List<object>(), new List<int>(), this.NumDim, this.idArr.ToLower(), true, this.visibilidad, this.tipo));
                }
                else
                {
                    TError error = new TError("Semantico","Se esperaba un arreglo como valor para: \""+this.idArr+"\", se encontro: \""+tipoaux+"\"  | Clase: " + this.clase + " | Archivo: " + ambito.archivo, this.linea, this.columna, false);
                    Estatico.errores.Add(error);
                    Estatico.ColocaError(error);
                }
            }
            else
            {
                TError error = new TError("Semantico", "Ya existe una declaracion de: \"" + this.idArr + "\" en este Ambito | Clase: " + this.clase + " | Archivo: " + ambito.archivo, this.linea, this.columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
        }


        private void declaracionApartirDeArbol(Ambito ambito)
        {
            if (!ambito.existeVariable(this.idArr.ToLower()))
            {
                GeneradorArreglo gen = new GeneradorArreglo(this.arbolArreglo);
                if(!gen.huboError)
                {
                    int numaux = gen.numDimensiones;
                    if(NumDim == gen.numDimensiones)
                    {

                        ///FALTA COMPROBAR SI TODOS LOS ELEMENTOS DEL ARREGLO SON DEL MISMO TIPO
                        ///
                        List<Object> valores = linealizadaValores(gen.linealizacion, ambito);
                        if(valores!=null)
                        {
                            Arreglo arr = new Arreglo(this.arbolArreglo, valores, gen.dimensiones, this.NumDim, this.idArr.ToLower(), true, this.visibilidad, this.tipo);
                            ambito.agregarVariableAlAmbito(this.idArr.ToLower(), arr);
                        }
                        else
                        {
                            TError error = new TError("Semantico", "El Arreglo: \"" + this.idArr + "\" no concuerda con el tipo esperado: " + this.tipo +" | Clase: " + this.clase + " | Archivo: " + ambito.archivo, this.linea, this.columna, false);
                            Estatico.errores.Add(error);
                            Estatico.ColocaError(error);
                        }
                    }
                    else
                    {
                        TError error = new TError("Semantico", "Las dimensiones declaradas para: \"" + this.idArr + "\" No concuerdan, Se esperaban: "+this.NumDim+" y se encontro: "+gen.numDimensiones+" | Clase: " + this.clase + " | Archivo: " + ambito.archivo, this.linea, this.columna, false);
                        Estatico.errores.Add(error);
                        Estatico.ColocaError(error);
                    }
                }
                else
                {
                    TError error = new TError("Semantico", "Declaracion erreonea de Arreglo: \"" + this.idArr + "\" en este Ambito | Error: "+gen.mensajesError+" | Clase: " + this.clase + " | Archivo: " + ambito.archivo, this.linea, this.columna, false);
                    Estatico.errores.Add(error);
                    Estatico.ColocaError(error);
                }
            }
            else
            {
                TError error = new TError("Semantico", "Ya existe una declaracion de: \"" + this.idArr + "\" en este Ambito | Clase: " + this.clase + " | Archivo: " + ambito.archivo, this.linea, this.columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
        }


        private List<Object> linealizadaValores(List<Object> linealizada, Ambito am)
        {
            List<Object> valores = new List<object>();
            foreach(Object o in linealizada)
            {
                if(o is Expresion)
                {
                    Expresion aux = (Expresion)o;
                    Object val = aux.getValor(am);
                    String tipo = aux.getTipo(am).ToLower();
                    if(tipo!=this.tipo.ToLower())
                    {
                        return null;
                    }
                    else
                    {
                        valores.Add(val);
                    }
                }
            }
            return valores;
        }

    }
}
