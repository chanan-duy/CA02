namespace CA02.Kt06;

public static class Task05
{
	/*
	Совместимость назначений двух групп

	Имеется n человек. Каждому требуется назначить одну из двух групп: 0 или 1.
	Последовательно поступают утверждения u v r. При r = 0 люди u и v должны находиться
	в одной группе, при r = 1 - в разных группах. После каждого утверждения определите,
	существует ли назначение, удовлетворяющее всем полученным утверждениям.

	Входные данные:
	В первой строке заданы n и q. В следующих q строках заданы u, v и r.

	Выходные данные:
	После каждого утверждения выведите YES, если все полученные утверждения совместимы,
	иначе NO. После первого ответа NO все последующие ответы также равны NO.

	Ограничения:
	1 <= n, q <= 200 000, 1 <= u, v <= n, r равен 0 или 1. Утверждения могут
	повторяться; при u = v и r = 1 возникает противоречие.
	*/
	public static bool[] ConsistentGroups(int personCount, int[][] statements)
	{
		// xor до родителя хранит, совпадают ли группы, а сжатие пути пересчитывает его до корня.
		// внутри одной компоненты требуемое отношение уже задано накопленными xor.
		// разные компоненты объединяем с таким xor корней, чтобы новое утверждение выполнялось
		var parents = Enumerable.Range(0, personCount + 1).ToArray();
		var sizes = Enumerable.Repeat(1, personCount + 1).ToArray();
		var groupXors = new int[personCount + 1];
		var answers = new bool[statements.Length];
		var consistent = true;

		int Find(int person)
		{
			if (parents[person] == person)
			{
				return person;
			}

			var parent = parents[person];
			parents[person] = Find(parent);
			groupXors[person] ^= groupXors[parent];

			return parents[person];
		}

		for (var index = 0; index < statements.Length; index++)
		{
			var first = statements[index][0];
			var second = statements[index][1];
			var relation = statements[index][2];
			var firstRoot = Find(first);
			var secondRoot = Find(second);
			var rootXor = groupXors[first] ^ groupXors[second] ^ relation;

			if (firstRoot == secondRoot)
			{
				consistent &= rootXor == 0;
			}
			else
			{
				if (sizes[firstRoot] < sizes[secondRoot])
				{
					(firstRoot, secondRoot) = (secondRoot, firstRoot);
				}

				parents[secondRoot] = firstRoot;
				groupXors[secondRoot] = rootXor;
				sizes[firstRoot] += sizes[secondRoot];
			}

			answers[index] = consistent;
		}

		return answers;
	}
}

public sealed class Task05Tests
{
	public static TheoryData<int, int[][], bool[]> Cases => new()
	{
		{ 1, [[1, 1, 0], [1, 1, 1], [1, 1, 0]], [true, false, false] },
		{ 3, [[1, 2, 1], [2, 3, 1], [1, 3, 0], [1, 3, 1]], [true, true, true, false] },
		{ 4, [[1, 2, 0], [3, 4, 1], [2, 3, 1], [1, 4, 0]], [true, true, true, true] },
		{ 2, [[1, 2, 1], [2, 1, 1], [1, 2, 0]], [true, true, false] },
	};

	[Theory]
	[MemberData(nameof(Cases))]
	public void DetectsFirstContradiction(int personCount, int[][] statements, bool[] expected)
	{
		var actual = Task05.ConsistentGroups(personCount, statements);

		Assert.Equal(expected, actual);
	}
}
