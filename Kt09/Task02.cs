namespace CA02.Kt09;

public static class Task02
{
	/*
	Количество маршрутов по карте

	Дана прямоугольная карта. Символ . обозначает свободную клетку, символ # -
	заблокированную. Требуется попасть из клетки (1, 1) в клетку (n, m). За один ход
	разрешается перейти на одну клетку вправо или вниз. Определите количество различных
	допустимых маршрутов.

	Входные данные:
	В первой строке заданы n и m. В следующих n строках задано по m символов карты без
	пробелов.

	Выходные данные:
	Выведите количество маршрутов по модулю 1 000 000 007. Если начальная или конечная
	клетка заблокирована, выведите 0.

	Ограничения:
	1 <= n, m <= 2 000, n * m <= 2 000 000.
	*/
	public static int CountGridRoutes(string[] map)
	{
		// в клетку можно прийти только сверху или слева, поэтому складываем числа способов этих двух клеток.
		// заблокированная клетка обнуляет состояние и не пропускает маршруты дальше.
		// одна строка хранит старый ответ сверху и уже обновлённый ответ слева
		const int modulus = 1_000_000_007;
		var routeCounts = new int[map[0].Length];
		routeCounts[0] = 1;

		foreach (var row in map)
		{
			for (var col = 0; col < row.Length; col++)
			{
				if (row[col] == '#')
				{
					routeCounts[col] = 0;
				}
				else if (col > 0)
				{
					routeCounts[col] = (routeCounts[col] + routeCounts[col - 1]) % modulus;
				}
			}
		}

		return routeCounts[^1];
	}
}

public sealed class Task02Tests
{
	public static TheoryData<string[], int> Cases => new()
	{
		{ ["."], 1 },
		{ ["#"], 0 },
		{ ["...", "...", "..."], 6 },
		{ ["...", ".#.", "..."], 2 },
		{ ["#.", ".."], 0 },
		{ ["..", ".#"], 0 },
		{ [".#."], 0 },
		{ [".", ".", "."], 1 },
	};

	[Theory]
	[MemberData(nameof(Cases))]
	public void CountsRoutesAroundBlockedCells(string[] map, int expected)
	{
		var actual = Task02.CountGridRoutes(map);

		Assert.Equal(expected, actual);
	}
}
