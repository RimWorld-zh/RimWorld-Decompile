using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000307 RID: 775
	public class GameCondition : IExposable
	{
		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000CE5 RID: 3301 RVA: 0x00070FF8 File Offset: 0x0006F3F8
		protected Map SingleMap
		{
			get
			{
				return this.gameConditionManager.ownerMap;
			}
		}

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06000CE6 RID: 3302 RVA: 0x00071018 File Offset: 0x0006F418
		public virtual string Label
		{
			get
			{
				return this.def.label;
			}
		}

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06000CE7 RID: 3303 RVA: 0x00071038 File Offset: 0x0006F438
		public virtual string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst();
			}
		}

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x06000CE8 RID: 3304 RVA: 0x00071058 File Offset: 0x0006F458
		public virtual bool Expired
		{
			get
			{
				return !this.Permanent && Find.TickManager.TicksGame > this.startTick + this.Duration;
			}
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x06000CE9 RID: 3305 RVA: 0x00071094 File Offset: 0x0006F494
		public int TicksPassed
		{
			get
			{
				return Find.TickManager.TicksGame - this.startTick;
			}
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x06000CEA RID: 3306 RVA: 0x000710BC File Offset: 0x0006F4BC
		// (set) Token: 0x06000CEB RID: 3307 RVA: 0x00071105 File Offset: 0x0006F505
		public int TicksLeft
		{
			get
			{
				int result;
				if (this.Permanent)
				{
					Log.ErrorOnce("Trying to get ticks left of a permanent condition.", 384767654, false);
					result = 360000000;
				}
				else
				{
					result = this.Duration - this.TicksPassed;
				}
				return result;
			}
			set
			{
				this.Duration = this.TicksPassed + value;
			}
		}

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x06000CEC RID: 3308 RVA: 0x00071118 File Offset: 0x0006F518
		// (set) Token: 0x06000CED RID: 3309 RVA: 0x00071133 File Offset: 0x0006F533
		public bool Permanent
		{
			get
			{
				return this.permanent;
			}
			set
			{
				if (value)
				{
					this.duration = -1;
				}
				this.permanent = value;
			}
		}

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x06000CEE RID: 3310 RVA: 0x0007114C File Offset: 0x0006F54C
		// (set) Token: 0x06000CEF RID: 3311 RVA: 0x0007118E File Offset: 0x0006F58E
		public int Duration
		{
			get
			{
				int result;
				if (this.Permanent)
				{
					Log.ErrorOnce("Trying to get duration of a permanent condition.", 100394867, false);
					result = 360000000;
				}
				else
				{
					result = this.duration;
				}
				return result;
			}
			set
			{
				this.permanent = false;
				this.duration = value;
			}
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06000CF0 RID: 3312 RVA: 0x000711A0 File Offset: 0x0006F5A0
		public virtual string TooltipString
		{
			get
			{
				string text = this.def.LabelCap;
				if (this.Permanent)
				{
					text = text + "\n" + "Permanent".Translate().CapitalizeFirst();
				}
				else
				{
					Vector2 location;
					if (this.SingleMap != null)
					{
						location = Find.WorldGrid.LongLatOf(this.SingleMap.Tile);
					}
					else if (Find.CurrentMap != null)
					{
						location = Find.WorldGrid.LongLatOf(Find.CurrentMap.Tile);
					}
					else if (Find.AnyPlayerHomeMap != null)
					{
						location = Find.WorldGrid.LongLatOf(Find.AnyPlayerHomeMap.Tile);
					}
					else
					{
						location = Vector2.zero;
					}
					string text2 = text;
					text = string.Concat(new string[]
					{
						text2,
						"\n",
						"Started".Translate(),
						": ",
						GenDate.DateFullStringAt((long)GenDate.TickGameToAbs(this.startTick), location)
					});
					text2 = text;
					text = string.Concat(new string[]
					{
						text2,
						"\n",
						"Lasted".Translate(),
						": ",
						this.TicksPassed.ToStringTicksToPeriod()
					});
				}
				text += "\n";
				return text + "\n" + this.def.description;
			}
		}

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06000CF1 RID: 3313 RVA: 0x0007130C File Offset: 0x0006F70C
		public List<Map> AffectedMaps
		{
			get
			{
				if (!GenCollection.ListsEqual<Map>(this.cachedAffectedMapsForMaps, Find.Maps))
				{
					this.cachedAffectedMapsForMaps.Clear();
					this.cachedAffectedMapsForMaps.AddRange(Find.Maps);
					this.cachedAffectedMaps.Clear();
					if (this.gameConditionManager.ownerMap != null)
					{
						this.cachedAffectedMaps.Add(this.gameConditionManager.ownerMap);
					}
					GameCondition.tmpGameConditionManagers.Clear();
					this.gameConditionManager.GetChildren(GameCondition.tmpGameConditionManagers);
					for (int i = 0; i < GameCondition.tmpGameConditionManagers.Count; i++)
					{
						if (GameCondition.tmpGameConditionManagers[i].ownerMap != null)
						{
							this.cachedAffectedMaps.Add(GameCondition.tmpGameConditionManagers[i].ownerMap);
						}
					}
					GameCondition.tmpGameConditionManagers.Clear();
				}
				return this.cachedAffectedMaps;
			}
		}

		// Token: 0x06000CF2 RID: 3314 RVA: 0x000713FC File Offset: 0x0006F7FC
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<GameConditionDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.startTick, "startTick", 0, false);
			Scribe_Values.Look<int>(ref this.duration, "duration", 0, false);
			Scribe_Values.Look<bool>(ref this.permanent, "permanent", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				BackCompatibility.GameConditionPostLoadInit(this);
			}
		}

		// Token: 0x06000CF3 RID: 3315 RVA: 0x00071461 File Offset: 0x0006F861
		public virtual void GameConditionTick()
		{
		}

		// Token: 0x06000CF4 RID: 3316 RVA: 0x00071464 File Offset: 0x0006F864
		public virtual void GameConditionDraw(Map map)
		{
		}

		// Token: 0x06000CF5 RID: 3317 RVA: 0x00071467 File Offset: 0x0006F867
		public virtual void Init()
		{
		}

		// Token: 0x06000CF6 RID: 3318 RVA: 0x0007146A File Offset: 0x0006F86A
		public virtual void End()
		{
			if (this.def.endMessage != null)
			{
				Messages.Message(this.def.endMessage, MessageTypeDefOf.NeutralEvent, true);
			}
			this.gameConditionManager.ActiveConditions.Remove(this);
		}

		// Token: 0x06000CF7 RID: 3319 RVA: 0x000714A8 File Offset: 0x0006F8A8
		public virtual float SkyGazeChanceFactor(Map map)
		{
			return 1f;
		}

		// Token: 0x06000CF8 RID: 3320 RVA: 0x000714C4 File Offset: 0x0006F8C4
		public virtual float SkyGazeJoyGainFactor(Map map)
		{
			return 1f;
		}

		// Token: 0x06000CF9 RID: 3321 RVA: 0x000714E0 File Offset: 0x0006F8E0
		public virtual float TemperatureOffset()
		{
			return 0f;
		}

		// Token: 0x06000CFA RID: 3322 RVA: 0x000714FC File Offset: 0x0006F8FC
		public virtual float SkyTargetLerpFactor(Map map)
		{
			return 0f;
		}

		// Token: 0x06000CFB RID: 3323 RVA: 0x00071518 File Offset: 0x0006F918
		public virtual SkyTarget? SkyTarget(Map map)
		{
			return null;
		}

		// Token: 0x06000CFC RID: 3324 RVA: 0x00071538 File Offset: 0x0006F938
		public virtual float AnimalDensityFactor(Map map)
		{
			return 1f;
		}

		// Token: 0x06000CFD RID: 3325 RVA: 0x00071554 File Offset: 0x0006F954
		public virtual float PlantDensityFactor(Map map)
		{
			return 1f;
		}

		// Token: 0x06000CFE RID: 3326 RVA: 0x00071570 File Offset: 0x0006F970
		public virtual bool AllowEnjoyableOutsideNow(Map map)
		{
			return true;
		}

		// Token: 0x06000CFF RID: 3327 RVA: 0x00071588 File Offset: 0x0006F988
		public virtual List<SkyOverlay> SkyOverlays(Map map)
		{
			return null;
		}

		// Token: 0x06000D00 RID: 3328 RVA: 0x0007159E File Offset: 0x0006F99E
		public virtual void DoCellSteadyEffects(IntVec3 c, Map map)
		{
		}

		// Token: 0x0400085D RID: 2141
		public GameConditionManager gameConditionManager;

		// Token: 0x0400085E RID: 2142
		public GameConditionDef def;

		// Token: 0x0400085F RID: 2143
		public int startTick;

		// Token: 0x04000860 RID: 2144
		private int duration = -1;

		// Token: 0x04000861 RID: 2145
		private bool permanent;

		// Token: 0x04000862 RID: 2146
		private List<Map> cachedAffectedMaps = new List<Map>();

		// Token: 0x04000863 RID: 2147
		private List<Map> cachedAffectedMapsForMaps = new List<Map>();

		// Token: 0x04000864 RID: 2148
		private static List<GameConditionManager> tmpGameConditionManagers = new List<GameConditionManager>();
	}
}
