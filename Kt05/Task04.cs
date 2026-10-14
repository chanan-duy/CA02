namespace CA02.Kt05;

public static class Task04
{
	/*
	Ферзи на доске с запрещёнными клетками

	На шахматной доске размера n x n некоторые клетки запрещены. Расставьте ровно n
	ферзей так, чтобы в каждой строке и каждом столбце находился ровно один ферзь,
	никакие два ферзя не находились на одной диагонали и ни один ферзь не стоял в
	запрещённой клетке. Найдите количество допустимых расстановок. Строки и столбцы
	нумеруются с 1.

	Входные данные:
	В первой строке заданы n и b - размер доски и количество запрещённых клеток. В
	следующих b строках заданы различные пары row и column.

	Выходные данные:
	Выведите количество допустимых расстановок.

	Ограничения:
	1 <= n <= 14, 0 <= b <= n^2, 1 <= row, column <= n.
	*/
	public static long CountQueenPlacements(int size, int[][] blockedCells)
	{
		// по одному ферзю в строке оставляет выбор только столбца, а битовые маски исключают занятые линии.
		// при переходе к следующей строке диагонали сдвигаются на один столбец в разные стороны.
		// запрещённые клетки исключаем из доступной маски строки до перебора
		var fullMask = (1 << size) - 1;
		var allowedCols = Enumerable.Repeat(fullMask, size).ToArray();

		foreach (var cell in blockedCells)
		{
			allowedCols[cell[0] - 1] &= ~(1 << (cell[1] - 1));
		}

		long Count(int row, int cols, int leftDiags, int rightDiags)
		{
			if (row == size)
			{
				return 1;
			}

			var available = allowedCols[row] & ~(cols | leftDiags | rightDiags);
			long placementCount = 0;

			while (available != 0)
			{
				var col = available & -available;
				available -= col;
				placementCount += Count(row + 1, cols | col,
					((leftDiags | col) << 1) & fullMask, (rightDiags | col) >> 1);
			}

			return placementCount;
		}

		return Count(0, 0, 0, 0);
	}
}

public sealed class Task04Tests
{
	public static TheoryData<int, int[][], long> Cases => new()
	{
		{ 1, [], 1 },
		{ 1, [[1, 1]], 0 },
		{ 2, [], 0 },
		{ 3, [], 0 },
		{ 4, [], 2 },
		{ 4, [[1, 2]], 1 },
		{ 4, [[1, 2], [1, 3]], 0 },
		{ 8, [], 92 },
		{ 14, [], 365_596 },
	};

	[Theory]
	[MemberData(nameof(Cases))]
	public void CountsAllowedQueenPlacements(int size, int[][] blockedCells, long expected)
	{
		var actual = Task04.CountQueenPlacements(size, blockedCells);

		Assert.Equal(expected, actual);
	}
}
