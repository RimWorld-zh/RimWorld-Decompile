using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020008F0 RID: 2288
	[StaticConstructorOnStartup]
	public class WorldTargeter
	{
		// Token: 0x1700088C RID: 2188
		// (get) Token: 0x060034EC RID: 13548 RVA: 0x001C47AC File Offset: 0x001C2BAC
		public bool IsTargeting
		{
			get
			{
				return this.action != null;
			}
		}

		// Token: 0x060034ED RID: 13549 RVA: 0x001C47CD File Offset: 0x001C2BCD
		public void BeginTargeting(Func<GlobalTargetInfo, bool> action, bool canTargetTiles, Texture2D mouseAttachment = null, bool closeWorldTabWhenFinished = false, Action onUpdate = null, Func<GlobalTargetInfo, string> extraLabelGetter = null)
		{
			this.action = action;
			this.canTargetTiles = canTargetTiles;
			this.mouseAttachment = mouseAttachment;
			this.closeWorldTabWhenFinished = closeWorldTabWhenFinished;
			this.onUpdate = onUpdate;
			this.extraLabelGetter = extraLabelGetter;
		}

		// Token: 0x060034EE RID: 13550 RVA: 0x001C47FD File Offset: 0x001C2BFD
		public void StopTargeting()
		{
			if (this.closeWorldTabWhenFinished)
			{
				CameraJumper.TryHideWorld();
			}
			this.action = null;
			this.canTargetTiles = false;
			this.mouseAttachment = null;
			this.closeWorldTabWhenFinished = false;
			this.onUpdate = null;
			this.extraLabelGetter = null;
		}

		// Token: 0x060034EF RID: 13551 RVA: 0x001C483C File Offset: 0x001C2C3C
		public void ProcessInputEvents()
		{
			if (Event.current.type == EventType.MouseDown)
			{
				if (Event.current.button == 0)
				{
					if (this.IsTargeting)
					{
						GlobalTargetInfo arg = this.CurrentTargetUnderMouse();
						if (this.action(arg))
						{
							SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
							this.StopTargeting();
						}
						Event.current.Use();
					}
				}
				if (Event.current.button == 1)
				{
					if (this.IsTargeting)
					{
						SoundDefOf.CancelMode.PlayOneShotOnCamera(null);
						this.StopTargeting();
						Event.current.Use();
					}
				}
			}
			if (KeyBindingDefOf.Cancel.KeyDownEvent && this.IsTargeting)
			{
				SoundDefOf.CancelMode.PlayOneShotOnCamera(null);
				this.StopTargeting();
				Event.current.Use();
			}
		}

		// Token: 0x060034F0 RID: 13552 RVA: 0x001C4920 File Offset: 0x001C2D20
		public void TargeterOnGUI()
		{
			if (this.IsTargeting && !Mouse.IsInputBlockedNow)
			{
				Vector2 mousePosition = Event.current.mousePosition;
				Texture2D image = this.mouseAttachment ?? TexCommand.Attack;
				Rect position = new Rect(mousePosition.x + 8f, mousePosition.y + 8f, 32f, 32f);
				GUI.DrawTexture(position, image);
				if (this.extraLabelGetter != null)
				{
					GUI.color = Color.white;
					string text = this.extraLabelGetter(this.CurrentTargetUnderMouse());
					if (!text.NullOrEmpty())
					{
						Color color = GUI.color;
						GUI.color = Color.white;
						Rect rect = new Rect(position.xMax, position.y, 9999f, 100f);
						Vector2 vector = Text.CalcSize(text);
						GUI.DrawTexture(new Rect(rect.x - vector.x * 0.1f, rect.y, vector.x * 1.2f, vector.y), TexUI.GrayTextBG);
						GUI.color = color;
						Widgets.Label(rect, text);
					}
					GUI.color = Color.white;
				}
			}
		}

		// Token: 0x060034F1 RID: 13553 RVA: 0x001C4A5C File Offset: 0x001C2E5C
		public void TargeterUpdate()
		{
			if (this.IsTargeting)
			{
				Vector3 pos = Vector3.zero;
				GlobalTargetInfo globalTargetInfo = this.CurrentTargetUnderMouse();
				if (globalTargetInfo.HasWorldObject)
				{
					pos = globalTargetInfo.WorldObject.DrawPos;
				}
				else if (globalTargetInfo.Tile >= 0)
				{
					pos = Find.WorldGrid.GetTileCenter(globalTargetInfo.Tile);
				}
				if (globalTargetInfo.IsValid && !Mouse.IsInputBlockedNow)
				{
					WorldRendererUtility.DrawQuadTangentialToPlanet(pos, 0.8f * Find.WorldGrid.averageTileSize, 0.018f, WorldMaterials.CurTargetingMat, false, false, null);
				}
				if (this.onUpdate != null)
				{
					this.onUpdate();
				}
			}
		}

		// Token: 0x060034F2 RID: 13554 RVA: 0x001C4B10 File Offset: 0x001C2F10
		public bool IsTargetedNow(WorldObject o, List<WorldObject> worldObjectsUnderMouse = null)
		{
			bool result;
			if (!this.IsTargeting)
			{
				result = false;
			}
			else
			{
				if (worldObjectsUnderMouse == null)
				{
					worldObjectsUnderMouse = GenWorldUI.WorldObjectsUnderMouse(UI.MousePositionOnUI);
				}
				result = (worldObjectsUnderMouse.Any<WorldObject>() && o == worldObjectsUnderMouse[0]);
			}
			return result;
		}

		// Token: 0x060034F3 RID: 13555 RVA: 0x001C4B68 File Offset: 0x001C2F68
		private GlobalTargetInfo CurrentTargetUnderMouse()
		{
			GlobalTargetInfo result;
			if (!this.IsTargeting)
			{
				result = GlobalTargetInfo.Invalid;
			}
			else
			{
				List<WorldObject> list = GenWorldUI.WorldObjectsUnderMouse(UI.MousePositionOnUI);
				if (list.Any<WorldObject>())
				{
					result = list[0];
				}
				else if (this.canTargetTiles)
				{
					int num = GenWorld.MouseTile(false);
					if (num >= 0)
					{
						result = new GlobalTargetInfo(num);
					}
					else
					{
						result = GlobalTargetInfo.Invalid;
					}
				}
				else
				{
					result = GlobalTargetInfo.Invalid;
				}
			}
			return result;
		}

		// Token: 0x04001C94 RID: 7316
		private Func<GlobalTargetInfo, bool> action;

		// Token: 0x04001C95 RID: 7317
		private bool canTargetTiles;

		// Token: 0x04001C96 RID: 7318
		private Texture2D mouseAttachment;

		// Token: 0x04001C97 RID: 7319
		public bool closeWorldTabWhenFinished;

		// Token: 0x04001C98 RID: 7320
		private Action onUpdate;

		// Token: 0x04001C99 RID: 7321
		private Func<GlobalTargetInfo, string> extraLabelGetter;

		// Token: 0x04001C9A RID: 7322
		private const float BaseFeedbackTexSize = 0.8f;
	}
}
