using System;

namespace RimWorld
{
	[DefOf]
	public static class IncidentTargetTypeDefOf
	{
		public static IncidentTargetTypeDef World;

		public static IncidentTargetTypeDef Caravan;

		public static IncidentTargetTypeDef Map_RaidBeacon;

		public static IncidentTargetTypeDef Map_PlayerHome;

		public static IncidentTargetTypeDef Map_Misc;

		static IncidentTargetTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(IncidentTargetTypeDefOf));
		}
	}
}
