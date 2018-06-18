using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000523 RID: 1315
	public class Pawn_PlayerSettings : IExposable
	{
		// Token: 0x060017F3 RID: 6131 RVA: 0x000D1340 File Offset: 0x000CF740
		public Pawn_PlayerSettings(Pawn pawn)
		{
			this.pawn = pawn;
			if (Current.ProgramState == ProgramState.Playing)
			{
				this.joinTick = Find.TickManager.TicksGame;
			}
			else
			{
				this.joinTick = 0;
			}
			this.Notify_FactionChanged();
		}

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x060017F4 RID: 6132 RVA: 0x000D13C8 File Offset: 0x000CF7C8
		// (set) Token: 0x060017F5 RID: 6133 RVA: 0x000D13E4 File Offset: 0x000CF7E4
		public Pawn Master
		{
			get
			{
				return this.master;
			}
			set
			{
				if (this.master != value)
				{
					if (value != null && !this.pawn.training.HasLearned(TrainableDefOf.Obedience))
					{
						Log.ErrorOnce("Attempted to set master for non-obedient pawn", 73908573, false);
					}
					else
					{
						bool flag = ThinkNode_ConditionalShouldFollowMaster.ShouldFollowMaster(this.pawn);
						this.master = value;
						if (this.pawn.Spawned && (flag || ThinkNode_ConditionalShouldFollowMaster.ShouldFollowMaster(this.pawn)))
						{
							this.pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true);
						}
					}
				}
			}
		}

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x060017F6 RID: 6134 RVA: 0x000D1484 File Offset: 0x000CF884
		public Area EffectiveAreaRestrictionInPawnCurrentMap
		{
			get
			{
				Area result;
				if (this.areaAllowedInt != null && this.areaAllowedInt.Map != this.pawn.MapHeld)
				{
					result = null;
				}
				else
				{
					result = this.EffectiveAreaRestriction;
				}
				return result;
			}
		}

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x060017F7 RID: 6135 RVA: 0x000D14CC File Offset: 0x000CF8CC
		public Area EffectiveAreaRestriction
		{
			get
			{
				Area result;
				if (!this.RespectsAllowedArea)
				{
					result = null;
				}
				else
				{
					result = this.areaAllowedInt;
				}
				return result;
			}
		}

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x060017F8 RID: 6136 RVA: 0x000D14FC File Offset: 0x000CF8FC
		// (set) Token: 0x060017F9 RID: 6137 RVA: 0x000D1517 File Offset: 0x000CF917
		public Area AreaRestriction
		{
			get
			{
				return this.areaAllowedInt;
			}
			set
			{
				this.areaAllowedInt = value;
			}
		}

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x060017FA RID: 6138 RVA: 0x000D1524 File Offset: 0x000CF924
		public bool RespectsAllowedArea
		{
			get
			{
				return this.pawn.GetLord() == null && this.pawn.Faction == Faction.OfPlayer && this.pawn.HostFaction == null;
			}
		}

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x060017FB RID: 6139 RVA: 0x000D1578 File Offset: 0x000CF978
		public bool RespectsMaster
		{
			get
			{
				return this.Master != null && this.pawn.Faction == Faction.OfPlayer && this.Master.Faction == this.pawn.Faction;
			}
		}

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x060017FC RID: 6140 RVA: 0x000D15D0 File Offset: 0x000CF9D0
		public Pawn RespectedMaster
		{
			get
			{
				return (!this.RespectsMaster) ? null : this.Master;
			}
		}

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x060017FD RID: 6141 RVA: 0x000D15FC File Offset: 0x000CF9FC
		public bool UsesConfigurableHostilityResponse
		{
			get
			{
				return this.pawn.IsColonist && this.pawn.HostFaction == null;
			}
		}

		// Token: 0x060017FE RID: 6142 RVA: 0x000D1634 File Offset: 0x000CFA34
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.joinTick, "joinTick", 0, false);
			Scribe_Values.Look<bool>(ref this.animalsReleased, "animalsReleased", false, false);
			Scribe_Values.Look<MedicalCareCategory>(ref this.medCare, "medCare", MedicalCareCategory.NoCare, false);
			Scribe_References.Look<Area>(ref this.areaAllowedInt, "areaAllowed", false);
			Scribe_References.Look<Pawn>(ref this.master, "master", false);
			Scribe_Values.Look<bool>(ref this.followDrafted, "followDrafted", false, false);
			Scribe_Values.Look<bool>(ref this.followFieldwork, "followFieldwork", false, false);
			Scribe_Values.Look<HostilityResponseMode>(ref this.hostilityResponse, "hostilityResponse", HostilityResponseMode.Flee, false);
			Scribe_Values.Look<bool>(ref this.selfTend, "selfTend", false, false);
			Scribe_Values.Look<int>(ref this.displayOrder, "displayOrder", 0, false);
		}

		// Token: 0x060017FF RID: 6143 RVA: 0x000D16F4 File Offset: 0x000CFAF4
		public IEnumerable<Gizmo> GetGizmos()
		{
			if (this.pawn.Drafted)
			{
				if (PawnUtility.SpawnedMasteredPawns(this.pawn).Any((Pawn p) => p.training.HasLearned(TrainableDefOf.Release)))
				{
					yield return new Command_Toggle
					{
						defaultLabel = "CommandReleaseAnimalsLabel".Translate(),
						defaultDesc = "CommandReleaseAnimalsDesc".Translate(),
						icon = TexCommand.ReleaseAnimals,
						hotKey = KeyBindingDefOf.Misc7,
						isActive = (() => this.animalsReleased),
						toggleAction = delegate()
						{
							this.animalsReleased = !this.animalsReleased;
							if (this.animalsReleased)
							{
								foreach (Pawn pawn in PawnUtility.SpawnedMasteredPawns(this.pawn))
								{
									if (pawn.caller != null)
									{
										pawn.caller.Notify_Released();
									}
									pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true);
								}
							}
						}
					};
				}
			}
			yield break;
		}

		// Token: 0x06001800 RID: 6144 RVA: 0x000D171E File Offset: 0x000CFB1E
		public void Notify_FactionChanged()
		{
			this.ResetMedicalCare();
		}

		// Token: 0x06001801 RID: 6145 RVA: 0x000D1727 File Offset: 0x000CFB27
		public void Notify_MadePrisoner()
		{
			this.ResetMedicalCare();
		}

		// Token: 0x06001802 RID: 6146 RVA: 0x000D1730 File Offset: 0x000CFB30
		public void ResetMedicalCare()
		{
			if (Scribe.mode != LoadSaveMode.LoadingVars)
			{
				if (this.pawn.Faction == Faction.OfPlayer)
				{
					if (!this.pawn.RaceProps.Animal)
					{
						if (!this.pawn.IsPrisoner)
						{
							this.medCare = Find.PlaySettings.defaultCareForColonyHumanlike;
						}
						else
						{
							this.medCare = Find.PlaySettings.defaultCareForColonyPrisoner;
						}
					}
					else
					{
						this.medCare = Find.PlaySettings.defaultCareForColonyAnimal;
					}
				}
				else if (this.pawn.Faction == null && this.pawn.RaceProps.Animal)
				{
					this.medCare = Find.PlaySettings.defaultCareForNeutralAnimal;
				}
				else if (this.pawn.Faction == null || !this.pawn.Faction.HostileTo(Faction.OfPlayer))
				{
					this.medCare = Find.PlaySettings.defaultCareForNeutralFaction;
				}
				else
				{
					this.medCare = Find.PlaySettings.defaultCareForHostileFaction;
				}
			}
		}

		// Token: 0x06001803 RID: 6147 RVA: 0x000D1857 File Offset: 0x000CFC57
		public void Notify_AreaRemoved(Area area)
		{
			if (this.areaAllowedInt == area)
			{
				this.areaAllowedInt = null;
			}
		}

		// Token: 0x04000E23 RID: 3619
		private Pawn pawn;

		// Token: 0x04000E24 RID: 3620
		private Area areaAllowedInt = null;

		// Token: 0x04000E25 RID: 3621
		public int joinTick = -1;

		// Token: 0x04000E26 RID: 3622
		private Pawn master = null;

		// Token: 0x04000E27 RID: 3623
		public bool followDrafted = true;

		// Token: 0x04000E28 RID: 3624
		public bool followFieldwork = true;

		// Token: 0x04000E29 RID: 3625
		public bool animalsReleased = false;

		// Token: 0x04000E2A RID: 3626
		public MedicalCareCategory medCare = MedicalCareCategory.NoMeds;

		// Token: 0x04000E2B RID: 3627
		public HostilityResponseMode hostilityResponse = HostilityResponseMode.Flee;

		// Token: 0x04000E2C RID: 3628
		public bool selfTend = false;

		// Token: 0x04000E2D RID: 3629
		public int displayOrder;
	}
}
