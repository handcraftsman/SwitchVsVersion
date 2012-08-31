namespace SwitchVsVersion
{
	public class FrameworkMapping : NamedConstant<FrameworkMapping>
	{
		public static readonly FrameworkMapping Framework35 = new FrameworkMapping("3.5Framework", "3.5");
		public static readonly FrameworkMapping Framework40 = new FrameworkMapping("4.0Framework", "4.0");
		public static readonly FrameworkMapping Framework45 = new FrameworkMapping("4.5Framework", "4.5");

		private FrameworkMapping(string commandLineArg, string fileValue)
		{
			CommandLineArg = commandLineArg;
			FileValue = fileValue;
			Add(commandLineArg.ToLower(), this);
		}

		public string CommandLineArg { get; private set; }
		public string FileValue { get; private set; }

		public static FrameworkMapping Getfor(string commandLineArg)
		{
			return Framework35.GetFor(commandLineArg);
		}
	}
}