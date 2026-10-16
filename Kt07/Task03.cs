namespace CA02.Kt07;

public static class Task03
{
	/*
	Ближайший пункт помощи

	Дан неориентированный взвешенный граф. В k его вершинах расположены пункты помощи.
	Для каждой вершины определите ближайший пункт и длину кратчайшего пути до него. Если
	несколько пунктов находятся на одинаковом минимальном расстоянии, выберите пункт с
	наименьшим номером.

	Входные данные:
	В первой строке заданы n, m и k. Во второй строке заданы k различных номеров пунктов
	помощи. В следующих m строках заданы u, v и w - двусторонняя дорога между u и v
	длины w.

	Выходные данные:
	Для каждой вершины от 1 до n выведите в отдельной строке номер выбранного пункта и
	расстояние до него. Если ни один пункт недостижим, выведите -1 -1.

	Ограничения:
	1 <= n <= 200 000, 0 <= m <= 400 000, 1 <= k <= n, 1 <= u, v <= n, 1 <= w <= 10^9.
	Для самого пункта ответом являются его номер и расстояние 0.
	*/
	public static (int Station, long Distance)[] NearestHelp(int vertexCount, int[][] roads, int[] stations)
	{
		// запускаем dijkstra сразу из всех пунктов: расстояние каждой вершины выбирается среди всех источников.
		// номер пункта входит в приоритет после расстояния, поэтому равные пути выбирают меньший номер.
		// при улучшении только номера тоже продолжаем обход, чтобы передать правильный пункт дальше
		var nextVertices = Enumerable.Range(0, vertexCount + 1)
			.Select(_ => new List<(int Vertex, int Weight)>()).ToArray();

		foreach (var road in roads)
		{
			nextVertices[road[0]].Add((road[1], road[2]));
			nextVertices[road[1]].Add((road[0], road[2]));
		}

		var dists = Enumerable.Repeat(long.MaxValue, vertexCount + 1).ToArray();
		var nearestStations = Enumerable.Repeat(-1, vertexCount + 1).ToArray();
		PriorityQueue<int, (long Distance, int Station)> pendingVertices = new();

		foreach (var station in stations)
		{
			dists[station] = 0;
			nearestStations[station] = station;
			pendingVertices.Enqueue(station, (0, station));
		}

		while (pendingVertices.TryDequeue(out var vertex, out var priority))
		{
			if (priority.Distance != dists[vertex] || priority.Station != nearestStations[vertex])
			{
				continue;
			}

			foreach (var (next, weight) in nextVertices[vertex])
			{
				var candidate = priority.Distance + weight;

				if (candidate < dists[next] || (candidate == dists[next] && priority.Station < nearestStations[next]))
				{
					dists[next] = candidate;
					nearestStations[next] = priority.Station;
					pendingVertices.Enqueue(next, (candidate, priority.Station));
				}
			}
		}

		return Enumerable.Range(1, vertexCount)
			.Select(vertex => (nearestStations[vertex], dists[vertex] == long.MaxValue ? -1 : dists[vertex]))
			.ToArray();
	}
}

public sealed class Task03Tests
{
	public static TheoryData<int, int[][], int[], long[][]> Cases => new()
	{
		{ 4, [[1, 2, 2], [2, 3, 2]], [3, 1], [[1, 0], [1, 2], [3, 0], [-1, -1]] },
		{ 3, [[1, 2, 1], [2, 3, 1]], [2], [[2, 1], [2, 0], [2, 1]] },
		{ 2, [], [2, 1], [[1, 0], [2, 0]] },
		{
			4, [[1, 2, 1_000_000_000], [2, 3, 1_000_000_000], [3, 4, 1_000_000_000]],
			[1], [[1, 0], [1, 1_000_000_000], [1, 2_000_000_000], [1, 3_000_000_000]]
		},
	};

	[Theory]
	[MemberData(nameof(Cases))]
	public void FindsNearestStationWithSmallestNumber(int vertexCount, int[][] roads, int[] stations, long[][] expected)
	{
		var actual = Task03.NearestHelp(vertexCount, roads, stations);

		for (var index = 0; index < expected.Length; index++)
		{
			Assert.Equal(expected[index][0], actual[index].Station);
			Assert.Equal(expected[index][1], actual[index].Distance);
		}
	}
}
