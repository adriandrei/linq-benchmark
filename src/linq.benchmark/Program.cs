using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Collections.Immutable;

namespace linq.benchmark;

internal class Program
{
    static void Main(string[] args)
    {
        BenchmarkRunner.Run(typeof(Program).Assembly);
    }
}

public class BenchmarkLINQPerformance
{
    private readonly List<int> data = new List<int>();
    private readonly Dictionary<string, int> count = new Dictionary<string, int>();

    [GlobalSetup]
    public void GlobalSetup()
    {
        for(int j = 0; j < 1000; j++)
        {
            for (int i = 0; i < 100; i++)
            {
                data.Add(i);

                var key = (i * j).ToString();
                if (count.ContainsKey(key))
                {
                    count[key]++;
                }
                else
                {
                    count.Add(key, 0);
                }
            }
        }
    }

    [Benchmark]
    public int Sum() => count.Sum(p => p.Value);
    [Benchmark]
    public int Sum_After_Select() => count.Select(t => t.Value).Sum();
    [Benchmark]
    public int? Last() => data.LastOrDefault(x => x.Equals(10000));
    [Benchmark]
    public int? First() => data.FirstOrDefault(x => x.Equals(10000));
    [Benchmark]
    public bool Any() => data.Any(t => t.Equals(10000));
    [Benchmark]
    public bool Contains() => data.Contains(10000);
    [Benchmark]
    public IReadOnlyDictionary<int, int> ToDictionary_Via_KVP_AsDictionary() => data.Select(t => new KeyValuePair<int, int>(t, t)).ToImmutableDictionary();

}