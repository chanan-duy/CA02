namespace CA02.Kt07;

public static class Task04
{
	/*
	Кратчайший путь с одним купоном

	Дана ориентированная сеть платных дорог. Не более одного раза можно применить купон
	к дороге и заплатить целую часть от w / 2 вместо w. Купон можно не использовать.
	Найдите минимальную стоимость маршрута из s в t.

	Входные данные:
	В первой строке заданы n, m, s и t. В следующих m строках заданы u, v и w -
	направленная дорога из u в v стоимости w.

	Выходные данные:
	Выведите минимальную стоимость маршрута из s в t. Если t недостижим, выведите -1.

	Ограничения:
	1 <= n <= 200 000, 0 <= m <= 400 000, 1 <= s, t, u, v <= n, 0 <= w <= 10^9. Если s =
	t, пустой маршрут имеет стоимость 0. Ответ может не помещаться в 32-битный целый
	тип.
	*/
	public static long MinimumDiscountedCost(int cityCount, int[][] roads, int start, int finish)
	{
		// вершина имеет два состояния: купон ещё доступен или уже использован.
		// переход со скидкой возможен только из первого состояния, поэтому два применения не попадут в ответ.
		// обычные переходы сохраняют состояние, а минимум в конце учитывает право не использовать купон
		var nextCities = Enumerable.Range(0, cityCount + 1)
			.Select(_ => new List<(int City, int Cost)>()).ToArray();

		foreach (var road in roads)
		{
			nextCities[road[0]].Add((road[1], road[2]));
		}

		var dists = new long[cityCount + 1, 2];

		for (var city = 0; city <= cityCount; city++)
		{
			dists[city, 0] = long.MaxValue;
			dists[city, 1] = long.MaxValue;
		}

		dists[start, 0] = 0;
		PriorityQueue<(int City, int Used), long> pendingCities = new();
		pendingCities.Enqueue((start, 0), 0);

		while (pendingCities.TryDequeue(out var current, out var dist))
		{
			if (dist != dists[current.City, current.Used])
			{
				continue;
			}

			foreach (var (next, cost) in nextCities[current.City])
			{
				var candidate = dist + cost;

				if (candidate < dists[next, current.Used])
				{
					dists[next, current.Used] = candidate;
					pendingCities.Enqueue((next, current.Used), candidate);
				}

				if (current.Used == 0 && dist + cost / 2 < dists[next, 1])
				{
					dists[next, 1] = dist + cost / 2;
					pendingCities.Enqueue((next, 1), dists[next, 1]);
				}
			}
		}

		var minimum = Math.Min(dists[finish, 0], dists[finish, 1]);

		return minimum == long.MaxValue ? -1 : minimum;
	}
}

public sealed class Task04Tests
{
	public static TheoryData<int, int[][], int, int, long> Cases => new()
	{
		{ 3, [[1, 2, 5], [2, 3, 5], [1, 3, 20]], 1, 3, 7 },
		{ 2, [[1, 2, 1]], 1, 2, 0 },
		{ 2, [], 1, 2, -1 },
		{ 1, [], 1, 1, 0 },
		{ 3, [[1, 2, 0], [2, 3, 0]], 1, 3, 0 },
		{
			4, [[1, 2, 1_000_000_000], [2, 3, 1_000_000_000], [3, 4, 1_000_000_000]],
			1, 4, 2_500_000_000
		},
	};

	[Theory]
	[MemberData(nameof(Cases))]
	public void FindsCheapestRouteUsingAtMostOneCoupon(int cityCount, int[][] roads, int start, int finish, long expected)
	{
		var actual = Task04.MinimumDiscountedCost(cityCount, roads, start, finish);

		Assert.Equal(expected, actual);
	}
}
