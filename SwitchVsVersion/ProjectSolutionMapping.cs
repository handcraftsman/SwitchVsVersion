namespace SwitchVsVersion
{
	public class ProjectSolutionMapping : NamedConstant<ProjectSolutionMapping>
	{
		public static readonly ProjectSolutionMapping FormatVersion20082010 = new ProjectSolutionMapping(
			"2008", "2010",
			@"Microsoft Visual Studio Solution File, Format Version 10.00",
			@"Microsoft Visual Studio Solution File, Format Version 11.00");
		public static readonly ProjectSolutionMapping FormatVersion20102012 = new ProjectSolutionMapping(
			"2010", "2012",
			@"Microsoft Visual Studio Solution File, Format Version 11.00",
			@"Microsoft Visual Studio Solution File, Format Version 12.00");
		public static readonly ProjectSolutionMapping ToolsVersion20082010 = new ProjectSolutionMapping(
			"2008", "2010",
			@"ToolsVersion=""3.5""",
			@"ToolsVersion=""4.0""");
		public static readonly ProjectSolutionMapping VisualStudio20082010 = new ProjectSolutionMapping(
			"2008", "2010",
			@"# Visual Studio 2008",
			@"# Visual Studio 2010");
		public static readonly ProjectSolutionMapping VisualStudio20102012 = new ProjectSolutionMapping(
			"2010", "2012",
			@"# Visual Studio 2010",
			@"# Visual Studio 2012");
		public static readonly ProjectSolutionMapping WebApplicationTargets20082010 = new ProjectSolutionMapping(
			"2008", "2010",
			@"<Import Project=""$(MSBuildExtensionsPath)\Microsoft\VisualStudio\v9.0\WebApplications\Microsoft.WebApplication.targets"" />",
			@"<Import Project=""$(MSBuildExtensionsPath32)\Microsoft\VisualStudio\v10.0\WebApplications\Microsoft.WebApplication.targets""/>");
		public static readonly ProjectSolutionMapping WebApplicationTargets20102012 = new ProjectSolutionMapping(
			"2010", "2012",
			@"<Import Project=""$(MSBuildExtensionsPath32)\Microsoft\VisualStudio\v10.0\WebApplications\Microsoft.WebApplication.targets""/>",
			@"<Import Project=""$(MSBuildExtensionsPath32)\Microsoft\VisualStudio\v11.0\WebApplications\Microsoft.WebApplication.targets""/>");

		private ProjectSolutionMapping(string sourceVersion, string targetVersion, string oldText, string newText, bool addReverse = true)
		{
			SourceVersion = sourceVersion;
			TargetVersion = targetVersion;
			OldText = oldText;
			NewText = newText;
			Add(sourceVersion + "->" + targetVersion + "=" + oldText, this);
			if (addReverse)
			{
				var _ = new ProjectSolutionMapping(targetVersion, sourceVersion, newText, oldText, false);
			}
		}

		public string NewText { get; private set; }
		public string OldText { get; private set; }
		public string SourceVersion { get; private set; }
		public string TargetVersion { get; private set; }
	}
}