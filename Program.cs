using System;
using System.Collections.Generic;
using System.Linq;

namespace infinite {
    class Program {
        static void Main(string[] args) {

            //           Infinite.FizzBuzz().Take(10).ForEach(x => Console.WriteLine(x));
            //           Infinite.Cartesian(Enumerable.Range(1, 3), Enumerable.Range(101, 3)).ForEach(x => Console.WriteLine(x));

            //            Infinite.Diagonal(Enumerable.Range(1, 2), Enumerable.Range(1, 5)).ForEach(x => Console.WriteLine(x));
            var triangle = Infinite.Diagonal(Infinite.Pos(), Infinite.Pos(), Infinite.Pos())
                .Where(x => x.Item1 * x.Item1 + x.Item2 * x.Item2 == x.Item3 * x.Item3)
                .Where(x => x.Item1 + x.Item2 + x.Item3 == 24)
                .First();

            Console.WriteLine(triangle);

        }
    }

    static class Infinite {
        public static IEnumerable<int> Nat() => Iterate(x => x + 1, 0);

        public static IEnumerable<int> Pos() => Nat().Skip(1);

        public static IEnumerable<int> Evens() => Nat().Where(x => x % 2 == 0);

        public static IEnumerable<int> Odds() => Nat().Where(x => x % 2 != 0);

        public static IEnumerable<int> Multiples(int n) => Nat().Select(x => x * n);

        public static IEnumerable<int> Collatz(int n) => Iterate(x => x % 2 == 0 ? x / 2 : x * 3 + 1, n);

        public static IEnumerable<int> Fibonacci() => FibonacciPair().Select(x => x.Item1);
        private static IEnumerable < (int, int) > FibonacciPair() => Iterate(x => (x.Item2, x.Item1 + x.Item2), (0, 1));

        public static IEnumerable<string> FizzBuzz() => Pos().Select(
            x =>
            x % 15 == 0 ? "FizzBuzz"
            : x % 3 == 0 ? "Fizz"
            : x % 5 == 0 ? "Buzz"
            : x.ToString());

        public static IEnumerable<T> Iterate<T>(Func<T, T> generator, T init) {
            if (generator == null) throw new ArgumentNullException("generator");

            while (true) {
                yield return init;
                init = generator(init);
            }
        }

        public static IEnumerable < (T1, T2) > Cartesian<T1, T2>(IEnumerable<T1> source1, IEnumerable<T2> source2) {
            if (source1 == null) throw new ArgumentNullException("source1");
            if (source2 == null) throw new ArgumentNullException("source2");

            foreach (var item1 in source1)
                foreach (var item2 in source2) yield return (item1, item2);
        }

        public static IEnumerable < (T1, T2, T3) > Cartesian<T1, T2, T3>(IEnumerable<T1> source1, IEnumerable<T2> source2, IEnumerable<T3> source3) {
            if (source1 == null) throw new ArgumentNullException("source1");
            if (source2 == null) throw new ArgumentNullException("source2");
            if (source3 == null) throw new ArgumentNullException("source3");

            foreach (var item1 in source1)
                foreach (var item2 in source2)
                    foreach (var item3 in source3) yield return (item1, item2, item3);
        }

        public static IEnumerable < (T1, T2) > Diagonal<T1, T2>(IEnumerable<T1> source1, IEnumerable<T2> source2) {
            if (source1 == null) throw new ArgumentNullException("source1");
            if (source2 == null) throw new ArgumentNullException("source2");

            var enumerator1 = source1.GetEnumerator();
            var enumerator2 = source2.GetEnumerator();
            var list1 = new List<T1>();
            var list2 = new List<T2>();
            var StartIndexOfList1 = 0;

            while (true) {
                var updated1 = enumerator1.MoveNext();
                var updated2 = enumerator2.MoveNext();

                if (updated1) list1.Add(enumerator1.Current);
                if (updated2) list2.Add(enumerator2.Current);

                if (!updated2) StartIndexOfList1++;
                if (StartIndexOfList1 >= list1.Count) break;

                var indexOfList1 = StartIndexOfList1;
                var indexOfList2 = list2.Count - 1;
                while (indexOfList1 < list1.Count && indexOfList2 >= 0) {
                yield return (list1[indexOfList1], list2[indexOfList2]);

                indexOfList1++;
                indexOfList2--;
                }
            }
        }

        public static IEnumerable < (T1, T2, T3) > Diagonal<T1, T2, T3>(IEnumerable<T1> source1, IEnumerable<T2> source2, IEnumerable<T3> source3) {
            if (source1 == null) throw new ArgumentNullException("source1");
            if (source2 == null) throw new ArgumentNullException("source2");
            if (source3 == null) throw new ArgumentNullException("source3");

            var source12 = Diagonal(source1, source2);
            return Diagonal(source12, source3).Select(x => (x.Item1.Item1, x.Item1.Item2, x.Item2));
        }
    }
}
