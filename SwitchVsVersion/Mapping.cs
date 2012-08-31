namespace SwitchVsVersion
{
	public class Mapping
	{
		private readonly string _newText;
		private readonly string _oldText;

		public Mapping(string oldText, string newText)
		{
			_oldText = oldText;
			_newText = newText;
		}

		public string NewText
		{
			get { return _newText; }
		}
		public string OldText
		{
			get { return _oldText; }
		}

		public Mapping InReverse()
		{
			return new Mapping(_newText, _oldText);
		}
	}
}