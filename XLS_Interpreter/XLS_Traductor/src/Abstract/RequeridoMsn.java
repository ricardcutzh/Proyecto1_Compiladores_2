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
public class RequeridoMsn extends Atributo implements ArbolForm{
    ArrayList<String> tabs;
    ArrayList<String> params;
    String padre, actual;
    public RequeridoMsn(String cadena) {
        super(cadena);
        this.params = new ArrayList<>();
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
    
    
    @Override
    public Object traducirLocal(TablaSimbolos ts, ArrayList<String> tabs, ArrayList<TError> errores) {
        String cad = "Cadena RequeridoMsn = ";
        StringReader stream = new StringReader(this.cadena);
        try {
            EtiqParser parse = new EtiqParser(stream);
            parse.setArchivo("Encuesta", "RequeridoMsn en Pregunta: "+this.actual);
            parse.setUp(errores, ts, padre, actual);
            cad += parse.INICIO();
            this.params = parse.getParams();
        } catch (Exception e) {
            cad = "$$EXISTIERON ERRORES EN LA RequeridoMSN: "+this.padre;
        }
        return cad+";\n";
    }

    @Override
    public Object traducirGlobal(TablaSimbolos ts, ArrayList<String> tabs, ArrayList<TError> errores) {
        throw new UnsupportedOperationException("Not supported yet."); //To change body of generated methods, choose Tools | Templates.
    }

    @Override
    public void tabula() {
        throw new UnsupportedOperationException("Not supported yet."); //To change body of generated methods, choose Tools | Templates.
    }

    @Override
    public void destabula() {
        throw new UnsupportedOperationException("Not supported yet."); //To change body of generated methods, choose Tools | Templates.
    }

    @Override
    public String dameTabulaciones() {
        throw new UnsupportedOperationException("Not supported yet."); //To change body of generated methods, choose Tools | Templates.
    }

    
    
}
