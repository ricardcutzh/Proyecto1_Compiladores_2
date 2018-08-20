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
import ASTTree.ASTNode;
import ManejoError.TError;
import Tablas.TablaSimbolos;
import java.util.ArrayList;
public class TraductorAST {
    //RAIZ DEL ARBOL GENERADO CON JAVACC
    ASTNode raizArbol;
    //TABLA DE SIMBOLOS GENERADOS POR JAVACC
    TablaSimbolos ts;
    //ERRORES QUE SE ORIGINENE EN LA TRADUCCION DEL ARCHIVO
    ArrayList<TError> errores;
    //NOMBRE DEL ARCHIVO XLS
    String nombreArchivo;
    //
    ArrayList<String> tabs;
    
    public TraductorAST(ASTNode raiz, TablaSimbolos ts, String nombreArchivo)
    {
        this.raizArbol = raiz;
        this.ts = ts;
        this.errores = new ArrayList<>();
        this.nombreArchivo = nombreArchivo;
        this.tabs = new ArrayList<>();
    }
    
    public String traduccion()
    {
        Conf config = new Conf(raizArbol, tabs, nombreArchivo);
        String cad = "";
        cad = config.TraducirConfiguracion();//ESTE VIENE UNA TABULACION MAS
        
        
        
        //FINAL
        desTabula();
        cad += "\n}";
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
}
