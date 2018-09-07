using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	public static class WorldObjectSelectionUtility
	{
		public static IEnumerable<WorldObject> MultiSelectableWorldObjectsInScreenRectDistinct(Rect rect)
		{
			List<WorldObject> allObjects = Find.WorldObjects.AllWorldObjects;
			for (int i = 0; i < allObjects.Count; i++)
			{
				if (!allObjects[i].NeverMultiSelect)
				{
					if (!allObjects[i].HiddenBehindTerrainNow())
					{
						if (ExpandableWorldObjectsUtility.IsExpanded(allObjects[i]))
						{
							if (rect.Overlaps(ExpandableWorldObjectsUtility.ExpandedIconScreenRect(allObjects[i])))
							{
								yield return allObjects[i];
							}
						}
						else if (rect.Contains(allObjects[i].ScreenPos()))
						{
							yield return allObjects[i];
						}
					}
				}
			}
			yield break;
		}

		public static bool HiddenBehindTerrainNow(this WorldObject o)
		{
			return WorldRendererUtility.HiddenBehindTerrainNow(o.DrawPos);
		}

		public static Vector2 ScreenPos(this WorldObject o)
		{
			Vector3 drawPos = o.DrawPos;
			return GenWorldUI.WorldToUIPosition(drawPos);
		}

		public static bool VisibleToCameraNow(this WorldObject o)
		{
			if (!WorldRendererUtility.WorldRenderedNow)
			{
				return false;
			}
			if (o.HiddenBehindTerrainNow())
			{
				return false;
			}
			Vector2 point = o.ScreenPos();
			Rect rect = new Rect(0f, 0f, (float)UI.screenWidth, (float)UI.screenHeight);
			return rect.Contains(point);
		}

		public static float DistanceToMouse(this WorldObject o, Vector2 mousePos)
		{
			Ray ray = Find.WorldCamera.ScreenPointToRay(mousePos * Prefs.UIScale);
			int worldLayerMask = WorldCameraManager.WorldLayerMask;
			RaycastHit raycastHit;
			if (Physics.Raycast(ray, out raycastHit, 1500f, worldLayerMask))
			{
				return Vector3.Distance(raycastHit.point, o.DrawPos);
			}
			return Vector3.Cross(ray.direction, o.DrawPos - ray.origin).magnitude;
		}

		[CompilerGenerated]
		private sealed class <MultiSelectableWorldObjectsInScreenRectDistinct>c__Iterator0 : IEnumerable, IEnumerable<WorldObject>, IEnumerator, IDisposable, IEnumerator<WorldObject>
		{
			internal List<WorldObject> <allObjects>__0;

			internal int <i>__1;

			internal Rect rect;

			internal WorldObject $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <MultiSelectableWorldObjectsInScreenRectDistinct>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					allObjects = Find.WorldObjects.AllWorldObjects;
					i = 0;
					goto IL_151;
				case 1u:
					IL_ED:
					break;
				case 2u:
					break;
				default:
					return false;
				}
				IL_143:
				i++;
				IL_151:
				if (i >= allObjects.Count)
				{
					this.$PC = -1;
				}
				else
				{
					if (allObjects[i].NeverMultiSelect)
					{
						goto IL_143;
					}
					if (allObjects[i].HiddenBehindTerrainNow())
					{
						goto IL_143;
					}
					if (ExpandableWorldObjectsUtility.IsExpanded(allObjects[i]))
					{
						if (!rect.Overlaps(ExpandableWorldObjectsUtility.ExpandedIconScreenRect(allObjects[i])))
						{
							goto IL_ED;
						}
						this.$current = allObjects[i];
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
					}
					else
					{
						if (!rect.Contains(allObjects[i].ScreenPos()))
						{
							goto IL_143;
						}
						this.$current = allObjects[i];
						if (!this.$disposing)
						{
							this.$PC = 2;
						}
					}
					return true;
				}
				return false;
			}

			WorldObject IEnumerator<WorldObject>.Current
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
				return this.System.Collections.Generic.IEnumerable<RimWorld.Planet.WorldObject>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<WorldObject> IEnumerable<WorldObject>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				WorldObjectSelectionUtility.<MultiSelectableWorldObjectsInScreenRectDistinct>c__Iterator0 <MultiSelectableWorldObjectsInScreenRectDistinct>c__Iterator = new WorldObjectSelectionUtility.<MultiSelectableWorldObjectsInScreenRectDistinct>c__Iterator0();
				<MultiSelectableWorldObjectsInScreenRectDistinct>c__Iterator.rect = rect;
				return <MultiSelectableWorldObjectsInScreenRectDistinct>c__Iterator;
			}
		}
	}
}
