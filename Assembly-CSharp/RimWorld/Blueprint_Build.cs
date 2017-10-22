using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	public class Blueprint_Build : Blueprint
	{
		public ThingDef stuffToUse = null;

		public override string Label
		{
			get
			{
				string label = base.Label;
				return (this.stuffToUse == null) ? label : "ThingMadeOfStuffLabel".Translate(this.stuffToUse.LabelAsStuff, label);
			}
		}

		protected override float WorkTotal
		{
			get
			{
				return base.def.entityDefToBuild.GetStatValueAbstract(StatDefOf.WorkToBuild, this.stuffToUse);
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.stuffToUse, "stuffToUse");
		}

		public override ThingDef UIStuff()
		{
			return this.stuffToUse;
		}

		public override List<ThingCountClass> MaterialsNeeded()
		{
			return base.def.entityDefToBuild.CostListAdjusted(this.stuffToUse, true);
		}

		protected override Thing MakeSolidThing()
		{
			return ThingMaker.MakeThing(base.def.entityDefToBuild.frameDef, this.stuffToUse);
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
			Command buildCopy = BuildCopyCommandUtility.BuildCopyCommand(base.def.entityDefToBuild, this.stuffToUse);
			if (buildCopy == null)
				yield break;
			yield return (Gizmo)buildCopy;
			/*Error: Unable to find new state assignment for yield return*/;
			IL_0112:
			/*Error near IL_0113: Unexpected return in MoveNext()*/;
		}

		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			if (stringBuilder.Length > 0)
			{
				stringBuilder.AppendLine();
			}
			stringBuilder.AppendLine("ContainedResources".Translate() + ":");
			bool flag = true;
			foreach (ThingCountClass item in this.MaterialsNeeded())
			{
				if (!flag)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.Append(item.thingDef.LabelCap + ": 0 / " + item.count);
				flag = false;
			}
			return stringBuilder.ToString().Trim();
		}
	}
}
