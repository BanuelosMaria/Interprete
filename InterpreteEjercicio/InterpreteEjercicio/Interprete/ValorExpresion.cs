using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpreteEjercicio.Interprete
{
    // Expresión terminal que representa una variable o un valor numérico
    class ValorExpresion : IExpresion
    {
        private string valor;

        // Constructor que toma el valor (puede ser una variable o número)
        public ValorExpresion(string valor)
        {
            this.valor = valor;
        }

        // Método para interpretar la variable o número y obtener su valor del contexto
        public int Interpretar(Contexto contexto)
        {
            if (int.TryParse(valor, out int numero))
            {
                return numero; // Si es un número, devuelve el valor numérico
            }
            else if (contexto.Variables.ContainsKey(valor))
            {
                return contexto.Variables[valor]; // Si es una variable, devuelve su valor
            }
            else
            {
                throw new InvalidOperationException($"Valor '{valor}' no encontrado");
            }
        }
    }
}
