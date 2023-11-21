using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpreteEjercicio.Interprete
{
    // Contexto que contiene la información de entrada
    class Contexto
    {
        public Dictionary<string, int> Variables { get; set; } = new Dictionary<string, int>();
    }
}
