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
import java.util.HashMap;
public class Pregunta implements ArbolForm{
    
    //ATRIBUTOS FIJOS
    String Idpregunta;
    String Etiqueta;
    int Tipo;
    
    //ATRIBUTOS OPCIONALES
    HashMap<String, Atributo> atributos;
    
    public Pregunta(String idpregunta, String etiqueta, int tipo)
    {
        this.Idpregunta = idpregunta;
        this.Etiqueta = etiqueta;
        this.Tipo = tipo;
        this.atributos = new HashMap<>();
    }

    public String getIdpregunta() {
        return Idpregunta;
    }

    public String getEtiqueta() {
        return Etiqueta;
    }

    public int getTipo() {
        return Tipo;
    }
    
    public boolean addAtributo(Atributo att, String nombre)
    {
        if(!this.atributos.containsKey(nombre))
        {
            this.atributos.put(nombre, att);
            return true;
        }
        else
        {
            return false;
        }
    }

    @Override
    public Object traducirLocal() {
        throw new UnsupportedOperationException("Not supported yet."); //To change body of generated methods, choose Tools | Templates.
    }

    @Override
    public Object traducirGlobal() {
        throw new UnsupportedOperationException("Not supported yet."); //To change body of generated methods, choose Tools | Templates.
    }
    
}
