using RimWorld;
using RimWorld.Planet;
using System;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	public abstract class Letter : IExposable
	{
		public LetterDef def;

		public string label;

		public GlobalTargetInfo lookTarget = GlobalTargetInfo.Invalid;

		public float arrivalTime;

		public string debugInfo;

		public const float DrawWidth = 38f;

		public const float DrawHeight = 30f;

		private const float FallTime = 1f;

		private const float FallDistance = 200f;

		public virtual bool StillValid
		{
			get
			{
				return (byte)((this.lookTarget.Thing == null || !this.lookTarget.Thing.Destroyed) ? ((this.lookTarget.WorldObject == null || this.lookTarget.WorldObject.Spawned) ? 1 : 0) : 0) != 0;
			}
		}

		public IThingHolder ParentHolder
		{
			get
			{
				return Find.World;
			}
		}

		public virtual void ExposeData()
		{
			Scribe_Defs.Look<LetterDef>(ref this.def, "def");
			Scribe_Values.Look<string>(ref this.label, "label", (string)null, false);
			if (Scribe.mode == LoadSaveMode.Saving && this.lookTarget.HasThing && this.lookTarget.Thing.Destroyed)
			{
				this.lookTarget = GlobalTargetInfo.Invalid;
			}
			Scribe_TargetInfo.Look(ref this.lookTarget, "lookTarget");
		}

		public virtual void DrawButtonAt(float topY)
		{
			float num = (float)((float)UI.screenWidth - 38.0 - 12.0);
			Rect rect = new Rect(num, topY, 38f, 30f);
			Rect rect2 = new Rect(rect);
			float num2 = Time.time - this.arrivalTime;
			Color color = this.def.color;
			if (num2 < 1.0)
			{
				rect2.y -= (float)((1.0 - num2) * 200.0);
				color.a = (float)(num2 / 1.0);
			}
			if (!Mouse.IsOver(rect) && this.def.bounce && num2 > 15.0 && num2 % 5.0 < 1.0)
			{
				float num3 = (float)((float)UI.screenWidth * 0.059999998658895493);
				float num4 = (float)(2.0 * (num2 % 1.0) - 1.0);
				float num5 = (float)(num3 * (1.0 - num4 * num4));
				rect2.x -= num5;
			}
			if (this.def.flashInterval > 0.0)
			{
				float num6 = (float)(Time.time - (this.arrivalTime + 1.0));
				if (num6 > 0.0 && num6 % this.def.flashInterval < 1.0)
				{
					GenUI.DrawFlash(num, topY, (float)((float)UI.screenWidth * 0.60000002384185791), (float)(Pulser.PulseBrightness(1f, 1f, num6) * 0.550000011920929), this.def.flashColor);
				}
			}
			GUI.color = color;
			Widgets.DrawShadowAround(rect2);
			GUI.DrawTexture(rect2, this.def.Icon);
			GUI.color = Color.white;
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.UpperRight;
			string text = this.PostProcessedLabel();
			Vector2 vector = Text.CalcSize(text);
			float x = vector.x;
			float y = vector.y;
			double x2 = rect2.x + rect2.width / 2.0;
			Vector2 center = rect2.center;
			Vector2 vector2 = new Vector2((float)x2, (float)(center.y - y / 4.0));
			float num7 = (float)(vector2.x + x / 2.0 - (float)(UI.screenWidth - 2));
			if (num7 > 0.0)
			{
				vector2.x -= num7;
			}
			Rect position = new Rect((float)(vector2.x - x / 2.0 - 4.0 - 1.0), vector2.y, (float)(x + 8.0), 12f);
			GUI.DrawTexture(position, TexUI.GrayTextBG);
			GUI.color = new Color(1f, 1f, 1f, 0.75f);
			Rect rect3 = new Rect((float)(vector2.x - x / 2.0), (float)(vector2.y - 3.0), x, 999f);
			Widgets.Label(rect3, text);
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperLeft;
			Text.Font = GameFont.Small;
			if (Event.current.type == EventType.MouseDown && Event.current.button == 1 && Mouse.IsOver(rect))
			{
				SoundDefOf.Click.PlayOneShotOnCamera(null);
				Find.LetterStack.RemoveLetter(this);
				Event.current.Use();
			}
			if (Widgets.ButtonInvisible(rect2, false))
			{
				this.OpenLetter();
				Event.current.Use();
			}
		}

		public virtual void CheckForMouseOverTextAt(float topY)
		{
			float num = (float)((float)UI.screenWidth - 38.0 - 12.0);
			Rect rect = new Rect(num, topY, 38f, 30f);
			if (Mouse.IsOver(rect))
			{
				Find.LetterStack.Notify_LetterMouseover(this);
				string mouseoverText = this.GetMouseoverText();
				if (!mouseoverText.NullOrEmpty())
				{
					Text.Font = GameFont.Small;
					Text.Anchor = TextAnchor.UpperLeft;
					float num2 = Text.CalcHeight(mouseoverText, 310f);
					num2 = (float)(num2 + 20.0);
					float x = (float)(num - 330.0 - 10.0);
					Rect infoRect = new Rect(x, (float)(topY - num2 / 2.0), 330f, num2);
					Find.WindowStack.ImmediateWindow(2768333, infoRect, WindowLayer.Super, (Action)delegate
					{
						Text.Font = GameFont.Small;
						Rect position = infoRect.AtZero().ContractedBy(10f);
						GUI.BeginGroup(position);
						Widgets.Label(new Rect(0f, 0f, position.width, position.height), mouseoverText);
						GUI.EndGroup();
					}, true, false, 1f);
				}
			}
		}

		protected abstract string GetMouseoverText();

		public abstract void OpenLetter();

		public virtual void Received()
		{
		}

		public virtual void Removed()
		{
		}

		protected virtual string PostProcessedLabel()
		{
			return this.label;
		}
	}
}
