using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F4A RID: 3914
	public static class GenRadial
	{
		// Token: 0x04003E29 RID: 15913
		public static IntVec3[] ManualRadialPattern = new IntVec3[49];

		// Token: 0x04003E2A RID: 15914
		public static IntVec3[] RadialPattern = new IntVec3[10000];

		// Token: 0x04003E2B RID: 15915
		private static float[] RadialPatternRadii = new float[10000];

		// Token: 0x04003E2C RID: 15916
		private const int RadialPatternCount = 10000;

		// Token: 0x04003E2D RID: 15917
		private static List<IntVec3> tmpCells = new List<IntVec3>();

		// Token: 0x04003E2E RID: 15918
		private static bool working = false;

		// Token: 0x06005EA6 RID: 24230 RVA: 0x00301BD4 File Offset: 0x002FFFD4
		static GenRadial()
		{
			GenRadial.SetupManualRadialPattern();
			GenRadial.SetupRadialPattern();
		}

		// Token: 0x17000F3D RID: 3901
		// (get) Token: 0x06005EA7 RID: 24231 RVA: 0x00301C28 File Offset: 0x00300028
		public static float MaxRadialPatternRadius
		{
			get
			{
				return GenRadial.RadialPatternRadii[GenRadial.RadialPatternRadii.Length - 1];
			}
		}

		// Token: 0x06005EA8 RID: 24232 RVA: 0x00301C4C File Offset: 0x0030004C
		private static void SetupManualRadialPattern()
		{
			GenRadial.ManualRadialPattern[0] = new IntVec3(0, 0, 0);
			GenRadial.ManualRadialPattern[1] = new IntVec3(0, 0, -1);
			GenRadial.ManualRadialPattern[2] = new IntVec3(1, 0, 0);
			GenRadial.ManualRadialPattern[3] = new IntVec3(0, 0, 1);
			GenRadial.ManualRadialPattern[4] = new IntVec3(-1, 0, 0);
			GenRadial.ManualRadialPattern[5] = new IntVec3(1, 0, -1);
			GenRadial.ManualRadialPattern[6] = new IntVec3(1, 0, 1);
			GenRadial.ManualRadialPattern[7] = new IntVec3(-1, 0, 1);
			GenRadial.ManualRadialPattern[8] = new IntVec3(-1, 0, -1);
			GenRadial.ManualRadialPattern[9] = new IntVec3(2, 0, 0);
			GenRadial.ManualRadialPattern[10] = new IntVec3(-2, 0, 0);
			GenRadial.ManualRadialPattern[11] = new IntVec3(0, 0, 2);
			GenRadial.ManualRadialPattern[12] = new IntVec3(0, 0, -2);
			GenRadial.ManualRadialPattern[13] = new IntVec3(2, 0, 1);
			GenRadial.ManualRadialPattern[14] = new IntVec3(2, 0, -1);
			GenRadial.ManualRadialPattern[15] = new IntVec3(-2, 0, 1);
			GenRadial.ManualRadialPattern[16] = new IntVec3(-2, 0, -1);
			GenRadial.ManualRadialPattern[17] = new IntVec3(-1, 0, 2);
			GenRadial.ManualRadialPattern[18] = new IntVec3(1, 0, 2);
			GenRadial.ManualRadialPattern[19] = new IntVec3(-1, 0, -2);
			GenRadial.ManualRadialPattern[20] = new IntVec3(1, 0, -2);
			GenRadial.ManualRadialPattern[21] = new IntVec3(2, 0, 2);
			GenRadial.ManualRadialPattern[22] = new IntVec3(-2, 0, -2);
			GenRadial.ManualRadialPattern[23] = new IntVec3(2, 0, -2);
			GenRadial.ManualRadialPattern[24] = new IntVec3(-2, 0, 2);
			GenRadial.ManualRadialPattern[25] = new IntVec3(3, 0, 0);
			GenRadial.ManualRadialPattern[26] = new IntVec3(0, 0, 3);
			GenRadial.ManualRadialPattern[27] = new IntVec3(-3, 0, 0);
			GenRadial.ManualRadialPattern[28] = new IntVec3(0, 0, -3);
			GenRadial.ManualRadialPattern[29] = new IntVec3(3, 0, 1);
			GenRadial.ManualRadialPattern[30] = new IntVec3(-3, 0, -1);
			GenRadial.ManualRadialPattern[31] = new IntVec3(1, 0, 3);
			GenRadial.ManualRadialPattern[32] = new IntVec3(-1, 0, -3);
			GenRadial.ManualRadialPattern[33] = new IntVec3(-3, 0, 1);
			GenRadial.ManualRadialPattern[34] = new IntVec3(3, 0, -1);
			GenRadial.ManualRadialPattern[35] = new IntVec3(-1, 0, 3);
			GenRadial.ManualRadialPattern[36] = new IntVec3(1, 0, -3);
			GenRadial.ManualRadialPattern[37] = new IntVec3(3, 0, 2);
			GenRadial.ManualRadialPattern[38] = new IntVec3(-3, 0, -2);
			GenRadial.ManualRadialPattern[39] = new IntVec3(2, 0, 3);
			GenRadial.ManualRadialPattern[40] = new IntVec3(-2, 0, -3);
			GenRadial.ManualRadialPattern[41] = new IntVec3(-3, 0, 2);
			GenRadial.ManualRadialPattern[42] = new IntVec3(3, 0, -2);
			GenRadial.ManualRadialPattern[43] = new IntVec3(-2, 0, 3);
			GenRadial.ManualRadialPattern[44] = new IntVec3(2, 0, -3);
			GenRadial.ManualRadialPattern[45] = new IntVec3(3, 0, 3);
			GenRadial.ManualRadialPattern[46] = new IntVec3(3, 0, -3);
			GenRadial.ManualRadialPattern[47] = new IntVec3(-3, 0, 3);
			GenRadial.ManualRadialPattern[48] = new IntVec3(-3, 0, -3);
		}

		// Token: 0x06005EA9 RID: 24233 RVA: 0x00302138 File Offset: 0x00300538
		private static void SetupRadialPattern()
		{
			List<IntVec3> list = new List<IntVec3>();
			for (int i = -60; i < 60; i++)
			{
				for (int j = -60; j < 60; j++)
				{
					list.Add(new IntVec3(i, 0, j));
				}
			}
			list.Sort(delegate(IntVec3 A, IntVec3 B)
			{
				float num = (float)A.LengthHorizontalSquared;
				float num2 = (float)B.LengthHorizontalSquared;
				int result;
				if (num < num2)
				{
					result = -1;
				}
				else if (num == num2)
				{
					result = 0;
				}
				else
				{
					result = 1;
				}
				return result;
			});
			for (int k = 0; k < 10000; k++)
			{
				GenRadial.RadialPattern[k] = list[k];
				GenRadial.RadialPatternRadii[k] = list[k].LengthHorizontal;
			}
		}

		// Token: 0x06005EAA RID: 24234 RVA: 0x003021F0 File Offset: 0x003005F0
		public static int NumCellsToFillForRadius_ManualRadialPattern(int radius)
		{
			int result;
			if (radius == 0)
			{
				result = 1;
			}
			else if (radius == 1)
			{
				result = 9;
			}
			else if (radius == 2)
			{
				result = 21;
			}
			else if (radius == 3)
			{
				result = 37;
			}
			else
			{
				Log.Error("NumSquares radius error", false);
				result = 0;
			}
			return result;
		}

		// Token: 0x06005EAB RID: 24235 RVA: 0x0030224C File Offset: 0x0030064C
		public static int NumCellsInRadius(float radius)
		{
			int result;
			if (radius >= GenRadial.MaxRadialPatternRadius)
			{
				Log.Error(string.Concat(new object[]
				{
					"Not enough squares to get to radius ",
					radius,
					". Max is ",
					GenRadial.MaxRadialPatternRadius
				}), false);
				result = 10000;
			}
			else
			{
				float num = radius + float.Epsilon;
				for (int i = 0; i < 10000; i++)
				{
					if (GenRadial.RadialPatternRadii[i] > num)
					{
						return i;
					}
				}
				result = 10000;
			}
			return result;
		}

		// Token: 0x06005EAC RID: 24236 RVA: 0x003022E8 File Offset: 0x003006E8
		public static float RadiusOfNumCells(int numCells)
		{
			return GenRadial.RadialPatternRadii[numCells];
		}

		// Token: 0x06005EAD RID: 24237 RVA: 0x00302304 File Offset: 0x00300704
		public static IEnumerable<IntVec3> RadialPatternInRadius(float radius)
		{
			int numSquares = GenRadial.NumCellsInRadius(radius);
			for (int i = 0; i < numSquares; i++)
			{
				yield return GenRadial.RadialPattern[i];
			}
			yield break;
		}

		// Token: 0x06005EAE RID: 24238 RVA: 0x00302330 File Offset: 0x00300730
		public static IEnumerable<IntVec3> RadialCellsAround(IntVec3 center, float radius, bool useCenter)
		{
			int numSquares = GenRadial.NumCellsInRadius(radius);
			for (int i = (!useCenter) ? 1 : 0; i < numSquares; i++)
			{
				yield return GenRadial.RadialPattern[i] + center;
			}
			yield break;
		}

		// Token: 0x06005EAF RID: 24239 RVA: 0x00302368 File Offset: 0x00300768
		public static IEnumerable<Thing> RadialDistinctThingsAround(IntVec3 center, Map map, float radius, bool useCenter)
		{
			int numCells = GenRadial.NumCellsInRadius(radius);
			HashSet<Thing> returnedThings = null;
			for (int i = (!useCenter) ? 1 : 0; i < numCells; i++)
			{
				IntVec3 cell = GenRadial.RadialPattern[i] + center;
				if (cell.InBounds(map))
				{
					List<Thing> thingList = cell.GetThingList(map);
					int j = 0;
					while (j < thingList.Count)
					{
						Thing t = thingList[j];
						if (t.def.size.x <= 1 || t.def.size.z <= 1)
						{
							goto IL_14F;
						}
						if (returnedThings == null)
						{
							returnedThings = new HashSet<Thing>();
						}
						if (!returnedThings.Contains(t))
						{
							returnedThings.Add(t);
							goto IL_14F;
						}
						IL_170:
						j++;
						continue;
						IL_14F:
						yield return t;
						goto IL_170;
					}
				}
			}
			yield break;
		}

		// Token: 0x06005EB0 RID: 24240 RVA: 0x003023A8 File Offset: 0x003007A8
		public static void ProcessEquidistantCells(IntVec3 center, float radius, Func<List<IntVec3>, bool> processor, Map map = null)
		{
			if (GenRadial.working)
			{
				Log.Error("Nested calls to ProcessEquidistantCells() are not allowed.", false);
			}
			else
			{
				GenRadial.tmpCells.Clear();
				GenRadial.working = true;
				try
				{
					float num = -1f;
					int num2 = GenRadial.NumCellsInRadius(radius);
					for (int i = 0; i < num2; i++)
					{
						IntVec3 intVec = center + GenRadial.RadialPattern[i];
						if (map == null || intVec.InBounds(map))
						{
							float num3 = (float)intVec.DistanceToSquared(center);
							if (Mathf.Abs(num3 - num) > 0.0001f)
							{
								if (GenRadial.tmpCells.Any<IntVec3>() && processor(GenRadial.tmpCells))
								{
									return;
								}
								num = num3;
								GenRadial.tmpCells.Clear();
							}
							GenRadial.tmpCells.Add(intVec);
						}
					}
					if (GenRadial.tmpCells.Any<IntVec3>())
					{
						processor(GenRadial.tmpCells);
					}
				}
				finally
				{
					GenRadial.tmpCells.Clear();
					GenRadial.working = false;
				}
			}
		}
	}
}
