using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpreteEjercicio.Interprete
{
    // Expresión no terminal que representa una operación binaria
    abstract class OperacionExpresion : IExpresion
    {
        protected IExpresion exp1;
        protected IExpresion exp2;

        // Constructor que toma dos expresiones a operar
        public OperacionExpresion(IExpresion exp1, IExpresion exp2)
        {
            this.exp1 = exp1;
            this.exp2 = exp2;
        }

        // Método abstracto que será implementado por las clases derivadas
        public abstract int Interpretar(Contexto contexto);
    }

}
