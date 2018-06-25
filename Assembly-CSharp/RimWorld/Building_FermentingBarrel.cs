using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020006A7 RID: 1703
	[StaticConstructorOnStartup]
	public class Building_FermentingBarrel : Building
	{
		// Token: 0x04001426 RID: 5158
		private int wortCount;

		// Token: 0x04001427 RID: 5159
		private float progressInt;

		// Token: 0x04001428 RID: 5160
		private Material barFilledCachedMat;

		// Token: 0x04001429 RID: 5161
		public const int MaxCapacity = 25;

		// Token: 0x0400142A RID: 5162
		private const int BaseFermentationDuration = 360000;

		// Token: 0x0400142B RID: 5163
		public const float MinIdealTemperature = 7f;

		// Token: 0x0400142C RID: 5164
		private static readonly Vector2 BarSize = new Vector2(0.55f, 0.1f);

		// Token: 0x0400142D RID: 5165
		private static readonly Color BarZeroProgressColor = new Color(0.4f, 0.27f, 0.22f);

		// Token: 0x0400142E RID: 5166
		private static readonly Color BarFermentedColor = new Color(0.9f, 0.85f, 0.2f);

		// Token: 0x0400142F RID: 5167
		private static readonly Material BarUnfilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.3f, 0.3f, 0.3f), false);

		// Token: 0x17000575 RID: 1397
		// (get) Token: 0x0600245A RID: 9306 RVA: 0x001379BC File Offset: 0x00135DBC
		// (set) Token: 0x0600245B RID: 9307 RVA: 0x001379D7 File Offset: 0x00135DD7
		public float Progress
		{
			get
			{
				return this.progressInt;
			}
			set
			{
				if (value != this.progressInt)
				{
					this.progressInt = value;
					this.barFilledCachedMat = null;
				}
			}
		}

		// Token: 0x17000576 RID: 1398
		// (get) Token: 0x0600245C RID: 9308 RVA: 0x001379FC File Offset: 0x00135DFC
		private Material BarFilledMat
		{
			get
			{
				if (this.barFilledCachedMat == null)
				{
					this.barFilledCachedMat = SolidColorMaterials.SimpleSolidColorMaterial(Color.Lerp(Building_FermentingBarrel.BarZeroProgressColor, Building_FermentingBarrel.BarFermentedColor, this.Progress), false);
				}
				return this.barFilledCachedMat;
			}
		}

		// Token: 0x17000577 RID: 1399
		// (get) Token: 0x0600245D RID: 9309 RVA: 0x00137A4C File Offset: 0x00135E4C
		public int SpaceLeftForWort
		{
			get
			{
				int result;
				if (this.Fermented)
				{
					result = 0;
				}
				else
				{
					result = 25 - this.wortCount;
				}
				return result;
			}
		}

		// Token: 0x17000578 RID: 1400
		// (get) Token: 0x0600245E RID: 9310 RVA: 0x00137A7C File Offset: 0x00135E7C
		private bool Empty
		{
			get
			{
				return this.wortCount <= 0;
			}
		}

		// Token: 0x17000579 RID: 1401
		// (get) Token: 0x0600245F RID: 9311 RVA: 0x00137AA0 File Offset: 0x00135EA0
		public bool Fermented
		{
			get
			{
				return !this.Empty && this.Progress >= 1f;
			}
		}

		// Token: 0x1700057A RID: 1402
		// (get) Token: 0x06002460 RID: 9312 RVA: 0x00137AD4 File Offset: 0x00135ED4
		private float CurrentTempProgressSpeedFactor
		{
			get
			{
				CompProperties_TemperatureRuinable compProperties = this.def.GetCompProperties<CompProperties_TemperatureRuinable>();
				float ambientTemperature = base.AmbientTemperature;
				float result;
				if (ambientTemperature < compProperties.minSafeTemperature)
				{
					result = 0.1f;
				}
				else if (ambientTemperature < 7f)
				{
					result = GenMath.LerpDouble(compProperties.minSafeTemperature, 7f, 0.1f, 1f, ambientTemperature);
				}
				else
				{
					result = 1f;
				}
				return result;
			}
		}

		// Token: 0x1700057B RID: 1403
		// (get) Token: 0x06002461 RID: 9313 RVA: 0x00137B44 File Offset: 0x00135F44
		private float ProgressPerTickAtCurrentTemp
		{
			get
			{
				return 2.77777781E-06f * this.CurrentTempProgressSpeedFactor;
			}
		}

		// Token: 0x1700057C RID: 1404
		// (get) Token: 0x06002462 RID: 9314 RVA: 0x00137B68 File Offset: 0x00135F68
		private int EstimatedTicksLeft
		{
			get
			{
				return Mathf.Max(Mathf.RoundToInt((1f - this.Progress) / this.ProgressPerTickAtCurrentTemp), 0);
			}
		}

		// Token: 0x06002463 RID: 9315 RVA: 0x00137B9B File Offset: 0x00135F9B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.wortCount, "wortCount", 0, false);
			Scribe_Values.Look<float>(ref this.progressInt, "progress", 0f, false);
		}

		// Token: 0x06002464 RID: 9316 RVA: 0x00137BCC File Offset: 0x00135FCC
		public override void TickRare()
		{
			base.TickRare();
			if (!this.Empty)
			{
				this.Progress = Mathf.Min(this.Progress + 250f * this.ProgressPerTickAtCurrentTemp, 1f);
			}
		}

		// Token: 0x06002465 RID: 9317 RVA: 0x00137C04 File Offset: 0x00136004
		public void AddWort(int count)
		{
			base.GetComp<CompTemperatureRuinable>().Reset();
			if (this.Fermented)
			{
				Log.Warning("Tried to add wort to a barrel full of beer. Colonists should take the beer first.", false);
			}
			else
			{
				int num = Mathf.Min(count, 25 - this.wortCount);
				if (num > 0)
				{
					this.Progress = GenMath.WeightedAverage(0f, (float)num, this.Progress, (float)this.wortCount);
					this.wortCount += num;
				}
			}
		}

		// Token: 0x06002466 RID: 9318 RVA: 0x00137C82 File Offset: 0x00136082
		protected override void ReceiveCompSignal(string signal)
		{
			if (signal == "RuinedByTemperature")
			{
				this.Reset();
			}
		}

		// Token: 0x06002467 RID: 9319 RVA: 0x00137C9B File Offset: 0x0013609B
		private void Reset()
		{
			this.wortCount = 0;
			this.Progress = 0f;
		}

		// Token: 0x06002468 RID: 9320 RVA: 0x00137CB0 File Offset: 0x001360B0
		public void AddWort(Thing wort)
		{
			int num = Mathf.Min(wort.stackCount, 25 - this.wortCount);
			if (num > 0)
			{
				this.AddWort(num);
				wort.SplitOff(num).Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06002469 RID: 9321 RVA: 0x00137CF0 File Offset: 0x001360F0
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			if (stringBuilder.Length != 0)
			{
				stringBuilder.AppendLine();
			}
			CompTemperatureRuinable comp = base.GetComp<CompTemperatureRuinable>();
			if (!this.Empty && !comp.Ruined)
			{
				if (this.Fermented)
				{
					stringBuilder.AppendLine("ContainsBeer".Translate(new object[]
					{
						this.wortCount,
						25
					}));
				}
				else
				{
					stringBuilder.AppendLine("ContainsWort".Translate(new object[]
					{
						this.wortCount,
						25
					}));
				}
			}
			if (!this.Empty)
			{
				if (this.Fermented)
				{
					stringBuilder.AppendLine("Fermented".Translate());
				}
				else
				{
					stringBuilder.AppendLine("FermentationProgress".Translate(new object[]
					{
						this.Progress.ToStringPercent(),
						this.EstimatedTicksLeft.ToStringTicksToPeriod()
					}));
					if (this.CurrentTempProgressSpeedFactor != 1f)
					{
						stringBuilder.AppendLine("FermentationBarrelOutOfIdealTemperature".Translate(new object[]
						{
							this.CurrentTempProgressSpeedFactor.ToStringPercent()
						}));
					}
				}
			}
			stringBuilder.AppendLine("Temperature".Translate() + ": " + base.AmbientTemperature.ToStringTemperature("F0"));
			stringBuilder.AppendLine(string.Concat(new string[]
			{
				"IdealFermentingTemperature".Translate(),
				": ",
				7f.ToStringTemperature("F0"),
				" ~ ",
				comp.Props.maxSafeTemperature.ToStringTemperature("F0")
			}));
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x0600246A RID: 9322 RVA: 0x00137EE0 File Offset: 0x001362E0
		public Thing TakeOutBeer()
		{
			Thing result;
			if (!this.Fermented)
			{
				Log.Warning("Tried to get beer but it's not yet fermented.", false);
				result = null;
			}
			else
			{
				Thing thing = ThingMaker.MakeThing(ThingDefOf.Beer, null);
				thing.stackCount = this.wortCount;
				this.Reset();
				result = thing;
			}
			return result;
		}

		// Token: 0x0600246B RID: 9323 RVA: 0x00137F34 File Offset: 0x00136334
		public override void Draw()
		{
			base.Draw();
			if (!this.Empty)
			{
				Vector3 drawPos = this.DrawPos;
				drawPos.y += 0.046875f;
				drawPos.z += 0.25f;
				GenDraw.DrawFillableBar(new GenDraw.FillableBarRequest
				{
					center = drawPos,
					size = Building_FermentingBarrel.BarSize,
					fillPercent = (float)this.wortCount / 25f,
					filledMat = this.BarFilledMat,
					unfilledMat = Building_FermentingBarrel.BarUnfilledMat,
					margin = 0.1f,
					rotation = Rot4.North
				});
			}
		}

		// Token: 0x0600246C RID: 9324 RVA: 0x00137FEC File Offset: 0x001363EC
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo g in this.<GetGizmos>__BaseCallProxy0())
			{
				yield return g;
			}
			if (Prefs.DevMode && !this.Empty)
			{
				yield return new Command_Action
				{
					defaultLabel = "Debug: Set progress to 1",
					action = delegate()
					{
						this.Progress = 1f;
					}
				};
			}
			yield break;
		}
	}
}
