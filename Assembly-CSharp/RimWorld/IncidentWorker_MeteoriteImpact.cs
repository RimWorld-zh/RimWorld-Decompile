using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000333 RID: 819
	public class IncidentWorker_MeteoriteImpact : IncidentWorker
	{
		// Token: 0x06000E03 RID: 3587 RVA: 0x000778DC File Offset: 0x00075CDC
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 intVec;
			return this.TryFindCell(out intVec, map);
		}

		// Token: 0x06000E04 RID: 3588 RVA: 0x00077908 File Offset: 0x00075D08
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 intVec;
			bool result;
			if (!this.TryFindCell(out intVec, map))
			{
				result = false;
			}
			else
			{
				List<Thing> list = ThingSetMakerDefOf.Meteorite.root.Generate();
				SkyfallerMaker.SpawnSkyfaller(ThingDefOf.MeteoriteIncoming, list, intVec, map);
				LetterDef textLetterDef = (!list[0].def.building.isResourceRock) ? LetterDefOf.NeutralEvent : LetterDefOf.PositiveEvent;
				string text = string.Format(this.def.letterText, list[0].def.label).CapitalizeFirst();
				Find.LetterStack.ReceiveLetter(this.def.letterLabel, text, textLetterDef, new TargetInfo(intVec, map, false), null, null);
				result = true;
			}
			return result;
		}

		// Token: 0x06000E05 RID: 3589 RVA: 0x000779DC File Offset: 0x00075DDC
		private bool TryFindCell(out IntVec3 cell, Map map)
		{
			int maxMineables = ThingSetMaker_Meteorite.MineablesCountRange.max;
			return CellFinderLoose.TryFindSkyfallerCell(ThingDefOf.MeteoriteIncoming, map, out cell, 10, default(IntVec3), -1, true, false, false, false, true, true, delegate(IntVec3 x)
			{
				int num = Mathf.CeilToInt(Mathf.Sqrt((float)maxMineables)) + 2;
				CellRect cellRect = CellRect.CenteredOn(x, num, num);
				int num2 = 0;
				CellRect.CellRectIterator iterator = cellRect.GetIterator();
				while (!iterator.Done())
				{
					if (iterator.Current.InBounds(map) && iterator.Current.Standable(map))
					{
						num2++;
					}
					iterator.MoveNext();
				}
				return num2 >= maxMineables;
			});
		}
	}
}
