using RimWorld.Planet;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	[StaticConstructorOnStartup]
	public abstract class Need : IExposable
	{
		public const float MaxDrawHeight = 70f;

		private const float BarInstantMarkerSize = 12f;

		public NeedDef def;

		protected Pawn pawn;

		protected float curLevelInt;

		protected List<float> threshPercents;

		private static readonly Texture2D BarInstantMarkerTex = ContentFinder<Texture2D>.Get("UI/Misc/BarInstantMarker", true);

		public string LabelCap
		{
			get
			{
				return this.def.LabelCap;
			}
		}

		public float CurInstantLevelPercentage
		{
			get
			{
				return this.CurInstantLevel / this.MaxLevel;
			}
		}

		public virtual int GUIChangeArrow
		{
			get
			{
				return 0;
			}
		}

		public virtual float CurInstantLevel
		{
			get
			{
				return -1f;
			}
		}

		public virtual float MaxLevel
		{
			get
			{
				return 1f;
			}
		}

		public virtual float CurLevel
		{
			get
			{
				return this.curLevelInt;
			}
			set
			{
				this.curLevelInt = Mathf.Clamp(value, 0f, this.MaxLevel);
			}
		}

		public float CurLevelPercentage
		{
			get
			{
				return this.CurLevel / this.MaxLevel;
			}
			set
			{
				this.CurLevel = value * this.MaxLevel;
			}
		}

		protected bool IsFrozen
		{
			get
			{
				if (ThingOwnerUtility.ContentsFrozen(this.pawn.ParentHolder))
				{
					return true;
				}
				if (this.def.freezeWhileSleeping && !this.pawn.Awake())
				{
					return true;
				}
				return !this.IsPawnInteractableOrVisible;
			}
		}

		private bool IsPawnInteractableOrVisible
		{
			get
			{
				if (this.pawn.SpawnedOrAnyParentSpawned)
				{
					return true;
				}
				if (this.pawn.IsCaravanMember())
				{
					return true;
				}
				if (PawnUtility.IsTravelingInTransportPodWorldObject(this.pawn))
				{
					return true;
				}
				return false;
			}
		}

		public Need()
		{
		}

		public Need(Pawn newPawn)
		{
			this.pawn = newPawn;
			this.SetInitialLevel();
		}

		public virtual void ExposeData()
		{
			Scribe_Defs.Look<NeedDef>(ref this.def, "def");
			Scribe_Values.Look<float>(ref this.curLevelInt, "curLevel", 0f, false);
		}

		public abstract void NeedInterval();

		public virtual string GetTipString()
		{
			return this.LabelCap + ": " + this.CurLevelPercentage.ToStringPercent() + "\n" + this.def.description;
		}

		public virtual void SetInitialLevel()
		{
			this.CurLevelPercentage = 0.5f;
		}

		public void ForceSetLevel(float levelPercent)
		{
			this.CurLevelPercentage = levelPercent;
		}

		public virtual void DrawOnGUI(Rect rect, int maxThresholdMarkers = 2147483647, float customMargin = -1f, bool drawArrows = true, bool doTooltip = true)
		{
			if (rect.height > 70.0)
			{
				float num = (float)((rect.height - 70.0) / 2.0);
				rect.height = 70f;
				rect.y += num;
			}
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
			}
			if (doTooltip)
			{
				TooltipHandler.TipRegion(rect, new TipSignal((Func<string>)(() => this.GetTipString()), rect.GetHashCode()));
			}
			float num2 = 14f;
			float num3 = (float)((!(customMargin >= 0.0)) ? (num2 + 15.0) : customMargin);
			if (rect.height < 50.0)
			{
				num2 *= Mathf.InverseLerp(0f, 50f, rect.height);
			}
			Text.Font = (GameFont)((rect.height > 55.0) ? 1 : 0);
			Text.Anchor = TextAnchor.LowerLeft;
			Rect rect2 = new Rect((float)(rect.x + num3 + rect.width * 0.10000000149011612), rect.y, (float)(rect.width - num3 - rect.width * 0.10000000149011612), (float)(rect.height / 2.0));
			Widgets.Label(rect2, this.LabelCap);
			Text.Anchor = TextAnchor.UpperLeft;
			Rect rect3 = new Rect(rect.x, (float)(rect.y + rect.height / 2.0), rect.width, (float)(rect.height / 2.0));
			rect3 = new Rect(rect3.x + num3, rect3.y, (float)(rect3.width - num3 * 2.0), rect3.height - num2);
			Widgets.FillableBar(rect3, this.CurLevelPercentage);
			if (drawArrows)
			{
				Widgets.FillableBarChangeArrows(rect3, this.GUIChangeArrow);
			}
			if (this.threshPercents != null)
			{
				for (int i = 0; i < Mathf.Min(this.threshPercents.Count, maxThresholdMarkers); i++)
				{
					this.DrawBarThreshold(rect3, this.threshPercents[i]);
				}
			}
			float curInstantLevelPercentage = this.CurInstantLevelPercentage;
			if (curInstantLevelPercentage >= 0.0)
			{
				this.DrawBarInstantMarkerAt(rect3, curInstantLevelPercentage);
			}
			if (!this.def.tutorHighlightTag.NullOrEmpty())
			{
				UIHighlighter.HighlightOpportunity(rect, this.def.tutorHighlightTag);
			}
			Text.Font = GameFont.Small;
		}

		protected void DrawBarInstantMarkerAt(Rect barRect, float pct)
		{
			if (pct > 1.0)
			{
				Log.ErrorOnce(this.def + " drawing bar percent > 1 : " + pct, 6932178);
			}
			float num = 12f;
			if (barRect.width < 150.0)
			{
				num = (float)(num / 2.0);
			}
			Vector2 vector = new Vector2(barRect.x + barRect.width * pct, barRect.y + barRect.height);
			Rect position = new Rect((float)(vector.x - num / 2.0), vector.y, num, num);
			GUI.DrawTexture(position, Need.BarInstantMarkerTex);
		}

		private void DrawBarThreshold(Rect barRect, float threshPct)
		{
			float num = (float)((!(barRect.width > 60.0)) ? 1 : 2);
			Rect position = new Rect((float)(barRect.x + barRect.width * threshPct - (num - 1.0)), (float)(barRect.y + barRect.height / 2.0), num, (float)(barRect.height / 2.0));
			Texture2D image;
			if (threshPct < this.CurLevelPercentage)
			{
				image = BaseContent.BlackTex;
				GUI.color = new Color(1f, 1f, 1f, 0.9f);
			}
			else
			{
				image = BaseContent.GreyTex;
				GUI.color = new Color(1f, 1f, 1f, 0.5f);
			}
			GUI.DrawTexture(position, image);
			GUI.color = Color.white;
		}
	}
}
