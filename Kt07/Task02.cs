namespace CA02.Kt07;

public static class Task02
{
	/*
	Кратчайшие пути с отрицательными весами

	Дана ориентированная сеть, в которой стоимость некоторых переходов может быть
	отрицательной. Гарантируется, что из начальной вершины s нельзя добраться до цикла с
	отрицательной суммарной стоимостью. Для каждой вершины найдите минимальную стоимость
	маршрута из s.

	Входные данные:
	В первой строке заданы n, m и s. В следующих m строках заданы u, v и w -
	направленный переход из u в v стоимости w.

	Выходные данные:
	Выведите n значений для вершин от 1 до n. Для достижимой вершины выведите
	минимальную стоимость, для недостижимой - INF.

	Ограничения:
	1 <= n <= 2 500, 0 <= m <= 5 000, 1 <= s, u, v <= n, -10^9 <= w <= 10^9.
	Параллельные переходы и петли допускаются. Ответы требуется хранить в 64-битном
	целом типе.
	*/
	public static long?[] ShortestPathCosts(int vertexCount, int[][] edges, int start)
	{
		// отрицательный вес может улучшить уже просмотренную вершину, поэтому используем bellman-ford.
		// без достижимого отрицательного цикла кратчайший путь содержит не больше vertexCount - 1 рёбер.
		// недостижимую вершину не расслабляем, чтобы её исходящие рёбра не создавали ложных путей
		var dists = Enumerable.Repeat(long.MaxValue, vertexCount + 1).ToArray();
		dists[start] = 0;

		for (var pass = 1; pass < vertexCount; pass++)
		{
			var changed = false;

			foreach (var edge in edges)
			{
				if (dists[edge[0]] == long.MaxValue)
				{
					continue;
				}

				var candidate = dists[edge[0]] + edge[2];

				if (candidate < dists[edge[1]])
				{
					dists[edge[1]] = candidate;
					changed = true;
				}
			}

			if (!changed)
			{
				break;
			}
		}

		return dists.Skip(1).Select(dist => dist == long.MaxValue ? (long?)null : dist).ToArray();
	}
}

public sealed class Task02Tests
{
	public static TheoryData<int, int[][], int, long?[]> Cases => new()
	{
		{ 3, [[1, 2, 5], [1, 3, 2], [2, 3, -8]], 1, [0, 5, -3] },
		{ 4, [[3, 4, -1], [4, 3, -1], [1, 2, 7]], 1, [0, 7, null, null] },
		{ 3, [[2, 3, -2], [1, 2, 1]], 1, [0, 1, -1] },
		{ 1, [[1, 1, 0]], 1, [0] },
		{
			4, [[1, 2, -1_000_000_000], [2, 3, -1_000_000_000], [3, 4, -1_000_000_000]],
			1, [0, -1_000_000_000, -2_000_000_000, -3_000_000_000]
		},
	};

	[Theory]
	[MemberData(nameof(Cases))]
	public void FindsShortestCostsWithNegativeEdges(int vertexCount, int[][] edges, int start, long?[] expected)
	{
		var actual = Task02.ShortestPathCosts(vertexCount, edges, start);

		Assert.Equal(expected, actual);
	}
}
