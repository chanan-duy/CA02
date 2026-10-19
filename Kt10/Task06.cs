namespace CA02.Kt10;

public static class Task06
{
	/*
	Ближайшие пункты с полным порядком сравнения

	Даны n пунктов на плоскости и число k. Номер пункта равен его позиции во входных
	данных, начиная с 1. Пункты сравниваются по возрастанию x^2 + y^2, затем по
	возрастанию x, затем y и, наконец, исходного номера. Выведите первые k пунктов в
	этом порядке.

	Входные данные:
	В первой строке заданы n и k. В следующих n строках заданы координаты x и y.

	Выходные данные:
	Выведите k строк. Для каждого выбранного пункта выведите x, y и его исходный номер.
	Строки должны идти в заданном порядке сравнения.

	Ограничения:
	1 <= k <= min(n, 100 000), 1 <= n <= 2 000 000, -10^9 <= x, y <= 10^9. Значение x^2
	+ y^2 помещается в 64-битный знаковый целый тип. Требуемое среднее время O(n + k log
	k), дополнительная память O(k) без учёта хранения входных данных и результата.
	*/
	public static (int X, int Y, int Index)[] ClosestPoints((int X, int Y, int Index)[] points, int requiredCount)
	{
		// исходный номер хранится вместе с координатами при чтении, поэтому перестановки не меняют последнее правило сравнения.
		// quickselect отделяет первые requiredCount пунктов за линейное среднее время без отдельного массива для всех пунктов.
		// сортируем только выбранную часть, что добавляет requiredCount * log(requiredCount) операций
		var left = 0;
		var right = points.Length - 1;
		var target = requiredCount - 1;

		while (left < right)
		{
			var pivot = points[Random.Shared.Next(left, right + 1)];
			var smallerEnd = left;
			var current = left;
			var largerStart = right;

			while (current <= largerStart)
			{
				var cmp = ComparePoints(points[current], pivot);

				if (cmp < 0)
				{
					(points[smallerEnd], points[current]) = (points[current], points[smallerEnd]);
					smallerEnd++;
					current++;
				}
				else if (cmp > 0)
				{
					(points[current], points[largerStart]) = (points[largerStart], points[current]);
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
				break;
			}
		}

		var result = new (int X, int Y, int Index)[requiredCount];
		Array.Copy(points, result, requiredCount);
		Array.Sort(result, ComparePoints);

		return result;
	}

	private static int ComparePoints((int X, int Y, int Index) first, (int X, int Y, int Index) second)
	{
		var firstDist = (long)first.X * first.X + (long)first.Y * first.Y;
		var secondDist = (long)second.X * second.X + (long)second.Y * second.Y;
		var cmp = firstDist.CompareTo(secondDist);

		if (cmp == 0)
		{
			cmp = first.X.CompareTo(second.X);
		}

		if (cmp == 0)
		{
			cmp = first.Y.CompareTo(second.Y);
		}

		return cmp == 0 ? first.Index.CompareTo(second.Index) : cmp;
	}
}

public sealed class Task06Tests
{
	public static TheoryData<int[][], int, int[][]> Cases => new()
	{
		{ [[1, 3], [-2, 2]], 1, [[-2, 2, 2]] },
		{
			[[1, 0], [-1, 0], [0, 1], [0, -1]], 4,
			[[-1, 0, 2], [0, -1, 4], [0, 1, 3], [1, 0, 1]]
		},
		{ [[2, 2], [0, 0], [2, 2], [2, 2]], 3, [[0, 0, 2], [2, 2, 1], [2, 2, 3]] },
		{
			[[1_000_000_000, 1_000_000_000], [-1_000_000_000, -1_000_000_000]], 1,
			[[-1_000_000_000, -1_000_000_000, 2]]
		},
		{ [[5, 7]], 1, [[5, 7, 1]] },
	};

	[Theory]
	[MemberData(nameof(Cases))]
	public void SelectsAndOrdersPointsByEveryTieBreaker(int[][] coordinates, int requiredCount, int[][] expected)
	{
		var points = coordinates.Select((point, index) => (X: point[0], Y: point[1], Index: index + 1)).ToArray();

		var actual = Task06.ClosestPoints(points, requiredCount);

		Assert.Equal(expected.Select(point => (point[0], point[1], point[2])), actual);
	}
}
