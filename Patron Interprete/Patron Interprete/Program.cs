using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patron_Interprete
{
    // Contexto que contiene la información de entrada
    class Contexto
    {
        public Dictionary<string, int> Variables { get; set; } = new Dictionary<string, int>();
    }

    // Interfaz de la expresión
    interface IExpresion
    {
        int Interpretar(Contexto contexto);
    }

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

    class RestaExpresion : OperacionExpresion
    {
        public RestaExpresion(IExpresion exp1, IExpresion exp2) : base(exp1, exp2) { }

        public override int Interpretar(Contexto contexto)
        {
            return exp1.Interpretar(contexto) - exp2.Interpretar(contexto);
        }
    }

    class MultiplicacionExpresion : OperacionExpresion
    {
        public MultiplicacionExpresion(IExpresion exp1, IExpresion exp2) : base(exp1, exp2) { }

        public override int Interpretar(Contexto contexto)
        {
            return exp1.Interpretar(contexto) * exp2.Interpretar(contexto);
        }
    }

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

    class Program
    {
        static void Main()
        {
            var contexto = new Contexto();

            // Ingresa una expresión aritmética
            Console.Write("Ingrese la expresión aritmética: ");
            string expresionInput = Console.ReadLine();

            // Ingresa valores para las variables
            foreach (char variable in ObtenerVariables(expresionInput))
            {
                Console.Write($"Ingrese el valor de {variable}: ");
                contexto.Variables[variable.ToString()] = int.Parse(Console.ReadLine());
            }

            // Crea una expresión y evalúa
            var expresion = ParseExpresion(expresionInput);
            int resultado = expresion.Interpretar(contexto);

            // Mostrar el resultado
            Console.WriteLine("Resultado: " + resultado);
            Console.ReadKey();
        }

        // Método para parsear la expresión y construir el árbol de expresiones
        static IExpresion ParseExpresion(string expresion)
        {
            // Implementar un analizador sintáctico para construir el árbol de expresiones
            // Aquí se proporciona un analizador muy simple para demostración
            string[] tokens = expresion.Split(' ');

            // Convierte la expresión en notación postfija para facilitar la construcción del árbol
            Queue<string> outputQueue = ShuntingYard(tokens);

            // Construye el árbol de expresiones a partir de la notación postfija
            Stack<IExpresion> stack = new Stack<IExpresion>();

            while (outputQueue.Count > 0)
            {
                string token = outputQueue.Dequeue();

                if (EsOperador(token))
                {
                    IExpresion exp2 = stack.Pop();
                    IExpresion exp1 = stack.Pop();

                    switch (token)
                    {
                        case "+":
                            stack.Push(new SumaExpresion(exp1, exp2));
                            break;
                        case "-":
                            stack.Push(new RestaExpresion(exp1, exp2));
                            break;
                        case "*":
                            stack.Push(new MultiplicacionExpresion(exp1, exp2));
                            break;
                        case "/":
                            stack.Push(new DivisionExpresion(exp1, exp2));
                            break;
                        default:
                            throw new InvalidOperationException($"Operador no reconocido: {token}");
                    }
                }
                else
                {
                    stack.Push(new ValorExpresion(token));
                }
            }

            // Al final, la cima de la pila debería contener la expresión completa
            return stack.Pop();
        }

        // Método para convertir la expresión a notación postfija utilizando el algoritmo de Shunting Yard
        static Queue<string> ShuntingYard(string[] tokens)
        {
            // Implementa el algoritmo de la yarda de clasificación para convertir la expresión a notación postfija
            // Aquí se proporciona una implementación simple para demostración
            Queue<string> outputQueue = new Queue<string>();
            Stack<string> operatorStack = new Stack<string>();

            foreach (string token in tokens)
            {
                if (EsOperador(token))
                {
                    while (operatorStack.Count > 0 && Precedencia(operatorStack.Peek()) >= Precedencia(token))
                    {
                        outputQueue.Enqueue(operatorStack.Pop());
                    }
                    operatorStack.Push(token);
                }
                else if (token == "(")
                {
                    operatorStack.Push(token);
                }
                else if (token == ")")
                {
                    while (operatorStack.Count > 0 && operatorStack.Peek() != "(")
                    {
                        outputQueue.Enqueue(operatorStack.Pop());
                    }
                    operatorStack.Pop(); // Desapila el paréntesis izquierdo
                }
                else
                {
                    outputQueue.Enqueue(token);
                }
            }

            while (operatorStack.Count > 0)
            {
                outputQueue.Enqueue(operatorStack.Pop());
            }

            return outputQueue;
        }

        // Método para verificar si un token es un operador
        static bool EsOperador(string token)
        {
            return token == "+" || token == "-" || token == "*" || token == "/";
        }

        // Método para determinar la precedencia de un operador
        static int Precedencia(string operador)
        {
            switch (operador)
            {
                case "+":
                case "-":
                    return 1;
                case "*":
                case "/":
                    return 2;
                default:
                    return 0;
            }
        }

        // Método para obtener las variables de la expresión
        static IEnumerable<char> ObtenerVariables(string expresion)
        {
            HashSet<char> variables = new HashSet<char>();

            foreach (char c in expresion)
            {
                if (char.IsLetter(c))
                {
                    variables.Add(c);
                }
            }

            return variables;
        }
    }
}
