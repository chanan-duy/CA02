namespace CA02.Kt04;

public static class Task02
{
	/*
	Самые частые числа

	Дана последовательность целых чисел. Найдите k различных чисел, которые
	встречаются в ней чаще остальных. Выведите числа по убыванию частоты.
	Если частоты равны, раньше выводится меньшее число.

	Входные данные:
	В первой строке заданы n и k. Во второй строке заданы n целых чисел nums.
	Гарантируется, что в последовательности не менее k различных чисел.

	Выходные данные:
	Ровно k чисел в требуемом порядке.

	Ограничения:
	1 <= k <= n <= 200 000
	-10^9 <= nums[i] <= 10^9
	*/
	public static int[] MostFrequentNumbers(int[] nums, int requiredCount)
	{
		// dictionary считает частоту каждого значения, чтобы повторы не попадали в ответ отдельно.
		// сортируем разные числа по убыванию частоты, а при равенстве по возрастанию значения.
		// после сортировки первые requiredCount чисел уже стоят в требуемом порядке
		Dictionary<int, int> numberCounts = [];

		foreach (var number in nums)
		{
			numberCounts[number] = numberCounts.GetValueOrDefault(number) + 1;
		}

		return numberCounts
			.OrderByDescending(pair => pair.Value)
			.ThenBy(pair => pair.Key)
			.Take(requiredCount)
			.Select(pair => pair.Key)
			.ToArray();
	}
}

public sealed class Task02Tests
{
	public static TheoryData<int[], int, int[]> Cases => new()
	{
		{ [1, 1, 1, 2, 2, 3], 2, [1, 2] },
		{ [3, 2, 3, 2, 1], 2, [2, 3] },
		{ [5], 1, [5] },
		{ [7, 7, 7], 1, [7] },
		{ [-1, -2, -1, -2, 0], 3, [-2, -1, 0] },
		{ [4, 3, 2, 1], 4, [1, 2, 3, 4] },
		{ [0, 0, -1, -1, 2, 2], 2, [-1, 0] },
		{ [1_000_000_000, -1_000_000_000, 1_000_000_000], 2, [1_000_000_000, -1_000_000_000] },
	};

	[Theory]
	[MemberData(nameof(Cases))]
	public void FindsMostFrequentNumbers(int[] nums, int requiredCount, int[] expected)
	{
		var actual = Task02.MostFrequentNumbers(nums, requiredCount);

		Assert.Equal(expected, actual);
	}

	[Fact]
	public void SupportsMaximumDistinctCount()
	{
		var nums = Enumerable.Range(0, 200_000).Reverse().ToArray();

		var actual = Task02.MostFrequentNumbers(nums, 200_000);

		Assert.Equal(Enumerable.Range(0, 200_000), actual);
	}
}
