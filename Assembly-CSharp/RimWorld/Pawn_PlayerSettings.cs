using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

		private Area areaAllowedInt = null;

		public int joinTick = -1;

		private Pawn master = null;

		public bool followDrafted = true;

		public bool followFieldwork = true;

		public bool animalsReleased = false;

		public MedicalCareCategory medCare = MedicalCareCategory.NoMeds;

		public HostilityResponseMode hostilityResponse = HostilityResponseMode.Flee;

		public bool selfTend = false;

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

		public void Notify_FactionChanged()
		{
			this.ResetMedicalCare();
		}

		public void Notify_MadePrisoner()
		{
			this.ResetMedicalCare();
		}

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
			internal Command_Toggle <c>__1;

			internal Pawn_PlayerSettings $this;

			internal Gizmo $current;

			internal bool $disposing;

			internal int $PC;

			private static Func<Pawn, bool> <>f__am$cache0;

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
						if (PawnUtility.SpawnedMasteredPawns(this.pawn).Any((Pawn p) => p.training.HasLearned(TrainableDefOf.Release)))
						{
							Command_Toggle c = new Command_Toggle();
							c.defaultLabel = "CommandReleaseAnimalsLabel".Translate();
							c.defaultDesc = "CommandReleaseAnimalsDesc".Translate();
							c.icon = TexCommand.ReleaseAnimals;
							c.hotKey = KeyBindingDefOf.Misc7;
							c.isActive = (() => this.animalsReleased);
							c.toggleAction = delegate()
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
							};
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

			private static bool <>m__0(Pawn p)
			{
				return p.training.HasLearned(TrainableDefOf.Release);
			}

			internal bool <>m__1()
			{
				return this.animalsReleased;
			}

			internal void <>m__2()
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
