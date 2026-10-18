namespace CA02.Kt09;

public static class Task06
{
	/*
	Связная часть дерева заданного размера

	Сеть из n пунктов образует дерево. Пункт i имеет ценность v[i]. Выберите ровно k
	пунктов максимальной суммарной ценности так, чтобы они образовывали связную часть
	дерева: путь между любыми двумя выбранными пунктами должен состоять только из
	выбранных пунктов.

	Входные данные:
	В первой строке заданы n и k. Во второй строке заданы n значений v[i]. В следующих n
	- 1 строках заданы рёбра дерева u и v.

	Выходные данные:
	Выведите максимальную суммарную ценность ровно k выбранных пунктов.

	Ограничения:
	1 <= k <= n <= 300, -10^9 <= v[i] <= 10^9, 1 <= u, v <= n. Гарантируется, что
	заданные рёбра образуют связное дерево. Ответ помещается в 64-битный знаковый целый
	тип.
	*/
	public static long MaximumConnectedValue(int[] values, int[][] edges, int requiredCount)
	{
		// состояние поддерева обязательно включает его корень, поэтому объединение с ребёнком сохраняет связность.
		// ребёнка можно пропустить целиком или взять связный набор, включающий самого ребёнка.
		// проверяем ответ в каждой вершине, поскольку лучший набор не обязан содержать корень всего дерева
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

		var bestVals = new long[values.Length + 1][];
		var sizes = new int[values.Length + 1];
		var largestValue = long.MinValue;

		for (var index = order.Count - 1; index >= 0; index--)
		{
			var vertex = order[index];
			bestVals[vertex] = [long.MinValue, values[vertex - 1]];
			sizes[vertex] = 1;

			foreach (var child in neighbours[vertex])
			{
				if (child == parents[vertex])
				{
					continue;
				}

				var combinedCount = Math.Min(requiredCount, sizes[vertex] + sizes[child]);
				var combined = Enumerable.Repeat(long.MinValue, combinedCount + 1).ToArray();

				for (var count = 1; count < bestVals[vertex].Length; count++)
				{
					if (bestVals[vertex][count] == long.MinValue)
					{
						continue;
					}

					combined[count] = Math.Max(combined[count], bestVals[vertex][count]);

					for (var childCount = 1; childCount < bestVals[child].Length && count + childCount <= requiredCount; childCount++)
					{
						if (bestVals[child][childCount] != long.MinValue)
						{
							combined[count + childCount] = Math.Max(combined[count + childCount],
								bestVals[vertex][count] + bestVals[child][childCount]);
						}
					}
				}

				bestVals[vertex] = combined;
				sizes[vertex] += sizes[child];
			}

			if (bestVals[vertex].Length > requiredCount)
			{
				largestValue = Math.Max(largestValue, bestVals[vertex][requiredCount]);
			}
		}

		return largestValue;
	}
}

public sealed class Task06Tests
{
	public static TheoryData<int[], int[][], int, long> Cases => new()
	{
		{ [-5], [], 1, -5 },
		{ [1, 5, 7], [[1, 2], [2, 3]], 2, 12 },
		{ [-100, 5, 7], [[1, 2], [1, 3]], 2, -93 },
		{ [-5, -2, -3], [[1, 2], [2, 3]], 2, -5 },
		{ [2, 1, 3], [[1, 2], [2, 3]], 3, 6 },
		{ [1, 9, 3], [[1, 2], [1, 3]], 1, 9 },
		{ [1_000_000_000, 1_000_000_000, 1_000_000_000], [[1, 2], [2, 3]], 3, 3_000_000_000 },
	};

	[Theory]
	[MemberData(nameof(Cases))]
	public void FindsBestConnectedSetOfExactlyTheRequiredSize(int[] values, int[][] edges, int requiredCount, long expected)
	{
		var actual = Task06.MaximumConnectedValue(values, edges, requiredCount);

		Assert.Equal(expected, actual);
	}
}
