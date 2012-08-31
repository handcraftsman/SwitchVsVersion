using System;
using System.IO;

namespace SwitchVsVersion
{
	internal static class FrameworkSwitcher
	{
		public static void ModifyAllProjectsUnderThisFolderTo(string path, string frameworkVersion)
		{
			var projectFilePaths = Disk.GetFiles(path, @"*.csproj");
			foreach (var eachProjectFilePath in projectFilePaths)
			{
				modifyProjectFile(eachProjectFilePath, frameworkVersion);
			}
		}

		private static void modifyProjectFile(string projectFilePath, string frameworkVersion)
		{
			Console.Write(@"Converting to {0} - {1}... ", frameworkVersion, projectFilePath);

			var allText = File.ReadAllText(projectFilePath);
			var xmlFragement = string.Format(
				@"<TargetFrameworkVersion>v{0}</TargetFrameworkVersion>", frameworkVersion);

			if (allText.Contains(xmlFragement))
			{
				Console.WriteLine(@"Already converted - did nothing.");
				return;
			}

			allText = allText.Replace(
				@"</PropertyGroup>",
				xmlFragement + @"</PropertyGroup>");

			File.WriteAllText(projectFilePath, allText);

			Console.WriteLine(@"Done");
		}
	}
}