namespace CA02.Kt09;

public static class Task05
{
	/*
	Максимальная сумма пути с экранами

	Дана таблица n x m, в клетке (i, j) записано число a[i][j]. Путешественник начинает
	в (1, 1) и может перемещаться только вправо или вниз до (n, m). Значения всех
	посещённых клеток суммируются. Не более k раз можно применить экран к посещённой
	клетке с отрицательным значением, и тогда значение этой клетки считается равным
	нулю. Экран разрешается применять к начальной и конечной клеткам. Найдите
	максимально возможную сумму.

	Входные данные:
	В первой строке заданы n, m и k. В следующих n строках задано по m целых чисел.

	Выходные данные:
	Выведите максимально возможную сумму.

	Ограничения:
	1 <= n, m <= 500, 0 <= k <= 10, -10^9 <= a[i][j] <= 10^9, n * m * (k + 1) <= 5 000
	000. Ответ помещается в 64-битный знаковый целый тип.
	*/
	public static long MaximumShieldedPath(int[][] grid, int shieldCount)
	{
		// состояние различает точное число использованных экранов, чтобы не превысить доступное количество.
		// экран убирает только отрицательное значение, включая начальную и конечную клетки.
		// число экранов обходим по убыванию, чтобы переход из верхней клетки не использовал уже изменённое состояние
		var colCount = grid[0].Length;
		var sums = new long[colCount, shieldCount + 1];

		for (var col = 0; col < colCount; col++)
		{
			for (var used = 0; used <= shieldCount; used++)
			{
				sums[col, used] = long.MinValue;
			}
		}

		sums[0, 0] = 0;

		foreach (var row in grid)
		{
			for (var col = 0; col < colCount; col++)
			{
				for (var used = shieldCount; used >= 0; used--)
				{
					var prev = Math.Max(sums[col, used], col > 0 ? sums[col - 1, used] : long.MinValue);
					var best = prev == long.MinValue ? long.MinValue : prev + row[col];

					if (row[col] < 0 && used > 0)
					{
						var shielded = Math.Max(sums[col, used - 1],
							col > 0 ? sums[col - 1, used - 1] : long.MinValue);
						best = Math.Max(best, shielded);
					}

					sums[col, used] = best;
				}
			}
		}

		return Enumerable.Range(0, shieldCount + 1).Max(used => sums[colCount - 1, used]);
	}
}

public sealed class Task05Tests
{
	public static TheoryData<int[][], int, long> Cases => new()
	{
		{ [[-5]], 0, -5 },
		{ [[-5]], 1, 0 },
		{ [[5]], 2, 5 },
		{ [[1, -10], [-2, 3]], 1, 4 },
		{ [[-5, -2, -3]], 2, -2 },
		{ [[-5, -2, -3]], 10, 0 },
		{ [[1_000_000_000, 1_000_000_000], [1_000_000_000, 1_000_000_000]], 0, 3_000_000_000 },
		{ [[-1, 4], [3, -2]], 2, 4 },
	};

	[Theory]
	[MemberData(nameof(Cases))]
	public void FindsBestPathUsingAtMostTheAvailableShields(int[][] grid, int shieldCount, long expected)
	{
		var actual = Task05.MaximumShieldedPath(grid, shieldCount);

		Assert.Equal(expected, actual);
	}
}
