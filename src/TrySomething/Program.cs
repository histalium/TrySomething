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
        }
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
