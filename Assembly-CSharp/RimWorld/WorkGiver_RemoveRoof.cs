using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class WorkGiver_RemoveRoof : WorkGiver_Scanner
	{
		public override bool Prioritized
		{
			get
			{
				return true;
			}
		}

		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.ClosestTouch;
			}
		}

		public override IEnumerable<IntVec3> PotentialWorkCellsGlobal(Pawn pawn)
		{
			return pawn.Map.areaManager.NoRoof.ActiveCells;
		}

		public override bool HasJobOnCell(Pawn pawn, IntVec3 c)
		{
			bool result;
			if (!((Area)pawn.Map.areaManager.NoRoof)[c])
			{
				result = false;
			}
			else if (!c.Roofed(pawn.Map))
			{
				result = false;
			}
			else if (c.IsForbidden(pawn))
			{
				result = false;
			}
			else
			{
				LocalTargetInfo target = c;
				ReservationLayerDef ceiling = ReservationLayerDefOf.Ceiling;
				result = ((byte)(pawn.CanReserve(target, 1, -1, ceiling, false) ? 1 : 0) != 0);
			}
			return result;
		}

		public override Job JobOnCell(Pawn pawn, IntVec3 c)
		{
			return new Job(JobDefOf.RemoveRoof, c, c);
		}

		public override float GetPriority(Pawn pawn, TargetInfo t)
		{
			IntVec3 cell = t.Cell;
			int num = 0;
			int num2 = 0;
			float result;
			while (true)
			{
				if (num2 < 8)
				{
					IntVec3 c = cell + GenAdj.AdjacentCells[num2];
					if (c.InBounds(t.Map))
					{
						Building edifice = c.GetEdifice(t.Map);
						if (edifice != null && edifice.def.holdsRoof)
						{
							result = -60f;
							break;
						}
						if (c.Roofed(pawn.Map))
						{
							num++;
						}
					}
					num2++;
					continue;
				}
				result = (float)(-Mathf.Min(num, 3));
				break;
			}
			return result;
		}
	}
}
