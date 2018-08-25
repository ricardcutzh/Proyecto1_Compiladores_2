/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package Abstract;

import ManejoError.TError;
import Tablas.TablaSimbolos;
import java.util.ArrayList;
import EtiquetaParser.EtiqParser;
import java.io.StringReader;
/**
 *
 * @author ricar
 */
public class Sugerencia extends Atributo implements ArbolForm{
    ArrayList<String> tabs;
    ArrayList<String> params;
    ArrayList<String> paramasPadre;
    String padre, actual;
    public Sugerencia(String cadena) {
        super(cadena);
        
    }

    public ArrayList<String> getParams() {
        return params;
    }

    public void setPadre(String padre) {
        this.padre = padre;
    }

    public void setActual(String actual) {
        this.actual = actual;
    }
    
    public ArrayList<String> getParamsPadre()
    {
        return this.paramasPadre;
    }
    
    
    @Override
    public Object traducirLocal(TablaSimbolos ts, ArrayList<String> tabs, ArrayList<TError> errores) {
        int cantidad = errores.size();
        this.params = new ArrayList<>();
        String cad = "Cadena Sugerir = ";
        StringReader stream = new StringReader(this.cadena);
        try {
            EtiqParser parse = new EtiqParser(stream);
            parse.setArchivo("Encuesta", "Sugerencia en Pregunta: "+this.actual);
            parse.setUp(errores, ts, padre, actual);
            cad += parse.INICIO();
            this.params = parse.getParams();
            this.paramasPadre = parse.getParamsPadre();
        } catch (Exception e) {
            cad = "$$EXISTIERON ERRORES EN LA Sugerir: "+this.padre;
        }
        return cad+";\n";
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
