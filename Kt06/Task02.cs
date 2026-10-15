namespace CA02.Kt06;

public static class Task02
{
	/*
	Связность станций после добавления соединений

	Изначально имеется n изолированных станций. Операция 1 u v создаёт двустороннюю
	связь между станциями u и v. Операция 2 u v запрашивает, можно ли добраться из u в v
	по уже созданным связям. Связи не удаляются и могут добавляться повторно.

	Входные данные:
	В первой строке заданы n и q. В следующих q строках записаны операции в формате type
	u v.

	Выходные данные:
	Для каждой операции второго типа выведите YES, если станции связаны, иначе NO.
	Станция всегда связана сама с собой.

	Ограничения:
	1 <= n, q <= 200 000, type равен 1 или 2, 1 <= u, v <= n.
	*/
	public static bool[] ConnectionQueries(int stationCount, int[][] ops)
	{
		// связи только добавляются, поэтому достаточно объединять компоненты через dsu.
		// меньшую компоненту подвешиваем к большей, чтобы дерево родителей не превращалось в длинную цепочку.
		// сжатие пути сохраняет найденный корень для следующих запросов
		var parents = Enumerable.Range(0, stationCount + 1).ToArray();
		var sizes = Enumerable.Repeat(1, stationCount + 1).ToArray();
		List<bool> answers = [];

		int Find(int station)
		{
			while (station != parents[station])
			{
				parents[station] = parents[parents[station]];
				station = parents[station];
			}

			return station;
		}

		foreach (var op in ops)
		{
			var firstRoot = Find(op[1]);
			var secondRoot = Find(op[2]);

			if (op[0] == 2)
			{
				answers.Add(firstRoot == secondRoot);
				continue;
			}

			if (firstRoot == secondRoot)
			{
				continue;
			}

			if (sizes[firstRoot] < sizes[secondRoot])
			{
				(firstRoot, secondRoot) = (secondRoot, firstRoot);
			}

			parents[secondRoot] = firstRoot;
			sizes[firstRoot] += sizes[secondRoot];
		}

		return answers.ToArray();
	}
}

public sealed class Task02Tests
{
	public static TheoryData<int, int[][], bool[]> Cases => new()
	{
		{ 1, [[2, 1, 1]], [true] },
		{
			3, [[2, 1, 2], [1, 1, 2], [2, 1, 2], [2, 1, 3], [1, 2, 3], [2, 1, 3]],
			[false, true, false, true]
		},
		{ 2, [[1, 1, 2], [1, 2, 1], [1, 1, 1], [2, 2, 1]], [true] },
		{ 4, [[1, 1, 2], [1, 3, 4], [2, 1, 4], [1, 2, 3], [2, 1, 4]], [false, true] },
	};

	[Theory]
	[MemberData(nameof(Cases))]
	public void AnswersConnectivityQueries(int stationCount, int[][] ops, bool[] expected)
	{
		var actual = Task02.ConnectionQueries(stationCount, ops);

		Assert.Equal(expected, actual);
	}
}
