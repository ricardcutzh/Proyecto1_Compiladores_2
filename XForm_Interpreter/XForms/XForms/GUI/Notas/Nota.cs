using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XForms.Objs;
using XForms.ASTTree.Sentencias;

namespace XForms.GUI.Notas
{
    partial class Nota : Form
    {

        public Object salir = null;

        public Nota()
        {
            InitializeComponent();

        }

        public Nota(Pregunta p)
        {
            InitializeComponent();

            String sal = "";

            sal = Estatico.toHTMLTitle("("+p.numeroPregunta+") Nota: "+p.idPregunta);

            if (!p.etiqueta.Equals("") || !p.etiqueta.Equals(" "))
            {
                sal =  sal + Estatico.toHTMLCard("Etiqueta: "+p.etiqueta);
            }
            if(!p.sugerencia.Equals("") || !p.sugerencia.Equals(" "))
            {
                sal = sal + Estatico.toHTMLCard("Sugerencia: "+p.sugerencia);
            }

            Salida.DocumentText = Estatico.header() +sal +Estatico.footer();
        }

        private void Next_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.salir = new Romper();
            this.Close();
        }
    }
}
