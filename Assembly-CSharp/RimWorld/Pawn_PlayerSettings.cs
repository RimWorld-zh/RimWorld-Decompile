using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	public class Pawn_PlayerSettings : IExposable
	{
		private Pawn pawn;

		private Area areaAllowedInt;

		public int joinTick = -1;

		private Pawn master;

		public bool followDrafted;

		public bool followFieldwork;

		public bool animalsReleased;

		public MedicalCareCategory medCare = MedicalCareCategory.NoMeds;

		public HostilityResponseMode hostilityResponse = HostilityResponseMode.Flee;

		public bool selfTend;

		public int displayOrder;

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

		public Pawn Master
		{
			get
			{
				return this.master;
			}
			set
			{
				if (this.master == value)
				{
					return;
				}
				if (value != null && !this.pawn.training.HasLearned(TrainableDefOf.Obedience))
				{
					Log.ErrorOnce("Attempted to set master for non-obedient pawn", 73908573, false);
					return;
				}
				bool flag = ThinkNode_ConditionalShouldFollowMaster.ShouldFollowMaster(this.pawn);
				this.master = value;
				if (this.pawn.Spawned && (flag || ThinkNode_ConditionalShouldFollowMaster.ShouldFollowMaster(this.pawn)))
				{
					this.pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true);
				}
			}
		}

		public Area EffectiveAreaRestrictionInPawnCurrentMap
		{
			get
			{
				if (this.areaAllowedInt != null && this.areaAllowedInt.Map != this.pawn.MapHeld)
				{
					return null;
				}
				return this.EffectiveAreaRestriction;
			}
		}

		public Area EffectiveAreaRestriction
		{
			get
			{
				if (!this.RespectsAllowedArea)
				{
					return null;
				}
				return this.areaAllowedInt;
			}
		}

		public Area AreaRestriction
		{
			get
			{
				return this.areaAllowedInt;
			}
			set
			{
				if (this.areaAllowedInt == value)
				{
					return;
				}
				this.areaAllowedInt = value;
				if (this.pawn.Spawned && value != null && value == this.EffectiveAreaRestrictionInPawnCurrentMap && value.TrueCount > 0 && !value[this.pawn.Position])
				{
					this.pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true);
				}
			}
		}

		public bool RespectsAllowedArea
		{
			get
			{
				return this.pawn.GetLord() == null && this.pawn.Faction == Faction.OfPlayer && this.pawn.HostFaction == null;
			}
		}

		public bool RespectsMaster
		{
			get
			{
				return this.Master != null && this.pawn.Faction == Faction.OfPlayer && this.Master.Faction == this.pawn.Faction;
			}
		}

		public Pawn RespectedMaster
		{
			get
			{
				return (!this.RespectsMaster) ? null : this.Master;
			}
		}

		public bool UsesConfigurableHostilityResponse
		{
			get
			{
				return this.pawn.IsColonist && this.pawn.HostFaction == null;
			}
		}

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

		public IEnumerable<Gizmo> GetGizmos()
		{
			if (this.pawn.Drafted)
			{
				int count = 0;
				bool anyCanRelease = false;
				foreach (Pawn pawn in PawnUtility.SpawnedMasteredPawns(this.pawn))
				{
					if (pawn.training.HasLearned(TrainableDefOf.Release))
					{
						anyCanRelease = true;
						if (ThinkNode_ConditionalShouldFollowMaster.ShouldFollowMaster(pawn))
						{
							count++;
						}
					}
				}
				if (anyCanRelease)
				{
					Command_Toggle c = new Command_Toggle();
					c.defaultLabel = "CommandReleaseAnimalsLabel".Translate() + ((count == 0) ? string.Empty : (" (" + count + ")"));
					c.defaultDesc = "CommandReleaseAnimalsDesc".Translate();
					c.icon = TexCommand.ReleaseAnimals;
					c.hotKey = KeyBindingDefOf.Misc7;
					c.isActive = (() => this.animalsReleased);
					c.toggleAction = delegate()
					{
						this.animalsReleased = !this.animalsReleased;
						if (this.animalsReleased)
						{
							foreach (Pawn pawn2 in PawnUtility.SpawnedMasteredPawns(this.pawn))
							{
								if (pawn2.caller != null)
								{
									pawn2.caller.Notify_Released();
								}
								pawn2.jobs.EndCurrentJob(JobCondition.InterruptForced, true);
							}
						}
					};
					if (count == 0)
					{
						c.Disable("CommandReleaseAnimalsFail_NoAnimals".Translate());
					}
					yield return c;
				}
			}
			yield break;
		}

		public void Notify_FactionChanged()
		{
			this.ResetMedicalCare();
			this.areaAllowedInt = null;
		}

		public void Notify_MadePrisoner()
		{
			this.ResetMedicalCare();
		}

		public void ResetMedicalCare()
		{
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				return;
			}
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

		public void Notify_AreaRemoved(Area area)
		{
			if (this.areaAllowedInt == area)
			{
				this.areaAllowedInt = null;
			}
		}

		[CompilerGenerated]
		private sealed class <GetGizmos>c__Iterator0 : IEnumerable, IEnumerable<Gizmo>, IEnumerator, IDisposable, IEnumerator<Gizmo>
		{
			internal int <count>__1;

			internal bool <anyCanRelease>__1;

			internal IEnumerator<Pawn> $locvar0;

			internal Command_Toggle <c>__2;

			internal Pawn_PlayerSettings $this;

			internal Gizmo $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetGizmos>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					if (this.pawn.Drafted)
					{
						count = 0;
						anyCanRelease = false;
						enumerator = PawnUtility.SpawnedMasteredPawns(this.pawn).GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								Pawn pawn = enumerator.Current;
								if (pawn.training.HasLearned(TrainableDefOf.Release))
								{
									anyCanRelease = true;
									if (ThinkNode_ConditionalShouldFollowMaster.ShouldFollowMaster(pawn))
									{
										count++;
									}
								}
							}
						}
						finally
						{
							if (enumerator != null)
							{
								enumerator.Dispose();
							}
						}
						if (anyCanRelease)
						{
							c = new Command_Toggle();
							c.defaultLabel = "CommandReleaseAnimalsLabel".Translate() + ((count == 0) ? string.Empty : (" (" + count + ")"));
							c.defaultDesc = "CommandReleaseAnimalsDesc".Translate();
							c.icon = TexCommand.ReleaseAnimals;
							c.hotKey = KeyBindingDefOf.Misc7;
							c.isActive = (() => this.animalsReleased);
							c.toggleAction = delegate()
							{
								this.animalsReleased = !this.animalsReleased;
								if (this.animalsReleased)
								{
									foreach (Pawn pawn2 in PawnUtility.SpawnedMasteredPawns(this.pawn))
									{
										if (pawn2.caller != null)
										{
											pawn2.caller.Notify_Released();
										}
										pawn2.jobs.EndCurrentJob(JobCondition.InterruptForced, true);
									}
								}
							};
							if (count == 0)
							{
								c.Disable("CommandReleaseAnimalsFail_NoAnimals".Translate());
							}
							this.$current = c;
							if (!this.$disposing)
							{
								this.$PC = 1;
							}
							return true;
						}
					}
					break;
				case 1u:
					break;
				default:
					return false;
				}
				this.$PC = -1;
				return false;
			}

			Gizmo IEnumerator<Gizmo>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.Gizmo>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				Pawn_PlayerSettings.<GetGizmos>c__Iterator0 <GetGizmos>c__Iterator = new Pawn_PlayerSettings.<GetGizmos>c__Iterator0();
				<GetGizmos>c__Iterator.$this = this;
				return <GetGizmos>c__Iterator;
			}

			internal bool <>m__0()
			{
				return this.animalsReleased;
			}

			internal void <>m__1()
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
		}
	}
}
