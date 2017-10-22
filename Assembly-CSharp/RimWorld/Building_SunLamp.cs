using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	internal class Building_SunLamp : Building
	{
		public IEnumerable<IntVec3> GrowableCells
		{
			get
			{
				return GenRadial.RadialCellsAround(base.Position, base.def.specialDisplayRadius, true);
			}
		}

		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			if (DesignatorUtility.FindAllowedDesignator<Designator_ZoneAdd_Growing>() != null)
			{
				yield return (Gizmo)new Command_Action
				{
					action = new Action(this.MakeMatchingGrowZone),
					hotKey = KeyBindingDefOf.Misc2,
					defaultDesc = "CommandSunLampMakeGrowingZoneDesc".Translate(),
					icon = ContentFinder<Texture2D>.Get("UI/Designators/ZoneCreate_Growing", true),
					defaultLabel = "CommandSunLampMakeGrowingZoneLabel".Translate()
				};
			}
		}

		private void MakeMatchingGrowZone()
		{
			Designator designator = DesignatorUtility.FindAllowedDesignator<Designator_ZoneAdd_Growing>();
			designator.DesignateMultiCell(from tempCell in this.GrowableCells
			where designator.CanDesignateCell(tempCell).Accepted
			select tempCell);
		}
	}
}
