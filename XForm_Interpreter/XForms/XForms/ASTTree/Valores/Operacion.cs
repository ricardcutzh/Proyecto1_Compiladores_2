using System;
using XForms.ASTTree.Interfaces;
using XForms.Simbolos;
using XForms.Objs;
using System.Windows.Forms;
using XForms.GramaticaIrony;
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
        object valorAux = null;
        public string getTipo(Ambito ambito)
        {
            object val;
            if (this.valorAux!=null)
            {
                val = valorAux;
            }
            else
            {
                val = getValor(ambito);
            }
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
            else if(val is Nulo)
            {
                return "Nulo";
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
                    String codigo = getNumCodigo(operando1, ambito);
                    switch (this.operador)
                    {
                        case Estatico.Operandos.INC:
                            {
                                valorAux = HazIncremento(this.operando1, codigo, ambito);
                                return valorAux;
                            }
                        case Estatico.Operandos.DEC:
                            {
                                valorAux = HazDecremento(this.operando1, codigo, ambito);
                                return valorAux;
                            }
                        case Estatico.Operandos.RESTA:
                            {
                                valorAux = HazNegativo(this.operando1, codigo, ambito);
                                return valorAux;
                            }
                        case Estatico.Operandos.NOT:
                            {
                                valorAux = operacionNot(this.operando1, codigo, ambito);
                                return valorAux;
                            }
                    }
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
                                valorAux = HazSuma(this.operando1, this.operando2, codigo, ambito);
                                return valorAux;
                            }
                        case Estatico.Operandos.RESTA:
                            {
                                valorAux = HazResta(this.operando1, this.operando2, codigo, ambito);
                                return valorAux;
                            }
                        case Estatico.Operandos.MULT:
                            {
                                valorAux = HazMultiplicacion(operando1, operando2, codigo, ambito);
                                return valorAux;
                            }
                        case Estatico.Operandos.DIVI:
                            {
                                valorAux = HazDivision(operando1, operando2, codigo, ambito);
                                return valorAux;
                            }
                        case Estatico.Operandos.IGUAL:
                            {
                                valorAux = comparaValores(operando1, operando2, codigo, ambito);
                                return valorAux;
                            }
                        case Estatico.Operandos.DIFERENTE:
                            {
                                valorAux = diferenteValores(operando1, operando2, codigo, ambito);
                                return valorAux;
                            }
                        case Estatico.Operandos.MAYOR:
                            {
                                valorAux = comparaMayor(operando1, operando2, codigo, ambito);
                                return valorAux;
                            }
                        case Estatico.Operandos.MENOR:
                            {
                                valorAux = comparaMenor(operando1, operando2, codigo, ambito);
                                return valorAux;
                            }
                        case Estatico.Operandos.MENORIGUAL:
                            {
                                valorAux = comparaMenorIgual(operando1, operando2, codigo, ambito);
                                return valorAux;
                            }
                        case Estatico.Operandos.MAYORIGUAL:
                            {
                                valorAux = comparaMayorIgual(operando1, operando2, codigo, ambito);
                                return valorAux;
                            }
                        case Estatico.Operandos.AND:
                            {
                                valorAux = operacionAnd(operando1, operando2, codigo, ambito);
                                return valorAux;
                            }
                        case Estatico.Operandos.OR:
                            {
                                valorAux = operacionOr(operando1, operando2, codigo, ambito);
                                return valorAux;
                            }
                    }
                }
            }
            catch(Exception e)
            {
                TError error = new TError("Ejecucion", "Error en ejecucion de operaciones en Clase: " + this.clase + " | Archivo: " + ambito.archivo+ " | "+e.Message, this.linea, this.columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);

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
                TError error = new TError("Semantico", "Error Al Ejecutar Suma en Clase: " + this.clase + " Archivo: " + this.archivoOrigen + ", Tipos Incompatibles: " + tipo1 + " con " + tipo2, this.linea, this.columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            catch(Exception e)
            {
                //MessageBox.Show("Error en la Ejecucion de la Operacion De Suma: Linea: " + this.linea + " y Columna: " + this.columna + " | En Clase: " + this.clase + " | Archivo: " + ambito.archivo +" | Error de Ejecucion: "+e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TError error = new TError("Ejecucion", "Error Ejecucion Suma, Archivo: " + this.archivoOrigen + " | " + e.Message, this.linea, this.columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
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
                TError error = new TError("Semantico", "Error Al Ejecutar Resta en Clase: " + this.clase + " Archivo: " + this.archivoOrigen + ", Tipos Incompatibles: " + tipo1 + " con " + tipo2, this.linea, this.columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            catch(Exception e)
            {
                TError error = new TError("Ejecucion", "Error Ejecucion Resta, Archivo: " + this.archivoOrigen + " | " + e.Message, this.linea, this.columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
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
                TError error = new TError("Semantico", "Error Al Ejecutar Multiplicacion en Clase: " + this.clase + " Archivo: " + this.archivoOrigen + ", Tipos Incompatibles: " + tipo1 + " con " + tipo2, this.linea, this.columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            catch(Exception e)
            {
                TError error = new TError("Ejecucion", "Error Ejecucion Multiplicacion, Archivo: " + this.archivoOrigen + " | " + e.Message, this.linea, this.columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
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
                TError error = new TError("Semantico", "Error Al Ejecutar Division en Clase: " + this.clase + " Archivo: " + this.archivoOrigen + ", Tipos Incompatibles: " + tipo1 + " con " + tipo2, this.linea, this.columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            catch(Exception e)
            {
                TError error = new TError("Ejecucion", "Error Ejecucion Division, Archivo: " + this.archivoOrigen + " | " + e.Message, this.linea, this.columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            return new Nulo();
        }
        #endregion

        #region INCREMENTO
        /*********************************************************
         *                  CODIGOS DE TIPOS                     *
         *********************************************************
         * BOOLEAN      |  1   |    0
         * CADENA       |  2   |    0
         * ENTERO       |  3   |    3
         * DECIMAL      |  4   |    4
         * HORA         |  5   |    0
         * FECHA        |  6   |    0
         * FECHAHORA    |  7   |    0
         */
        private Object HazIncremento(Expresion op1, String codigo, Ambito ambito)
        {
            try
            {
                Object val1 = op1.getValor(ambito);
                String tipo1 = op1.getTipo(ambito);
                switch(codigo)
                {
                    case "3":
                        {
                            int valor1 = (int)val1;
                            return valor1+1;
                        }
                    case "4":
                        {
                            double valor1 = (double)val1;
                            return valor1+1;
                        }
                }
                TError error = new TError("Semantico", "Error Al Ejecutar Incremento en Clase: " + this.clase + " Archivo: " + this.archivoOrigen + ", Tipo Incompatible: " + tipo1, this.linea, this.columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            catch(Exception e)
            {
                TError error = new TError("Ejecucion", "Error Ejecucion Incremento, Archivo: "+this.archivoOrigen+" | " + e.Message, this.linea, this.columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            return new Nulo();
        }
        #endregion

        #region DECREMENTO
        /*********************************************************
         *                  CODIGOS DE TIPOS                     *
         *********************************************************
         * BOOLEAN      |  1   |    0
         * CADENA       |  2   |    0
         * ENTERO       |  3   |    3
         * DECIMAL      |  4   |    4
         * HORA         |  5   |    0
         * FECHA        |  6   |    0
         * FECHAHORA    |  7   |    0
         */
        private Object HazDecremento(Expresion op1, String codigo, Ambito ambito)
        {
            try
            {
                Object val1 = op1.getValor(ambito);
                String tipo1 = op1.getTipo(ambito);
                switch (codigo)
                {
                    case "3":
                        {
                            int valor1 = (int)val1;
                            return valor1-1;
                        }
                    case "4":
                        {
                            double valor1 = (double)val1;
                            return valor1-1;
                        }
                }
                TError error = new TError("Semantico", "Error Al Ejecutar Decremento en Clase: " + this.clase + " Archivo: " + this.archivoOrigen + ", Tipo Incompatible: " + tipo1, this.linea, this.columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            catch (Exception e)
            {
                TError error = new TError("Ejecucion", "Error Ejecucion Decremento, Archivo: " + this.archivoOrigen + " | " + e.Message, this.linea, this.columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            return new Nulo();    
        }
        #endregion

        #region NEGATIVO
        /*********************************************************
         *                  CODIGOS DE TIPOS                     *
         *********************************************************
         * BOOLEAN      |  1   |    0
         * CADENA       |  2   |    0
         * ENTERO       |  3   |    3
         * DECIMAL      |  4   |    4
         * HORA         |  5   |    0
         * FECHA        |  6   |    0
         * FECHAHORA    |  7   |    0
         */
        private Object HazNegativo(Expresion op1, String codigo, Ambito ambito)
        {
            try
            {
                Object val1 = op1.getValor(ambito);
                String tipo1 = op1.getTipo(ambito);
                switch (codigo)
                {
                    case "3":
                        {
                            int valor1 = (int)val1;
                            return valor1*-1;
                        }
                    case "4":
                        {
                            double valor1 = (double)val1;
                            return valor1*-1;
                        }
                }
                TError error = new TError("Semantico", "Error Al Ejecutar Negativo en Clase: " + this.clase + " Archivo: " + this.archivoOrigen + ", Tipo Incompatible: " + tipo1, this.linea, this.columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            catch (Exception e)
            {
                TError error = new TError("Ejecucion", "Error Ejecucion Negativo, Archivo: " + this.archivoOrigen + " | " + e.Message, this.linea, this.columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            return new Nulo();
        }
        #endregion

        #region IGUAL
        /*
         * *****************************************************************************************************************
         * *                                       TABLA DE CODIGOS DE IGUAL                                               *
         * *****************************************************************************************************************
         * *               |    BOOLEANO   |     CADENA    |    ENTERO  |   DECIMAL  |    HORA   |   FECHA   |  FECHAHORA  |
         * *****************************************************************************************************************
         * |  BOOELANO     |      11       |       00      |     00     |     00     |     00    |     00    |     00      |
         * *****************************************************************************************************************
         * |  CADENA       |      00       |       22      |     00     |     00     |     00    |     00    |     00      |
         * *****************************************************************************************************************
         * |  ENTERO       |      00       |       00      |     33     |     34     |     00    |     00    |     00      |
         * *****************************************************************************************************************
         * |  DECIMAL      |      00       |       00      |     43     |     44     |     00    |     00    |     00      |
         * *****************************************************************************************************************
         * |  FECHA/HORA   |      00       |       00      |     00     |     00     |     00    |     00    |     00      |
         * *****************************************************************************************************************
         */
        private Object comparaValores(Expresion op1, Expresion op2, String codigo, Ambito ambito)
        {
            try
            {
                Object val1 = op1.getValor(ambito);
                String tipo1 = op1.getTipo(ambito);
                Object val2 = op2.getValor(ambito);
                String tipo2 = op2.getTipo(ambito);
                switch (codigo)
                {
                    case "11":
                        {
                            Boolean valor1 = (Boolean)val1;
                            Boolean valor2 = (Boolean)val2;
                            return valor1 == valor2;
                        }
                    case "22":
                        {
                            String valor1 = (String)val1;
                            String valor2 = (String)val2;
                            return valor1 == valor2;
                        }
                    case "33":
                        {
                            int valor1 = (int)val1;
                            int valor2 = (int)val2;
                            return valor1 == valor2;
                        }
                    case "34":
                        {
                            int valor1 = (int)val1;
                            double valor2 = (double)val2;
                            return valor1 == valor2;
                        }
                    case "43":
                        {
                            double valor1 = (double)val1;
                            int valor2 = (int)val2;
                            return valor1 == valor2;
                        }
                    case "44":
                        {
                            double valor1 = (double)val1;
                            double valor2 = (double)val2;
                            return valor1 == valor2;
                        }
                    case "55":
                        {
                            Hour valor1 = (Hour)val1;
                            Hour valor2 = (Hour)val2;
                            return valor1.CompareHour(valor2, "==");
                        }
                    case "66":
                        {
                            Date valor1 = (Date)val1;
                            Date valor2 = (Date)val2;
                            return valor1.CompareDates(valor2, "==");
                        }
                    case "77":
                        {
                            DateTime valor1 = (DateTime)val1;
                            DateTime valor2 = (DateTime)val2;
                            return valor1.Equals(valor2);
                        }
                }
                TError error = new TError("Semantico", "Error Al Ejecutar Igual en Clase: " + this.clase + " Archivo: " + this.archivoOrigen + ", Tipos Incompatibles: " + tipo1 + " con " + tipo2, this.linea, this.columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            catch (Exception e)
            {
                TError error = new TError("Ejecucion", "Error Ejecucion Igual, Archivo: " + this.archivoOrigen + " | " + e.Message, this.linea, this.columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            return new Nulo();
        }
        #endregion

        #region DIFERENTE
                /*
         * *****************************************************************************************************************
         * *                                       TABLA DE CODIGOS DE IGUAL                                               *
         * *****************************************************************************************************************
         * *               |    BOOLEANO   |     CADENA    |    ENTERO  |   DECIMAL  |    HORA   |   FECHA   |  FECHAHORA  |
         * *****************************************************************************************************************
         * |  BOOELANO     |      11       |       00      |     00     |     00     |     00    |     00    |     00      |
         * *****************************************************************************************************************
         * |  CADENA       |      00       |       22      |     00     |     00     |     00    |     00    |     00      |
         * *****************************************************************************************************************
         * |  ENTERO       |      00       |       00      |     33     |     34     |     00    |     00    |     00      |
         * *****************************************************************************************************************
         * |  DECIMAL      |      00       |       00      |     43     |     44     |     00    |     00    |     00      |
         * *****************************************************************************************************************
         * |  FECHA/HORA   |      00       |       00      |     00     |     00     |     00    |     00    |     00      |
         * *****************************************************************************************************************
         */
        private Object diferenteValores(Expresion op1, Expresion op2, String codigo, Ambito ambito)
        {
            try
            {
                Object val1 = op1.getValor(ambito);
                String tipo1 = op1.getTipo(ambito);
                Object val2 = op2.getValor(ambito);
                String tipo2 = op2.getTipo(ambito);
                switch(codigo)
                {
                    case "11":
                        {
                            Boolean valor1 = (Boolean)val1;
                            Boolean valor2 = (Boolean)val2;
                            return valor1 != valor2;
                        }
                    case "22":
                        {
                            String valor1 = (String)val1;
                            String valor2 = (String)val2;
                            return valor1 != valor2;
                        }
                    case "33":
                        {
                            int valor1 = (int)val1;
                            int valor2 = (int)val2;
                            return valor1 != valor2;
                        }
                    case "34":
                        {
                            int valor1 = (int)val1;
                            double valor2 = (double)val2;
                            return valor1 != valor2;
                        }
                    case "43":
                        {
                            double valor1 = (double)val1;
                            int valor2 = (int)val2;
                            return valor1 != valor2;
                        }
                    case "44":
                        {
                            double valor1 = (double)val1;
                            double valor2 = (double)val2;
                            return valor1 != valor2;
                        }
                    case "55":
                        {
                            Hour valor1 = (Hour)val1;
                            Hour valor2 = (Hour)val2;
                            return valor1.CompareHour(valor2, "!=");
                        }
                    case "66":
                        {
                            Date valor1 = (Date)val1;
                            Date valor2 = (Date)val2;
                            return valor1.CompareDates(valor2, "!=");
                        }
                    case "77":
                        {
                            DateTime valor1 = (DateTime)val1;
                            DateTime valor2 = (DateTime)val2;
                            return !valor1.Equals(valor2);
                        }
                }
                TError error = new TError("Semantico", "Error Al Ejecutar Diferente en Clase: " + this.clase + " Archivo: " + this.archivoOrigen + ", Tipos Incompatibles: " + tipo1 + " con " + tipo2, this.linea, this.columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            catch(Exception e)
            {
                TError error = new TError("Ejecucion", "Error Ejecucion Diferente, Archivo: " + this.archivoOrigen + " | " + e.Message, this.linea, this.columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            return new Nulo();
        }
        #endregion

        #region MAYOR
        /*
         * *****************************************************************************************************************
         * *                                       TABLA DE CODIGOS DE IGUAL                                               *
         * *****************************************************************************************************************
         * *               |    BOOLEANO   |     CADENA    |    ENTERO  |   DECIMAL  |    HORA   |   FECHA   |  FECHAHORA  |
         * *****************************************************************************************************************
         * |  BOOELANO     |      00       |       00      |     00     |     00     |     00    |     00    |     00      |
         * *****************************************************************************************************************
         * |  CADENA       |      00       |       22      |     00     |     00     |     00    |     00    |     00      |
         * *****************************************************************************************************************
         * |  ENTERO       |      00       |       00      |     33     |     34     |     00    |     00    |     00      |
         * *****************************************************************************************************************
         * |  DECIMAL      |      00       |       00      |     43     |     44     |     00    |     00    |     00      |
         * *****************************************************************************************************************
         * |  FECHA/HORA   |      00       |       00      |     00     |     00     |     00    |     00    |     00      |
         * *****************************************************************************************************************
         */
        private Object comparaMayor(Expresion op1, Expresion op2, String codigo, Ambito ambito)
        {
            try
            {
                Object val1 = op1.getValor(ambito);
                String tipo1 = op1.getTipo(ambito);
                Object val2 = op2.getValor(ambito);
                String tipo2 = op2.getTipo(ambito);
                switch(codigo)
                {
                    case "22":
                        {
                            String valor1 = (String)val1;
                            String valor2 = (String)val2;
                            int c = valor1.CompareTo(valor2);
                            if(c > 0) { return true; }
                            return false;
                        }
                    case "33":
                        {
                            int valor1 = (int)val1;
                            int valor2 = (int)val2;
                            return valor1 > valor2;
                        }
                    case "34":
                        {
                            int valor1 = (int)val1;
                            double valor2 = (double)val2;
                            return valor1 > valor2;
                        }
                    case "43":
                        {
                            double valor1 = (double)val1;
                            int valor2 = (int)val2;
                            return valor1 > valor2;
                        }
                    case "44":
                        {
                            double valor1 = (double)val1;
                            double valor2 = (double)val2;
                            return valor1 > valor2;
                        }
                    case "55":
                        {
                            Hour valor1 = (Hour)val1;
                            Hour valor2 = (Hour)val2;
                            return valor1.CompareHour(valor2, ">");
                        }
                    case "66":
                        {
                            Date valor1 = (Date)val1;
                            Date valor2 = (Date)val2;
                            return valor1.CompareDates(valor2, ">");
                        }
                    case "77":
                        {
                            DateTime valor1 = (DateTime)val1;
                            DateTime valor2 = (DateTime)val2;
                            int c = valor1.CompareTo(valor2);
                            if(c > 0) { return true; }
                            return false;
                        }
                }
                TError error = new TError("Semantico", "Error Al Ejecutar Mayor en Clase: " + this.clase + " Archivo: " + this.archivoOrigen + ", Tipos Incompatibles: " + tipo1 + " con " + tipo2, this.linea, this.columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            catch(Exception e)
            {
                TError error = new TError("Ejecucion", "Error Ejecucion Mayor, Archivo: " + this.archivoOrigen + " | " + e.Message, this.linea, this.columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            return new Nulo();
        }
        #endregion

        #region MENOR
        /*
         * *****************************************************************************************************************
         * *                                       TABLA DE CODIGOS DE IGUAL                                               *
         * *****************************************************************************************************************
         * *               |    BOOLEANO   |     CADENA    |    ENTERO  |   DECIMAL  |    HORA   |   FECHA   |  FECHAHORA  |
         * *****************************************************************************************************************
         * |  BOOELANO     |      00       |       00      |     00     |     00     |     00    |     00    |     00      |
         * *****************************************************************************************************************
         * |  CADENA       |      00       |       22      |     00     |     00     |     00    |     00    |     00      |
         * *****************************************************************************************************************
         * |  ENTERO       |      00       |       00      |     33     |     34     |     00    |     00    |     00      |
         * *****************************************************************************************************************
         * |  DECIMAL      |      00       |       00      |     43     |     44     |     00    |     00    |     00      |
         * *****************************************************************************************************************
         * |  FECHA/HORA   |      00       |       00      |     00     |     00     |     00    |     00    |     00      |
         * *****************************************************************************************************************
         */
        private Object comparaMenor(Expresion op1, Expresion op2, String codigo, Ambito ambito)
        {
            try
            {
                Object val1 = op1.getValor(ambito);
                String tipo1 = op1.getTipo(ambito);
                Object val2 = op2.getValor(ambito);
                String tipo2 = op2.getTipo(ambito);
                switch (codigo)
                {
                    case "22":
                        {
                            String valor1 = (String)val1;
                            String valor2 = (String)val2;
                            int c = valor1.CompareTo(valor2);
                            if (c < 0) { return true; }
                            return false;
                        }
                    case "33":
                        {
                            int valor1 = (int)val1;
                            int valor2 = (int)val2;
                            return valor1 < valor2;
                        }
                    case "34":
                        {
                            int valor1 = (int)val1;
                            double valor2 = (double)val2;
                            return valor1 < valor2;
                        }
                    case "43":
                        {
                            double valor1 = (double)val1;
                            int valor2 = (int)val2;
                            return valor1 < valor2;
                        }
                    case "44":
                        {
                            double valor1 = (double)val1;
                            double valor2 = (double)val2;
                            return valor1 < valor2;
                        }
                    case "55":
                        {
                            Hour valor1 = (Hour)val1;
                            Hour valor2 = (Hour)val2;
                            return valor1.CompareHour(valor2, "<");
                        }
                    case "66":
                        {
                            Date valor1 = (Date)val1;
                            Date valor2 = (Date)val2;
                            return valor1.CompareDates(valor2, "<");
                        }
                    case "77":
                        {
                            DateTime valor1 = (DateTime)val1;
                            DateTime valor2 = (DateTime)val2;
                            int c = valor1.CompareTo(valor2);
                            if (c < 0) { return true; }
                            return false;
                        }
                }
                TError error = new TError("Semantico", "Error Al Ejecutar Menor en Clase: " + this.clase + " Archivo: " + this.archivoOrigen + ", Tipos Incompatibles: " + tipo1 + " con " + tipo2, this.linea, this.columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            catch (Exception e)
            {
                TError error = new TError("Ejecucion", "Error Ejecucion Menor, Archivo: " + this.archivoOrigen + " | " + e.Message, this.linea, this.columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            return new Nulo();
        }
        #endregion

        #region MAYORIGUAL
        private Object comparaMayorIgual(Expresion op1, Expresion op2, String codigo, Ambito ambito)
        {
            try
            {
                Boolean igual = (Boolean)comparaValores(op1, op2, codigo, ambito);
                Boolean mayor = (Boolean)comparaMayor(op1, op2, codigo, ambito);
                if(igual || mayor)
                {
                    return true;
                }
                return false;
            }
            catch(Exception e)
            {
                MessageBox.Show("Error en la Ejecucion de la Comparacion \">=\" Linea: " + this.linea + " y Columna: " + this.columna + " | En Clase: " + this.clase + " | Archivo: " + ambito.archivo + " | Error de Ejecucion: " + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return new Nulo();
        }
        #endregion

        #region MENORIGUAL
        private Object comparaMenorIgual(Expresion op1, Expresion op2, String codigo, Ambito ambito)
        {
            try
            {
                Boolean igual = (Boolean)comparaValores(op1, op2, codigo, ambito);
                Boolean menor = (Boolean)comparaMenor(op1, op2, codigo, ambito);
                if(igual || menor)
                {
                    return true;
                }
                return false;
            }
            catch(Exception e)
            {
                MessageBox.Show("Error en la Ejecucion de la Comparacion \"<=\" Linea: " + this.linea + " y Columna: " + this.columna + " | En Clase: " + this.clase + " | Archivo: " + ambito.archivo + " | Error de Ejecucion: " + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return new Nulo();
        }
        #endregion

        #region AND
        private Object operacionAnd(Expresion op1, Expresion op2, String codigo, Ambito ambito)
        {
            try
            {
                Object val1 = op1.getValor(ambito);
                String tipo1 = op1.getTipo(ambito);
                Object val2 = op2.getValor(ambito);
                String tipo2 = op2.getTipo(ambito);
                switch(codigo)
                {
                    case "11":
                        {
                            Boolean valor1 = (Boolean)val1;
                            Boolean valor2 = (Boolean)val2;
                            return valor1 && valor2;
                        }
                }
                TError error = new TError("Semantico", "Error Al Ejecutar AND en Clase: " + this.clase + " Archivo: " + this.archivoOrigen + ", Tipos Incompatibles: " + tipo1 + " con " + tipo2, this.linea, this.columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            catch(Exception e)
            {
                TError error = new TError("Ejecucion", "Error Ejecucion AND, Archivo: " + this.archivoOrigen + " | " + e.Message, this.linea, this.columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            return new Nulo();
        }
        #endregion

        #region OR
        private Object operacionOr(Expresion op1, Expresion op2, String codigo, Ambito ambito)
        {
            try
            {
                Object val1 = op1.getValor(ambito);
                String tipo1 = op1.getTipo(ambito);
                Object val2 = op2.getValor(ambito);
                String tipo2 = op2.getTipo(ambito);
                switch (codigo)
                {
                    case "11":
                        {
                            Boolean valor1 = (Boolean)val1;
                            Boolean valor2 = (Boolean)val2;
                            return valor1 || valor2;
                        }
                }
                TError error = new TError("Semantico", "Error Al Ejecutar OR en Clase: " + this.clase + " Archivo: " + this.archivoOrigen + ", Tipos Incompatibles: " + tipo1 + " con " + tipo2, this.linea, this.columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            catch (Exception e)
            {
                TError error = new TError("Ejecucion", "Error Ejecucion OR, Archivo: " + this.archivoOrigen + " | " + e.Message, this.linea, this.columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            return new Nulo();
        }
        #endregion

        #region NOT
        private Object operacionNot(Expresion op1, String codigo, Ambito ambito)
        {
            try
            {
                Object val1 = op1.getValor(ambito);
                String tipo1 = op1.getTipo(ambito);
                switch (codigo)
                {
                    case "1":
                        {
                            Boolean valor1 = (Boolean)val1;
                            return !valor1;
                        }
                }
                TError error = new TError("Semantico", "Error Al Ejecutar NOT en Clase: " + this.clase + " Archivo: " + this.archivoOrigen + ", Tipo Incompatible: " + tipo1, this.linea, this.columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            catch (Exception e)
            {
                TError error = new TError("Ejecucion", "Error Ejecucion NOT, Archivo: " + this.archivoOrigen + " | " + e.Message, this.linea, this.columna, false);
                Estatico.errores.Add(error);
                Estatico.ColocaError(error);
            }
            return new Nulo();
        }
        #endregion
    }
}
