namespace CA02.Kt01;

public static class Task03
{
	/*
	Самый длинный участок с ограниченным числом типов

	Дан массив items, в котором каждое число обозначает тип товара. Покупатель
	может взять товары только с одного непрерывного участка массива.
	Он может взять товары не более чем двух разных типов. Найдите максимальное
	количество товаров, которое он сможет взять.

	Входные данные:
	items - массив целых чисел.

	Выходные данные:
	Одно целое число - максимальная длина подходящего непрерывного участка.

	Ограничения:
	0 <= len(items) <= 200 000
	0 <= items[i] <= 1 000 000

	Примеры:
	[1, 2, 1] -> 3
	[0, 1, 2, 2] -> 3, можно выбрать участок [1, 2, 2]
	[1, 2, 3, 2, 2] -> 4, можно выбрать участок [2, 3, 2, 2]
	*/
	public static int LongestSectionWithAtMostTwoTypes(int[] items)
	{
		// участок можно расширять через right, пока в нём остаётся не больше двух типов.
		// при появлении третьего типа условие вернёт только сдвиг left, поэтому выбираем sliding window.
		// dictionary хранит счётчики, чтобы удалить тип только после выхода его последнего элемента
		Dictionary<int, int> typeCounts = [];
		var left = 0;
		var longestLn = 0;

		for (var right = 0; right < items.Length; right++)
		{
			typeCounts[items[right]] = typeCounts.GetValueOrDefault(items[right]) + 1;

			while (typeCounts.Count > 2)
			{
				var leftType = items[left];
				typeCounts[leftType]--;

				if (typeCounts[leftType] == 0)
				{
					typeCounts.Remove(leftType);
				}

				left++;
			}

			longestLn = Math.Max(longestLn, right - left + 1);
		}

		return longestLn;
	}
}

public sealed class Task03Tests
{
	public static TheoryData<int[], int> Cases => new()
	{
		{ [1, 2, 1], 3 },
		{ [0, 1, 2, 2], 3 },
		{ [1, 2, 3, 2, 2], 4 },
		{ [], 0 },
		{ [4], 1 },
		{ [4, 4, 4], 3 },
		{ [1, 2, 3, 4], 2 },
		{ [1, 2, 1, 3, 4, 3, 5, 1, 2], 3 },
	};

	[Theory]
	[MemberData(nameof(Cases))]
	public void FindsLongestSectionWithAtMostTwoTypes(int[] items, int expected)
	{
		var actual = Task03.LongestSectionWithAtMostTwoTypes(items);

		Assert.Equal(expected, actual);
	}
}
