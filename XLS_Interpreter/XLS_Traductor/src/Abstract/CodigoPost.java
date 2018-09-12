/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package Abstract;

import ManejoError.TError;
import Tablas.Simbolo;
import Tablas.TablaSimbolos;
import java.util.ArrayList;
import java.util.regex.Matcher;
import java.util.regex.Pattern;
/**
 *
 * @author ricar
 */
public class CodigoPost extends Atributo implements ArbolForm{
    ArrayList<String> tabs;
    String idPregunta;
    String padre;
    public CodigoPost(String cadena) {
        super(cadena);
    }

    public void setIdPregunta(String idPregunta) {
        this.idPregunta = idPregunta;
    }

    public void setPadre(String padre) {
        this.padre = padre;
        this.padre = "";
    }
    
    @Override
    public Object traducirLocal(TablaSimbolos ts, ArrayList<String> tabs, ArrayList<TError> errores) {
        this.tabs = tabs;
        tabula();
        if(!this.padre.equals(""))
        {
            this.padre += "().";
        }
        String cadena = "";
        String [] lineas = this.cadena.split(";");
        for(int x = 0; x < lineas.length; x++)
        {
            String line = lineas[x].replace("\n", "");
            cadena += dameTabulaciones();
            for(char c : lineas[x].toCharArray())
            {
                if(c == '@')
                {
                    cadena += this.padre+this.idPregunta+"().Respuesta";
                }
                else if(c != '\n')
                {
                    cadena += c;
                }
            }
            cadena += ";\n";
        }
        cadena = reemplazaLosIDS(cadena, ts, tabs, errores);
        destabula();
        return cadena;
    }
    
    private String reemplazaLosIDS(String cad, TablaSimbolos ts, ArrayList<String> tabs, ArrayList<TError> errores)
    {
        Pattern p = Pattern.compile("\\#\\[[a-zA-Z][a-zA-Z0-9_]*\\]");
        Matcher m = p.matcher(cad);
        while(m.find())
        {
            String group = m.group(0);
            String id = remueveRededor(group);
            if(ts.existeElemento(id))
            {
                Simbolo s = ts.getSimbolo(id);
                String pad = s.getPadre();
                String idP = s.getId();
                pad = "";//BORRO PADRE
                if(!pad.equals(""))
                {
                    pad = pad + "().";
                }
                String reemplazo = pad+idP+"().Respuesta";
                cad = cad.replace(group, reemplazo);
            }
            else
            {
                errores.add(new TError("Semantico", "Se hace referencia a un Identificador que no existe: "+id+" | Codigo Post", "Codigo Post","Encuesta"));
            }
            
        }
        return  cad;
    }

    private String remueveRededor(String cad)
    {
        cad = cad.replace("#[", "");
        cad = cad.replace("]", "");
        return cad;
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

    @Override
    public Object traducirGlobal(TablaSimbolos ts, ArrayList<String> tabs, ArrayList<TError> errores) {
        throw new UnsupportedOperationException("Not supported yet."); //To change body of generated methods, choose Tools | Templates.
    }

    

   
    
}
