/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package Abstract;

/**
 *
 * @author ricar
 */
public class Lectura extends Atributo implements ArbolForm{
    boolean val;
    public Lectura(String cadena, boolean  val) {
        super(cadena);
        this.val = val;
    }

    @Override
    public Object traducirLocal() {
        throw new UnsupportedOperationException("Not supported yet."); //To change body of generated methods, choose Tools | Templates.
    }

    @Override
    public Object traducirGlobal() {
        throw new UnsupportedOperationException("Not supported yet."); //To change body of generated methods, choose Tools | Templates.
    }
    
}
