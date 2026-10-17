namespace CA02.Kt08;

public static class Task03
{
	/*
	Длина наибольшей общей подпоследовательности

	Даны две строки s и t. Удалите из каждой строки некоторое количество символов,
	возможно ноль, так, чтобы получившиеся строки совпали. Порядок оставшихся символов
	изменять нельзя. Найдите максимальную возможную длину общей строки.

	Входные данные:
	В первой строке задана строка s, во второй - строка t. Строки состоят только из
	строчных латинских букв.

	Выходные данные:
	Выведите максимальную длину общей строки.

	Ограничения:
	1 <= длина s, длина t <= 3 000, произведение длин не превышает 9 000 000.
	*/
	public static int LongestCommonSubsequence(string first, string second)
	{
		// совпавшие последние символы продолжают ответ для двух укороченных префиксов.
		// при несовпадении отбрасываем один из последних символов и выбираем лучший вариант.
		// одной строки dp достаточно, если сохранить старое диагональное значение перед перезаписью
		var lengths = new int[second.Length + 1];

		foreach (var ch in first)
		{
			var diagonal = 0;

			for (var index = 1; index <= second.Length; index++)
			{
				var prev = lengths[index];
				lengths[index] = ch == second[index - 1]
					? diagonal + 1
					: Math.Max(lengths[index], lengths[index - 1]);
				diagonal = prev;
			}
		}

		return lengths[^1];
	}
}

public sealed class Task03Tests
{
	public static TheoryData<string, string, int> Cases => new()
	{
		{ "abcde", "ace", 3 },
		{ "abc", "abc", 3 },
		{ "abc", "def", 0 },
		{ "aaaa", "aa", 2 },
		{ "ab", "ba", 1 },
		{ "a", "ba", 1 },
	};

	[Theory]
	[MemberData(nameof(Cases))]
	public void FindsLongestCommonSubsequenceLength(string first, string second, int expected)
	{
		var actual = Task03.LongestCommonSubsequence(first, second);

		Assert.Equal(expected, actual);
	}
}
