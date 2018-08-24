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
import Traductor.TablaOpciones;
import java.util.ArrayList;

public class Formulario implements ArbolForm {

    ArrayList<Pregunta> preguntas;
    ArrayList<Grupo> grupos;
    ArrayList<Ciclo> ciclos;
    ArrayList<Object> elementos;

    ArrayList<String> tabs;
    TablaOpciones tOpc;

    public Formulario() {
        this.preguntas = new ArrayList<>();
        this.grupos = new ArrayList<>();
        this.ciclos = new ArrayList<>();
        this.elementos = new ArrayList<>();

    }

    public void settOpc(TablaOpciones tOpc) {
        this.tOpc = tOpc;
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

    public void addPregunta(Pregunta p) {
        this.preguntas.add(p);
        this.elementos.add(p);
    }

    public void addGrupo(Grupo p) {
        this.grupos.add(p);
        this.elementos.add(p);
    }

    public void addCiclo(Ciclo c) {
        this.ciclos.add(c);
        this.elementos.add(c);
    }

    @Override
    public Object traducirLocal(TablaSimbolos ts, ArrayList<String> tabs, ArrayList<TError> errores) {
        throw new UnsupportedOperationException("Not supported yet."); //To change body of generated methods, choose Tools | Templates.
    }

    @Override
    public Object traducirGlobal(TablaSimbolos ts, ArrayList<String> tabs, ArrayList<TError> errores) {
        try {
            this.tabs = tabs;
            String cad = dameTabulaciones() + "$$INICIA LA TRADUCCION DEL FORMULARIO\n\n";
            ///AQUI EN MEDIO IRIA LA TRADUCCION DE LAS PREGUNTAS DE FORMA GENERAL
            for (Object ob : elementos) {
                if (ob instanceof Pregunta) {
                    //cad += dameTabulaciones() + "$$TRADUCIENDO PREGUNTA...\n";
                    ((Pregunta) ob).setTablaOpciones(tOpc);
                    ((Pregunta) ob).setPadre("");
                    cad += ((Pregunta) ob).traducirLocal(ts, tabs, errores);
                } else if (ob instanceof Grupo) {
                    cad += dameTabulaciones() + "$$TRADUCIENDO GRUPO...\n";
                } else if (ob instanceof Ciclo) {
                    cad += dameTabulaciones() + "$$TRADUCIENDO CICLO...\n";
                }
            }
            cad += "\n"+dameTabulaciones() + "$$FINALIZA LA TRADUCCION DEL FORMULARIO\n";
            return cad;
        } catch (Exception e) {
            return "$$ERROR EN LA TRADUCCION DE EL FORMULARIO";
        }
    }

    @Override
    public void tabula() {
        this.tabs.add("\t");
    }

    @Override
    public void destabula() {
        if (this.tabs.size() > 0) {
            this.tabs.remove(0);
        }
    }

    @Override
    public String dameTabulaciones() {
        String cad = "";
        for (String t : this.tabs) {
            cad += t;
        }
        return cad;
    }

}
