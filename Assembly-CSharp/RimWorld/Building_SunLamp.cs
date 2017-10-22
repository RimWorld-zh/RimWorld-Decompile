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
			using (IEnumerator<Gizmo> enumerator = this._003CGetGizmos_003E__BaseCallProxy0().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					Gizmo baseGizmo = enumerator.Current;
					yield return baseGizmo;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (DesignatorUtility.FindAllowedDesignator<Designator_ZoneAdd_Growing>() == null)
				yield break;
			yield return (Gizmo)new Command_Action
			{
				action = new Action(this.MakeMatchingGrowZone),
				hotKey = KeyBindingDefOf.Misc2,
				defaultDesc = "CommandSunLampMakeGrowingZoneDesc".Translate(),
				icon = ContentFinder<Texture2D>.Get("UI/Designators/ZoneCreate_Growing", true),
				defaultLabel = "CommandSunLampMakeGrowingZoneLabel".Translate()
			};
			/*Error: Unable to find new state assignment for yield return*/;
			IL_0164:
			/*Error near IL_0165: Unexpected return in MoveNext()*/;
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
