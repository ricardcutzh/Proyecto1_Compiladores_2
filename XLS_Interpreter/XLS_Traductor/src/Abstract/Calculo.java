/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package Abstract;

import Exprs.ExpParser;
import ManejoError.TError;
import Tablas.TablaSimbolos;
import java.io.StringReader;
import java.util.ArrayList;

/**
 *
 * @author ricar
 */
public class Calculo extends Atributo implements ArbolForm{
    ArrayList<String> tabs;
    ArrayList<String> params;
    ArrayList<String> paramsPadre;
    public Calculo(String cadena) {
        super(cadena);
    }
    
    String actual, padre;

    public void setActual(String actual) {
        this.actual = actual;
    }

    public void setPadre(String padre) {
        this.padre = padre;
    }

    public ArrayList<String> getParams() {
        return params;
    }
    
    public ArrayList<String> getParamspPadre()
    {
        return this.paramsPadre;
    }
    
    String tipoCalcular = "";
    public String getTipoCalcular()
    {
       return tipoCalcular;
    }
    @Override
    public Object traducirLocal(TablaSimbolos ts, ArrayList<String> tabs, ArrayList<TError> errores) {
        this.tabs = tabs;
        String cad = "";
        StringReader stream = new StringReader(this.cadena);
        try {
            cad += dameTabulaciones()+"publico vacio Calcular(){\n";
            ////////////////////////////////////////////////////////////////////
            tabula();
            ExpParser exp = new ExpParser(stream);
            exp.setUp(errores, ts, padre, actual, cadena, actual, TipoPregunta.TRADUC_1);
            String aux = exp.S();
            this.tipoCalcular = exp.getTipoCalcular()+" Respuesta";
            this.params = exp.getParams();
            this.paramsPadre = exp.getParamsPadre();
            cad += dameTabulaciones()+"Respuesta = "+aux+";\n";
            destabula();
            ////////////////////////////////////////////////////////////////////
            cad += dameTabulaciones()+"}\n";
        } catch (Exception e) {
            errores.add(new TError("Ejecucion", "Ocurrio un error en el parser en Calcular: "+e.getMessage(), "Calculo", "Encuesta"));
        }
        return cad;
    }

    @Override
    public Object traducirGlobal(TablaSimbolos ts, ArrayList<String> tabs, ArrayList<TError> errores) {
        throw new UnsupportedOperationException("Not supported yet."); //To change body of generated methods, choose Tools | Templates.
    }

    @Override
    public void tabula() {
        this.tabs.add("\t");
    }

    @Override
    public void destabula() {
        if (this.tabs.size() > 0) {
            this.tabs.remove(0);
        }
    }

    @Override
    public String dameTabulaciones() {
        String cad = "";
        for (String t : this.tabs) {
            cad += t;
        }
        return cad;
    }

    

    
    
}
