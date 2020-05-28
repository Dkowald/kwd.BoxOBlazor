namespace kwd.BoxOBlazor.Demo.Unit
{
	/// <summary>
	/// A number between 0 and 100.
	/// With specified precision.
	/// </summary>
	public class Percent
	{
		private readonly int _precision;
		
		public Percent(float value, int precision = 2)
		{
			Value = value;
			_precision = precision;
		}

		public readonly float Value;
		
		public override string ToString()
		{
			return Value.ToString($"f{_precision}");
		}
	}
}