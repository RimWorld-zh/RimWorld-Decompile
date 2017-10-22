using System;
using System.Collections.Generic;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	public class Blueprint_Install : Blueprint
	{
		private MinifiedThing miniToInstall;

		private Building buildingToReinstall;

		public Thing MiniToInstallOrBuildingToReinstall
		{
			get
			{
				Thing result;
				if (this.miniToInstall != null)
				{
					result = this.miniToInstall;
					goto IL_003a;
				}
				if (this.buildingToReinstall != null)
				{
					result = this.buildingToReinstall;
					goto IL_003a;
				}
				throw new InvalidOperationException("Nothing to install.");
				IL_003a:
				return result;
			}
		}

		private Thing ThingToInstall
		{
			get
			{
				return this.MiniToInstallOrBuildingToReinstall.GetInnerIfMinified();
			}
		}

		public override Graphic Graphic
		{
			get
			{
				Graphic graphic = this.ThingToInstall.def.installBlueprintDef.graphic;
				return graphic.ExtractInnerGraphicFor(this.ThingToInstall);
			}
		}

		protected override float WorkTotal
		{
			get
			{
				return 150f;
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MinifiedThing>(ref this.miniToInstall, "miniToInstall", false);
			Scribe_References.Look<Building>(ref this.buildingToReinstall, "buildingToReinstall", false);
		}

		public override ThingDef UIStuff()
		{
			return this.ThingToInstall.Stuff;
		}

		public override List<ThingCountClass> MaterialsNeeded()
		{
			Log.Error("Called MaterialsNeeded on a Blueprint_Install.");
			return new List<ThingCountClass>();
		}

		protected override Thing MakeSolidThing()
		{
			return this.ThingToInstall;
		}

		public override bool TryReplaceWithSolidThing(Pawn workerPawn, out Thing createdThing, out bool jobEnded)
		{
			Map map = base.Map;
			bool flag = base.TryReplaceWithSolidThing(workerPawn, out createdThing, out jobEnded);
			if (flag)
			{
				SoundDefOf.BuildingComplete.PlayOneShot(new TargetInfo(base.Position, map, false));
				workerPawn.records.Increment(RecordDefOf.ThingsInstalled);
			}
			return flag;
		}

		public override IEnumerable<Gizmo> GetGizmos()
		{
			using (IEnumerator<Gizmo> enumerator = this._003CGetGizmos_003E__BaseCallProxy0().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					Gizmo c = enumerator.Current;
					yield return c;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			Command buildCopy = BuildCopyCommandUtility.BuildCopyCommand(this.ThingToInstall.def, this.ThingToInstall.Stuff);
			if (buildCopy == null)
				yield break;
			yield return (Gizmo)buildCopy;
			/*Error: Unable to find new state assignment for yield return*/;
			IL_0117:
			/*Error near IL_0118: Unexpected return in MoveNext()*/;
		}

		internal void SetThingToInstallFromMinified(MinifiedThing itemToInstall)
		{
			this.miniToInstall = itemToInstall;
			this.buildingToReinstall = null;
		}

		internal void SetBuildingToReinstall(Building buildingToReinstall)
		{
			if (!buildingToReinstall.def.Minifiable)
			{
				Log.Error("Tried to reinstall non-minifiable building.");
			}
			else
			{
				this.miniToInstall = null;
				this.buildingToReinstall = buildingToReinstall;
			}
		}
	}
}
