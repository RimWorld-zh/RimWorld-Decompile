using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007EC RID: 2028
	public class Designator_ZoneDelete : Designator_Zone
	{
		// Token: 0x06002D0C RID: 11532 RVA: 0x0017AD84 File Offset: 0x00179184
		public Designator_ZoneDelete()
		{
			this.defaultLabel = "DesignatorZoneDelete".Translate();
			this.defaultDesc = "DesignatorZoneDeleteDesc".Translate();
			this.soundDragSustain = SoundDefOf.Designate_DragAreaDelete;
			this.soundDragChanged = null;
			this.soundSucceeded = SoundDefOf.Designate_ZoneDelete;
			this.useMouseIcon = true;
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/ZoneDelete", true);
			this.hotKey = KeyBindingDefOf.Misc4;
		}

		// Token: 0x06002D0D RID: 11533 RVA: 0x0017AE04 File Offset: 0x00179204
		public override AcceptanceReport CanDesignateCell(IntVec3 sq)
		{
			AcceptanceReport result;
			if (!sq.InBounds(base.Map))
			{
				result = false;
			}
			else if (sq.Fogged(base.Map))
			{
				result = false;
			}
			else if (base.Map.zoneManager.ZoneAt(sq) == null)
			{
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06002D0E RID: 11534 RVA: 0x0017AE7C File Offset: 0x0017927C
		public override void DesignateSingleCell(IntVec3 c)
		{
			Zone zone = base.Map.zoneManager.ZoneAt(c);
			zone.RemoveCell(c);
			if (!this.justDesignated.Contains(zone))
			{
				this.justDesignated.Add(zone);
			}
		}

		// Token: 0x06002D0F RID: 11535 RVA: 0x0017AEC0 File Offset: 0x001792C0
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			for (int i = 0; i < this.justDesignated.Count; i++)
			{
				this.justDesignated[i].CheckContiguous();
			}
			this.justDesignated.Clear();
		}

		// Token: 0x040017B6 RID: 6070
		private List<Zone> justDesignated = new List<Zone>();
	}
}
