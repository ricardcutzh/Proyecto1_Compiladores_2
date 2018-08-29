using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Irony.Parsing;
using System.Diagnostics;
using System.Windows.Forms;
namespace XForms.GramaticaIrony
{
    class ASTGraph
    {
        int index;

        private readonly string rutaDot = "C:\\Program Files (x86)\\Graphviz2.38\bin\\dot.exe";

        public void graficarAST(ParseTreeNode nodo)
        {
            StreamWriter archivo = new StreamWriter("Arbol.dot");
            string contenido = "Graph G {node [shape = egg];";
            index = 0;
            definirNodos(nodo, ref contenido);
            index = 0;
            enlazarNodos(nodo, 0, ref contenido);
            contenido += "}";
            archivo.Write(contenido);
            archivo.Close();
            ProcessStartInfo startInfo = new ProcessStartInfo(rutaDot);
            startInfo.Arguments = "-Tpng Arbol.dot -o Arbol.png";
            DialogResult verImagen = MessageBox.Show("¿Desea visualizar el AST de la cadena ingresada?", "Grafica AST", MessageBoxButtons.YesNo);
            if (verImagen == DialogResult.Yes)
            {
                Thread.Sleep(1000);
                Process.Start(startInfo);
                startInfo.FileName = "Arbol.png";
                Process.Start(startInfo);
            }
        }

        public void definirNodos(ParseTreeNode nodo, ref string contenido)
        {
            if (nodo != null)
            {
                contenido += "node" + index.ToString() + "[label = \"" + nodo.ToString() + "\", style = filled, color = lightblue];";
                index++;

                foreach (ParseTreeNode hijo in nodo.ChildNodes)
                {
                    definirNodos(hijo, ref contenido);
                }
            }
        }

        public void enlazarNodos(ParseTreeNode nodo, int actual, ref string contenido)
        {
            if (nodo != null)
            {
                foreach (ParseTreeNode hijo in nodo.ChildNodes)
                {
                    index++;
                    contenido += "\"node" + actual.ToString() + "\"--" + "\"node" + index.ToString() + "\"";
                    enlazarNodos(hijo, index, ref contenido);
                }
            }
        }
    }
}
