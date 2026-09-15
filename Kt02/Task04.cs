namespace CA02.Kt02;

public static class Task04
{
	/*
	Минимальная вместимость корабля

	Дан массив weights, где weights[i] - вес i-го груза. Все грузы необходимо
	перевезти строго в исходном порядке за не более чем days дней.
	За один день можно загрузить несколько подряд идущих грузов, но их суммарный
	вес не должен превышать вместимость корабля.
	Определите минимальную возможную вместимость корабля.

	Входные данные:
	В первой строке заданы n и days. Во второй строке заданы n целых чисел weights.

	Выходные данные:
	Одно целое число - минимальная вместимость корабля.

	Ограничения:
	1 <= n <= 200 000
	1 <= days <= n
	1 <= weights[i] <= 10^9
	*/
	public static long MinimumShipCapacity(int[] weights, int days)
	{
		// вместимость не меньше самого тяжёлого груза, а сумма всех весов позволяет уложиться в один день.
		// если вместимости хватает, любая большая тоже подходит, поэтому выбираем binary search по ответу.
		// для проверки заполняем каждый день до предела: оставлять груз на завтра при свободном месте нет смысла
		long left = 0;
		long right = 0;

		foreach (var weight in weights)
		{
			left = Math.Max(left, weight);
			right += weight;
		}

		while (left < right)
		{
			var capacity = left + (right - left) / 2;
			var requiredDays = 1;
			long currentWeight = 0;

			foreach (var weight in weights)
			{
				if (currentWeight + weight > capacity)
				{
					requiredDays++;
					currentWeight = 0;
				}

				currentWeight += weight;
			}

			if (requiredDays > days)
			{
				left = capacity + 1;
			}
			else
			{
				right = capacity;
			}
		}

		return left;
	}
}

public sealed class Task04Tests
{
	public static TheoryData<int[], int, long> Cases => new()
	{
		{ [1, 2, 3, 4, 5, 6, 7, 8, 9, 10], 5, 15 },
		{ [3, 2, 2, 4, 1, 4], 3, 6 },
		{ [1, 2, 3, 1, 1], 4, 3 },
		{ [5], 1, 5 },
		{ [3, 1, 2], 1, 6 },
		{ [3, 1, 2], 3, 3 },
		{ [5, 5, 1, 1], 2, 7 },
		{ [1_000_000_000, 1_000_000_000, 1_000_000_000], 1, 3_000_000_000 },
	};

	[Theory]
	[MemberData(nameof(Cases))]
	public void FindsMinimumShipCapacity(int[] weights, int days, long expected)
	{
		var actual = Task04.MinimumShipCapacity(weights, days);

		Assert.Equal(expected, actual);
	}

	[Fact]
	public void SupportsMaximumTotalWeight()
	{
		var weights = Enumerable.Repeat(1_000_000_000, 200_000).ToArray();

		var actual = Task04.MinimumShipCapacity(weights, 1);

		Assert.Equal(200_000_000_000_000, actual);
	}
}
