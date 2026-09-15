namespace CA02.Kt02;

public static class Task02
{
	/*
	Суммы на отрезках массива

	Дан массив целых чисел nums и q запросов. Каждый запрос содержит два индекса
	L и R. Для каждого запроса определите сумму элементов массива с индексами
	от L до R включительно.

	Входные данные:
	В первой строке задано n. Во второй строке заданы n целых чисел nums.
	В третьей строке задано q. В следующих q строках заданы пары L и R.

	Выходные данные:
	Сумма элементов для каждого запроса в исходном порядке.

	Ограничения:
	1 <= n, q <= 200 000
	-10^9 <= nums[i] <= 10^9
	0 <= L <= R < n
	*/
	public static long[] RangeSums(int[] nums, int[][] queries)
	{
		// повторный проход по каждому отрезку может потребовать n * q операций.
		// поэтому заранее считаем prefixSums: сумма [left..right] равна prefixSums[right + 1] - prefixSums[left].
		// нулевой элемент хранит сумму пустого префикса, а long нужен для сумм за пределами int
		var prefixSums = new long[nums.Length + 1];

		for (var index = 0; index < nums.Length; index++)
		{
			prefixSums[index + 1] = prefixSums[index] + nums[index];
		}

		var sums = new long[queries.Length];

		for (var index = 0; index < queries.Length; index++)
		{
			var left = queries[index][0];
			var right = queries[index][1];
			sums[index] = prefixSums[right + 1] - prefixSums[left];
		}

		return sums;
	}
}

public sealed class Task02Tests
{
	public static TheoryData<int[], int[][], long[]> Cases => new()
	{
		{ [1, -2, 3, 4], [[0, 3], [1, 2], [2, 2], [0, 0]], [6, 1, 3, 1] },
		{ [-5], [[0, 0]], [-5] },
		{ [0, 0], [[0, 1], [1, 1]], [0, 0] },
		{ [-3, -2, -1], [[0, 2], [1, 2], [0, 2]], [-6, -3, -6] },
		{ [1_000_000_000, 1_000_000_000, 1_000_000_000], [[0, 2]], [3_000_000_000] },
	};

	[Theory]
	[MemberData(nameof(Cases))]
	public void FindsRequestedRangeSums(int[] nums, int[][] queries, long[] expected)
	{
		var actual = Task02.RangeSums(nums, queries);

		Assert.Equal(expected, actual);
	}

	[Fact]
	public void SupportsMaximumInputSize()
	{
		var nums = Enumerable.Repeat(-1_000_000_000, 200_000).ToArray();
		var queries = Enumerable.Range(0, 200_000).Select(index => new[] { 0, index }).ToArray();

		var actual = Task02.RangeSums(nums, queries);

		Assert.Equal(200_000, actual.Length);
		for (var index = 0; index < actual.Length; index++)
		{
			Assert.Equal(-1_000_000_000L * (index + 1), actual[index]);
		}
	}
}
