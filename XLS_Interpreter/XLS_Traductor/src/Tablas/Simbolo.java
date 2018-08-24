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
import Abstract.ArbolForm;

public class Simbolo {
    
    String id;
    ArbolForm elemento;
    String tipo;
    
    String padre;
    
    public Simbolo(String id, String tipo, ArbolForm elemento)
    {
        this.id = id;
        this.tipo = tipo;
        this.elemento = elemento;
    }

    public String getPadre() {
        return padre;
    }

    public void setPadre(String padre) {
        this.padre = padre;
    }

    public String getId() {
        return id;
    }

    public ArbolForm getElemento() {
        return elemento;
    }

    public String getTipo() {
        return tipo;
    }
    
    
}
