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
public class Ciclo implements ArbolForm{
    
    String idCiclo;
    String etiqueta;
    Repeticion rep;
    Aplicable apli;
    
    public Ciclo(String idCiclo, String etiqueta)
    {
        this.etiqueta = etiqueta;
        this.idCiclo = idCiclo;
        this.rep = null;
        this.apli = null;
    }

    public String getIdCiclo() {
        return idCiclo;
    }

    public String getEtiqueta() {
        return etiqueta;
    }

    public void setRep(Repeticion rep) {
        this.rep = rep;
    }

    public void setApli(Aplicable apli) {
        this.apli = apli;
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
