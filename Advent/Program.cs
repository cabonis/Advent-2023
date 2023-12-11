using System;
using System.IO;
using System.Reflection;

namespace Advent
{
	internal class Program
	{
		static void Main(string[] args)
		{
			//string dayName = $"Day{DateTime.Now.Date.Day.ToString("00")}";
			string dayName = "Day10";

			string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), dayName, @"input.txt");

			try
			{
				string[] lines = File.ReadAllLines(path);
				Type type = Type.GetType($"Advent.{dayName}.{dayName}");
				IDay day = (IDay)Activator.CreateInstance(type);
				string result = day.DoWork(lines);
				Console.WriteLine(result);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}

			Console.ReadLine();
		}
	}
}
