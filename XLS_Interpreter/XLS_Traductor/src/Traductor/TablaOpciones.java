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
import java.util.ArrayList;
public class TablaOpciones {
    
    ArrayList<ListaOpciones> listados;
    
    public TablaOpciones()
    {
        this.listados = new ArrayList<>();
    }
    
    public void addListado(ListaOpciones lista)
    {
        this.listados.add(lista);
    }
    
    public ListaOpciones getListadoAt(int index)
    {
        try {
            return this.listados.get(index);
        } catch (Exception e) {
            return null;
        }
    }
    
    public ListaOpciones getListadoByID(String id)
    {
        for(ListaOpciones ls : this.listados)
        {
            if(ls.getNombreLista().toLowerCase().equals(id.toLowerCase()))
            {
                return ls;
            }
        }
        return null;
    }
}
