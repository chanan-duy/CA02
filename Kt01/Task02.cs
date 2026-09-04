namespace CA02.Kt01;

public static class Task02
{
	/*
	Тройки с нулевой суммой

	Дан массив целых чисел nums. Найдите все уникальные тройки элементов
	[a, b, c], для которых a + b + c = 0.
	Один и тот же индекс нельзя использовать несколько раз. В ответе не должно
	быть одинаковых троек. Порядок чисел внутри тройки и порядок троек в ответе
	не важны.

	Входные данные:
	nums - массив целых чисел.

	Выходные данные:
	Массив уникальных троек, сумма элементов которых равна нулю.

	Ограничения:
	3 <= len(nums) <= 3 000
	-100 000 <= nums[i] <= 100 000

	Примеры:
	[-1, 0, 1, 2, -1, -4] -> [[-1, -1, 2], [-1, 0, 1]]
	[0, 0, 0, 0] -> [[0, 0, 0]]
	[1, 2, -2, -1] -> []
	*/
	public static List<List<int>> ThreeSum(int[] numbers)
	{
		// фиксируем первое число, после чего нужно найти пару с суммой, равной его отрицанию.
		// сортируем массив: сдвиг left увеличивает сумму, а right уменьшает, поэтому пару ищем через two pointers.
		// одинаковые соседние значения пропускаем, чтобы не добавить одну тройку несколько раз
		var sortedNums = numbers.ToArray();
		Array.Sort(sortedNums);

		List<List<int>> triples = [];

		for (var index = 0; index < sortedNums.Length - 2; index++)
		{
			if (index > 0 && sortedNums[index] == sortedNums[index - 1])
			{
				continue;
			}

			if (sortedNums[index] > 0)
			{
				break;
			}

			var left = index + 1;
			var right = sortedNums.Length - 1;

			while (left < right)
			{
				var sum = sortedNums[index] + sortedNums[left] + sortedNums[right];

				if (sum < 0)
				{
					left++;
					continue;
				}

				if (sum > 0)
				{
					right--;
					continue;
				}

				triples.Add([sortedNums[index], sortedNums[left], sortedNums[right]]);
				left++;
				right--;

				while (left < right && sortedNums[left] == sortedNums[left - 1])
				{
					left++;
				}

				while (left < right && sortedNums[right] == sortedNums[right + 1])
				{
					right--;
				}
			}
		}

		return triples;
	}
}

public sealed class Task02Tests
{
	public static TheoryData<int[], int[][]> Cases => new()
	{
		{ [-1, 0, 1, 2, -1, -4], [[-1, -1, 2], [-1, 0, 1]] },
		{ [0, 0, 0, 0], [[0, 0, 0]] },
		{ [1, 2, -2, -1], [] },
		{ [-2, 0, 1, 1, 2], [[-2, 0, 2], [-2, 1, 1]] },
		{ [0, 0, 0], [[0, 0, 0]] },
	};

	[Theory]
	[MemberData(nameof(Cases))]
	public void FindsUniqueZeroSumTriples(int[] numbers, int[][] expected)
	{
		var actual = Task02.ThreeSum(numbers);

		Assert.Equal(Normalize(expected), Normalize(actual));
	}

	[Fact]
	public void DoesNotChangeInputOrder()
	{
		int[] numbers = [-1, 0, 1, 2, -1, -4];
		var original = numbers.ToArray();

		Task02.ThreeSum(numbers);

		Assert.Equal(original, numbers);
	}

	private static string[] Normalize(IEnumerable<IEnumerable<int>> triples)
	{
		return triples
			.Select(triple => string.Join(',', triple.Order()))
			.Order(StringComparer.Ordinal)
			.ToArray();
	}
}
