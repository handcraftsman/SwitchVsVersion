namespace SwitchVsVersion
{
	internal interface ISwitcher
	{
		bool IsMatch(string version);
		void Switch(string path, string version);
	}
}