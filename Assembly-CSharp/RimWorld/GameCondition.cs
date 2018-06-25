using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000309 RID: 777
	public class GameCondition : IExposable
	{
		// Token: 0x04000860 RID: 2144
		public GameConditionManager gameConditionManager;

		// Token: 0x04000861 RID: 2145
		public GameConditionDef def;

		// Token: 0x04000862 RID: 2146
		public int startTick;

		// Token: 0x04000863 RID: 2147
		private int duration = -1;

		// Token: 0x04000864 RID: 2148
		private bool permanent;

		// Token: 0x04000865 RID: 2149
		private List<Map> cachedAffectedMaps = new List<Map>();

		// Token: 0x04000866 RID: 2150
		private List<Map> cachedAffectedMapsForMaps = new List<Map>();

		// Token: 0x04000867 RID: 2151
		private static List<GameConditionManager> tmpGameConditionManagers = new List<GameConditionManager>();

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000CE8 RID: 3304 RVA: 0x00071150 File Offset: 0x0006F550
		protected Map SingleMap
		{
			get
			{
				return this.gameConditionManager.ownerMap;
			}
		}

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06000CE9 RID: 3305 RVA: 0x00071170 File Offset: 0x0006F570
		public virtual string Label
		{
			get
			{
				return this.def.label;
			}
		}

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06000CEA RID: 3306 RVA: 0x00071190 File Offset: 0x0006F590
		public virtual string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst();
			}
		}

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x06000CEB RID: 3307 RVA: 0x000711B0 File Offset: 0x0006F5B0
		public virtual bool Expired
		{
			get
			{
				return !this.Permanent && Find.TickManager.TicksGame > this.startTick + this.Duration;
			}
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x06000CEC RID: 3308 RVA: 0x000711EC File Offset: 0x0006F5EC
		public int TicksPassed
		{
			get
			{
				return Find.TickManager.TicksGame - this.startTick;
			}
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x06000CED RID: 3309 RVA: 0x00071214 File Offset: 0x0006F614
		// (set) Token: 0x06000CEE RID: 3310 RVA: 0x0007125D File Offset: 0x0006F65D
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
		// (get) Token: 0x06000CEF RID: 3311 RVA: 0x00071270 File Offset: 0x0006F670
		// (set) Token: 0x06000CF0 RID: 3312 RVA: 0x0007128B File Offset: 0x0006F68B
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
		// (get) Token: 0x06000CF1 RID: 3313 RVA: 0x000712A4 File Offset: 0x0006F6A4
		// (set) Token: 0x06000CF2 RID: 3314 RVA: 0x000712E6 File Offset: 0x0006F6E6
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
		// (get) Token: 0x06000CF3 RID: 3315 RVA: 0x000712F8 File Offset: 0x0006F6F8
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
		// (get) Token: 0x06000CF4 RID: 3316 RVA: 0x00071464 File Offset: 0x0006F864
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

		// Token: 0x06000CF5 RID: 3317 RVA: 0x00071554 File Offset: 0x0006F954
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

		// Token: 0x06000CF6 RID: 3318 RVA: 0x000715B9 File Offset: 0x0006F9B9
		public virtual void GameConditionTick()
		{
		}

		// Token: 0x06000CF7 RID: 3319 RVA: 0x000715BC File Offset: 0x0006F9BC
		public virtual void GameConditionDraw(Map map)
		{
		}

		// Token: 0x06000CF8 RID: 3320 RVA: 0x000715BF File Offset: 0x0006F9BF
		public virtual void Init()
		{
		}

		// Token: 0x06000CF9 RID: 3321 RVA: 0x000715C2 File Offset: 0x0006F9C2
		public virtual void End()
		{
			if (this.def.endMessage != null)
			{
				Messages.Message(this.def.endMessage, MessageTypeDefOf.NeutralEvent, true);
			}
			this.gameConditionManager.ActiveConditions.Remove(this);
		}

		// Token: 0x06000CFA RID: 3322 RVA: 0x00071600 File Offset: 0x0006FA00
		public virtual float SkyGazeChanceFactor(Map map)
		{
			return 1f;
		}

		// Token: 0x06000CFB RID: 3323 RVA: 0x0007161C File Offset: 0x0006FA1C
		public virtual float SkyGazeJoyGainFactor(Map map)
		{
			return 1f;
		}

		// Token: 0x06000CFC RID: 3324 RVA: 0x00071638 File Offset: 0x0006FA38
		public virtual float TemperatureOffset()
		{
			return 0f;
		}

		// Token: 0x06000CFD RID: 3325 RVA: 0x00071654 File Offset: 0x0006FA54
		public virtual float SkyTargetLerpFactor(Map map)
		{
			return 0f;
		}

		// Token: 0x06000CFE RID: 3326 RVA: 0x00071670 File Offset: 0x0006FA70
		public virtual SkyTarget? SkyTarget(Map map)
		{
			return null;
		}

		// Token: 0x06000CFF RID: 3327 RVA: 0x00071690 File Offset: 0x0006FA90
		public virtual float AnimalDensityFactor(Map map)
		{
			return 1f;
		}

		// Token: 0x06000D00 RID: 3328 RVA: 0x000716AC File Offset: 0x0006FAAC
		public virtual float PlantDensityFactor(Map map)
		{
			return 1f;
		}

		// Token: 0x06000D01 RID: 3329 RVA: 0x000716C8 File Offset: 0x0006FAC8
		public virtual bool AllowEnjoyableOutsideNow(Map map)
		{
			return true;
		}

		// Token: 0x06000D02 RID: 3330 RVA: 0x000716E0 File Offset: 0x0006FAE0
		public virtual List<SkyOverlay> SkyOverlays(Map map)
		{
			return null;
		}

		// Token: 0x06000D03 RID: 3331 RVA: 0x000716F6 File Offset: 0x0006FAF6
		public virtual void DoCellSteadyEffects(IntVec3 c, Map map)
		{
		}
	}
}
