using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020007B5 RID: 1973
	[StaticConstructorOnStartup]
	public class ColonistBarColonistDrawer
	{
		// Token: 0x170006CD RID: 1741
		// (get) Token: 0x06002B9D RID: 11165 RVA: 0x001712F4 File Offset: 0x0016F6F4
		private ColonistBar ColonistBar
		{
			get
			{
				return Find.ColonistBar;
			}
		}

		// Token: 0x06002B9E RID: 11166 RVA: 0x00171310 File Offset: 0x0016F710
		public void DrawColonist(Rect rect, Pawn colonist, Map pawnMap, bool highlight, bool reordering)
		{
			float num = this.ColonistBar.GetEntryRectAlpha(rect);
			this.ApplyEntryInAnotherMapAlphaFactor(pawnMap, ref num);
			if (reordering)
			{
				num *= 0.5f;
			}
			Color color = new Color(1f, 1f, 1f, num);
			GUI.color = color;
			GUI.DrawTexture(rect, ColonistBar.BGTex);
			if (colonist.needs != null && colonist.needs.mood != null)
			{
				Rect position = rect.ContractedBy(2f);
				float num2 = position.height * colonist.needs.mood.CurLevelPercentage;
				position.yMin = position.yMax - num2;
				position.height = num2;
				GUI.DrawTexture(position, ColonistBarColonistDrawer.MoodBGTex);
			}
			if (highlight)
			{
				int thickness = (rect.width > 22f) ? 3 : 2;
				GUI.color = Color.white;
				Widgets.DrawBox(rect, thickness);
				GUI.color = color;
			}
			Rect rect2 = rect.ContractedBy(-2f * this.ColonistBar.Scale);
			bool flag = (!colonist.Dead) ? Find.Selector.SelectedObjects.Contains(colonist) : Find.Selector.SelectedObjects.Contains(colonist.Corpse);
			if (flag && !WorldRendererUtility.WorldRenderedNow)
			{
				this.DrawSelectionOverlayOnGUI(colonist, rect2);
			}
			else if (WorldRendererUtility.WorldRenderedNow && colonist.IsCaravanMember() && Find.WorldSelector.IsSelected(colonist.GetCaravan()))
			{
				this.DrawCaravanSelectionOverlayOnGUI(colonist.GetCaravan(), rect2);
			}
			GUI.DrawTexture(this.GetPawnTextureRect(rect.position), PortraitsCache.Get(colonist, ColonistBarColonistDrawer.PawnTextureSize, ColonistBarColonistDrawer.PawnTextureCameraOffset, 1.28205f));
			GUI.color = new Color(1f, 1f, 1f, num * 0.8f);
			this.DrawIcons(rect, colonist);
			GUI.color = color;
			if (colonist.Dead)
			{
				GUI.DrawTexture(rect, ColonistBarColonistDrawer.DeadColonistTex);
			}
			float num3 = 4f * this.ColonistBar.Scale;
			Vector2 pos = new Vector2(rect.center.x, rect.yMax - num3);
			GenMapUI.DrawPawnLabel(colonist, pos, num, rect.width + this.ColonistBar.SpaceBetweenColonistsHorizontal - 2f, this.pawnLabelsCache, GameFont.Tiny, true, true);
			Text.Font = GameFont.Small;
			GUI.color = Color.white;
		}

		// Token: 0x06002B9F RID: 11167 RVA: 0x0017158C File Offset: 0x0016F98C
		private Rect GroupFrameRect(int group)
		{
			float num = 99999f;
			float num2 = 0f;
			float num3 = 0f;
			List<ColonistBar.Entry> entries = this.ColonistBar.Entries;
			List<Vector2> drawLocs = this.ColonistBar.DrawLocs;
			for (int i = 0; i < entries.Count; i++)
			{
				if (entries[i].group == group)
				{
					num = Mathf.Min(num, drawLocs[i].x);
					num2 = Mathf.Max(num2, drawLocs[i].x + this.ColonistBar.Size.x);
					num3 = Mathf.Max(num3, drawLocs[i].y + this.ColonistBar.Size.y);
				}
			}
			return new Rect(num, 0f, num2 - num, num3).ContractedBy(-12f * this.ColonistBar.Scale);
		}

		// Token: 0x06002BA0 RID: 11168 RVA: 0x001716A0 File Offset: 0x0016FAA0
		public void DrawGroupFrame(int group)
		{
			Rect position = this.GroupFrameRect(group);
			List<ColonistBar.Entry> entries = this.ColonistBar.Entries;
			Map map = entries.Find((ColonistBar.Entry x) => x.group == group).map;
			float num;
			if (map == null)
			{
				if (WorldRendererUtility.WorldRenderedNow)
				{
					num = 1f;
				}
				else
				{
					num = 0.75f;
				}
			}
			else if (map != Find.CurrentMap || WorldRendererUtility.WorldRenderedNow)
			{
				num = 0.75f;
			}
			else
			{
				num = 1f;
			}
			Widgets.DrawRectFast(position, new Color(0.5f, 0.5f, 0.5f, 0.4f * num), null);
		}

		// Token: 0x06002BA1 RID: 11169 RVA: 0x00171768 File Offset: 0x0016FB68
		private void ApplyEntryInAnotherMapAlphaFactor(Map map, ref float alpha)
		{
			if (map == null)
			{
				if (!WorldRendererUtility.WorldRenderedNow)
				{
					alpha = Mathf.Min(alpha, 0.4f);
				}
			}
			else if (map != Find.CurrentMap || WorldRendererUtility.WorldRenderedNow)
			{
				alpha = Mathf.Min(alpha, 0.4f);
			}
		}

		// Token: 0x06002BA2 RID: 11170 RVA: 0x001717C0 File Offset: 0x0016FBC0
		public void HandleClicks(Rect rect, Pawn colonist, int reorderableGroup, out bool reordering)
		{
			if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && Event.current.clickCount == 2 && Mouse.IsOver(rect))
			{
				Event.current.Use();
				CameraJumper.TryJump(colonist);
			}
			reordering = ReorderableWidget.Reorderable(reorderableGroup, rect, true);
			if (Event.current.type == EventType.MouseDown && Event.current.button == 1 && Mouse.IsOver(rect))
			{
				Event.current.Use();
			}
		}

		// Token: 0x06002BA3 RID: 11171 RVA: 0x00171860 File Offset: 0x0016FC60
		public void HandleGroupFrameClicks(int group)
		{
			Rect rect = this.GroupFrameRect(group);
			if (Event.current.type == EventType.MouseUp && Event.current.button == 0 && Mouse.IsOver(rect) && !this.ColonistBar.AnyColonistOrCorpseAt(UI.MousePositionOnUIInverted))
			{
				bool worldRenderedNow = WorldRendererUtility.WorldRenderedNow;
				if ((!worldRenderedNow && !Find.Selector.dragBox.IsValidAndActive) || (worldRenderedNow && !Find.WorldSelector.dragBox.IsValidAndActive))
				{
					Find.Selector.dragBox.active = false;
					Find.WorldSelector.dragBox.active = false;
					ColonistBar.Entry entry = this.ColonistBar.Entries.Find((ColonistBar.Entry x) => x.group == group);
					Map map = entry.map;
					if (map == null)
					{
						if (WorldRendererUtility.WorldRenderedNow)
						{
							CameraJumper.TrySelect(entry.pawn);
						}
						else
						{
							CameraJumper.TryJumpAndSelect(entry.pawn);
						}
					}
					else
					{
						if (!CameraJumper.TryHideWorld() && Find.CurrentMap != map)
						{
							SoundDefOf.MapSelected.PlayOneShotOnCamera(null);
						}
						Current.Game.CurrentMap = map;
					}
				}
			}
			if (Event.current.type == EventType.MouseDown && Event.current.button == 1 && Mouse.IsOver(rect))
			{
				Event.current.Use();
			}
		}

		// Token: 0x06002BA4 RID: 11172 RVA: 0x001719F1 File Offset: 0x0016FDF1
		public void Notify_RecachedEntries()
		{
			this.pawnLabelsCache.Clear();
		}

		// Token: 0x06002BA5 RID: 11173 RVA: 0x00171A00 File Offset: 0x0016FE00
		public Rect GetPawnTextureRect(Vector2 pos)
		{
			float x = pos.x;
			float y = pos.y;
			Vector2 vector = ColonistBarColonistDrawer.PawnTextureSize * this.ColonistBar.Scale;
			Rect rect = new Rect(x + 1f, y - (vector.y - this.ColonistBar.Size.y) - 1f, vector.x, vector.y);
			rect = rect.ContractedBy(1f);
			return rect;
		}

		// Token: 0x06002BA6 RID: 11174 RVA: 0x00171A8C File Offset: 0x0016FE8C
		private void DrawIcons(Rect rect, Pawn colonist)
		{
			if (!colonist.Dead)
			{
				float num = 20f * this.ColonistBar.Scale;
				Vector2 vector = new Vector2(rect.x + 1f, rect.yMax - num - 1f);
				bool flag = false;
				if (colonist.CurJob != null)
				{
					JobDef def = colonist.CurJob.def;
					if (def == JobDefOf.AttackMelee || def == JobDefOf.AttackStatic)
					{
						flag = true;
					}
					else if (def == JobDefOf.Wait_Combat)
					{
						Stance_Busy stance_Busy = colonist.stances.curStance as Stance_Busy;
						if (stance_Busy != null && stance_Busy.focusTarg.IsValid)
						{
							flag = true;
						}
					}
				}
				if (colonist.IsFormingCaravan())
				{
					this.DrawIcon(ColonistBarColonistDrawer.Icon_FormingCaravan, ref vector, "ActivityIconFormingCaravan".Translate());
				}
				if (colonist.InAggroMentalState)
				{
					this.DrawIcon(ColonistBarColonistDrawer.Icon_MentalStateAggro, ref vector, colonist.MentalStateDef.LabelCap);
				}
				else if (colonist.InMentalState)
				{
					this.DrawIcon(ColonistBarColonistDrawer.Icon_MentalStateNonAggro, ref vector, colonist.MentalStateDef.LabelCap);
				}
				else if (colonist.InBed() && colonist.CurrentBed().Medical)
				{
					this.DrawIcon(ColonistBarColonistDrawer.Icon_MedicalRest, ref vector, "ActivityIconMedicalRest".Translate());
				}
				else if (colonist.CurJob != null && colonist.jobs.curDriver.asleep)
				{
					this.DrawIcon(ColonistBarColonistDrawer.Icon_Sleeping, ref vector, "ActivityIconSleeping".Translate());
				}
				else if (colonist.CurJob != null && colonist.CurJob.def == JobDefOf.FleeAndCower)
				{
					this.DrawIcon(ColonistBarColonistDrawer.Icon_Fleeing, ref vector, "ActivityIconFleeing".Translate());
				}
				else if (flag)
				{
					this.DrawIcon(ColonistBarColonistDrawer.Icon_Attacking, ref vector, "ActivityIconAttacking".Translate());
				}
				else if (colonist.mindState.IsIdle && GenDate.DaysPassed >= 1)
				{
					this.DrawIcon(ColonistBarColonistDrawer.Icon_Idle, ref vector, "ActivityIconIdle".Translate());
				}
				if (colonist.IsBurning() && vector.x + num <= rect.xMax)
				{
					this.DrawIcon(ColonistBarColonistDrawer.Icon_Burning, ref vector, "ActivityIconBurning".Translate());
				}
				if (colonist.Inspired && vector.x + num <= rect.xMax)
				{
					this.DrawIcon(ColonistBarColonistDrawer.Icon_Inspired, ref vector, colonist.InspirationDef.LabelCap);
				}
			}
		}

		// Token: 0x06002BA7 RID: 11175 RVA: 0x00171D34 File Offset: 0x00170134
		private void DrawIcon(Texture2D icon, ref Vector2 pos, string tooltip)
		{
			float num = 20f * this.ColonistBar.Scale;
			Rect rect = new Rect(pos.x, pos.y, num, num);
			GUI.DrawTexture(rect, icon);
			TooltipHandler.TipRegion(rect, tooltip);
			pos.x += num;
		}

		// Token: 0x06002BA8 RID: 11176 RVA: 0x00171D8C File Offset: 0x0017018C
		private void DrawSelectionOverlayOnGUI(Pawn colonist, Rect rect)
		{
			Thing obj = colonist;
			if (colonist.Dead)
			{
				obj = colonist.Corpse;
			}
			float num = 0.4f * this.ColonistBar.Scale;
			Vector2 textureSize = new Vector2((float)SelectionDrawerUtility.SelectedTexGUI.width * num, (float)SelectionDrawerUtility.SelectedTexGUI.height * num);
			SelectionDrawerUtility.CalculateSelectionBracketPositionsUI<object>(ColonistBarColonistDrawer.bracketLocs, obj, rect, SelectionDrawer.SelectTimes, textureSize, 20f * this.ColonistBar.Scale);
			this.DrawSelectionOverlayOnGUI(ColonistBarColonistDrawer.bracketLocs, num);
		}

		// Token: 0x06002BA9 RID: 11177 RVA: 0x00171E10 File Offset: 0x00170210
		private void DrawCaravanSelectionOverlayOnGUI(Caravan caravan, Rect rect)
		{
			float num = 0.4f * this.ColonistBar.Scale;
			Vector2 textureSize = new Vector2((float)SelectionDrawerUtility.SelectedTexGUI.width * num, (float)SelectionDrawerUtility.SelectedTexGUI.height * num);
			SelectionDrawerUtility.CalculateSelectionBracketPositionsUI<WorldObject>(ColonistBarColonistDrawer.bracketLocs, caravan, rect, WorldSelectionDrawer.SelectTimes, textureSize, 20f * this.ColonistBar.Scale);
			this.DrawSelectionOverlayOnGUI(ColonistBarColonistDrawer.bracketLocs, num);
		}

		// Token: 0x06002BAA RID: 11178 RVA: 0x00171E80 File Offset: 0x00170280
		private void DrawSelectionOverlayOnGUI(Vector2[] bracketLocs, float selectedTexScale)
		{
			int num = 90;
			for (int i = 0; i < 4; i++)
			{
				Widgets.DrawTextureRotated(bracketLocs[i], SelectionDrawerUtility.SelectedTexGUI, (float)num, selectedTexScale);
				num += 90;
			}
		}

		// Token: 0x0400176C RID: 5996
		private Dictionary<string, string> pawnLabelsCache = new Dictionary<string, string>();

		// Token: 0x0400176D RID: 5997
		private static readonly Texture2D MoodBGTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.4f, 0.47f, 0.53f, 0.44f));

		// Token: 0x0400176E RID: 5998
		private static readonly Texture2D DeadColonistTex = ContentFinder<Texture2D>.Get("UI/Misc/DeadColonist", true);

		// Token: 0x0400176F RID: 5999
		private static readonly Texture2D Icon_FormingCaravan = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/FormingCaravan", true);

		// Token: 0x04001770 RID: 6000
		private static readonly Texture2D Icon_MentalStateNonAggro = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/MentalStateNonAggro", true);

		// Token: 0x04001771 RID: 6001
		private static readonly Texture2D Icon_MentalStateAggro = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/MentalStateAggro", true);

		// Token: 0x04001772 RID: 6002
		private static readonly Texture2D Icon_MedicalRest = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/MedicalRest", true);

		// Token: 0x04001773 RID: 6003
		private static readonly Texture2D Icon_Sleeping = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/Sleeping", true);

		// Token: 0x04001774 RID: 6004
		private static readonly Texture2D Icon_Fleeing = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/Fleeing", true);

		// Token: 0x04001775 RID: 6005
		private static readonly Texture2D Icon_Attacking = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/Attacking", true);

		// Token: 0x04001776 RID: 6006
		private static readonly Texture2D Icon_Idle = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/Idle", true);

		// Token: 0x04001777 RID: 6007
		private static readonly Texture2D Icon_Burning = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/Burning", true);

		// Token: 0x04001778 RID: 6008
		private static readonly Texture2D Icon_Inspired = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/Inspired", true);

		// Token: 0x04001779 RID: 6009
		public static readonly Vector2 PawnTextureSize = new Vector2(ColonistBar.BaseSize.x - 2f, 75f);

		// Token: 0x0400177A RID: 6010
		public static readonly Vector3 PawnTextureCameraOffset = new Vector3(0f, 0f, 0.3f);

		// Token: 0x0400177B RID: 6011
		public const float PawnTextureCameraZoom = 1.28205f;

		// Token: 0x0400177C RID: 6012
		private const float PawnTextureHorizontalPadding = 1f;

		// Token: 0x0400177D RID: 6013
		private const float BaseIconSize = 20f;

		// Token: 0x0400177E RID: 6014
		private const float BaseGroupFrameMargin = 12f;

		// Token: 0x0400177F RID: 6015
		public const float DoubleClickTime = 0.5f;

		// Token: 0x04001780 RID: 6016
		private static Vector2[] bracketLocs = new Vector2[4];
	}
}
