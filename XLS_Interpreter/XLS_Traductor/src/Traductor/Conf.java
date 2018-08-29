/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package Traductor;

/**
 *
 * @author ricar
 */
import ASTTree.ASTNode;
import java.util.ArrayList;
import java.util.regex.Matcher;
import java.util.regex.Pattern;
public class Conf {
    
    ASTNode raiz;
    ArrayList<String> tabs;
    
    String titulo = "";
    String idform = "";
    String estilo = "";
    String importa = "";
    String principal = "";
    String global = "";
    String file = "";
    
    public Conf(ASTNode raiz, ArrayList<String> tabs, String nombreArchivo)
    {
        this.raiz = raiz;
        this.tabs = tabs;
        this.file = nombreArchivo;
    }
    
    private Object obtInfo(ASTNode raiz)
    {
        switch(raiz.getEtiqueta())
        {
            case "INICIO":
            {
                if(raiz.contarHijos()==3)
                {
                    obtInfo(raiz.getHijo(0));
                }
                break;
            }
            case"CONFIG":
            {
                if(raiz.contarHijos()>0)
                {
                    for(int x = 0; x < raiz.contarHijos(); x++)
                    {
                        obtInfo(raiz.getHijo(x));
                    }
                }
                break;
            }
            case "TITULO":
            {
                if(raiz.contarHijos()==1)
                {
                    this.titulo = raiz.getHijo(0).getEtiqueta();
                }
                break;
            }
            case "IDFORM":
            {
                if(raiz.contarHijos()==1)
                {
                    this.idform = raiz.getHijo(0).getEtiqueta();
                }
                break;
            }
            case "ESTILO":
            {
                if(raiz.contarHijos()==1)
                {
                    this.estilo = raiz.getHijo(0).getEtiqueta();
                }
                break;
            }
            case "IMPORTA":
            {
                if(raiz.contarHijos()==1)
                {
                    this.importa = raiz.getHijo(0).getEtiqueta();
                    if(this.importa.equals("NULL"))
                    {
                        this.importa = "";
                    }
                    else
                    {
                        //this.importa = componeCodigo(this.importa);
                    }
                    ArreglaImportaciones(this.importa);
                }
                break;
            }
            case "GLOBAL":
            {
                if(raiz.contarHijos()==1)
                {
                    this.global = raiz.getHijo(0).getEtiqueta();
                    if(this.global.equals("NULL"))
                    {
                        this.global = "";
                    }
                    else
                    {
                        //this.global = componeCodigo(this.global);
                    }
                }
                break;
            }
            case "PRINCIPAL":
            {
                if(raiz.contarHijos()==1)
                {
                    this.principal = raiz.getHijo(0).getEtiqueta();
                    if(this.principal.equals("NULL"))
                    {
                        this.principal = "";
                    }
                    else
                    {
                        //this.principal = componeCodigo(this.principal);
                    }
                }
                break;
            }
        }
        return null;
    }
    
    
    public String TraducirConfiguracion()
    {
        String cad = "";
        //OBTENGO LA INFO
        obtInfo(this.raiz);
        if(this.idform.equals(""))
        {
            this.idform = this.file;
        }
        if(!this.importa.equals("NULL"))
        {
            //cad += componeCodigo(this.importa);
            cad += this.importa;
        }
        cad += "\nClase "+this.idform+"{\n";
        tabula();
        if(!this.global.equals("NULL"))
        {
            cad += componeCodigo(this.global);
        }
        ///////////////////////////////////////////
        cad += dameTabulaciones()+"Principal(){\n";
        tabula();
        if(!this.principal.equals("NULL"))
        {
            cad += componeCodigo(this.principal);
        }
        cad += dameTabulaciones()+"Nuevo "+this.idform+"()";
        if(!this.estilo.equals("NULL") && !this.estilo.equals(""))
        {
            cad += "."+this.estilo+";\n";
        }
        else
        {
            cad += ";\n";
        }
        desTabula();
        cad += dameTabulaciones()+"}";
        ///////////////////////////////////////////
        
        return cad;
    }
    
    
    private void ArreglaImportaciones(String cadena)
    {
        ArrayList<String> importaciones = new ArrayList<>();
        cadena = cadena.replace("importar", "");
        cadena = cadena.replace(".xform", "");
        Pattern p = Pattern.compile("[a-zA-Z]([a-zA-Z0-9_])*");
        Matcher m = p.matcher(cadena.toLowerCase());
        while(m.find())
        {
            String group = m.group(0);
            importaciones.add("importar ("+group+".xform);\n");
        }
        this.importa = "";
        for(String s :importaciones)
        {
            this.importa += s;
        }
    }
    
    private void tabula() {
        this.tabs.add("\t");
    }

    private void desTabula() {
        if (this.tabs.size() > 0) {
            this.tabs.remove(0);
        }
    }
    
    private String dameTabulaciones() {
        String cad = "";
        for (String t : this.tabs) {
            cad += t;
        }
        return cad;
    }
    
    private String componeCodigo(String rawCode)
    {
        String aux = rawCode;
        aux = aux.replace("\t", "");
        //aux = aux.replace(" ","");
        aux = aux.replace("\n", "");
        String arr[] = aux.split(";");
        aux = "";
        for(int x = 0; x < arr.length; x++)
        {
            if(!arr[x].equals(""))
            {aux += dameTabulaciones()+arr[x]+";\n";}
            
        }
        return aux;
    }

    public ASTNode getRaiz() {
        return raiz;
    }

    public ArrayList<String> getTabs() {
        return tabs;
    }

    public String getTitulo() {
        return titulo;
    }

    public String getIdform() {
        return idform;
    }

    public String getEstilo() {
        return estilo;
    }

    public String getImporta() {
        return importa;
    }

    public String getPrincipal() {
        return principal;
    }

    public String getGlobal() {
        return global;
    }

    public String getFile() {
        return file;
    }
    
    
}
