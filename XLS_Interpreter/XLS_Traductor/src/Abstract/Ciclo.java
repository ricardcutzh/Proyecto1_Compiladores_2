/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package Abstract;

import Exprs.ExpParser;
import ManejoError.TError;
import Tablas.TablaSimbolos;
import Traductor.TablaOpciones;
import java.io.StringReader;
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
    ArrayList<Object> elementos;
    
    ArrayList<String> tabs;
    
    //PADRE
    String padre = "";
    //OPCS
    TablaOpciones tOpc;
    
    public Ciclo(String idCiclo, String etiqueta)
    {
        this.etiqueta = etiqueta;
        this.idCiclo = idCiclo;
        this.rep = null;
        this.apli = null;
        this.preguntas = new ArrayList<>();
        this.ciclos = new ArrayList<>();
        this.grupos = new ArrayList<>();
        this.elementos = new ArrayList<>();
    }

    public void settOpc(TablaOpciones tOpc) {
        this.tOpc = tOpc;
    }

    public String getIdCiclo() {
        return idCiclo;
    }

    public void setPadre(String padre) {
        this.padre = padre;
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
        //String cond = "Verdadero";
        String cond = this.idCiclo+"_iter<3";
        String contenido = evaluaAplicable(ts, tabs, errores);
        try {
            if(this.rep!=null) //AQUI EVALUO LAS REPETICIONES
            {
                StringReader s = new StringReader(rep.cadena);
                Exprs.ExpParser par = new ExpParser(s);
                par.setUp(errores, ts, padre, this.idCiclo, "Repeticion: "+this.idCiclo, "Encuesta", TipoPregunta.TRADUC_2);
                cond = par.S();
                if(cond.contains(this.idCiclo+"().Respuesta"))
                {
                    cond = cond.replace(this.idCiclo+"().Respuesta", this.idCiclo + "_iter");
                }
            }
            cad += dameTabulaciones() + "Para(Entero "+this.idCiclo+"_iter = 0; "+cond+" ; "+this.idCiclo+"_iter++){\n";
            tabula();
            cad += contenido;
            destabula();
            cad +=  dameTabulaciones() + "}\n";
            
        } catch (Exception e) {
            errores.add(new TError("Ejecucion", "Error al Traducir el Ciclo: "+this.idCiclo, "Ciclo: "+this.idCiclo, "Encuesta"));
        }
        return cad;
    }
    
    private String evaluaAplicable(TablaSimbolos ts, ArrayList<String> tabs, ArrayList<TError> errores)
    {
      String cad = "";
      for(String s : this.Traducciones)
      {
          
          cad +=  s;
      }
      if(this.apli!=null)
      {
          try {
              StringReader s = new StringReader(apli.cadena);
              Exprs.ExpParser par = new ExpParser(s);
              par.setUp(errores, ts, padre, this.idCiclo, "Aplicable: "+this.idCiclo, "Encuesta", TipoPregunta.TRADUC_2);
              String cond = par.S();
              String cadenaif = dameTabulaciones()+"Si("+cond+"){\n";
              ///////////////////////////////////////////////////////
              tabula();
              cadenaif += dameTabulaciones() + cad;
              destabula();
              ////////////////////////////////////////////////////////
              cadenaif +=dameTabulaciones() +"}\n";
              cad = cadenaif;
          } catch (Exception e) {
              errores.add(new TError("Ejecucion", "Error al Traducir el Ciclo: "+this.idCiclo, "Ciclo: "+this.idCiclo, "Encuesta"));
          }
      }
      return cad;
    }
    
    ArrayList<String> Traducciones;
    @Override
    public Object traducirGlobal(TablaSimbolos ts, ArrayList<String> tabs, ArrayList<TError> errores) {
        this.tabs = tabs;
        this.Traducciones = new ArrayList<>();
        try {
            String cad = "\n";
            for(Object ob : this.elementos)
            {
                if(ob instanceof Pregunta)
                {
                    ((Pregunta) ob).setTablaOpciones(tOpc);
                    ((Pregunta) ob).setPadre("");
                    cad += ((Pregunta) ob).traducirLocal(ts, tabs, errores);
                    tabula();
                    Traducciones.add((String)((Pregunta) ob).traducirGlobal(ts, tabs, errores));
                    destabula();
                }
                else if(ob instanceof Grupo)
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
            return cad;
        } catch (Exception e) {
            errores.add(new TError("Ejecucion", "Error al Traducir el Ciclo: "+this.idCiclo, "Ciclo: "+this.idCiclo, "Encuesta"));
        }
        return "";
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
