using System;

namespace AssemblyCSharp
{
	public enum Buildables
	{
		Coinage,
		Factory,
		Granary,
		Mine,
		Testunit,
		Newcity
	}
	public enum EnumCategories
	{
		CitizenCount,
		TurnCount
	}
	public enum CitizenCount
	{
		CitizenCount = 0,
		Low = 1,
		Med = 5,
		High = 9
	}
	public enum TurnCount
	{
		TurnCount = 0,
		Early = 30,
		Mid = 60,
		Late = 90
	}
}
