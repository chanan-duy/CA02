namespace CA02.Kt08;

public static class Task02
{
	/*
	Максимальная полезность предметов

	Для экспедиции доступно n предметов. У предмета i масса w[i] и полезность v[i].
	Каждый предмет разрешается взять не более одного раза, а суммарная масса выбранных
	предметов не должна превышать W. Найдите максимальную возможную суммарную
	полезность.

	Входные данные:
	В первой строке заданы n и W. В следующих n строках заданы w[i] и v[i].

	Выходные данные:
	Выведите максимальную суммарную полезность выбранных предметов.

	Ограничения:
	1 <= n <= 200, 1 <= W <= 100 000, 1 <= w[i] <= W, 1 <= v[i] <= 10^9, n * W <= 20 000
	000. Ответ помещается в 64-битный знаковый целый тип.
	*/
	public static long MaximumKnapsackValue(int[][] items, int capacity)
	{
		// bestVals[weight] хранит лучшую полезность при ограничении массы weight.
		// массы обходим по убыванию, чтобы текущий предмет не использовал собственное обновление второй раз.
		// нулевые начальные значения допускают пустой набор и не требуют заполнять рюкзак целиком
		var bestVals = new long[capacity + 1];

		foreach (var item in items)
		{
			for (var weight = capacity; weight >= item[0]; weight--)
			{
				bestVals[weight] = Math.Max(bestVals[weight], bestVals[weight - item[0]] + item[1]);
			}
		}

		return bestVals[capacity];
	}
}

public sealed class Task02Tests
{
	public static TheoryData<int[][], int, long> Cases => new()
	{
		{ [[3, 30], [4, 50], [5, 60]], 8, 90 },
		{ [[2, 5]], 4, 5 },
		{ [[1, 7], [1, 7], [1, 7]], 2, 14 },
		{ [[5, 4], [4, 10]], 5, 10 },
		{ [[1, 1_000_000_000], [1, 1_000_000_000], [1, 1_000_000_000]], 3, 3_000_000_000 },
	};

	[Theory]
	[MemberData(nameof(Cases))]
	public void FindsBestValueWithoutReusingItems(int[][] items, int capacity, long expected)
	{
		var actual = Task02.MaximumKnapsackValue(items, capacity);

		Assert.Equal(expected, actual);
	}
}
