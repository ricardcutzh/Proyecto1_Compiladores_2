using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.GramaticaIrony;
using XForms.ASTTree.Interfaces;
using XForms.Simbolos;
using XForms.Objs;
using XForms.ASTTree.Valores;

namespace XForms.ASTTree.Sentencias
{
    class SwitchCase:NodoAST, Instruccion
    {
        List<NodoCaso> casos;
        NodoDefecto defecto;
        Expresion AEvaluar;

        int hayDefecto;

        public SwitchCase(NodoDefecto defecto, List<NodoCaso> casos, Expresion aEvaluar, String clase, int linea, int col):base(linea, col, clase)
        {
            this.defecto = defecto;
            this.casos = casos;
            this.AEvaluar = aEvaluar;
            hayDefecto = 1;
        }

        public SwitchCase(List<NodoCaso> casos, Expresion aEvaluar, String clase, int linea, int col):base(linea, col, clase)
        {
            this.defecto = null;
            this.casos = casos;
            this.AEvaluar = aEvaluar;
            hayDefecto = 2;
        }

        public object Ejecutar(Ambito ambito)
        {
            try
            {
                if(hayDefecto==1)///HAY POR DEFECTO
                {
                    foreach(NodoCaso caso in casos)
                    {
                        Expresion op2 = caso.getExpresion();
                        /// HAGO LA COMPARACION
                        Operacion operacion = new Operacion(this.AEvaluar, op2,Estatico.Operandos.IGUAL, linea, columna, clase);
                        Object re = operacion.getValor(ambito);
                        if(re is Boolean) /// EL RESULTADO ES BOOLEANO?
                        {
                            Boolean v = (Boolean)re;/// CASTEO A BOOLENAO
                            if(v)/// SI ES VERDADERO.... EJECUTO LAS INSTRUCCIONES
                            {
                                List<Object> instrucciones = caso.getSentencias();
                                Ambito local = new Ambito(ambito, this.clase, ambito.archivo);
                                local.tomaValoresDelAmbito(ambito, true);

                                foreach(Object o in instrucciones)
                                {
                                    if (o is Instruccion)
                                    {
                                        Instruccion aux = (Instruccion)o;
                                        Object res = aux.Ejecutar(local);
                                        if (res is NodoReturn)
                                        {
                                            return res;
                                        }
                                        else if (res is Romper)
                                        {
                                            goto HacerBreak;
                                        }
                                    }
                                    else if (o is Expresion)
                                    {
                                        Expresion exp = (Expresion)o;
                                        Object res = exp.getValor(local);
                                        if (res is NodoReturn)
                                        {
                                            return res;
                                        }
                                        else if (res is Romper)
                                        {
                                            goto HacerBreak;
                                        }
                                    }

                                }
                            }
                        }
                    }
                    goto HacerDefecto;
                    
                }
                else
                {
                    foreach (NodoCaso caso in casos)
                    {
                        Expresion op2 = caso.getExpresion();
                        /// HAGO LA COMPARACION
                        Operacion operacion = new Operacion(this.AEvaluar, op2, Estatico.Operandos.IGUAL, linea, columna, clase);
                        Object re = operacion.getValor(ambito);
                        if (re is Boolean) /// EL RESULTADO ES BOOLEANO?
                        {
                            Boolean v = (Boolean)re;/// CASTEO A BOOLENAO
                            if (v)/// SI ES VERDADERO.... EJECUTO LAS INSTRUCCIONES
                            {
                                List<Object> instrucciones = caso.getSentencias();
                                Ambito local = new Ambito(ambito, this.clase, ambito.archivo);
                                local.tomaValoresDelAmbito(ambito, true);

                                foreach (Object o in instrucciones)
                                {
                                    if (o is Instruccion)
                                    {
                                        Instruccion aux = (Instruccion)o;
                                        Object res = aux.Ejecutar(local);
                                        if (res is NodoReturn)
                                        {
                                            return res;
                                        }
                                        else if (res is Romper)
                                        {
                                            goto HacerBreak;
                                        }
                                    }
                                    else if (o is Expresion)
                                    {
                                        Expresion exp = (Expresion)o;
                                        Object res = exp.getValor(local);
                                        if (res is NodoReturn)
                                        {
                                            return res;
                                        }
                                        else if (res is Romper)
                                        {
                                            goto HacerBreak;
                                        }
                                    }

                                }
                            }
                        }
                    }

                    goto HacerBreak;
                }

                /// EJECUTAR EL DEFECTO
                HacerDefecto:
                List<object> instrucci = defecto.getSentencias();
                Ambito loca = new Ambito(ambito, this.clase, ambito.archivo);
                loca.tomaValoresDelAmbito(ambito, true);
                foreach (object o in instrucci)
                {
                    if (o is Instruccion)
                    {
                        Instruccion aux = (Instruccion)o;
                        Object res = aux.Ejecutar(loca);
                        if (res is NodoReturn)
                        {
                            return res;
                        }
                        else if (res is Romper)
                        {
                            goto HacerBreak;
                        }
                    }
                    else if (o is Expresion)
                    {
                        Expresion exp = (Expresion)o;
                        Object res = exp.getValor(loca);
                        if (res is NodoReturn)
                        {
                            return res;
                        }
                        else if (res is Romper)
                        {
                            goto HacerBreak;
                        }
                    }
                }
                HacerBreak:
                return new Nulo();
            }
            catch
            {
                TError error = new TError("Ejecucion", "Error al Ejecutar sentencias Casos | Clase: "+this.clase+" | Archivo: "+ambito.archivo, linea, columna, false);
                Estatico.ColocaError(error);
                Estatico.errores.Add(error);
            }
            return new Nulo();
        }


    }
}
