using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007B1 RID: 1969
	[StaticConstructorOnStartup]
	public class ColonistBar
	{
		// Token: 0x0400174D RID: 5965
		public ColonistBarColonistDrawer drawer = new ColonistBarColonistDrawer();

		// Token: 0x0400174E RID: 5966
		private ColonistBarDrawLocsFinder drawLocsFinder = new ColonistBarDrawLocsFinder();

		// Token: 0x0400174F RID: 5967
		private List<ColonistBar.Entry> cachedEntries = new List<ColonistBar.Entry>();

		// Token: 0x04001750 RID: 5968
		private List<Vector2> cachedDrawLocs = new List<Vector2>();

		// Token: 0x04001751 RID: 5969
		private float cachedScale = 1f;

		// Token: 0x04001752 RID: 5970
		private bool entriesDirty = true;

		// Token: 0x04001753 RID: 5971
		private List<Pawn> colonistsToHighlight = new List<Pawn>();

		// Token: 0x04001754 RID: 5972
		public static readonly Texture2D BGTex = Command.BGTex;

		// Token: 0x04001755 RID: 5973
		public static readonly Vector2 BaseSize = new Vector2(48f, 48f);

		// Token: 0x04001756 RID: 5974
		public const float BaseSelectedTexJump = 20f;

		// Token: 0x04001757 RID: 5975
		public const float BaseSelectedTexScale = 0.4f;

		// Token: 0x04001758 RID: 5976
		public const float EntryInAnotherMapAlpha = 0.4f;

		// Token: 0x04001759 RID: 5977
		public const float BaseSpaceBetweenGroups = 25f;

		// Token: 0x0400175A RID: 5978
		public const float BaseSpaceBetweenColonistsHorizontal = 24f;

		// Token: 0x0400175B RID: 5979
		public const float BaseSpaceBetweenColonistsVertical = 32f;

		// Token: 0x0400175C RID: 5980
		private static List<Pawn> tmpPawns = new List<Pawn>();

		// Token: 0x0400175D RID: 5981
		private static List<Map> tmpMaps = new List<Map>();

		// Token: 0x0400175E RID: 5982
		private static List<Caravan> tmpCaravans = new List<Caravan>();

		// Token: 0x0400175F RID: 5983
		private static List<Pawn> tmpColonistsInOrder = new List<Pawn>();

		// Token: 0x04001760 RID: 5984
		private static List<Pair<Thing, Map>> tmpColonistsWithMap = new List<Pair<Thing, Map>>();

		// Token: 0x04001761 RID: 5985
		private static List<Thing> tmpColonists = new List<Thing>();

		// Token: 0x04001762 RID: 5986
		private static List<Thing> tmpMapColonistsOrCorpsesInScreenRect = new List<Thing>();

		// Token: 0x04001763 RID: 5987
		private static List<Pawn> tmpCaravanPawns = new List<Pawn>();

		// Token: 0x170006C7 RID: 1735
		// (get) Token: 0x06002B7A RID: 11130 RVA: 0x00170644 File Offset: 0x0016EA44
		public List<ColonistBar.Entry> Entries
		{
			get
			{
				this.CheckRecacheEntries();
				return this.cachedEntries;
			}
		}

		// Token: 0x170006C8 RID: 1736
		// (get) Token: 0x06002B7B RID: 11131 RVA: 0x00170668 File Offset: 0x0016EA68
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

		// Token: 0x170006C9 RID: 1737
		// (get) Token: 0x06002B7C RID: 11132 RVA: 0x001706C0 File Offset: 0x0016EAC0
		public float Scale
		{
			get
			{
				return this.cachedScale;
			}
		}

		// Token: 0x170006CA RID: 1738
		// (get) Token: 0x06002B7D RID: 11133 RVA: 0x001706DC File Offset: 0x0016EADC
		public List<Vector2> DrawLocs
		{
			get
			{
				return this.cachedDrawLocs;
			}
		}

		// Token: 0x170006CB RID: 1739
		// (get) Token: 0x06002B7E RID: 11134 RVA: 0x001706F8 File Offset: 0x0016EAF8
		public Vector2 Size
		{
			get
			{
				return ColonistBar.BaseSize * this.Scale;
			}
		}

		// Token: 0x170006CC RID: 1740
		// (get) Token: 0x06002B7F RID: 11135 RVA: 0x00170720 File Offset: 0x0016EB20
		public float SpaceBetweenColonistsHorizontal
		{
			get
			{
				return 24f * this.Scale;
			}
		}

		// Token: 0x170006CD RID: 1741
		// (get) Token: 0x06002B80 RID: 11136 RVA: 0x00170744 File Offset: 0x0016EB44
		private bool Visible
		{
			get
			{
				return UI.screenWidth >= 800 && UI.screenHeight >= 500;
			}
		}

		// Token: 0x06002B81 RID: 11137 RVA: 0x0017077F File Offset: 0x0016EB7F
		public void MarkColonistsDirty()
		{
			this.entriesDirty = true;
		}

		// Token: 0x06002B82 RID: 11138 RVA: 0x0017078C File Offset: 0x0016EB8C
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

		// Token: 0x06002B83 RID: 11139 RVA: 0x001709EC File Offset: 0x0016EDEC
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

		// Token: 0x06002B84 RID: 11140 RVA: 0x00170DA0 File Offset: 0x0016F1A0
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

		// Token: 0x06002B85 RID: 11141 RVA: 0x00170DDD File Offset: 0x0016F1DD
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

		// Token: 0x06002B86 RID: 11142 RVA: 0x00170E10 File Offset: 0x0016F210
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

		// Token: 0x06002B87 RID: 11143 RVA: 0x00171028 File Offset: 0x0016F428
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

		// Token: 0x06002B88 RID: 11144 RVA: 0x0017118C File Offset: 0x0016F58C
		public bool AnyColonistOrCorpseAt(Vector2 pos)
		{
			ColonistBar.Entry entry;
			return this.TryGetEntryAt(pos, out entry) && entry.pawn != null;
		}

		// Token: 0x06002B89 RID: 11145 RVA: 0x001711C4 File Offset: 0x0016F5C4
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

		// Token: 0x06002B8A RID: 11146 RVA: 0x00171274 File Offset: 0x0016F674
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

		// Token: 0x06002B8B RID: 11147 RVA: 0x001712E8 File Offset: 0x0016F6E8
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

		// Token: 0x06002B8C RID: 11148 RVA: 0x00171500 File Offset: 0x0016F900
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

		// Token: 0x06002B8D RID: 11149 RVA: 0x00171580 File Offset: 0x0016F980
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

		// Token: 0x06002B8E RID: 11150 RVA: 0x00171604 File Offset: 0x0016FA04
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

		// Token: 0x06002B8F RID: 11151 RVA: 0x00171678 File Offset: 0x0016FA78
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

		// Token: 0x06002B90 RID: 11152 RVA: 0x001716CC File Offset: 0x0016FACC
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

		// Token: 0x020007B2 RID: 1970
		public struct Entry
		{
			// Token: 0x0400176B RID: 5995
			public Pawn pawn;

			// Token: 0x0400176C RID: 5996
			public Map map;

			// Token: 0x0400176D RID: 5997
			public int group;

			// Token: 0x06002B99 RID: 11161 RVA: 0x001718AD File Offset: 0x0016FCAD
			public Entry(Pawn pawn, Map map, int group)
			{
				this.pawn = pawn;
				this.map = map;
				this.group = group;
			}
		}
	}
}
