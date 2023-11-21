using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpreteEjercicio.Interprete
{
    // Implementaciones concretas de operaciones
    class SumaExpresion : OperacionExpresion
    {
        // Constructor que llama al constructor de la clase base
        public SumaExpresion(IExpresion exp1, IExpresion exp2) : base(exp1, exp2) { }

        // Implementación del método abstracto para la suma
        public override int Interpretar(Contexto contexto)
        {
            return exp1.Interpretar(contexto) + exp2.Interpretar(contexto);
        }
    }

}
