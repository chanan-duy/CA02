namespace CA02.Kt04;

public static class Task06
{
	/*
	Медиана скользящего окна

	Дан массив из n чисел и длина окна k. Рассмотрите все непрерывные фрагменты
	массива длины k слева направо. Для каждого фрагмента найдите медиану:
	при нечётной длине это центральный элемент после сортировки, при чётной -
	среднее арифметическое двух центральных элементов.

	Входные данные:
	В первой строке заданы n и k. Во второй строке заданы n целых чисел nums.

	Выходные данные:
	n - k + 1 медиан через пробел. Каждое значение выводится ровно с одним
	знаком после десятичной точки.

	Ограничения:
	1 <= k <= n <= 200 000
	-10^9 <= nums[i] <= 10^9
	*/
	public static double[] SlidingWindowMedians(int[] nums, int windowLn)
	{
		// sortedset позволяет получать границы двух половин и удалять число, вышедшее из окна, без перебора.
		// вместе со значением храним индекс, чтобы одинаковые числа оставались отдельными элементами.
		// нижняя половина содержит столько же элементов, сколько верхняя, или на один больше, поэтому медиана лежит на их границе
		SortedSet<(int Number, int Index)> lowerNums = [];
		SortedSet<(int Number, int Index)> upperNums = [];
		var medians = new double[nums.Length - windowLn + 1];

		for (var right = 0; right < nums.Length; right++)
		{
			var addedNumber = (Number: nums[right], Index: right);

			if (lowerNums.Count == 0 || addedNumber.CompareTo(lowerNums.Max) <= 0)
			{
				lowerNums.Add(addedNumber);
			}
			else
			{
				upperNums.Add(addedNumber);
			}

			if (right >= windowLn)
			{
				var left = right - windowLn;
				var removedNumber = (nums[left], left);

				if (!lowerNums.Remove(removedNumber))
				{
					upperNums.Remove(removedNumber);
				}
			}

			while (lowerNums.Count > upperNums.Count + 1)
			{
				var movedNumber = lowerNums.Max;
				lowerNums.Remove(movedNumber);
				upperNums.Add(movedNumber);
			}

			while (lowerNums.Count < upperNums.Count)
			{
				var movedNumber = upperNums.Min;
				upperNums.Remove(movedNumber);
				lowerNums.Add(movedNumber);
			}

			if (right >= windowLn - 1)
			{
				medians[right - windowLn + 1] = lowerNums.Count > upperNums.Count
					? lowerNums.Max.Number
					: ((long)lowerNums.Max.Number + upperNums.Min.Number) / 2.0;
			}
		}

		return medians;
	}
}

public sealed class Task06Tests
{
	public static TheoryData<int[], int, double[]> Cases => new()
	{
		{ [1, 3, -1, -3, 5, 3, 6, 7], 3, [1.0, -1.0, -1.0, 3.0, 5.0, 6.0] },
		{ [1, 2, 3, 4], 2, [1.5, 2.5, 3.5] },
		{ [4, 3, 2, 1], 2, [3.5, 2.5, 1.5] },
		{ [-5], 1, [-5.0] },
		{ [3, 1, 2], 1, [3.0, 1.0, 2.0] },
		{ [3, 1, 2], 3, [2.0] },
		{ [2, 2, 2, 2], 2, [2.0, 2.0, 2.0] },
		{ [1, 2, 2, 2, 1], 3, [2.0, 2.0, 2.0] },
		{ [-1, -2, -3, -4], 2, [-1.5, -2.5, -3.5] },
		{ [1_000_000_000, -1_000_000_000, 1_000_000_000], 2, [0.0, 0.0] },
		{ [1_000_000_000, 1_000_000_000], 2, [1_000_000_000.0] },
	};

	[Theory]
	[MemberData(nameof(Cases))]
	public void FindsMedianForEachWindow(int[] nums, int windowLn, double[] expected)
	{
		var actual = Task06.SlidingWindowMedians(nums, windowLn);

		Assert.Equal(expected, actual);
	}

	[Fact]
	public void SupportsMaximumInputSize()
	{
		var nums = Enumerable.Range(0, 200_000).ToArray();
		var expected = Enumerable.Range(0, 100_001).Select(index => index + 49_999.5).ToArray();

		var actual = Task06.SlidingWindowMedians(nums, 100_000);

		Assert.Equal(expected, actual);
	}
}
