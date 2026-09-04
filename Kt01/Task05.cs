namespace CA02.Kt01;

public static class Task05
{
	/*
	Максимальный объём воды между стенками

	Дан массив неотрицательных целых чисел height. Каждое число обозначает высоту
	вертикальной стенки на соответствующей позиции.
	Выберите две стенки так, чтобы вместе с осью координат они образовали контейнер
	с максимально возможной площадью. Верните объём воды, который поместится в
	таком контейнере.

	Объём определяется формулой:
	расстояние между стенками * высота меньшей стенки.

	Наклонять стенки нельзя.

	Входные данные:
	height - массив неотрицательных целых чисел.

	Выходные данные:
	Одно целое число - максимальная площадь контейнера.

	Ограничения:
	2 <= len(height) <= 200 000
	0 <= height[i] <= 10^9

	Примеры:
	[1, 8, 6, 2, 5, 4, 8, 3, 7] -> 49
	Стенки высотой 8 и 7 находятся на расстоянии 7, поэтому 7 * 7 = 49.

	[1, 1] -> 1
	*/
	public static long MaxWaterArea(int[] heights)
	{
		// площадь между left и right равна ширине, умноженной на высоту меньшей стенки.
		// если сдвинуть большую стенку, высота останется ограничена меньшей, а ширина сократится.
		// поэтому выбираем two pointers и отбрасываем меньшую стенку, только её замена может улучшить результат
		var left = 0;
		var right = heights.Length - 1;
		long largestArea = 0;

		while (left < right)
		{
			var width = right - left;
			var limitingHeight = Math.Min(heights[left], heights[right]);
			var area = (long)width * limitingHeight;
			largestArea = Math.Max(largestArea, area);

			if (heights[left] <= heights[right])
			{
				left++;
			}
			else
			{
				right--;
			}
		}

		return largestArea;
	}
}

public sealed class Task05Tests
{
	public static TheoryData<int[], long> Cases => new()
	{
		{ [1, 8, 6, 2, 5, 4, 8, 3, 7], 49 },
		{ [1, 1], 1 },
		{ [0, 0], 0 },
		{ [4, 3, 2, 1, 4], 16 },
		{ [1, 2, 1], 2 },
		{ [1_000_000_000, 0, 0, 1_000_000_000], 3_000_000_000 },
	};

	[Theory]
	[MemberData(nameof(Cases))]
	public void FindsLargestContainerArea(int[] heights, long expected)
	{
		var actual = Task05.MaxWaterArea(heights);

		Assert.Equal(expected, actual);
	}
}
