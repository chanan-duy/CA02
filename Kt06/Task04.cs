namespace CA02.Kt06;

public static class Task04
{
	/*
	Разворот полных блоков списка

	Дана последовательность целых чисел. Постройте из неё односвязный список и разбейте
	его от головы на последовательные блоки по k узлов. В каждом полном блоке измените
	порядок узлов на обратный. Последний блок, если в нём меньше k узлов, оставьте без
	изменений. Нельзя создавать новые узлы, менять их значения или переносить значения
	во вспомогательную коллекцию. Дополнительная память должна занимать O(1).

	Входные данные:
	В первой строке заданы n и k. Во второй строке заданы n целых чисел a[i] - значения
	узлов в исходном порядке.

	Выходные данные:
	Выведите значения узлов получившегося списка от головы до конца.

	Ограничения:
	1 <= n, k <= 200 000, -10^9 <= a[i] <= 10^9. Если k > n, список остаётся без
	изменений.
	*/
	public static Task01.Node? ReverseGroups(Task01.Node? head, int blockLn)
	{
		// сначала ищем конец полного блока, чтобы короткий остаток не пришлось разворачивать обратно.
		// при развороте последняя ссылка сразу ведёт к следующему блоку, поэтому список не теряет продолжение.
		// хвост предыдущего блока соединяем с новой головой без создания вспомогательного узла
		var newHead = head;
		Task01.Node? prevTail = null;
		var current = head;

		while (current != null)
		{
			var blockEnd = current;

			for (var index = 1; index < blockLn && blockEnd != null; index++)
			{
				blockEnd = blockEnd.Next;
			}

			if (blockEnd == null)
			{
				break;
			}

			var nextBlock = blockEnd.Next;
			var prev = nextBlock;
			var node = current;

			while (node != nextBlock)
			{
				var next = node!.Next;
				node.Next = prev;
				prev = node;
				node = next;
			}

			if (prevTail == null)
			{
				newHead = blockEnd;
			}
			else
			{
				prevTail.Next = blockEnd;
			}

			prevTail = current;
			current = nextBlock;
		}

		return newHead;
	}
}

public sealed class Task04Tests
{
	public static TheoryData<int[], int, int[]> Cases => new()
	{
		{ [1, 2, 3, 4, 5], 2, [2, 1, 4, 3, 5] },
		{ [1, 2, 3, 4, 5], 3, [3, 2, 1, 4, 5] },
		{ [1, 2, 3], 1, [1, 2, 3] },
		{ [1, 2, 3], 4, [1, 2, 3] },
		{ [1, 2, 3], 3, [3, 2, 1] },
		{ [-1, -1, 2, 2], 2, [-1, -1, 2, 2] },
	};

	[Theory]
	[MemberData(nameof(Cases))]
	public void ReversesOnlyCompleteGroups(int[] nums, int blockLn, int[] expected)
	{
		var head = Task01.CreateList(nums);
		List<Task01.Node> original = [];

		for (var node = head; node != null; node = node.Next)
		{
			original.Add(node);
		}

		var actual = Task04.ReverseGroups(head, blockLn);

		for (var index = 0; index < expected.Length; index++)
		{
			var blockStart = index / blockLn * blockLn;
			var originalIndex = blockStart + blockLn <= nums.Length
				? blockStart + blockLn - 1 - index % blockLn
				: index;
			Assert.Same(original[originalIndex], actual);
			Assert.Equal(expected[index], actual!.Value);
			actual = actual.Next;
		}

		Assert.Null(actual);
	}
}
