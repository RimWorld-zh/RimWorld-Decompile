using Verse;

namespace RimWorld
{
	public static class NamePlayerFactionDialogUtility
	{
		public static bool IsValidName(string s)
		{
			if (s.Length == 0)
			{
				return false;
			}
			if (!GenText.IsValidFilename(s))
			{
				return false;
			}
			return true;
		}

		public static void Named(string s)
		{
			Faction.OfPlayer.Name = s;
		}
	}
}
