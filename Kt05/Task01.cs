namespace CA02.Kt05;

public static class Task01
{
	/*
	Правильные скобочные последовательности

	По заданному числу пар круглых скобок выведите все правильные скобочные
	последовательности. В каждом префиксе правильной последовательности количество
	закрывающих скобок не превышает количество открывающих. Последовательности выведите
	в лексикографическом порядке, считая символ ( меньше символа ).

	Входные данные:
	В единственной строке задано n - количество пар скобок.

	Выходные данные:
	Выведите все правильные последовательности длины 2 * n, по одной в строке.

	Ограничения:
	1 <= n <= 10.
	*/
	public static string[] GenerateParentheses(int pairCount)
	{
		// закрывающую скобку можно добавить только к уже открытой, поэтому неверные префиксы не перебираем.
		// сначала пробуем открывающую скобку, чтобы сразу получить лексикографический порядок.
		// общий буфер меняется при возврате из рекурсии, а готовая строка сохраняет отдельную копию
		var seq = new char[pairCount * 2];
		List<string> seqs = [];

		void Generate(int openCount, int closedCount)
		{
			var index = openCount + closedCount;

			if (index == seq.Length)
			{
				seqs.Add(new string(seq));

				return;
			}

			if (openCount < pairCount)
			{
				seq[index] = '(';
				Generate(openCount + 1, closedCount);
			}

			if (closedCount < openCount)
			{
				seq[index] = ')';
				Generate(openCount, closedCount + 1);
			}
		}

		Generate(0, 0);

		return seqs.ToArray();
	}
}

public sealed class Task01Tests
{
	public static TheoryData<int, string[]> Cases => new()
	{
		{ 1, ["()"] },
		{ 2, ["(())", "()()"] },
		{ 3, ["((()))", "(()())", "(())()", "()(())", "()()()"] },
	};

	[Theory]
	[MemberData(nameof(Cases))]
	public void GeneratesBalancedSequencesInOrder(int pairCount, string[] expected)
	{
		var actual = Task01.GenerateParentheses(pairCount);

		Assert.Equal(expected, actual);
	}

	[Fact]
	public void SupportsMaximumPairCount()
	{
		var actual = Task01.GenerateParentheses(10);

		Assert.Equal(16_796, actual.Length);
		Assert.Equal(actual.Order(StringComparer.Ordinal), actual);
		Assert.Equal(actual.Length, actual.Distinct().Count());

		foreach (var seq in actual)
		{
			var balance = 0;

			foreach (var ch in seq)
			{
				balance += ch == '(' ? 1 : -1;
				Assert.True(balance >= 0);
			}

			Assert.Equal(20, seq.Length);
			Assert.Equal(0, balance);
		}
	}
}
