using System;
using System.Runtime.CompilerServices;

namespace Verse
{
	public static class RoofCollapseUtility
	{
		public const float RoofMaxSupportDistance = 6.9f;

		public static readonly int RoofSupportRadialCellsCount = GenRadial.NumCellsInRadius(6.9f);

		public static bool WithinRangeOfRoofHolder(IntVec3 c, Map map, bool assumeNonNoRoofCellsAreRoofed = false)
		{
			bool connected = false;
			map.floodFiller.FloodFill(c, (IntVec3 x) => (x.Roofed(map) || x == c || (assumeNonNoRoofCellsAreRoofed && !map.areaManager.NoRoof[x])) && x.InHorDistOf(c, 6.9f), delegate(IntVec3 x)
			{
				for (int i = 0; i < 5; i++)
				{
					IntVec3 c2 = x + GenAdj.CardinalDirectionsAndInside[i];
					if (c2.InBounds(map) && c2.InHorDistOf(c, 6.9f))
					{
						Building edifice = c2.GetEdifice(map);
						if (edifice != null && edifice.def.holdsRoof)
						{
							connected = true;
							return true;
						}
					}
				}
				return false;
			}, int.MaxValue, false, null);
			return connected;
		}

		public static bool ConnectedToRoofHolder(IntVec3 c, Map map, bool assumeRoofAtRoot)
		{
			bool connected = false;
			map.floodFiller.FloodFill(c, (IntVec3 x) => (x.Roofed(map) || (x == c && assumeRoofAtRoot)) && !connected, delegate(IntVec3 x)
			{
				for (int i = 0; i < 5; i++)
				{
					IntVec3 c2 = x + GenAdj.CardinalDirectionsAndInside[i];
					if (c2.InBounds(map))
					{
						Building edifice = c2.GetEdifice(map);
						if (edifice != null && edifice.def.holdsRoof)
						{
							connected = true;
							break;
						}
					}
				}
			}, int.MaxValue, false, null);
			return connected;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static RoofCollapseUtility()
		{
		}

		[CompilerGenerated]
		private sealed class <WithinRangeOfRoofHolder>c__AnonStorey0
		{
			internal Map map;

			internal IntVec3 c;

			internal bool assumeNonNoRoofCellsAreRoofed;

			internal bool connected;

			public <WithinRangeOfRoofHolder>c__AnonStorey0()
			{
			}

			internal bool <>m__0(IntVec3 x)
			{
				return (x.Roofed(this.map) || x == this.c || (this.assumeNonNoRoofCellsAreRoofed && !this.map.areaManager.NoRoof[x])) && x.InHorDistOf(this.c, 6.9f);
			}

			internal bool <>m__1(IntVec3 x)
			{
				for (int i = 0; i < 5; i++)
				{
					IntVec3 intVec = x + GenAdj.CardinalDirectionsAndInside[i];
					if (intVec.InBounds(this.map) && intVec.InHorDistOf(this.c, 6.9f))
					{
						Building edifice = intVec.GetEdifice(this.map);
						if (edifice != null && edifice.def.holdsRoof)
						{
							this.connected = true;
							return true;
						}
					}
				}
				return false;
			}
		}

		[CompilerGenerated]
		private sealed class <ConnectedToRoofHolder>c__AnonStorey1
		{
			internal Map map;

			internal IntVec3 c;

			internal bool assumeRoofAtRoot;

			internal bool connected;

			public <ConnectedToRoofHolder>c__AnonStorey1()
			{
			}

			internal bool <>m__0(IntVec3 x)
			{
				return (x.Roofed(this.map) || (x == this.c && this.assumeRoofAtRoot)) && !this.connected;
			}

			internal void <>m__1(IntVec3 x)
			{
				for (int i = 0; i < 5; i++)
				{
					IntVec3 intVec = x + GenAdj.CardinalDirectionsAndInside[i];
					if (intVec.InBounds(this.map))
					{
						Building edifice = intVec.GetEdifice(this.map);
						if (edifice != null && edifice.def.holdsRoof)
						{
							this.connected = true;
							break;
						}
					}
				}
			}
		}
	}
}
