namespace CA02.Kt06;

public static class Task01
{
	/*
	Разворот односвязного списка

	Дана последовательность целых чисел. Постройте из неё односвязный список, сохранив
	исходный порядок, затем измените связи между узлами так, чтобы порядок элементов
	стал обратным. Нельзя создавать новые узлы, менять значения узлов или переносить
	элементы во вспомогательный массив либо коллекцию. Разрешено использовать только
	константное число дополнительных ссылок.

	Входные данные:
	В первой строке задано n. Во второй строке заданы n целых чисел a[i] - значения
	узлов в исходном порядке.

	Выходные данные:
	Выведите значения узлов после изменения списка в порядке от новой головы до конца.

	Ограничения:
	1 <= n <= 200 000, -10^9 <= a[i] <= 10^9.
	*/
	public static Node? ReverseList(Node? head)
	{
		// после смены Next исходное продолжение теряется, поэтому сохраняем его до изменения связи.
		// prev хранит уже развёрнутую часть, а current указывает на ещё не обработанную.
		// меняются только ссылки существующих узлов, поэтому дополнительная память не зависит от длины списка
		Node? prev = null;
		var current = head;

		while (current != null)
		{
			var next = current.Next;
			current.Next = prev;
			prev = current;
			current = next;
		}

		return prev;
	}

	public static Node? CreateList(int[] nums)
	{
		Node? head = null;
		Node? tail = null;

		foreach (var number in nums)
		{
			var node = new Node(number);

			if (tail == null)
			{
				head = node;
			}
			else
			{
				tail.Next = node;
			}

			tail = node;
		}

		return head;
	}

	public sealed class Node(int value)
	{
		public int Value { get; } = value;
		public Node? Next { get; set; }
	}
}

public sealed class Task01Tests
{
	public static TheoryData<int[]> Cases => new()
	{
		{ [1] },
		{ [1, 2, 3, 4] },
		{ [2, 2, 2] },
		{ [-1_000_000_000, 0, 1_000_000_000] },
	};

	[Theory]
	[MemberData(nameof(Cases))]
	public void ReversesLinksWithoutReplacingNodes(int[] nums)
	{
		var head = Task01.CreateList(nums);
		List<Task01.Node> original = [];

		for (var node = head; node != null; node = node.Next)
		{
			original.Add(node);
		}

		var actual = Task01.ReverseList(head);

		for (var index = nums.Length - 1; index >= 0; index--)
		{
			Assert.Same(original[index], actual);
			Assert.Equal(nums[index], actual!.Value);
			actual = actual.Next;
		}

		Assert.Null(actual);
	}
}
