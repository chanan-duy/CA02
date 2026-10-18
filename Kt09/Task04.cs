namespace CA02.Kt09;

public static class Task04
{
	/*
	Минимальная стоимость объединения соседних фрагментов

	В ряд расположены n фрагментов размеров a[1], a[2], ..., a[n]. За одну операцию
	разрешается объединить две соседние текущие группы. Стоимость операции равна
	суммарному размеру объединяемых групп, после чего образуется одна группа этого
	размера. Найдите минимальную стоимость объединения всех фрагментов в одну группу.

	Входные данные:
	В первой строке задано n. Во второй строке заданы n целых чисел a[i].

	Выходные данные:
	Выведите минимальную суммарную стоимость. Для n = 1 ответ равен 0.

	Ограничения:
	1 <= n <= 500, 1 <= a[i] <= 10^9. Ответ помещается в 64-битный знаковый целый тип.
	*/
	public static long MinimumMergeCost(int[] sizes)
	{
		// последнее объединение отрезка разделяет его на две соседние группы, поэтому перебираем место этого разделения.
		// к стоимости двух подотрезков добавляется их общая масса, которую получаем через префиксные суммы.
		// короткие отрезки считаем раньше длинных, чтобы все варианты последнего объединения уже были известны
		var prefixSums = new long[sizes.Length + 1];

		for (var index = 0; index < sizes.Length; index++)
		{
			prefixSums[index + 1] = prefixSums[index] + sizes[index];
		}

		var costs = new long[sizes.Length, sizes.Length];

		for (var length = 2; length <= sizes.Length; length++)
		{
			for (var left = 0; left + length <= sizes.Length; left++)
			{
				var right = left + length - 1;
				var minimum = long.MaxValue;

				for (var middle = left; middle < right; middle++)
				{
					minimum = Math.Min(minimum, costs[left, middle] + costs[middle + 1, right]);
				}

				costs[left, right] = minimum + prefixSums[right + 1] - prefixSums[left];
			}
		}

		return costs[0, sizes.Length - 1];
	}
}

public sealed class Task04Tests
{
	public static TheoryData<int[], long> Cases => new()
	{
		{ [5], 0 },
		{ [1, 2], 3 },
		{ [10, 20, 30], 90 },
		{ [40, 30, 30, 50], 300 },
		{ [1, 1, 1, 1], 8 },
		{ [1_000_000_000, 1_000_000_000, 1_000_000_000], 5_000_000_000 },
	};

	[Theory]
	[MemberData(nameof(Cases))]
	public void FindsCheapestAdjacentMergeOrder(int[] sizes, long expected)
	{
		var actual = Task04.MinimumMergeCost(sizes);

		Assert.Equal(expected, actual);
	}
}
