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

public class NodoColumna {
    
    String nombreColumna; //NOMBRE DE LA COLUMNA
    ArrayList<String> valoresColumna; //VALORES DE LA COLUMNA
    
    public NodoColumna(String nombre)
    {
        this.nombreColumna = nombre;
        this.valoresColumna = new ArrayList<>();
    }

    public String getNombreColumna() {
        return nombreColumna;
    }
    
    public String getValorFila(int index)
    {
        return this.valoresColumna.get(index);
    }
    
    public void addValor(String valor)
    {
        this.valoresColumna.add(valor);
    }
    
    public int getFilas()
    {
        return this.valoresColumna.size();
    }
}
