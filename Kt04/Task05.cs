namespace CA02.Kt04;

public static class Task05
{
	/*
	k-я наименьшая сумма пары

	Даны два массива first и second, отсортированные по неубыванию.
	Для каждой пары индексов i и j рассматривается сумма first[i] + second[j].
	Одинаковые суммы от разных пар учитываются отдельно.
	Найдите k-ю наименьшую сумму среди всех пар.

	Входные данные:
	В первой строке заданы n, m и k. Во второй строке заданы n целых чисел
	массива first. В третьей строке заданы m целых чисел массива second.

	Выходные данные:
	k-я наименьшая сумма пары.

	Ограничения:
	1 <= n, m <= 200 000
	1 <= k <= min(n * m, 200 000)
	-10^9 <= first[i], second[j] <= 10^9
	*/
	public static long KthSmallestPairSum(int[] first, int[] second, int rank)
	{
		// для каждого first[i] суммы с second идут по возрастанию, поэтому объединяем эти последовательности через priority queue.
		// после извлечения пары добавляем следующую с тем же первым индексом, а одинаковые суммы остаются отдельными элементами.
		// достаточно первых rank последовательностей: перед любой более поздней уже есть rank не больших сумм
		PriorityQueue<(int Left, int Right), long> pendingPairs = new();

		for (var index = 0; index < Math.Min(first.Length, rank); index++)
		{
			pendingPairs.Enqueue((index, 0), (long)first[index] + second[0]);
		}

		long currentSum = 0;

		for (var index = 0; index < rank; index++)
		{
			var (left, right) = pendingPairs.Dequeue();
			currentSum = (long)first[left] + second[right];

			if (right + 1 < second.Length)
			{
				pendingPairs.Enqueue((left, right + 1), (long)first[left] + second[right + 1]);
			}
		}

		return currentSum;
	}
}

public sealed class Task05Tests
{
	public static TheoryData<int[], int[], int, long> Cases => new()
	{
		{ [1, 7, 11], [2, 4, 6], 3, 7 },
		{ [1, 2], [3, 4], 3, 5 },
		{ [1, 2], [3, 4], 4, 6 },
		{ [5], [-2], 1, 3 },
		{ [0, 0], [0, 0], 4, 0 },
		{ [-3, -1], [-2, 4], 3, 1 },
		{ [1, 2, 3, 4], [10], 2, 12 },
		{ [10], [1, 2, 3, 4], 4, 14 },
		{ [-1_000_000_000], [-1_000_000_000], 1, -2_000_000_000 },
		{ [1_000_000_000], [1_000_000_000], 1, 2_000_000_000 },
	};

	[Theory]
	[MemberData(nameof(Cases))]
	public void FindsKthSmallestPairSum(int[] first, int[] second, int rank, long expected)
	{
		var actual = Task05.KthSmallestPairSum(first, second, rank);

		Assert.Equal(expected, actual);
	}

	[Fact]
	public void SupportsMaximumArraySizesAndRank()
	{
		var first = Enumerable.Repeat(1_000_000_000, 200_000).ToArray();
		var second = Enumerable.Repeat(1_000_000_000, 200_000).ToArray();

		var actual = Task05.KthSmallestPairSum(first, second, 200_000);

		Assert.Equal(2_000_000_000, actual);
	}
}
