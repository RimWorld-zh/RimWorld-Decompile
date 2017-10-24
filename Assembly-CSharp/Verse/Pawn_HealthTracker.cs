using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse.AI;
using Verse.AI.Group;

namespace Verse
{
	public class Pawn_HealthTracker : IExposable
	{
		private Pawn pawn;

		private PawnHealthState healthState = PawnHealthState.Mobile;

		[Unsaved]
		public Effecter deflectionEffecter = null;

		public bool forceIncap = false;

		public HediffSet hediffSet = null;

		public PawnCapacitiesHandler capacities = null;

		public BillStack surgeryBills = null;

		public SummaryHealthHandler summaryHealth = null;

		public ImmunityHandler immunity = null;

		public PawnHealthState State
		{
			get
			{
				return this.healthState;
			}
		}

		public bool Downed
		{
			get
			{
				return this.healthState == PawnHealthState.Down;
			}
		}

		public bool Dead
		{
			get
			{
				return this.healthState == PawnHealthState.Dead;
			}
		}

		public bool InPainShock
		{
			get
			{
				return this.hediffSet.PainTotal >= this.pawn.GetStatValue(StatDefOf.PainShockThreshold, true);
			}
		}

		public Pawn_HealthTracker(Pawn pawn)
		{
			this.pawn = pawn;
			this.hediffSet = new HediffSet(pawn);
			this.capacities = new PawnCapacitiesHandler(pawn);
			this.summaryHealth = new SummaryHealthHandler(pawn);
			this.surgeryBills = new BillStack(pawn);
			this.immunity = new ImmunityHandler(pawn);
		}

		public void Reset()
		{
			this.healthState = PawnHealthState.Mobile;
			this.hediffSet.Clear();
			this.capacities.Clear();
			this.summaryHealth.Notify_HealthChanged();
			this.surgeryBills.Clear();
			this.immunity = new ImmunityHandler(this.pawn);
		}

		public void ExposeData()
		{
			Scribe_Values.Look<PawnHealthState>(ref this.healthState, "healthState", PawnHealthState.Mobile, false);
			Scribe_Values.Look<bool>(ref this.forceIncap, "forceIncap", false, false);
			Scribe_Deep.Look<HediffSet>(ref this.hediffSet, "hediffSet", new object[1]
			{
				this.pawn
			});
			Scribe_Deep.Look<BillStack>(ref this.surgeryBills, "surgeryBills", new object[1]
			{
				this.pawn
			});
			Scribe_Deep.Look<ImmunityHandler>(ref this.immunity, "immunity", new object[1]
			{
				this.pawn
			});
		}

		public void AddHediff(HediffDef def, BodyPartRecord part = null, DamageInfo? dinfo = default(DamageInfo?))
		{
			this.AddHediff(HediffMaker.MakeHediff(def, this.pawn, null), part, dinfo);
		}

		public void AddHediff(Hediff hediff, BodyPartRecord part = null, DamageInfo? dinfo = default(DamageInfo?))
		{
			if (part != null)
			{
				hediff.Part = part;
			}
			this.hediffSet.AddDirect(hediff, dinfo);
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

		public void RemoveHediff(Hediff hediff)
		{
			this.hediffSet.hediffs.Remove(hediff);
			hediff.PostRemoved();
			this.Notify_HediffChanged(null);
		}

		public void Notify_HediffChanged(Hediff hediff)
		{
			this.hediffSet.DirtyCache();
			this.CheckForStateChange(default(DamageInfo?), hediff);
		}

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
					GenClamor.DoClamor(this.pawn, 18f, ClamorType.Harm);
				}
				this.pawn.mindState.Notify_DamageTaken(dinfo);
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
				if (dinfo.Def.makesBlood && !dinfo.InstantOldInjury)
				{
					this.TryDropBloodFilth();
				}
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
					else if (this.pawn.RaceProps.Humanlike && pawn2.RaceProps.Humanlike)
					{
						this.pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.HarmedMe, pawn2);
					}
				}
				TaleRecorder.RecordTale(TaleDefOf.Wounded, this.pawn, pawn2, dinfo.Weapon);
			}
			absorbed = false;
		}

		public void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			if (this.ShouldBeDead())
			{
				if (!this.pawn.Destroyed)
				{
					this.pawn.Kill(new DamageInfo?(dinfo), null);
				}
			}
			else
			{
				if (dinfo.Def.additionalHediffs != null)
				{
					List<DamageDefAdditionalHediff> additionalHediffs = dinfo.Def.additionalHediffs;
					for (int i = 0; i < additionalHediffs.Count; i++)
					{
						DamageDefAdditionalHediff damageDefAdditionalHediff = additionalHediffs[i];
						if (damageDefAdditionalHediff.hediff != null)
						{
							float num = totalDamageDealt * damageDefAdditionalHediff.severityPerDamageDealt;
							if (num >= 0.0)
							{
								Hediff hediff = HediffMaker.MakeHediff(damageDefAdditionalHediff.hediff, this.pawn, null);
								hediff.Severity = num;
								this.AddHediff(hediff, null, new DamageInfo?(dinfo));
								if (this.Dead)
									return;
							}
						}
					}
				}
				if (!this.Dead)
				{
					this.pawn.mindState.mentalStateHandler.Notify_DamageTaken(dinfo);
				}
			}
		}

		public void RestorePart(BodyPartRecord part, Hediff diffException = null, bool checkStateChange = true)
		{
			if (part == null)
			{
				Log.Error("Tried to restore null body part.");
			}
			else
			{
				this.RestorePartRecursiveInt(part, diffException);
				this.hediffSet.DirtyCache();
				if (checkStateChange)
				{
					this.CheckForStateChange(default(DamageInfo?), null);
				}
			}
		}

		private void RestorePartRecursiveInt(BodyPartRecord part, Hediff diffException = null)
		{
			List<Hediff> hediffs = this.hediffSet.hediffs;
			for (int num = hediffs.Count - 1; num >= 0; num--)
			{
				Hediff hediff = hediffs[num];
				if (hediff.Part == part && hediff != diffException)
				{
					Hediff hediff2 = hediffs[num];
					hediffs.RemoveAt(num);
					hediff2.PostRemoved();
				}
			}
			for (int i = 0; i < part.parts.Count; i++)
			{
				this.RestorePartRecursiveInt(part.parts[i], diffException);
			}
		}

		private void CheckForStateChange(DamageInfo? dinfo, Hediff hediff)
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
						float num = (float)((!this.pawn.RaceProps.Animal) ? 0.67000001668930054 : 0.4699999988079071);
						if (!this.forceIncap && dinfo.HasValue && dinfo.Value.Def.externalViolence && (this.pawn.Faction == null || !this.pawn.Faction.IsPlayer) && !this.pawn.IsPrisonerOfColony && this.pawn.RaceProps.IsFlesh && Rand.Value < num)
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
						if (this.pawn.carryTracker != null && this.pawn.carryTracker.CarriedThing != null && this.pawn.jobs != null && this.pawn.CurJob != null)
						{
							this.pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true);
						}
						if (this.pawn.equipment != null && this.pawn.equipment.Primary != null)
						{
							if (this.pawn.InContainerEnclosed)
							{
								this.pawn.equipment.TryTransferEquipmentToContainer(this.pawn.equipment.Primary, this.pawn.holdingOwner);
							}
							else if (this.pawn.SpawnedOrAnyParentSpawned)
							{
								ThingWithComps thingWithComps = default(ThingWithComps);
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

		private bool ShouldBeDowned()
		{
			return this.InPainShock || !this.capacities.CanBeAwake || !this.capacities.CapableOf(PawnCapacityDefOf.Moving);
		}

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
						goto IL_0036;
				}
				if (this.ShouldBeDeadFromRequiredCapacity() != null)
				{
					result = true;
				}
				else
				{
					float num = PawnCapacityUtility.CalculatePartEfficiency(this.hediffSet, this.pawn.RaceProps.body.corePart, false, null);
					result = ((byte)((num <= 9.9999997473787516E-05) ? 1 : 0) != 0);
				}
			}
			goto IL_00a6;
			IL_0036:
			result = true;
			goto IL_00a6;
			IL_00a6:
			return result;
		}

		public PawnCapacityDef ShouldBeDeadFromRequiredCapacity()
		{
			List<PawnCapacityDef> allDefsListForReading = DefDatabase<PawnCapacityDef>.AllDefsListForReading;
			int num = 0;
			PawnCapacityDef result;
			while (true)
			{
				if (num < allDefsListForReading.Count)
				{
					PawnCapacityDef pawnCapacityDef = allDefsListForReading[num];
					if (((!this.pawn.RaceProps.IsFlesh) ? pawnCapacityDef.lethalMechanoids : pawnCapacityDef.lethalFlesh) && !this.capacities.CapableOf(pawnCapacityDef))
					{
						result = pawnCapacityDef;
						break;
					}
					num++;
					continue;
				}
				result = null;
				break;
			}
			return result;
		}

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

		public bool WouldDieAfterAddingHediff(HediffDef def, BodyPartRecord part, float severity)
		{
			Hediff hediff = HediffMaker.MakeHediff(def, this.pawn, part);
			hediff.Severity = severity;
			return this.WouldDieAfterAddingHediff(hediff);
		}

		public void SetDead()
		{
			if (this.Dead)
			{
				Log.Error(this.pawn + " set dead while already dead.");
			}
			this.healthState = PawnHealthState.Dead;
		}

		private void MakeDowned(DamageInfo? dinfo, Hediff hediff)
		{
			if (this.Downed)
			{
				Log.Error(this.pawn + " tried to do MakeDowned while already downed.");
			}
			else
			{
				this.healthState = PawnHealthState.Down;
				PawnDiedOrDownedThoughtsUtility.TryGiveThoughts(this.pawn, dinfo, PawnDiedOrDownedThoughtsKind.Downed);
				if (this.pawn.MentalState != null)
				{
					this.pawn.mindState.mentalStateHandler.CurState.RecoverFromState();
				}
				if (this.pawn.Spawned)
				{
					if (this.pawn.IsColonist && dinfo.HasValue && dinfo.Value.Def.externalViolence)
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
				if (dinfo.HasValue && dinfo.Value.Instigator != null)
				{
					Pawn pawn = dinfo.Value.Instigator as Pawn;
					if (pawn != null)
					{
						RecordsUtility.Notify_PawnDowned(this.pawn, pawn);
					}
				}
				if (this.pawn.Spawned)
				{
					TaleRecorder.RecordTale(TaleDefOf.Downed, this.pawn, (!dinfo.HasValue) ? null : (dinfo.Value.Instigator as Pawn), (!dinfo.HasValue) ? null : dinfo.Value.Weapon);
					Find.BattleLog.Add(new BattleLogEntry_StateTransition(this.pawn, RulePackDefOf.Transition_Downed, (!dinfo.HasValue) ? null : (dinfo.Value.Instigator as Pawn), hediff, (!dinfo.HasValue) ? null : dinfo.Value.HitPart.def));
				}
			}
		}

		private void MakeUndowned()
		{
			if (!this.Downed)
			{
				Log.Error(this.pawn + " tried to do MakeUndowned when already undowned.");
			}
			else
			{
				this.healthState = PawnHealthState.Mobile;
				if (PawnUtility.ShouldSendNotificationAbout(this.pawn))
				{
					Messages.Message("MessageNoLongerDowned".Translate(this.pawn.LabelCap), (Thing)this.pawn, MessageTypeDefOf.PositiveEvent);
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

		public void NotifyPlayerOfKilled(DamageInfo? dinfo, Hediff hediff, Caravan caravan)
		{
			string text = (string)null;
			if (dinfo.HasValue)
			{
				text = string.Format(dinfo.Value.Def.deathMessage, this.pawn.NameStringShort.CapitalizeFirst());
			}
			else if (hediff != null)
			{
				text = "PawnDiedBecauseOf".Translate(this.pawn.NameStringShort.CapitalizeFirst(), hediff.def.label);
			}
			if (!text.NullOrEmpty())
			{
				text = text.AdjustedFor(this.pawn);
				GlobalTargetInfo lookTarget = (caravan == null) ? ((Thing)this.pawn) : ((WorldObject)caravan);
				Messages.Message(text, lookTarget, MessageTypeDefOf.PawnDeath);
			}
		}

		public void Notify_Resurrected()
		{
			this.healthState = PawnHealthState.Mobile;
			this.hediffSet.hediffs.RemoveAll((Predicate<Hediff>)((Hediff x) => x.TryGetComp<HediffComp_Immunizable>() != null));
			this.hediffSet.hediffs.RemoveAll((Predicate<Hediff>)((Hediff x) => x is Hediff_Injury && !x.IsOld()));
			this.hediffSet.hediffs.RemoveAll((Predicate<Hediff>)((Hediff x) => x.def.lethalSeverity >= 0.0 || (x.def.stages != null && x.def.stages.Any((Predicate<HediffStage>)((HediffStage y) => y.lifeThreatening)))));
			this.hediffSet.hediffs.RemoveAll((Predicate<Hediff>)((Hediff x) => x is Hediff_Injury && x.IsOld() && this.hediffSet.GetPartHealth(x.Part) <= 0.0));
			while (true)
			{
				Hediff_MissingPart hediff_MissingPart = (from x in this.hediffSet.GetMissingPartsCommonAncestors()
				where !this.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(x.Part)
				select x).FirstOrDefault();
				if (hediff_MissingPart != null)
				{
					this.RestorePart(hediff_MissingPart.Part, null, false);
					continue;
				}
				break;
			}
			this.hediffSet.DirtyCache();
			if (this.ShouldBeDead())
			{
				this.hediffSet.hediffs.Clear();
			}
			this.Notify_HediffChanged(null);
		}

		public void HealthTick()
		{
			if (!this.Dead)
			{
				for (int num = this.hediffSet.hediffs.Count - 1; num >= 0; num--)
				{
					Hediff hediff = this.hediffSet.hediffs[num];
					hediff.Tick();
					hediff.PostTick();
				}
				bool flag = false;
				for (int num2 = this.hediffSet.hediffs.Count - 1; num2 >= 0; num2--)
				{
					Hediff hediff2 = this.hediffSet.hediffs[num2];
					if (hediff2.ShouldRemove)
					{
						this.hediffSet.hediffs.RemoveAt(num2);
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
							float num3 = 8f;
							if (this.pawn.GetPosture() != 0)
							{
								num3 = (float)(num3 + 4.0);
								Building_Bed building_Bed = this.pawn.CurrentBed();
								if (building_Bed != null)
								{
									num3 += building_Bed.def.building.bed_healPerDay;
								}
							}
							Hediff_Injury hediff_Injury = (from x in this.hediffSet.GetHediffs<Hediff_Injury>()
							where x.CanHealNaturally()
							select x).RandomElement();
							hediff_Injury.Heal((float)(num3 * this.pawn.HealthScale * 0.0099999997764825821));
							flag2 = true;
						}
						if (this.hediffSet.HasTendedAndHealingInjury() && (this.pawn.needs.food == null || !this.pawn.needs.food.Starving))
						{
							Hediff_Injury hediff_Injury2 = (from x in this.hediffSet.GetHediffs<Hediff_Injury>()
							where x.CanHealFromTending()
							select x).RandomElement();
							float tendQuality = hediff_Injury2.TryGetComp<HediffComp_TendDuration>().tendQuality;
							float num4 = GenMath.LerpDouble(0f, 1f, 0.5f, 1.5f, Mathf.Clamp01(tendQuality));
							hediff_Injury2.Heal((float)(22.0 * num4 * this.pawn.HealthScale * 0.0099999997764825821));
							flag2 = true;
						}
						if (flag2 && !this.HasHediffsNeedingTendByColony(false) && !HealthAIUtility.ShouldSeekMedicalRest(this.pawn) && !this.hediffSet.HasTendedAndHealingInjury() && PawnUtility.ShouldSendNotificationAbout(this.pawn))
						{
							Messages.Message("MessageFullyHealed".Translate(this.pawn.LabelCap), (Thing)this.pawn, MessageTypeDefOf.PositiveEvent);
						}
					}
					if (this.pawn.RaceProps.IsFlesh && this.hediffSet.BleedRateTotal >= 0.10000000149011612)
					{
						float num5 = this.hediffSet.BleedRateTotal * this.pawn.BodySize;
						num5 = (float)((this.pawn.GetPosture() != 0) ? (num5 * 0.00079999997979030013) : (num5 * 0.00800000037997961));
						if (Rand.Value < num5)
						{
							this.TryDropBloodFilth();
						}
					}
					List<HediffGiverSetDef> hediffGiverSets = this.pawn.RaceProps.hediffGiverSets;
					if (hediffGiverSets != null && this.pawn.IsHashIntervalTick(60))
					{
						for (int i = 0; i < hediffGiverSets.Count; i++)
						{
							List<HediffGiver> hediffGivers = hediffGiverSets[i].hediffGivers;
							int num6 = 0;
							while (num6 < hediffGivers.Count)
							{
								hediffGivers[num6].OnIntervalPassed(this.pawn, null);
								if (!this.pawn.Dead)
								{
									num6++;
									continue;
								}
								return;
							}
						}
					}
				}
			}
		}

		public bool HasHediffsNeedingTend(bool forAlert = false)
		{
			return this.hediffSet.HasTendableHediff(forAlert);
		}

		public bool HasHediffsNeedingTendByColony(bool forAlert = false)
		{
			bool result;
			if (this.HasHediffsNeedingTend(forAlert))
			{
				if (!this.pawn.RaceProps.Humanlike)
				{
					if (this.pawn.Faction == Faction.OfPlayer)
					{
						result = true;
						goto IL_00ba;
					}
					Building_Bed building_Bed = this.pawn.CurrentBed();
					if (building_Bed != null && building_Bed.Faction == Faction.OfPlayer)
					{
						result = true;
						goto IL_00ba;
					}
				}
				else
				{
					if (this.pawn.Faction == Faction.OfPlayer && this.pawn.HostFaction == null)
					{
						goto IL_00aa;
					}
					if (this.pawn.HostFaction == Faction.OfPlayer)
						goto IL_00aa;
				}
			}
			result = false;
			goto IL_00ba;
			IL_00ba:
			return result;
			IL_00aa:
			result = true;
			goto IL_00ba;
		}

		protected void TryDropBloodFilth()
		{
			if (Rand.Value < 0.5)
			{
				this.DropBloodFilth();
			}
		}

		public void DropBloodFilth()
		{
			if (!this.pawn.Spawned && !(this.pawn.ParentHolder is Pawn_CarryTracker))
				return;
			if (this.pawn.SpawnedOrAnyParentSpawned && this.pawn.RaceProps.BloodDef != null)
			{
				FilthMaker.MakeFilth(this.pawn.PositionHeld, this.pawn.MapHeld, this.pawn.RaceProps.BloodDef, this.pawn.LabelIndefinite(), 1);
			}
		}
	}
}
