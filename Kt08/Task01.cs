namespace CA02.Kt08;

public static class Task01
{
	/*
	Минимальная стоимость переходов по платформам

	На маршруте расположены n платформ с высотами h[i]. Робот начинает на платформе 1 и
	за один ход может перейти с платформы i на платформу i + 1 или i + 2. Стоимость
	перехода с i на j равна модулю разности h[i] и h[j]. Определите минимальную
	суммарную стоимость пути до платформы n.

	Входные данные:
	В первой строке задано n. Во второй строке заданы n целых чисел h[i].

	Выходные данные:
	Выведите минимальную суммарную стоимость пути.

	Ограничения:
	2 <= n <= 200 000, -10^9 <= h[i] <= 10^9. Ответ помещается в 64-битный знаковый
	целый тип.
	*/
	public static long MinimumJumpCost(int[] heights)
	{
		// до очередной платформы можно попасть только с двух предыдущих, поэтому остальные состояния не нужны.
		// сравниваем полные стоимости обоих переходов, а не только разность высот последнего прыжка.
		// приводим высоты к long до вычитания, чтобы разность крайних значений не переполнила int
		long prevCost = 0;
		var currentCost = Math.Abs((long)heights[1] - heights[0]);

		for (var index = 2; index < heights.Length; index++)
		{
			var nextCost = Math.Min(currentCost + Math.Abs((long)heights[index] - heights[index - 1]),
				prevCost + Math.Abs((long)heights[index] - heights[index - 2]));
			prevCost = currentCost;
			currentCost = nextCost;
		}

		return currentCost;
	}
}

public sealed class Task01Tests
{
	public static TheoryData<int[], long> Cases => new()
	{
		{ [10, 30, 40, 20], 30 },
		{ [10, 10], 0 },
		{ [1, 100, 2], 1 },
		{ [0, 1, 2, 3, 4], 4 },
		{ [-1_000_000_000, 1_000_000_000], 2_000_000_000 },
		{
			[
				-1_000_000_000, -1_000_000_000, 1_000_000_000, 1_000_000_000,
				-1_000_000_000, -1_000_000_000,
			],
			4_000_000_000
		},
	};

	[Theory]
	[MemberData(nameof(Cases))]
	public void FindsMinimumJumpCost(int[] heights, long expected)
	{
		var actual = Task01.MinimumJumpCost(heights);

		Assert.Equal(expected, actual);
	}
}
