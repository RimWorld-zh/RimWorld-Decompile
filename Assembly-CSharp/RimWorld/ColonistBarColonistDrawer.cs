using RimWorld.Planet;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	[StaticConstructorOnStartup]
	public class ColonistBarColonistDrawer
	{
		private Dictionary<string, string> pawnLabelsCache = new Dictionary<string, string>();

		private static readonly Texture2D MoodBGTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.4f, 0.47f, 0.53f, 0.44f));

		private static readonly Texture2D DeadColonistTex = ContentFinder<Texture2D>.Get("UI/Misc/DeadColonist", true);

		private static readonly Texture2D Icon_MentalStateNonAggro = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/MentalStateNonAggro", true);

		private static readonly Texture2D Icon_MentalStateAggro = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/MentalStateAggro", true);

		private static readonly Texture2D Icon_MedicalRest = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/MedicalRest", true);

		private static readonly Texture2D Icon_Sleeping = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/Sleeping", true);

		private static readonly Texture2D Icon_Fleeing = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/Fleeing", true);

		private static readonly Texture2D Icon_Attacking = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/Attacking", true);

		private static readonly Texture2D Icon_Idle = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/Idle", true);

		private static readonly Texture2D Icon_Burning = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/Burning", true);

		public static readonly Vector2 PawnTextureSize;

		private static readonly Vector3 PawnTextureCameraOffset;

		private const float PawnTextureCameraZoom = 1.28205f;

		private const float PawnTextureHorizontalPadding = 1f;

		private const float BaseIconSize = 20f;

		private const float BaseGroupFrameMargin = 12f;

		public const float DoubleClickTime = 0.5f;

		private static Vector2[] bracketLocs;

		private ColonistBar ColonistBar
		{
			get
			{
				return Find.ColonistBar;
			}
		}

		public void DrawColonist(Rect rect, Pawn colonist, Map pawnMap)
		{
			float entryRectAlpha = this.ColonistBar.GetEntryRectAlpha(rect);
			this.ApplyEntryInAnotherMapAlphaFactor(pawnMap, ref entryRectAlpha);
			bool flag = (!colonist.Dead) ? Find.Selector.SelectedObjects.Contains(colonist) : Find.Selector.SelectedObjects.Contains(colonist.Corpse);
			Color color = new Color(1f, 1f, 1f, entryRectAlpha);
			GUI.color = color;
			GUI.DrawTexture(rect, ColonistBar.BGTex);
			if (colonist.needs != null && colonist.needs.mood != null)
			{
				Rect position = rect.ContractedBy(2f);
				float num = position.height * colonist.needs.mood.CurLevelPercentage;
				position.yMin = position.yMax - num;
				position.height = num;
				GUI.DrawTexture(position, ColonistBarColonistDrawer.MoodBGTex);
			}
			Rect rect2 = rect.ContractedBy((float)(-2.0 * this.ColonistBar.Scale));
			if (flag && !WorldRendererUtility.WorldRenderedNow)
			{
				this.DrawSelectionOverlayOnGUI(colonist, rect2);
			}
			else if (WorldRendererUtility.WorldRenderedNow && colonist.IsCaravanMember() && Find.WorldSelector.IsSelected(colonist.GetCaravan()))
			{
				this.DrawCaravanSelectionOverlayOnGUI(colonist.GetCaravan(), rect2);
			}
			GUI.DrawTexture(this.GetPawnTextureRect(rect.x, rect.y), PortraitsCache.Get(colonist, ColonistBarColonistDrawer.PawnTextureSize, ColonistBarColonistDrawer.PawnTextureCameraOffset, 1.28205f));
			GUI.color = new Color(1f, 1f, 1f, (float)(entryRectAlpha * 0.800000011920929));
			this.DrawIcons(rect, colonist);
			GUI.color = color;
			if (colonist.Dead)
			{
				GUI.DrawTexture(rect, ColonistBarColonistDrawer.DeadColonistTex);
			}
			float num2 = (float)(4.0 * this.ColonistBar.Scale);
			Vector2 center = rect.center;
			Vector2 pos = new Vector2(center.x, rect.yMax - num2);
			GenMapUI.DrawPawnLabel(colonist, pos, entryRectAlpha, (float)(rect.width + this.ColonistBar.SpaceBetweenColonistsHorizontal - 2.0), this.pawnLabelsCache, GameFont.Tiny, true, true);
			Text.Font = GameFont.Small;
			GUI.color = Color.white;
		}

		private Rect GroupFrameRect(int group)
		{
			float num = 99999f;
			float num2 = 0f;
			float num3 = 0f;
			List<ColonistBar.Entry> entries = this.ColonistBar.Entries;
			List<Vector2> drawLocs = this.ColonistBar.DrawLocs;
			for (int i = 0; i < entries.Count; i++)
			{
				ColonistBar.Entry entry = entries[i];
				if (entry.group == group)
				{
					float a = num;
					Vector2 vector = drawLocs[i];
					num = Mathf.Min(a, vector.x);
					float a2 = num2;
					Vector2 vector2 = drawLocs[i];
					float x = vector2.x;
					Vector2 size = this.ColonistBar.Size;
					num2 = Mathf.Max(a2, x + size.x);
					float a3 = num3;
					Vector2 vector3 = drawLocs[i];
					float y = vector3.y;
					Vector2 size2 = this.ColonistBar.Size;
					num3 = Mathf.Max(a3, y + size2.y);
				}
			}
			return new Rect(num, 0f, num2 - num, num3).ContractedBy((float)(-12.0 * this.ColonistBar.Scale));
		}

		public void DrawGroupFrame(int group)
		{
			Rect position = this.GroupFrameRect(group);
			List<ColonistBar.Entry> entries = this.ColonistBar.Entries;
			ColonistBar.Entry entry = entries.Find((Predicate<ColonistBar.Entry>)((ColonistBar.Entry x) => x.group == group));
			Map map = entry.map;
			float num = (float)((map != null) ? ((map == Find.VisibleMap && !WorldRendererUtility.WorldRenderedNow) ? 1.0 : 0.75) : ((!WorldRendererUtility.WorldRenderedNow) ? 0.75 : 1.0));
			Widgets.DrawRectFast(position, new Color(0.5f, 0.5f, 0.5f, (float)(0.40000000596046448 * num)), null);
		}

		private void ApplyEntryInAnotherMapAlphaFactor(Map map, ref float alpha)
		{
			if (map == null)
			{
				if (!WorldRendererUtility.WorldRenderedNow)
				{
					alpha = Mathf.Min(alpha, 0.4f);
				}
			}
			else
			{
				if (map == Find.VisibleMap && !WorldRendererUtility.WorldRenderedNow)
					return;
				alpha = Mathf.Min(alpha, 0.4f);
			}
		}

		public void HandleClicks(Rect rect, Pawn colonist)
		{
			if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && Event.current.clickCount == 2 && Mouse.IsOver(rect))
			{
				Event.current.Use();
				CameraJumper.TryJump((Thing)colonist);
			}
			if (Event.current.button == 1 && Widgets.ButtonInvisible(rect, false))
			{
				CameraJumper.TryJumpAndSelect(CameraJumper.GetWorldTarget((Thing)colonist));
			}
		}

		public void HandleGroupFrameClicks(int group)
		{
			Rect rect = this.GroupFrameRect(group);
			if (Event.current.type == EventType.MouseUp && Event.current.button == 0 && Mouse.IsOver(rect) && !this.ColonistBar.AnyColonistOrCorpseAt(UI.MousePositionOnUIInverted))
			{
				bool worldRenderedNow = WorldRendererUtility.WorldRenderedNow;
				if (!worldRenderedNow && !Find.Selector.dragBox.IsValidAndActive)
				{
					goto IL_0095;
				}
				if (worldRenderedNow && !Find.WorldSelector.dragBox.IsValidAndActive)
					goto IL_0095;
			}
			goto IL_0155;
			IL_0095:
			Find.Selector.dragBox.active = false;
			Find.WorldSelector.dragBox.active = false;
			ColonistBar.Entry entry = this.ColonistBar.Entries.Find((Predicate<ColonistBar.Entry>)((ColonistBar.Entry x) => x.group == group));
			Map map = entry.map;
			if (map == null)
			{
				if (WorldRendererUtility.WorldRenderedNow)
				{
					CameraJumper.TrySelect((Thing)entry.pawn);
				}
				else
				{
					CameraJumper.TryJumpAndSelect((Thing)entry.pawn);
				}
			}
			else
			{
				if (!CameraJumper.TryHideWorld() && Current.Game.VisibleMap != map)
				{
					SoundDefOf.MapSelected.PlayOneShotOnCamera(null);
				}
				Current.Game.VisibleMap = map;
			}
			goto IL_0155;
			IL_0155:
			if (Event.current.button == 1 && Widgets.ButtonInvisible(rect, false))
			{
				ColonistBar.Entry entry2 = this.ColonistBar.Entries.Find((Predicate<ColonistBar.Entry>)((ColonistBar.Entry x) => x.group == group));
				if (entry2.map != null)
				{
					CameraJumper.TryJumpAndSelect(CameraJumper.GetWorldTargetOfMap(entry2.map));
				}
				else if (entry2.pawn != null)
				{
					CameraJumper.TryJumpAndSelect((Thing)entry2.pawn);
				}
			}
		}

		public void Notify_RecachedEntries()
		{
			this.pawnLabelsCache.Clear();
		}

		private Rect GetPawnTextureRect(float x, float y)
		{
			Vector2 vector = ColonistBarColonistDrawer.PawnTextureSize * this.ColonistBar.Scale;
			double x2 = x + 1.0;
			float y2 = vector.y;
			Vector2 size = this.ColonistBar.Size;
			Rect rect = new Rect((float)x2, (float)(y - (y2 - size.y) - 1.0), vector.x, vector.y);
			rect = rect.ContractedBy(1f);
			return rect;
		}

		private void DrawIcons(Rect rect, Pawn colonist)
		{
			if (!colonist.Dead)
			{
				float num = (float)(20.0 * this.ColonistBar.Scale);
				Vector2 vector = new Vector2((float)(rect.x + 1.0), (float)(rect.yMax - num - 1.0));
				bool flag = false;
				if (colonist.CurJob != null)
				{
					JobDef def = colonist.CurJob.def;
					if (def == JobDefOf.AttackMelee || def == JobDefOf.AttackStatic)
					{
						flag = true;
					}
					else if (def == JobDefOf.WaitCombat)
					{
						Stance_Busy stance_Busy = colonist.stances.curStance as Stance_Busy;
						if (stance_Busy != null && stance_Busy.focusTarg.IsValid)
						{
							flag = true;
						}
					}
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
				if (colonist.IsBurning())
				{
					this.DrawIcon(ColonistBarColonistDrawer.Icon_Burning, ref vector, "ActivityIconBurning".Translate());
				}
			}
		}

		private void DrawIcon(Texture2D icon, ref Vector2 pos, string tooltip)
		{
			float num = (float)(20.0 * this.ColonistBar.Scale);
			Rect rect = new Rect(pos.x, pos.y, num, num);
			GUI.DrawTexture(rect, icon);
			TooltipHandler.TipRegion(rect, tooltip);
			pos.x += num;
		}

		private void DrawSelectionOverlayOnGUI(Pawn colonist, Rect rect)
		{
			Thing obj = colonist;
			if (colonist.Dead)
			{
				obj = colonist.Corpse;
			}
			float num = (float)(0.40000000596046448 * this.ColonistBar.Scale);
			Vector2 textureSize = new Vector2((float)SelectionDrawerUtility.SelectedTexGUI.width * num, (float)SelectionDrawerUtility.SelectedTexGUI.height * num);
			SelectionDrawerUtility.CalculateSelectionBracketPositionsUI(ColonistBarColonistDrawer.bracketLocs, obj, rect, SelectionDrawer.SelectTimes, textureSize, (float)(20.0 * this.ColonistBar.Scale));
			this.DrawSelectionOverlayOnGUI(ColonistBarColonistDrawer.bracketLocs, num);
		}

		private void DrawCaravanSelectionOverlayOnGUI(Caravan caravan, Rect rect)
		{
			float num = (float)(0.40000000596046448 * this.ColonistBar.Scale);
			Vector2 textureSize = new Vector2((float)SelectionDrawerUtility.SelectedTexGUI.width * num, (float)SelectionDrawerUtility.SelectedTexGUI.height * num);
			SelectionDrawerUtility.CalculateSelectionBracketPositionsUI(ColonistBarColonistDrawer.bracketLocs, caravan, rect, WorldSelectionDrawer.SelectTimes, textureSize, (float)(20.0 * this.ColonistBar.Scale));
			this.DrawSelectionOverlayOnGUI(ColonistBarColonistDrawer.bracketLocs, num);
		}

		private void DrawSelectionOverlayOnGUI(Vector2[] bracketLocs, float selectedTexScale)
		{
			int num = 90;
			for (int i = 0; i < 4; i++)
			{
				Widgets.DrawTextureRotated(bracketLocs[i], SelectionDrawerUtility.SelectedTexGUI, (float)num, selectedTexScale);
				num += 90;
			}
		}

		static ColonistBarColonistDrawer()
		{
			Vector2 baseSize = ColonistBar.BaseSize;
			ColonistBarColonistDrawer.PawnTextureSize = new Vector2((float)(baseSize.x - 2.0), 75f);
			ColonistBarColonistDrawer.PawnTextureCameraOffset = new Vector3(0f, 0f, 0.3f);
			ColonistBarColonistDrawer.bracketLocs = new Vector2[4];
		}
	}
}
