namespace CA02.Kt01;

public static class Task06
{
	/*
	Минимальная подстрока-покрытие

	Даны строки source и target. Найдите самую короткую подстроку source, которая
	содержит все символы строки target в нужном количестве.
	Если символ встречается в target несколько раз, в найденной подстроке он тоже
	должен встретиться не меньшее число раз.
	Если подходящей подстроки нет, верните пустую строку.
	Символы чувствительны к регистру: "A" и "a" - разные символы.

	Входные данные:
	source - строка.
	target - строка.

	Выходные данные:
	Самая короткая подходящая подстрока source или пустая строка.

	Ограничения:
	1 <= len(source) <= 200 000
	1 <= len(target) <= 100 000
	source и target содержат латинские буквы и цифры.

	Примеры:
	source = "ADOBECODEBANC", target = "ABC" -> "BANC"
	source = "a", target = "aa" -> ""
	source = "AAABBC", target = "AABC" -> "AABBC"
	*/
	public static string MinimumCoveringSubstring(string source, string target)
	{
		// пока подстрока не покрывает target, только сдвиг right может добавить недостающий символ.
		// после покрытия только сдвиг left может сделать её короче, поэтому выбираем sliding window.
		// dictionary хранит оставшееся нужное количество каждого символа и учитывает повторы
		if (target.Length == 0 || target.Length > source.Length)
		{
			return "";
		}

		Dictionary<char, int> requiredCounts = [];

		foreach (var ch in target)
		{
			requiredCounts[ch] = requiredCounts.GetValueOrDefault(ch) + 1;
		}

		var missingCount = target.Length;
		var left = 0;
		var bestStart = 0;
		var bestLn = int.MaxValue;

		for (var right = 0; right < source.Length; right++)
		{
			var addedCharacter = source[right];

			if (requiredCounts.TryGetValue(addedCharacter, out var remainingCount))
			{
				if (remainingCount > 0)
				{
					missingCount--;
				}

				requiredCounts[addedCharacter] = remainingCount - 1;
			}

			while (missingCount == 0)
			{
				var currentLn = right - left + 1;

				if (currentLn < bestLn)
				{
					bestStart = left;
					bestLn = currentLn;
				}

				var removedChar = source[left];
				left++;

				if (!requiredCounts.TryGetValue(removedChar, out var countBeforeRem))
				{
					continue;
				}

				requiredCounts[removedChar] = countBeforeRem + 1;

				if (countBeforeRem >= 0)
				{
					missingCount++;
				}
			}
		}

		return bestLn == int.MaxValue ? "" : source.Substring(bestStart, bestLn);
	}
}

public sealed class Task06Tests
{
	public static TheoryData<string, string, string> Cases => new()
	{
		{ "ADOBECODEBANC", "ABC", "BANC" },
		{ "a", "aa", "" },
		{ "AAABBC", "AABC", "AABBC" },
		{ "a", "a", "a" },
		{ "aA", "Aa", "aA" },
		{ "aaab", "aab", "aab" },
		{ "a1b2c1", "11", "1b2c1" },
		{ "abcdef", "xyz", "" },
	};

	[Theory]
	[MemberData(nameof(Cases))]
	public void FindsMinimumCoveringSubstring(string source, string target, string expected)
	{
		var actual = Task06.MinimumCoveringSubstring(source, target);

		Assert.Equal(expected, actual);
	}
}
