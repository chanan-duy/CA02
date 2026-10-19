namespace CA02.Kt10;

public static class Task05
{
	/*
	Кэш с частотой и давностью использования

	Реализуйте кэш вместимости C. Команда GET key выводит значение существующего ключа и
	увеличивает его частоту использования на 1, а при отсутствии ключа выводит -1 и не
	меняет состояние. Команда PUT key value заменяет значение существующего ключа на
	value, увеличивает его частоту на 1 и отмечает текущий момент использования; новый
	ключ добавляется с частотой 1. Если при добавлении кэш заполнен, удаляется ключ с
	минимальной частотой, а при равенстве - ключ с наиболее ранним моментом последнего
	успешного GET или PUT. Момент добавления считается первым использованием. Каждая
	команда должна выполняться за O(1) в среднем.

	Входные данные:
	В первой строке заданы C и q. В следующих q строках записано по одной команде GET
	key или PUT key value.

	Выходные данные:
	Для каждой команды GET выведите результат в отдельной строке.

	Ограничения:
	1 <= C <= 200 000, 1 <= q <= 500 000, 0 <= key, value <= 10^9.
	*/
	public static int[] CacheResults(int capacity, string[] ops)
	{
		// ключи одной частоты храним в порядке использования, поэтому первый элемент её списка удаляется при равенстве частот.
		// ссылка на узел списка позволяет переносить ключ в следующую частоту без поиска.
		// минимальная частота после успешного обращения либо остаётся прежней, либо увеличивается на один
		Dictionary<int, (int Value, int Freq, LinkedListNode<int> Node)> entries = [];
		Dictionary<int, LinkedList<int>> freqKeys = [];
		var minFreq = 0;
		List<int> answers = [];

		void Touch(int key, int value)
		{
			var entry = entries[key];
			var prevKeys = freqKeys[entry.Freq];
			prevKeys.Remove(entry.Node);

			if (prevKeys.Count == 0)
			{
				freqKeys.Remove(entry.Freq);

				if (minFreq == entry.Freq)
				{
					minFreq++;
				}
			}

			var freq = entry.Freq + 1;

			if (!freqKeys.TryGetValue(freq, out var nextKeys))
			{
				nextKeys = new LinkedList<int>();
				freqKeys[freq] = nextKeys;
			}

			entries[key] = (value, freq, nextKeys.AddLast(key));
		}

		foreach (var op in ops)
		{
			var parts = op.Split(' ');
			var key = int.Parse(parts[1]);

			if (parts[0] == "GET")
			{
				if (entries.TryGetValue(key, out var entry))
				{
					answers.Add(entry.Value);
					Touch(key, entry.Value);
				}
				else
				{
					answers.Add(-1);
				}

				continue;
			}

			var value = int.Parse(parts[2]);

			if (entries.ContainsKey(key))
			{
				Touch(key, value);
				continue;
			}

			if (entries.Count == capacity)
			{
				var oldestKeys = freqKeys[minFreq];
				entries.Remove(oldestKeys.First!.Value);
				oldestKeys.RemoveFirst();

				if (oldestKeys.Count == 0)
				{
					freqKeys.Remove(minFreq);
				}
			}

			if (!freqKeys.TryGetValue(1, out var newKeys))
			{
				newKeys = new LinkedList<int>();
				freqKeys[1] = newKeys;
			}

			entries[key] = (value, 1, newKeys.AddLast(key));
			minFreq = 1;
		}

		return answers.ToArray();
	}
}

public sealed class Task05Tests
{
	public static TheoryData<int, string[], int[]> Cases => new()
	{
		{
			2, ["PUT 1 10", "PUT 2 20", "GET 1", "PUT 3 30", "GET 2", "GET 3", "GET 1"],
			[10, -1, 30, 10]
		},
		{
			2, ["PUT 1 10", "PUT 2 20", "GET 1", "GET 2", "PUT 3 30", "GET 1", "GET 2"],
			[10, 20, -1, 20]
		},
		{
			1, ["GET 0", "PUT 0 0", "GET 0", "PUT 0 9", "GET 0", "PUT 1 2", "GET 0", "GET 1"],
			[-1, 0, 9, -1, 2]
		},
		{ 2, ["PUT 1 10", "PUT 2 20", "PUT 1 11", "PUT 3 30", "GET 1", "GET 2"], [11, -1] },
		{ 2, ["PUT 1 10", "PUT 2 20", "GET 9", "PUT 3 30", "GET 1", "GET 2"], [-1, -1, 20] },
	};

	[Theory]
	[MemberData(nameof(Cases))]
	public void EvictsLeastFrequentThenLeastRecentlyUsedKey(int capacity, string[] ops, int[] expected)
	{
		var actual = Task05.CacheResults(capacity, ops);

		Assert.Equal(expected, actual);
	}
}
