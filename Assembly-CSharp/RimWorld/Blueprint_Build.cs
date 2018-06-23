using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000673 RID: 1651
	public class Blueprint_Build : Blueprint
	{
		// Token: 0x04001390 RID: 5008
		public ThingDef stuffToUse = null;

		// Token: 0x17000515 RID: 1301
		// (get) Token: 0x060022B5 RID: 8885 RVA: 0x0012B1B0 File Offset: 0x001295B0
		public override string Label
		{
			get
			{
				string label = base.Label;
				string result;
				if (this.stuffToUse != null)
				{
					result = "ThingMadeOfStuffLabel".Translate(new object[]
					{
						this.stuffToUse.LabelAsStuff,
						label
					});
				}
				else
				{
					result = label;
				}
				return result;
			}
		}

		// Token: 0x17000516 RID: 1302
		// (get) Token: 0x060022B6 RID: 8886 RVA: 0x0012B200 File Offset: 0x00129600
		protected override float WorkTotal
		{
			get
			{
				return this.def.entityDefToBuild.GetStatValueAbstract(StatDefOf.WorkToBuild, this.stuffToUse);
			}
		}

		// Token: 0x060022B7 RID: 8887 RVA: 0x0012B230 File Offset: 0x00129630
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.stuffToUse, "stuffToUse");
		}

		// Token: 0x060022B8 RID: 8888 RVA: 0x0012B24C File Offset: 0x0012964C
		public override ThingDef UIStuff()
		{
			return this.stuffToUse;
		}

		// Token: 0x060022B9 RID: 8889 RVA: 0x0012B268 File Offset: 0x00129668
		public override List<ThingDefCountClass> MaterialsNeeded()
		{
			return this.def.entityDefToBuild.CostListAdjusted(this.stuffToUse, true);
		}

		// Token: 0x060022BA RID: 8890 RVA: 0x0012B294 File Offset: 0x00129694
		protected override Thing MakeSolidThing()
		{
			return ThingMaker.MakeThing(this.def.entityDefToBuild.frameDef, this.stuffToUse);
		}

		// Token: 0x060022BB RID: 8891 RVA: 0x0012B2C4 File Offset: 0x001296C4
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo c in this.<GetGizmos>__BaseCallProxy0())
			{
				yield return c;
			}
			Command buildCopy = BuildCopyCommandUtility.BuildCopyCommand(this.def.entityDefToBuild, this.stuffToUse);
			if (buildCopy != null)
			{
				yield return buildCopy;
			}
			if (base.Faction == Faction.OfPlayer)
			{
				foreach (Command facility in BuildFacilityCommandUtility.BuildFacilityCommands(this.def.entityDefToBuild))
				{
					yield return facility;
				}
			}
			yield break;
		}

		// Token: 0x060022BC RID: 8892 RVA: 0x0012B2F0 File Offset: 0x001296F0
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
			foreach (ThingDefCountClass thingDefCountClass in this.MaterialsNeeded())
			{
				if (!flag)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.Append(thingDefCountClass.thingDef.LabelCap + ": 0 / " + thingDefCountClass.count);
				flag = false;
			}
			return stringBuilder.ToString().Trim();
		}
	}
}
