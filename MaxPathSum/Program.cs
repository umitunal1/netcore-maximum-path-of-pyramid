using System;

namespace MaxPathSum
{
    class Program
    {
        public static System.Collections.Generic.Dictionary<int, bool> PrimeCache = new System.Collections.Generic.Dictionary<int, bool>();
        static void Main(string[] args)
        {
			string path  = System.IO.Directory.GetCurrentDirectory().ToString()+"\\"+ "TestFile\\pyramid.txt";			
			FindMaximumSumOfPyramid(path);			
        }		
		public static void FindMaximumSumOfPyramid(string path)
		{
			string input = GetInputFromFile(path);
			string[] rows = input.Split('\n');
			int[,] tableHolder = PyramidToTable(rows);
			int[,] result = CheckNodes(rows, tableHolder);
			Console.WriteLine(String.Format("Result : {0}", result[0, 0].ToString()));
		}

		private static string GetInputFromFile(string path)
		{
			string input = System.IO.File.ReadAllText(path);
			return input;
		}

		private static int[,] CheckNodes(string[] rows, int[,] tableHolder)
		{
			int[,] tableHolderWithoutPrimeNumbers = ExcludePrimeNumbers(rows, tableHolder);

			for (int i = rows.Length - 2; i >= 0; i--)
			{
				for (int j = 0; j < rows.Length; j++)
				{
					int x = tableHolderWithoutPrimeNumbers[i, j];
					int y = tableHolderWithoutPrimeNumbers[i + 1, j];
					int z = tableHolderWithoutPrimeNumbers[i + 1, j + 1];

					//only sum through the non - prime node
					if ((!IsPrime(x) && !IsPrime(y)) || (!IsPrime(x) && !IsPrime(z)))
						tableHolder[i, j] = x + Math.Max(y, z);
				}
			}
			return tableHolder;
		}

		private static int[,] ExcludePrimeNumbers(string[] rows, int[,] tableHolder)
		{
			for (int i = 0; i < rows.Length; i++)
			{
				for (int j = 0; j < rows.Length; j++)
				{
					if (IsPrime(tableHolder[i, j]))
						tableHolder[i, j] = 0;
				}
			}
			return tableHolder;
		}

		private static int[,] PyramidToTable(string[] rows)
		{
			int[,] tableHolder = new int[rows.Length, rows.Length + 1];

			for (int row = 0; row < rows.Length; row++)
			{
				var eachCharactersInRow = rows[row].Trim().Split(' ');

				for (int column = 0; column < eachCharactersInRow.Length; column++)
				{
					int number;
					int.TryParse(eachCharactersInRow[column], out number);
					tableHolder[row, column] = number;
				}
			}
			return tableHolder;
		}

		public static bool IsPrime(int number)
		{
			if (PrimeCache.ContainsKey(number))
			{
				bool value;
				PrimeCache.TryGetValue(number, out value);
				return value;
			}

			// checking bitwise whether the number is a prime number
			if ((number & 1) == 0)
			{
				if (number == 2)
				{
					if (!PrimeCache.ContainsKey(number))
						PrimeCache.Add(number, true);
					return true;
				}
				if (!PrimeCache.ContainsKey(number))
					PrimeCache.Add(number, false);
				return false;
			}

			// checking whether the number is a prime number
			for (int i = 3; (i * i) <= number; i += 2)
			{
				if ((number % i) == 0)
				{
					if (!PrimeCache.ContainsKey(number))
						PrimeCache.Add(number, false);
					return false;
				}
			}

			bool isNotEqualToOne = number != 1;
			if (!PrimeCache.ContainsKey(number))
				PrimeCache.Add(number, isNotEqualToOne);
			return isNotEqualToOne;
		}
	}
}
