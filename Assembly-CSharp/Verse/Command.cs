using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	[StaticConstructorOnStartup]
	public abstract class Command : Gizmo
	{
		public string defaultLabel;

		public string defaultDesc = "No description.";

		public Texture2D icon;

		public Vector2 iconProportions = Vector2.one;

		public Rect iconTexCoords = new Rect(0f, 0f, 1f, 1f);

		public float iconDrawScale = 1f;

		public Color defaultIconColor = Color.white;

		public KeyBindingDef hotKey;

		public SoundDef activateSound;

		public int groupKey;

		public string tutorTag = "TutorTagNotSet";

		public static readonly Texture2D BGTex = ContentFinder<Texture2D>.Get("UI/Widgets/DesButBG", true);

		public virtual string Label
		{
			get
			{
				return this.defaultLabel;
			}
		}

		public virtual string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst();
			}
		}

		public virtual string Desc
		{
			get
			{
				return this.defaultDesc;
			}
		}

		public virtual Color IconDrawColor
		{
			get
			{
				return this.defaultIconColor;
			}
		}

		public virtual SoundDef CurActivateSound
		{
			get
			{
				return this.activateSound;
			}
		}

		protected virtual bool DoTooltip
		{
			get
			{
				return true;
			}
		}

		public override float Width
		{
			get
			{
				return 75f;
			}
		}

		public virtual string HighlightTag
		{
			get
			{
				return this.tutorTag;
			}
		}

		public virtual string TutorTagSelect
		{
			get
			{
				return this.tutorTag;
			}
		}

		public override GizmoResult GizmoOnGUI(Vector2 topLeft)
		{
			Rect rect = new Rect(topLeft.x, topLeft.y, this.Width, 75f);
			bool flag = false;
			if (Mouse.IsOver(rect))
			{
				flag = true;
				GUI.color = GenUI.MouseoverColor;
			}
			Texture2D badTex = this.icon;
			if ((Object)badTex == (Object)null)
			{
				badTex = BaseContent.BadTex;
			}
			GUI.DrawTexture(rect, Command.BGTex);
			MouseoverSounds.DoRegion(rect, SoundDefOf.MouseoverCommand);
			GUI.color = this.IconDrawColor;
			Widgets.DrawTextureFitted(new Rect(rect), badTex, (float)(this.iconDrawScale * 0.85000002384185791), this.iconProportions, this.iconTexCoords);
			GUI.color = Color.white;
			bool flag2 = false;
			KeyCode keyCode = (this.hotKey != null) ? this.hotKey.MainKey : KeyCode.None;
			if (keyCode != 0 && !GizmoGridDrawer.drawnHotKeys.Contains(keyCode))
			{
				Rect rect2 = new Rect((float)(rect.x + 5.0), (float)(rect.y + 5.0), (float)(rect.width - 10.0), 18f);
				Widgets.Label(rect2, keyCode.ToStringReadable());
				GizmoGridDrawer.drawnHotKeys.Add(keyCode);
				if (this.hotKey.KeyDownEvent)
				{
					flag2 = true;
					Event.current.Use();
				}
			}
			if (Widgets.ButtonInvisible(rect, false))
			{
				flag2 = true;
			}
			string labelCap = this.LabelCap;
			if (!labelCap.NullOrEmpty())
			{
				float num = Text.CalcHeight(labelCap, rect.width);
				Rect rect3 = new Rect(rect.x, (float)(rect.yMax - num + 12.0), rect.width, num);
				GUI.DrawTexture(rect3, TexUI.GrayTextBG);
				GUI.color = Color.white;
				Text.Anchor = TextAnchor.UpperCenter;
				Widgets.Label(rect3, labelCap);
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.color = Color.white;
			}
			GUI.color = Color.white;
			if (this.DoTooltip)
			{
				TipSignal tip = this.Desc;
				if (base.disabled && !base.disabledReason.NullOrEmpty())
				{
					string text = tip.text;
					tip.text = text + "\n\n" + "DisabledCommand".Translate() + ": " + base.disabledReason;
				}
				TooltipHandler.TipRegion(rect, tip);
			}
			if (!this.HighlightTag.NullOrEmpty() && (Find.WindowStack.FloatMenu == null || !Find.WindowStack.FloatMenu.windowRect.Overlaps(rect)))
			{
				UIHighlighter.HighlightOpportunity(rect, this.HighlightTag);
			}
			if (flag2)
			{
				if (base.disabled)
				{
					if (!base.disabledReason.NullOrEmpty())
					{
						Messages.Message(base.disabledReason, MessageSound.RejectInput);
					}
					return new GizmoResult(GizmoState.Mouseover, null);
				}
				if (!TutorSystem.AllowAction(this.TutorTagSelect))
				{
					return new GizmoResult(GizmoState.Mouseover, null);
				}
				GizmoResult result = new GizmoResult(GizmoState.Interacted, Event.current);
				TutorSystem.Notify_Event(this.TutorTagSelect);
				return result;
			}
			if (flag)
			{
				return new GizmoResult(GizmoState.Mouseover, null);
			}
			return new GizmoResult(GizmoState.Clear, null);
		}

		public override bool GroupsWith(Gizmo other)
		{
			Command command = other as Command;
			if (command == null)
			{
				return false;
			}
			if (this.hotKey == command.hotKey && this.Label == command.Label && (Object)this.icon == (Object)command.icon)
			{
				return true;
			}
			if (((this.groupKey != 0) ? command.groupKey : 0) != 0)
			{
				if (this.groupKey == command.groupKey)
				{
					return true;
				}
				return false;
			}
			return false;
		}

		public override void ProcessInput(Event ev)
		{
			if (this.CurActivateSound != null)
			{
				this.CurActivateSound.PlayOneShotOnCamera(null);
			}
		}

		public override int GetHashCode()
		{
			int seed = 0;
			seed = Gen.HashCombine(seed, this.hotKey);
			seed = Gen.HashCombine(seed, this.icon);
			return Gen.HashCombine(seed, this.defaultDesc);
		}

		public override string ToString()
		{
			return "Command(label=" + this.defaultLabel + ", defaultDesc=" + this.defaultDesc + ")";
		}
	}
}
