namespace CA02.Kt10;

public static class Task02
{
	/*
	Стек с минимумом и максимумом

	Обработайте последовательность команд над журналом, работающим по правилу: последним
	добавлен - первым удалён. Команда PUSH x добавляет значение x, POP удаляет последнее
	добавленное значение, MIN выводит минимальное, а MAX - максимальное значение среди
	находящихся в журнале. Каждая команда должна выполняться за O(1) в худшем случае.

	Входные данные:
	В первой строке задано q. В следующих q строках записано по одной команде.
	Гарантируется, что POP, MIN и MAX не выполняются для пустого журнала.

	Выходные данные:
	Для каждой команды MIN или MAX выведите ответ в отдельной строке. Команды PUSH и POP
	ничего не выводят.

	Ограничения:
	1 <= q <= 1 000 000, -10^9 <= x <= 10^9.
	*/
	public static int[] StackExtremes(string[] ops)
	{
		// для каждой глубины сохраняем минимум и максимум всего префикса стека.
		// после pop предыдущая глубина уже содержит нужные границы, поэтому пересчёт не требуется.
		// массивы выделяем по числу команд заранее, чтобы push не вызывал расширение коллекции
		var mins = new int[ops.Length];
		var maxs = new int[ops.Length];
		var count = 0;
		List<int> answers = new(ops.Length);

		foreach (var op in ops)
		{
			var parts = op.Split(' ');

			if (parts[0] == "PUSH")
			{
				var number = int.Parse(parts[1]);
				mins[count] = count == 0 ? number : Math.Min(mins[count - 1], number);
				maxs[count] = count == 0 ? number : Math.Max(maxs[count - 1], number);
				count++;
			}
			else if (parts[0] == "POP")
			{
				count--;
			}
			else
			{
				answers.Add(parts[0] == "MIN" ? mins[count - 1] : maxs[count - 1]);
			}
		}

		return answers.ToArray();
	}
}

public sealed class Task02Tests
{
	public static TheoryData<string[], int[]> Cases => new()
	{
		{ ["PUSH 3", "PUSH 1", "MIN", "MAX", "POP", "MIN", "MAX"], [1, 3, 3, 3] },
		{ ["PUSH 2", "PUSH 2", "POP", "MIN", "MAX"], [2, 2] },
		{ ["PUSH -1000000000", "PUSH 1000000000", "MIN", "MAX"], [-1_000_000_000, 1_000_000_000] },
		{ ["PUSH 1", "POP", "PUSH -5", "MAX", "MIN"], [-5, -5] },
		{ ["PUSH 1", "POP"], [] },
	};

	[Theory]
	[MemberData(nameof(Cases))]
	public void RestoresExtremesAfterPopping(string[] ops, int[] expected)
	{
		var actual = Task02.StackExtremes(ops);

		Assert.Equal(expected, actual);
	}
}
