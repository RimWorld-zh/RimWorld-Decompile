using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public static class SiegeBlueprintPlacer
	{
		private static IntVec3 center;

		private static Faction faction;

		private static List<IntVec3> placedSandbagLocs = new List<IntVec3>();

		private const int MaxArtyCount = 2;

		public const float ArtyCost = 60f;

		private const int MinSandbagDistSquared = 36;

		private static readonly IntRange NumSandbagRange = new IntRange(2, 4);

		private static readonly IntRange SandbagLengthRange = new IntRange(2, 7);

		public static IEnumerable<Blueprint_Build> PlaceBlueprints(IntVec3 placeCenter, Map map, Faction placeFaction, float points)
		{
			SiegeBlueprintPlacer.center = placeCenter;
			SiegeBlueprintPlacer.faction = placeFaction;
			foreach (Blueprint_Build blue in SiegeBlueprintPlacer.PlaceSandbagBlueprints(map))
			{
				yield return blue;
			}
			foreach (Blueprint_Build blue2 in SiegeBlueprintPlacer.PlaceArtilleryBlueprints(points, map))
			{
				yield return blue2;
			}
			yield break;
		}

		private static bool CanPlaceBlueprintAt(IntVec3 root, Rot4 rot, ThingDef buildingDef, Map map)
		{
			return GenConstruct.CanPlaceBlueprintAt(buildingDef, root, rot, map, false, null).Accepted;
		}

		private static IEnumerable<Blueprint_Build> PlaceSandbagBlueprints(Map map)
		{
			SiegeBlueprintPlacer.placedSandbagLocs.Clear();
			int numSandbags = SiegeBlueprintPlacer.NumSandbagRange.RandomInRange;
			for (int i = 0; i < numSandbags; i++)
			{
				IntVec3 bagRoot = SiegeBlueprintPlacer.FindSandbagRoot(map);
				if (!bagRoot.IsValid)
				{
					yield break;
				}
				Rot4 growDirA;
				if (bagRoot.x > SiegeBlueprintPlacer.center.x)
				{
					growDirA = Rot4.West;
				}
				else
				{
					growDirA = Rot4.East;
				}
				Rot4 growDirB;
				if (bagRoot.z > SiegeBlueprintPlacer.center.z)
				{
					growDirB = Rot4.South;
				}
				else
				{
					growDirB = Rot4.North;
				}
				foreach (Blueprint_Build bag in SiegeBlueprintPlacer.MakeSandbagLine(bagRoot, map, growDirA, SiegeBlueprintPlacer.SandbagLengthRange.RandomInRange))
				{
					yield return bag;
				}
				bagRoot += growDirB.FacingCell;
				foreach (Blueprint_Build bag2 in SiegeBlueprintPlacer.MakeSandbagLine(bagRoot, map, growDirB, SiegeBlueprintPlacer.SandbagLengthRange.RandomInRange))
				{
					yield return bag2;
				}
			}
			yield break;
		}

		private static IEnumerable<Blueprint_Build> MakeSandbagLine(IntVec3 root, Map map, Rot4 growDir, int maxLength)
		{
			IntVec3 cur = root;
			for (int i = 0; i < maxLength; i++)
			{
				if (!SiegeBlueprintPlacer.CanPlaceBlueprintAt(cur, Rot4.North, ThingDefOf.Sandbags, map))
				{
					break;
				}
				yield return GenConstruct.PlaceBlueprintForBuild(ThingDefOf.Sandbags, cur, map, Rot4.North, SiegeBlueprintPlacer.faction, null);
				SiegeBlueprintPlacer.placedSandbagLocs.Add(cur);
				cur += growDir.FacingCell;
			}
			yield break;
		}

		private static IEnumerable<Blueprint_Build> PlaceArtilleryBlueprints(float points, Map map)
		{
			IEnumerable<ThingDef> artyDefs = from def in DefDatabase<ThingDef>.AllDefs
			where def.building != null && def.building.buildingTags.Contains("Artillery_BaseDestroyer")
			select def;
			int numArtillery = Mathf.RoundToInt(points / 60f);
			numArtillery = Mathf.Clamp(numArtillery, 1, 2);
			for (int i = 0; i < numArtillery; i++)
			{
				Rot4 rot = Rot4.Random;
				ThingDef artyDef = artyDefs.RandomElement<ThingDef>();
				IntVec3 artySpot = SiegeBlueprintPlacer.FindArtySpot(artyDef, rot, map);
				if (!artySpot.IsValid)
				{
					yield break;
				}
				yield return GenConstruct.PlaceBlueprintForBuild(artyDef, artySpot, map, rot, SiegeBlueprintPlacer.faction, ThingDefOf.Steel);
				points -= 60f;
			}
			yield break;
		}

		private static IntVec3 FindSandbagRoot(Map map)
		{
			CellRect cellRect = CellRect.CenteredOn(SiegeBlueprintPlacer.center, 13);
			cellRect.ClipInsideMap(map);
			CellRect cellRect2 = CellRect.CenteredOn(SiegeBlueprintPlacer.center, 8);
			cellRect2.ClipInsideMap(map);
			int num = 0;
			for (;;)
			{
				num++;
				if (num > 200)
				{
					break;
				}
				IntVec3 randomCell = cellRect.RandomCell;
				if (!cellRect2.Contains(randomCell))
				{
					if (map.reachability.CanReach(randomCell, SiegeBlueprintPlacer.center, PathEndMode.OnCell, TraverseMode.NoPassClosedDoors, Danger.Deadly))
					{
						if (SiegeBlueprintPlacer.CanPlaceBlueprintAt(randomCell, Rot4.North, ThingDefOf.Sandbags, map))
						{
							bool flag = false;
							for (int i = 0; i < SiegeBlueprintPlacer.placedSandbagLocs.Count; i++)
							{
								float num2 = (float)(SiegeBlueprintPlacer.placedSandbagLocs[i] - randomCell).LengthHorizontalSquared;
								if (num2 < 36f)
								{
									flag = true;
								}
							}
							if (!flag)
							{
								return randomCell;
							}
						}
					}
				}
			}
			return IntVec3.Invalid;
		}

		private static IntVec3 FindArtySpot(ThingDef artyDef, Rot4 rot, Map map)
		{
			CellRect cellRect = CellRect.CenteredOn(SiegeBlueprintPlacer.center, 8);
			cellRect.ClipInsideMap(map);
			int num = 0;
			for (;;)
			{
				num++;
				if (num > 200)
				{
					break;
				}
				IntVec3 randomCell = cellRect.RandomCell;
				if (map.reachability.CanReach(randomCell, SiegeBlueprintPlacer.center, PathEndMode.OnCell, TraverseMode.NoPassClosedDoors, Danger.Deadly))
				{
					if (!randomCell.Roofed(map))
					{
						if (SiegeBlueprintPlacer.CanPlaceBlueprintAt(randomCell, rot, artyDef, map))
						{
							return randomCell;
						}
					}
				}
			}
			return IntVec3.Invalid;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static SiegeBlueprintPlacer()
		{
		}

		[CompilerGenerated]
		private sealed class <PlaceBlueprints>c__Iterator0 : IEnumerable, IEnumerable<Blueprint_Build>, IEnumerator, IDisposable, IEnumerator<Blueprint_Build>
		{
			internal IntVec3 placeCenter;

			internal Faction placeFaction;

			internal Map map;

			internal IEnumerator<Blueprint_Build> $locvar0;

			internal Blueprint_Build <blue>__1;

			internal float points;

			internal IEnumerator<Blueprint_Build> $locvar1;

			internal Blueprint_Build <blue>__2;

			internal Blueprint_Build $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <PlaceBlueprints>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					SiegeBlueprintPlacer.center = placeCenter;
					SiegeBlueprintPlacer.faction = placeFaction;
					enumerator = SiegeBlueprintPlacer.PlaceSandbagBlueprints(map).GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					goto IL_E9;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						blue = enumerator.Current;
						this.$current = blue;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				enumerator2 = SiegeBlueprintPlacer.PlaceArtilleryBlueprints(points, map).GetEnumerator();
				num = 4294967293u;
				try
				{
					IL_E9:
					switch (num)
					{
					}
					if (enumerator2.MoveNext())
					{
						blue2 = enumerator2.Current;
						this.$current = blue2;
						if (!this.$disposing)
						{
							this.$PC = 2;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator2 != null)
						{
							enumerator2.Dispose();
						}
					}
				}
				this.$PC = -1;
				return false;
			}

			Blueprint_Build IEnumerator<Blueprint_Build>.Current
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
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				case 2u:
					try
					{
					}
					finally
					{
						if (enumerator2 != null)
						{
							enumerator2.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<RimWorld.Blueprint_Build>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Blueprint_Build> IEnumerable<Blueprint_Build>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				SiegeBlueprintPlacer.<PlaceBlueprints>c__Iterator0 <PlaceBlueprints>c__Iterator = new SiegeBlueprintPlacer.<PlaceBlueprints>c__Iterator0();
				<PlaceBlueprints>c__Iterator.placeCenter = placeCenter;
				<PlaceBlueprints>c__Iterator.placeFaction = placeFaction;
				<PlaceBlueprints>c__Iterator.map = map;
				<PlaceBlueprints>c__Iterator.points = points;
				return <PlaceBlueprints>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <PlaceSandbagBlueprints>c__Iterator1 : IEnumerable, IEnumerable<Blueprint_Build>, IEnumerator, IDisposable, IEnumerator<Blueprint_Build>
		{
			internal int <numSandbags>__0;

			internal int <i>__1;

			internal Map map;

			internal IntVec3 <bagRoot>__2;

			internal Rot4 <growDirA>__2;

			internal Rot4 <growDirB>__2;

			internal IEnumerator<Blueprint_Build> $locvar0;

			internal Blueprint_Build <bag>__3;

			internal IEnumerator<Blueprint_Build> $locvar1;

			internal Blueprint_Build <bag>__4;

			internal Blueprint_Build $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <PlaceSandbagBlueprints>c__Iterator1()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					SiegeBlueprintPlacer.placedSandbagLocs.Clear();
					numSandbags = SiegeBlueprintPlacer.NumSandbagRange.RandomInRange;
					i = 0;
					goto IL_257;
				case 1u:
					Block_5:
					try
					{
						switch (num)
						{
						}
						if (enumerator.MoveNext())
						{
							bag = enumerator.Current;
							this.$current = bag;
							if (!this.$disposing)
							{
								this.$PC = 1;
							}
							flag = true;
							return true;
						}
					}
					finally
					{
						if (!flag)
						{
							if (enumerator != null)
							{
								enumerator.Dispose();
							}
						}
					}
					bagRoot += growDirB.FacingCell;
					enumerator2 = SiegeBlueprintPlacer.MakeSandbagLine(bagRoot, map, growDirB, SiegeBlueprintPlacer.SandbagLengthRange.RandomInRange).GetEnumerator();
					num = 4294967293u;
					break;
				case 2u:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator2.MoveNext())
					{
						bag2 = enumerator2.Current;
						this.$current = bag2;
						if (!this.$disposing)
						{
							this.$PC = 2;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator2 != null)
						{
							enumerator2.Dispose();
						}
					}
				}
				i++;
				IL_257:
				if (i >= numSandbags)
				{
					this.$PC = -1;
				}
				else
				{
					bagRoot = SiegeBlueprintPlacer.FindSandbagRoot(map);
					if (bagRoot.IsValid)
					{
						if (bagRoot.x > SiegeBlueprintPlacer.center.x)
						{
							growDirA = Rot4.West;
						}
						else
						{
							growDirA = Rot4.East;
						}
						if (bagRoot.z > SiegeBlueprintPlacer.center.z)
						{
							growDirB = Rot4.South;
						}
						else
						{
							growDirB = Rot4.North;
						}
						enumerator = SiegeBlueprintPlacer.MakeSandbagLine(bagRoot, map, growDirA, SiegeBlueprintPlacer.SandbagLengthRange.RandomInRange).GetEnumerator();
						num = 4294967293u;
						goto Block_5;
					}
				}
				return false;
			}

			Blueprint_Build IEnumerator<Blueprint_Build>.Current
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
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				case 2u:
					try
					{
					}
					finally
					{
						if (enumerator2 != null)
						{
							enumerator2.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<RimWorld.Blueprint_Build>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Blueprint_Build> IEnumerable<Blueprint_Build>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				SiegeBlueprintPlacer.<PlaceSandbagBlueprints>c__Iterator1 <PlaceSandbagBlueprints>c__Iterator = new SiegeBlueprintPlacer.<PlaceSandbagBlueprints>c__Iterator1();
				<PlaceSandbagBlueprints>c__Iterator.map = map;
				return <PlaceSandbagBlueprints>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <MakeSandbagLine>c__Iterator2 : IEnumerable, IEnumerable<Blueprint_Build>, IEnumerator, IDisposable, IEnumerator<Blueprint_Build>
		{
			internal IntVec3 root;

			internal IntVec3 <cur>__0;

			internal int <j>__1;

			internal int maxLength;

			internal Map map;

			internal Rot4 growDir;

			internal Blueprint_Build $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <MakeSandbagLine>c__Iterator2()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					cur = root;
					i = 0;
					break;
				case 1u:
					SiegeBlueprintPlacer.placedSandbagLocs.Add(cur);
					cur += growDir.FacingCell;
					i++;
					break;
				default:
					return false;
				}
				if (i < maxLength)
				{
					if (SiegeBlueprintPlacer.CanPlaceBlueprintAt(cur, Rot4.North, ThingDefOf.Sandbags, map))
					{
						this.$current = GenConstruct.PlaceBlueprintForBuild(ThingDefOf.Sandbags, cur, map, Rot4.North, SiegeBlueprintPlacer.faction, null);
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
				}
				this.$PC = -1;
				return false;
			}

			Blueprint_Build IEnumerator<Blueprint_Build>.Current
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
				return this.System.Collections.Generic.IEnumerable<RimWorld.Blueprint_Build>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Blueprint_Build> IEnumerable<Blueprint_Build>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				SiegeBlueprintPlacer.<MakeSandbagLine>c__Iterator2 <MakeSandbagLine>c__Iterator = new SiegeBlueprintPlacer.<MakeSandbagLine>c__Iterator2();
				<MakeSandbagLine>c__Iterator.root = root;
				<MakeSandbagLine>c__Iterator.maxLength = maxLength;
				<MakeSandbagLine>c__Iterator.map = map;
				<MakeSandbagLine>c__Iterator.growDir = growDir;
				return <MakeSandbagLine>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <PlaceArtilleryBlueprints>c__Iterator3 : IEnumerable, IEnumerable<Blueprint_Build>, IEnumerator, IDisposable, IEnumerator<Blueprint_Build>
		{
			internal IEnumerable<ThingDef> <artyDefs>__0;

			internal float points;

			internal int <numArtillery>__0;

			internal int <i>__1;

			internal Rot4 <rot>__2;

			internal ThingDef <artyDef>__2;

			internal Map map;

			internal IntVec3 <artySpot>__2;

			internal Blueprint_Build $current;

			internal bool $disposing;

			internal float <$>points;

			internal int $PC;

			private static Func<ThingDef, bool> <>f__am$cache0;

			[DebuggerHidden]
			public <PlaceArtilleryBlueprints>c__Iterator3()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					artyDefs = from def in DefDatabase<ThingDef>.AllDefs
					where def.building != null && def.building.buildingTags.Contains("Artillery_BaseDestroyer")
					select def;
					numArtillery = Mathf.RoundToInt(points / 60f);
					numArtillery = Mathf.Clamp(numArtillery, 1, 2);
					i = 0;
					break;
				case 1u:
					points -= 60f;
					i++;
					break;
				default:
					return false;
				}
				if (i >= numArtillery)
				{
					this.$PC = -1;
				}
				else
				{
					rot = Rot4.Random;
					artyDef = artyDefs.RandomElement<ThingDef>();
					artySpot = SiegeBlueprintPlacer.FindArtySpot(artyDef, rot, map);
					if (artySpot.IsValid)
					{
						this.$current = GenConstruct.PlaceBlueprintForBuild(artyDef, artySpot, map, rot, SiegeBlueprintPlacer.faction, ThingDefOf.Steel);
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
				}
				return false;
			}

			Blueprint_Build IEnumerator<Blueprint_Build>.Current
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
				return this.System.Collections.Generic.IEnumerable<RimWorld.Blueprint_Build>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Blueprint_Build> IEnumerable<Blueprint_Build>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				SiegeBlueprintPlacer.<PlaceArtilleryBlueprints>c__Iterator3 <PlaceArtilleryBlueprints>c__Iterator = new SiegeBlueprintPlacer.<PlaceArtilleryBlueprints>c__Iterator3();
				<PlaceArtilleryBlueprints>c__Iterator.points = points;
				<PlaceArtilleryBlueprints>c__Iterator.map = map;
				return <PlaceArtilleryBlueprints>c__Iterator;
			}

			private static bool <>m__0(ThingDef def)
			{
				return def.building != null && def.building.buildingTags.Contains("Artillery_BaseDestroyer");
			}
		}
	}
}
