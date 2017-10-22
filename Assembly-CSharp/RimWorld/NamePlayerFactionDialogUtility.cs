using Verse;

namespace RimWorld
{
	public static class NamePlayerFactionDialogUtility
	{
		public static bool IsValidName(string s)
		{
			return (byte)((s.Length != 0) ? (GenText.IsValidFilename(s) ? 1 : 0) : 0) != 0;
		}

		public static void Named(string s)
		{
			Faction.OfPlayer.Name = s;
		}
	}
}
