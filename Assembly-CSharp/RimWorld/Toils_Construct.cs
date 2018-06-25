using System;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public static class Toils_Construct
	{
		public static Toil MakeSolidThingFromBlueprintIfNecessary(TargetIndex blueTarget, TargetIndex targetToUpdate = TargetIndex.None)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				Blueprint blueprint = curJob.GetTarget(blueTarget).Thing as Blueprint;
				if (blueprint != null)
				{
					bool flag = targetToUpdate != TargetIndex.None && curJob.GetTarget(targetToUpdate).Thing == blueprint;
					Thing thing;
					bool flag2;
					if (blueprint.TryReplaceWithSolidThing(actor, out thing, out flag2))
					{
						curJob.SetTarget(blueTarget, thing);
						if (flag)
						{
							curJob.SetTarget(targetToUpdate, thing);
						}
						if (thing is Frame)
						{
							actor.Reserve(thing, curJob, 1, -1, null);
						}
					}
					if (flag2)
					{
					}
				}
			};
			return toil;
		}

		public static Toil UninstallIfMinifiable(TargetIndex thingInd)
		{
			Toil uninstallIfMinifiable = new Toil().FailOnDestroyedNullOrForbidden(thingInd);
			uninstallIfMinifiable.initAction = delegate()
			{
				Pawn actor = uninstallIfMinifiable.actor;
				JobDriver curDriver = actor.jobs.curDriver;
				Thing thing = actor.CurJob.GetTarget(thingInd).Thing;
				if (thing.def.Minifiable)
				{
					curDriver.uninstallWorkLeft = 90f;
				}
				else
				{
					curDriver.ReadyForNextToil();
				}
			};
			uninstallIfMinifiable.tickAction = delegate()
			{
				Pawn actor = uninstallIfMinifiable.actor;
				JobDriver curDriver = actor.jobs.curDriver;
				Job curJob = actor.CurJob;
				curDriver.uninstallWorkLeft -= actor.GetStatValue(StatDefOf.ConstructionSpeed, true);
				if (curDriver.uninstallWorkLeft <= 0f)
				{
					Thing thing = curJob.GetTarget(thingInd).Thing;
					MinifiedThing minifiedThing = thing.MakeMinified();
					GenSpawn.Spawn(minifiedThing, thing.Position, uninstallIfMinifiable.actor.Map, WipeMode.Vanish);
					curJob.SetTarget(thingInd, minifiedThing);
					actor.jobs.curDriver.ReadyForNextToil();
				}
			};
			uninstallIfMinifiable.defaultCompleteMode = ToilCompleteMode.Never;
			uninstallIfMinifiable.WithProgressBar(thingInd, () => 1f - uninstallIfMinifiable.actor.jobs.curDriver.uninstallWorkLeft / 90f, false, -0.5f);
			return uninstallIfMinifiable;
		}

		[CompilerGenerated]
		private sealed class <MakeSolidThingFromBlueprintIfNecessary>c__AnonStorey0
		{
			internal Toil toil;

			internal TargetIndex blueTarget;

			internal TargetIndex targetToUpdate;

			public <MakeSolidThingFromBlueprintIfNecessary>c__AnonStorey0()
			{
			}

			internal void <>m__0()
			{
				Pawn actor = this.toil.actor;
				Job curJob = actor.jobs.curJob;
				Blueprint blueprint = curJob.GetTarget(this.blueTarget).Thing as Blueprint;
				if (blueprint != null)
				{
					bool flag = this.targetToUpdate != TargetIndex.None && curJob.GetTarget(this.targetToUpdate).Thing == blueprint;
					Thing thing;
					bool flag2;
					if (blueprint.TryReplaceWithSolidThing(actor, out thing, out flag2))
					{
						curJob.SetTarget(this.blueTarget, thing);
						if (flag)
						{
							curJob.SetTarget(this.targetToUpdate, thing);
						}
						if (thing is Frame)
						{
							actor.Reserve(thing, curJob, 1, -1, null);
						}
					}
					if (flag2)
					{
					}
				}
			}
		}

		[CompilerGenerated]
		private sealed class <UninstallIfMinifiable>c__AnonStorey1
		{
			internal Toil uninstallIfMinifiable;

			internal TargetIndex thingInd;

			public <UninstallIfMinifiable>c__AnonStorey1()
			{
			}

			internal void <>m__0()
			{
				Pawn actor = this.uninstallIfMinifiable.actor;
				JobDriver curDriver = actor.jobs.curDriver;
				Thing thing = actor.CurJob.GetTarget(this.thingInd).Thing;
				if (thing.def.Minifiable)
				{
					curDriver.uninstallWorkLeft = 90f;
				}
				else
				{
					curDriver.ReadyForNextToil();
				}
			}

			internal void <>m__1()
			{
				Pawn actor = this.uninstallIfMinifiable.actor;
				JobDriver curDriver = actor.jobs.curDriver;
				Job curJob = actor.CurJob;
				curDriver.uninstallWorkLeft -= actor.GetStatValue(StatDefOf.ConstructionSpeed, true);
				if (curDriver.uninstallWorkLeft <= 0f)
				{
					Thing thing = curJob.GetTarget(this.thingInd).Thing;
					MinifiedThing minifiedThing = thing.MakeMinified();
					GenSpawn.Spawn(minifiedThing, thing.Position, this.uninstallIfMinifiable.actor.Map, WipeMode.Vanish);
					curJob.SetTarget(this.thingInd, minifiedThing);
					actor.jobs.curDriver.ReadyForNextToil();
				}
			}

			internal float <>m__2()
			{
				return 1f - this.uninstallIfMinifiable.actor.jobs.curDriver.uninstallWorkLeft / 90f;
			}
		}
	}
}
