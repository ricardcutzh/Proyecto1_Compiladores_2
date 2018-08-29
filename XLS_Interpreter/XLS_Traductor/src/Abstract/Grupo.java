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
import Exprs.ExpParser;
import ManejoError.TError;
import Tablas.TablaSimbolos;
import Traductor.TablaOpciones;
import java.io.StringReader;
import java.util.ArrayList;
public class Grupo implements ArbolForm{
    
    String idGrupo;
    String etiqueta;
    Aplicable aplicable;
    ArrayList<Pregunta> preguntas;
    ArrayList<Ciclo> ciclos;
    ArrayList<Grupo> grupos;
    ArrayList<Object> elementos;
    ArrayList<String> tabs;
    
    //PADRE DEL GRUPO
    String padre = "";
    
    //OPCS
    TablaOpciones tOpc;
    
    public Grupo(String idGrupo, String etiqueta)
    {
        this.idGrupo = idGrupo;
        this.etiqueta = etiqueta;
        this.aplicable = null;
        this.preguntas = new ArrayList<>();
        this.ciclos = new ArrayList<>();
        this.grupos = new ArrayList<>();
        this.elementos = new ArrayList<>();
    }

    public void settOpc(TablaOpciones tOpc) {
        this.tOpc = tOpc;
    }

    public void setPadre(String padre) {
        this.padre = padre;
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
        this.elementos.add(p);
    }
    
    public void addGrupo(Grupo p)
    {
        this.grupos.add(p);
        this.elementos.add(p);
    }
    
    public void addCiclo(Ciclo c)
    {
        this.ciclos.add(c);
        this.elementos.add(c);
    }

    @Override
    public Object traducirLocal(TablaSimbolos ts, ArrayList<String> tabs, ArrayList<TError> errores) {
        String cad = "";
        String llamadaGrupo = this.idGrupo+"();\n";
        if(this.aplicable!=null)
        {
            StringReader s = new StringReader(this.aplicable.cadena);
            Exprs.ExpParser par = new ExpParser(s);
            par.setUp(errores, ts, "", this.idGrupo, "Aplicable", "Encuesta", TipoPregunta.TRADUC_2);
            try {
                String cadenaIf = "";
                String cond = par.S();
                cadenaIf += dameTabulaciones()+"Si("+cond+"){\n";
                tabula();
                cadenaIf += dameTabulaciones()+llamadaGrupo;
                destabula();
                cadenaIf += dameTabulaciones()+"}\n";
                cad = cadenaIf;
            } catch (Exception e) {
                errores.add(new TError("Ejecucion", "Error Al Ejecutar el Parser de Aplicable", "Aplicable " +idGrupo, "Encuesta"));
            }
            
        }
        else
        {
            //tabula();
            cad = dameTabulaciones() + llamadaGrupo;
            //cad = llamadaGrupo;
            //destabula();
        }
        return cad;
    }
    
    ArrayList<String> Traducciones;
    @Override
    public Object traducirGlobal(TablaSimbolos ts, ArrayList<String> tabs, ArrayList<TError> errores) {
        this.Traducciones = new ArrayList<>();
        this.tabs = tabs;
        try {
            String cad  = dameTabulaciones()+"\n";//+"$$ PREGUNTAS DEL GRUPO: "+this.idGrupo+"\n";
            for(Object ob : this.elementos)
            {
                if(ob instanceof Pregunta)
                {
                    ((Pregunta) ob).setTablaOpciones(tOpc);
                    ((Pregunta) ob).setPadre("");
                    cad += ((Pregunta) ob).traducirLocal(ts, tabs, errores);
                    //tabula();
                    Traducciones.add((String)((Pregunta) ob).traducirGlobal(ts, tabs, errores));
                    //destabula();
                }
                else if(ob instanceof  Grupo)
                {
                    Grupo g = (Grupo)ob;
                    g.setPadre("");
                    g.settOpc(tOpc);
                    cad += g.traducirGlobal(ts, tabs, errores);
                    tabula();
                    Traducciones.add((String)g.traducirLocal(ts, tabs, errores));
                    destabula();
                }
                else if(ob instanceof Ciclo)
                {
                    Ciclo c = (Ciclo)ob;
                    c.setPadre("");
                    c.settOpc(tOpc);
                    cad += c.traducirGlobal(ts, tabs, errores);
                    tabula();
                    Traducciones.add((String)c.traducirLocal(ts, tabs, errores));
                    destabula();
                }
            }
            cad += dameTabulaciones()+ "\n"; //+"$$ FIN PREGUNTAS DEL GRUPO: "+this.idGrupo+"\n";
            /////////////////////////////////////////////////////////////////////////////////////
            cad += dameTabulaciones()+"Grupo "+this.idGrupo+"{\n";
            tabula();
            cad += dameTabulaciones() +"Respuestas resp;\n";
            destabula();
            for(String s : this.Traducciones)
            {
                cad +=  s;
            }            
            cad += dameTabulaciones()+"}\n";
            /////////////////////////////////////////////////////////////////////////////////////
            return  cad;
        } catch (Exception e) {
            errores.add(new TError("Ejecucion", "Error al Traducir el Grupo: "+this.idGrupo, "Grupo: "+this.idGrupo, "Encuesta"));
        }
        return "$$ ERROR EN GRUPO: "+idGrupo+"\n";
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
