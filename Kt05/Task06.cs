namespace CA02.Kt05;

public static class Task06
{
	/*
	Максимальный XOR непрерывного фрагмента

	Дан массив неотрицательных целых чисел. Для каждого непустого непрерывного фрагмента
	вычисляется результат побитовой операции XOR над всеми его элементами. Найдите
	максимальное значение, которое можно получить.

	Входные данные:
	В первой строке задано n. Во второй строке заданы n целых чисел.

	Выходные данные:
	Выведите максимальный результат среди всех непустых непрерывных фрагментов.

	Ограничения:
	1 <= n <= 200 000, 0 <= a[i] < 2^30.
	*/
	public static int MaximumSubarrayXor(int[] nums)
	{
		// xor фрагмента равен xor двух префиксов, поэтому для каждого нового префикса ищем лучший предыдущий.
		// в двоичном trie сначала выбираем противоположный бит: старший различающийся бит важнее всех младших.
		// нулевой префикс добавляем заранее, чтобы учитывать фрагменты с начала массива
		var children = new List<int[]> { new[] { -1, -1 } };

		void Add(int number)
		{
			var current = 0;

			for (var bit = 29; bit >= 0; bit--)
			{
				var digit = (number >> bit) & 1;

				if (children[current][digit] == -1)
				{
					children[current][digit] = children.Count;
					children.Add([-1, -1]);
				}

				current = children[current][digit];
			}
		}

		Add(0);
		var prefixXor = 0;
		var largestXor = 0;

		foreach (var number in nums)
		{
			prefixXor ^= number;
			var current = 0;
			var candidate = 0;

			for (var bit = 29; bit >= 0; bit--)
			{
				var digit = (prefixXor >> bit) & 1;

				if (children[current][digit ^ 1] != -1)
				{
					candidate |= 1 << bit;
					digit ^= 1;
				}

				current = children[current][digit];
			}

			largestXor = Math.Max(largestXor, candidate);
			Add(prefixXor);
		}

		return largestXor;
	}
}

public sealed class Task06Tests
{
	public static TheoryData<int[], int> Cases => new()
	{
		{ [1, 2, 3], 3 },
		{ [8, 1, 2, 12], 15 },
		{ [0], 0 },
		{ [7], 7 },
		{ [5, 5, 5], 5 },
		{ [1 << 29, (1 << 29) - 1], (1 << 30) - 1 },
	};

	[Theory]
	[MemberData(nameof(Cases))]
	public void FindsLargestSubarrayXor(int[] nums, int expected)
	{
		var actual = Task06.MaximumSubarrayXor(nums);

		Assert.Equal(expected, actual);
	}
}
