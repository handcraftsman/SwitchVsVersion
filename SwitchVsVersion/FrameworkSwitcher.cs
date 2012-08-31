using System;
using System.IO;

namespace SwitchVsVersion
{
	internal class FrameworkSwitcher : ISwitcher
	{
		public bool IsMatch(string version)
		{
			return FrameworkMapping.Getfor(version) != null;
		}

		public void Switch(string path, string version)
		{
			ModifyAllProjectsUnderThisFolderTo(path, FrameworkMapping.Getfor(version).FileValue);
		}

		private static void ModifyAllProjectsUnderThisFolderTo(string path, string frameworkVersion)
		{
			var projectFilePaths = Disk.GetFiles(path, @"*.csproj");
			foreach (var eachProjectFilePath in projectFilePaths)
			{
				ModifyProjectFile(eachProjectFilePath, frameworkVersion);
			}
		}

		private static void ModifyProjectFile(string projectFilePath, string frameworkVersion)
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