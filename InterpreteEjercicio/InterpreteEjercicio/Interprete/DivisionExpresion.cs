using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpreteEjercicio.Interprete
{
    class DivisionExpresion : OperacionExpresion
    {
        public DivisionExpresion(IExpresion exp1, IExpresion exp2) : base(exp1, exp2) { }

        public override int Interpretar(Contexto contexto)
        {
            int divisor = exp2.Interpretar(contexto);

            if (divisor != 0)
            {
                return exp1.Interpretar(contexto) / divisor;
            }
            else
            {
                throw new InvalidOperationException("División por cero no permitida");
            }
        }
    }

}
