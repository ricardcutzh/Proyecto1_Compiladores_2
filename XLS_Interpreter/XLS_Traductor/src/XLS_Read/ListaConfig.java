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
import java.util.ArrayList;
import java.util.HashMap;
import java.util.Iterator;
import java.util.Set;
public class ListaConfig {
    HashMap<String, ArrayList<NodoOpcion>> opciones;
    
    public ListaConfig()
    {
        opciones = new HashMap<>();
    }
    
    public boolean existeLista(String nombre)
    {
        return this.opciones.containsKey(nombre);
    }
    
    public void insertaEnExistente(String lista, String nombre, String etiqueta, String multimedia)
    {
        //OBTENGO LA LISTA
        this.opciones.get(lista).add(new NodoOpcion(nombre, etiqueta, multimedia));
    }
    
    public void creaNuevaListaEInserta(String lista, String nombre, String etiqueta, String multimedia)
    {
        this.opciones.put(lista, new ArrayList<>());
        insertaEnExistente(lista, nombre, etiqueta, multimedia);
    }
    
    public String getListaConfiguracion()
    {
        String ret = "";
        Set setofKeys = this.opciones.keySet();
        Iterator iterator = setofKeys.iterator();
        while(iterator.hasNext())
        {
            String key = (String)iterator.next();
            ArrayList<NodoOpcion> opc = (ArrayList<NodoOpcion>)this.opciones.get(key);
            ret += "\t"+key+"{";
            for(NodoOpcion op : opc)
            {
                ret += "["+op.getIdNombre()+","+op.getEtiqueta()+","+op.getMultimedia()+"] ";
            }
            ret += "};\n";
        }
        return ret;
    }
}
