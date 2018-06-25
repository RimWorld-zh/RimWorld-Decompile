using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000675 RID: 1653
	public class Blueprint_Build : Blueprint
	{
		// Token: 0x04001390 RID: 5008
		public ThingDef stuffToUse = null;

		// Token: 0x17000515 RID: 1301
		// (get) Token: 0x060022B9 RID: 8889 RVA: 0x0012B300 File Offset: 0x00129700
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
		// (get) Token: 0x060022BA RID: 8890 RVA: 0x0012B350 File Offset: 0x00129750
		protected override float WorkTotal
		{
			get
			{
				return this.def.entityDefToBuild.GetStatValueAbstract(StatDefOf.WorkToBuild, this.stuffToUse);
			}
		}

		// Token: 0x060022BB RID: 8891 RVA: 0x0012B380 File Offset: 0x00129780
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.stuffToUse, "stuffToUse");
		}

		// Token: 0x060022BC RID: 8892 RVA: 0x0012B39C File Offset: 0x0012979C
		public override ThingDef UIStuff()
		{
			return this.stuffToUse;
		}

		// Token: 0x060022BD RID: 8893 RVA: 0x0012B3B8 File Offset: 0x001297B8
		public override List<ThingDefCountClass> MaterialsNeeded()
		{
			return this.def.entityDefToBuild.CostListAdjusted(this.stuffToUse, true);
		}

		// Token: 0x060022BE RID: 8894 RVA: 0x0012B3E4 File Offset: 0x001297E4
		protected override Thing MakeSolidThing()
		{
			return ThingMaker.MakeThing(this.def.entityDefToBuild.frameDef, this.stuffToUse);
		}

		// Token: 0x060022BF RID: 8895 RVA: 0x0012B414 File Offset: 0x00129814
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

		// Token: 0x060022C0 RID: 8896 RVA: 0x0012B440 File Offset: 0x00129840
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
