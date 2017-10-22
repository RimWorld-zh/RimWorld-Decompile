using RimWorld;
using System;
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
					List<Thing>.Enumerator enumerator = roofCollapseBuffer.CrushedThingsForLetter.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							Thing current = enumerator.Current;
							string item = current.LabelShort.CapitalizeFirst();
							if (current.def.category == ThingCategory.Pawn)
							{
								item = current.LabelCap;
							}
							if (!hashSet.Contains(item))
							{
								hashSet.Add(item);
							}
						}
					}
					finally
					{
						((IDisposable)(object)enumerator).Dispose();
					}
					HashSet<string>.Enumerator enumerator2 = hashSet.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							string current2 = enumerator2.Current;
							stringBuilder.AppendLine("    -" + current2);
						}
					}
					finally
					{
						((IDisposable)(object)enumerator2).Dispose();
					}
					Find.LetterStack.ReceiveLetter("LetterLabelRoofCollapsed".Translate(), stringBuilder.ToString(), LetterDefOf.BadNonUrgent, new TargetInfo(roofCollapseBuffer.CellsMarkedToCollapse[0], this.map, false), (string)null);
				}
				else
				{
					string text = "RoofCollapsed".Translate();
					Messages.Message(text, new TargetInfo(roofCollapseBuffer.CellsMarkedToCollapse[0], this.map, false), MessageSound.Negative);
				}
				roofCollapseBuffer.Clear();
			}
		}
	}
}
