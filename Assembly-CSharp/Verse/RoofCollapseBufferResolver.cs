using RimWorld;
using System.Collections.Generic;
using System.Text;

namespace Verse
{
	public class RoofCollapseBufferResolver
	{
		private Map map;

		public RoofCollapseBufferResolver(Map map)
		{
			this.map = map;
		}

		public void CollapseRoofsMarkedToCollapse()
		{
			RoofCollapseBuffer roofCollapseBuffer = this.map.roofCollapseBuffer;
			if (roofCollapseBuffer.CellsMarkedToCollapse.Count > 0)
			{
				RoofCollapserImmediate.DropRoofInCells(roofCollapseBuffer.CellsMarkedToCollapse, this.map);
				if (roofCollapseBuffer.CrushedThingsForLetter.Count > 0)
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine("RoofCollapsed".Translate());
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("TheseThingsCrushed".Translate());
					HashSet<string> hashSet = new HashSet<string>();
					foreach (Thing item2 in roofCollapseBuffer.CrushedThingsForLetter)
					{
						string item = item2.LabelShort.CapitalizeFirst();
						if (item2.def.category == ThingCategory.Pawn)
						{
							item = item2.LabelCap;
						}
						if (!hashSet.Contains(item))
						{
							hashSet.Add(item);
						}
					}
					foreach (string item3 in hashSet)
					{
						stringBuilder.AppendLine("    -" + item3);
					}
					Find.LetterStack.ReceiveLetter("LetterLabelRoofCollapsed".Translate(), stringBuilder.ToString(), LetterDefOf.NegativeEvent, new TargetInfo(roofCollapseBuffer.CellsMarkedToCollapse[0], this.map, false), (string)null);
				}
				else
				{
					string text = "RoofCollapsed".Translate();
					Messages.Message(text, new TargetInfo(roofCollapseBuffer.CellsMarkedToCollapse[0], this.map, false), MessageTypeDefOf.NegativeHealthEvent);
				}
				roofCollapseBuffer.Clear();
			}
		}
	}
}
