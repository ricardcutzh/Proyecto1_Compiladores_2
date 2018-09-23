using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.Objs;
using System.IO;

namespace XForms.SavedForms
{
    class SavedForm
    {
        String name;

        List<PreguntaAlmacenada> preguntas;

        public SavedForm(String name, List<PreguntaAlmacenada> preguntas)
        {
            this.name = name;
            this.preguntas = preguntas;
        }

        public Boolean writeForm()
        {
            try
            {
                StreamWriter sw = new StreamWriter(Directory.GetCurrentDirectory()+"\\FORMS\\"+name+".html");
                writeHeader(sw);
                writeAnswers(sw);
                writeFooter(sw);
                sw.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }


        private void writeHeader(StreamWriter sw)
        {
            try
            {
                sw.WriteLine("<!doctype html>");
                sw.WriteLine("<html lang=\"en\">");
                sw.WriteLine("<head>");
                sw.WriteLine("<meta charset=\"utf - 8\">");
                sw.WriteLine("<meta name=\"viewport\" content=\"width = device - width, initial - scale = 1, shrink - to - fit = no\">");
                sw.WriteLine("<link rel=\"stylesheet\" href=\"https://stackpath.bootstrapcdn.com/bootstrap/4.1.3/css/bootstrap.min.css\" integrity=\"sha384-MCw98/SFnGE8fJT3GXwEOngsV7Zt27NXFoaoApmYm81iuXoPkFOJwJ8ERdknLPMO\" crossorigin=\"anonymous\">");
                sw.WriteLine("<title>Formulario: "+this.name+"</title>");
                sw.WriteLine("</head>");
                sw.WriteLine("<body style='background-color:#5DADE2;'>");
                sw.WriteLine("<div class=\"container\">");
                sw.WriteLine("<br>");
                sw.WriteLine("<div class=\"jumbotron\">");
                sw.WriteLine("<h1 class=\"display - 4\">Formulario: "+this.name+"</h1>");
                sw.WriteLine("<p class=\"lead\">Respuestas almacenadas: </p>");
                sw.WriteLine("</div>");
                sw.WriteLine("<br>");

            }
            catch
            {
                return;
            }
        }


        private void writeAnswers(StreamWriter sw)
        {
            try
            {
                int x = 1;
                foreach(PreguntaAlmacenada p in this.preguntas)
                {
                    sw.WriteLine(p.toHTML(x));
                    x++;
                }
            }
            catch
            {
                return;
            }
        }


        private void writeFooter(StreamWriter sw)
        {
            try
            {
                sw.WriteLine("</div>");
                sw.WriteLine("<script src=\"https://code.jquery.com/jquery-3.3.1.slim.min.js\" integrity=\"sha384-q8i/X+965DzO0rT7abK41JStQIAqVgRVzpbzo5smXKp4YfRvH+8abtTE1Pi6jizo\" crossorigin=\"anonymous\"></script>");
                sw.WriteLine("<script src=\"https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.3/umd/popper.min.js\" integrity=\"sha384-ZMP7rVo3mIykV+2+9J3UJ46jBk0WLaUAdn689aCwoqbBJiSnjAK/l8WvCWPIPm49\" crossorigin=\"anonymous\"></script>");
                sw.WriteLine("<script src=\"https://stackpath.bootstrapcdn.com/bootstrap/4.1.3/js/bootstrap.min.js\" integrity=\"sha384-ChfqqxuZUCnJSK3+MXmPNIyE6ZbWh2IMqE241rYiqJxyMiZ6OW/JmZQ5stwEULTy\" crossorigin=\"anonymous\"></script>");
                sw.WriteLine("</body>");
                sw.WriteLine("</html>");
            }
            catch
            {
                return;
            }
        }
    }
}
