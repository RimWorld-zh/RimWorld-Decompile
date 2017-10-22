using RimWorld;
using System;
using System.Collections.Generic;

namespace Verse.AI
{
	public class JobDriver_Wait : JobDriver
	{
		private const int TargetSearchInterval = 4;

		public override string GetReport()
		{
			if (base.CurJob.def == JobDefOf.WaitCombat)
			{
				if (base.pawn.RaceProps.Humanlike && base.pawn.story.WorkTagIsDisabled(WorkTags.Violent))
				{
					return "ReportStanding".Translate();
				}
				return base.GetReport();
			}
			return base.GetReport();
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			Toil wait = new Toil
			{
				initAction = (Action)delegate
				{
					((_003CMakeNewToils_003Ec__Iterator1B4)/*Error near IL_0032: stateMachine*/)._003C_003Ef__this.Map.pawnDestinationManager.ReserveDestinationFor(((_003CMakeNewToils_003Ec__Iterator1B4)/*Error near IL_0032: stateMachine*/)._003C_003Ef__this.pawn, ((_003CMakeNewToils_003Ec__Iterator1B4)/*Error near IL_0032: stateMachine*/)._003C_003Ef__this.pawn.Position);
					((_003CMakeNewToils_003Ec__Iterator1B4)/*Error near IL_0032: stateMachine*/)._003C_003Ef__this.pawn.pather.StopDead();
					((_003CMakeNewToils_003Ec__Iterator1B4)/*Error near IL_0032: stateMachine*/)._003C_003Ef__this.CheckForAutoAttack();
				},
				tickAction = (Action)delegate
				{
					if (((_003CMakeNewToils_003Ec__Iterator1B4)/*Error near IL_0049: stateMachine*/)._003C_003Ef__this.CurJob.expiryInterval == -1 && ((_003CMakeNewToils_003Ec__Iterator1B4)/*Error near IL_0049: stateMachine*/)._003C_003Ef__this.CurJob.def == JobDefOf.WaitCombat && !((_003CMakeNewToils_003Ec__Iterator1B4)/*Error near IL_0049: stateMachine*/)._003C_003Ef__this.pawn.Drafted)
					{
						Log.Error(((_003CMakeNewToils_003Ec__Iterator1B4)/*Error near IL_0049: stateMachine*/)._003C_003Ef__this.pawn + " in eternal WaitCombat without being drafted.");
						((_003CMakeNewToils_003Ec__Iterator1B4)/*Error near IL_0049: stateMachine*/)._003C_003Ef__this.ReadyForNextToil();
					}
					else if ((Find.TickManager.TicksGame + ((_003CMakeNewToils_003Ec__Iterator1B4)/*Error near IL_0049: stateMachine*/)._003C_003Ef__this.pawn.thingIDNumber) % 4 == 0)
					{
						((_003CMakeNewToils_003Ec__Iterator1B4)/*Error near IL_0049: stateMachine*/)._003C_003Ef__this.CheckForAutoAttack();
					}
				}
			};
			this.DecorateWaitToil(wait);
			wait.defaultCompleteMode = ToilCompleteMode.Never;
			yield return wait;
		}

		public virtual void DecorateWaitToil(Toil wait)
		{
		}

		public override void Notify_StanceChanged()
		{
			if (base.pawn.stances.curStance is Stance_Mobile)
			{
				this.CheckForAutoAttack();
			}
		}

		private void CheckForAutoAttack()
		{
			if (!base.pawn.Downed && !base.pawn.stances.FullBodyBusy)
			{
				bool flag = base.pawn.story == null || !base.pawn.story.WorkTagIsDisabled(WorkTags.Violent);
				bool flag2 = base.pawn.RaceProps.ToolUser && base.pawn.Faction == Faction.OfPlayer && !base.pawn.story.WorkTagIsDisabled(WorkTags.Firefighting);
				if (!flag && !flag2)
					return;
				Fire fire = null;
				for (int i = 0; i < 9; i++)
				{
					IntVec3 c = base.pawn.Position + GenAdj.AdjacentCellsAndInside[i];
					if (c.InBounds(base.pawn.Map))
					{
						List<Thing> thingList = c.GetThingList(base.Map);
						for (int j = 0; j < thingList.Count; j++)
						{
							if (flag)
							{
								Pawn pawn = thingList[j] as Pawn;
								if (pawn != null && !pawn.Downed && base.pawn.HostileTo(pawn))
								{
									base.pawn.meleeVerbs.TryMeleeAttack(pawn, null, false);
									return;
								}
							}
							if (flag2)
							{
								Fire fire2 = thingList[j] as Fire;
								if (fire2 != null && (fire == null || fire2.fireSize < fire.fireSize || i == 8) && (fire2.parent == null || fire2.parent != base.pawn))
								{
									fire = fire2;
								}
							}
						}
					}
				}
				if (fire != null && (!base.pawn.InMentalState || base.pawn.MentalState.def.allowBeatfire))
				{
					base.pawn.natives.TryBeatFire(fire);
				}
				else if (flag && base.pawn.Faction != null && base.pawn.jobs.curJob.def == JobDefOf.WaitCombat)
				{
					if (base.pawn.drafter != null && !base.pawn.drafter.FireAtWill)
						return;
					bool allowManualCastWeapons = !base.pawn.IsColonist;
					Verb verb = base.pawn.TryGetAttackVerb(allowManualCastWeapons);
					if (verb != null && !verb.verbProps.MeleeRange)
					{
						TargetScanFlags targetScanFlags = TargetScanFlags.NeedLOSToPawns | TargetScanFlags.NeedLOSToNonPawns | TargetScanFlags.NeedThreat;
						if (verb.verbProps.ai_IsIncendiary)
						{
							targetScanFlags = (TargetScanFlags)(byte)((int)targetScanFlags | 16);
						}
						Thing thing = (Thing)AttackTargetFinder.BestShootTargetFromCurrentPosition(base.pawn, null, verb.verbProps.range, verb.verbProps.minRange, targetScanFlags);
						if (thing != null)
						{
							base.pawn.equipment.TryStartAttack(thing);
						}
					}
				}
			}
		}
	}
}
