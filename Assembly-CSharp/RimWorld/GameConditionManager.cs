using System;
using System.Collections.Generic;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200030C RID: 780
	public sealed class GameConditionManager : IExposable
	{
		// Token: 0x04000865 RID: 2149
		public Map ownerMap;

		// Token: 0x04000866 RID: 2150
		private List<GameCondition> activeConditions = new List<GameCondition>();

		// Token: 0x06000D0A RID: 3338 RVA: 0x00071807 File Offset: 0x0006FC07
		public GameConditionManager(Map map)
		{
			this.ownerMap = map;
		}

		// Token: 0x06000D0B RID: 3339 RVA: 0x00071822 File Offset: 0x0006FC22
		public GameConditionManager(World world)
		{
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06000D0C RID: 3340 RVA: 0x00071838 File Offset: 0x0006FC38
		public List<GameCondition> ActiveConditions
		{
			get
			{
				return this.activeConditions;
			}
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06000D0D RID: 3341 RVA: 0x00071854 File Offset: 0x0006FC54
		public GameConditionManager Parent
		{
			get
			{
				return (this.ownerMap != null) ? Find.World.gameConditionManager : null;
			}
		}

		// Token: 0x06000D0E RID: 3342 RVA: 0x00071884 File Offset: 0x0006FC84
		public void RegisterCondition(GameCondition cond)
		{
			this.activeConditions.Add(cond);
			cond.gameConditionManager = this;
			cond.Init();
		}

		// Token: 0x06000D0F RID: 3343 RVA: 0x000718A0 File Offset: 0x0006FCA0
		public void ExposeData()
		{
			Scribe_Collections.Look<GameCondition>(ref this.activeConditions, "activeConditions", LookMode.Deep, new object[0]);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				for (int i = 0; i < this.activeConditions.Count; i++)
				{
					this.activeConditions[i].gameConditionManager = this;
				}
			}
		}

		// Token: 0x06000D10 RID: 3344 RVA: 0x00071904 File Offset: 0x0006FD04
		public void GameConditionManagerTick()
		{
			for (int i = this.activeConditions.Count - 1; i >= 0; i--)
			{
				GameCondition gameCondition = this.activeConditions[i];
				if (gameCondition.Expired)
				{
					gameCondition.End();
				}
				else
				{
					gameCondition.GameConditionTick();
				}
			}
		}

		// Token: 0x06000D11 RID: 3345 RVA: 0x0007195C File Offset: 0x0006FD5C
		public void GameConditionManagerDraw(Map map)
		{
			for (int i = this.activeConditions.Count - 1; i >= 0; i--)
			{
				this.activeConditions[i].GameConditionDraw(map);
			}
			if (this.Parent != null)
			{
				this.Parent.GameConditionManagerDraw(map);
			}
		}

		// Token: 0x06000D12 RID: 3346 RVA: 0x000719B4 File Offset: 0x0006FDB4
		public void DoSteadyEffects(IntVec3 c, Map map)
		{
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				this.activeConditions[i].DoCellSteadyEffects(c, map);
			}
			if (this.Parent != null)
			{
				this.Parent.DoSteadyEffects(c, map);
			}
		}

		// Token: 0x06000D13 RID: 3347 RVA: 0x00071A0C File Offset: 0x0006FE0C
		public bool ConditionIsActive(GameConditionDef def)
		{
			return this.GetActiveCondition(def) != null;
		}

		// Token: 0x06000D14 RID: 3348 RVA: 0x00071A30 File Offset: 0x0006FE30
		public GameCondition GetActiveCondition(GameConditionDef def)
		{
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				if (def == this.activeConditions[i].def)
				{
					return this.activeConditions[i];
				}
			}
			if (this.Parent != null)
			{
				return this.Parent.GetActiveCondition(def);
			}
			return null;
		}

		// Token: 0x06000D15 RID: 3349 RVA: 0x00071AAC File Offset: 0x0006FEAC
		public T GetActiveCondition<T>() where T : GameCondition
		{
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				T t = this.activeConditions[i] as T;
				if (t != null)
				{
					return t;
				}
			}
			if (this.Parent != null)
			{
				return this.Parent.GetActiveCondition<T>();
			}
			return (T)((object)null);
		}

		// Token: 0x06000D16 RID: 3350 RVA: 0x00071B2C File Offset: 0x0006FF2C
		public void GetChildren(List<GameConditionManager> outChildren)
		{
			if (this == Find.World.gameConditionManager)
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					outChildren.Add(maps[i].gameConditionManager);
				}
			}
		}

		// Token: 0x06000D17 RID: 3351 RVA: 0x00071B80 File Offset: 0x0006FF80
		public float TotalHeightAt(float width)
		{
			float num = 0f;
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				num += Text.CalcHeight(this.activeConditions[i].LabelCap, width);
			}
			if (this.Parent != null)
			{
				num += this.Parent.TotalHeightAt(width);
			}
			return num;
		}

		// Token: 0x06000D18 RID: 3352 RVA: 0x00071BF0 File Offset: 0x0006FFF0
		public void DoConditionsUI(Rect rect)
		{
			GUI.BeginGroup(rect);
			float num = 0f;
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				float width = rect.width - 15f;
				Rect rect2 = new Rect(0f, num, width, Text.CalcHeight(this.activeConditions[i].LabelCap, width));
				Text.Font = GameFont.Small;
				Text.Anchor = TextAnchor.MiddleRight;
				Widgets.Label(rect2, this.activeConditions[i].LabelCap);
				GameCondition localCond = this.activeConditions[i];
				TooltipHandler.TipRegion(rect2, () => localCond.TooltipString, i * 631);
				num += rect2.height;
			}
			rect.yMin += num;
			GUI.EndGroup();
			Text.Anchor = TextAnchor.UpperLeft;
			if (this.Parent != null)
			{
				this.Parent.DoConditionsUI(rect);
			}
		}

		// Token: 0x06000D19 RID: 3353 RVA: 0x00071CF0 File Offset: 0x000700F0
		internal float AggregateSkyTargetLerpFactor(Map map)
		{
			float num = 0f;
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				num += (1f - num) * this.activeConditions[i].SkyTargetLerpFactor(map);
			}
			if (this.Parent != null)
			{
				num += this.Parent.AggregateSkyTargetLerpFactor(map);
			}
			return Mathf.Clamp01(num);
		}

		// Token: 0x06000D1A RID: 3354 RVA: 0x00071D68 File Offset: 0x00070168
		internal SkyTarget? AggregateSkyTarget(Map map)
		{
			SkyTarget value = default(SkyTarget);
			float num = 0f;
			this.AggregateSkyTargetWorker(ref value, ref num, map);
			SkyTarget? result;
			if (num == 0f)
			{
				result = null;
			}
			else
			{
				result = new SkyTarget?(value);
			}
			return result;
		}

		// Token: 0x06000D1B RID: 3355 RVA: 0x00071DB8 File Offset: 0x000701B8
		private void AggregateSkyTargetWorker(ref SkyTarget total, ref float lfTotal, Map map)
		{
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				GameCondition gameCondition = this.activeConditions[i];
				float num = gameCondition.SkyTargetLerpFactor(map);
				if (num > 0f)
				{
					if (lfTotal == 0f)
					{
						total = gameCondition.SkyTarget(map).Value;
						lfTotal = num;
					}
					else
					{
						lfTotal += num;
						total = SkyTarget.LerpDarken(total, gameCondition.SkyTarget(map).Value, num / lfTotal);
					}
				}
			}
			if (this.Parent != null)
			{
				this.Parent.AggregateSkyTargetWorker(ref total, ref lfTotal, map);
			}
		}

		// Token: 0x06000D1C RID: 3356 RVA: 0x00071E78 File Offset: 0x00070278
		internal float AggregateTemperatureOffset()
		{
			float num = 0f;
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				num += this.activeConditions[i].TemperatureOffset();
			}
			if (this.Parent != null)
			{
				num += this.Parent.AggregateTemperatureOffset();
			}
			return num;
		}

		// Token: 0x06000D1D RID: 3357 RVA: 0x00071EE0 File Offset: 0x000702E0
		internal float AggregateAnimalDensityFactor(Map map)
		{
			float num = 1f;
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				num *= this.activeConditions[i].AnimalDensityFactor(map);
			}
			if (this.Parent != null)
			{
				num *= this.Parent.AggregateAnimalDensityFactor(map);
			}
			return num;
		}

		// Token: 0x06000D1E RID: 3358 RVA: 0x00071F4C File Offset: 0x0007034C
		internal float AggregatePlantDensityFactor(Map map)
		{
			float num = 1f;
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				num *= this.activeConditions[i].PlantDensityFactor(map);
			}
			if (this.Parent != null)
			{
				num *= this.Parent.AggregatePlantDensityFactor(map);
			}
			return num;
		}

		// Token: 0x06000D1F RID: 3359 RVA: 0x00071FB8 File Offset: 0x000703B8
		internal float AggregateSkyGazeJoyGainFactor(Map map)
		{
			float num = 1f;
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				num *= this.activeConditions[i].SkyGazeJoyGainFactor(map);
			}
			if (this.Parent != null)
			{
				num *= this.Parent.AggregateSkyGazeJoyGainFactor(map);
			}
			return num;
		}

		// Token: 0x06000D20 RID: 3360 RVA: 0x00072024 File Offset: 0x00070424
		internal float AggregateSkyGazeChanceFactor(Map map)
		{
			float num = 1f;
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				num *= this.activeConditions[i].SkyGazeChanceFactor(map);
			}
			if (this.Parent != null)
			{
				num *= this.Parent.AggregateSkyGazeChanceFactor(map);
			}
			return num;
		}

		// Token: 0x06000D21 RID: 3361 RVA: 0x00072090 File Offset: 0x00070490
		internal bool AllowEnjoyableOutsideNow(Map map)
		{
			GameConditionDef gameConditionDef;
			return this.AllowEnjoyableOutsideNow(map, out gameConditionDef);
		}

		// Token: 0x06000D22 RID: 3362 RVA: 0x000720B0 File Offset: 0x000704B0
		internal bool AllowEnjoyableOutsideNow(Map map, out GameConditionDef reason)
		{
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				GameCondition gameCondition = this.activeConditions[i];
				if (!gameCondition.AllowEnjoyableOutsideNow(map))
				{
					reason = gameCondition.def;
					return false;
				}
			}
			reason = null;
			return this.Parent == null || this.Parent.AllowEnjoyableOutsideNow(map, out reason);
		}

		// Token: 0x06000D23 RID: 3363 RVA: 0x00072130 File Offset: 0x00070530
		public string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (GameCondition saveable in this.activeConditions)
			{
				stringBuilder.AppendLine(Scribe.saver.DebugOutputFor(saveable));
			}
			return stringBuilder.ToString();
		}
	}
}
