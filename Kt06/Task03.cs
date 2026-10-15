namespace CA02.Kt06;

public static class Task03
{
	/*
	Лексикографически наименьший порядок этапов

	Для сборки устройства требуется выполнить n этапов. Дано m требований, каждое из
	которых указывает, что этап a должен завершиться раньше этапа b. Найдите допустимый
	порядок всех этапов. Если порядков несколько, выведите лексикографически наименьший.

	Входные данные:
	В первой строке заданы n и m. В следующих m строках заданы пары a и b. Одинаковое
	требование может встречаться несколько раз.

	Выходные данные:
	Если выполнить все требования невозможно, выведите -1. Иначе выведите n номеров
	этапов в найденном порядке.

	Ограничения:
	1 <= n <= 200 000, 0 <= m <= 400 000, 1 <= a, b <= n.
	*/
	public static int[] SmallestStageOrder(int stageCount, int[][] requirements)
	{
		// этап доступен после выполнения всех входящих требований, поэтому используем топологическую сортировку.
		// priority queue выбирает наименьший доступный номер и тем самым минимизирует порядок лексикографически.
		// если доступные этапы закончились раньше времени, оставшиеся требования содержат цикл
		var nextStages = Enumerable.Range(0, stageCount + 1).Select(_ => new List<int>()).ToArray();
		var inDegrees = new int[stageCount + 1];

		foreach (var requirement in requirements)
		{
			nextStages[requirement[0]].Add(requirement[1]);
			inDegrees[requirement[1]]++;
		}

		PriorityQueue<int, int> availableStages = new();

		for (var stage = 1; stage <= stageCount; stage++)
		{
			if (inDegrees[stage] == 0)
			{
				availableStages.Enqueue(stage, stage);
			}
		}

		List<int> order = [];

		while (availableStages.Count > 0)
		{
			var stage = availableStages.Dequeue();
			order.Add(stage);

			foreach (var next in nextStages[stage])
			{
				inDegrees[next]--;

				if (inDegrees[next] == 0)
				{
					availableStages.Enqueue(next, next);
				}
			}
		}

		return order.Count == stageCount ? order.ToArray() : [-1];
	}
}

public sealed class Task03Tests
{
	public static TheoryData<int, int[][], int[]> Cases => new()
	{
		{ 3, [], [1, 2, 3] },
		{ 4, [[2, 1], [3, 1]], [2, 3, 1, 4] },
		{ 3, [[1, 2], [1, 2], [2, 3]], [1, 2, 3] },
		{ 2, [[1, 2], [2, 1]], [-1] },
		{ 1, [[1, 1]], [-1] },
		{ 1, [], [1] },
	};

	[Theory]
	[MemberData(nameof(Cases))]
	public void FindsSmallestValidStageOrder(int stageCount, int[][] requirements, int[] expected)
	{
		var actual = Task03.SmallestStageOrder(stageCount, requirements);

		Assert.Equal(expected, actual);
	}
}
