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
public class Ciclo implements ArbolForm{
    
    String idCiclo;
    String etiqueta;
    Repeticion rep;
    Aplicable apli;
    ArrayList<Pregunta> preguntas;
    ArrayList<Ciclo> ciclos;
    ArrayList<Grupo> grupos;
    
    ArrayList<String> tabs;
    
    public Ciclo(String idCiclo, String etiqueta)
    {
        this.etiqueta = etiqueta;
        this.idCiclo = idCiclo;
        this.rep = null;
        this.apli = null;
        this.preguntas = new ArrayList<>();
        this.ciclos = new ArrayList<>();
        this.grupos = new ArrayList<>();
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
    public Object traducirLocal(TablaSimbolos ts, ArrayList<String> tabs, ArrayList<TError> errores) {
        throw new UnsupportedOperationException("Not supported yet."); //To change body of generated methods, choose Tools | Templates.
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
