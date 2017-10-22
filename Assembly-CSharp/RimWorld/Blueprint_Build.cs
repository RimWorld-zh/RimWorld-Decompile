using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	public class Blueprint_Build : Blueprint
	{
		public ThingDef stuffToUse;

		public override string Label
		{
			get
			{
				string label = base.Label;
				if (this.stuffToUse != null)
				{
					return "ThingMadeOfStuffLabel".Translate(this.stuffToUse.LabelAsStuff, label);
				}
				return label;
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
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			Command buildCopy = BuildCopyCommandUtility.BuildCopyCommand(base.def.entityDefToBuild, this.stuffToUse);
			if (buildCopy != null)
			{
				yield return (Gizmo)buildCopy;
			}
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
			List<ThingCountClass>.Enumerator enumerator = this.MaterialsNeeded().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					ThingCountClass current = enumerator.Current;
					if (!flag)
					{
						stringBuilder.AppendLine();
					}
					stringBuilder.Append(current.thingDef.LabelCap + ": 0 / " + current.count);
					flag = false;
				}
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
			return stringBuilder.ToString().Trim();
		}
	}
}
