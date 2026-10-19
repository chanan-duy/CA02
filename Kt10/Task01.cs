namespace CA02.Kt10;

public static class Task01
{
	/*
	Максимальное число совместимых заявок

	Для одного зала поступило n заявок. Заявка i занимает полуоткрытый промежуток
	времени [l[i], r[i]). Выберите максимальное количество заявок, которые можно
	провести без пересечений. Заявка, заканчивающаяся в момент t, совместима с заявкой,
	начинающейся в момент t.

	Входные данные:
	В первой строке задано n. В следующих n строках заданы l[i] и r[i].

	Выходные данные:
	Выведите максимальное количество заявок.

	Ограничения:
	1 <= n <= 200 000, 0 <= l[i] < r[i] <= 10^9.
	*/
	public static int MaximumCompatibleMeetings(int[][] meetings)
	{
		// заявка с самым ранним окончанием оставляет не меньше времени для остальных, чем любая другая первая заявка.
		// после выбора рассматриваем только заявки, начинающиеся не раньше её окончания.
		// равенство границ допустимо, потому что правая граница не входит в занятый промежуток
		var sortedMeetings = meetings.OrderBy(meeting => meeting[1]).ToArray();
		var lastEnd = 0;
		var meetingCount = 0;

		foreach (var meeting in sortedMeetings)
		{
			if (meeting[0] >= lastEnd)
			{
				meetingCount++;
				lastEnd = meeting[1];
			}
		}

		return meetingCount;
	}
}

public sealed class Task01Tests
{
	public static TheoryData<int[][], int> Cases => new()
	{
		{ [[0, 10], [1, 2], [2, 3], [3, 4]], 3 },
		{ [[1, 2]], 1 },
		{ [[1, 5], [1, 5], [1, 5]], 1 },
		{ [[4, 5], [0, 1], [1, 4]], 3 },
		{ [[0, 1_000_000_000], [0, 2], [2, 1_000_000_000]], 2 },
	};

	[Theory]
	[MemberData(nameof(Cases))]
	public void SelectsMaximumNumberOfCompatibleMeetings(int[][] meetings, int expected)
	{
		var actual = Task01.MaximumCompatibleMeetings(meetings);

		Assert.Equal(expected, actual);
	}
}
