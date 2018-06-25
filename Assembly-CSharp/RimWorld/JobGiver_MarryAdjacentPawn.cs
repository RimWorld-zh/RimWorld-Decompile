using System;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_MarryAdjacentPawn : ThinkNode_JobGiver
	{
		public JobGiver_MarryAdjacentPawn()
		{
		}

		protected override Job TryGiveJob(Pawn pawn)
		{
			Job result;
			if (!pawn.RaceProps.IsFlesh)
			{
				result = null;
			}
			else
			{
				for (int i = 0; i < 4; i++)
				{
					IntVec3 c = pawn.Position + GenAdj.CardinalDirections[i];
					if (c.InBounds(pawn.Map))
					{
						Thing thing = c.GetThingList(pawn.Map).Find((Thing x) => x is Pawn && this.CanMarry(pawn, (Pawn)x));
						if (thing != null)
						{
							return new Job(JobDefOf.MarryAdjacentPawn, thing);
						}
					}
				}
				result = null;
			}
			return result;
		}

		private bool CanMarry(Pawn pawn, Pawn toMarry)
		{
			return !toMarry.Drafted && pawn.relations.DirectRelationExists(PawnRelationDefOf.Fiance, toMarry);
		}

		[CompilerGenerated]
		private sealed class <TryGiveJob>c__AnonStorey0
		{
			internal Pawn pawn;

			internal JobGiver_MarryAdjacentPawn $this;

			public <TryGiveJob>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Thing x)
			{
				return x is Pawn && this.$this.CanMarry(this.pawn, (Pawn)x);
			}
		}
	}
}
