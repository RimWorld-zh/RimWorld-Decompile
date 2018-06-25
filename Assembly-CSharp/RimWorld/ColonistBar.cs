using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	[StaticConstructorOnStartup]
	public class ColonistBar
	{
		public ColonistBarColonistDrawer drawer = new ColonistBarColonistDrawer();

		private ColonistBarDrawLocsFinder drawLocsFinder = new ColonistBarDrawLocsFinder();

		private List<ColonistBar.Entry> cachedEntries = new List<ColonistBar.Entry>();

		private List<Vector2> cachedDrawLocs = new List<Vector2>();

		private float cachedScale = 1f;

		private bool entriesDirty = true;

		private List<Pawn> colonistsToHighlight = new List<Pawn>();

		public static readonly Texture2D BGTex = Command.BGTex;

		public static readonly Vector2 BaseSize = new Vector2(48f, 48f);

		public const float BaseSelectedTexJump = 20f;

		public const float BaseSelectedTexScale = 0.4f;

		public const float EntryInAnotherMapAlpha = 0.4f;

		public const float BaseSpaceBetweenGroups = 25f;

		public const float BaseSpaceBetweenColonistsHorizontal = 24f;

		public const float BaseSpaceBetweenColonistsVertical = 32f;

		private static List<Pawn> tmpPawns = new List<Pawn>();

		private static List<Map> tmpMaps = new List<Map>();

		private static List<Caravan> tmpCaravans = new List<Caravan>();

		private static List<Pawn> tmpColonistsInOrder = new List<Pawn>();

		private static List<Pair<Thing, Map>> tmpColonistsWithMap = new List<Pair<Thing, Map>>();

		private static List<Thing> tmpColonists = new List<Thing>();

		private static List<Thing> tmpMapColonistsOrCorpsesInScreenRect = new List<Thing>();

		private static List<Pawn> tmpCaravanPawns = new List<Pawn>();

		[CompilerGenerated]
		private static Func<Map, bool> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<Map, int> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<Caravan, int> <>f__am$cache2;

		[CompilerGenerated]
		private static Predicate<Pair<Thing, Map>> <>f__am$cache3;

		[CompilerGenerated]
		private static Predicate<Pair<Thing, Map>> <>f__am$cache4;

		[CompilerGenerated]
		private static Predicate<Pair<Thing, Map>> <>f__am$cache5;

		[CompilerGenerated]
		private static Predicate<Pair<Thing, Map>> <>f__am$cache6;

		public ColonistBar()
		{
		}

		public List<ColonistBar.Entry> Entries
		{
			get
			{
				this.CheckRecacheEntries();
				return this.cachedEntries;
			}
		}

		private bool ShowGroupFrames
		{
			get
			{
				List<ColonistBar.Entry> entries = this.Entries;
				int num = -1;
				for (int i = 0; i < entries.Count; i++)
				{
					num = Mathf.Max(num, entries[i].group);
				}
				return num >= 1;
			}
		}

		public float Scale
		{
			get
			{
				return this.cachedScale;
			}
		}

		public List<Vector2> DrawLocs
		{
			get
			{
				return this.cachedDrawLocs;
			}
		}

		public Vector2 Size
		{
			get
			{
				return ColonistBar.BaseSize * this.Scale;
			}
		}

		public float SpaceBetweenColonistsHorizontal
		{
			get
			{
				return 24f * this.Scale;
			}
		}

		private bool Visible
		{
			get
			{
				return UI.screenWidth >= 800 && UI.screenHeight >= 500;
			}
		}

		public void MarkColonistsDirty()
		{
			this.entriesDirty = true;
		}

		public void ColonistBarOnGUI()
		{
			if (this.Visible)
			{
				if (Event.current.type != EventType.Layout)
				{
					List<ColonistBar.Entry> entries = this.Entries;
					int num = -1;
					bool showGroupFrames = this.ShowGroupFrames;
					int reorderableGroup = -1;
					for (int i = 0; i < this.cachedDrawLocs.Count; i++)
					{
						Rect rect = new Rect(this.cachedDrawLocs[i].x, this.cachedDrawLocs[i].y, this.Size.x, this.Size.y);
						ColonistBar.Entry entry = entries[i];
						bool flag = num != entry.group;
						num = entry.group;
						if (flag)
						{
							reorderableGroup = ReorderableWidget.NewGroup(delegate(int from, int to)
							{
								this.Reorder(from, to, entry.group);
							}, ReorderableDirection.Horizontal, this.SpaceBetweenColonistsHorizontal, delegate(int index, Vector2 dragStartPos)
							{
								this.DrawColonistMouseAttachment(index, dragStartPos, entry.group);
							});
						}
						bool reordering;
						if (entry.pawn != null)
						{
							this.drawer.HandleClicks(rect, entry.pawn, reorderableGroup, out reordering);
						}
						else
						{
							reordering = false;
						}
						if (Event.current.type == EventType.Repaint)
						{
							if (flag && showGroupFrames)
							{
								this.drawer.DrawGroupFrame(entry.group);
							}
							if (entry.pawn != null)
							{
								this.drawer.DrawColonist(rect, entry.pawn, entry.map, this.colonistsToHighlight.Contains(entry.pawn), reordering);
							}
						}
					}
					num = -1;
					if (showGroupFrames)
					{
						for (int j = 0; j < this.cachedDrawLocs.Count; j++)
						{
							ColonistBar.Entry entry2 = entries[j];
							bool flag2 = num != entry2.group;
							num = entry2.group;
							if (flag2)
							{
								this.drawer.HandleGroupFrameClicks(entry2.group);
							}
						}
					}
				}
				if (Event.current.type == EventType.Repaint)
				{
					this.colonistsToHighlight.Clear();
				}
			}
		}

		private void CheckRecacheEntries()
		{
			if (this.entriesDirty)
			{
				this.entriesDirty = false;
				this.cachedEntries.Clear();
				if (Find.PlaySettings.showColonistBar)
				{
					ColonistBar.tmpMaps.Clear();
					ColonistBar.tmpMaps.AddRange(Find.Maps);
					ColonistBar.tmpMaps.SortBy((Map x) => !x.IsPlayerHome, (Map x) => x.uniqueID);
					int num = 0;
					for (int i = 0; i < ColonistBar.tmpMaps.Count; i++)
					{
						ColonistBar.tmpPawns.Clear();
						ColonistBar.tmpPawns.AddRange(ColonistBar.tmpMaps[i].mapPawns.FreeColonists);
						List<Thing> list = ColonistBar.tmpMaps[i].listerThings.ThingsInGroup(ThingRequestGroup.Corpse);
						for (int j = 0; j < list.Count; j++)
						{
							if (!list[j].IsDessicated())
							{
								Pawn innerPawn = ((Corpse)list[j]).InnerPawn;
								if (innerPawn != null)
								{
									if (innerPawn.IsColonist)
									{
										ColonistBar.tmpPawns.Add(innerPawn);
									}
								}
							}
						}
						List<Pawn> allPawnsSpawned = ColonistBar.tmpMaps[i].mapPawns.AllPawnsSpawned;
						for (int k = 0; k < allPawnsSpawned.Count; k++)
						{
							Corpse corpse = allPawnsSpawned[k].carryTracker.CarriedThing as Corpse;
							if (corpse != null && !corpse.IsDessicated() && corpse.InnerPawn.IsColonist)
							{
								ColonistBar.tmpPawns.Add(corpse.InnerPawn);
							}
						}
						PlayerPawnsDisplayOrderUtility.Sort(ColonistBar.tmpPawns);
						for (int l = 0; l < ColonistBar.tmpPawns.Count; l++)
						{
							this.cachedEntries.Add(new ColonistBar.Entry(ColonistBar.tmpPawns[l], ColonistBar.tmpMaps[i], num));
						}
						if (!ColonistBar.tmpPawns.Any<Pawn>())
						{
							this.cachedEntries.Add(new ColonistBar.Entry(null, ColonistBar.tmpMaps[i], num));
						}
						num++;
					}
					ColonistBar.tmpCaravans.Clear();
					ColonistBar.tmpCaravans.AddRange(Find.WorldObjects.Caravans);
					ColonistBar.tmpCaravans.SortBy((Caravan x) => x.ID);
					for (int m = 0; m < ColonistBar.tmpCaravans.Count; m++)
					{
						if (ColonistBar.tmpCaravans[m].IsPlayerControlled)
						{
							ColonistBar.tmpPawns.Clear();
							ColonistBar.tmpPawns.AddRange(ColonistBar.tmpCaravans[m].PawnsListForReading);
							PlayerPawnsDisplayOrderUtility.Sort(ColonistBar.tmpPawns);
							for (int n = 0; n < ColonistBar.tmpPawns.Count; n++)
							{
								if (ColonistBar.tmpPawns[n].IsColonist)
								{
									this.cachedEntries.Add(new ColonistBar.Entry(ColonistBar.tmpPawns[n], null, num));
								}
							}
							num++;
						}
					}
				}
				this.drawer.Notify_RecachedEntries();
				ColonistBar.tmpPawns.Clear();
				ColonistBar.tmpMaps.Clear();
				ColonistBar.tmpCaravans.Clear();
				this.drawLocsFinder.CalculateDrawLocs(this.cachedDrawLocs, out this.cachedScale);
			}
		}

		public float GetEntryRectAlpha(Rect rect)
		{
			float t;
			float result;
			if (Messages.CollidesWithAnyMessage(rect, out t))
			{
				result = Mathf.Lerp(1f, 0.2f, t);
			}
			else
			{
				result = 1f;
			}
			return result;
		}

		public void Highlight(Pawn pawn)
		{
			if (this.Visible)
			{
				if (!this.colonistsToHighlight.Contains(pawn))
				{
					this.colonistsToHighlight.Add(pawn);
				}
			}
		}

		private void Reorder(int from, int to, int entryGroup)
		{
			int num = 0;
			Pawn pawn = null;
			Pawn pawn2 = null;
			Pawn pawn3 = null;
			for (int i = 0; i < this.cachedEntries.Count; i++)
			{
				if (this.cachedEntries[i].group == entryGroup && this.cachedEntries[i].pawn != null)
				{
					if (num == from)
					{
						pawn = this.cachedEntries[i].pawn;
					}
					if (num == to)
					{
						pawn2 = this.cachedEntries[i].pawn;
					}
					pawn3 = this.cachedEntries[i].pawn;
					num++;
				}
			}
			if (pawn != null)
			{
				int num2 = (pawn2 == null) ? (pawn3.playerSettings.displayOrder + 1) : pawn2.playerSettings.displayOrder;
				for (int j = 0; j < this.cachedEntries.Count; j++)
				{
					Pawn pawn4 = this.cachedEntries[j].pawn;
					if (pawn4 != null)
					{
						if (pawn4.playerSettings.displayOrder == num2)
						{
							if (pawn2 != null && this.cachedEntries[j].group == entryGroup)
							{
								if (pawn4.thingIDNumber < pawn2.thingIDNumber)
								{
									pawn4.playerSettings.displayOrder--;
								}
								else
								{
									pawn4.playerSettings.displayOrder++;
								}
							}
						}
						else if (pawn4.playerSettings.displayOrder > num2)
						{
							pawn4.playerSettings.displayOrder++;
						}
						else
						{
							pawn4.playerSettings.displayOrder--;
						}
					}
				}
				pawn.playerSettings.displayOrder = num2;
				this.MarkColonistsDirty();
				MainTabWindowUtility.NotifyAllPawnTables_PawnsChanged();
			}
		}

		private void DrawColonistMouseAttachment(int index, Vector2 dragStartPos, int entryGroup)
		{
			Pawn pawn = null;
			Vector2 vector = default(Vector2);
			int num = 0;
			for (int i = 0; i < this.cachedEntries.Count; i++)
			{
				if (this.cachedEntries[i].group == entryGroup && this.cachedEntries[i].pawn != null)
				{
					if (num == index)
					{
						pawn = this.cachedEntries[i].pawn;
						vector = this.cachedDrawLocs[i];
						break;
					}
					num++;
				}
			}
			if (pawn != null)
			{
				RenderTexture renderTexture = PortraitsCache.Get(pawn, ColonistBarColonistDrawer.PawnTextureSize, ColonistBarColonistDrawer.PawnTextureCameraOffset, 1.28205f);
				Rect rect = new Rect(vector.x, vector.y, this.Size.x, this.Size.y);
				Rect pawnTextureRect = this.drawer.GetPawnTextureRect(rect.position);
				pawnTextureRect.position += Event.current.mousePosition - dragStartPos;
				RenderTexture iconTex = renderTexture;
				Rect? customRect = new Rect?(pawnTextureRect);
				GenUI.DrawMouseAttachment(iconTex, "", 0f, default(Vector2), customRect);
			}
		}

		public bool AnyColonistOrCorpseAt(Vector2 pos)
		{
			ColonistBar.Entry entry;
			return this.TryGetEntryAt(pos, out entry) && entry.pawn != null;
		}

		public bool TryGetEntryAt(Vector2 pos, out ColonistBar.Entry entry)
		{
			List<Vector2> drawLocs = this.DrawLocs;
			List<ColonistBar.Entry> entries = this.Entries;
			Vector2 size = this.Size;
			for (int i = 0; i < drawLocs.Count; i++)
			{
				Rect rect = new Rect(drawLocs[i].x, drawLocs[i].y, size.x, size.y);
				if (rect.Contains(pos))
				{
					entry = entries[i];
					return true;
				}
			}
			entry = default(ColonistBar.Entry);
			return false;
		}

		public List<Pawn> GetColonistsInOrder()
		{
			List<ColonistBar.Entry> entries = this.Entries;
			ColonistBar.tmpColonistsInOrder.Clear();
			for (int i = 0; i < entries.Count; i++)
			{
				if (entries[i].pawn != null)
				{
					ColonistBar.tmpColonistsInOrder.Add(entries[i].pawn);
				}
			}
			return ColonistBar.tmpColonistsInOrder;
		}

		public List<Thing> ColonistsOrCorpsesInScreenRect(Rect rect)
		{
			List<Vector2> drawLocs = this.DrawLocs;
			List<ColonistBar.Entry> entries = this.Entries;
			Vector2 size = this.Size;
			ColonistBar.tmpColonistsWithMap.Clear();
			for (int i = 0; i < drawLocs.Count; i++)
			{
				if (rect.Overlaps(new Rect(drawLocs[i].x, drawLocs[i].y, size.x, size.y)))
				{
					Pawn pawn = entries[i].pawn;
					if (pawn != null)
					{
						Thing first;
						if (pawn.Dead && pawn.Corpse != null && pawn.Corpse.SpawnedOrAnyParentSpawned)
						{
							first = pawn.Corpse;
						}
						else
						{
							first = pawn;
						}
						ColonistBar.tmpColonistsWithMap.Add(new Pair<Thing, Map>(first, entries[i].map));
					}
				}
			}
			if (WorldRendererUtility.WorldRenderedNow)
			{
				if (ColonistBar.tmpColonistsWithMap.Any((Pair<Thing, Map> x) => x.Second == null))
				{
					ColonistBar.tmpColonistsWithMap.RemoveAll((Pair<Thing, Map> x) => x.Second != null);
					goto IL_1A8;
				}
			}
			if (ColonistBar.tmpColonistsWithMap.Any((Pair<Thing, Map> x) => x.Second == Find.CurrentMap))
			{
				ColonistBar.tmpColonistsWithMap.RemoveAll((Pair<Thing, Map> x) => x.Second != Find.CurrentMap);
			}
			IL_1A8:
			ColonistBar.tmpColonists.Clear();
			for (int j = 0; j < ColonistBar.tmpColonistsWithMap.Count; j++)
			{
				ColonistBar.tmpColonists.Add(ColonistBar.tmpColonistsWithMap[j].First);
			}
			ColonistBar.tmpColonistsWithMap.Clear();
			return ColonistBar.tmpColonists;
		}

		public List<Thing> MapColonistsOrCorpsesInScreenRect(Rect rect)
		{
			ColonistBar.tmpMapColonistsOrCorpsesInScreenRect.Clear();
			List<Thing> result;
			if (!this.Visible)
			{
				result = ColonistBar.tmpMapColonistsOrCorpsesInScreenRect;
			}
			else
			{
				List<Thing> list = this.ColonistsOrCorpsesInScreenRect(rect);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].Spawned)
					{
						ColonistBar.tmpMapColonistsOrCorpsesInScreenRect.Add(list[i]);
					}
				}
				result = ColonistBar.tmpMapColonistsOrCorpsesInScreenRect;
			}
			return result;
		}

		public List<Pawn> CaravanMembersInScreenRect(Rect rect)
		{
			ColonistBar.tmpCaravanPawns.Clear();
			List<Pawn> result;
			if (!this.Visible)
			{
				result = ColonistBar.tmpCaravanPawns;
			}
			else
			{
				List<Thing> list = this.ColonistsOrCorpsesInScreenRect(rect);
				for (int i = 0; i < list.Count; i++)
				{
					Pawn pawn = list[i] as Pawn;
					if (pawn != null && pawn.IsCaravanMember())
					{
						ColonistBar.tmpCaravanPawns.Add(pawn);
					}
				}
				result = ColonistBar.tmpCaravanPawns;
			}
			return result;
		}

		public List<Caravan> CaravanMembersCaravansInScreenRect(Rect rect)
		{
			ColonistBar.tmpCaravans.Clear();
			List<Caravan> result;
			if (!this.Visible)
			{
				result = ColonistBar.tmpCaravans;
			}
			else
			{
				List<Pawn> list = this.CaravanMembersInScreenRect(rect);
				for (int i = 0; i < list.Count; i++)
				{
					ColonistBar.tmpCaravans.Add(list[i].GetCaravan());
				}
				result = ColonistBar.tmpCaravans;
			}
			return result;
		}

		public Caravan CaravanMemberCaravanAt(Vector2 at)
		{
			Caravan result;
			if (!this.Visible)
			{
				result = null;
			}
			else
			{
				Pawn pawn = this.ColonistOrCorpseAt(at) as Pawn;
				if (pawn != null && pawn.IsCaravanMember())
				{
					result = pawn.GetCaravan();
				}
				else
				{
					result = null;
				}
			}
			return result;
		}

		public Thing ColonistOrCorpseAt(Vector2 pos)
		{
			Thing result;
			ColonistBar.Entry entry;
			if (!this.Visible)
			{
				result = null;
			}
			else if (!this.TryGetEntryAt(pos, out entry))
			{
				result = null;
			}
			else
			{
				Pawn pawn = entry.pawn;
				Thing thing;
				if (pawn != null && pawn.Dead && pawn.Corpse != null && pawn.Corpse.SpawnedOrAnyParentSpawned)
				{
					thing = pawn.Corpse;
				}
				else
				{
					thing = pawn;
				}
				result = thing;
			}
			return result;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static ColonistBar()
		{
		}

		[CompilerGenerated]
		private static bool <CheckRecacheEntries>m__0(Map x)
		{
			return !x.IsPlayerHome;
		}

		[CompilerGenerated]
		private static int <CheckRecacheEntries>m__1(Map x)
		{
			return x.uniqueID;
		}

		[CompilerGenerated]
		private static int <CheckRecacheEntries>m__2(Caravan x)
		{
			return x.ID;
		}

		[CompilerGenerated]
		private static bool <ColonistsOrCorpsesInScreenRect>m__3(Pair<Thing, Map> x)
		{
			return x.Second == null;
		}

		[CompilerGenerated]
		private static bool <ColonistsOrCorpsesInScreenRect>m__4(Pair<Thing, Map> x)
		{
			return x.Second != null;
		}

		[CompilerGenerated]
		private static bool <ColonistsOrCorpsesInScreenRect>m__5(Pair<Thing, Map> x)
		{
			return x.Second == Find.CurrentMap;
		}

		[CompilerGenerated]
		private static bool <ColonistsOrCorpsesInScreenRect>m__6(Pair<Thing, Map> x)
		{
			return x.Second != Find.CurrentMap;
		}

		public struct Entry
		{
			public Pawn pawn;

			public Map map;

			public int group;

			public Entry(Pawn pawn, Map map, int group)
			{
				this.pawn = pawn;
				this.map = map;
				this.group = group;
			}
		}

		[CompilerGenerated]
		private sealed class <ColonistBarOnGUI>c__AnonStorey0
		{
			internal ColonistBar.Entry entry;

			internal ColonistBar $this;

			public <ColonistBarOnGUI>c__AnonStorey0()
			{
			}

			internal void <>m__0(int from, int to)
			{
				this.$this.Reorder(from, to, this.entry.group);
			}

			internal void <>m__1(int index, Vector2 dragStartPos)
			{
				this.$this.DrawColonistMouseAttachment(index, dragStartPos, this.entry.group);
			}
		}
	}
}
