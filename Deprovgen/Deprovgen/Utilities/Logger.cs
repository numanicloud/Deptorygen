using System;
using System.IO;
using System.Threading.Tasks;

namespace Deprovgen.Utilities
{
	class Logger
	{
		private const string LogFilePath = "D:\\Naohiro\\Documents\\#Documents\\開発\\deprovgen.txt";

		public static async Task WriteLine(string message)
		{
			using var file = File.Open(LogFilePath, FileMode.Append);
			using var writer = new StreamWriter(file);
			await writer.WriteLineAsync(DateTime.Now.ToString("O"));
			await writer.WriteLineAsync(message);
		}
	}
}
