namespace CA02.Kt09;

public static class Task03
{
	/*
	Набор несоседних станций максимальной ценности

	Сеть состоит из n станций и n - 1 двусторонних соединений; между любой парой станций
	существует единственный путь. Станция i имеет ценность v[i]. Выберите набор станций
	максимальной суммарной ценности так, чтобы никакие две выбранные станции не были
	соединены непосредственно. Разрешается не выбирать ни одной станции.

	Входные данные:
	В первой строке задано n. Во второй строке заданы n значений v[i]. В следующих n - 1
	строках заданы соединения u и v.

	Выходные данные:
	Выведите максимальную суммарную ценность.

	Ограничения:
	1 <= n <= 200 000, -10^9 <= v[i] <= 10^9, 1 <= u, v <= n. Граф гарантированно
	является деревом. Ответ помещается в 64-битный знаковый целый тип.
	*/
	public static long MaximumIndependentValue(int[] values, int[][] edges)
	{
		// для выбранной вершины всех детей пропускаем, а для пропущенной выбираем лучшее состояние каждого ребёнка.
		// обратный порядок обхода вычисляет состояния детей раньше родителя без глубокой рекурсии.
		// пропуск всех вершин имеет ценность 0, поэтому отрицательные значения не вынуждают брать станцию
		var neighbours = Enumerable.Range(0, values.Length + 1).Select(_ => new List<int>()).ToArray();

		foreach (var edge in edges)
		{
			neighbours[edge[0]].Add(edge[1]);
			neighbours[edge[1]].Add(edge[0]);
		}

		var parents = new int[values.Length + 1];
		var order = new List<int> { 1 };

		for (var index = 0; index < order.Count; index++)
		{
			var vertex = order[index];

			foreach (var next in neighbours[vertex])
			{
				if (next != parents[vertex])
				{
					parents[next] = vertex;
					order.Add(next);
				}
			}
		}

		var selectedVals = new long[values.Length + 1];
		var skippedVals = new long[values.Length + 1];

		for (var index = order.Count - 1; index >= 0; index--)
		{
			var vertex = order[index];
			selectedVals[vertex] += values[vertex - 1];
			var parent = parents[vertex];
			selectedVals[parent] += skippedVals[vertex];
			skippedVals[parent] += Math.Max(selectedVals[vertex], skippedVals[vertex]);
		}

		return Math.Max(selectedVals[1], skippedVals[1]);
	}
}

public sealed class Task03Tests
{
	public static TheoryData<int[], int[][], long> Cases => new()
	{
		{ [5], [], 5 },
		{ [-5], [], 0 },
		{ [5, 1, 5], [[1, 2], [2, 3]], 10 },
		{ [10, 6, 6, 6], [[1, 2], [1, 3], [1, 4]], 18 },
		{ [-1, -2, -3], [[1, 2], [1, 3]], 0 },
		{
			[1_000_000_000, 0, 1_000_000_000, 1_000_000_000], [[1, 2], [2, 3], [2, 4]],
			3_000_000_000
		},
	};

	[Theory]
	[MemberData(nameof(Cases))]
	public void FindsBestSetWithoutAdjacentStations(int[] values, int[][] edges, long expected)
	{
		var actual = Task03.MaximumIndependentValue(values, edges);

		Assert.Equal(expected, actual);
	}
}
