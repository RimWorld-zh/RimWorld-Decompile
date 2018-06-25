using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A37 RID: 2615
	public class JobDriver_Wait : JobDriver
	{
		// Token: 0x040024FE RID: 9470
		private const int TargetSearchInterval = 4;

		// Token: 0x06003A0B RID: 14859 RVA: 0x001EB198 File Offset: 0x001E9598
		public override string GetReport()
		{
			string result;
			if (this.job.def == JobDefOf.Wait_Combat)
			{
				if (this.pawn.RaceProps.Humanlike && this.pawn.story.WorkTagIsDisabled(WorkTags.Violent))
				{
					result = "ReportStanding".Translate();
				}
				else
				{
					result = base.GetReport();
				}
			}
			else
			{
				result = base.GetReport();
			}
			return result;
		}

		// Token: 0x06003A0C RID: 14860 RVA: 0x001EB210 File Offset: 0x001E9610
		public override bool TryMakePreToilReservations()
		{
			return true;
		}

		// Token: 0x06003A0D RID: 14861 RVA: 0x001EB228 File Offset: 0x001E9628
		protected override IEnumerable<Toil> MakeNewToils()
		{
			Toil wait = new Toil();
			wait.initAction = delegate()
			{
				base.Map.pawnDestinationReservationManager.Reserve(this.pawn, this.job, this.pawn.Position);
				this.pawn.pather.StopDead();
				this.CheckForAutoAttack();
			};
			wait.tickAction = delegate()
			{
				if (this.job.expiryInterval == -1 && this.job.def == JobDefOf.Wait_Combat && !this.pawn.Drafted)
				{
					Log.Error(this.pawn + " in eternal WaitCombat without being drafted.", false);
					base.ReadyForNextToil();
				}
				else if ((Find.TickManager.TicksGame + this.pawn.thingIDNumber) % 4 == 0)
				{
					this.CheckForAutoAttack();
				}
			};
			this.DecorateWaitToil(wait);
			wait.defaultCompleteMode = ToilCompleteMode.Never;
			yield return wait;
			yield break;
		}

		// Token: 0x06003A0E RID: 14862 RVA: 0x001EB252 File Offset: 0x001E9652
		public virtual void DecorateWaitToil(Toil wait)
		{
		}

		// Token: 0x06003A0F RID: 14863 RVA: 0x001EB255 File Offset: 0x001E9655
		public override void Notify_StanceChanged()
		{
			if (this.pawn.stances.curStance is Stance_Mobile)
			{
				this.CheckForAutoAttack();
			}
		}

		// Token: 0x06003A10 RID: 14864 RVA: 0x001EB278 File Offset: 0x001E9678
		private void CheckForAutoAttack()
		{
			if (!this.pawn.Downed)
			{
				if (!this.pawn.stances.FullBodyBusy)
				{
					this.collideWithPawns = false;
					bool flag = this.pawn.story == null || !this.pawn.story.WorkTagIsDisabled(WorkTags.Violent);
					bool flag2 = this.pawn.RaceProps.ToolUser && this.pawn.Faction == Faction.OfPlayer && !this.pawn.story.WorkTagIsDisabled(WorkTags.Firefighting);
					if (flag || flag2)
					{
						Fire fire = null;
						for (int i = 0; i < 9; i++)
						{
							IntVec3 c = this.pawn.Position + GenAdj.AdjacentCellsAndInside[i];
							if (c.InBounds(this.pawn.Map))
							{
								List<Thing> thingList = c.GetThingList(base.Map);
								for (int j = 0; j < thingList.Count; j++)
								{
									if (flag)
									{
										Pawn pawn = thingList[j] as Pawn;
										if (pawn != null && !pawn.Downed && this.pawn.HostileTo(pawn))
										{
											this.pawn.meleeVerbs.TryMeleeAttack(pawn, null, false);
											this.collideWithPawns = true;
											return;
										}
									}
									if (flag2)
									{
										Fire fire2 = thingList[j] as Fire;
										if (fire2 != null && (fire == null || fire2.fireSize < fire.fireSize || i == 8) && (fire2.parent == null || fire2.parent != this.pawn))
										{
											fire = fire2;
										}
									}
								}
							}
						}
						if (fire != null && (!this.pawn.InMentalState || this.pawn.MentalState.def.allowBeatfire))
						{
							this.pawn.natives.TryBeatFire(fire);
						}
						else if (flag && this.pawn.Faction != null && this.job.def == JobDefOf.Wait_Combat && (this.pawn.drafter == null || this.pawn.drafter.FireAtWill))
						{
							bool allowManualCastWeapons = !this.pawn.IsColonist;
							Verb verb = this.pawn.TryGetAttackVerb(null, allowManualCastWeapons);
							if (verb != null && !verb.verbProps.IsMeleeAttack)
							{
								TargetScanFlags targetScanFlags = TargetScanFlags.NeedLOSToPawns | TargetScanFlags.NeedLOSToNonPawns | TargetScanFlags.NeedThreat;
								if (verb.IsIncendiary())
								{
									targetScanFlags |= TargetScanFlags.NeedNonBurning;
								}
								Thing thing = (Thing)AttackTargetFinder.BestShootTargetFromCurrentPosition(this.pawn, null, verb.verbProps.range, verb.verbProps.minRange, targetScanFlags);
								if (thing != null)
								{
									this.pawn.TryStartAttack(thing);
									this.collideWithPawns = true;
								}
							}
						}
					}
				}
			}
		}
	}
}
