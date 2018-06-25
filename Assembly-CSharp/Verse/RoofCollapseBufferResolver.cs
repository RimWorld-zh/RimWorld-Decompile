using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;

namespace Verse
{
	// Token: 0x02000C9D RID: 3229
	public class RoofCollapseBufferResolver
	{
		// Token: 0x04003059 RID: 12377
		private Map map;

		// Token: 0x0400305A RID: 12378
		private List<Thing> tmpCrushedThings = new List<Thing>();

		// Token: 0x0400305B RID: 12379
		private HashSet<string> tmpCrushedNames = new HashSet<string>();

		// Token: 0x06004721 RID: 18209 RVA: 0x0025862F File Offset: 0x00256A2F
		public RoofCollapseBufferResolver(Map map)
		{
			this.map = map;
		}

		// Token: 0x06004722 RID: 18210 RVA: 0x00258658 File Offset: 0x00256A58
		public void CollapseRoofsMarkedToCollapse()
		{
			RoofCollapseBuffer roofCollapseBuffer = this.map.roofCollapseBuffer;
			if (roofCollapseBuffer.CellsMarkedToCollapse.Any<IntVec3>())
			{
				this.tmpCrushedThings.Clear();
				RoofCollapserImmediate.DropRoofInCells(roofCollapseBuffer.CellsMarkedToCollapse, this.map, this.tmpCrushedThings);
				if (this.tmpCrushedThings.Any<Thing>())
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine("RoofCollapsed".Translate());
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("TheseThingsCrushed".Translate());
					this.tmpCrushedNames.Clear();
					for (int i = 0; i < this.tmpCrushedThings.Count; i++)
					{
						Thing thing = this.tmpCrushedThings[i];
						string item = thing.LabelShort.CapitalizeFirst();
						if (thing.def.category == ThingCategory.Pawn)
						{
							item = thing.LabelCap;
						}
						if (!this.tmpCrushedNames.Contains(item))
						{
							this.tmpCrushedNames.Add(item);
						}
					}
					foreach (string str in this.tmpCrushedNames)
					{
						stringBuilder.AppendLine("    -" + str);
					}
					Find.LetterStack.ReceiveLetter("LetterLabelRoofCollapsed".Translate(), stringBuilder.ToString().TrimEndNewlines(), LetterDefOf.NegativeEvent, new TargetInfo(roofCollapseBuffer.CellsMarkedToCollapse[0], this.map, false), null, null);
				}
				else
				{
					string text = "RoofCollapsed".Translate();
					Messages.Message(text, new TargetInfo(roofCollapseBuffer.CellsMarkedToCollapse[0], this.map, false), MessageTypeDefOf.SilentInput, true);
				}
				this.tmpCrushedThings.Clear();
				roofCollapseBuffer.Clear();
			}
		}
	}
}
