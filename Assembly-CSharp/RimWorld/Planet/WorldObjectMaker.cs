using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000629 RID: 1577
	public static class WorldObjectMaker
	{
		// Token: 0x0600202D RID: 8237 RVA: 0x0011406C File Offset: 0x0011246C
		public static WorldObject MakeWorldObject(WorldObjectDef def)
		{
			WorldObject worldObject = (WorldObject)Activator.CreateInstance(def.worldObjectClass);
			worldObject.def = def;
			worldObject.ID = Find.UniqueIDsManager.GetNextWorldObjectID();
			worldObject.creationGameTicks = Find.TickManager.TicksGame;
			worldObject.PostMake();
			return worldObject;
		}
	}
}
