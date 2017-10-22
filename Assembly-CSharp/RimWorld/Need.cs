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
		public NeedDef def;

		protected Pawn pawn;

		protected float curLevelInt;

		protected List<float> threshPercents = null;

		public const float MaxDrawHeight = 70f;

		private static readonly Texture2D BarInstantMarkerTex = ContentFinder<Texture2D>.Get("UI/Misc/BarInstantMarker", true);

		private static readonly Texture2D NeedUnitDividerTex = ContentFinder<Texture2D>.Get("UI/Misc/NeedUnitDivider", true);

		private const float BarInstantMarkerSize = 12f;

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
				return ThingOwnerUtility.ContentsFrozen(this.pawn.ParentHolder) || (this.def.freezeWhileSleeping && !this.pawn.Awake()) || !this.IsPawnInteractableOrVisible;
			}
		}

		private bool IsPawnInteractableOrVisible
		{
			get
			{
				return (byte)(this.pawn.SpawnedOrAnyParentSpawned ? 1 : (this.pawn.IsCaravanMember() ? 1 : (PawnUtility.IsTravelingInTransportPodWorldObject(this.pawn) ? 1 : 0))) != 0;
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
			Rect barRect = new Rect(rect.x, (float)(rect.y + rect.height / 2.0), rect.width, (float)(rect.height / 2.0));
			Rect rect3;
			barRect = (rect3 = new Rect(barRect.x + num3, barRect.y, (float)(barRect.width - num3 * 2.0), barRect.height - num2));
			float num4 = 1f;
			if (this.def.scaleBar && this.MaxLevel < 1.0)
			{
				num4 = this.MaxLevel;
			}
			rect3.width *= num4;
			Rect barRect2 = Widgets.FillableBar(rect3, this.CurLevelPercentage);
			if (drawArrows)
			{
				Widgets.FillableBarChangeArrows(rect3, this.GUIChangeArrow);
			}
			if (this.threshPercents != null)
			{
				for (int i = 0; i < Mathf.Min(this.threshPercents.Count, maxThresholdMarkers); i++)
				{
					this.DrawBarThreshold(barRect2, this.threshPercents[i] * num4);
				}
			}
			if (this.def.scaleBar)
			{
				int num5 = 1;
				while ((float)num5 < this.MaxLevel)
				{
					this.DrawBarDivision(barRect2, (float)num5 / this.MaxLevel * num4);
					num5++;
				}
			}
			float curInstantLevelPercentage = this.CurInstantLevelPercentage;
			if (curInstantLevelPercentage >= 0.0)
			{
				this.DrawBarInstantMarkerAt(barRect, curInstantLevelPercentage * num4);
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

		private void DrawBarDivision(Rect barRect, float threshPct)
		{
			float num = 5f;
			Rect rect = new Rect((float)(barRect.x + barRect.width * threshPct - (num - 1.0)), barRect.y, num, barRect.height);
			if (threshPct < this.CurLevelPercentage)
			{
				GUI.color = new Color(0f, 0f, 0f, 0.9f);
			}
			else
			{
				GUI.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
			}
			Rect position = rect;
			position.yMax = (float)(position.yMin + 4.0);
			GUI.DrawTextureWithTexCoords(position, Need.NeedUnitDividerTex, new Rect(0f, 0.5f, 1f, 0.5f));
			Rect position2 = rect;
			position2.yMin = (float)(position2.yMax - 4.0);
			GUI.DrawTextureWithTexCoords(position2, Need.NeedUnitDividerTex, new Rect(0f, 0f, 1f, 0.5f));
			Rect position3 = rect;
			position3.yMin = position.yMax;
			position3.yMax = position2.yMin;
			if (position3.height > 0.0)
			{
				GUI.DrawTextureWithTexCoords(position3, Need.NeedUnitDividerTex, new Rect(0f, 0.4f, 1f, 0.2f));
			}
			GUI.color = Color.white;
		}
	}
}
