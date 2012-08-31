using System;
using System.Linq;

namespace SwitchVsVersion
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var supportedVisualStudioVersions = ProjectSolutionMapping.GetAll()
				.Select(x => x.TargetVersion)
				.ToHashSet();
			var supportedFrameworkVersions = FrameworkMapping.GetAll()
				.Select(x => x.CommandLineArg)
				.OrderBy(x => x)
				.ToArray();
			var supportedTargetPlatformVersions = TargetPlatformMapping.GetAll()
				.Select(x => x.CommandLineArg)
				.OrderBy(x => x)
				.ToArray();

			try
			{
				if (args.Length == 0)
				{
					Console.WriteLine(@"usage: SwitchVsVersion [folder] [{0}|{1}|{2}]",
					                  String.Join("|", supportedVisualStudioVersions.OrderBy(x => x).ToArray()),
					                  String.Join("|", supportedFrameworkVersions),
					                  String.Join("|", supportedTargetPlatformVersions)
						);
					return;
				}

				string path;
				string version;

				if (args.Length == 1)
				{
					path = Environment.CurrentDirectory;
					version = args[0];
				}
				else
				{
					path = args[0];
					version = args[1];
				}

				var switcher = new ISwitcher[] { new FrameworkSwitcher(), new TargetPlatformSwitcher(), new ProjectAndSolutionSwitcher() }
					.FirstOrDefault(x => x.IsMatch(version));

				if (switcher != null)
				{
					switcher.Switch(path, version);
				}

				Console.WriteLine(@"Invalid version '{0}'.  Use {1}, {2}, {3}",
				                  version,
				                  String.Join(", ", supportedVisualStudioVersions.ToArray()),
				                  String.Join(", ", supportedFrameworkVersions.ToArray()),
				                  String.Join(", ", supportedTargetPlatformVersions.ToArray())
					);
			}
			catch (Exception e)
			{
				Console.WriteLine(@"Error: " + e.Message);
			}
			finally
			{
				Console.WriteLine(@"Finished");
			}
		}
	}
}