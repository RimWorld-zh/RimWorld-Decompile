using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public static class Toils_Construct
	{
		public static Toil MakeSolidThingFromBlueprintIfNecessary(TargetIndex blueTarget, TargetIndex targetToUpdate = TargetIndex.None)
		{
			Toil toil = new Toil();
			toil.initAction = (Action)delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				Blueprint blueprint = curJob.GetTarget(blueTarget).Thing as Blueprint;
				if (blueprint != null)
				{
					bool flag = targetToUpdate != 0 && curJob.GetTarget(targetToUpdate).Thing == blueprint;
					Thing thing = default(Thing);
					bool flag2 = default(bool);
					if (blueprint.TryReplaceWithSolidThing(actor, out thing, out flag2))
					{
						curJob.SetTarget(blueTarget, thing);
						if (flag)
						{
							curJob.SetTarget(targetToUpdate, thing);
						}
						if (thing is Frame)
						{
							actor.Reserve(thing, 1, -1, null);
						}
					}
					if (!flag2)
						return;
				}
			};
			return toil;
		}

		public static Toil UninstallIfMinifiable(TargetIndex thingInd)
		{
			Toil uninstallIfMinifiable = new Toil().FailOnDestroyedNullOrForbidden(thingInd);
			uninstallIfMinifiable.initAction = (Action)delegate()
			{
				Pawn actor2 = uninstallIfMinifiable.actor;
				JobDriver curDriver2 = actor2.jobs.curDriver;
				Thing thing2 = actor2.CurJob.GetTarget(thingInd).Thing;
				if (thing2.def.Minifiable)
				{
					curDriver2.uninstallWorkLeft = 90f;
				}
				else
				{
					curDriver2.ReadyForNextToil();
				}
			};
			uninstallIfMinifiable.tickAction = (Action)delegate()
			{
				Pawn actor = uninstallIfMinifiable.actor;
				JobDriver curDriver = actor.jobs.curDriver;
				Job curJob = actor.CurJob;
				curDriver.uninstallWorkLeft -= actor.GetStatValue(StatDefOf.ConstructionSpeed, true);
				if (curDriver.uninstallWorkLeft <= 0.0)
				{
					Thing thing = curJob.GetTarget(thingInd).Thing;
					MinifiedThing minifiedThing = thing.MakeMinified();
					GenSpawn.Spawn(minifiedThing, thing.Position, uninstallIfMinifiable.actor.Map);
					curJob.SetTarget(thingInd, (Thing)minifiedThing);
					actor.jobs.curDriver.ReadyForNextToil();
				}
			};
			uninstallIfMinifiable.defaultCompleteMode = ToilCompleteMode.Never;
			uninstallIfMinifiable.WithProgressBar(thingInd, (Func<float>)(() => (float)(1.0 - uninstallIfMinifiable.actor.jobs.curDriver.uninstallWorkLeft / 90.0)), false, -0.5f);
			return uninstallIfMinifiable;
		}
	}
}
