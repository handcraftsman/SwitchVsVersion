using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SwitchVsVersion
{
	internal static class Disk
	{
		private static bool AnythingNeedsReplacing(string allText, IEnumerable<ProjectSolutionMapping> mappings)
		{
			return mappings.Any(m => allText.Contains(m.OldText));
		}

		public static IEnumerable<string> GetFiles(string path, string extension)
		{
			foreach (var filename in Directory.GetFiles(path, extension))
			{
				yield return filename;
			}

			var directories = Directory.GetDirectories(path);
			foreach (var file in directories.SelectMany(eachDirectoryPath => GetFiles(eachDirectoryPath, extension)))
			{
				yield return file;
			}
		}

		public static void ModifyFile(string pathToFile, IList<ProjectSolutionMapping> mappings)
		{
			string allText;
			Encoding originalEncoding;
			using (var fileStream = File.OpenText(pathToFile))
			{
				originalEncoding = fileStream.CurrentEncoding;
				allText = fileStream.ReadToEnd();
			}

			if (string.IsNullOrEmpty(allText))
			{
				return;
			}

			if (!AnythingNeedsReplacing(allText, mappings))
			{
				Console.WriteLine(@"nothing to modify in " + pathToFile);
				return;
			}

			Console.WriteLine(@"modifying " + pathToFile);

			allText = mappings.Aggregate(
				allText,
				(current, eachMapping) => current.Replace(eachMapping.OldText, eachMapping.NewText));

			File.WriteAllText(pathToFile, allText, originalEncoding);
		}
	}
}