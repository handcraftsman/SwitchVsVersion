namespace SwitchVsVersion
{
	public class TargetPlatformMapping : NamedConstant<TargetPlatformMapping>
	{
		public static readonly TargetPlatformMapping AnyCPU = new TargetPlatformMapping("AnyCPU", "AnyCPU");
		public static readonly TargetPlatformMapping X86 = new TargetPlatformMapping("x86", "x86");

		private TargetPlatformMapping(string commandLineArg, string fileValue)
		{
			CommandLineArg = commandLineArg;
			FileValue = fileValue;
			Add(commandLineArg.ToLower(), this);
		}

		public string CommandLineArg { get; set; }
		public string FileValue { get; set; }

		public static TargetPlatformMapping Getfor(string commandLineArg)
		{
			return AnyCPU.GetFor(commandLineArg);
		}
	}
}