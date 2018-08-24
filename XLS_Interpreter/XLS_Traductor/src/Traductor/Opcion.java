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
public class Opcion {
    String idOpcion;
    String etiqueta;
    String multimedia;
    int tipo;
    
    public Opcion(String idopcion, String etiqueta, String multi)
    {
        this.idOpcion = idopcion;
        this.etiqueta = etiqueta;
        this.multimedia = multi;
        this.tipo = 1;
    }
    
    public Opcion(String idopcion, String etiqueta)
    {
        this.idOpcion = idopcion;
        this.etiqueta = etiqueta;
        this.multimedia = "";
        this.tipo = 2;
    }

    public String getIdOpcion() {
        return idOpcion;
    }

    public void setIdOpcion(String idOpcion) {
        this.idOpcion = idOpcion;
    }

    public String getEtiqueta() {
        return etiqueta;
    }

    public void setEtiqueta(String etiqueta) {
        this.etiqueta = etiqueta;
    }

    public String getMultimedia() {
        return multimedia;
    }

    public void setMultimedia(String multimedia) {
        this.multimedia = multimedia;
    }

    public int getTipo() {
        return tipo;
    }

    public void setTipo(int tipo) {
        this.tipo = tipo;
    }
    
    
}
