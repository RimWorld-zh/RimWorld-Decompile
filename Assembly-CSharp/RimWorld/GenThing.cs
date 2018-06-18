using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000984 RID: 2436
	public static class GenThing
	{
		// Token: 0x060036D8 RID: 14040 RVA: 0x001D470C File Offset: 0x001D2B0C
		public static Vector3 TrueCenter(this Thing t)
		{
			Pawn pawn = t as Pawn;
			Vector3 result;
			if (pawn != null)
			{
				result = pawn.Drawer.DrawPos;
			}
			else
			{
				result = GenThing.TrueCenter(t.Position, t.Rotation, t.def.size, t.def.Altitude);
			}
			return result;
		}

		// Token: 0x060036D9 RID: 14041 RVA: 0x001D4768 File Offset: 0x001D2B68
		public static Vector3 TrueCenter(IntVec3 loc, Rot4 rotation, IntVec2 thingSize, float altitude)
		{
			Vector3 result = loc.ToVector3ShiftedWithAltitude(altitude);
			if (thingSize.x != 1 || thingSize.z != 1)
			{
				if (rotation.IsHorizontal)
				{
					int x = thingSize.x;
					thingSize.x = thingSize.z;
					thingSize.z = x;
				}
				switch (rotation.AsInt)
				{
				case 0:
					if (thingSize.x % 2 == 0)
					{
						result.x += 0.5f;
					}
					if (thingSize.z % 2 == 0)
					{
						result.z += 0.5f;
					}
					break;
				case 1:
					if (thingSize.x % 2 == 0)
					{
						result.x += 0.5f;
					}
					if (thingSize.z % 2 == 0)
					{
						result.z -= 0.5f;
					}
					break;
				case 2:
					if (thingSize.x % 2 == 0)
					{
						result.x -= 0.5f;
					}
					if (thingSize.z % 2 == 0)
					{
						result.z -= 0.5f;
					}
					break;
				case 3:
					if (thingSize.x % 2 == 0)
					{
						result.x -= 0.5f;
					}
					if (thingSize.z % 2 == 0)
					{
						result.z += 0.5f;
					}
					break;
				}
			}
			return result;
		}

		// Token: 0x060036DA RID: 14042 RVA: 0x001D4910 File Offset: 0x001D2D10
		public static bool TryDropAndSetForbidden(Thing th, IntVec3 pos, Map map, ThingPlaceMode mode, out Thing resultingThing, bool forbidden)
		{
			bool result;
			if (GenDrop.TryDropSpawn(th, pos, map, ThingPlaceMode.Near, out resultingThing, null, null))
			{
				if (resultingThing != null)
				{
					resultingThing.SetForbidden(forbidden, false);
				}
				result = true;
			}
			else
			{
				resultingThing = null;
				result = false;
			}
			return result;
		}

		// Token: 0x060036DB RID: 14043 RVA: 0x001D4958 File Offset: 0x001D2D58
		public static string ThingsToCommaList(IList<Thing> things, bool useAnd = false, bool aggregate = true)
		{
			GenThing.tmpThings.Clear();
			GenThing.tmpThingLabels.Clear();
			GenThing.tmpThingCounts.Clear();
			GenThing.tmpThings.AddRange(things);
			if (GenThing.tmpThings.Count >= 2)
			{
				GenThing.tmpThings.SortByDescending((Thing x) => x is Pawn, (Thing x) => x.MarketValue * (float)x.stackCount);
			}
			for (int i = 0; i < GenThing.tmpThings.Count; i++)
			{
				string labelNoCount = GenThing.tmpThings[i].LabelNoCount;
				bool flag = false;
				if (aggregate)
				{
					for (int j = 0; j < GenThing.tmpThingCounts.Count; j++)
					{
						if (GenThing.tmpThingCounts[j].First == labelNoCount)
						{
							GenThing.tmpThingCounts[j] = new Pair<string, int>(GenThing.tmpThingCounts[j].First, GenThing.tmpThingCounts[j].Second + GenThing.tmpThings[i].stackCount);
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					GenThing.tmpThingCounts.Add(new Pair<string, int>(labelNoCount, GenThing.tmpThings[i].stackCount));
				}
			}
			GenThing.tmpThings.Clear();
			for (int k = 0; k < GenThing.tmpThingCounts.Count; k++)
			{
				string text = GenThing.tmpThingCounts[k].First;
				if (GenThing.tmpThingCounts[k].Second != 1)
				{
					text = text + " x" + GenThing.tmpThingCounts[k].Second;
				}
				GenThing.tmpThingLabels.Add(text);
			}
			return GenThing.tmpThingLabels.ToCommaList(useAnd);
		}

		// Token: 0x060036DC RID: 14044 RVA: 0x001D4B74 File Offset: 0x001D2F74
		public static float GetMarketValue(IList<Thing> things)
		{
			float num = 0f;
			for (int i = 0; i < things.Count; i++)
			{
				num += things[i].MarketValue * (float)things[i].stackCount;
			}
			return num;
		}

		// Token: 0x04002360 RID: 9056
		private static List<Thing> tmpThings = new List<Thing>();

		// Token: 0x04002361 RID: 9057
		private static List<string> tmpThingLabels = new List<string>();

		// Token: 0x04002362 RID: 9058
		private static List<Pair<string, int>> tmpThingCounts = new List<Pair<string, int>>();
	}
}
