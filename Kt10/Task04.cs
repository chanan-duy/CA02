namespace CA02.Kt10;

public static class Task04
{
	/*
	Максимальная прибыль заказов с крайними сроками

	Имеется n заказов. Выполнение любого заказа занимает ровно один целый временной
	слот. Для заказа i заданы крайний срок d[i] и прибыль p[i]. Заказ приносит прибыль,
	если выполнен в одном из слотов от 1 до d[i] включительно. В одном слоте можно
	выполнить не более одного заказа, а заказы разрешается пропускать. Найдите
	максимальную суммарную прибыль.

	Входные данные:
	В первой строке задано n. В следующих n строках заданы d[i] и p[i].

	Выходные данные:
	Выведите максимальную суммарную прибыль.

	Ограничения:
	1 <= n <= 500 000, 1 <= d[i] <= 10^9, 1 <= p[i] <= 10^9. Ответ помещается в
	64-битный знаковый целый тип.
	*/
	public static long MaximumJobProfit(int[][] jobs)
	{
		// после сортировки по сроку все уже рассмотренные заказы должны помещаться в первые deadline слотов.
		// при избытке удаляем наименее прибыльный заказ, сохраняя лучшую сумму среди допустимых наборов.
		// priority queue позволяет заменить худший заказ, не перебирая занятые слоты
		var sortedJobs = jobs.OrderBy(job => job[0]).ToArray();
		PriorityQueue<int, int> selectedProfits = new();
		long totalProfit = 0;

		foreach (var job in sortedJobs)
		{
			selectedProfits.Enqueue(job[1], job[1]);
			totalProfit += job[1];

			if (selectedProfits.Count > job[0])
			{
				totalProfit -= selectedProfits.Dequeue();
			}
		}

		return totalProfit;
	}
}

public sealed class Task04Tests
{
	public static TheoryData<int[][], long> Cases => new()
	{
		{ [[1, 10], [1, 20], [2, 15]], 35 },
		{ [[1, 5]], 5 },
		{ [[1, 1], [1, 3], [1, 2]], 3 },
		{ [[1_000_000_000, 4], [1_000_000_000, 7]], 11 },
		{ [[2, 100], [1, 20], [2, 30], [1, 40]], 140 },
		{ [[3, 1_000_000_000], [3, 1_000_000_000], [3, 1_000_000_000]], 3_000_000_000 },
	};

	[Theory]
	[MemberData(nameof(Cases))]
	public void FindsMaximumProfitBeforeDeadlines(int[][] jobs, long expected)
	{
		var actual = Task04.MaximumJobProfit(jobs);

		Assert.Equal(expected, actual);
	}
}
