namespace CA02.Kt04;

public static class Task01
{
	/*
	Суммы значений по уровням дерева

	Дано непустое бинарное дерево. Для каждого уровня найдите сумму значений
	всех вершин на этом уровне. Уровень корня считается нулевым.

	Входные данные:
	В первой строке задано n. В следующих n строках описаны вершины с индексами
	от 0 до n - 1. Каждая строка содержит value, left и right: значение вершины
	и индексы её левого и правого потомков. Если потомка нет, указан индекс -1.
	Корнем является вершина 0. Гарантируется, что задано корректное бинарное дерево.

	Выходные данные:
	Суммы значений вершин на каждом уровне, начиная с уровня корня.
	Числа разделяются пробелами.

	Ограничения:
	1 <= n <= 200 000
	-10^9 <= value <= 10^9
	Ответы могут не помещаться в 32-битный целый тип.
	*/
	public static long[] LevelSums(int[][] nodes)
	{
		// выбираем bfs: очередь хранит вершины в порядке уровней, поэтому их суммы можно считать за один проход.
		// размер очереди перед проходом уровня отделяет его вершины от потомков, которые добавятся позже.
		// суммы храним в long, потому что на одном уровне может быть много больших значений
		Queue<int> pendingNodes = new();
		pendingNodes.Enqueue(0);
		List<long> sums = [];

		while (pendingNodes.Count > 0)
		{
			var levelCount = pendingNodes.Count;
			long levelSum = 0;

			for (var index = 0; index < levelCount; index++)
			{
				var node = nodes[pendingNodes.Dequeue()];
				levelSum += node[0];

				if (node[1] != -1)
				{
					pendingNodes.Enqueue(node[1]);
				}

				if (node[2] != -1)
				{
					pendingNodes.Enqueue(node[2]);
				}
			}

			sums.Add(levelSum);
		}

		return sums.ToArray();
	}
}

public sealed class Task01Tests
{
	public static TheoryData<int[][], long[]> Cases => new()
	{
		{ [[5, -1, -1]], [5] },
		{ [[1, 1, 2], [2, -1, -1], [3, -1, -1]], [1, 5] },
		{ [[-5, 1, 2], [-2, -1, -1], [-3, -1, -1]], [-5, -5] },
		{ [[0, 1, 2], [-4, -1, -1], [4, -1, -1]], [0, 0] },
		{ [[1, -1, 2], [3, -1, -1], [2, 1, -1]], [1, 2, 3] },
		{
			[
				[0, 1, 2], [0, 3, 4], [0, 5, 6], [1_000_000_000, -1, -1],
				[1_000_000_000, -1, -1], [1_000_000_000, -1, -1], [1_000_000_000, -1, -1],
			],
			[0, 0, 4_000_000_000]
		},
	};

	[Theory]
	[MemberData(nameof(Cases))]
	public void FindsSumsForEachTreeLevel(int[][] nodes, long[] expected)
	{
		var actual = Task01.LevelSums(nodes);

		Assert.Equal(expected, actual);
	}

	[Fact]
	public void SupportsMaximumTreeDepth()
	{
		var nodes = Enumerable.Range(0, 200_000)
			.Select(index => new[] { 1, index < 199_999 ? index + 1 : -1, -1 }).ToArray();

		var actual = Task01.LevelSums(nodes);

		Assert.Equal(Enumerable.Repeat(1L, 200_000), actual);
	}
}
