namespace CA02.Kt02;

public static class Task06
{
	/*
	Минимальное количество переговорных комнат

	Дан список встреч. Каждая встреча задана полуоткрытым временным интервалом
	[start, end), где момент end уже свободен для новой встречи.
	Определите минимальное количество переговорных комнат, необходимое для
	проведения всех встреч без переноса времени.

	Входные данные:
	В первой строке задано n. В следующих n строках заданы пары start и end.

	Выходные данные:
	Одно целое число - минимальное количество переговорных комнат.

	Ограничения:
	0 <= n <= 200 000
	0 <= start < end <= 10^9
	*/
	public static int MinimumMeetingRooms(int[][] meetings)
	{
		// число комнат равно максимальному числу одновременных встреч.
		// сортируем начала и окончания отдельно и проходим их через two pointers.
		// при равном времени сначала освобождаем комнату, потому что end уже не входит во встречу
		var starts = meetings.Select(meeting => meeting[0]).Order().ToArray();
		var ends = meetings.Select(meeting => meeting[1]).Order().ToArray();
		var endIndex = 0;
		var currentRooms = 0;
		var requiredRooms = 0;

		foreach (var start in starts)
		{
			while (endIndex < ends.Length && ends[endIndex] <= start)
			{
				currentRooms--;
				endIndex++;
			}

			currentRooms++;
			requiredRooms = Math.Max(requiredRooms, currentRooms);
		}

		return requiredRooms;
	}
}

public sealed class Task06Tests
{
	public static TheoryData<int[][], int> Cases => new()
	{
		{ [[0, 30], [5, 10], [15, 20]], 2 },
		{ [[7, 10], [2, 4]], 1 },
		{ [], 0 },
		{ [[1, 2]], 1 },
		{ [[1, 2], [2, 3], [3, 4]], 1 },
		{ [[1, 5], [1, 5], [1, 5]], 3 },
		{ [[0, 10], [1, 9], [2, 8]], 3 },
		{ [[0, 2], [0, 2], [2, 4], [2, 4]], 2 },
		{ [[0, 1_000_000_000], [1, 2], [5, 6]], 2 },
	};

	[Theory]
	[MemberData(nameof(Cases))]
	public void FindsMinimumMeetingRooms(int[][] meetings, int expected)
	{
		var actual = Task06.MinimumMeetingRooms(meetings);

		Assert.Equal(expected, actual);
	}

	[Fact]
	public void DoesNotChangeInputOrder()
	{
		int[][] meetings = [[7, 10], [2, 4], [0, 8]];
		var original = meetings.Select(meeting => meeting.ToArray()).ToArray();

		Task06.MinimumMeetingRooms(meetings);

		for (var index = 0; index < meetings.Length; index++)
		{
			Assert.Equal(original[index], meetings[index]);
		}
	}
}
