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
public class Grupo implements ArbolForm{
    
    String idGrupo;
    String etiqueta;
    Aplicable aplicable;
    
    public Grupo(String idGrupo, String etiqueta)
    {
        this.idGrupo = idGrupo;
        this.etiqueta = etiqueta;
        this.aplicable = null;
    }

    public void setAplicable(Aplicable aplicable) {
        this.aplicable = aplicable;
    }

    public String getEtiqueta() {
        return etiqueta;
    }

    public Aplicable getAplicable() {
        return aplicable;
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
