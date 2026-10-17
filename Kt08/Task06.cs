using System.Text;

namespace CA02.Kt08;

public static class Task06
{
	/*
	Кратчайшая общая надпоследовательность

	Даны строки s и t. Найдите строку минимальной длины, в которой и s, и t встречаются
	как подпоследовательности. Если подходящих строк несколько, выведите
	лексикографически наименьшую из них.

	Входные данные:
	В первой строке задана строка s, во второй - строка t. Обе строки состоят только из
	строчных латинских букв.

	Выходные данные:
	Выведите искомую строку.

	Ограничения:
	1 <= длина s, длина t <= 1 000.
	*/
	public static string SmallestCommonSupersequence(string first, string second)
	{
		// длины ответов для суффиксов позволяют восстановить кратчайшую строку без перебора всех вариантов.
		// совпавший символ добавляем один раз, а при разных символах выбираем переход с меньшей итоговой длиной.
		// при равенстве длин меньший первый символ определяет лексикографически меньший ответ
		var lengths = new int[first.Length + 1, second.Length + 1];

		for (var left = first.Length; left >= 0; left--)
		{
			for (var right = second.Length; right >= 0; right--)
			{
				if (left == first.Length)
				{
					lengths[left, right] = second.Length - right;
				}
				else if (right == second.Length)
				{
					lengths[left, right] = first.Length - left;
				}
				else
				{
					lengths[left, right] = first[left] == second[right]
						? 1 + lengths[left + 1, right + 1]
						: 1 + Math.Min(lengths[left + 1, right], lengths[left, right + 1]);
				}
			}
		}

		var result = new StringBuilder();
		var firstIndex = 0;
		var secondIndex = 0;

		while (firstIndex < first.Length && secondIndex < second.Length)
		{
			if (first[firstIndex] == second[secondIndex])
			{
				result.Append(first[firstIndex++]);
				secondIndex++;
			}
			else if (lengths[firstIndex + 1, secondIndex] < lengths[firstIndex, secondIndex + 1]
			         || (lengths[firstIndex + 1, secondIndex] == lengths[firstIndex, secondIndex + 1]
			             && first[firstIndex] < second[secondIndex]))
			{
				result.Append(first[firstIndex++]);
			}
			else
			{
				result.Append(second[secondIndex++]);
			}
		}

		result.Append(first.AsSpan(firstIndex));
		result.Append(second.AsSpan(secondIndex));

		return result.ToString();
	}
}

public sealed class Task06Tests
{
	public static TheoryData<string, string, string> Cases => new()
	{
		{ "ab", "ba", "aba" },
		{ "abc", "abc", "abc" },
		{ "a", "b", "ab" },
		{ "aaaa", "aa", "aaaa" },
		{ "abc", "acb", "abcb" },
		{ "cab", "abc", "cabc" },
		{ "z", "a", "az" },
	};

	[Theory]
	[MemberData(nameof(Cases))]
	public void FindsSmallestAmongShortestCommonSupersequences(string first, string second, string expected)
	{
		var actual = Task06.SmallestCommonSupersequence(first, second);

		Assert.Equal(expected, actual);
	}
}
