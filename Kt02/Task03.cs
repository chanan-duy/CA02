namespace CA02.Kt02;

public static class Task03
{
	/*
	Объединение интервалов

	Дан набор закрытых интервалов [start, end]. Объедините все интервалы,
	которые пересекаются или имеют общую границу. Верните итоговый набор
	непересекающихся интервалов, упорядоченный по возрастанию начала.

	Входные данные:
	В первой строке задано n. В следующих n строках заданы пары start и end.

	Выходные данные:
	Набор объединённых интервалов по возрастанию начала.

	Ограничения:
	0 <= n <= 200 000
	-10^9 <= start <= end <= 10^9
	*/
	public static List<List<int>> MergeIntervals(int[][] intervals)
	{
		// сортируем по началу: следующий интервал может продолжить только последний из уже объединённых.
		// общая граница тоже считается пересечением, поэтому новый интервал нужен только при start > end.
		// результат хранит отдельные списки, чтобы расширение границы не меняло входные интервалы
		var sortedIntervals = intervals.OrderBy(interval => interval[0]).ToArray();
		List<List<int>> mergedIntervals = [];

		foreach (var interval in sortedIntervals)
		{
			if (mergedIntervals.Count == 0 || interval[0] > mergedIntervals[^1][1])
			{
				mergedIntervals.Add([interval[0], interval[1]]);
				continue;
			}

			mergedIntervals[^1][1] = Math.Max(mergedIntervals[^1][1], interval[1]);
		}

		return mergedIntervals;
	}
}

public sealed class Task03Tests
{
	public static TheoryData<int[][], int[][]> Cases => new()
	{
		{ [[1, 3], [2, 6], [8, 10], [15, 18]], [[1, 6], [8, 10], [15, 18]] },
		{ [[4, 5], [1, 4]], [[1, 5]] },
		{ [], [] },
		{ [[2, 2]], [[2, 2]] },
		{ [[1, 10], [2, 3], [1, 10]], [[1, 10]] },
		{ [[5, 7], [1, 3], [3, 5]], [[1, 7]] },
		{ [[1, 2], [3, 4]], [[1, 2], [3, 4]] },
		{ [[0, 0], [-1_000_000_000, 0], [0, 1_000_000_000]], [[-1_000_000_000, 1_000_000_000]] },
	};

	[Theory]
	[MemberData(nameof(Cases))]
	public void MergesOverlappingIntervals(int[][] intervals, int[][] expected)
	{
		var actual = Task03.MergeIntervals(intervals);

		Assert.Equal(expected.Length, actual.Count);
		for (var index = 0; index < expected.Length; index++)
		{
			Assert.Equal(expected[index], actual[index]);
		}
	}

	[Fact]
	public void DoesNotChangeInputIntervals()
	{
		int[][] intervals = [[4, 5], [1, 4], [2, 3]];
		var original = intervals.Select(interval => interval.ToArray()).ToArray();

		Task03.MergeIntervals(intervals);

		for (var index = 0; index < intervals.Length; index++)
		{
			Assert.Equal(original[index], intervals[index]);
		}
	}
}
