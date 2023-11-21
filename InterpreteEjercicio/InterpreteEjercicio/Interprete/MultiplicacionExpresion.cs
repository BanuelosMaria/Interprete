using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpreteEjercicio.Interprete
{
    class MultiplicacionExpresion : OperacionExpresion
    {
        public MultiplicacionExpresion(IExpresion exp1, IExpresion exp2) : base(exp1, exp2) { }

        public override int Interpretar(Contexto contexto)
        {
            return exp1.Interpretar(contexto) * exp2.Interpretar(contexto);
        }
    }

}
