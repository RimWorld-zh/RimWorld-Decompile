using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007EE RID: 2030
	public class Designator_ZoneDelete : Designator_Zone
	{
		// Token: 0x040017B6 RID: 6070
		private List<Zone> justDesignated = new List<Zone>();

		// Token: 0x06002D10 RID: 11536 RVA: 0x0017AED4 File Offset: 0x001792D4
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

		// Token: 0x06002D11 RID: 11537 RVA: 0x0017AF54 File Offset: 0x00179354
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

		// Token: 0x06002D12 RID: 11538 RVA: 0x0017AFCC File Offset: 0x001793CC
		public override void DesignateSingleCell(IntVec3 c)
		{
			Zone zone = base.Map.zoneManager.ZoneAt(c);
			zone.RemoveCell(c);
			if (!this.justDesignated.Contains(zone))
			{
				this.justDesignated.Add(zone);
			}
		}

		// Token: 0x06002D13 RID: 11539 RVA: 0x0017B010 File Offset: 0x00179410
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			for (int i = 0; i < this.justDesignated.Count; i++)
			{
				this.justDesignated[i].CheckContiguous();
			}
			this.justDesignated.Clear();
		}
	}
}
