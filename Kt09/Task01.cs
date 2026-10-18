namespace CA02.Kt09;

public static class Task01
{
	/*
	Минимальное число палиндромных частей

	Дана строка s. Разделите её на минимальное количество непустых подряд идущих частей
	так, чтобы каждая часть одинаково читалась слева направо и справа налево.

	Входные данные:
	В единственной строке задана строка s, состоящая из строчных латинских букв.

	Выходные данные:
	Выведите минимальное количество частей.

	Ограничения:
	1 <= длина s <= 4 000.
	*/
	public static int MinimumPalindromeParts(string source)
	{
		// подстрока является палиндромом, если совпадают её края и внутренняя часть тоже является палиндромом.
		// для каждого конца пробуем начало последней части и добавляем её к лучшему разбиению предыдущего префикса.
		// порядок по правой границе гарантирует, что оба нужных состояния уже вычислены
		var palindromes = new bool[source.Length, source.Length];
		var partCounts = Enumerable.Repeat(source.Length, source.Length + 1).ToArray();
		partCounts[0] = 0;

		for (var right = 0; right < source.Length; right++)
		{
			for (var left = 0; left <= right; left++)
			{
				if (source[left] == source[right] && (right - left < 2 || palindromes[left + 1, right - 1]))
				{
					palindromes[left, right] = true;
					partCounts[right + 1] = Math.Min(partCounts[right + 1], partCounts[left] + 1);
				}
			}
		}

		return partCounts[^1];
	}
}

public sealed class Task01Tests
{
	public static TheoryData<string, int> Cases => new()
	{
		{ "a", 1 },
		{ "aab", 2 },
		{ "racecar", 1 },
		{ "abcd", 4 },
		{ "abacdc", 2 },
		{ "aaaa", 1 },
		{ "abcbdd", 3 },
	};

	[Theory]
	[MemberData(nameof(Cases))]
	public void FindsFewestPalindromeParts(string source, int expected)
	{
		var actual = Task01.MinimumPalindromeParts(source);

		Assert.Equal(expected, actual);
	}
}
