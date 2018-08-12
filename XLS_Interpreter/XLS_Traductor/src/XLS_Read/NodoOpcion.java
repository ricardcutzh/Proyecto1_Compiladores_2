/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package XLS_Read;

/**
 *
 * @author ricar
 */
public class NodoOpcion {
    
    String idNombre;
    String etiqueta;
    String multimedia;
    public NodoOpcion(String nombre, String etiqueta, String multimedia)
    {
        this.idNombre = nombre;
        this.etiqueta = etiqueta;
        this.multimedia = multimedia;
    }

    public String getIdNombre() {
        return idNombre;
    }

    public String getEtiqueta() {
        return etiqueta;
    }

    public String getMultimedia() {
        return multimedia;
    }
    
    
}
