using System.Text;

namespace CA02.Kt05;

public static class Task05
{
	/*
	k-е слово с заданным префиксом

	Поддерживайте изменяющееся множество слов. Операции ADD word и REMOVE word добавляют
	и удаляют слово. Операция KTH prefix k запрашивает k-е слово в лексикографическом
	порядке среди всех слов, начинающихся с prefix. Если подходящих слов меньше k,
	выведите NONE. Если одно слово является началом другого, более короткое считается
	меньшим. Перед ADD слово отсутствует, перед REMOVE присутствует.

	Входные данные:
	В первой строке задано q. В следующих q строках записано по одной операции. Слова и
	префиксы состоят только из строчных латинских букв.

	Выходные данные:
	Для каждой операции KTH выведите найденное слово или NONE в отдельной строке.

	Ограничения:
	1 <= q <= 200 000, 1 <= k <= 200 000, 1 <= длина слова или префикса <= 50. Суммарная
	длина строк не превышает 1 000 000.
	*/
	public static string[] KthWords(string[] ops)
	{
		// счётчик поддерева trie позволяет пропустить сразу все слова, которые идут раньше искомого.
		// конец слова проверяем перед потомками, потому что короткое слово меньше своего продолжения.
		// потомков обходим по буквам, поэтому удаление слова не нарушает порядок остальных
		var root = new TrieNode();
		List<string> words = [];

		foreach (var op in ops)
		{
			var parts = op.Split(' ');
			var current = root;

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

				if (parts[0] != "KTH")
				{
					current.WordCount += parts[0] == "ADD" ? 1 : -1;
				}
			}

			if (parts[0] != "KTH")
			{
				current!.IsWord = parts[0] == "ADD";
				continue;
			}

			var rank = int.Parse(parts[2]);

			if (current == null || current.WordCount < rank)
			{
				words.Add("NONE");
				continue;
			}

			var word = new StringBuilder(parts[1]);

			while (true)
			{
				if (current.IsWord)
				{
					rank--;

					if (rank == 0)
					{
						words.Add(word.ToString());
						break;
					}
				}

				for (var ch = 'a'; ch <= 'z'; ch++)
				{
					if (!current.Children.TryGetValue(ch, out var child))
					{
						continue;
					}

					if (rank > child.WordCount)
					{
						rank -= child.WordCount;
						continue;
					}

					word.Append(ch);
					current = child;
					break;
				}
			}
		}

		return words.ToArray();
	}

	private sealed class TrieNode
	{
		public Dictionary<char, TrieNode> Children { get; } = [];
		public int WordCount { get; set; }
		public bool IsWord { get; set; }
	}
}

public sealed class Task05Tests
{
	public static TheoryData<string[], string[]> Cases => new()
	{
		{ ["KTH a 1"], ["NONE"] },
		{
			["ADD app", "ADD apple", "ADD apply", "KTH app 1", "KTH app 2", "KTH app 3", "KTH app 4"],
			["app", "apple", "apply", "NONE"]
		},
		{ ["ADD ab", "ADD aa", "ADD ac", "KTH a 2", "REMOVE ab", "KTH a 2"], ["ab", "ac"] },
		{
			["ADD a", "ADD ab", "REMOVE a", "KTH a 1", "REMOVE ab", "KTH a 1", "ADD a", "KTH a 1"],
			["ab", "NONE", "a"]
		},
		{ ["ADD cat", "ADD dog", "KTH c 1", "KTH d 1", "KTH x 1"], ["cat", "dog", "NONE"] },
	};

	[Theory]
	[MemberData(nameof(Cases))]
	public void FindsKthWordAfterDictionaryChanges(string[] ops, string[] expected)
	{
		var actual = Task05.KthWords(ops);

		Assert.Equal(expected, actual);
	}
}
