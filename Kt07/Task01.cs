namespace CA02.Kt07;

public static class Task01
{
	/*
	Кратчайшее время пути

	Дана ориентированная сеть из n городов и m дорог. Проезд по каждой дороге занимает
	неотрицательное время. Для каждого города найдите минимальное время пути из города
	s.

	Входные данные:
	В первой строке заданы n, m и s. В следующих m строках заданы u, v и w -
	направленная дорога из u в v со временем проезда w.

	Выходные данные:
	Выведите n значений для городов от 1 до n. Для достижимого города выведите
	минимальное время пути, для недостижимого - INF.

	Ограничения:
	1 <= n <= 200 000, 0 <= m <= 400 000, 1 <= s, u, v <= n, 0 <= w <= 10^9.
	Параллельные дороги и петли допускаются. Ответы могут не помещаться в 32-битный
	целый тип.
	*/
	public static long?[] ShortestTravelTimes(int cityCount, int[][] roads, int start)
	{
		// неотрицательные веса позволяют фиксировать кратчайшее расстояние через dijkstra.
		// в очереди могут остаться старые расстояния, поэтому перед обработкой сравниваем их с текущими.
		// long хранит сумму весов, а null в результате отличает недостижимый город от расстояния 0
		var nextCities = Enumerable.Range(0, cityCount + 1)
			.Select(_ => new List<(int City, int Time)>()).ToArray();

		foreach (var road in roads)
		{
			nextCities[road[0]].Add((road[1], road[2]));
		}

		var dists = Enumerable.Repeat(long.MaxValue, cityCount + 1).ToArray();
		dists[start] = 0;
		PriorityQueue<int, long> pendingCities = new();
		pendingCities.Enqueue(start, 0);

		while (pendingCities.TryDequeue(out var city, out var dist))
		{
			if (dist != dists[city])
			{
				continue;
			}

			foreach (var (next, time) in nextCities[city])
			{
				var candidate = dist + time;

				if (candidate < dists[next])
				{
					dists[next] = candidate;
					pendingCities.Enqueue(next, candidate);
				}
			}
		}

		return dists.Skip(1).Select(dist => dist == long.MaxValue ? (long?)null : dist).ToArray();
	}
}

public sealed class Task01Tests
{
	public static TheoryData<int, int[][], int, long?[]> Cases => new()
	{
		{ 4, [[1, 2, 8], [1, 3, 1], [3, 2, 2], [2, 4, 3]], 1, [0, 3, 1, 6] },
		{ 3, [], 2, [null, 0, null] },
		{ 2, [[1, 1, 0], [1, 2, 9], [1, 2, 0], [2, 1, 2]], 1, [0, 0] },
		{
			4, [[1, 2, 1_000_000_000], [2, 3, 1_000_000_000], [3, 4, 1_000_000_000]],
			1, [0, 1_000_000_000, 2_000_000_000, 3_000_000_000]
		},
	};

	[Theory]
	[MemberData(nameof(Cases))]
	public void FindsShortestTravelTimes(int cityCount, int[][] roads, int start, long?[] expected)
	{
		var actual = Task01.ShortestTravelTimes(cityCount, roads, start);

		Assert.Equal(expected, actual);
	}
}
