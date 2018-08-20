/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package Traductor;

import ASTTree.ASTNode;
import Abstract.Formulario;
import ManejoError.TError;
import java.util.ArrayList;

/**
 *
 * @author ricar
 */
import Tablas.TablaSimbolos;
public class TraductorForm {
    
    ArrayList<String> tabs;
    ASTNode raiz;
    String file;
    Formulario f;
    TablaSimbolos ts;
    ArrayList<TError> errores;
    public TraductorForm(ASTNode raiz, ArrayList<String> tabs, String nombreArchivo, TablaSimbolos ts)
    {
        this.raiz = raiz;
        this.tabs = tabs;
        this.file = nombreArchivo;
        this.ts = ts;
        this.errores = new ArrayList<>();
    }
    
    private void obtForm()
    {
        if(this.raiz.contarHijos()>0)
        {
            for(int x = 0; x < raiz.contarHijos(); x++)
            {
                if(raiz.getHijo(x).getEtiqueta().equals("ENCUESTA"))
                {
                    f = this.raiz.getHijo(x).getForm();
                    break;
                }
            }
        }
    }
    
    public String traducirForm()
    {
        String cad = dameTabulaciones()+"\n";
        //OBTENER EL FORMULARIO
        obtForm();
        //MANDAR A LLAMAR LA CADENA DEL FORMULARIO
        if(this.f!=null)
        {
            cad += (String)this.f.traducirGlobal(ts, tabs, errores);
        }
        
        return cad;
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
}
