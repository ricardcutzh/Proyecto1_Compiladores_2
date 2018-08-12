/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package XLS_Read;

import java.util.ArrayList;
import jdk.nashorn.internal.parser.TokenType;

/**
 *
 * @author ricar
 */
public class EncuestaWriter {

    TablaExcel encuesta;
    String cadena_encuesta;

    String error; // SI EXISTIESE UN ERROR

    public EncuestaWriter(TablaExcel encuesta) {
        this.encuesta = encuesta;
    }

    private boolean validaQueExistanCols(NodoColumna tipo, NodoColumna idpregunta, NodoColumna etiquta) {
        if (tipo == null) {
            this.error = "_errorFatal: \"Columna tipo no definida\";";
            return false;
        }
        if (idpregunta == null) {
            this.error = "_errorFatal: \"Columna idpregunta no definida\";";
            return false;
        }
        if (etiquta == null) {
            this.error = "_errorFatal: \"Columna etiqueta no definida\";";
            return false;
        }
        return true;
    }

    ArrayList<String> tabs;
    public String escribeEncuesta() {
        tabs = new ArrayList<>();
        String cad = "";
        NodoColumna tipo = this.encuesta.getColumnaPorNombre("tipo");
        NodoColumna idp = this.encuesta.getColumnaPorNombre("idpregunta");
        NodoColumna eit = this.encuesta.getColumnaPorNombre("etiqueta");
        if (validaQueExistanCols(tipo, idp, eit)) {
            int fila = 0;
            for (String s : tipo.valoresColumna) {
                if (tipo.getValorFila(fila).equals("iniciar agrupacion")) {
                    tabula();
                    cad += dameTabulaciones() + "_grupo{\n";
                    cad += obtener_agrupacion(fila);
                    //LLAMAR AL PROCEDIMIENTO DE AGRUPACIONES
                } else if (tipo.getValorFila(fila).equals("iniciar ciclo")) {
                    tabula();
                    cad += dameTabulaciones() + "_ciclo{\n";
                    cad += obtener_ciclo(fila);
                    //LLAMAR AL PROCEDIMIENTO DEL CICLO

                } else if (tipo.getValorFila(fila).equals("finalizar agrupacion") || tipo.getValorFila(fila).equals("finalizar ciclo")) {
                    
                    cad += dameTabulaciones()+"fin: "+this.encuesta.getColumnaPorNombre("idpregunta").getValorFila(fila)+";\n";
                    cad += dameTabulaciones() + "}\n";
                    desTabula();
                } else {
                    tabula();
                    //LLAMAR AL PROCEDIMIENTO DE LA PREGUNTA
                    cad += dameTabulaciones() + "_pregunta{\n";
                    cad += obtener_pregunta(fila);
                    cad += dameTabulaciones() + "}\n";
                    desTabula();
                }
                fila++;
            }
            
        } else {
            cad += this.error;
        }
        return cad;
    }
    
    private String obtener_pregunta(int fila)
    {
        String cad = "";
        tabula();
        
        //INFORMACION FIJA
        cad += obtTipo(fila);
        cad += obtIdPregunta(fila);
        cad += obtEtiqueta(fila);
        //FIN DE LA INFO
        
        cad +=obtColumna(fila, "sugerir", "", ""); //SUGERIR
        String temp = obtColumna(fila, "requerido", "", "");
        cad += temp; //REQUERIDO
        if(!temp.equals(""))
        {
            cad += obtColumna(fila, "requeridomsn", "", "");
        }
        cad += obtColumna(fila, "pordefecto", "", "");
        cad += obtColumna(fila, "lectura", "", "");
        cad += obtColumna(fila, "calculo", "", "");
        cad += obtColumna(fila, "multimedia", "", "");
        temp = obtColumna(fila, "restringir", "", "");
        cad += temp;
        if(!temp.equals(""))
        {
            cad += obtColumna(fila ,"restringirmsn" , "", "");
        }
        cad += obtColumna(fila, "codigo_pre", "<<", ">>");
        cad += obtColumna(fila, "codigo_post", "<<", ">>");
        cad += obtColumna(fila, "aplicable", "", "");
        cad += obtColumna(fila, "repeticion", "", "");
        cad += obtColumna(fila, "apariencia", "", "");
        cad += obtColumna(fila, "parametro", "|", "|");
        desTabula();
        return cad;
    }
    
    private String obtener_ciclo(int fila)
    {
        String cad = "";
        tabula();
        String id = obtIdPregunta(fila);
        cad += id;
        cad += obtColumna(fila, "aplicable", "", "");
        cad += obtColumna(fila, "repeticion", "", "");
        //desTabula();
        return cad;
    }
    
    private String obtener_agrupacion(int fila)
    {
        String cad = "";
        tabula();
        String id = obtIdPregunta(fila);
        cad += id;
        cad += obtColumna(fila, "aplicable", "", "");
        //desTabula();
        return cad;
    }
    
    private String dameTabulaciones() {
        String cad = "";
        for (String t : this.tabs) {
            cad += t;
        }
        return cad;
    }

    private void tabula() {
        this.tabs.add("\t");
    }

    private void desTabula() {
        if (this.tabs.size() > 0) {
            this.tabs.remove(0);
        }
    }
    
    private boolean compruebaCol(String NCol)
    {
        NodoColumna aux = this.encuesta.getColumnaPorNombre(NCol);
        if(aux!=null)
        {
            return true;
        }
        return false;
    }
    
    //PROCEDIMIENTOS PARA PODER CONCATENAR LOS DATOS DE CADA UNA DE LAS 
    private String obtTipo(int fila)//TIPO
    {
        NodoColumna ti = this.encuesta.getColumnaPorNombre("tipo");
        return dameTabulaciones()+"tipo: "+ti.getValorFila(fila)+";\n";
    }
    
    private String obtIdPregunta(int fila)
    {
        NodoColumna id = this.encuesta.getColumnaPorNombre("idpregunta");
        return dameTabulaciones()+"idpregunta: "+id.getValorFila(fila)+";\n";
    }
    
    private String obtEtiqueta(int fila)
    {
        NodoColumna et = this.encuesta.getColumnaPorNombre("etiqueta");
        return dameTabulaciones() +"etiqueta: "+et.getValorFila(fila)+";\n";
    }
    
    private String obtColumna(int fila, String colum, String open, String close)
    {
        if(compruebaCol(colum))
        {
            NodoColumna aux = this.encuesta.getColumnaPorNombre(colum);
            if(!aux.getValorFila(fila).equals("NULL"))
            {
                return dameTabulaciones() +colum+": "+open+aux.getValorFila(fila)+close+";\n";
            }
            return "";
        }
        else
        {
            return "";
        }
    }
            
}
