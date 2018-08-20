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
public class Formulario implements ArbolForm{
    ArrayList<Pregunta> preguntas;
    ArrayList<Grupo> grupos;
    ArrayList<Ciclo> ciclos;
    
    public Formulario()
    {
        this.preguntas = new ArrayList<>();
        this.grupos = new ArrayList<>();
        this.ciclos = new ArrayList<>();
    }

    public ArrayList<Pregunta> getPreguntas() {
        return preguntas;
    }

    public ArrayList<Grupo> getGrupos() {
        return grupos;
    }

    public ArrayList<Ciclo> getCiclos() {
        return ciclos;
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
