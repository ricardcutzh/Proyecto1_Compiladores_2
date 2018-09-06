using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace XForms.Objs
{
    class Date
    {
        DateTime fecha;
        int Dia { get; }
        int Mes { get; }
        int Anio { get; }
 
        public Date(DateTime fecha)
        {
            this.fecha = fecha;
            this.Dia = fecha.Day;
            this.Mes = fecha.Month;
            this.Anio = fecha.Year;
            
        }

        public override bool Equals(object obj)//SI COMPARO DOS DATES
        {
            try
            {
                Date aux = (Date)obj;
                if(this.Dia == aux.Dia && this.Mes == aux.Mes && this.Anio == aux.Anio)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public override string ToString()
        {
            String cad = this.Dia+"/"+this.Mes+"/"+this.Anio;

            return cad;
        }

        public bool CompareDates(Date fecha, String operador)
        {
            switch(operador)
            {
                case ">":
                    {
                        return this.MayorQue(fecha);
                    }
                case "<":
                    {
                        return this.MenorQue(fecha);
                    }
                case ">=":
                    {
                        if (this.Equals(fecha)) { return true; }
                        return this.MayorQue(fecha);
                    }
                case "<=":
                    {
                        if(this.Equals(fecha)) { return true; }
                        return this.MenorQue(fecha);
                    }
                case "==":
                    {
                        return this.Equals(fecha);
                    }
                case "!=":
                    {
                        return !this.Equals(fecha);
                    }
            }

            return false;
        }

        private bool MenorQue(Date Fecha)
        {
            if(this.Anio < Fecha.Anio)
            {
                return true;
            }
            else if(this.Anio == Fecha.Anio)
            {
                if(this.Mes < Fecha.Mes)
                {
                    return true;
                }
                else if(this.Mes == Fecha.Mes)
                {
                    if(this.Dia < Fecha.Dia)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;
            }
            return false;
        }

        private bool MayorQue(Date Fecha)
        {
            if(this.Anio > Fecha.Anio)
            {
                return true;
            }
            else if(this.Anio == Fecha.Anio)
            {
                if(this.Mes > Fecha.Mes)
                {
                    return true;
                }
                else if(this.Mes == Fecha.Mes)
                {
                    if(this.Dia > Fecha.Dia)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

}
