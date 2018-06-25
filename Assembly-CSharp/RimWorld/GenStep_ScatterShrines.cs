using System;
using System.Collections.Generic;
using RimWorld.BaseGen;
using Verse;

namespace RimWorld
{
	// Token: 0x02000402 RID: 1026
	public class GenStep_ScatterShrines : GenStep_ScatterRuinsSimple
	{
		// Token: 0x04000AB3 RID: 2739
		private static readonly IntRange ShrinesCountX = new IntRange(1, 4);

		// Token: 0x04000AB4 RID: 2740
		private static readonly IntRange ShrinesCountZ = new IntRange(1, 4);

		// Token: 0x04000AB5 RID: 2741
		private static readonly IntRange ExtraHeightRange = new IntRange(0, 8);

		// Token: 0x04000AB6 RID: 2742
		private const int MarginCells = 1;

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x0600119F RID: 4511 RVA: 0x00098BD4 File Offset: 0x00096FD4
		public override int SeedPart
		{
			get
			{
				return 1801222485;
			}
		}

		// Token: 0x060011A0 RID: 4512 RVA: 0x00098BF0 File Offset: 0x00096FF0
		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			bool result;
			if (!base.CanScatterAt(c, map))
			{
				result = false;
			}
			else
			{
				Building edifice = c.GetEdifice(map);
				result = (edifice != null && edifice.def.building.isNaturalRock);
			}
			return result;
		}

		// Token: 0x060011A1 RID: 4513 RVA: 0x00098C44 File Offset: 0x00097044
		protected override void ScatterAt(IntVec3 loc, Map map, int stackCount = 1)
		{
			int randomInRange = GenStep_ScatterShrines.ShrinesCountX.RandomInRange;
			int randomInRange2 = GenStep_ScatterShrines.ShrinesCountZ.RandomInRange;
			int randomInRange3 = GenStep_ScatterShrines.ExtraHeightRange.RandomInRange;
			IntVec2 standardAncientShrineSize = SymbolResolver_AncientShrinesGroup.StandardAncientShrineSize;
			int num = 1;
			int num2 = randomInRange * standardAncientShrineSize.x + (randomInRange - 1) * num;
			int num3 = randomInRange2 * standardAncientShrineSize.z + (randomInRange2 - 1) * num;
			int num4 = num2 + 2;
			int num5 = num3 + 2 + randomInRange3;
			CellRect rect = new CellRect(loc.x, loc.z, num4, num5);
			rect.ClipInsideMap(map);
			if (rect.Width == num4 && rect.Height == num5)
			{
				foreach (IntVec3 c in rect.Cells)
				{
					List<Thing> list = map.thingGrid.ThingsListAt(c);
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].def == ThingDefOf.AncientCryptosleepCasket)
						{
							return;
						}
					}
				}
				if (base.CanPlaceAncientBuildingInRange(rect, map))
				{
					ResolveParams resolveParams = default(ResolveParams);
					resolveParams.rect = rect;
					resolveParams.disableSinglePawn = new bool?(true);
					resolveParams.disableHives = new bool?(true);
					resolveParams.ancientTempleEntranceHeight = new int?(randomInRange3);
					BaseGen.globalSettings.map = map;
					BaseGen.symbolStack.Push("ancientTemple", resolveParams);
					BaseGen.Generate();
					int nextSignalTagID = Find.UniqueIDsManager.GetNextSignalTagID();
					string signalTag = "ancientTempleApproached-" + nextSignalTagID;
					SignalAction_Letter signalAction_Letter = (SignalAction_Letter)ThingMaker.MakeThing(ThingDefOf.SignalAction_Letter, null);
					signalAction_Letter.signalTag = signalTag;
					signalAction_Letter.letter = LetterMaker.MakeLetter("LetterLabelAncientShrineWarning".Translate(), "AncientShrineWarning".Translate(), LetterDefOf.NeutralEvent, new TargetInfo(rect.CenterCell, map, false), null);
					GenSpawn.Spawn(signalAction_Letter, rect.CenterCell, map, WipeMode.Vanish);
					RectTrigger rectTrigger = (RectTrigger)ThingMaker.MakeThing(ThingDefOf.RectTrigger, null);
					rectTrigger.signalTag = signalTag;
					rectTrigger.Rect = rect.ExpandedBy(1).ClipInsideMap(map);
					rectTrigger.destroyIfUnfogged = true;
					GenSpawn.Spawn(rectTrigger, rect.CenterCell, map, WipeMode.Vanish);
				}
			}
		}
	}
}
