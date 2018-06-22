using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020004EE RID: 1262
	[StaticConstructorOnStartup]
	public abstract class Need : IExposable
	{
		// Token: 0x060016A9 RID: 5801 RVA: 0x000C9010 File Offset: 0x000C7410
		public Need()
		{
		}

		// Token: 0x060016AA RID: 5802 RVA: 0x000C9020 File Offset: 0x000C7420
		public Need(Pawn newPawn)
		{
			this.pawn = newPawn;
			this.SetInitialLevel();
		}

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x060016AB RID: 5803 RVA: 0x000C9040 File Offset: 0x000C7440
		public string LabelCap
		{
			get
			{
				return this.def.LabelCap;
			}
		}

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x060016AC RID: 5804 RVA: 0x000C9060 File Offset: 0x000C7460
		public float CurInstantLevelPercentage
		{
			get
			{
				return this.CurInstantLevel / this.MaxLevel;
			}
		}

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x060016AD RID: 5805 RVA: 0x000C9084 File Offset: 0x000C7484
		public virtual int GUIChangeArrow
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x060016AE RID: 5806 RVA: 0x000C909C File Offset: 0x000C749C
		public virtual float CurInstantLevel
		{
			get
			{
				return -1f;
			}
		}

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x060016AF RID: 5807 RVA: 0x000C90B8 File Offset: 0x000C74B8
		public virtual float MaxLevel
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x060016B0 RID: 5808 RVA: 0x000C90D4 File Offset: 0x000C74D4
		// (set) Token: 0x060016B1 RID: 5809 RVA: 0x000C90EF File Offset: 0x000C74EF
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

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x060016B2 RID: 5810 RVA: 0x000C910C File Offset: 0x000C750C
		// (set) Token: 0x060016B3 RID: 5811 RVA: 0x000C912E File Offset: 0x000C752E
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

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x060016B4 RID: 5812 RVA: 0x000C9140 File Offset: 0x000C7540
		protected bool IsFrozen
		{
			get
			{
				return this.pawn.Suspended || (this.def.freezeWhileSleeping && !this.pawn.Awake()) || !this.IsPawnInteractableOrVisible;
			}
		}

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x060016B5 RID: 5813 RVA: 0x000C919C File Offset: 0x000C759C
		private bool IsPawnInteractableOrVisible
		{
			get
			{
				return this.pawn.SpawnedOrAnyParentSpawned || this.pawn.IsCaravanMember() || PawnUtility.IsTravelingInTransportPodWorldObject(this.pawn);
			}
		}

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x060016B6 RID: 5814 RVA: 0x000C91F8 File Offset: 0x000C75F8
		public virtual bool ShowOnNeedList
		{
			get
			{
				return this.def.showOnNeedList;
			}
		}

		// Token: 0x060016B7 RID: 5815 RVA: 0x000C9218 File Offset: 0x000C7618
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<NeedDef>(ref this.def, "def");
			Scribe_Values.Look<float>(ref this.curLevelInt, "curLevel", 0f, false);
		}

		// Token: 0x060016B8 RID: 5816
		public abstract void NeedInterval();

		// Token: 0x060016B9 RID: 5817 RVA: 0x000C9244 File Offset: 0x000C7644
		public virtual string GetTipString()
		{
			return string.Concat(new string[]
			{
				this.LabelCap,
				": ",
				this.CurLevelPercentage.ToStringPercent(),
				"\n",
				this.def.description
			});
		}

		// Token: 0x060016BA RID: 5818 RVA: 0x000C9299 File Offset: 0x000C7699
		public virtual void SetInitialLevel()
		{
			this.CurLevelPercentage = 0.5f;
		}

		// Token: 0x060016BB RID: 5819 RVA: 0x000C92A7 File Offset: 0x000C76A7
		public void ForceSetLevel(float levelPercent)
		{
			this.CurLevelPercentage = levelPercent;
		}

		// Token: 0x060016BC RID: 5820 RVA: 0x000C92B4 File Offset: 0x000C76B4
		public virtual void DrawOnGUI(Rect rect, int maxThresholdMarkers = 2147483647, float customMargin = -1f, bool drawArrows = true, bool doTooltip = true)
		{
			if (rect.height > 70f)
			{
				float num = (rect.height - 70f) / 2f;
				rect.height = 70f;
				rect.y += num;
			}
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
			}
			if (doTooltip)
			{
				TooltipHandler.TipRegion(rect, new TipSignal(() => this.GetTipString(), rect.GetHashCode()));
			}
			float num2 = 14f;
			float num3 = (customMargin < 0f) ? (num2 + 15f) : customMargin;
			if (rect.height < 50f)
			{
				num2 *= Mathf.InverseLerp(0f, 50f, rect.height);
			}
			Text.Font = ((rect.height <= 55f) ? GameFont.Tiny : GameFont.Small);
			Text.Anchor = TextAnchor.LowerLeft;
			Rect rect2 = new Rect(rect.x + num3 + rect.width * 0.1f, rect.y, rect.width - num3 - rect.width * 0.1f, rect.height / 2f);
			Widgets.Label(rect2, this.LabelCap);
			Text.Anchor = TextAnchor.UpperLeft;
			Rect rect3 = new Rect(rect.x, rect.y + rect.height / 2f, rect.width, rect.height / 2f);
			rect3 = new Rect(rect3.x + num3, rect3.y, rect3.width - num3 * 2f, rect3.height - num2);
			Rect rect4 = rect3;
			float num4 = 1f;
			if (this.def.scaleBar && this.MaxLevel < 1f)
			{
				num4 = this.MaxLevel;
			}
			rect4.width *= num4;
			Rect barRect = Widgets.FillableBar(rect4, this.CurLevelPercentage);
			if (drawArrows)
			{
				Widgets.FillableBarChangeArrows(rect4, this.GUIChangeArrow);
			}
			if (this.threshPercents != null)
			{
				for (int i = 0; i < Mathf.Min(this.threshPercents.Count, maxThresholdMarkers); i++)
				{
					this.DrawBarThreshold(barRect, this.threshPercents[i] * num4);
				}
			}
			if (this.def.scaleBar)
			{
				int num5 = 1;
				while ((float)num5 < this.MaxLevel)
				{
					this.DrawBarDivision(barRect, (float)num5 / this.MaxLevel * num4);
					num5++;
				}
			}
			float curInstantLevelPercentage = this.CurInstantLevelPercentage;
			if (curInstantLevelPercentage >= 0f)
			{
				this.DrawBarInstantMarkerAt(rect3, curInstantLevelPercentage * num4);
			}
			if (!this.def.tutorHighlightTag.NullOrEmpty())
			{
				UIHighlighter.HighlightOpportunity(rect, this.def.tutorHighlightTag);
			}
			Text.Font = GameFont.Small;
		}

		// Token: 0x060016BD RID: 5821 RVA: 0x000C95B4 File Offset: 0x000C79B4
		protected void DrawBarInstantMarkerAt(Rect barRect, float pct)
		{
			if (pct > 1f)
			{
				Log.ErrorOnce(this.def + " drawing bar percent > 1 : " + pct, 6932178, false);
			}
			float num = 12f;
			if (barRect.width < 150f)
			{
				num /= 2f;
			}
			Vector2 vector = new Vector2(barRect.x + barRect.width * pct, barRect.y + barRect.height);
			Rect position = new Rect(vector.x - num / 2f, vector.y, num, num);
			GUI.DrawTexture(position, Need.BarInstantMarkerTex);
		}

		// Token: 0x060016BE RID: 5822 RVA: 0x000C9660 File Offset: 0x000C7A60
		private void DrawBarThreshold(Rect barRect, float threshPct)
		{
			float num = (float)((barRect.width <= 60f) ? 1 : 2);
			Rect position = new Rect(barRect.x + barRect.width * threshPct - (num - 1f), barRect.y + barRect.height / 2f, num, barRect.height / 2f);
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

		// Token: 0x060016BF RID: 5823 RVA: 0x000C973C File Offset: 0x000C7B3C
		private void DrawBarDivision(Rect barRect, float threshPct)
		{
			float num = 5f;
			Rect rect = new Rect(barRect.x + barRect.width * threshPct - (num - 1f), barRect.y, num, barRect.height);
			if (threshPct < this.CurLevelPercentage)
			{
				GUI.color = new Color(0f, 0f, 0f, 0.9f);
			}
			else
			{
				GUI.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
			}
			Rect position = rect;
			position.yMax = position.yMin + 4f;
			GUI.DrawTextureWithTexCoords(position, Need.NeedUnitDividerTex, new Rect(0f, 0.5f, 1f, 0.5f));
			Rect position2 = rect;
			position2.yMin = position2.yMax - 4f;
			GUI.DrawTextureWithTexCoords(position2, Need.NeedUnitDividerTex, new Rect(0f, 0f, 1f, 0.5f));
			Rect position3 = rect;
			position3.yMin = position.yMax;
			position3.yMax = position2.yMin;
			if (position3.height > 0f)
			{
				GUI.DrawTextureWithTexCoords(position3, Need.NeedUnitDividerTex, new Rect(0f, 0.4f, 1f, 0.2f));
			}
			GUI.color = Color.white;
		}

		// Token: 0x04000D39 RID: 3385
		public NeedDef def;

		// Token: 0x04000D3A RID: 3386
		protected Pawn pawn;

		// Token: 0x04000D3B RID: 3387
		protected float curLevelInt;

		// Token: 0x04000D3C RID: 3388
		protected List<float> threshPercents = null;

		// Token: 0x04000D3D RID: 3389
		public const float MaxDrawHeight = 70f;

		// Token: 0x04000D3E RID: 3390
		private static readonly Texture2D BarInstantMarkerTex = ContentFinder<Texture2D>.Get("UI/Misc/BarInstantMarker", true);

		// Token: 0x04000D3F RID: 3391
		private static readonly Texture2D NeedUnitDividerTex = ContentFinder<Texture2D>.Get("UI/Misc/NeedUnitDivider", true);

		// Token: 0x04000D40 RID: 3392
		private const float BarInstantMarkerSize = 12f;
	}
}
