namespace CA02.Kt04;

public static class Task04
{
	/*
	Максимальная сумма маршрута в дереве

	Дано бинарное дерево со значениями в вершинах. Маршрут является непустой
	последовательностью различных вершин, в которой каждые две соседние вершины
	соединены ребром. Начальная и конечная вершины могут быть любыми, а маршрут
	не обязан проходить через корень. Найдите максимальную сумму значений
	вошедших в маршрут вершин.

	Входные данные:
	В первой строке задано n. В следующих n строках описаны вершины с индексами
	от 0 до n - 1. Каждая строка содержит value, left и right. Значения left
	и right равны индексам потомков или -1, если потомка нет.
	Корнем является вершина 0. Гарантируется, что задано корректное бинарное дерево.

	Выходные данные:
	Максимально возможная сумма значений вершин маршрута.

	Ограничения:
	1 <= n <= 200 000
	-10^9 <= value <= 10^9
	Ответ может не помещаться в 32-битный целый тип.
	*/
	public static long MaximumPathSum(int[][] nodes)
	{
		// для маршрута через вершину можно взять обе ветви, но к её родителю продолжается только одна.
		// отрицательные ветви не берём, а long.MinValue позволяет найти ответ и в дереве из отрицательных чисел.
		// stack заменяет рекурсию: после обработки потомков возвращаемся к вершине без риска переполнить стек вызовов
		Stack<(int Index, bool Visited)> pendingNodes = new();
		pendingNodes.Push((0, false));
		var branchSums = new long[nodes.Length];
		var largestSum = long.MinValue;

		while (pendingNodes.Count > 0)
		{
			var (index, visited) = pendingNodes.Pop();
			var node = nodes[index];

			if (!visited)
			{
				pendingNodes.Push((index, true));

				if (node[1] != -1)
				{
					pendingNodes.Push((node[1], false));
				}

				if (node[2] != -1)
				{
					pendingNodes.Push((node[2], false));
				}

				continue;
			}

			var leftSum = node[1] == -1 ? 0 : Math.Max(0, branchSums[node[1]]);
			var rightSum = node[2] == -1 ? 0 : Math.Max(0, branchSums[node[2]]);
			largestSum = Math.Max(largestSum, node[0] + leftSum + rightSum);
			branchSums[index] = node[0] + Math.Max(leftSum, rightSum);
		}

		return largestSum;
	}
}

public sealed class Task04Tests
{
	public static TheoryData<int[][], long> Cases => new()
	{
		{ [[5, -1, -1]], 5 },
		{ [[-5, -1, -1]], -5 },
		{ [[1, 1, 2], [2, -1, -1], [3, -1, -1]], 6 },
		{ [[-10, 1, 2], [9, -1, -1], [20, 3, 4], [15, -1, -1], [7, -1, -1]], 42 },
		{ [[-3, 1, 2], [-2, -1, -1], [-5, -1, -1]], -2 },
		{ [[2, 1, -1], [-1, 2, -1], [3, -1, -1]], 4 },
		{ [[0, 1, 2], [0, -1, -1], [0, -1, -1]], 0 },
		{ [[1, -1, 2], [3, -1, -1], [2, 1, -1]], 6 },
		{
			[[1_000_000_000, 1, 2], [1_000_000_000, -1, -1], [1_000_000_000, -1, -1]],
			3_000_000_000
		},
	};

	[Theory]
	[MemberData(nameof(Cases))]
	public void FindsMaximumTreePathSum(int[][] nodes, long expected)
	{
		var actual = Task04.MaximumPathSum(nodes);

		Assert.Equal(expected, actual);
	}

	[Fact]
	public void SupportsMaximumTreeDepthAndSum()
	{
		var nodes = Enumerable.Range(0, 200_000)
			.Select(index => new[] { 1_000_000_000, -1, index < 199_999 ? index + 1 : -1 }).ToArray();

		var actual = Task04.MaximumPathSum(nodes);

		Assert.Equal(200_000_000_000_000, actual);
	}
}
