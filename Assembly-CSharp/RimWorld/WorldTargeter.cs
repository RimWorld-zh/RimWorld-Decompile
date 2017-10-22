using RimWorld.Planet;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	[StaticConstructorOnStartup]
	public class WorldTargeter
	{
		private Func<GlobalTargetInfo, bool> action;

		private bool canTargetTiles;

		private Texture2D mouseAttachment;

		public bool closeWorldTabWhenFinished;

		private Action onUpdate;

		private Func<GlobalTargetInfo, string> extraLabelGetter;

		private const float BaseFeedbackTexSize = 0.8f;

		public bool IsTargeting
		{
			get
			{
				return (object)this.action != null;
			}
		}

		public void BeginTargeting(Func<GlobalTargetInfo, bool> action, bool canTargetTiles, Texture2D mouseAttachment = null, bool closeWorldTabWhenFinished = false, Action onUpdate = null, Func<GlobalTargetInfo, string> extraLabelGetter = null)
		{
			this.action = action;
			this.canTargetTiles = canTargetTiles;
			this.mouseAttachment = mouseAttachment;
			this.closeWorldTabWhenFinished = closeWorldTabWhenFinished;
			this.onUpdate = onUpdate;
			this.extraLabelGetter = extraLabelGetter;
		}

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

		public void ProcessInputEvents()
		{
			if (Event.current.type == EventType.MouseDown)
			{
				if (Event.current.button == 0 && this.IsTargeting)
				{
					GlobalTargetInfo arg = this.CurrentTargetUnderMouse();
					if (this.action(arg))
					{
						SoundDefOf.TickHigh.PlayOneShotOnCamera(null);
						this.StopTargeting();
					}
					Event.current.Use();
				}
				if (Event.current.button == 1 && this.IsTargeting)
				{
					SoundDefOf.CancelMode.PlayOneShotOnCamera(null);
					this.StopTargeting();
					Event.current.Use();
				}
			}
			if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape && this.IsTargeting)
			{
				SoundDefOf.CancelMode.PlayOneShotOnCamera(null);
				this.StopTargeting();
				Event.current.Use();
			}
		}

		public void TargeterOnGUI()
		{
			if (this.IsTargeting)
			{
				Vector2 mousePosition = Event.current.mousePosition;
				Texture2D image = this.mouseAttachment ?? TexCommand.Attack;
				Rect position = new Rect((float)(mousePosition.x + 8.0), (float)(mousePosition.y + 8.0), 32f, 32f);
				GUI.DrawTexture(position, image);
				if ((object)this.extraLabelGetter != null)
				{
					string text = this.extraLabelGetter(this.CurrentTargetUnderMouse());
					if (!text.NullOrEmpty())
					{
						GUI.color = Color.red;
						Widgets.Label(new Rect(position.xMax, position.y, 250f, 100f), text);
						GUI.color = Color.white;
					}
				}
			}
		}

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
				if (globalTargetInfo.IsValid)
				{
					WorldRendererUtility.DrawQuadTangentialToPlanet(pos, (float)(0.800000011920929 * Find.WorldGrid.averageTileSize), 0.018f, WorldMaterials.CurTargetingMat, false, false, null);
				}
				if ((object)this.onUpdate != null)
				{
					this.onUpdate();
				}
			}
		}

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
				result = (worldObjectsUnderMouse.Any() && o == worldObjectsUnderMouse[0]);
			}
			return result;
		}

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
				if (list.Any())
				{
					result = list[0];
				}
				else if (this.canTargetTiles)
				{
					int num = GenWorld.MouseTile(false);
					result = ((num < 0) ? GlobalTargetInfo.Invalid : new GlobalTargetInfo(num));
				}
				else
				{
					result = GlobalTargetInfo.Invalid;
				}
			}
			return result;
		}
	}
}
