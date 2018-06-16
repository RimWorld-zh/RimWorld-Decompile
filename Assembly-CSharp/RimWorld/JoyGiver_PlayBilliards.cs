using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000F5 RID: 245
	public class JoyGiver_PlayBilliards : JoyGiver_InteractBuilding
	{
		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000529 RID: 1321 RVA: 0x00038E74 File Offset: 0x00037274
		protected override bool CanDoDuringParty
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x00038E8C File Offset: 0x0003728C
		protected override Job TryGivePlayJob(Pawn pawn, Thing t)
		{
			Job result;
			if (!JoyGiver_PlayBilliards.ThingHasStandableSpaceOnAllSides(t))
			{
				result = null;
			}
			else
			{
				result = new Job(this.def.jobDef, t);
			}
			return result;
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x00038ECC File Offset: 0x000372CC
		public static bool ThingHasStandableSpaceOnAllSides(Thing t)
		{
			CellRect cellRect = t.OccupiedRect();
			CellRect.CellRectIterator iterator = cellRect.ExpandedBy(1).GetIterator();
			while (!iterator.Done())
			{
				IntVec3 c = iterator.Current;
				if (!cellRect.Contains(c))
				{
					if (!c.Standable(t.Map))
					{
						return false;
					}
				}
				iterator.MoveNext();
			}
			return true;
		}
	}
}
