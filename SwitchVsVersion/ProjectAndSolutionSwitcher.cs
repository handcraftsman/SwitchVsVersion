using System.Collections.Generic;
using System.Linq;

namespace SwitchVsVersion
{
	public class ProjectAndSolutionSwitcher : ISwitcher
	{
		public bool IsMatch(string version)
		{
			return ProjectSolutionMapping.GetAll()
				.Any(x => x.TargetVersion == version);
		}

		public void Switch(string path, string version)
		{
			var mappings = GetMappingPathsToVersion(version);
			SwitchProjectsAndSolutions(path, mappings);
		}

		private static IList<ProjectSolutionMapping> GetMappingPathsToVersion(string version)
		{
			var allMappings = ProjectSolutionMapping.GetAll().ToList();
			var usedVersions = new HashSet<string>();

			var sourceVersions = new List<string>
				{
					version
				};

			var mappings = new List<ProjectSolutionMapping>();
			while (sourceVersions.Any())
			{
				var nextSourceVersions = new List<string>();
				foreach (var sourceVersion in sourceVersions.Where(x => !usedVersions.Contains(x)))
				{
					var versionMappings = allMappings
						.Where(x => x.TargetVersion == sourceVersion)
						.Where(x => !usedVersions.Contains(x.SourceVersion))
						.ToList();
					mappings.AddRange(versionMappings);
					nextSourceVersions.AddRange(versionMappings.Select(x => x.SourceVersion));
					usedVersions.Add(sourceVersion);
				}
				sourceVersions = nextSourceVersions.Distinct().ToList();
			}

			mappings.Reverse();
			return mappings;
		}

		private static void SwitchProjectsAndSolutions(string path, IList<ProjectSolutionMapping> mappings)
		{
			foreach (var wildcard in new[]
				{
					@"*.csproj",
					@"*.sln",
					@"*.vbproj"
				})
			{
				foreach (var eachFilename in Disk.GetFiles(path, wildcard))
				{
					Disk.ModifyFile(eachFilename, mappings);
				}
			}
		}
	}
}