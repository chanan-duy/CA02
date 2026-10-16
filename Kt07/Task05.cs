using System.Globalization;

namespace CA02.Kt07;

public static class Task05
{
	/*
	Конечные и неограниченные стоимости маршрутов

	Дан ориентированный граф с целыми весами рёбер и начальная вершина s. Для каждой
	вершины определите, недостижима ли она, имеет ли конечную минимальную стоимость
	маршрута или эту стоимость можно уменьшать без ограничения. В последнем случае для
	любого целого L существует маршрут из s стоимостью меньше L.

	Входные данные:
	В первой строке заданы n, m и s. В следующих m строках заданы u, v и w -
	направленное ребро из u в v веса w.

	Выходные данные:
	Для каждой вершины от 1 до n выведите отдельную строку: символ * для недостижимой
	вершины, символ - при неограниченном снижении стоимости или конечную минимальную
	стоимость в остальных случаях.

	Ограничения:
	1 <= n <= 2 500, 0 <= m <= 5 000, 1 <= s, u, v <= n, -10^9 <= w <= 10^9.
	Параллельные рёбра и петли допускаются. Конечные ответы требуется хранить в
	64-битном целом типе.
	*/
	public static string[] PathCostStates(int vertexCount, int[][] edges, int start)
	{
		// после vertexCount - 1 проходов улучшение возможно только через достижимый отрицательный цикл.
		// помечаем концы улучшаемых рёбер и все достижимые из них вершины, поскольку стоимость до них тоже не ограничена.
		// недостижимые циклы пропускаем, чтобы они не меняли ответы в компоненте начальной вершины
		var dists = Enumerable.Repeat(long.MaxValue, vertexCount + 1).ToArray();
		dists[start] = 0;

		for (var pass = 1; pass < vertexCount; pass++)
		{
			var changed = false;

			foreach (var edge in edges)
			{
				if (dists[edge[0]] != long.MaxValue && dists[edge[0]] + edge[2] < dists[edge[1]])
				{
					dists[edge[1]] = dists[edge[0]] + edge[2];
					changed = true;
				}
			}

			if (!changed)
			{
				break;
			}
		}

		var nextVertices = Enumerable.Range(0, vertexCount + 1).Select(_ => new List<int>()).ToArray();
		var unbounded = new bool[vertexCount + 1];
		Queue<int> pendingVertices = new();

		foreach (var edge in edges)
		{
			nextVertices[edge[0]].Add(edge[1]);

			if (dists[edge[0]] != long.MaxValue && dists[edge[0]] + edge[2] < dists[edge[1]] && !unbounded[edge[1]])
			{
				unbounded[edge[1]] = true;
				pendingVertices.Enqueue(edge[1]);
			}
		}

		while (pendingVertices.Count > 0)
		{
			var vertex = pendingVertices.Dequeue();

			foreach (var next in nextVertices[vertex])
			{
				if (!unbounded[next])
				{
					unbounded[next] = true;
					pendingVertices.Enqueue(next);
				}
			}
		}

		return Enumerable.Range(1, vertexCount).Select(vertex => unbounded[vertex] ? "-"
			: dists[vertex] == long.MaxValue ? "*"
			: dists[vertex].ToString(CultureInfo.InvariantCulture)).ToArray();
	}
}

public sealed class Task05Tests
{
	public static TheoryData<int, int[][], int, string[]> Cases => new()
	{
		{ 4, [[1, 2, 1], [2, 2, -1], [2, 3, 0]], 1, ["0", "-", "-", "*"] },
		{ 4, [[1, 2, 5], [3, 4, -1], [4, 3, -1]], 1, ["0", "5", "*", "*"] },
		{ 3, [[1, 2, -4], [2, 3, 2]], 1, ["0", "-4", "-2"] },
		{ 1, [[1, 1, -1]], 1, ["-"] },
		{ 1, [], 1, ["0"] },
		{ 3, [[1, 2, 0], [2, 3, -1], [3, 1, 0]], 1, ["-", "-", "-"] },
	};

	[Theory]
	[MemberData(nameof(Cases))]
	public void ClassifiesReachableAndUnboundedCosts(int vertexCount, int[][] edges, int start, string[] expected)
	{
		var actual = Task05.PathCostStates(vertexCount, edges, start);

		Assert.Equal(expected, actual);
	}
}
