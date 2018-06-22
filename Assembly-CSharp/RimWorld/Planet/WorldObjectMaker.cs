using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000627 RID: 1575
	public static class WorldObjectMaker
	{
		// Token: 0x0600202A RID: 8234 RVA: 0x00113CB4 File Offset: 0x001120B4
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
