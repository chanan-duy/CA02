using System.Text;

namespace CA02.Kt06;

public static class Task06
{
	/*
	Восстановление порядка букв

	Алфавит состоит из первых k строчных латинских букв, порядок которых неизвестен. Дан
	список слов, считающийся отсортированным по этому порядку. При сравнении
	используется первая различающаяся позиция; если одно слово является началом другого,
	более короткое должно идти раньше. Определите, существует ли подходящий порядок
	букв. Если вариантов несколько, выведите лексикографически наименьшую строку в
	обычном порядке латинских букв. Каждая из k букв должна встретиться в строке ровно
	один раз.

	Входные данные:
	В первой строке заданы n и k. В следующих n строках записано по одному непустому
	слову из первых k строчных латинских букв.

	Выходные данные:
	Если подходящего порядка нет, выведите IMPOSSIBLE. Иначе выведите строку длины k,
	задающую порядок букв от наименьшей к наибольшей.

	Ограничения:
	1 <= n <= 100 000, 1 <= k <= 26. Суммарная длина слов не превышает 200 000.
	Одинаковые соседние слова допускаются.
	*/
	public static string SmallestAlphabet(string[] words, int letterCount)
	{
		// порядок соседних слов задаёт только первая различающаяся буква, остальные позиции не добавляют требований.
		// длинное слово перед собственным префиксом невозможно исправить перестановкой алфавита.
		// из букв без входящих требований берём наименьшую, включая буквы, которых в словах нет
		var edges = new bool[letterCount, letterCount];
		var inDegrees = new int[letterCount];

		for (var index = 1; index < words.Length; index++)
		{
			var prev = words[index - 1];
			var current = words[index];
			var position = 0;

			while (position < Math.Min(prev.Length, current.Length) && prev[position] == current[position])
			{
				position++;
			}

			if (position == Math.Min(prev.Length, current.Length))
			{
				if (prev.Length > current.Length)
				{
					return "IMPOSSIBLE";
				}

				continue;
			}

			var first = prev[position] - 'a';
			var second = current[position] - 'a';

			if (!edges[first, second])
			{
				edges[first, second] = true;
				inDegrees[second]++;
			}
		}

		PriorityQueue<int, int> availableLetters = new();

		for (var letter = 0; letter < letterCount; letter++)
		{
			if (inDegrees[letter] == 0)
			{
				availableLetters.Enqueue(letter, letter);
			}
		}

		var alphabet = new StringBuilder();

		while (availableLetters.Count > 0)
		{
			var letter = availableLetters.Dequeue();
			alphabet.Append((char)('a' + letter));

			for (var next = 0; next < letterCount; next++)
			{
				if (edges[letter, next] && --inDegrees[next] == 0)
				{
					availableLetters.Enqueue(next, next);
				}
			}
		}

		return alphabet.Length == letterCount ? alphabet.ToString() : "IMPOSSIBLE";
	}
}

public sealed class Task06Tests
{
	public static TheoryData<string[], int, string> Cases => new()
	{
		{ ["ba", "bc", "ac", "cab"], 3, "bac" },
		{ ["abc", "ab"], 3, "IMPOSSIBLE" },
		{ ["a", "b", "a"], 2, "IMPOSSIBLE" },
		{ ["a", "a"], 3, "abc" },
		{ ["ab", "aba"], 3, "abc" },
		{ ["b", "a"], 3, "bac" },
		{ ["a"], 1, "a" },
	};

	[Theory]
	[MemberData(nameof(Cases))]
	public void FindsSmallestCompatibleAlphabet(string[] words, int letterCount, string expected)
	{
		var actual = Task06.SmallestAlphabet(words, letterCount);

		Assert.Equal(expected, actual);
	}
}
