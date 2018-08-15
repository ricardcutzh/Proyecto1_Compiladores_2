/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package Tablas;

/**
 *
 * @author ricar
 */
import java.util.HashMap;

public class TablaSimbolos {
    
    HashMap<String, Simbolo> simbolos;
    
    public TablaSimbolos()
    {
        this.simbolos = new HashMap<>();
    }
    
    private boolean existeElemento(String id)
    {
        return this.simbolos.containsKey(id);
    }
    
    public boolean insertaEnTabla(String id, Simbolo s)
    {
        if(!this.existeElemento(id))
        {
            this.simbolos.put(id, s);
            return true;
        }
        return false;
    }
    
    public Simbolo getSimbolo(String id)
    {
        return this.simbolos.get(id);
    }
    
    public void printCountElements()
    {
        System.out.println("Simbolos en Tabla: "+this.simbolos.size());
    }
}
