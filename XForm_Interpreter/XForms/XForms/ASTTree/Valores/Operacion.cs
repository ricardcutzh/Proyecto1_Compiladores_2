using System;
using XForms.ASTTree.Interfaces;
using XForms.Simbolos;
using XForms.Objs;
using System.Windows.Forms;

namespace XForms.ASTTree.Valores
{
    class Operacion : NodoAST, Expresion
    {

        Expresion operando1;
        Expresion operando2;
        Estatico.Operandos operador;
        Boolean unoSolo;

        //OPERACIONES CON DOS EXPRESIONES
        public Operacion(Expresion operando1, Expresion operando2, Estatico.Operandos operador, int linea, int columna, String clase):base(linea,columna, clase)
        {
            this.operando1 = operando1;
            this.operando2 = operando2;
            this.operador = operador;
            unoSolo = false;
        }

        /*OPERANDOS CON UNA SOLA EXPRESION*/
        public Operacion(Expresion operando1, Estatico.Operandos operador, int linea, int columna, String clase):base(linea, columna, clase)
        {
            this.operando1 = operando1;
            this.operador = operador;
            this.unoSolo = true;
        }

        /*********************************************************
         *                  CODIGOS DE TIPOS                     *
         *********************************************************
         * BOOLEAN      |  1
         * CADENA       |  2
         * ENTERO       |  3
         * DECIMAL      |  4
         * HORA         |  5
         * FECHA        |  6
         * FECHAHORA    |  7
         */
        public string getTipo(Ambito ambito)
        {
            object val = getValor(ambito);
            if (val is bool)
            {
                return "Booleano";
            }
            else if (val is string)
            {
                return "Cadena";
            }
            else if (val is int)
            {
                return "Entero";
            }
            else if (val is double)
            {
                return "Decimal";
            }
            else if (val is System.DateTime)
            {
                return "FechaHora";
            }
            else if (val is Date)
            {
                return "Fecha";
            }
            else if (val is Hour)
            {
                return "Hora";
            }

            //AQUI FALTA EL TIPO OBJETO
            return "Objeto";
        }

        public object getValor(Ambito ambito)
        {
            try
            {
                if(unoSolo)
                {

                }
                else
                {
                    ///OBTENGO EL CODIGO DE LA OPERACION
                    String codigo = getNumCodigo(operando1, ambito);
                    codigo += getNumCodigo(operando2, ambito);
                    switch(this.operador)
                    {
                        case Estatico.Operandos.SUMA:
                            {
                                return HazSuma(this.operando1, this.operando2, codigo, ambito);
                            }
                        case Estatico.Operandos.RESTA:
                            {
                                return HazResta(this.operando1, this.operando2, codigo, ambito);
                            }
                        case Estatico.Operandos.MULT:
                            {
                                return HazMultiplicacion(operando1, operando2, codigo, ambito);
                            }
                        case Estatico.Operandos.DIVI:
                            {
                                return HazDivision(operando1, operando2, codigo, ambito);
                            }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Error en la Ejecucion de la Operacion: Linea: "+this.linea+" y Columna: "+this.columna+" | En Clase: "+this.clase+" | Archivo: "+ambito.archivo, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            return null;
        }


        private String getNumCodigo(Expresion operando, Ambito ambito)
        {
            String tipo = operando.getTipo(ambito).ToLower();
            switch(tipo)
            {
                case "booleano":
                    {
                        return "1";
                    }
                case "cadena":
                    {
                        return "2";
                    }
                case "entero":
                    {
                        return "3";
                    }
                case "decimal":
                    {
                        return "4";
                    }
                case "hora":
                    {
                        return "5";
                    }
                case "fecha":
                    {
                        return "6";
                    }
                case "fechahora":
                    {
                        return "7";
                    }
            }
            return "0";
        }


        #region SUMA
        /*
         * *****************************************************************************************************************
         * *                                       TABLA DE CODIGOS DE SUMA                                                *
         * *****************************************************************************************************************
         * *               |    BOOLEANO   |     CADENA    |    ENTERO  |   DECIMAL  |    HORA   |   FECHA   |  FECHAHORA  |
         * *****************************************************************************************************************
         * |  BOOELANO     |      11       |       12      |     13     |     14     |     00    |     00    |     00      |
         * *****************************************************************************************************************
         * |  CADENA       |      21       |       22      |     23     |     24     |     00    |     00    |     00      |
         * *****************************************************************************************************************
         * |  ENTERO       |      31       |       32      |     33     |     34     |     00    |     00    |     00      |
         * *****************************************************************************************************************
         * |  DECIMAL      |      41       |       42      |     43     |     44     |     00    |     00    |     00      |
         * *****************************************************************************************************************
         * |  FECHA/HORA   |      00       |       00      |     00     |     00     |     00    |     00    |     00      |
         * *****************************************************************************************************************
         */

        private Object HazSuma(Expresion op1, Expresion op2, String codigo, Ambito ambito)
        {
            try
            {
                Object val1 = op1.getValor(ambito);
                Object val2 = op2.getValor(ambito);
                String tipo1 = op1.getTipo(ambito);
                String tipo2 = op2.getTipo(ambito);
                switch(codigo)
                {
                    case "11":
                        {
                            Boolean valor1 = (Boolean)val1;
                            Boolean valor2 = (Boolean)val2;
                            return valor1 || valor2;
                        }
                    case "12":
                        {
                            String val = "verdadero";
                            Boolean valor1 = (Boolean)val1;
                            if (!valor1) { val = "falso";}
                            String valor2 = (String)val2;
                            return val + valor2;
                        }
                    case "13":
                        {
                            int val = 1;
                            Boolean valor1 = (Boolean)val1;
                            if(!valor1) { val = 0; }
                            int valor2 = (int)val2;
                            return val + valor2;
                        }
                    case "14":
                        {
                            double val = 1;
                            Boolean valor1 = (Boolean)val1;
                            if (!valor1) { val = 0; }
                            double valor2 = (double)val2;
                            return val + valor2;
                        }
                    case "21":
                        {
                            String val = "verdadero";
                            String valor1 = (String)val1;
                            Boolean valor2 = (Boolean)val2;
                            if(!valor2) { val = "falso"; }
                            return valor1 + val;
                        }
                    case "22":
                        {
                            String valor1 = (String)val1;
                            String valor2 = (String)val2;
                            return valor1 + valor2;
                        }
                    case "23":
                        {
                            String valor1 = (String)val1;
                            int valor2 = (int)val2;
                            return valor1 + valor2;
                        }
                    case "24":
                        {
                            String valor1 = (String)val1;
                            Double valor2 = (Double)val2;
                            return valor1 + valor2;
                        }
                    case "31":
                        {
                            int valor1 = (int)val1;
                            Boolean valor2 = (Boolean)val2;
                            return valor1 + Convert.ToInt32(valor2);
                        }
                    case "32":
                        {
                            int valor1 = (int)val1;
                            String valor2 = (String)val2;
                            return valor1 + valor2;
                        }
                    case "33":
                        {
                            int valor1 = (int)val1;
                            int valor2 = (int)val2;
                            return valor1 + valor2;
                        }
                    case "34":
                        {
                            int valor1 = (int)val1;
                            double valor2 = (double)val2;
                            return valor1 + valor2;
                        }
                    case "41":
                        {
                            double valor1 = (double)val1;
                            Boolean valor2 = (Boolean)val2;
                            return valor1 + Convert.ToDouble(valor2);
                        }
                    case "42":
                        {
                            double valor1 = (double)val1;
                            String valor2 = (String)val2;
                            return valor1 + valor2;
                        }
                    case "43":
                        {
                            double valor1 = (double)val1;
                            int valor2 = (int)val2;
                            return valor1 + valor2;
                        }
                    case "44":
                        {
                            double valor1 = (double)val1;
                            double valor2 = (double)val2;
                            return valor1 + valor2;
                        }
                }
                MessageBox.Show("Error en la Ejecucion de la Operacion De Suma: Linea: " + this.linea + " y Columna: " + this.columna + " | En Clase: " + this.clase + " | Archivo: " + ambito.archivo + " | Tipos Incompatibles: \""+tipo1+"\" y \""+tipo2+"\"", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch(Exception e)
            {
                MessageBox.Show("Error en la Ejecucion de la Operacion De Suma: Linea: " + this.linea + " y Columna: " + this.columna + " | En Clase: " + this.clase + " | Archivo: " + ambito.archivo +" | Error de Ejecucion: "+e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return new Nulo();
        }
        #endregion

        #region RESTA
        /*
         * *****************************************************************************************************************
         * *                                       TABLA DE CODIGOS DE RESTA                                               *
         * *****************************************************************************************************************
         * *               |    BOOLEANO   |     CADENA    |    ENTERO  |   DECIMAL  |    HORA   |   FECHA   |  FECHAHORA  |
         * *****************************************************************************************************************
         * |  BOOELANO     |      00       |       00      |     13     |     14     |     00    |     00    |     00      |
         * *****************************************************************************************************************
         * |  CADENA       |      00       |       00      |     00     |     00     |     00    |     00    |     00      |
         * *****************************************************************************************************************
         * |  ENTERO       |      31       |       00      |     33     |     34     |     00    |     00    |     00      |
         * *****************************************************************************************************************
         * |  DECIMAL      |      41       |       00      |     43     |     44     |     00    |     00    |     00      |
         * *****************************************************************************************************************
         * |  FECHA/HORA   |      00       |       00      |     00     |     00     |     00    |     00    |     00      |
         * *****************************************************************************************************************
         */
        private Object HazResta(Expresion op1, Expresion op2, String codigo, Ambito ambito)
        {
            try
            {
                Object val1 = op1.getValor(ambito);
                Object val2 = op2.getValor(ambito);
                String tipo1 = op1.getTipo(ambito);
                String tipo2 = op2.getTipo(ambito);
                switch(codigo)
                {
                    case "13":
                        {
                            Boolean valor1 = (Boolean)val1;
                            int valor2 = (int)val2;
                            return Convert.ToInt32(valor1)-valor2;
                        }
                    case "14":
                        {
                            Boolean valor1 = (Boolean)val1;
                            Double valor2 = (Double)val2;
                            return Convert.ToDouble(valor1) - valor2;
                        }
                    case "31":
                        {
                            int valor1 = (int)val1;
                            Boolean valor2 = (Boolean)val2;
                            return valor1 - Convert.ToInt32(valor2);
                        }
                    case "33":
                        {
                            int valor1 = (int)val1;
                            int valor2 = (int)val2;
                            return valor1 - valor2;
                        }
                    case "34":
                        {
                            int valor1 = (int)val1;
                            double valor2 = (double)val2;
                            return valor1 - valor2;
                        }
                    case "41":
                        {
                            double valor1 = (double)val1;
                            Boolean valor2 = (Boolean)val2;
                            return valor1 - Convert.ToDouble(valor2);
                        }
                    case "43":
                        {
                            double valor1 = (double)val1;
                            int valor2 = (int)val2;
                            return valor1 - valor2;
                        }
                    case "44":
                        {
                            double valor1 = (double)val1;
                            double valor2 = (double)val2;
                            return valor1 - valor2;
                        }
                }
                MessageBox.Show("Error en la Ejecucion de la Operacion De Resta: Linea: " + this.linea + " y Columna: " + this.columna + " | En Clase: " + this.clase + " | Archivo: " + ambito.archivo + " | Tipos Incompatibles: \"" + tipo1 + "\" y \"" + tipo2 + "\"", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch(Exception e)
            {
                MessageBox.Show("Error en la Ejecucion de la Operacion De Resta: Linea: " + this.linea + " y Columna: " + this.columna + " | En Clase: " + this.clase + " | Archivo: " + ambito.archivo + " | Error de Ejecucion: " + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return new Nulo();
        }
        #endregion

        #region MULTIPLICACION
        /*
         * *****************************************************************************************************************
         * *                                       TABLA DE CODIGOS DE MULT                                                *
         * *****************************************************************************************************************
         * *               |    BOOLEANO   |     CADENA    |    ENTERO  |   DECIMAL  |    HORA   |   FECHA   |  FECHAHORA  |
         * *****************************************************************************************************************
         * |  BOOELANO     |      11       |       00      |     13     |     14     |     00    |     00    |     00      |
         * *****************************************************************************************************************
         * |  CADENA       |      00       |       00      |     00     |     00     |     00    |     00    |     00      |
         * *****************************************************************************************************************
         * |  ENTERO       |      31       |       00      |     33     |     34     |     00    |     00    |     00      |
         * *****************************************************************************************************************
         * |  DECIMAL      |      41       |       00      |     43     |     44     |     00    |     00    |     00      |
         * *****************************************************************************************************************
         * |  FECHA/HORA   |      00       |       00      |     00     |     00     |     00    |     00    |     00      |
         * *****************************************************************************************************************
         */
        private Object HazMultiplicacion(Expresion op1, Expresion op2, String codigo, Ambito ambito)
        {
            try
            {
                Object val1 = op1.getValor(ambito);
                Object val2 = op2.getValor(ambito);
                String tipo1 = op1.getTipo(ambito);
                String tipo2 = op2.getTipo(ambito);
                switch(codigo)
                {
                    case "11":
                        {
                            Boolean valor1 = (Boolean)val1;
                            Boolean valor2 = (Boolean)val2;
                            return valor1 && valor2;
                        }
                    case "13":
                        {
                            Boolean valor1 = (Boolean)val1;
                            int valor2 = (int)val2;
                            return Convert.ToInt32(valor1) * valor2;
                        }
                    case "14":
                        {
                            Boolean valor1 =(Boolean)val1;
                            double valor2 = (double)val2;
                            return Convert.ToDouble(valor1) * valor2;
                        }
                    case "31":
                        {
                            int valor1 = (int)val1;
                            Boolean valor2 = (Boolean)val2;
                            return valor1 * Convert.ToInt32(valor2);
                        }
                    case "33":
                        {
                            int valor1 = (int)val1;
                            int valor2 = (int)val2;
                            return valor1 * valor2;
                        }
                    case "34":
                        {
                            int valor1 = (int)val1;
                            double valor2 = (double)val2;
                            return valor1 * valor2;
                        }
                    case "41":
                        {
                            double valor1 = (double)val1;
                            Boolean valor2 = (Boolean)val2;
                            return valor1 * Convert.ToDouble(valor2);
                        }
                    case "43":
                        {
                            double valor1 = (double)val1;
                            int valor2 = (int)val2;
                            return valor1 * valor2;
                        }
                    case "44":
                        {
                            double valor1 = (double)val1;
                            double valor2 = (double)val2;
                            return valor1 * valor2;
                        }
                }
                MessageBox.Show("Error en la Ejecucion de la Operacion De Multiplicacion: Linea: " + this.linea + " y Columna: " + this.columna + " | En Clase: " + this.clase + " | Archivo: " + ambito.archivo + " | Tipos Incompatibles: \"" + tipo1 + "\" y \"" + tipo2 + "\"", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch(Exception e)
            {
                MessageBox.Show("Error en la Ejecucion de la Operacion De Multiplicacion: Linea: " + this.linea + " y Columna: " + this.columna + " | En Clase: " + this.clase + " | Archivo: " + ambito.archivo + " | Error de Ejecucion: " + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return new Nulo();
        }
        #endregion

        #region DIVISION
        /*
         * *****************************************************************************************************************
         * *                                       TABLA DE CODIGOS DE DIVI                                                *
         * *****************************************************************************************************************
         * *               |    BOOLEANO   |     CADENA    |    ENTERO  |   DECIMAL  |    HORA   |   FECHA   |  FECHAHORA  |
         * *****************************************************************************************************************
         * |  BOOELANO     |      00       |       00      |     13     |     14     |     00    |     00    |     00      |
         * *****************************************************************************************************************
         * |  CADENA       |      00       |       00      |     00     |     00     |     00    |     00    |     00      |
         * *****************************************************************************************************************
         * |  ENTERO       |      31       |       00      |     33     |     34     |     00    |     00    |     00      |
         * *****************************************************************************************************************
         * |  DECIMAL      |      41       |       00      |     43     |     44     |     00    |     00    |     00      |
         * *****************************************************************************************************************
         * |  FECHA/HORA   |      00       |       00      |     00     |     00     |     00    |     00    |     00      |
         * *****************************************************************************************************************
         */
        private Object HazDivision(Expresion op1, Expresion op2, String codigo, Ambito ambito)
        {
            try
            {
                Object val1 = op1.getValor(ambito);
                Object val2 = op2.getValor(ambito);
                String tipo1 = op1.getTipo(ambito);
                String tipo2 = op2.getTipo(ambito);
                switch(codigo)
                {
                    case "13":
                        {
                            Boolean valor1 = (Boolean)val1;
                            int valor2 = (int)val2;
                            return Convert.ToInt32(valor1) / valor2;
                        }
                    case "14":
                        {
                            Boolean valor1 = (Boolean)val1;
                            double valor2 = (double)val2;
                            return Convert.ToDouble(valor1) / valor2;
                        }
                    case "31":
                        {
                            int valor1 = (int)val1;
                            Boolean valor2 = (Boolean)val2;
                            if (valor2)
                            {
                                return valor1 / 1;
                            }
                            else;
                            {
                                return 0.0;
                            }
                        }
                    case "33":
                        {
                            try
                            {
                                int valor1 = (int)val1;
                                int valor2 = (int)val2;
                                return valor1 / valor2;
                            }
                            catch
                            {
                                MessageBox.Show("Error en la Ejecucion de la Operacion De Division: Linea: " + this.linea + " y Columna: " + this.columna + " | En Clase: " + this.clase + " | Archivo: " + ambito.archivo + " | Division Entre Cero! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return 0.0;
                            }
                        }
                    case "34":
                        {
                            try
                            {
                                int valor1 = (int)val1;
                                double valor2 = (double)val2;
                                return valor1 / valor2;
                            }
                            catch
                            {
                                MessageBox.Show("Error en la Ejecucion de la Operacion De Division: Linea: " + this.linea + " y Columna: " + this.columna + " | En Clase: " + this.clase + " | Archivo: " + ambito.archivo + " | Division Entre Cero! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return 0.0;
                            }
                        }
                    case "41":
                        {
                            double valor1 = (Double)val1;
                            return valor1;
                        }
                    case "43":
                        {
                            try
                            {
                                double valor1 = (double)val1;
                                int valor2 = (int)val2;
                                return valor1 / valor2;
                            }
                            catch
                            {
                                MessageBox.Show("Error en la Ejecucion de la Operacion De Division: Linea: " + this.linea + " y Columna: " + this.columna + " | En Clase: " + this.clase + " | Archivo: " + ambito.archivo + " | Division Entre Cero! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return 0.0;
                            }
                        }
                    case "44":
                        {
                            try
                            {
                                double valor1 = (double)val1;
                                double valor2 = (double)val2;
                                return valor1 / valor2;
                            }
                            catch
                            {
                                MessageBox.Show("Error en la Ejecucion de la Operacion De Division: Linea: " + this.linea + " y Columna: " + this.columna + " | En Clase: " + this.clase + " | Archivo: " + ambito.archivo + " | Division Entre Cero! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return 0.0;
                            }
                        }
                }
                MessageBox.Show("Error en la Ejecucion de la Operacion De Division: Linea: " + this.linea + " y Columna: " + this.columna + " | En Clase: " + this.clase + " | Archivo: " + ambito.archivo + " | Tipos Incompatibles: \"" + tipo1 + "\" y \"" + tipo2 + "\"", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch(Exception e)
            {
                MessageBox.Show("Error en la Ejecucion de la Operacion De la Divison: Linea: " + this.linea + " y Columna: " + this.columna + " | En Clase: " + this.clase + " | Archivo: " + ambito.archivo + " | Error de Ejecucion: " + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return new Nulo();
        }
        #endregion
    }
}
