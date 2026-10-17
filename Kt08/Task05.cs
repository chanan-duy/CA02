namespace CA02.Kt08;

public static class Task05
{
	/*
	Длина и число возрастающих подпоследовательностей

	Дан массив a из n целых чисел. Выберите последовательность индексов i1 < i2 < ... <
	ik, для которой a[i1] < a[i2] < ... < a[ik]. Найдите максимальное значение k и
	количество последовательностей индексов такой длины. Две последовательности
	различны, если хотя бы в одной позиции содержат разные индексы.

	Входные данные:
	В первой строке задано n. Во второй строке заданы n целых чисел массива a.

	Выходные данные:
	Выведите максимальную длину и количество последовательностей этой длины по модулю 1
	000 000 007.

	Ограничения:
	1 <= n <= 200 000, -10^9 <= a[i] <= 10^9.
	*/
	public static (int Length, int Count) CountLongestIncreasingSubsequences(int[] nums)
	{
		// дерево fenwick объединяет лучшие длины и число способов для всех меньших значений.
		// запрос заканчивается перед рангом текущего числа, поэтому равные значения не продолжают друг друга.
		// одинаковые значения обновляются отдельно, поскольку разные индексы задают разные подпоследовательности
		const int modulus = 1_000_000_007;
		var sortedNums = nums.Distinct().Order().ToArray();
		var bestSeqs = new (int Length, int Count)[sortedNums.Length + 1];
		var longest = (Length: 0, Count: 0);

		static (int Length, int Count) Merge((int Length, int Count) first, (int Length, int Count) second)
		{
			if (first.Length != second.Length)
			{
				return first.Length > second.Length ? first : second;
			}

			return (first.Length, (first.Count + second.Count) % modulus);
		}

		foreach (var number in nums)
		{
			var rank = Array.BinarySearch(sortedNums, number) + 1;
			var prev = (Length: 0, Count: 0);

			for (var index = rank - 1; index > 0; index -= index & -index)
			{
				prev = Merge(prev, bestSeqs[index]);
			}

			var current = (Length: prev.Length + 1, Count: prev.Length == 0 ? 1 : prev.Count);

			for (var index = rank; index < bestSeqs.Length; index += index & -index)
			{
				bestSeqs[index] = Merge(bestSeqs[index], current);
			}

			longest = Merge(longest, current);
		}

		return longest;
	}
}

public sealed class Task05Tests
{
	public static TheoryData<int[], int, int> Cases => new()
	{
		{ [1, 3, 5, 4, 7], 4, 2 },
		{ [2, 2, 2, 2, 2], 1, 5 },
		{ [1, 2, 3], 3, 1 },
		{ [3, 2, 1], 1, 3 },
		{ [1, 1, 2, 2, 3, 3], 3, 8 },
		{ [-1_000_000_000, 0, 1_000_000_000], 3, 1 },
	};

	[Theory]
	[MemberData(nameof(Cases))]
	public void FindsLongestLengthAndNumberOfIndexSequences(int[] nums, int expectedLength, int expectedCount)
	{
		var actual = Task05.CountLongestIncreasingSubsequences(nums);

		Assert.Equal(expectedLength, actual.Length);
		Assert.Equal(expectedCount, actual.Count);
	}
}
