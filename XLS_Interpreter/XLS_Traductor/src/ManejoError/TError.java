/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package ManejoError;

/**
 *
 * @author ricar
 */
public class TError {
    
    String tipo;
    String mensaje;
    String columna;
    String hoja;
    
    public TError(String tipo, String mensaje, String columna, String hoja)
    {
        this.tipo = tipo;
        this.mensaje = mensaje;
        this.columna = columna;
        this.hoja = hoja;
    }

    public String getTipo() {
        return tipo;
    }

    public String getMensaje() {
        return mensaje;
    }

    public String getColumna() {
        return columna;
    }

    public String getHoja() {
        return hoja;
    }
    
    
}
