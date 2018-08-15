/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package ManejoError;

/**
 *
 * @author ricar
 */
import java.io.BufferedWriter;
import java.io.File;
import java.io.FileWriter;
import java.io.IOException;
import java.util.ArrayList;

public class ReporteError {
    String path;
    ArrayList<TError> errores;
    
    
    
    public ReporteError(String path, ArrayList<TError> errores)
    {
        this.path = path;
        this.errores = errores;
    }
    
    public boolean writeReport()
    {
        try 
        {
            File repo = new File(this.path);
            repo.createNewFile();
            FileWriter flw = new FileWriter(repo);
            BufferedWriter buffw = new BufferedWriter(flw);
            escribeEncabezado(buffw);
            escribeTabla(buffw);
            escribeFooter(buffw);
            buffw.close();
            return true;
        } catch (Exception e) 
        {
            return false;
        }
    }
    
    private void escribeEncabezado(BufferedWriter buff) throws IOException
    {
        buff.write("<html lang=\"en\">\n");
        buff.write("<head>\n");
        buff.write("<meta charset=\"utf-8\">\n");
        buff.write("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1, shrink-to-fit=no\">\n");
        buff.write("<link rel=\"stylesheet\" href=\"https://stackpath.bootstrapcdn.com/bootstrap/4.1.3/css/bootstrap.min.css\" integrity=\"sha384-MCw98/SFnGE8fJT3GXwEOngsV7Zt27NXFoaoApmYm81iuXoPkFOJwJ8ERdknLPMO\" crossorigin=\"anonymous\">\n");
        buff.write("<title>Reporte de Errores XLS</title>\n");
        buff.write("</head>\n");
        buff.write("<body>\n");
        buff.write("<br>\n");
        buff.write("<div class=\"container\">\n");
        buff.write("<div class=\"jumbotron\">\n");
        buff.write("<h1 class=\"display-4\">Errores en XLS</h1>\n");
        buff.write("<p class=\"lead\">Reporte de Errores de la Entrada de XLS | Ricardo Antonio Cutz Hernandez | 201503476</p>\n");
        buff.write("</div>\n");
        buff.write("<br>\n");
    }
    
    private void escribeTabla(BufferedWriter buff) throws IOException
    {
        buff.write("<table class=\"table table-hover table-bordered\">\n");
        buff.write("<thead class=\"thead-dark\">\n");
        buff.write("<tr>\n");
        buff.write("<th scope=\"col\">No.</th>\n");
        buff.write("<th scope=\"col\">Tipo de Error</th>\n");
        buff.write("<th scope=\"col\">Detalle de Error</th>\n");
        buff.write("<th scope=\"col\">Hoja</th>");
        buff.write("<th scope=\"col\">Columna</th>\n");
        buff.write("</tr>\n");
        buff.write("</thead>\n");
        buff.write("<tbody>\n");
        int s = 1;
        for(TError e : this.errores)
        {
            buff.write("<tr>\n");
            buff.write("<td>"+s+"</td>\n");
            buff.write("<td>"+e.getTipo()+"</td>\n");
            buff.write("<td>"+e.getMensaje()+"</td>\n");
            buff.write("<td>"+e.getHoja()+"</td>\n");
            buff.write("<td>"+e.getColumna()+"</td>\n");
            buff.write("</tr>\n");
            s++;
        }
        buff.write("</tbody>\n");
        buff.write("</table>\n");
    }
    
    private void escribeFooter(BufferedWriter buff) throws IOException
    {
        buff.write("</div>\n");
        buff.write("<script src=\"https://code.jquery.com/jquery-3.3.1.slim.min.js\" integrity=\"sha384-q8i/X+965DzO0rT7abK41JStQIAqVgRVzpbzo5smXKp4YfRvH+8abtTE1Pi6jizo\" crossorigin=\"anonymous\"></script>\n");
        buff.write("<script src=\"https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.3/umd/popper.min.js\" integrity=\"sha384-ZMP7rVo3mIykV+2+9J3UJ46jBk0WLaUAdn689aCwoqbBJiSnjAK/l8WvCWPIPm49\" crossorigin=\"anonymous\"></script>\n");
        buff.write("<script src=\"https://stackpath.bootstrapcdn.com/bootstrap/4.1.3/js/bootstrap.min.js\" integrity=\"sha384-ChfqqxuZUCnJSK3+MXmPNIyE6ZbWh2IMqE241rYiqJxyMiZ6OW/JmZQ5stwEULTy\" crossorigin=\"anonymous\"></script>\n");
        buff.write("</body>\n</html>");
    }
}
