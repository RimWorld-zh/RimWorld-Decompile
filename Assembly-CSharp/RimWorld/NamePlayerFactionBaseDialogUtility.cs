using RimWorld.Planet;

namespace RimWorld
{
	public static class NamePlayerFactionBaseDialogUtility
	{
		public static bool IsValidName(string s)
		{
			if (s.Length == 0)
			{
				return false;
			}
			return true;
		}

		public static void Named(FactionBase factionBase, string s)
		{
			factionBase.Name = s;
			factionBase.namedByPlayer = true;
		}
	}
}
