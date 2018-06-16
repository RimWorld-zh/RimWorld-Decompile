using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020006B2 RID: 1714
	internal class Building_SunLamp : Building
	{
		// Token: 0x1700058B RID: 1419
		// (get) Token: 0x060024BE RID: 9406 RVA: 0x0013A4FC File Offset: 0x001388FC
		public IEnumerable<IntVec3> GrowableCells
		{
			get
			{
				return GenRadial.RadialCellsAround(base.Position, this.def.specialDisplayRadius, true);
			}
		}

		// Token: 0x060024BF RID: 9407 RVA: 0x0013A528 File Offset: 0x00138928
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

		// Token: 0x060024C0 RID: 9408 RVA: 0x0013A554 File Offset: 0x00138954
		private void MakeMatchingGrowZone()
		{
			Designator designator = DesignatorUtility.FindAllowedDesignator<Designator_ZoneAdd_Growing>();
			designator.DesignateMultiCell(from tempCell in this.GrowableCells
			where designator.CanDesignateCell(tempCell).Accepted
			select tempCell);
		}
	}
}
