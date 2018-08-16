/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package xls_traductor;

/**
 *
 * @author ricar
 */
import Abstract.TipoPregunta;
import Analizadores.XLSParser;
import java.io.File;
import java.io.FileInputStream;
import GUI.Interfaz;
import ManejoError.TError;
import java.util.ArrayList;
import ManejoError.ReporteError;
public class XLS_Traductor {

    /**
     * @param args the command line arguments
     */
    public static void main(String[] args) {
        // TODO code application logic here
        //Interfaz interfaz = new Interfaz();
        //interfaz.show();
        prueba();
    }
    
    public static void prueba()
    {
        ArrayList<TError> errores = new ArrayList<>();
        File f = new File("C:\\Users\\ricar\\Documents\\Universidad\\Segundo_S_2018\\Compiladores 2\\Entradas\\prJavaCC.txt");
        try {
            FileInputStream fis = new FileInputStream(f);
            XLSParser p = new XLSParser(fis);
            ASTTree.ASTNode n = p.INICIO();
            n.graficaAST(n);
            System.out.println(n.graficaAST(n));
            errores = p.getErrores();
            if(errores.size()>0)
            {
                ReporteError rep = new ReporteError("C:\\Users\\ricar\\Documents\\Universidad\\Segundo_S_2018\\Compiladores 2\\Entradas\\Reporte.html", errores);
                if(rep.writeReport())
                {
                    System.out.println("Reporte generado");
                }
            }
            
        } catch (Exception e) {
            System.err.println("ERROR AL INICIAR EL PARSER");
        }
    }
    
}
