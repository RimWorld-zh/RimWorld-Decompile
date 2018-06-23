using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200030B RID: 779
	public class GameCondition_Aurora : GameCondition
	{
		// Token: 0x04000867 RID: 2151
		private int curColorIndex = -1;

		// Token: 0x04000868 RID: 2152
		private int prevColorIndex = -1;

		// Token: 0x04000869 RID: 2153
		private float curColorTransition;

		// Token: 0x0400086A RID: 2154
		private const int LerpTicks = 200;

		// Token: 0x0400086B RID: 2155
		public const float MaxSunGlow = 0.5f;

		// Token: 0x0400086C RID: 2156
		private const float Glow = 0.25f;

		// Token: 0x0400086D RID: 2157
		private const float SkyColorStrength = 0.075f;

		// Token: 0x0400086E RID: 2158
		private const float OverlayColorStrength = 0.025f;

		// Token: 0x0400086F RID: 2159
		private const float BaseBrightness = 0.73f;

		// Token: 0x04000870 RID: 2160
		private const int TransitionDurationTicks_NotPermanent = 280;

		// Token: 0x04000871 RID: 2161
		private const int TransitionDurationTicks_Permanent = 3750;

		// Token: 0x04000872 RID: 2162
		private static readonly Color[] Colors = new Color[]
		{
			new Color(0f, 1f, 0f),
			new Color(0.3f, 1f, 0f),
			new Color(0f, 1f, 0.7f),
			new Color(0.3f, 1f, 0.7f),
			new Color(0f, 0.5f, 1f),
			new Color(0f, 0f, 1f),
			new Color(0.87f, 0f, 1f),
			new Color(0.75f, 0f, 1f)
		};

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06000D21 RID: 3361 RVA: 0x000720A0 File Offset: 0x000704A0
		public Color CurrentColor
		{
			get
			{
				return Color.Lerp(GameCondition_Aurora.Colors[this.prevColorIndex], GameCondition_Aurora.Colors[this.curColorIndex], this.curColorTransition);
			}
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06000D22 RID: 3362 RVA: 0x000720EC File Offset: 0x000704EC
		private int TransitionDurationTicks
		{
			get
			{
				return (!base.Permanent) ? 280 : 3750;
			}
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x06000D23 RID: 3363 RVA: 0x0007211C File Offset: 0x0007051C
		private bool BrightInAllMaps
		{
			get
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					if (GenCelestial.CurCelestialSunGlow(maps[i]) <= 0.5f)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x06000D24 RID: 3364 RVA: 0x00072170 File Offset: 0x00070570
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.curColorIndex, "curColorIndex", 0, false);
			Scribe_Values.Look<int>(ref this.prevColorIndex, "prevColorIndex", 0, false);
			Scribe_Values.Look<float>(ref this.curColorTransition, "curColorTransition", 0f, false);
		}

		// Token: 0x06000D25 RID: 3365 RVA: 0x000721BE File Offset: 0x000705BE
		public override void Init()
		{
			base.Init();
			this.curColorIndex = Rand.Range(0, GameCondition_Aurora.Colors.Length);
			this.prevColorIndex = this.curColorIndex;
			this.curColorTransition = 1f;
		}

		// Token: 0x06000D26 RID: 3366 RVA: 0x000721F4 File Offset: 0x000705F4
		public override float SkyGazeChanceFactor(Map map)
		{
			return 8f;
		}

		// Token: 0x06000D27 RID: 3367 RVA: 0x00072210 File Offset: 0x00070610
		public override float SkyGazeJoyGainFactor(Map map)
		{
			return 5f;
		}

		// Token: 0x06000D28 RID: 3368 RVA: 0x0007222C File Offset: 0x0007062C
		public override float SkyTargetLerpFactor(Map map)
		{
			return GameConditionUtility.LerpInOutValue(this, 200f, 1f);
		}

		// Token: 0x06000D29 RID: 3369 RVA: 0x00072254 File Offset: 0x00070654
		public override SkyTarget? SkyTarget(Map map)
		{
			Color currentColor = this.CurrentColor;
			SkyColorSet colorSet = new SkyColorSet(Color.Lerp(Color.white, currentColor, 0.075f) * this.Brightness(map), new Color(0.92f, 0.92f, 0.92f), Color.Lerp(Color.white, currentColor, 0.025f) * this.Brightness(map), 1f);
			float glow = Mathf.Max(GenCelestial.CurCelestialSunGlow(map), 0.25f);
			return new SkyTarget?(new SkyTarget(glow, colorSet, 1f, 1f));
		}

		// Token: 0x06000D2A RID: 3370 RVA: 0x000722F0 File Offset: 0x000706F0
		private float Brightness(Map map)
		{
			return Mathf.Max(0.73f, GenCelestial.CurCelestialSunGlow(map));
		}

		// Token: 0x06000D2B RID: 3371 RVA: 0x00072318 File Offset: 0x00070718
		public override void GameConditionTick()
		{
			this.curColorTransition += 1f / (float)this.TransitionDurationTicks;
			if (this.curColorTransition >= 1f)
			{
				this.prevColorIndex = this.curColorIndex;
				this.curColorIndex = this.GetNewColorIndex();
				this.curColorTransition = 0f;
			}
			if (!base.Permanent && base.TicksLeft > 200 && this.BrightInAllMaps)
			{
				base.TicksLeft = 200;
			}
		}

		// Token: 0x06000D2C RID: 3372 RVA: 0x000723A8 File Offset: 0x000707A8
		private int GetNewColorIndex()
		{
			return (from x in Enumerable.Range(0, GameCondition_Aurora.Colors.Length)
			where x != this.curColorIndex
			select x).RandomElement<int>();
		}
	}
}
