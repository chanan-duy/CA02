namespace CA02.Kt05;

public static class Task03
{
	/*
	Два числа без пары

	Дан массив, в котором ровно два различных числа встречаются по одному разу, а каждое
	из остальных чисел встречается ровно два раза. Найдите два числа, встретившиеся по
	одному разу.

	Входные данные:
	В первой строке задано чётное число n. Во второй строке заданы n целых чисел.

	Выходные данные:
	Выведите два искомых числа в порядке возрастания.

	Ограничения:
	2 <= n <= 200 000, 0 <= a[i] <= 10^9.
	*/
	public static int[] FindSingleNumbers(int[] nums)
	{
		// xor одинаковых чисел равен нулю, поэтому после прохода остаётся xor двух искомых чисел.
		// в его младшем установленном бите эти числа различаются, что позволяет разделить их на две группы.
		// каждая повторяющаяся пара попадает в одну группу и снова сокращается
		var combined = 0;

		foreach (var number in nums)
		{
			combined ^= number;
		}

		var splitBit = combined & -combined;
		var first = 0;
		var second = 0;

		foreach (var number in nums)
		{
			if ((number & splitBit) == 0)
			{
				first ^= number;
			}
			else
			{
				second ^= number;
			}
		}

		return first < second ? [first, second] : [second, first];
	}
}

public sealed class Task03Tests
{
	public static TheoryData<int[], int[]> Cases => new()
	{
		{ [1, 2, 1, 3, 2, 5], [3, 5] },
		{ [0, 1], [0, 1] },
		{ [7, 0, 7, 4], [0, 4] },
		{ [10, 8, 2, 2], [8, 10] },
		{ [1_000_000_000, 0, 4, 4], [0, 1_000_000_000] },
	};

	[Theory]
	[MemberData(nameof(Cases))]
	public void FindsBothUnpairedNumbers(int[] nums, int[] expected)
	{
		var actual = Task03.FindSingleNumbers(nums);

		Assert.Equal(expected, actual);
	}
}
