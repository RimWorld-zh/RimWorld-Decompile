using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007D9 RID: 2009
	public class Designator_RemoveBridge : Designator_RemoveFloor
	{
		// Token: 0x06002C83 RID: 11395 RVA: 0x0017710C File Offset: 0x0017550C
		public Designator_RemoveBridge()
		{
			this.defaultLabel = "DesignatorRemoveBridge".Translate();
			this.defaultDesc = "DesignatorRemoveBridgeDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/RemoveBridge", true);
			this.hotKey = KeyBindingDefOf.Misc5;
		}

		// Token: 0x06002C84 RID: 11396 RVA: 0x0017715C File Offset: 0x0017555C
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			AcceptanceReport result;
			if (c.InBounds(base.Map) && c.GetTerrain(base.Map) != TerrainDefOf.Bridge)
			{
				result = false;
			}
			else
			{
				result = base.CanDesignateCell(c);
			}
			return result;
		}
	}
}
