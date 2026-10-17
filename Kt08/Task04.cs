namespace CA02.Kt08;

public static class Task04
{
	/*
	Максимальная прибыль при точной массе

	Имеются n типов грузов. Один груз типа i имеет массу w[i] и приносит прибыль p[i].
	Грузов каждого типа доступно неограниченное количество. Требуется набрать грузы
	общей массой ровно C. Сначала максимизируйте суммарную прибыль, а среди вариантов с
	максимальной прибылью минимизируйте количество грузов.

	Входные данные:
	В первой строке заданы n и C. В следующих n строках заданы w[i] и p[i].

	Выходные данные:
	Если набрать массу ровно C невозможно, выведите -1. Иначе выведите максимальную
	прибыль и минимальное количество грузов, необходимое для получения этой прибыли.

	Ограничения:
	1 <= n <= 200, 1 <= C <= 200 000, 1 <= w[i] <= C, 1 <= p[i] <= 10^9, n * C <= 30 000
	000. Прибыль помещается в 64-битный знаковый целый тип.
	*/
	public static (long Profit, int Count)? BestExactCargo(int[][] cargoTypes, int capacity)
	{
		// состояние задаёт точную массу, поэтому недостижимые массы нельзя заполнять нулевой прибылью.
		// меньшие массы уже обработаны, что позволяет повторно брать любой тип груза.
		// при равной прибыли сохраняем меньшее число грузов, чтобы выполнить второе условие выбора
		var profits = Enumerable.Repeat(long.MinValue, capacity + 1).ToArray();
		var counts = new int[capacity + 1];
		profits[0] = 0;

		for (var weight = 1; weight <= capacity; weight++)
		{
			foreach (var cargo in cargoTypes)
			{
				if (weight < cargo[0] || profits[weight - cargo[0]] == long.MinValue)
				{
					continue;
				}

				var profit = profits[weight - cargo[0]] + cargo[1];
				var count = counts[weight - cargo[0]] + 1;

				if (profit > profits[weight] || (profit == profits[weight] && count < counts[weight]))
				{
					profits[weight] = profit;
					counts[weight] = count;
				}
			}
		}

		return profits[capacity] == long.MinValue ? null : (profits[capacity], counts[capacity]);
	}
}

public sealed class Task04Tests
{
	public static TheoryData<int[][], int, long?, int> Cases => new()
	{
		{ [[2, 3], [3, 5]], 6, 10, 2 },
		{ [[1, 2], [2, 4]], 4, 8, 2 },
		{ [[2, 3], [4, 8]], 3, null, 0 },
		{ [[3, 5]], 9, 15, 3 },
		{ [[1, 1_000_000_000]], 4, 4_000_000_000, 4 },
	};

	[Theory]
	[MemberData(nameof(Cases))]
	public void MaximizesProfitThenMinimizesCargoCount(int[][] cargoTypes, int capacity, long? expectedProfit, int expectedCount)
	{
		var actual = Task04.BestExactCargo(cargoTypes, capacity);

		Assert.Equal(expectedProfit, actual?.Profit);
		Assert.Equal(expectedCount, actual?.Count ?? 0);
	}
}
