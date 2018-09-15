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
public class Requerido extends Atributo implements ArbolForm{
    ArrayList<String> tabs;
    boolean valor;
    public Requerido(String cadena, boolean val) {
        super(cadena);
        this.valor = val;
    }

    @Override
    public Object traducirLocal(TablaSimbolos ts, ArrayList<String> tabs, ArrayList<TError> errores) {
        String val = "Falso";
        if(this.valor)
        {
            val = "Verdadero";
        }
        return "Booleano Requerido = "+val+";\n";
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
