namespace CA02.Kt01;

public static class Task01
{
	/*
	Самая длинная последовательность номеров

	Дан массив целых чисел nums. Найдите длину самой длинной последовательности
	чисел, идущих подряд по значению.

	Элементы последовательности могут находиться в массиве в любом порядке.
	Повторяющиеся числа не увеличивают длину последовательности.
	Например, из массива [100, 4, 200, 1, 3, 2] можно составить
	последовательность 1, 2, 3, 4. Её длина равна 4.

	Входные данные:
	nums - массив целых чисел.

	Выходные данные:
	Одно целое число - длина самой длинной последовательности.

	Ограничения:
	0 <= len(nums) <= 200 000
	-10^9 <= nums[i] <= 10^9

	Примеры:
	[100, 4, 200, 1, 3, 2] -> 4
	[0, 3, 7, 2, 5, 8, 4, 6, 0, 1] -> 9
	[] -> 0
	*/
	public static int LongestConsecutive(int[] numbers)
	{
		// в искомой последовательности соседние значения отличаются на 1, но в массиве они лежат в любом порядке.
		// поэтому можно выбрать hashset: в нём можно искать num - 1 и num + 1 без повторного прохода по массиву.
		// число без num - 1 является началом, от него считаем num + 1, num + 2 и дальше, пока они есть
		var uniqNums = numbers.ToHashSet();
		var longestLn = 0;

		foreach (var num in uniqNums)
		{
			if (uniqNums.Contains(num - 1))
			{
				continue;
			}

			var current = num;
			var currentLn = 1;

			while (uniqNums.Contains(current + 1))
			{
				current++;
				currentLn++;
			}

			longestLn = Math.Max(longestLn, currentLn);
		}

		return longestLn;
	}
}

public sealed class Task01Tests
{
	public static TheoryData<int[], int> Cases => new()
	{
		{ [100, 4, 200, 1, 3, 2], 4 },
		{ [0, 3, 7, 2, 5, 8, 4, 6, 0, 1], 9 },
		{ [], 0 },
		{ [5], 1 },
		{ [1, 2, 0, 1], 3 },
		{ [-3, -2, -1, 5, 6], 3 },
		{ [7, 7, 7], 1 },
	};

	[Theory]
	[MemberData(nameof(Cases))]
	public void FindsLongestConsecutiveSequence(int[] numbers, int expected)
	{
		var actual = Task01.LongestConsecutive(numbers);

		Assert.Equal(expected, actual);
	}
}
