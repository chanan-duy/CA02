namespace CA02.Kt02;

public static class Task05
{
	/*
	Количество подмассивов с заданной суммой

	Дан массив целых чисел nums и целое число k. Найдите количество непрерывных
	подмассивов, сумма элементов которых равна k.
	В массиве могут быть отрицательные числа, нули и повторяющиеся значения.

	Входные данные:
	В первой строке заданы n и k. Во второй строке заданы n целых чисел nums.

	Выходные данные:
	Одно целое число - количество подходящих подмассивов.

	Ограничения:
	1 <= n <= 200 000
	-10^9 <= nums[i] <= 10^9
	-10^14 <= k <= 10^14

	Примеры:
	nums = [1, 1, 1], k = 2 -> 2
	Подходящие подмассивы: [1, 1] на позициях 0-1 и [1, 1] на позициях 1-2.

	nums = [1, -1, 0], k = 0 -> 3
	Подходящие подмассивы: [1, -1], [0], [1, -1, 0].
	*/
	public static long CountSubarraysWithSum(int[] nums, long target)
	{
		// сумма nums[left..right] равна prefix[right] - prefix[left - 1].
		// поэтому для текущего prefixSum ищем, сколько раз раньше встречалось prefixSum - target.
		// для этого выбираем dictionary; sliding window не подходит, потому что отрицательные числа меняют сумму в обе стороны
		Dictionary<long, long> prefixSumCounts = new()
		{
			[0] = 1,
		};

		long prefixSum = 0;
		long matchingCount = 0;

		foreach (var number in nums)
		{
			prefixSum += number;
			matchingCount += prefixSumCounts.GetValueOrDefault(prefixSum - target);
			prefixSumCounts[prefixSum] = prefixSumCounts.GetValueOrDefault(prefixSum) + 1;
		}

		return matchingCount;
	}
}

public sealed class Task05Tests
{
	public static TheoryData<int[], long, long> Cases => new()
	{
		{ [1, 1, 1], 2, 2 },
		{ [1, -1, 0], 0, 3 },
		{ [0, 0, 0], 0, 6 },
		{ [3], 3, 1 },
		{ [1, 2, 3], 7, 0 },
		{ [1_000_000_000, 1_000_000_000, 1_000_000_000], 3_000_000_000, 1 },
	};

	[Theory]
	[MemberData(nameof(Cases))]
	public void CountsSubarraysWithRequestedSum(int[] nums, long target, long expected)
	{
		var actual = Task05.CountSubarraysWithSum(nums, target);

		Assert.Equal(expected, actual);
	}

	[Fact]
	public void SupportsResultsLargerThanInt()
	{
		var nums = new int[100_000];

		var actual = Task05.CountSubarraysWithSum(nums, 0);

		Assert.Equal(5_000_050_000, actual);
	}
}
