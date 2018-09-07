using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	public class Building_CryptosleepCasket : Building_Casket
	{
		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache0;

		public Building_CryptosleepCasket()
		{
		}

		public override bool TryAcceptThing(Thing thing, bool allowSpecialEffects = true)
		{
			if (base.TryAcceptThing(thing, allowSpecialEffects))
			{
				if (allowSpecialEffects)
				{
					SoundDefOf.CryptosleepCasket_Accept.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
				}
				return true;
			}
			return false;
		}

		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn myPawn)
		{
			foreach (FloatMenuOption o in base.GetFloatMenuOptions(myPawn))
			{
				yield return o;
			}
			if (this.innerContainer.Count == 0)
			{
				if (!myPawn.CanReach(this, PathEndMode.InteractionCell, Danger.Deadly, false, TraverseMode.ByPawn))
				{
					FloatMenuOption failer = new FloatMenuOption("CannotUseNoPath".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
					yield return failer;
				}
				else
				{
					JobDef jobDef = JobDefOf.EnterCryptosleepCasket;
					string jobStr = "EnterCryptosleepCasket".Translate();
					Action jobAction = delegate()
					{
						Job job = new Job(jobDef, this.$this);
						myPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
					};
					yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(jobStr, jobAction, MenuOptionPriority.Default, null, null, 0f, null, null), myPawn, this, "ReservedBy");
				}
			}
			yield break;
		}

		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo c in base.GetGizmos())
			{
				yield return c;
			}
			if (base.Faction == Faction.OfPlayer && this.innerContainer.Count > 0 && this.def.building.isPlayerEjectable)
			{
				Command_Action eject = new Command_Action();
				eject.action = new Action(this.EjectContents);
				eject.defaultLabel = "CommandPodEject".Translate();
				eject.defaultDesc = "CommandPodEjectDesc".Translate();
				if (this.innerContainer.Count == 0)
				{
					eject.Disable("CommandPodEjectFailEmpty".Translate());
				}
				eject.hotKey = KeyBindingDefOf.Misc1;
				eject.icon = ContentFinder<Texture2D>.Get("UI/Commands/PodEject", true);
				yield return eject;
			}
			yield break;
		}

		public override void EjectContents()
		{
			ThingDef filth_Slime = ThingDefOf.Filth_Slime;
			foreach (Thing thing in ((IEnumerable<Thing>)this.innerContainer))
			{
				Pawn pawn = thing as Pawn;
				if (pawn != null)
				{
					PawnComponentsUtility.AddComponentsForSpawn(pawn);
					pawn.filth.GainFilth(filth_Slime);
					if (pawn.RaceProps.IsFlesh)
					{
						pawn.health.AddHediff(HediffDefOf.CryptosleepSickness, null, null, null);
					}
				}
			}
			if (!base.Destroyed)
			{
				SoundDefOf.CryptosleepCasket_Eject.PlayOneShot(SoundInfo.InMap(new TargetInfo(base.Position, base.Map, false), MaintenanceType.None));
			}
			base.EjectContents();
		}

		public static Building_CryptosleepCasket FindCryptosleepCasketFor(Pawn p, Pawn traveler, bool ignoreOtherReservations = false)
		{
			IEnumerable<ThingDef> enumerable = from def in DefDatabase<ThingDef>.AllDefs
			where typeof(Building_CryptosleepCasket).IsAssignableFrom(def.thingClass)
			select def;
			foreach (ThingDef singleDef in enumerable)
			{
				Building_CryptosleepCasket building_CryptosleepCasket = (Building_CryptosleepCasket)GenClosest.ClosestThingReachable(p.Position, p.Map, ThingRequest.ForDef(singleDef), PathEndMode.InteractionCell, TraverseParms.For(traveler, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, delegate(Thing x)
				{
					bool result;
					if (!((Building_CryptosleepCasket)x).HasAnyContents)
					{
						Pawn traveler2 = traveler;
						LocalTargetInfo target = x;
						bool ignoreOtherReservations2 = ignoreOtherReservations;
						result = traveler2.CanReserve(target, 1, -1, null, ignoreOtherReservations2);
					}
					else
					{
						result = false;
					}
					return result;
				}, null, 0, -1, false, RegionType.Set_Passable, false);
				if (building_CryptosleepCasket != null)
				{
					return building_CryptosleepCasket;
				}
			}
			return null;
		}

		[DebuggerHidden]
		[CompilerGenerated]
		private IEnumerable<FloatMenuOption> <GetFloatMenuOptions>__BaseCallProxy0(Pawn selPawn)
		{
			return base.GetFloatMenuOptions(selPawn);
		}

		[DebuggerHidden]
		[CompilerGenerated]
		private IEnumerable<Gizmo> <GetGizmos>__BaseCallProxy1()
		{
			return base.GetGizmos();
		}

		[CompilerGenerated]
		private static bool <FindCryptosleepCasketFor>m__0(ThingDef def)
		{
			return typeof(Building_CryptosleepCasket).IsAssignableFrom(def.thingClass);
		}

		[CompilerGenerated]
		private sealed class <GetFloatMenuOptions>c__Iterator0 : IEnumerable, IEnumerable<FloatMenuOption>, IEnumerator, IDisposable, IEnumerator<FloatMenuOption>
		{
			internal Pawn myPawn;

			internal IEnumerator<FloatMenuOption> $locvar0;

			internal FloatMenuOption <o>__1;

			internal FloatMenuOption <failer>__2;

			internal string <jobStr>__3;

			internal Action <jobAction>__3;

			internal Building_CryptosleepCasket $this;

			internal FloatMenuOption $current;

			internal bool $disposing;

			internal int $PC;

			private Building_CryptosleepCasket.<GetFloatMenuOptions>c__Iterator0.<GetFloatMenuOptions>c__AnonStorey3 $locvar1;

			private Building_CryptosleepCasket.<GetFloatMenuOptions>c__Iterator0.<GetFloatMenuOptions>c__AnonStorey2 $locvar2;

			[DebuggerHidden]
			public <GetFloatMenuOptions>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					enumerator = base.<GetFloatMenuOptions>__BaseCallProxy0(myPawn).GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					goto IL_21D;
				case 3u:
					goto IL_21D;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						o = enumerator.Current;
						this.$current = o;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				if (this.innerContainer.Count != 0)
				{
					goto IL_21D;
				}
				if (!<GetFloatMenuOptions>c__AnonStorey.myPawn.CanReach(this, PathEndMode.InteractionCell, Danger.Deadly, false, TraverseMode.ByPawn))
				{
					failer = new FloatMenuOption("CannotUseNoPath".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
					this.$current = failer;
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
				}
				else
				{
					JobDef jobDef = JobDefOf.EnterCryptosleepCasket;
					jobStr = "EnterCryptosleepCasket".Translate();
					jobAction = delegate()
					{
						Job job = new Job(jobDef, this.$this);
						<GetFloatMenuOptions>c__AnonStorey.myPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
					};
					this.$current = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(jobStr, jobAction, MenuOptionPriority.Default, null, null, 0f, null, null), <GetFloatMenuOptions>c__AnonStorey.myPawn, this, "ReservedBy");
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
				}
				return true;
				IL_21D:
				this.$PC = -1;
				return false;
			}

			FloatMenuOption IEnumerator<FloatMenuOption>.Current
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
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.FloatMenuOption>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				Building_CryptosleepCasket.<GetFloatMenuOptions>c__Iterator0 <GetFloatMenuOptions>c__Iterator = new Building_CryptosleepCasket.<GetFloatMenuOptions>c__Iterator0();
				<GetFloatMenuOptions>c__Iterator.$this = this;
				<GetFloatMenuOptions>c__Iterator.myPawn = myPawn;
				return <GetFloatMenuOptions>c__Iterator;
			}

			private sealed class <GetFloatMenuOptions>c__AnonStorey3
			{
				internal Pawn myPawn;

				internal Building_CryptosleepCasket.<GetFloatMenuOptions>c__Iterator0 <>f__ref$0;

				public <GetFloatMenuOptions>c__AnonStorey3()
				{
				}
			}

			private sealed class <GetFloatMenuOptions>c__AnonStorey2
			{
				internal JobDef jobDef;

				internal Building_CryptosleepCasket.<GetFloatMenuOptions>c__Iterator0 <>f__ref$0;

				internal Building_CryptosleepCasket.<GetFloatMenuOptions>c__Iterator0.<GetFloatMenuOptions>c__AnonStorey3 <>f__ref$3;

				public <GetFloatMenuOptions>c__AnonStorey2()
				{
				}

				internal void <>m__0()
				{
					Job job = new Job(this.jobDef, this.<>f__ref$0.$this);
					this.<>f__ref$3.myPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
				}
			}
		}

		[CompilerGenerated]
		private sealed class <GetGizmos>c__Iterator1 : IEnumerable, IEnumerable<Gizmo>, IEnumerator, IDisposable, IEnumerator<Gizmo>
		{
			internal IEnumerator<Gizmo> $locvar0;

			internal Gizmo <c>__1;

			internal Command_Action <eject>__2;

			internal Building_CryptosleepCasket $this;

			internal Gizmo $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetGizmos>c__Iterator1()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					enumerator = base.<GetGizmos>__BaseCallProxy1().GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					goto IL_1BB;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						c = enumerator.Current;
						this.$current = c;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				if (base.Faction != Faction.OfPlayer || this.innerContainer.Count <= 0 || !this.def.building.isPlayerEjectable)
				{
					goto IL_1BB;
				}
				eject = new Command_Action();
				eject.action = new Action(this.EjectContents);
				eject.defaultLabel = "CommandPodEject".Translate();
				eject.defaultDesc = "CommandPodEjectDesc".Translate();
				if (this.innerContainer.Count == 0)
				{
					eject.Disable("CommandPodEjectFailEmpty".Translate());
				}
				eject.hotKey = KeyBindingDefOf.Misc1;
				eject.icon = ContentFinder<Texture2D>.Get("UI/Commands/PodEject", true);
				this.$current = eject;
				if (!this.$disposing)
				{
					this.$PC = 2;
				}
				return true;
				IL_1BB:
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
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				}
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
				Building_CryptosleepCasket.<GetGizmos>c__Iterator1 <GetGizmos>c__Iterator = new Building_CryptosleepCasket.<GetGizmos>c__Iterator1();
				<GetGizmos>c__Iterator.$this = this;
				return <GetGizmos>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <FindCryptosleepCasketFor>c__AnonStorey4
		{
			internal Pawn traveler;

			internal bool ignoreOtherReservations;

			public <FindCryptosleepCasketFor>c__AnonStorey4()
			{
			}

			internal bool <>m__0(Thing x)
			{
				bool result;
				if (!((Building_CryptosleepCasket)x).HasAnyContents)
				{
					Pawn p = this.traveler;
					LocalTargetInfo target = x;
					bool flag = this.ignoreOtherReservations;
					result = p.CanReserve(target, 1, -1, null, flag);
				}
				else
				{
					result = false;
				}
				return result;
			}
		}
	}
}
