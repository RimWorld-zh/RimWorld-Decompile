using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x02000D5B RID: 3419
	public class Pawn_HealthTracker : IExposable
	{
		// Token: 0x06004C4F RID: 19535 RVA: 0x0027BB44 File Offset: 0x00279F44
		public Pawn_HealthTracker(Pawn pawn)
		{
			this.pawn = pawn;
			this.hediffSet = new HediffSet(pawn);
			this.capacities = new PawnCapacitiesHandler(pawn);
			this.summaryHealth = new SummaryHealthHandler(pawn);
			this.surgeryBills = new BillStack(pawn);
			this.immunity = new ImmunityHandler(pawn);
		}

		// Token: 0x17000C72 RID: 3186
		// (get) Token: 0x06004C50 RID: 19536 RVA: 0x0027BBD4 File Offset: 0x00279FD4
		public PawnHealthState State
		{
			get
			{
				return this.healthState;
			}
		}

		// Token: 0x17000C73 RID: 3187
		// (get) Token: 0x06004C51 RID: 19537 RVA: 0x0027BBF0 File Offset: 0x00279FF0
		public bool Downed
		{
			get
			{
				return this.healthState == PawnHealthState.Down;
			}
		}

		// Token: 0x17000C74 RID: 3188
		// (get) Token: 0x06004C52 RID: 19538 RVA: 0x0027BC10 File Offset: 0x0027A010
		public bool Dead
		{
			get
			{
				return this.healthState == PawnHealthState.Dead;
			}
		}

		// Token: 0x17000C75 RID: 3189
		// (get) Token: 0x06004C53 RID: 19539 RVA: 0x0027BC30 File Offset: 0x0027A030
		public float LethalDamageThreshold
		{
			get
			{
				return 150f * this.pawn.HealthScale;
			}
		}

		// Token: 0x17000C76 RID: 3190
		// (get) Token: 0x06004C54 RID: 19540 RVA: 0x0027BC58 File Offset: 0x0027A058
		public bool InPainShock
		{
			get
			{
				return this.hediffSet.PainTotal >= this.pawn.GetStatValue(StatDefOf.PainShockThreshold, true);
			}
		}

		// Token: 0x06004C55 RID: 19541 RVA: 0x0027BC90 File Offset: 0x0027A090
		public void Reset()
		{
			this.healthState = PawnHealthState.Mobile;
			this.hediffSet.Clear();
			this.capacities.Clear();
			this.summaryHealth.Notify_HealthChanged();
			this.surgeryBills.Clear();
			this.immunity = new ImmunityHandler(this.pawn);
		}

		// Token: 0x06004C56 RID: 19542 RVA: 0x0027BCE4 File Offset: 0x0027A0E4
		public void ExposeData()
		{
			Scribe_Values.Look<PawnHealthState>(ref this.healthState, "healthState", PawnHealthState.Mobile, false);
			Scribe_Values.Look<bool>(ref this.forceIncap, "forceIncap", false, false);
			Scribe_Deep.Look<HediffSet>(ref this.hediffSet, "hediffSet", new object[]
			{
				this.pawn
			});
			Scribe_Deep.Look<BillStack>(ref this.surgeryBills, "surgeryBills", new object[]
			{
				this.pawn
			});
			Scribe_Deep.Look<ImmunityHandler>(ref this.immunity, "immunity", new object[]
			{
				this.pawn
			});
		}

		// Token: 0x06004C57 RID: 19543 RVA: 0x0027BD74 File Offset: 0x0027A174
		public Hediff AddHediff(HediffDef def, BodyPartRecord part = null, DamageInfo? dinfo = null, DamageWorker.DamageResult result = null)
		{
			Hediff hediff = HediffMaker.MakeHediff(def, this.pawn, null);
			this.AddHediff(hediff, part, dinfo, result);
			return hediff;
		}

		// Token: 0x06004C58 RID: 19544 RVA: 0x0027BDA4 File Offset: 0x0027A1A4
		public void AddHediff(Hediff hediff, BodyPartRecord part = null, DamageInfo? dinfo = null, DamageWorker.DamageResult result = null)
		{
			if (part != null)
			{
				hediff.Part = part;
			}
			this.hediffSet.AddDirect(hediff, dinfo, result);
			this.CheckForStateChange(dinfo, hediff);
			if (this.pawn.RaceProps.hediffGiverSets != null)
			{
				for (int i = 0; i < this.pawn.RaceProps.hediffGiverSets.Count; i++)
				{
					HediffGiverSetDef hediffGiverSetDef = this.pawn.RaceProps.hediffGiverSets[i];
					for (int j = 0; j < hediffGiverSetDef.hediffGivers.Count; j++)
					{
						hediffGiverSetDef.hediffGivers[j].OnHediffAdded(this.pawn, hediff);
					}
				}
			}
		}

		// Token: 0x06004C59 RID: 19545 RVA: 0x0027BE63 File Offset: 0x0027A263
		public void RemoveHediff(Hediff hediff)
		{
			this.hediffSet.hediffs.Remove(hediff);
			hediff.PostRemoved();
			this.Notify_HediffChanged(null);
		}

		// Token: 0x06004C5A RID: 19546 RVA: 0x0027BE88 File Offset: 0x0027A288
		public void Notify_HediffChanged(Hediff hediff)
		{
			this.hediffSet.DirtyCache();
			this.CheckForStateChange(null, hediff);
		}

		// Token: 0x06004C5B RID: 19547 RVA: 0x0027BEB4 File Offset: 0x0027A2B4
		public void PreApplyDamage(DamageInfo dinfo, out bool absorbed)
		{
			if (dinfo.Instigator != null && this.pawn.Faction != null && this.pawn.Faction.IsPlayer && !this.pawn.InAggroMentalState)
			{
				Pawn pawn = dinfo.Instigator as Pawn;
				if (pawn != null && pawn.guilt != null && pawn.mindState != null)
				{
					pawn.guilt.Notify_Guilty();
				}
			}
			if (this.pawn.Spawned)
			{
				if (!this.pawn.Position.Fogged(this.pawn.Map))
				{
					this.pawn.mindState.Active = true;
				}
				Lord lord = this.pawn.GetLord();
				if (lord != null)
				{
					lord.Notify_PawnDamaged(this.pawn, dinfo);
				}
				if (dinfo.Def.externalViolence)
				{
					GenClamor.DoClamor(this.pawn, 18f, ClamorDefOf.Harm);
				}
				this.pawn.jobs.Notify_DamageTaken(dinfo);
			}
			if (this.pawn.Faction != null)
			{
				this.pawn.Faction.Notify_MemberTookDamage(this.pawn, dinfo);
				if (Current.ProgramState == ProgramState.Playing && this.pawn.Faction == Faction.OfPlayer && dinfo.Def.externalViolence && this.pawn.SpawnedOrAnyParentSpawned)
				{
					this.pawn.MapHeld.dangerWatcher.Notify_ColonistHarmedExternally();
				}
			}
			if (this.pawn.apparel != null)
			{
				List<Apparel> wornApparel = this.pawn.apparel.WornApparel;
				for (int i = 0; i < wornApparel.Count; i++)
				{
					if (wornApparel[i].CheckPreAbsorbDamage(dinfo))
					{
						absorbed = true;
						return;
					}
				}
			}
			if (this.pawn.Spawned)
			{
				this.pawn.stances.Notify_DamageTaken(dinfo);
				this.pawn.stances.stunner.Notify_DamageApplied(dinfo, !this.pawn.RaceProps.IsFlesh);
			}
			if (this.pawn.RaceProps.IsFlesh && dinfo.Def.externalViolence)
			{
				Pawn pawn2 = dinfo.Instigator as Pawn;
				if (pawn2 != null)
				{
					if (pawn2.HostileTo(this.pawn))
					{
						this.pawn.relations.canGetRescuedThought = true;
					}
					if (this.pawn.RaceProps.Humanlike && pawn2.RaceProps.Humanlike && this.pawn.needs.mood != null && (!pawn2.HostileTo(this.pawn) || (pawn2.Faction == this.pawn.Faction && pawn2.InMentalState)))
					{
						this.pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.HarmedMe, pawn2);
					}
				}
				TaleRecorder.RecordTale(TaleDefOf.Wounded, new object[]
				{
					this.pawn,
					pawn2,
					dinfo.Weapon
				});
			}
			absorbed = false;
		}

		// Token: 0x06004C5C RID: 19548 RVA: 0x0027C21C File Offset: 0x0027A61C
		public void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			if (this.ShouldBeDead())
			{
				if (!this.pawn.Destroyed)
				{
					this.pawn.Kill(new DamageInfo?(dinfo), null);
				}
			}
			else if (dinfo.Def.additionalHediffs != null)
			{
				List<DamageDefAdditionalHediff> additionalHediffs = dinfo.Def.additionalHediffs;
				for (int i = 0; i < additionalHediffs.Count; i++)
				{
					DamageDefAdditionalHediff damageDefAdditionalHediff = additionalHediffs[i];
					if (damageDefAdditionalHediff.hediff != null)
					{
						float num = totalDamageDealt * damageDefAdditionalHediff.severityPerDamageDealt;
						if (damageDefAdditionalHediff.victimSeverityScaling != null)
						{
							num *= this.pawn.GetStatValue(damageDefAdditionalHediff.victimSeverityScaling, true);
						}
						if (num >= 0f)
						{
							Hediff hediff = HediffMaker.MakeHediff(damageDefAdditionalHediff.hediff, this.pawn, null);
							hediff.Severity = num;
							this.AddHediff(hediff, null, new DamageInfo?(dinfo), null);
							if (this.Dead)
							{
								break;
							}
						}
					}
				}
			}
		}

		// Token: 0x06004C5D RID: 19549 RVA: 0x0027C320 File Offset: 0x0027A720
		public void RestorePart(BodyPartRecord part, Hediff diffException = null, bool checkStateChange = true)
		{
			if (part == null)
			{
				Log.Error("Tried to restore null body part.", false);
			}
			else
			{
				this.RestorePartRecursiveInt(part, diffException);
				this.hediffSet.DirtyCache();
				if (checkStateChange)
				{
					this.CheckForStateChange(null, null);
				}
			}
		}

		// Token: 0x06004C5E RID: 19550 RVA: 0x0027C370 File Offset: 0x0027A770
		private void RestorePartRecursiveInt(BodyPartRecord part, Hediff diffException = null)
		{
			List<Hediff> hediffs = this.hediffSet.hediffs;
			for (int i = hediffs.Count - 1; i >= 0; i--)
			{
				Hediff hediff = hediffs[i];
				if (hediff.Part == part && hediff != diffException)
				{
					Hediff hediff2 = hediffs[i];
					hediffs.RemoveAt(i);
					hediff2.PostRemoved();
				}
			}
			for (int j = 0; j < part.parts.Count; j++)
			{
				this.RestorePartRecursiveInt(part.parts[j], diffException);
			}
		}

		// Token: 0x06004C5F RID: 19551 RVA: 0x0027C410 File Offset: 0x0027A810
		public void CheckForStateChange(DamageInfo? dinfo, Hediff hediff)
		{
			if (!this.Dead)
			{
				if (this.ShouldBeDead())
				{
					if (!this.pawn.Destroyed)
					{
						this.pawn.Kill(dinfo, hediff);
					}
				}
				else if (!this.Downed)
				{
					if (this.ShouldBeDowned())
					{
						float chance;
						if (this.pawn.RaceProps.Animal)
						{
							chance = 0.47f;
						}
						else if (this.pawn.RaceProps.IsMechanoid)
						{
							chance = 1f;
						}
						else
						{
							chance = 0.67f;
						}
						if (!this.forceIncap && dinfo != null && dinfo.Value.Def.externalViolence && (this.pawn.Faction == null || !this.pawn.Faction.IsPlayer) && !this.pawn.IsPrisonerOfColony && Rand.Chance(chance))
						{
							this.pawn.Kill(dinfo, null);
						}
						else
						{
							this.forceIncap = false;
							this.MakeDowned(dinfo, hediff);
						}
					}
					else if (!this.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
					{
						if (this.pawn.carryTracker != null && this.pawn.carryTracker.CarriedThing != null)
						{
							if (this.pawn.jobs != null && this.pawn.CurJob != null)
							{
								this.pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true);
							}
						}
						if (this.pawn.equipment != null && this.pawn.equipment.Primary != null)
						{
							if (this.pawn.kindDef.destroyGearOnDrop)
							{
								this.pawn.equipment.DestroyEquipment(this.pawn.equipment.Primary);
							}
							else if (this.pawn.InContainerEnclosed)
							{
								this.pawn.equipment.TryTransferEquipmentToContainer(this.pawn.equipment.Primary, this.pawn.holdingOwner);
							}
							else if (this.pawn.SpawnedOrAnyParentSpawned)
							{
								ThingWithComps thingWithComps;
								this.pawn.equipment.TryDropEquipment(this.pawn.equipment.Primary, out thingWithComps, this.pawn.PositionHeld, true);
							}
							else
							{
								this.pawn.equipment.DestroyEquipment(this.pawn.equipment.Primary);
							}
						}
					}
				}
				else if (!this.ShouldBeDowned())
				{
					this.MakeUndowned();
				}
			}
		}

		// Token: 0x06004C60 RID: 19552 RVA: 0x0027C6E4 File Offset: 0x0027AAE4
		private bool ShouldBeDowned()
		{
			return this.InPainShock || !this.capacities.CanBeAwake || !this.capacities.CapableOf(PawnCapacityDefOf.Moving);
		}

		// Token: 0x06004C61 RID: 19553 RVA: 0x0027C72C File Offset: 0x0027AB2C
		private bool ShouldBeDead()
		{
			bool result;
			if (this.Dead)
			{
				result = true;
			}
			else
			{
				for (int i = 0; i < this.hediffSet.hediffs.Count; i++)
				{
					if (this.hediffSet.hediffs[i].CauseDeathNow())
					{
						return true;
					}
				}
				if (this.ShouldBeDeadFromRequiredCapacity() != null)
				{
					result = true;
				}
				else
				{
					float num = PawnCapacityUtility.CalculatePartEfficiency(this.hediffSet, this.pawn.RaceProps.body.corePart, false, null);
					result = (num <= 0.0001f || this.ShouldBeDeadFromLethalDamageThreshold());
				}
			}
			return result;
		}

		// Token: 0x06004C62 RID: 19554 RVA: 0x0027C7F4 File Offset: 0x0027ABF4
		public PawnCapacityDef ShouldBeDeadFromRequiredCapacity()
		{
			List<PawnCapacityDef> allDefsListForReading = DefDatabase<PawnCapacityDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				PawnCapacityDef pawnCapacityDef = allDefsListForReading[i];
				bool flag = (!this.pawn.RaceProps.IsFlesh) ? pawnCapacityDef.lethalMechanoids : pawnCapacityDef.lethalFlesh;
				if (flag && !this.capacities.CapableOf(pawnCapacityDef))
				{
					return pawnCapacityDef;
				}
			}
			return null;
		}

		// Token: 0x06004C63 RID: 19555 RVA: 0x0027C87C File Offset: 0x0027AC7C
		public bool ShouldBeDeadFromLethalDamageThreshold()
		{
			float num = 0f;
			for (int i = 0; i < this.hediffSet.hediffs.Count; i++)
			{
				if (this.hediffSet.hediffs[i] is Hediff_Injury)
				{
					num += this.hediffSet.hediffs[i].Severity;
				}
			}
			return num >= this.LethalDamageThreshold;
		}

		// Token: 0x06004C64 RID: 19556 RVA: 0x0027C8FC File Offset: 0x0027ACFC
		public bool WouldDieAfterAddingHediff(Hediff hediff)
		{
			bool result;
			if (this.Dead)
			{
				result = true;
			}
			else
			{
				this.hediffSet.hediffs.Add(hediff);
				this.hediffSet.DirtyCache();
				bool flag = this.ShouldBeDead();
				this.hediffSet.hediffs.Remove(hediff);
				this.hediffSet.DirtyCache();
				result = flag;
			}
			return result;
		}

		// Token: 0x06004C65 RID: 19557 RVA: 0x0027C964 File Offset: 0x0027AD64
		public bool WouldDieAfterAddingHediff(HediffDef def, BodyPartRecord part, float severity)
		{
			Hediff hediff = HediffMaker.MakeHediff(def, this.pawn, part);
			hediff.Severity = severity;
			return this.WouldDieAfterAddingHediff(hediff);
		}

		// Token: 0x06004C66 RID: 19558 RVA: 0x0027C998 File Offset: 0x0027AD98
		public bool WouldBeDownedAfterAddingHediff(Hediff hediff)
		{
			bool result;
			if (this.Dead)
			{
				result = false;
			}
			else
			{
				this.hediffSet.hediffs.Add(hediff);
				this.hediffSet.DirtyCache();
				bool flag = this.ShouldBeDowned();
				this.hediffSet.hediffs.Remove(hediff);
				this.hediffSet.DirtyCache();
				result = flag;
			}
			return result;
		}

		// Token: 0x06004C67 RID: 19559 RVA: 0x0027CA00 File Offset: 0x0027AE00
		public bool WouldBeDownedAfterAddingHediff(HediffDef def, BodyPartRecord part, float severity)
		{
			Hediff hediff = HediffMaker.MakeHediff(def, this.pawn, part);
			hediff.Severity = severity;
			return this.WouldBeDownedAfterAddingHediff(hediff);
		}

		// Token: 0x06004C68 RID: 19560 RVA: 0x0027CA31 File Offset: 0x0027AE31
		public void SetDead()
		{
			if (this.Dead)
			{
				Log.Error(this.pawn + " set dead while already dead.", false);
			}
			this.healthState = PawnHealthState.Dead;
		}

		// Token: 0x06004C69 RID: 19561 RVA: 0x0027CA5C File Offset: 0x0027AE5C
		private void MakeDowned(DamageInfo? dinfo, Hediff hediff)
		{
			if (this.Downed)
			{
				Log.Error(this.pawn + " tried to do MakeDowned while already downed.", false);
			}
			else
			{
				if (this.pawn.guilt != null && this.pawn.GetLord() != null && this.pawn.GetLord().LordJob != null && this.pawn.GetLord().LordJob.GuiltyOnDowned)
				{
					this.pawn.guilt.Notify_Guilty();
				}
				this.healthState = PawnHealthState.Down;
				PawnDiedOrDownedThoughtsUtility.TryGiveThoughts(this.pawn, dinfo, PawnDiedOrDownedThoughtsKind.Downed);
				if (this.pawn.InMentalState)
				{
					this.pawn.mindState.mentalStateHandler.CurState.RecoverFromState();
				}
				if (this.pawn.Spawned)
				{
					if (this.pawn.IsColonist && dinfo != null && dinfo.Value.Def.externalViolence)
					{
						Find.StoryWatcher.watcherRampUp.Notify_ColonistViolentlyDownedOrKilled(this.pawn);
					}
					this.pawn.DropAndForbidEverything(true);
					this.pawn.stances.CancelBusyStanceSoft();
				}
				this.pawn.ClearMind(true);
				if (Current.ProgramState == ProgramState.Playing)
				{
					Lord lord = this.pawn.GetLord();
					if (lord != null)
					{
						lord.Notify_PawnLost(this.pawn, PawnLostCondition.IncappedOrKilled);
					}
				}
				if (this.pawn.Drafted)
				{
					this.pawn.drafter.Drafted = false;
				}
				PortraitsCache.SetDirty(this.pawn);
				if (this.pawn.SpawnedOrAnyParentSpawned)
				{
					GenHostility.Notify_PawnLostForTutor(this.pawn, this.pawn.MapHeld);
				}
				if (this.pawn.RaceProps.Humanlike && Current.ProgramState == ProgramState.Playing && this.pawn.SpawnedOrAnyParentSpawned)
				{
					if (this.pawn.HostileTo(Faction.OfPlayer))
					{
						LessonAutoActivator.TeachOpportunity(ConceptDefOf.Capturing, this.pawn, OpportunityType.Important);
					}
					if (this.pawn.Faction == Faction.OfPlayer)
					{
						LessonAutoActivator.TeachOpportunity(ConceptDefOf.Rescuing, this.pawn, OpportunityType.Critical);
					}
				}
				if (dinfo != null && dinfo.Value.Instigator != null)
				{
					Pawn pawn = dinfo.Value.Instigator as Pawn;
					if (pawn != null)
					{
						RecordsUtility.Notify_PawnDowned(this.pawn, pawn);
					}
				}
				if (this.pawn.Spawned)
				{
					TaleRecorder.RecordTale(TaleDefOf.Downed, new object[]
					{
						this.pawn,
						(dinfo == null) ? null : (dinfo.Value.Instigator as Pawn),
						(dinfo == null) ? null : dinfo.Value.Weapon
					});
					Find.BattleLog.Add(new BattleLogEntry_StateTransition(this.pawn, RulePackDefOf.Transition_Downed, (dinfo == null) ? null : (dinfo.Value.Instigator as Pawn), hediff, (dinfo == null) ? null : dinfo.Value.HitPart));
				}
			}
		}

		// Token: 0x06004C6A RID: 19562 RVA: 0x0027CDD8 File Offset: 0x0027B1D8
		private void MakeUndowned()
		{
			if (!this.Downed)
			{
				Log.Error(this.pawn + " tried to do MakeUndowned when already undowned.", false);
			}
			else
			{
				this.healthState = PawnHealthState.Mobile;
				if (PawnUtility.ShouldSendNotificationAbout(this.pawn))
				{
					Messages.Message("MessageNoLongerDowned".Translate(new object[]
					{
						this.pawn.LabelCap
					}), this.pawn, MessageTypeDefOf.PositiveEvent, true);
				}
				if (this.pawn.Spawned && !this.pawn.InBed())
				{
					this.pawn.jobs.EndCurrentJob(JobCondition.Incompletable, true);
				}
				PortraitsCache.SetDirty(this.pawn);
				if (this.pawn.guest != null)
				{
					this.pawn.guest.Notify_PawnUndowned();
				}
			}
		}

		// Token: 0x06004C6B RID: 19563 RVA: 0x0027CEB8 File Offset: 0x0027B2B8
		public void NotifyPlayerOfKilled(DamageInfo? dinfo, Hediff hediff, Caravan caravan)
		{
			string text = "";
			if (dinfo != null)
			{
				text = string.Format(dinfo.Value.Def.deathMessage, this.pawn.LabelShort.CapitalizeFirst());
			}
			else if (hediff != null)
			{
				text = "PawnDiedBecauseOf".Translate(new object[]
				{
					this.pawn.LabelShort.CapitalizeFirst(),
					hediff.def.LabelCap
				});
			}
			else
			{
				text = "PawnDied".Translate(new object[]
				{
					this.pawn.LabelShort.CapitalizeFirst()
				});
			}
			text = text.AdjustedFor(this.pawn);
			if (this.pawn.Faction == Faction.OfPlayer)
			{
				string label = "Death".Translate();
				this.pawn.relations.CheckAppendBondedAnimalDiedInfo(ref text, ref label);
				Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.Death, this.pawn, null, null);
			}
			else
			{
				Messages.Message(text, this.pawn, MessageTypeDefOf.PawnDeath, true);
			}
		}

		// Token: 0x06004C6C RID: 19564 RVA: 0x0027CFE8 File Offset: 0x0027B3E8
		public void Notify_Resurrected()
		{
			this.healthState = PawnHealthState.Mobile;
			this.hediffSet.hediffs.RemoveAll((Hediff x) => x.def.everCurableByItem && x.TryGetComp<HediffComp_Immunizable>() != null);
			this.hediffSet.hediffs.RemoveAll((Hediff x) => x.def.everCurableByItem && x is Hediff_Injury && !x.IsPermanent());
			this.hediffSet.hediffs.RemoveAll(delegate(Hediff x)
			{
				bool result;
				if (x.def.everCurableByItem)
				{
					if (x.def.lethalSeverity < 0f)
					{
						if (x.def.stages != null)
						{
							result = x.def.stages.Any((HediffStage y) => y.lifeThreatening);
						}
						else
						{
							result = false;
						}
					}
					else
					{
						result = true;
					}
				}
				else
				{
					result = false;
				}
				return result;
			});
			this.hediffSet.hediffs.RemoveAll((Hediff x) => x.def.everCurableByItem && x is Hediff_Injury && x.IsPermanent() && this.hediffSet.GetPartHealth(x.Part) <= 0f);
			for (;;)
			{
				Hediff_MissingPart hediff_MissingPart = (from x in this.hediffSet.GetMissingPartsCommonAncestors()
				where !this.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(x.Part)
				select x).FirstOrDefault<Hediff_MissingPart>();
				if (hediff_MissingPart == null)
				{
					break;
				}
				this.RestorePart(hediff_MissingPart.Part, null, false);
			}
			this.hediffSet.DirtyCache();
			if (this.ShouldBeDead())
			{
				this.hediffSet.hediffs.Clear();
			}
			this.Notify_HediffChanged(null);
		}

		// Token: 0x06004C6D RID: 19565 RVA: 0x0027D114 File Offset: 0x0027B514
		public void HealthTick()
		{
			if (!this.Dead)
			{
				for (int i = this.hediffSet.hediffs.Count - 1; i >= 0; i--)
				{
					Hediff hediff = this.hediffSet.hediffs[i];
					try
					{
						hediff.Tick();
						hediff.PostTick();
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Exception ticking hediff ",
							hediff.ToStringSafe<Hediff>(),
							" for pawn ",
							this.pawn.ToStringSafe<Pawn>(),
							". Removing hediff... Exception: ",
							ex
						}), false);
						try
						{
							this.RemoveHediff(hediff);
						}
						catch (Exception arg)
						{
							Log.Error("Error while removing hediff: " + arg, false);
						}
					}
				}
				bool flag = false;
				for (int j = this.hediffSet.hediffs.Count - 1; j >= 0; j--)
				{
					Hediff hediff2 = this.hediffSet.hediffs[j];
					if (hediff2.ShouldRemove)
					{
						this.hediffSet.hediffs.RemoveAt(j);
						hediff2.PostRemoved();
						flag = true;
					}
				}
				if (flag)
				{
					this.Notify_HediffChanged(null);
				}
				if (!this.Dead)
				{
					this.immunity.ImmunityHandlerTick();
					if (this.pawn.RaceProps.IsFlesh && this.pawn.IsHashIntervalTick(600) && (this.pawn.needs.food == null || !this.pawn.needs.food.Starving))
					{
						bool flag2 = false;
						if (this.hediffSet.HasNaturallyHealingInjury())
						{
							float num = 8f;
							if (this.pawn.GetPosture() != PawnPosture.Standing)
							{
								num += 4f;
								Building_Bed building_Bed = this.pawn.CurrentBed();
								if (building_Bed != null)
								{
									num += building_Bed.def.building.bed_healPerDay;
								}
							}
							Hediff_Injury hediff_Injury = (from x in this.hediffSet.GetHediffs<Hediff_Injury>()
							where x.CanHealNaturally()
							select x).RandomElement<Hediff_Injury>();
							hediff_Injury.Heal(num * this.pawn.HealthScale * 0.01f);
							flag2 = true;
						}
						if (this.hediffSet.HasTendedAndHealingInjury() && (this.pawn.needs.food == null || !this.pawn.needs.food.Starving))
						{
							Hediff_Injury hediff_Injury2 = (from x in this.hediffSet.GetHediffs<Hediff_Injury>()
							where x.CanHealFromTending()
							select x).RandomElement<Hediff_Injury>();
							float tendQuality = hediff_Injury2.TryGetComp<HediffComp_TendDuration>().tendQuality;
							float num2 = GenMath.LerpDouble(0f, 1f, 0.5f, 1.5f, Mathf.Clamp01(tendQuality));
							hediff_Injury2.Heal(22f * num2 * this.pawn.HealthScale * 0.01f);
							flag2 = true;
						}
						if (flag2 && !this.HasHediffsNeedingTendByPlayer(false) && !HealthAIUtility.ShouldSeekMedicalRest(this.pawn) && !this.hediffSet.HasTendedAndHealingInjury() && PawnUtility.ShouldSendNotificationAbout(this.pawn))
						{
							Messages.Message("MessageFullyHealed".Translate(new object[]
							{
								this.pawn.LabelCap
							}), this.pawn, MessageTypeDefOf.PositiveEvent, true);
						}
					}
					if (this.pawn.RaceProps.IsFlesh && this.hediffSet.BleedRateTotal >= 0.1f)
					{
						float num3 = this.hediffSet.BleedRateTotal * this.pawn.BodySize;
						if (this.pawn.GetPosture() == PawnPosture.Standing)
						{
							num3 *= 0.004f;
						}
						else
						{
							num3 *= 0.0004f;
						}
						if (Rand.Value < num3)
						{
							this.DropBloodFilth();
						}
					}
					if (this.pawn.IsHashIntervalTick(60))
					{
						List<HediffGiverSetDef> hediffGiverSets = this.pawn.RaceProps.hediffGiverSets;
						if (hediffGiverSets != null)
						{
							for (int k = 0; k < hediffGiverSets.Count; k++)
							{
								List<HediffGiver> hediffGivers = hediffGiverSets[k].hediffGivers;
								for (int l = 0; l < hediffGivers.Count; l++)
								{
									hediffGivers[l].OnIntervalPassed(this.pawn, null);
									if (this.pawn.Dead)
									{
										return;
									}
								}
							}
						}
						if (this.pawn.story != null)
						{
							List<Trait> allTraits = this.pawn.story.traits.allTraits;
							for (int m = 0; m < allTraits.Count; m++)
							{
								TraitDegreeData currentData = allTraits[m].CurrentData;
								if (currentData.randomDiseaseMtbDays > 0f && Rand.MTBEventOccurs(currentData.randomDiseaseMtbDays, 60000f, 60f))
								{
									BiomeDef biome;
									if (this.pawn.Tile != -1)
									{
										biome = Find.WorldGrid[this.pawn.Tile].biome;
									}
									else
									{
										biome = DefDatabase<BiomeDef>.GetRandom();
									}
									IncidentDef incidentDef = (from d in DefDatabase<IncidentDef>.AllDefs
									where d.category == IncidentCategoryDefOf.DiseaseHuman
									select d).RandomElementByWeightWithFallback((IncidentDef d) => biome.CommonalityOfDisease(d), null);
									if (incidentDef != null)
									{
										((IncidentWorker_Disease)incidentDef.Worker).ApplyToPawns(Gen.YieldSingle<Pawn>(this.pawn));
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06004C6E RID: 19566 RVA: 0x0027D744 File Offset: 0x0027BB44
		public bool HasHediffsNeedingTend(bool forAlert = false)
		{
			return this.hediffSet.HasTendableHediff(forAlert);
		}

		// Token: 0x06004C6F RID: 19567 RVA: 0x0027D768 File Offset: 0x0027BB68
		public bool HasHediffsNeedingTendByPlayer(bool forAlert = false)
		{
			if (this.HasHediffsNeedingTend(forAlert))
			{
				if (this.pawn.NonHumanlikeOrWildMan())
				{
					if (this.pawn.Faction == Faction.OfPlayer)
					{
						return true;
					}
					Building_Bed building_Bed = this.pawn.CurrentBed();
					if (building_Bed != null && building_Bed.Faction == Faction.OfPlayer)
					{
						return true;
					}
				}
				else if ((this.pawn.Faction == Faction.OfPlayer && this.pawn.HostFaction == null) || this.pawn.HostFaction == Faction.OfPlayer)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004C70 RID: 19568 RVA: 0x0027D82C File Offset: 0x0027BC2C
		public void DropBloodFilth()
		{
			if ((this.pawn.Spawned || this.pawn.ParentHolder is Pawn_CarryTracker) && this.pawn.SpawnedOrAnyParentSpawned && this.pawn.RaceProps.BloodDef != null)
			{
				FilthMaker.MakeFilth(this.pawn.PositionHeld, this.pawn.MapHeld, this.pawn.RaceProps.BloodDef, this.pawn.LabelIndefinite(), 1);
			}
		}

		// Token: 0x04003302 RID: 13058
		private Pawn pawn;

		// Token: 0x04003303 RID: 13059
		private PawnHealthState healthState = PawnHealthState.Mobile;

		// Token: 0x04003304 RID: 13060
		[Unsaved]
		public Effecter deflectionEffecter = null;

		// Token: 0x04003305 RID: 13061
		public bool forceIncap = false;

		// Token: 0x04003306 RID: 13062
		public HediffSet hediffSet = null;

		// Token: 0x04003307 RID: 13063
		public PawnCapacitiesHandler capacities = null;

		// Token: 0x04003308 RID: 13064
		public BillStack surgeryBills = null;

		// Token: 0x04003309 RID: 13065
		public SummaryHealthHandler summaryHealth = null;

		// Token: 0x0400330A RID: 13066
		public ImmunityHandler immunity = null;
	}
}
