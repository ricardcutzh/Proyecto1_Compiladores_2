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
public class ListaOpciones {
    
    String nombreLista;
    
    ArrayList<Opcion> opciones;
    
    public ListaOpciones(String nombreLista)
    {
        this.nombreLista = nombreLista;
        this.opciones = new ArrayList<>();
    }

    public String getNombreLista() {
        return nombreLista;
    }

    public void setNombreLista(String nombreLista) {
        this.nombreLista = nombreLista;
    }
    
    public boolean addOpcion(Opcion op)
    {
        if(!yaExiste(op.getEtiqueta()))
        {
            this.opciones.add(op);
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public Opcion getOpcionAt(int index)
    {
        if(index < this.opciones.size() && index >=0)
        {
            return this.opciones.get(index);
        }
        return null;
    }
    
    private boolean yaExiste(String id)
    {
        for(Opcion op : this.opciones)
        {
            if(op.getIdOpcion().toLowerCase().equals(id.toLowerCase()))
            {
                return true;
            }
        }
        return false;
    }
    
    public ArrayList<String> listaTraducida()
    {
        ArrayList<String> listado = new ArrayList<>();
        listado.add("Opciones "+this.nombreLista+" = Nuevo Opciones();");
        for(Opcion op : this.opciones)
        {
            String opc = removeComillas(op.getIdOpcion());
            String eti = removeComillas(op.getEtiqueta());
            String mul = removeComillas(op.getMultimedia());
            
            if(mul.equals(""))
            {
                listado.add(this.nombreLista+".Agregar(\""+opc+"\",\""+eti+"\");");
            }
            else
            {
                listado.add(this.nombreLista+".Agregar(\""+opc+"\",\""+eti+"\",\""+mul+"\");");
            }
            
        }
        return listado;
    }
    
    private String removeComillas(String cadena)
    {
        cadena = cadena.replace("\"", "");
        cadena = cadena.replace("\'", "");
        return cadena;
    }
}
