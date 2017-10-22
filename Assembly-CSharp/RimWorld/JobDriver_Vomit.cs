using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_Vomit : JobDriver
	{
		private int ticksLeft;

		private PawnPosture lastPosture;

		public override PawnPosture Posture
		{
			get
			{
				return this.lastPosture;
			}
		}

		public override void Notify_LastPosture(PawnPosture posture, LayingDownState layingDown)
		{
			this.lastPosture = posture;
			base.layingDown = layingDown;
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeft, "ticksLeft", 0, false);
			Scribe_Values.Look<PawnPosture>(ref this.lastPosture, "lastPosture", PawnPosture.Standing, false);
		}

		public override bool TryMakePreToilReservations()
		{
			return true;
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			Toil to = new Toil
			{
				initAction = (Action)delegate
				{
					((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0033: stateMachine*/)._0024this.ticksLeft = Rand.Range(300, 900);
					int num = 0;
					IntVec3 c;
					while (true)
					{
						c = ((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0033: stateMachine*/)._0024this.pawn.Position + GenAdj.AdjacentCellsAndInside[Rand.Range(0, 9)];
						num++;
						if (num > 12)
						{
							c = ((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0033: stateMachine*/)._0024this.pawn.Position;
							break;
						}
						if (c.InBounds(((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0033: stateMachine*/)._0024this.pawn.Map) && c.Standable(((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0033: stateMachine*/)._0024this.pawn.Map))
							break;
					}
					((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0033: stateMachine*/)._0024this.job.targetA = c;
					((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0033: stateMachine*/)._0024this.pawn.pather.StopDead();
				},
				tickAction = (Action)delegate
				{
					if (((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_004a: stateMachine*/)._0024this.ticksLeft % 150 == 149)
					{
						FilthMaker.MakeFilth(((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_004a: stateMachine*/)._0024this.job.targetA.Cell, ((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_004a: stateMachine*/)._0024this.Map, ThingDefOf.FilthVomit, ((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_004a: stateMachine*/)._0024this.pawn.LabelIndefinite(), 1);
						if (((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_004a: stateMachine*/)._0024this.pawn.needs.food.CurLevelPercentage > 0.10000000149011612)
						{
							((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_004a: stateMachine*/)._0024this.pawn.needs.food.CurLevel -= (float)(((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_004a: stateMachine*/)._0024this.pawn.needs.food.MaxLevel * 0.039999999105930328);
						}
					}
					((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_004a: stateMachine*/)._0024this.ticksLeft--;
					if (((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_004a: stateMachine*/)._0024this.ticksLeft <= 0)
					{
						((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_004a: stateMachine*/)._0024this.ReadyForNextToil();
						TaleRecorder.RecordTale(TaleDefOf.Vomited, ((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_004a: stateMachine*/)._0024this.pawn);
					}
				},
				defaultCompleteMode = ToilCompleteMode.Never
			};
			to.WithEffect(EffecterDefOf.Vomit, TargetIndex.A);
			to.PlaySustainerOrSound((Func<SoundDef>)(() => SoundDef.Named("Vomit")));
			yield return to;
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}
