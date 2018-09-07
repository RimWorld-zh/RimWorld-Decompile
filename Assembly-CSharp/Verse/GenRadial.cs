using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace Verse
{
	public static class GenRadial
	{
		public static IntVec3[] ManualRadialPattern = new IntVec3[49];

		public static IntVec3[] RadialPattern = new IntVec3[10000];

		private static float[] RadialPatternRadii = new float[10000];

		private const int RadialPatternCount = 10000;

		private static List<IntVec3> tmpCells = new List<IntVec3>();

		private static bool working = false;

		[CompilerGenerated]
		private static Comparison<IntVec3> <>f__am$cache0;

		static GenRadial()
		{
			GenRadial.SetupManualRadialPattern();
			GenRadial.SetupRadialPattern();
		}

		public static float MaxRadialPatternRadius
		{
			get
			{
				return GenRadial.RadialPatternRadii[GenRadial.RadialPatternRadii.Length - 1];
			}
		}

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
				if (num < num2)
				{
					return -1;
				}
				if (num == num2)
				{
					return 0;
				}
				return 1;
			});
			for (int k = 0; k < 10000; k++)
			{
				GenRadial.RadialPattern[k] = list[k];
				GenRadial.RadialPatternRadii[k] = list[k].LengthHorizontal;
			}
		}

		public static int NumCellsToFillForRadius_ManualRadialPattern(int radius)
		{
			if (radius == 0)
			{
				return 1;
			}
			if (radius == 1)
			{
				return 9;
			}
			if (radius == 2)
			{
				return 21;
			}
			if (radius == 3)
			{
				return 37;
			}
			Log.Error("NumSquares radius error", false);
			return 0;
		}

		public static int NumCellsInRadius(float radius)
		{
			if (radius >= GenRadial.MaxRadialPatternRadius)
			{
				Log.Error(string.Concat(new object[]
				{
					"Not enough squares to get to radius ",
					radius,
					". Max is ",
					GenRadial.MaxRadialPatternRadius
				}), false);
				return 10000;
			}
			float num = radius + float.Epsilon;
			for (int i = 0; i < 10000; i++)
			{
				if (GenRadial.RadialPatternRadii[i] > num)
				{
					return i;
				}
			}
			return 10000;
		}

		public static float RadiusOfNumCells(int numCells)
		{
			return GenRadial.RadialPatternRadii[numCells];
		}

		public static IEnumerable<IntVec3> RadialPatternInRadius(float radius)
		{
			int numSquares = GenRadial.NumCellsInRadius(radius);
			for (int i = 0; i < numSquares; i++)
			{
				yield return GenRadial.RadialPattern[i];
			}
			yield break;
		}

		public static IEnumerable<IntVec3> RadialCellsAround(IntVec3 center, float radius, bool useCenter)
		{
			int numSquares = GenRadial.NumCellsInRadius(radius);
			for (int i = (!useCenter) ? 1 : 0; i < numSquares; i++)
			{
				yield return GenRadial.RadialPattern[i] + center;
			}
			yield break;
		}

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
							goto IL_14A;
						}
						if (returnedThings == null)
						{
							returnedThings = new HashSet<Thing>();
						}
						if (!returnedThings.Contains(t))
						{
							returnedThings.Add(t);
							goto IL_14A;
						}
						IL_16A:
						j++;
						continue;
						IL_14A:
						yield return t;
						goto IL_16A;
					}
				}
			}
			yield break;
		}

		public static void ProcessEquidistantCells(IntVec3 center, float radius, Func<List<IntVec3>, bool> processor, Map map = null)
		{
			if (GenRadial.working)
			{
				Log.Error("Nested calls to ProcessEquidistantCells() are not allowed.", false);
				return;
			}
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

		[CompilerGenerated]
		private static int <SetupRadialPattern>m__0(IntVec3 A, IntVec3 B)
		{
			float num = (float)A.LengthHorizontalSquared;
			float num2 = (float)B.LengthHorizontalSquared;
			if (num < num2)
			{
				return -1;
			}
			if (num == num2)
			{
				return 0;
			}
			return 1;
		}

		[CompilerGenerated]
		private sealed class <RadialPatternInRadius>c__Iterator0 : IEnumerable, IEnumerable<IntVec3>, IEnumerator, IDisposable, IEnumerator<IntVec3>
		{
			internal float radius;

			internal int <numSquares>__0;

			internal int <i>__1;

			internal IntVec3 $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <RadialPatternInRadius>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					numSquares = GenRadial.NumCellsInRadius(radius);
					i = 0;
					break;
				case 1u:
					i++;
					break;
				default:
					return false;
				}
				if (i < numSquares)
				{
					this.$current = GenRadial.RadialPattern[i];
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				this.$PC = -1;
				return false;
			}

			IntVec3 IEnumerator<IntVec3>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.IntVec3>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<IntVec3> IEnumerable<IntVec3>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				GenRadial.<RadialPatternInRadius>c__Iterator0 <RadialPatternInRadius>c__Iterator = new GenRadial.<RadialPatternInRadius>c__Iterator0();
				<RadialPatternInRadius>c__Iterator.radius = radius;
				return <RadialPatternInRadius>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <RadialCellsAround>c__Iterator1 : IEnumerable, IEnumerable<IntVec3>, IEnumerator, IDisposable, IEnumerator<IntVec3>
		{
			internal float radius;

			internal int <numSquares>__0;

			internal bool useCenter;

			internal int <i>__1;

			internal IntVec3 center;

			internal IntVec3 $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <RadialCellsAround>c__Iterator1()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					numSquares = GenRadial.NumCellsInRadius(radius);
					i = ((!useCenter) ? 1 : 0);
					break;
				case 1u:
					i++;
					break;
				default:
					return false;
				}
				if (i < numSquares)
				{
					this.$current = GenRadial.RadialPattern[i] + center;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				this.$PC = -1;
				return false;
			}

			IntVec3 IEnumerator<IntVec3>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.IntVec3>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<IntVec3> IEnumerable<IntVec3>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				GenRadial.<RadialCellsAround>c__Iterator1 <RadialCellsAround>c__Iterator = new GenRadial.<RadialCellsAround>c__Iterator1();
				<RadialCellsAround>c__Iterator.radius = radius;
				<RadialCellsAround>c__Iterator.useCenter = useCenter;
				<RadialCellsAround>c__Iterator.center = center;
				return <RadialCellsAround>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <RadialDistinctThingsAround>c__Iterator2 : IEnumerable, IEnumerable<Thing>, IEnumerator, IDisposable, IEnumerator<Thing>
		{
			internal float radius;

			internal int <numCells>__0;

			internal HashSet<Thing> <returnedThings>__0;

			internal bool useCenter;

			internal int <i>__1;

			internal IntVec3 center;

			internal IntVec3 <cell>__2;

			internal Map map;

			internal List<Thing> <thingList>__2;

			internal int <j>__3;

			internal Thing <t>__4;

			internal Thing $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <RadialDistinctThingsAround>c__Iterator2()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					numCells = GenRadial.NumCellsInRadius(radius);
					returnedThings = null;
					i = ((!useCenter) ? 1 : 0);
					goto IL_19C;
				case 1u:
					IL_16A:
					j++;
					break;
				default:
					return false;
				}
				IL_178:
				if (j < thingList.Count)
				{
					t = thingList[j];
					if (t.def.size.x > 1 && t.def.size.z > 1)
					{
						if (returnedThings == null)
						{
							returnedThings = new HashSet<Thing>();
						}
						if (returnedThings.Contains(t))
						{
							goto IL_16A;
						}
						returnedThings.Add(t);
					}
					this.$current = t;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				IL_18E:
				i++;
				IL_19C:
				if (i >= numCells)
				{
					this.$PC = -1;
				}
				else
				{
					cell = GenRadial.RadialPattern[i] + center;
					if (!cell.InBounds(map))
					{
						goto IL_18E;
					}
					thingList = cell.GetThingList(map);
					j = 0;
					goto IL_178;
				}
				return false;
			}

			Thing IEnumerator<Thing>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.Thing>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				GenRadial.<RadialDistinctThingsAround>c__Iterator2 <RadialDistinctThingsAround>c__Iterator = new GenRadial.<RadialDistinctThingsAround>c__Iterator2();
				<RadialDistinctThingsAround>c__Iterator.radius = radius;
				<RadialDistinctThingsAround>c__Iterator.useCenter = useCenter;
				<RadialDistinctThingsAround>c__Iterator.center = center;
				<RadialDistinctThingsAround>c__Iterator.map = map;
				return <RadialDistinctThingsAround>c__Iterator;
			}
		}
	}
}
