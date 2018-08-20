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
import ManejoError.TError;
import Tablas.TablaSimbolos;
import java.util.ArrayList;
public interface ArbolForm {
    
    //METODO PARA PODER EJECUTAR LA TRADUCCION
    
    public Object traducirLocal(TablaSimbolos ts, ArrayList<String> tabs, ArrayList<TError> errores);
    
    public Object traducirGlobal(TablaSimbolos ts, ArrayList<String> tabs, ArrayList<TError> errores);
    
    public void tabula();
    
    public void destabula();
    
    public String dameTabulaciones();
}
