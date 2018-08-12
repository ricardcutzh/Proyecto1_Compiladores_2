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
public class TablaExcel {
    
    
    String nombreTabla;
    ArrayList<NodoColumna> columnas;
    
    public TablaExcel(String nombreString)
    {
        this.nombreTabla = nombreString;
        columnas = new ArrayList<>();
    }

    public String getNombreTabla() {
        return nombreTabla;
    }
    
    //OBTENGO EL NODO COLUMNA POR UN INDICES
    public NodoColumna getColumnaEn(int index)
    {
        return this.columnas.get(index);
    }
    
    //OBTENGO EL NODO COLUMNAR POR SU NOMBRE
    public NodoColumna getColumnaPorNombre(String nombre)
    {
        for(NodoColumna n : columnas)
        {
            if(n.getNombreColumna().equals(nombre))
            {
                return n;
            }
        }
        return null;
    }
    
    public void AgregarNodoColumna(String nombre)
    {
        this.columnas.add(new NodoColumna(nombre));
    }
    
    public int getColums()
    {
        return this.columnas.size();
    }
    
    public int getFilas()
    {
        return this.columnas.get(0).getFilas();
    }
    
    public void printTablaExcel()
    {
        System.out.println("Hoja de : "+this.getNombreTabla());
        columnas.forEach((n) -> {
            System.out.print("| "+n.getNombreColumna()+": "+n.getFilas()+" |");
        });
        System.out.println();
    }
}
