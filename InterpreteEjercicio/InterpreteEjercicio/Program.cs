using InterpreteEjercicio.Interprete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpreteEjercicio
{
    class Program
    {
        static void Main()
        {
            Console.Title = "Ejercico Patron interprete";
            Console.WriteLine("\tPATRON INTERPRETE");
            Console.WriteLine("Programa para resolver expresiones aritmeticas con suma, resta, division y multiplicacion");
            Console.WriteLine("Recuerda separar los signos para que puedan ser reconocidos.");
            Console.WriteLine("SI (a + b)  NO (a+b)");

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
