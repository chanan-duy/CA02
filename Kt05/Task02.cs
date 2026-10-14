namespace CA02.Kt05;

public static class Task02
{
	/*
	Количество слов с заданным префиксом

	Требуется обрабатывать изменяющийся словарь. Операция ADD word добавляет слово,
	REMOVE word удаляет слово, а COUNT prefix запрашивает количество слов, начинающихся
	с указанного префикса. Перед добавлением слово отсутствует в словаре, а перед
	удалением присутствует.

	Входные данные:
	В первой строке задано q. В следующих q строках записано по одной операции. Все
	слова и префиксы состоят только из строчных латинских букв.

	Выходные данные:
	Для каждой операции COUNT выведите ответ в отдельной строке.

	Ограничения:
	1 <= q <= 200 000, 1 <= длина слова или префикса <= 50. Суммарная длина всех строк
	во входных данных не превышает 1 000 000.
	*/
	public static int[] PrefixCounts(string[] ops)
	{
		// в trie все слова с общим префиксом проходят через одну вершину.
		// её счётчик меняется при добавлении и удалении, поэтому запрос не перебирает весь словарь.
		// вершины с нулевым счётчиком можно сохранить для последующих добавлений
		var root = new TrieNode();
		List<int> counts = [];

		foreach (var op in ops)
		{
			var parts = op.Split(' ');
			var current = root;
			var change = parts[0] == "ADD" ? 1 : -1;

			foreach (var ch in parts[1])
			{
				if (!current.Children.TryGetValue(ch, out var child))
				{
					if (parts[0] != "ADD")
					{
						current = null;
						break;
					}

					child = new TrieNode();
					current.Children[ch] = child;
				}

				current = child;

				if (parts[0] != "COUNT")
				{
					current.WordCount += change;
				}
			}

			if (parts[0] == "COUNT")
			{
				counts.Add(current?.WordCount ?? 0);
			}
		}

		return counts.ToArray();
	}

	private sealed class TrieNode
	{
		public Dictionary<char, TrieNode> Children { get; } = [];
		public int WordCount { get; set; }
	}
}

public sealed class Task02Tests
{
	public static TheoryData<string[], int[]> Cases => new()
	{
		{ ["COUNT a"], [0] },
		{ ["ADD apple", "ADD app", "COUNT app", "COUNT apple", "COUNT b"], [2, 1, 0] },
		{ ["ADD a", "ADD ab", "REMOVE a", "COUNT a", "COUNT ab"], [1, 1] },
		{ ["ADD abc", "REMOVE abc", "COUNT a", "ADD abc", "COUNT ab"], [0, 1] },
		{ ["ADD cat", "ADD car", "ADD dog", "REMOVE car", "COUNT ca", "COUNT d"], [1, 1] },
	};

	[Theory]
	[MemberData(nameof(Cases))]
	public void CountsWordsAfterDictionaryChanges(string[] ops, int[] expected)
	{
		var actual = Task02.PrefixCounts(ops);

		Assert.Equal(expected, actual);
	}
}
