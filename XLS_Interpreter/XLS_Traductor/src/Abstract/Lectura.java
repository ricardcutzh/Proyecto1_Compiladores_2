/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package Abstract;

import ManejoError.TError;
import Tablas.TablaSimbolos;
import java.util.ArrayList;

/**
 *
 * @author ricar
 */
public class Lectura extends Atributo implements ArbolForm{
    boolean val;
    ArrayList<String> tabs;
    
    public Lectura(String cadena, boolean  val) {
        super(cadena);
        this.val = val;
    }

    @Override
    public Object traducirLocal(TablaSimbolos ts, ArrayList<String> tabs, ArrayList<TError> errores) {
        String v = "publico ";
        if(this.val)
        {
            v = "privado ";
        }
        return v;
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
