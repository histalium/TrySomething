using System;

namespace TrySomething
{
    class Program
    {
        static void Main(string[] args)
        {
            var t1 = new Test1 { Value = 23 };
            var v = new Union<Test1, Test2>(t1);

            var result = v.Match(
                t1 => t1.Value,
                t2 => t2.Value
            );

            v.Match(
                t1 => Console.WriteLine("Test1"),
                t2 => Console.WriteLine("Test2")
            );

            Console.WriteLine(result);

            var input1 = new EmptyConsoleInputContext(NextConsoleInput());
            var input2 = input1.Next();
            Console.WriteLine(input2.Value);
            var input3 = input1.Next();
            Console.WriteLine(input3.Value);
            var input4 = input2.Next();
            Console.WriteLine(input4.Value);
            var input5 = input3.Next();
            Console.WriteLine(input5.Value);

            Iterate(input1, t =>
            {
                Console.WriteLine(t);
                return t == "quit";
            });
        }

        static void Iterate(EmptyConsoleInputContext context, Func<string, bool> action)
        {
            var next = context.Next();
            var done = action(next.Value);
            if (!done)
            {
                Iterate(next, action);
            }
        }

        static void Iterate(ConsoleInputContext context, Func<string, bool> action)
        {
            var next = context.Next();
            var done = action(next.Value);
            if (!done)
            {
                Iterate(next, action);
            }
        }

        static Func<ConsoleInputContext> NextConsoleInput()
        {
            ConsoleInputContext context = null;

            Func<ConsoleInputContext> next = () =>
            {
                if (context == null)
                {
                    var input = Console.ReadLine();
                    context = new ConsoleInputContext(input, NextConsoleInput());
                }
                return context;
            };

            return next;
        }
    }

    class EmptyConsoleInputContext
    {
        public EmptyConsoleInputContext(Func<ConsoleInputContext> next)
        {
            Next = next;
        }

        public Func<ConsoleInputContext> Next { get; }
    }

    class ConsoleInputContext
    {
        public ConsoleInputContext(string value, Func<ConsoleInputContext> next)
        {
            Value = value;
            Next = next;
        }

        public string Value { get; }

        public Func<ConsoleInputContext> Next { get; }
    }

    class Test1
    {
        public int Value { get; set; }
    }

    class Test2
    {
        public int Value { get; set; }
    }

    class Union<T1, T2>
    {
        private readonly object value;
        private readonly Type type;

        public Union(T1 test1)
        {
            value = test1;
            type = typeof(T1);
        }

        public Union(T2 test2)
        {
            value = test2;
            type = typeof(T2);
        }

        public T Match<T>(Func<T1, T> matchTest1, Func<T2, T> matchTest2)
        {
            if (type == typeof(T1))
            {
                return matchTest1((T1)value);
            }
            if (type == typeof(T2))
            {
                return matchTest2((T2)value);
            }

            throw new InvalidProgramException();
        }

        public void Match(Action<T1> matchTest1, Action<T2> matchTest2)
        {
            if (type == typeof(T1))
            {
                matchTest1((T1)value);
                return;
            }
            if (type == typeof(T2))
            {
                matchTest2((T2)value);
                return;
            }

            throw new InvalidProgramException();
        }
    }
}
