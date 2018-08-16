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
import java.util.ArrayList;
public class Grupo implements ArbolForm{
    
    String idGrupo;
    String etiqueta;
    Aplicable aplicable;
    ArrayList<Pregunta> preguntas;
    ArrayList<Ciclo> ciclos;
    ArrayList<Grupo> grupos;
    
    
    public Grupo(String idGrupo, String etiqueta)
    {
        this.idGrupo = idGrupo;
        this.etiqueta = etiqueta;
        this.aplicable = null;
        this.preguntas = new ArrayList<>();
        this.ciclos = new ArrayList<>();
        this.grupos = new ArrayList<>();
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
    
    public void addPregunta(Pregunta p)
    {
        this.preguntas.add(p);
    }
    
    public void addGrupo(Grupo p)
    {
        this.grupos.add(p);
    }
    
    public void addCiclo(Ciclo c)
    {
        this.ciclos.add(c);
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
