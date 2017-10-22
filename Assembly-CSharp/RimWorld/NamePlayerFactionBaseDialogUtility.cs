using RimWorld.Planet;

namespace RimWorld
{
	public static class NamePlayerFactionBaseDialogUtility
	{
		public static bool IsValidName(string s)
		{
			return (byte)((s.Length != 0) ? 1 : 0) != 0;
		}

		public static void Named(FactionBase factionBase, string s)
		{
			factionBase.Name = s;
			factionBase.namedByPlayer = true;
		}
	}
}
