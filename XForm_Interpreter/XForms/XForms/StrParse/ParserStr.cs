using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Irony.Parsing;

namespace XForms.StrParse
{
    class ParserStr
    {
        String cadenaOriginal;

        public ParserStr(String cadena)
        {
            this.cadenaOriginal = cadena;
        }


        public String reemplazaCadena()
        {
            String aux = this.cadenaOriginal;
            String pattern1 = "\\@\\{(.*?)\\}\\@";
            Match m = Regex.Match(this.cadenaOriginal, pattern1);
            while (m.Success)
            {
                String reemplazarEsto = m.Value;
                String nuevo = resultado(m.Value);
                aux = this.cadenaOriginal.Replace(reemplazarEsto, nuevo);
                m = m.NextMatch();
            }

            return aux;
        }

        private String resultado(String valorOiginal)
        {
            if (valorOiginal.Contains(":"))
            {

                String patternEst = "\\{(.*?)\\}";
                Match m2 = Regex.Match(valorOiginal, patternEst);
                String estilo = m2.Value;

                String cadena = valorOiginal.Replace(estilo, "");

                cadena = obtenerCadena(cadena);

                StrAnalizador a = new StrAnalizador(estilo);
                ParseTreeNode n = a.getRoot();
                if (n != null)
                {
                    String cad = (String)cadenaEstilo(n);

                    cad = "<label style='" + cad + "'>" + cadena + "</label>";
                    return cad;
                }
                else
                {
                    return valorOiginal;
                }
            }
            else
            {
                return valorOiginal;
            }
        }

        private String obtenerCadena(String original)
        {
            String aux = original;
            int tipo = 0;
            String nuevo = "";
            foreach (char i in aux)
            {
                if (i.Equals('{'))
                {
                    tipo = 1;
                    continue;
                }
                else if (i.Equals('}'))
                {
                    tipo = 0;
                    continue;
                }
                if (tipo == 1)
                {
                    nuevo = nuevo + i.ToString();
                }
            }
            return nuevo;
        }

        private object cadenaEstilo(ParseTreeNode raiz)
        {
            String etiqueta = raiz.ToString();
            switch (etiqueta)
            {
                case "INICIO":
                    {
                        if (raiz.ChildNodes.Count == 1)
                        {
                            return cadenaEstilo(raiz.ChildNodes.ElementAt(0));
                        }
                        break;
                    }
                case "LISTA":
                    {
                        if (raiz.ChildNodes.Count > 0)
                        {
                            String cadenaRes = "";
                            foreach (ParseTreeNode nodo in raiz.ChildNodes)
                            {
                                cadenaRes += (String)cadenaEstilo(nodo);
                            }

                            return cadenaRes;
                        }
                        break;
                    }
                case "ESTILO":
                    {
                        if (raiz.ChildNodes.Count == 2)
                        {
                            if (raiz.ChildNodes.ElementAt(0).ToString().ToLower().Contains("color"))
                            {
                                String color = raiz.ChildNodes.ElementAt(1).Token.Text;

                                return "color:" + color + ";";
                            }
                            else if (raiz.ChildNodes.ElementAt(0).ToString().ToLower().Contains("tam"))
                            {
                                String tam = raiz.ChildNodes.ElementAt(1).Token.Text;
                                return "font-size:" + tam + ";";
                            }
                        }
                        if (raiz.ChildNodes.Count == 1)
                        {
                            if (raiz.ChildNodes.ElementAt(0).ToString().ToLower().Contains("subrayado"))
                            {
                                return "text-decoration:underline;";
                            }
                            else if (raiz.ChildNodes.ElementAt(0).ToString().ToLower().Contains("negrilla"))
                            {
                                return "font-weight:bold;";
                            }
                            else if (raiz.ChildNodes.ElementAt(0).ToString().ToLower().Contains("cursiva"))
                            {
                                return "font-style: italic;";
                            }
                        }
                        break;
                    }
            }
            return "";
        }
    }
}
