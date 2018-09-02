using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XForms.Objs
{
    class Hour
    {
        DateTime fecha;
        int Hora { get; }
        int Min { get; }
        int Seg { get; }

        public Hour(DateTime fecha)
        {
            this.fecha = fecha;
            this.Hora = fecha.Hour;
            this.Min = fecha.Minute;
            this.Seg = fecha.Second;
        }

        public override string ToString()
        {
            return this.Hora + ":" + this.Min + ":" + this.Seg;
        }

        public override bool Equals(object obj)
        {
            try
            {
                Hour aux = (Hour)obj;
                if(this.Hora == aux.Hora && this.Min == aux.Min && this.Seg == aux.Seg )
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

        public bool CompareHour(Hour hora, String operador)
        {
            switch(operador)
            {
                case ">":
                    {
                        return MayorQue(hora);
                    }
                case "<":
                    {
                        return MenorQue(hora);
                    }
                case ">=":
                    {
                        if (this.Equals(hora)) { return true; }
                        return MayorQue(hora);
                    }
                case "<=":
                    {
                        if (this.Equals(hora)) { return true; }
                        return MenorQue(hora);
                    }
                case "==":
                    {
                        return this.Equals(Hora);
                    }
                case "!=":
                    {
                        return !this.Equals(Hora);
                    }
            }
            return false;
        }

        private bool MayorQue(Hour hora)
        {
            if(this.Hora > hora.Hora)
            {
                return true;
            }
            else if(this.Hora == hora.Hora)
            {
                if(this.Min > hora.Min)
                {
                    return true;
                }
                else if(this.Min == hora.Min)
                {
                    if(this.Seg > hora.Seg)
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

        private bool MenorQue(Hour hora)
        {
            if(this.Hora < hora.Hora)
            {
                return true;
            }
            else if(this.Hora == hora.Hora)
            {
                if(this.Min < hora.Min)
                {
                    return true;
                }
                else if(this.Min == hora.Min)
                {
                    if(this.Seg < hora.Seg)
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
    }
}
