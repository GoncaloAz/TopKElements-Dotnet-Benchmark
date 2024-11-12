using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
namespace TopKBenchmark{
    [SimpleJob(RuntimeMoniker.Net70)]
    [SimpleJob(RuntimeMoniker.Net80)]
    [MemoryDiagnoser]
    public class TopKBenchmarker
    {
        public int[] nums;
        public int k;
        [GlobalSetup]
        public void TopKBenchmark()
        {
            nums = new int[10000];
            Random random = new Random();
            for (int i = 0; i < nums.Length; i++)
            {
                nums[i] = random.Next(1, 10); // Generates numbers from 1 to 9
            }
            
            k = 6;
        }


        [Benchmark(Baseline = true)]
        public int[] TopKFrequent_UsingLinq() {
        
		Dictionary<int, int> topK = new();
		
		foreach( int num in nums) {
			if(!topK.ContainsKey(num)){
				topK.Add(num, 1);
			}
			else{
				topK[num] = topK.GetValueOrDefault(num) + 1;
			}
		}
		
		var ordered = topK.OrderByDescending(x => x.Value).Select(x => x.Key).Take(k).ToArray();
		return ordered;
        }

        [Benchmark]
        public int[] TopKFrequent_UsingHeap() {
        Dictionary<int, int> count = new();
        foreach (var num in nums) {
            if (count.ContainsKey(num)) {
                count[num]++;
            } else {
                count[num] = 1;
            }
        }

        var heap = new PriorityQueue<int, int>();
        foreach (var entry in count) {
            heap.Enqueue(entry.Key, entry.Value);
            if (heap.Count > k) {
                heap.Dequeue();
            }
        }
        
        var res = new int[k];
        for (int i = 0; i < k; i++) {
            res[i] = heap.Dequeue();
        }
        return res;
    }

    [Benchmark]
    public int[] TopKFrequent_bucketsort() {
        Dictionary<int, int> count = new Dictionary<int, int>();
        List<int>[] freq = new List<int>[nums.Length + 1];
        for (int i = 0; i < freq.Length; i++) {
            freq[i] = new List<int>();
        }

        foreach (int n in nums) {
            if (count.ContainsKey(n)) {
                count[n]++;
            } else {
                count[n] = 1;
            }
        }
        foreach (var entry in count){
            freq[entry.Value].Add(entry.Key);
        }

        int[] res = new int[k];
        int index = 0;
        for (int i = freq.Length - 1; i > 0 && index < k; i--) {
            foreach (int n in freq[i]) {
                res[index++] = n;
                if (index == k) {
                    return res;
                }
            }
        }
        return res;
    }


    }
}