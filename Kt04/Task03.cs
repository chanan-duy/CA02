namespace CA02.Kt04;

public static class Task03
{
	/*
	Медиана поступающих чисел

	Числа поступают одно за другим. После появления каждого числа определите
	медиану всех чисел, полученных к этому моменту. Для нечётного количества
	элементов медиана является центральным элементом отсортированной
	последовательности, для чётного - средним арифметическим двух центральных.

	Входные данные:
	В первой строке задано n. Во второй строке заданы n целых чисел nums
	в порядке поступления.

	Выходные данные:
	n медиан через пробел. Каждое значение выводится ровно с одним знаком
	после десятичной точки.

	Ограничения:
	1 <= n <= 200 000
	-10^9 <= nums[i] <= 10^9
	*/
	public static double[] RunningMedians(int[] nums)
	{
		// делим числа на две половины: priority queue отдаёт максимум нижней и минимум верхней.
		// отрицательный приоритет позволяет получать максимум из нижней очереди, которая обычно отдаёт минимум.
		// нижняя половина содержит столько же чисел, сколько верхняя, или на одно больше, поэтому её вершина задаёт медиану
		PriorityQueue<int, int> lowerNums = new();
		PriorityQueue<int, int> upperNums = new();
		var medians = new double[nums.Length];

		for (var index = 0; index < nums.Length; index++)
		{
			var number = nums[index];

			if (lowerNums.Count == 0 || number <= lowerNums.Peek())
			{
				lowerNums.Enqueue(number, -number);
			}
			else
			{
				upperNums.Enqueue(number, number);
			}

			if (lowerNums.Count > upperNums.Count + 1)
			{
				var movedNumber = lowerNums.Dequeue();
				upperNums.Enqueue(movedNumber, movedNumber);
			}
			else if (lowerNums.Count < upperNums.Count)
			{
				var movedNumber = upperNums.Dequeue();
				lowerNums.Enqueue(movedNumber, -movedNumber);
			}

			medians[index] = lowerNums.Count > upperNums.Count
				? lowerNums.Peek()
				: ((long)lowerNums.Peek() + upperNums.Peek()) / 2.0;
		}

		return medians;
	}
}

public sealed class Task03Tests
{
	public static TheoryData<int[], double[]> Cases => new()
	{
		{ [1, 2, 3, 4], [1.0, 1.5, 2.0, 2.5] },
		{ [5, 2, 8, 1], [5.0, 3.5, 5.0, 3.5] },
		{ [-5], [-5.0] },
		{ [3, 3, 3, 3], [3.0, 3.0, 3.0, 3.0] },
		{ [-1, -2, -3, -4], [-1.0, -1.5, -2.0, -2.5] },
		{ [0, -1, 1, 0], [0.0, -0.5, 0.0, 0.0] },
		{ [-1_000_000_000, 1_000_000_000], [-1_000_000_000.0, 0.0] },
		{ [1_000_000_000, 1_000_000_000], [1_000_000_000.0, 1_000_000_000.0] },
	};

	[Theory]
	[MemberData(nameof(Cases))]
	public void FindsMedianAfterEachNumber(int[] nums, double[] expected)
	{
		var actual = Task03.RunningMedians(nums);

		Assert.Equal(expected, actual);
	}

	[Fact]
	public void SupportsMaximumInputSize()
	{
		var nums = Enumerable.Range(0, 200_000).ToArray();
		var expected = nums.Select(number => number / 2.0).ToArray();

		var actual = Task03.RunningMedians(nums);

		Assert.Equal(expected, actual);
	}
}
