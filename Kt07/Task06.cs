namespace CA02.Kt07;

public static class Task06
{
	/*
	Минимальная стоимость и число маршрутов с ограничением длины

	Дан ориентированный граф с целыми весами рёбер. Найдите минимальную стоимость
	маршрута из s в t не более чем из K рёбер и количество маршрутов с такой стоимостью.
	Вершины и рёбра можно посещать повторно. Маршруты различны, если различаются
	последовательности номеров рёбер; номера задаются порядком ввода.

	Входные данные:
	В первой строке заданы n, m, s, t и K. В следующих m строках заданы u, v и w -
	направленное ребро из u в v веса w.

	Выходные данные:
	Если маршрута нет, выведите IMPOSSIBLE. Иначе выведите минимальную стоимость и
	количество таких маршрутов по модулю 1 000 000 007.

	Ограничения:
	1 <= n <= 2 000, 0 <= m <= 10 000, 0 <= K <= 500, 1 <= s, t, u, v <= n, -10^9 <= w
	<= 10^9. При s = t допустим пустой маршрут стоимости 0.
	*/
	public static (long Cost, int Count)? CheapestLimitedRoutes(int vertexCount, int[][] edges, int start, int finish, int edgeLimit)
	{
		// каждый слой хранит маршруты ровно из заданного числа рёбер, чтобы один маршрут не считался несколько раз.
		// параллельные рёбра обрабатываем отдельно, поскольку их номера задают разные маршруты.
		// минимум и число способов объединяем по всем слоям, включая пустой маршрут при совпадении концов
		const int modulus = 1_000_000_007;
		var dists = Enumerable.Repeat(long.MaxValue, vertexCount + 1).ToArray();
		var counts = new int[vertexCount + 1];
		dists[start] = 0;
		counts[start] = 1;
		var minimum = start == finish ? 0 : long.MaxValue;
		var routeCount = start == finish ? 1 : 0;

		for (var length = 1; length <= edgeLimit; length++)
		{
			var nextDists = Enumerable.Repeat(long.MaxValue, vertexCount + 1).ToArray();
			var nextCounts = new int[vertexCount + 1];

			foreach (var edge in edges)
			{
				if (dists[edge[0]] == long.MaxValue)
				{
					continue;
				}

				var candidate = dists[edge[0]] + edge[2];

				if (candidate < nextDists[edge[1]])
				{
					nextDists[edge[1]] = candidate;
					nextCounts[edge[1]] = counts[edge[0]];
				}
				else if (candidate == nextDists[edge[1]])
				{
					nextCounts[edge[1]] = (nextCounts[edge[1]] + counts[edge[0]]) % modulus;
				}
			}

			if (nextDists[finish] < minimum)
			{
				minimum = nextDists[finish];
				routeCount = nextCounts[finish];
			}
			else if (nextDists[finish] == minimum)
			{
				routeCount = (routeCount + nextCounts[finish]) % modulus;
			}

			dists = nextDists;
			counts = nextCounts;
		}

		return minimum == long.MaxValue ? null : (minimum, routeCount);
	}
}

public sealed class Task06Tests
{
	public static TheoryData<int, int[][], int, int, int, long?, int> Cases => new()
	{
		{ 2, [[1, 2, 3], [1, 2, 3]], 1, 2, 1, 3, 2 },
		{ 3, [[1, 3, 5], [1, 2, 2], [2, 3, 3]], 1, 3, 2, 5, 2 },
		{ 1, [[1, 1, 0]], 1, 1, 3, 0, 4 },
		{ 1, [[1, 1, -2]], 1, 1, 3, -6, 1 },
		{ 2, [[1, 2, 1]], 1, 2, 0, null, 0 },
		{ 1, [], 1, 1, 0, 0, 1 },
		{ 2, [], 1, 2, 4, null, 0 },
	};

	[Theory]
	[MemberData(nameof(Cases))]
	public void FindsMinimumAndCountsDistinctEdgeSequences(
		int vertexCount, int[][] edges, int start,
		int finish, int edgeLimit, long? expectedCost, int expectedCount
	)
	{
		var actual = Task06.CheapestLimitedRoutes(vertexCount, edges, start, finish, edgeLimit);

		Assert.Equal(expectedCost, actual?.Cost);
		Assert.Equal(expectedCount, actual?.Count ?? 0);
	}
}
