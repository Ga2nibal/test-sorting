using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadixVsQuickSort
{
	class Program
	{

		#region [Dual pivot c#]

		public static void DualPivotQuickSort(int[] a)
		{
			DualPivotQuickSort(a, 0, a.Length);
		}

		public static void DualPivotQuickSort(int[] a, int fromIndex, int toIndex)
		{
			rangeCheck(a.Length, fromIndex, toIndex);
			dualPivotQuicksort(a, fromIndex, toIndex - 1, 3);
		}

		private static void rangeCheck(int length, int fromIndex, int toIndex)
		{
			if (fromIndex > toIndex)
			{
				throw new ArgumentException("fromIndex > toIndex");
			}
			if (fromIndex < 0)
			{
				throw new IndexOutOfRangeException(nameof(fromIndex));
			}
			if (toIndex > length)
			{
				throw new IndexOutOfRangeException(nameof(toIndex));
			}
		}

		private static void Swap(int[] a, int i, int j)
		{
			int temp = a[i];
			a[i] = a[j];
			a[j] = temp;
		}

		private static void dualPivotQuicksort(int[] a, int left, int right, int div)
		{
			int len = right - left;
			if (len < 27)
			{
				// insertion sort for tiny array
				for (int i = left + 1; i <= right; i++)
				{
					for (int j = i; j > left && a[j] < a[j - 1]; j--)
					{
						Swap(a, j, j - 1);
					}
				}
				return;
			}
			int third = len/div;
			// "medians"
			int m1 = left + third;
			int m2 = right - third;
			if (m1 <= left)
			{
				m1 = left + 1;
			}
			if (m2 >= right)
			{
				m2 = right - 1;
			}
			if (a[m1] < a[m2])
			{
				Swap(a, m1, left);
				Swap(a, m2, right);
			}
			else
			{
				Swap(a, m1, right);
				Swap(a, m2, left);
			}
			// pivots
			int pivot1 = a[left];
			int pivot2 = a[right];
			// pointers
			int less = left + 1;
			int great = right - 1;
			// sorting
			for (int k = less; k <= great; k++)
			{
				if (a[k] < pivot1)
				{
					Swap(a, k, less++);
				}
				else if (a[k] > pivot2)
				{
					while (k < great && a[great] > pivot2)
					{
						great--;
					}
					Swap(a, k, great--);
					if (a[k] < pivot1)
					{
						Swap(a, k, less++);
					}
				}
			}
			// swaps
			int dist = great - less;
			if (dist < 13)
			{
				div++;
			}
			Swap(a, less - 1, left);
			Swap(a, great + 1, right);
			// subarrays
			dualPivotQuicksort(a, left, less - 2, div);
			dualPivotQuicksort(a, great + 2, right, div);
			// equal elements
			if (dist > len - 13 && pivot1 != pivot2)
			{
				for (int k = less; k <= great; k++)
				{
					if (a[k] == pivot1)
					{
						Swap(a, k, less++);
					}
					else if (a[k] == pivot2)
					{
						Swap(a, k, great--);
						if (a[k] == pivot1)
						{
							Swap(a, k, less++);
						}
					}
				}
			}
			// subarray
			if (pivot1 < pivot2)
			{
				dualPivotQuicksort(a, less, great, div);
			}
		}

		#endregion

		public static void HeapSort(int[] input)
		{
			//Build-Max-Heap
			int heapSize = input.Length;
			for (int p = (heapSize - 1) / 2; p >= 0; p--)
				MaxHeapify(input, heapSize, p);

			for (int i = input.Length - 1; i > 0; i--)
			{
				//Swap
				int temp = input[i];
				input[i] = input[0];
				input[0] = temp;

				heapSize--;
				MaxHeapify(input, heapSize, 0);
			}
		}

		private static void MaxHeapify(int[] input, int heapSize, int index)
		{
			int left = (index + 1) * 2 - 1;
			int right = (index + 1) * 2;
			int largest = 0;

			if (left < heapSize && input[left] > input[index])
				largest = left;
			else
				largest = index;

			if (right < heapSize && input[right] > input[largest])
				largest = right;

			if (largest != index)
			{
				int temp = input[index];
				input[index] = input[largest];
				input[largest] = temp;

				MaxHeapify(input, heapSize, largest);
			}
		}

		private static void ShellSort(int[] array)
		{
			int n = array.Length;
			int gap = n / 2;
			int temp;

			while (gap > 0)
			{
				for (int i = 0; i + gap < n; i++)
				{
					int j = i + gap;
					temp = array[j];

					while (j - gap >= 0 && temp < array[j - gap])
					{
						array[j] = array[j - gap];
						j = j - gap;
					}

					array[j] = temp;
				}

				gap = gap / 2;
			}
		}

		static void RadixSort(int[] arr)
		{
			int i, j;
			int[] tmp = new int[arr.Length];
			for (int shift = 31; shift > -1; --shift)
			{
				j = 0;
				for (i = 0; i < arr.Length; ++i)
				{
					bool move = (arr[i] << shift) >= 0;
					if (shift == 0 ? !move : move)
						arr[i - j] = arr[i];
					else
						tmp[j++] = arr[i];
				}
				Array.Copy(tmp, 0, arr, arr.Length - j, j);
				//Buffer.BlockCopy(tmp, 0, arr, arr.Length - j, j);
			}
		}

		static int partition(int[] array, int start, int end)
		{
			int temp; //swap helper
			int marker = start; //divides left and right subarrays
			for (int i = start; i <= end; i++)
			{
				if (array[i] < array[end]) //array[end] is pivot
				{
					temp = array[marker]; // swap
					array[marker] = array[i];
					array[i] = temp;
					marker += 1;
				}
			}
			//put pivot(array[end]) between left and right subarrays
			temp = array[marker];
			array[marker] = array[end];
			array[end] = temp;
			return marker;
		}

		static void SortQuick(int[] array, int start, int end)
		{
			if (start >= end)
			{
				return;
			}
			int pivot = partition(array, start, end);
			SortQuick(array, start, pivot - 1);
			SortQuick(array, pivot + 1, end);
		}

		static int[] GetRandomArray(int min, int max, int count)
		{
			int[] test2 = new int[count];

			Random randNum = new Random();
			for (int i = 0; i < test2.Length; i++)
				test2[i] = randNum.Next(min, max);

			return test2;
		}

		static int N = 100000;
		//static int min = Int32.MinValue + 1;
		//static int max = Int32.MaxValue - 1;

		static int TESTSETCOUNTS = 50;
		static int min = 0;
		static int max = N / 100;

		static void Main(string[] args)
		{

			Console.WriteLine($"min: '{min}', max: '{max}'");
			List<int[]> etalonTestSet = new List<int[]>();
			for (int i = 0; i < TESTSETCOUNTS; i++)
				etalonTestSet.Add(GetRandomArray(min, max, N));

			List<int[]> testSet = etalonTestSet.Select(intse => intse.ToArray()).ToList();
			List<Stopwatch> testWatchers = testQuickSort(testSet);
			report(testWatchers);

			testSet = etalonTestSet.Select(intse => intse.ToArray()).ToList();
			testWatchers = testRadix(testSet);
			report(testWatchers);

			testSet = etalonTestSet.Select(intse => intse.ToArray()).ToList();
			testWatchers = testDualPivotQuickSort(testSet);
			report(testWatchers);

			testSet = etalonTestSet.Select(intse => intse.ToArray()).ToList();
			testWatchers = testShellSort(testSet);
			report(testWatchers);

			testSet = etalonTestSet.Select(intse => intse.ToArray()).ToList();
			testWatchers = testPHeapSort(testSet);
			report(testWatchers);


			//Console.WriteLine("Press any key to exit:");
			//Console.ReadKey();
		}



		private static void report(List<Stopwatch> testWatchers)
		{
			double sumTime = testWatchers.Sum(sw => sw.ElapsedMilliseconds);
			double minTime = testWatchers.Min(sw => sw.ElapsedMilliseconds);
			Console.WriteLine($"{TESTSETCOUNTS} randomly distributed arrays of {N} length sorted for {sumTime} milliseconds");
			Console.WriteLine($"Average {sumTime/TESTSETCOUNTS} milliseconds");
			Console.WriteLine($"Min {minTime} milliseconds");
			//Console.WriteLine("Experiment elapsed times:");
			Console.WriteLine();
		}

		static List<Stopwatch> testQuickSort(List<int[]> testSet)
		{
			Console.WriteLine("QuickSort By Recursive Method");
			List<Stopwatch> testWatchers = new List<Stopwatch>();
			for (int i = 0; i < testSet.Count; i++)
			{
				GC.Collect();
				Stopwatch stopwatch = new Stopwatch();
				stopwatch.Start();
				SortQuick(testSet[i], 0, testSet[i].Length - 1);
				stopwatch.Stop();
				testWatchers.Add(stopwatch);
				ShowSortEtryInfo(i, testWatchers[i].ElapsedMilliseconds);
			}
			return testWatchers;
		}

		static List<Stopwatch> testDualPivotQuickSort(List<int[]> testSet)
		{
			Console.WriteLine("Dual pivot By Recursive Method");
			List<Stopwatch> testWatchers = new List<Stopwatch>();
			for (int i = 0; i < testSet.Count; i++)
			{
				GC.Collect();
				Stopwatch stopwatch = new Stopwatch();
				stopwatch.Start();
				DualPivotQuickSort(testSet[i]);
				stopwatch.Stop();
				testWatchers.Add(stopwatch);
				ShowSortEtryInfo(i, testWatchers[i].ElapsedMilliseconds);
			}
			return testWatchers;
		}

		static List<Stopwatch> testRadix(List<int[]> testSet)
		{
			Console.WriteLine("RadixSort Method");
			List<Stopwatch> testWatchers = new List<Stopwatch>();
			for (int i = 0; i < testSet.Count; i++)
			{
				GC.Collect();
				Stopwatch stopwatch = new Stopwatch();
				stopwatch.Start();
				RadixSort(testSet[i]);
				stopwatch.Stop();
				testWatchers.Add(stopwatch);
				ShowSortEtryInfo(i, testWatchers[i].ElapsedMilliseconds);
			}
			return testWatchers;
		}

		static List<Stopwatch> testShellSort(List<int[]> testSet)
		{
			Console.WriteLine("Shell Sort Method");
			List<Stopwatch> testWatchers = new List<Stopwatch>();
			for (int i = 0; i < testSet.Count; i++)
			{
				GC.Collect();
				Stopwatch stopwatch = new Stopwatch();
				stopwatch.Start();
				ShellSort(testSet[i]);
				stopwatch.Stop();
				testWatchers.Add(stopwatch);
				ShowSortEtryInfo(i, testWatchers[i].ElapsedMilliseconds);
			}
			return testWatchers;
		}

		//Pyramid_Sort
		static List<Stopwatch> testPHeapSort(List<int[]> testSet)
		{
			Console.WriteLine("Heap Sort Method");
			List<Stopwatch> testWatchers = new List<Stopwatch>();
			for (int i = 0; i < testSet.Count; i++)
			{
				GC.Collect();
				Stopwatch stopwatch = new Stopwatch();
				stopwatch.Start();
				HeapSort(testSet[i]);
				stopwatch.Stop();
				testWatchers.Add(stopwatch);
				ShowSortEtryInfo(i, testWatchers[i].ElapsedMilliseconds);
			}
			return testWatchers;
		}

		private static void ShowSortEtryInfo(int i, double alpsedTime)
		{
			//Console.WriteLine($"{i}: {alpsedTime} ms");
		}
	}
}
