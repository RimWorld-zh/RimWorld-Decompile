using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020006B0 RID: 1712
	internal class Building_SunLamp : Building
	{
		// Token: 0x1700058B RID: 1419
		// (get) Token: 0x060024BC RID: 9404 RVA: 0x0013A80C File Offset: 0x00138C0C
		public IEnumerable<IntVec3> GrowableCells
		{
			get
			{
				return GenRadial.RadialCellsAround(base.Position, this.def.specialDisplayRadius, true);
			}
		}

		// Token: 0x060024BD RID: 9405 RVA: 0x0013A838 File Offset: 0x00138C38
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo baseGizmo in this.<GetGizmos>__BaseCallProxy0())
			{
				yield return baseGizmo;
			}
			if (DesignatorUtility.FindAllowedDesignator<Designator_ZoneAdd_Growing>() != null)
			{
				yield return new Command_Action
				{
					action = new Action(this.MakeMatchingGrowZone),
					hotKey = KeyBindingDefOf.Misc2,
					defaultDesc = "CommandSunLampMakeGrowingZoneDesc".Translate(),
					icon = ContentFinder<Texture2D>.Get("UI/Designators/ZoneCreate_Growing", true),
					defaultLabel = "CommandSunLampMakeGrowingZoneLabel".Translate()
				};
			}
			yield break;
		}

		// Token: 0x060024BE RID: 9406 RVA: 0x0013A864 File Offset: 0x00138C64
		private void MakeMatchingGrowZone()
		{
			Designator designator = DesignatorUtility.FindAllowedDesignator<Designator_ZoneAdd_Growing>();
			designator.DesignateMultiCell(from tempCell in this.GrowableCells
			where designator.CanDesignateCell(tempCell).Accepted
			select tempCell);
		}
	}
}
