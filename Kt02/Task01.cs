namespace CA02.Kt02;

public static class Task01
{
	/*
	Первая позиция для вставки

	Дан отсортированный по неубыванию массив целых чисел nums и целое число target.
	Определите индекс первой позиции, на которую можно вставить target так,
	чтобы массив остался отсортированным. Если target уже встречается несколько
	раз, нужно вернуть индекс его первого вхождения.

	Входные данные:
	В первой строке заданы n и target. Во второй строке заданы n целых чисел nums.

	Выходные данные:
	Одно целое число - индекс первой позиции для вставки.

	Ограничения:
	0 <= n <= 200 000
	-10^9 <= nums[i], target <= 10^9
	*/
	public static int FirstInsertionIndex(int[] nums, int target)
	{
		// в отсортированном массиве все числа меньше target лежат перед искомой позицией.
		// поэтому выбираем binary search: при равенстве сдвигаем right, чтобы найти первое вхождение.
		// right начинается за массивом, потому что вставить число можно и после последнего элемента
		var left = 0;
		var right = nums.Length;

		while (left < right)
		{
			var middle = left + (right - left) / 2;

			if (nums[middle] < target)
			{
				left = middle + 1;
			}
			else
			{
				right = middle;
			}
		}

		return left;
	}
}

public sealed class Task01Tests
{
	public static TheoryData<int[], int, int> Cases => new()
	{
		{ [], 5, 0 },
		{ [1, 3, 3, 3, 5], 3, 1 },
		{ [1, 3, 5], 4, 2 },
		{ [1, 3, 5], 0, 0 },
		{ [1, 3, 5], 6, 3 },
		{ [2, 2, 2], 2, 0 },
		{ [5], 5, 0 },
		{ [-1_000_000_000, 0, 1_000_000_000], 1_000_000_000, 2 },
	};

	[Theory]
	[MemberData(nameof(Cases))]
	public void FindsFirstInsertionIndex(int[] nums, int target, int expected)
	{
		var actual = Task01.FirstInsertionIndex(nums, target);

		Assert.Equal(expected, actual);
	}
}
