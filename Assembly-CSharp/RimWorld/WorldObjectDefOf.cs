using System;

namespace RimWorld
{
	[DefOf]
	public static class WorldObjectDefOf
	{
		public static WorldObjectDef Caravan;

		public static WorldObjectDef FactionBase;

		public static WorldObjectDef AbandonedBase;

		public static WorldObjectDef EscapeShip;

		public static WorldObjectDef Ambush;

		public static WorldObjectDef DestroyedFactionBase;

		public static WorldObjectDef AttackedNonPlayerCaravan;

		public static WorldObjectDef TravelingTransportPods;

		public static WorldObjectDef RoutePlannerWaypoint;

		public static WorldObjectDef Site;

		public static WorldObjectDef PeaceTalks;

		public static WorldObjectDef Debug_Arena;

		static WorldObjectDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(WorldObjectDefOf));
		}
	}
}
