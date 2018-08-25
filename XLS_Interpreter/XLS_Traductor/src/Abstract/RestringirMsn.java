/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package Abstract;

import EtiquetaParser.EtiqParser;
import ManejoError.TError;
import Tablas.TablaSimbolos;
import java.io.StringReader;
import java.util.ArrayList;

/**
 *
 * @author ricar
 */
public class RestringirMsn extends Atributo implements ArbolForm{
    ArrayList<String> tabs;
    ArrayList<String> params;
    ArrayList<String> paramsPadre;
    String padre, actual;
    public RestringirMsn(String cadena) {
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
        return this.paramsPadre;
    }
    
    @Override
    public Object traducirLocal(TablaSimbolos ts, ArrayList<String> tabs, ArrayList<TError> errores) {
        this.params = new ArrayList<>();
        this.tabs = tabs;
        tabula();
        String cad = dameTabulaciones()+"Sino{\n";
        StringReader stream = new StringReader(this.cadena);
        try {
            EtiqParser parse = new EtiqParser(stream);
            parse.setArchivo("Encuesta", "RestringirMsn en Pregunta: "+this.actual);
            parse.setUp(errores, ts, padre, actual);
            String aux = parse.INICIO();
            ////////////////////////////////////////////////
            tabula();
            cad += dameTabulaciones()+"Mensajes("+aux+");\n";
            destabula();
            ////////////////////////////////////////////////
            this.params = parse.getParams();
            this.paramsPadre = parse.getParamsPadre();
        } catch (Exception e) {
            errores.add(new TError("Ejecucion", "Ocurrio un Error: "+e.getMessage(), "RestrigirMsn", "Encuesta"));
        }
        cad += dameTabulaciones()+"}\n";
        destabula();
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
