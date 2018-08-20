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
import java.io.FileInputStream;
import java.util.Iterator;
import org.apache.poi.ss.usermodel.Cell;
import org.apache.poi.ss.usermodel.Row;
import org.apache.poi.xssf.usermodel.XSSFSheet;
import org.apache.poi.xssf.usermodel.XSSFWorkbook;
import java.util.ArrayList;

public class XLSLector {
    private String CADINI_FIN = "%%";
    private String DELIMITADOR = ";";
    String ruta;
    ArrayList<TablaExcel> tablas;

    public XLSLector(String ruta) {
        //CONSTRUCTOR
        this.ruta = ruta;
        this.tablas = new ArrayList<>();
    }

    //LEE EL ARCHIVO DE LA RUTA PROPUESTA
    public boolean leerArchivoXLS() {
        try {
            FileInputStream f = new FileInputStream(this.ruta);
            //OBTENIENDO EL LIBRO DEL EXCEL
            XSSFWorkbook libro = new XSSFWorkbook(f);
            for (int x = 0; x < 4; x++) {
                if (existeHoja(x, libro)) {
                    XSSFSheet hoja = libro.getSheetAt(x);
                    if (hoja != null) {
                        switch (hoja.getSheetName().toLowerCase()) {
                            case "encuesta": {
                                leerEncuesta(hoja);
                                break;
                            }
                            case "opciones": {
                                leerOpciones(hoja);
                                break;
                            }
                            case "configuracion": {
                                leerConfiguraciones(hoja);
                                break;
                            }
                        }
                    }
                }
            }
            return true;
        } catch (Exception e) {
            System.err.println("Error al Abrir el XLS: " + e.getMessage());
            return false;
        }
    }

    private boolean existeHoja(int indice, XSSFWorkbook libro) {
        try {
            XSSFSheet hoja = libro.getSheetAt(indice);
            return true;
        } catch (Exception e) {
            return false;
        }
    }

    public void leerTabla(XSSFSheet hoja, TablaExcel tab) {
        Row fila;
        Cell celda;
        Iterator<Row> filas = hoja.iterator();
        Iterator<Cell> celdas;
        int titulo = 0;
        int x = 0;
        while (filas.hasNext()) {
            fila = filas.next();
            //AQUI ME ENCARGO DE CONTAR CUANTOS TITULOS HAY
            if(titulo == 0)
            {
                 x = fila.getLastCellNum();
            }
            //ASI NO TENGO CLAVOS
            /////////////////////////////
            for (int y = 0; y < x; y++) {
                celda = fila.getCell(y);
                if (celda == null) {
                    //SI ES VACIA LA LLENO CON NADA
                    tab.getColumnaEn(y).addValor("NULL");
                } else {
                    //DE LO CONTRARIO LA METO
                    if (titulo == 0) {
                        try {
                            tab.AgregarNodoColumna(celda.getStringCellValue().toLowerCase().replace(" ", ""));
                        } catch (Exception e) {
                            tab.AgregarNodoColumna(String.valueOf(celda.getNumericCellValue()));
                        }
                    } else if(!celda.getStringCellValue().equals("")) {
                        try
                        {
                            tab.getColumnaEn(y).addValor(celda.getStringCellValue().toLowerCase());
                        }
                        catch(Exception e)
                        {
                            tab.getColumnaEn(y).addValor(String.valueOf(celda.getNumericCellValue()));
                        }
                    }
                    else
                    {
                        tab.getColumnaEn(y).addValor("NULL");
                    }
                }
            }
            ///////////////////////////////
            titulo++;
        }
    }

    //VA LEER LA ENCUESTA
    private void leerEncuesta(XSSFSheet hoja) {
        try {
            TablaExcel encs = new TablaExcel("Encuesta");
            leerTabla(hoja, encs);
            //encs.printTablaExcel();
            this.tablas.add(encs);
        } catch (Exception e) {
            System.err.println("Error: leer Encuesta: " + e.getMessage());
        }

    }

    //VA A LEER LAS OPCIONES
    private void leerOpciones(XSSFSheet hoja) {
        try {
            TablaExcel opcs = new TablaExcel("Opciones");
            leerTabla(hoja, opcs);
            //opcs.printTablaExcel();
            this.tablas.add(opcs);
        } catch (Exception e) {
            System.err.println("Error: leer Opciones: " + e.getMessage());
        }

    }

    //VAL LEER LAS CONFIGURACIONES
    private void leerConfiguraciones(XSSFSheet hoja) {
        try {
            TablaExcel conf = new TablaExcel("Configuraciones");
            leerTabla(hoja, conf);
            //conf.printTablaExcel();
            this.tablas.add(conf);
        } catch (Exception e) {
            System.err.println("Error: leer Configuraciones: " + e.getMessage());
        }

    }

    //BUSCA LAS TABLAS DE EXCEL CARGADAS A MEMORIA
    private TablaExcel buscarTabla(String nombre) {
        for (TablaExcel t : this.tablas) {
            if (t.getNombreTabla().equals(nombre)) {
                return t;
            }
        }
        return null;
    }

    public String cadenaXLS() {
        String cad = "";
        TablaExcel temp = buscarTabla("Configuraciones");
        if (temp != null) {
            cad += "_Config\n{\n";
            cad += escribeConfiguraciones(temp);
            cad += "\n}";
        }
        temp = buscarTabla("Opciones");
        if (temp != null) {
            cad += "_Opciones\n{\n " + escribeOpciones(temp) + " \n}\n";
        }
        temp = buscarTabla("Encuesta");
        if (temp != null) {
            cad += "_Encuesta\n{\n "+cadenaEncuesta(temp)+" \n}\n";
        }
        return cad;
    }
    
    String error ;
    //CADENA DE LAS OPCIONES
    private String escribeOpciones(TablaExcel opcs) {
        try {
            String cad = "";
            ListaConfig config = new ListaConfig();
            // COLUMNAS QUE ME SIRVEN PARA PODER ESCRIBIR MI ARHICVO DE ENTRADA
            NodoColumna nombreL = opcs.getColumnaPorNombre("nombre_lista");
            NodoColumna nomb = opcs.getColumnaPorNombre("nombre");
            NodoColumna etiq = opcs.getColumnaPorNombre("etiqueta");
            NodoColumna mult = opcs.getColumnaPorNombre("multimedia");

            error = "";
            if (!compruebaColumnas(nombreL, nomb, etiq)) {
                int fila = 0;
                for (String s : nombreL.valoresColumna) {
                    String nb = nomb.getValorFila(fila);
                    String et = "<<"+etiq.getValorFila(fila)+">>";
                    String mu = "NULL";
                    if (mult != null) {
                        mu = mult.getValorFila(fila);
                        if(!mu.contains("NULL"))
                        {
                            mu = "<<"+mu+">>";
                        }
                    }
                    if (config.existeLista(s)) {
                        config.insertaEnExistente(s, nb, et, mu);
                    } else {
                        config.creaNuevaListaEInserta(s, nb, et, mu);
                    }
                    fila++;
                }
                cad = config.getListaConfiguracion();
            } else {
                cad += error + ";\n";
            }
            return cad;
        } catch (Exception e) {
            System.err.println("Error al Leer Opciones (escribeOpciones) "+e.getMessage());
            return "";
        }        
    }

    //CADENA DE CONFIGURACIONES
    private String escribeConfiguraciones(TablaExcel config) {
        //COLUMNA DE TITULO DE FORMULARIO
        String aux = "";
        aux += "\ttitulo_formulario: <<";
        NodoColumna temp = config.getColumnaPorNombre("titulo_formulario");
        if (temp != null) {
            for (String s : temp.valoresColumna) {
                if (!s.equals("NULL")) {
                    aux += s;
                    break;
                }
            }
        } else {
            aux += "NULL";
        }
        aux += ">>"+this.DELIMITADOR + "\n";//SIMBOLO DE TERMINACION

        //COLUMNA DE ID_FORM
        aux += "\tidform: ";
        temp = config.getColumnaPorNombre("idform");
        if (temp != null) {
            for (String s : temp.valoresColumna) {
                if (!s.equals("NULL")) {
                    aux += s;
                    break;
                }
            }
        } else {
            aux += "NULL";
        }
        aux += this.DELIMITADOR + "\n";

        //COLUMNA DE ESTILO
        aux += "\testilo: ";
        temp = config.getColumnaPorNombre("estilo");
        if (temp != null) {
            for (String s : temp.valoresColumna) {
                if (!s.equals("NULL")) {
                    aux += s;
                    break;
                }
            }
        } else {
            aux += "todo";
        }
        aux += this.DELIMITADOR + "\n";

        //COLUMNA IMPORTACIONES
        aux += "\timport:<<";
        temp = config.getColumnaPorNombre("importar");
        if (temp != null) {
            int ban = temp.valoresColumna.size();
            for (int x = 0; x < ban; x++) {
                if (!temp.valoresColumna.get(x).equals("NULL")) {
                    aux += temp.valoresColumna.get(x);
                    aux += " ";
                    /*if (!(x == ban - 1)) {
                        aux += ", ";
                    }*/
                }
            }
        } else {
            aux += "NULL";
        }
        aux +=">>";
        aux += this.DELIMITADOR + "\n";

        //COLUMNA DE CODIGO GLOBAL
        aux += "\tcodigo_global: ";
        temp = config.getColumnaPorNombre("codigo_global");
        if (temp != null) {
            aux += "<< \n\t\t\t";
            for (String s : temp.valoresColumna) {

                if (!s.equals("NULL")) {
                    aux += s + "\n";
                    aux += "\t\t\t";
                }

            }
            aux += ">>";
        } else {
            aux += "NULL";
        }
        aux += this.DELIMITADOR + "\n";

        //COLUMNA DE CODIGO PRINCIPAL
        aux += "\tcodigo_principal: ";
        temp = config.getColumnaPorNombre("codigo_principal");
        if (temp != null) {
            aux += "<< \n\t\t\t";
            for (String s : temp.valoresColumna) {

                if (!s.equals("NULL")) {
                    aux += s + "\n";
                    aux += "\t\t\t";
                }

            }
            aux += ">>";
        } else {
            aux += "NULL";
        }
        aux += this.DELIMITADOR + "\n";

        return aux;
    }

    //CADENA DE ENCUESTA
    private String cadenaEncuesta(TablaExcel encues)
    {
        EncuestaWriter escritor = new EncuestaWriter(encues);
        
        String cad = "";
        cad += escritor.escribeEncuesta();
        return cad;
    }
    
    
    private boolean compruebaColumnas(NodoColumna nL, NodoColumna nomb, NodoColumna etiqueta) {
        if (nL == null) {
            error += "\t_errorFatal: \"Columna 'nombre_lista' no definida\"";
            return true;
        }
        if (nomb == null) {
            error += "\t_errorFatal: \"Columna 'nombre' no definida\"";
            return true;
        }
        if (etiqueta == null) {
            error += "\t_errorFatal: \"Columna 'etiqueta' no definida\"";
            return true;
        }
        return false;
    }

    

}
