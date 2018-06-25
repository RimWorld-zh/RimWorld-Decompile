using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007D7 RID: 2007
	public class Designator_RemoveBridge : Designator_RemoveFloor
	{
		// Token: 0x06002C7F RID: 11391 RVA: 0x00177698 File Offset: 0x00175A98
		public Designator_RemoveBridge()
		{
			this.defaultLabel = "DesignatorRemoveBridge".Translate();
			this.defaultDesc = "DesignatorRemoveBridgeDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/RemoveBridge", true);
			this.hotKey = KeyBindingDefOf.Misc5;
		}

		// Token: 0x06002C80 RID: 11392 RVA: 0x001776E8 File Offset: 0x00175AE8
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
