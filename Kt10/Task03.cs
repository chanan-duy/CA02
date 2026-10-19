namespace CA02.Kt10;

public static class Task03
{
	/*
	k-е число после упорядочивания

	Дан массив из n целых чисел и число k. Найдите значение, которое находилось бы на
	позиции k после упорядочивания массива по неубыванию. Позиции нумеруются с 1,
	одинаковые значения занимают отдельные позиции. Массив разрешается изменять.

	Входные данные:
	В первой строке заданы n и k. Во второй строке заданы n целых чисел.

	Выходные данные:
	Выведите значение на позиции k.

	Ограничения:
	1 <= k <= n <= 5 000 000. Элементы находятся в диапазоне 32-битного знакового целого
	типа. Требуемое среднее время работы O(n), дополнительная память O(1) без учёта
	входного массива.
	*/
	public static int KthSmallestNumber(int[] nums, int rank)
	{
		// quickselect оставляет только часть массива, содержащую нужную позицию, без сортировки остальных элементов.
		// случайная опора даёт линейное среднее время независимо от исходного порядка.
		// три части отделяют все равные опоре значения сразу, чтобы повторы не приводили к квадратичному перебору
		var left = 0;
		var right = nums.Length - 1;
		var target = rank - 1;

		while (left < right)
		{
			var pivot = nums[Random.Shared.Next(left, right + 1)];
			var smallerEnd = left;
			var current = left;
			var largerStart = right;

			while (current <= largerStart)
			{
				if (nums[current] < pivot)
				{
					(nums[smallerEnd], nums[current]) = (nums[current], nums[smallerEnd]);
					smallerEnd++;
					current++;
				}
				else if (nums[current] > pivot)
				{
					(nums[current], nums[largerStart]) = (nums[largerStart], nums[current]);
					largerStart--;
				}
				else
				{
					current++;
				}
			}

			if (target < smallerEnd)
			{
				right = smallerEnd - 1;
			}
			else if (target > largerStart)
			{
				left = largerStart + 1;
			}
			else
			{
				return nums[target];
			}
		}

		return nums[left];
	}
}

public sealed class Task03Tests
{
	public static TheoryData<int[], int, int> Cases => new()
	{
		{ [3, 2, 1, 5, 6, 4], 2, 2 },
		{ [3, 2, 3, 1, 2, 4, 5, 5, 6], 4, 3 },
		{ [7], 1, 7 },
		{ [2, 2, 2, 2], 3, 2 },
		{ [1, 2, 3, 4], 4, 4 },
		{ [4, 3, 2, 1], 1, 1 },
		{ [int.MaxValue, 0, int.MinValue], 2, 0 },
	};

	[Theory]
	[MemberData(nameof(Cases))]
	public void FindsValueAtRequestedSortedPosition(int[] nums, int rank, int expected)
	{
		var original = nums.Order().ToArray();

		var actual = Task03.KthSmallestNumber(nums, rank);

		Assert.Equal(expected, actual);
		Assert.Equal(original, nums.Order());
	}
}
