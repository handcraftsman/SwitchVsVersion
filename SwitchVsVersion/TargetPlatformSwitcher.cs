using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace SwitchVsVersion
{
	internal class TargetPlatformSwitcher : ISwitcher
	{
		public bool IsMatch(string version)
		{
			return TargetPlatformMapping.Getfor(version) != null;
		}

		public void Switch(string path, string version)
		{
			ModifyAllProjectsUnderThisFolderTo(path, TargetPlatformMapping.Getfor(version).FileValue);
		}

		private static IEnumerable<string> GetProjectFiles(string path)
		{
			if (StringComparer.Ordinal.Compare(Path.GetExtension(path), @".csproj") == 0)
			{
				return new[] { path };
			}

			return Disk.GetFiles(path, @"*.csproj");
		}

		private static void ModifyAllProjectsUnderThisFolderTo(string path, string targetPlatform)
		{
			var projectFilePaths = GetProjectFiles(path);

			foreach (var eachProjectFilePath in projectFilePaths)
			{
				try
				{
					ModifyProjectFile(eachProjectFilePath, targetPlatform);
				}
				catch (XmlException ex)
				{
					Console.WriteLine(@"SKIPPED '{0}' because '{1}'.", eachProjectFilePath, ex.Message);
				}
			}
		}

		private static void ModifyProjectFile(string projectFilePath, string targetPlatform)
		{
			Console.Write(@"Converting to {0} - {1}... ", targetPlatform, projectFilePath);

			var e = XElement.Load(projectFilePath);

			XNamespace ns = @"http://schemas.microsoft.com/developer/msbuild/2003";

			var propertyGroupElements = e.Elements(ns + @"PropertyGroup");

			var modified = false;

			foreach (var eachElement in propertyGroupElements)
			{
				var conditionAttribute = eachElement.Attribute(@"Condition");
				if (conditionAttribute == null)
				{
					continue;
				}

				if (!conditionAttribute.Value.Contains(@"'$(Configuration)|$(Platform)'"))
				{
					continue;
				}

				var platformTargetElement = eachElement.Element(ns + @"PlatformTarget");
				if (platformTargetElement == null)
				{
					modified = true;
					platformTargetElement = new XElement(ns + @"PlatformTarget", targetPlatform);
					eachElement.Add(platformTargetElement);
				}
				else
				{
					if (platformTargetElement.Value != targetPlatform)
					{
						platformTargetElement.Value = targetPlatform;
						modified = true;
					}
				}
			}

			if (modified)
			{
				File.WriteAllText(projectFilePath, e.ToString());
				Console.WriteLine(@"DONE");
			}
			else
			{
				Console.WriteLine(@"NOTHING TO CHANGE, all configuration already set to '{0}'", targetPlatform);
			}
		}
	}
}