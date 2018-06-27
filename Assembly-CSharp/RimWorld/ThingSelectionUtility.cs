using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class ThingSelectionUtility
	{
		private static HashSet<Thing> yieldedThings = new HashSet<Thing>();

		private static HashSet<Zone> yieldedZones = new HashSet<Zone>();

		private static List<Pawn> tmpColonists = new List<Pawn>();

		[CompilerGenerated]
		private static Func<Pawn, bool> <>f__mg$cache0;

		[CompilerGenerated]
		private static Func<Pawn, bool> <>f__mg$cache1;

		public static bool SelectableByMapClick(Thing t)
		{
			bool result;
			if (!t.def.selectable)
			{
				result = false;
			}
			else if (!t.Spawned)
			{
				result = false;
			}
			else if (t.def.size.x == 1 && t.def.size.z == 1)
			{
				result = !t.Position.Fogged(t.Map);
			}
			else
			{
				CellRect.CellRectIterator iterator = t.OccupiedRect().GetIterator();
				while (!iterator.Done())
				{
					if (!iterator.Current.Fogged(t.Map))
					{
						return true;
					}
					iterator.MoveNext();
				}
				result = false;
			}
			return result;
		}

		public static bool SelectableByHotkey(Thing t)
		{
			return t.def.selectable && t.Spawned;
		}

		public static IEnumerable<Thing> MultiSelectableThingsInScreenRectDistinct(Rect rect)
		{
			CellRect mapRect = ThingSelectionUtility.GetMapRect(rect);
			ThingSelectionUtility.yieldedThings.Clear();
			try
			{
				foreach (IntVec3 c in mapRect)
				{
					if (c.InBounds(Find.CurrentMap))
					{
						List<Thing> cellThings = Find.CurrentMap.thingGrid.ThingsListAt(c);
						if (cellThings != null)
						{
							for (int i = 0; i < cellThings.Count; i++)
							{
								Thing t = cellThings[i];
								if (ThingSelectionUtility.SelectableByMapClick(t) && !t.def.neverMultiSelect && !ThingSelectionUtility.yieldedThings.Contains(t))
								{
									yield return t;
									ThingSelectionUtility.yieldedThings.Add(t);
								}
							}
						}
					}
				}
			}
			finally
			{
				ThingSelectionUtility.yieldedThings.Clear();
			}
			yield break;
		}

		public static IEnumerable<Zone> MultiSelectableZonesInScreenRectDistinct(Rect rect)
		{
			CellRect mapRect = ThingSelectionUtility.GetMapRect(rect);
			ThingSelectionUtility.yieldedZones.Clear();
			try
			{
				foreach (IntVec3 c in mapRect)
				{
					if (c.InBounds(Find.CurrentMap))
					{
						Zone zone = c.GetZone(Find.CurrentMap);
						if (zone != null)
						{
							if (zone.IsMultiselectable)
							{
								if (!ThingSelectionUtility.yieldedZones.Contains(zone))
								{
									yield return zone;
									ThingSelectionUtility.yieldedZones.Add(zone);
								}
							}
						}
					}
				}
			}
			finally
			{
				ThingSelectionUtility.yieldedZones.Clear();
			}
			yield break;
		}

		private static CellRect GetMapRect(Rect rect)
		{
			Vector2 screenLoc = new Vector2(rect.x, (float)UI.screenHeight - rect.y);
			Vector2 screenLoc2 = new Vector2(rect.x + rect.width, (float)UI.screenHeight - (rect.y + rect.height));
			Vector3 vector = UI.UIToMapPosition(screenLoc);
			Vector3 vector2 = UI.UIToMapPosition(screenLoc2);
			return new CellRect
			{
				minX = Mathf.FloorToInt(vector.x),
				minZ = Mathf.FloorToInt(vector2.z),
				maxX = Mathf.FloorToInt(vector2.x),
				maxZ = Mathf.FloorToInt(vector.z)
			};
		}

		public static void SelectNextColonist()
		{
			ThingSelectionUtility.tmpColonists.Clear();
			List<Pawn> list = ThingSelectionUtility.tmpColonists;
			IEnumerable<Pawn> colonistsInOrder = Find.ColonistBar.GetColonistsInOrder();
			if (ThingSelectionUtility.<>f__mg$cache0 == null)
			{
				ThingSelectionUtility.<>f__mg$cache0 = new Func<Pawn, bool>(ThingSelectionUtility.SelectableByHotkey);
			}
			list.AddRange(colonistsInOrder.Where(ThingSelectionUtility.<>f__mg$cache0));
			if (ThingSelectionUtility.tmpColonists.Count != 0)
			{
				bool worldRenderedNow = WorldRendererUtility.WorldRenderedNow;
				int num = -1;
				for (int i = ThingSelectionUtility.tmpColonists.Count - 1; i >= 0; i--)
				{
					if ((!worldRenderedNow && Find.Selector.IsSelected(ThingSelectionUtility.tmpColonists[i])) || (worldRenderedNow && ThingSelectionUtility.tmpColonists[i].IsCaravanMember() && Find.WorldSelector.IsSelected(ThingSelectionUtility.tmpColonists[i].GetCaravan())))
					{
						num = i;
						break;
					}
				}
				if (num == -1)
				{
					CameraJumper.TryJumpAndSelect(ThingSelectionUtility.tmpColonists[0]);
				}
				else
				{
					CameraJumper.TryJumpAndSelect(ThingSelectionUtility.tmpColonists[(num + 1) % ThingSelectionUtility.tmpColonists.Count]);
				}
				ThingSelectionUtility.tmpColonists.Clear();
			}
		}

		public static void SelectPreviousColonist()
		{
			ThingSelectionUtility.tmpColonists.Clear();
			List<Pawn> list = ThingSelectionUtility.tmpColonists;
			IEnumerable<Pawn> colonistsInOrder = Find.ColonistBar.GetColonistsInOrder();
			if (ThingSelectionUtility.<>f__mg$cache1 == null)
			{
				ThingSelectionUtility.<>f__mg$cache1 = new Func<Pawn, bool>(ThingSelectionUtility.SelectableByHotkey);
			}
			list.AddRange(colonistsInOrder.Where(ThingSelectionUtility.<>f__mg$cache1));
			if (ThingSelectionUtility.tmpColonists.Count != 0)
			{
				bool worldRenderedNow = WorldRendererUtility.WorldRenderedNow;
				int num = -1;
				for (int i = 0; i < ThingSelectionUtility.tmpColonists.Count; i++)
				{
					if ((!worldRenderedNow && Find.Selector.IsSelected(ThingSelectionUtility.tmpColonists[i])) || (worldRenderedNow && ThingSelectionUtility.tmpColonists[i].IsCaravanMember() && Find.WorldSelector.IsSelected(ThingSelectionUtility.tmpColonists[i].GetCaravan())))
					{
						num = i;
						break;
					}
				}
				if (num == -1)
				{
					CameraJumper.TryJumpAndSelect(ThingSelectionUtility.tmpColonists[ThingSelectionUtility.tmpColonists.Count - 1]);
				}
				else
				{
					CameraJumper.TryJumpAndSelect(ThingSelectionUtility.tmpColonists[GenMath.PositiveMod(num - 1, ThingSelectionUtility.tmpColonists.Count)]);
				}
				ThingSelectionUtility.tmpColonists.Clear();
			}
		}

		// Note: this type is marked as 'beforefieldinit'.
		static ThingSelectionUtility()
		{
		}

		[CompilerGenerated]
		private sealed class <MultiSelectableThingsInScreenRectDistinct>c__Iterator0 : IEnumerable, IEnumerable<Thing>, IEnumerator, IDisposable, IEnumerator<Thing>
		{
			internal Rect rect;

			internal CellRect <mapRect>__0;

			internal IEnumerator<IntVec3> $locvar0;

			internal IntVec3 <c>__1;

			internal List<Thing> <cellThings>__2;

			internal int <k>__3;

			internal Thing <t>__4;

			internal Thing $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <MultiSelectableThingsInScreenRectDistinct>c__Iterator0()
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
					mapRect = ThingSelectionUtility.GetMapRect(rect);
					ThingSelectionUtility.yieldedThings.Clear();
					num = 4294967293u;
					break;
				case 1u:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					case 1u:
						break;
					default:
						enumerator = mapRect.GetEnumerator();
						num = 4294967293u;
						break;
					}
					try
					{
						switch (num)
						{
						case 1u:
							ThingSelectionUtility.yieldedThings.Add(t);
							goto IL_15B;
						}
						IL_182:
						while (enumerator.MoveNext())
						{
							c = enumerator.Current;
							if (c.InBounds(Find.CurrentMap))
							{
								cellThings = Find.CurrentMap.thingGrid.ThingsListAt(c);
								if (cellThings != null)
								{
									i = 0;
									goto IL_16A;
								}
							}
						}
						goto IL_1B2;
						IL_15B:
						i++;
						IL_16A:
						if (i < cellThings.Count)
						{
							t = cellThings[i];
							if (ThingSelectionUtility.SelectableByMapClick(t) && !t.def.neverMultiSelect && !ThingSelectionUtility.yieldedThings.Contains(t))
							{
								this.$current = t;
								if (!this.$disposing)
								{
									this.$PC = 1;
								}
								flag = true;
								return true;
							}
							goto IL_15B;
						}
						goto IL_182;
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
					IL_1B2:;
				}
				finally
				{
					if (!flag)
					{
						this.<>__Finally0();
					}
				}
				this.$PC = -1;
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
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
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
					}
					finally
					{
						this.<>__Finally0();
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
				return this.System.Collections.Generic.IEnumerable<Verse.Thing>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				ThingSelectionUtility.<MultiSelectableThingsInScreenRectDistinct>c__Iterator0 <MultiSelectableThingsInScreenRectDistinct>c__Iterator = new ThingSelectionUtility.<MultiSelectableThingsInScreenRectDistinct>c__Iterator0();
				<MultiSelectableThingsInScreenRectDistinct>c__Iterator.rect = rect;
				return <MultiSelectableThingsInScreenRectDistinct>c__Iterator;
			}

			private void <>__Finally0()
			{
				ThingSelectionUtility.yieldedThings.Clear();
			}
		}

		[CompilerGenerated]
		private sealed class <MultiSelectableZonesInScreenRectDistinct>c__Iterator1 : IEnumerable, IEnumerable<Zone>, IEnumerator, IDisposable, IEnumerator<Zone>
		{
			internal Rect rect;

			internal CellRect <mapRect>__0;

			internal IEnumerator<IntVec3> $locvar0;

			internal IntVec3 <c>__1;

			internal Zone <zone>__2;

			internal Zone $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <MultiSelectableZonesInScreenRectDistinct>c__Iterator1()
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
					mapRect = ThingSelectionUtility.GetMapRect(rect);
					ThingSelectionUtility.yieldedZones.Clear();
					num = 4294967293u;
					break;
				case 1u:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					case 1u:
						break;
					default:
						enumerator = mapRect.GetEnumerator();
						num = 4294967293u;
						break;
					}
					try
					{
						switch (num)
						{
						case 1u:
							ThingSelectionUtility.yieldedZones.Add(zone);
							break;
						}
						IL_127:
						while (enumerator.MoveNext())
						{
							c = enumerator.Current;
							if (c.InBounds(Find.CurrentMap))
							{
								zone = c.GetZone(Find.CurrentMap);
								if (zone != null)
								{
									if (zone.IsMultiselectable)
									{
										if (!ThingSelectionUtility.yieldedZones.Contains(zone))
										{
											this.$current = zone;
											if (!this.$disposing)
											{
												this.$PC = 1;
											}
											flag = true;
											return true;
										}
									}
								}
							}
						}
						goto IL_157;
						goto IL_127;
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
					IL_157:;
				}
				finally
				{
					if (!flag)
					{
						this.<>__Finally0();
					}
				}
				this.$PC = -1;
				return false;
			}

			Zone IEnumerator<Zone>.Current
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
					}
					finally
					{
						this.<>__Finally0();
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
				return this.System.Collections.Generic.IEnumerable<Verse.Zone>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Zone> IEnumerable<Zone>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				ThingSelectionUtility.<MultiSelectableZonesInScreenRectDistinct>c__Iterator1 <MultiSelectableZonesInScreenRectDistinct>c__Iterator = new ThingSelectionUtility.<MultiSelectableZonesInScreenRectDistinct>c__Iterator1();
				<MultiSelectableZonesInScreenRectDistinct>c__Iterator.rect = rect;
				return <MultiSelectableZonesInScreenRectDistinct>c__Iterator;
			}

			private void <>__Finally0()
			{
				ThingSelectionUtility.yieldedZones.Clear();
			}
		}
	}
}
