using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DFC RID: 3580
	public class ThingWithComps : Thing
	{
		// Token: 0x0400353D RID: 13629
		private List<ThingComp> comps;

		// Token: 0x0400353E RID: 13630
		private static readonly List<ThingComp> EmptyCompsList = new List<ThingComp>();

		// Token: 0x17000D3C RID: 3388
		// (get) Token: 0x060050F4 RID: 20724 RVA: 0x00128364 File Offset: 0x00126764
		public List<ThingComp> AllComps
		{
			get
			{
				List<ThingComp> emptyCompsList;
				if (this.comps == null)
				{
					emptyCompsList = ThingWithComps.EmptyCompsList;
				}
				else
				{
					emptyCompsList = this.comps;
				}
				return emptyCompsList;
			}
		}

		// Token: 0x17000D3D RID: 3389
		// (get) Token: 0x060050F5 RID: 20725 RVA: 0x00128398 File Offset: 0x00126798
		// (set) Token: 0x060050F6 RID: 20726 RVA: 0x001283D7 File Offset: 0x001267D7
		public override Color DrawColor
		{
			get
			{
				CompColorable comp = this.GetComp<CompColorable>();
				Color result;
				if (comp != null && comp.Active)
				{
					result = comp.Color;
				}
				else
				{
					result = base.DrawColor;
				}
				return result;
			}
			set
			{
				this.SetColor(value, true);
			}
		}

		// Token: 0x17000D3E RID: 3390
		// (get) Token: 0x060050F7 RID: 20727 RVA: 0x001283E4 File Offset: 0x001267E4
		public override string LabelNoCount
		{
			get
			{
				string text = base.LabelNoCount;
				if (this.comps != null)
				{
					int i = 0;
					int count = this.comps.Count;
					while (i < count)
					{
						text = this.comps[i].TransformLabel(text);
						i++;
					}
				}
				return text;
			}
		}

		// Token: 0x17000D3F RID: 3391
		// (get) Token: 0x060050F8 RID: 20728 RVA: 0x00128444 File Offset: 0x00126844
		public override string DescriptionFlavor
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(base.DescriptionFlavor);
				if (this.comps != null)
				{
					for (int i = 0; i < this.comps.Count; i++)
					{
						string descriptionPart = this.comps[i].GetDescriptionPart();
						if (!descriptionPart.NullOrEmpty())
						{
							if (stringBuilder.Length > 0)
							{
								stringBuilder.AppendLine();
								stringBuilder.AppendLine();
							}
							stringBuilder.Append(descriptionPart);
						}
					}
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x060050F9 RID: 20729 RVA: 0x001284E0 File Offset: 0x001268E0
		public override void PostMake()
		{
			base.PostMake();
			this.InitializeComps();
		}

		// Token: 0x060050FA RID: 20730 RVA: 0x001284F0 File Offset: 0x001268F0
		public T GetComp<T>() where T : ThingComp
		{
			if (this.comps != null)
			{
				int i = 0;
				int count = this.comps.Count;
				while (i < count)
				{
					T t = this.comps[i] as T;
					if (t != null)
					{
						return t;
					}
					i++;
				}
			}
			return (T)((object)null);
		}

		// Token: 0x060050FB RID: 20731 RVA: 0x00128564 File Offset: 0x00126964
		public IEnumerable<T> GetComps<T>() where T : ThingComp
		{
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					T cT = this.comps[i] as T;
					if (cT != null)
					{
						yield return cT;
					}
				}
			}
			yield break;
		}

		// Token: 0x060050FC RID: 20732 RVA: 0x00128590 File Offset: 0x00126990
		public ThingComp GetCompByDef(CompProperties def)
		{
			if (this.comps != null)
			{
				int i = 0;
				int count = this.comps.Count;
				while (i < count)
				{
					if (this.comps[i].props == def)
					{
						return this.comps[i];
					}
					i++;
				}
			}
			return null;
		}

		// Token: 0x060050FD RID: 20733 RVA: 0x001285FC File Offset: 0x001269FC
		public void InitializeComps()
		{
			if (this.def.comps.Any<CompProperties>())
			{
				this.comps = new List<ThingComp>();
				for (int i = 0; i < this.def.comps.Count; i++)
				{
					ThingComp thingComp = (ThingComp)Activator.CreateInstance(this.def.comps[i].compClass);
					thingComp.parent = this;
					this.comps.Add(thingComp);
					thingComp.Initialize(this.def.comps[i]);
				}
			}
		}

		// Token: 0x060050FE RID: 20734 RVA: 0x0012869C File Offset: 0x00126A9C
		public override string GetCustomLabelNoCount(bool includeHp = true)
		{
			string text = base.GetCustomLabelNoCount(includeHp);
			if (this.comps != null)
			{
				int i = 0;
				int count = this.comps.Count;
				while (i < count)
				{
					text = this.comps[i].TransformLabel(text);
					i++;
				}
			}
			return text;
		}

		// Token: 0x060050FF RID: 20735 RVA: 0x001286FC File Offset: 0x00126AFC
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.InitializeComps();
			}
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostExposeData();
				}
			}
		}

		// Token: 0x06005100 RID: 20736 RVA: 0x00128760 File Offset: 0x00126B60
		public void BroadcastCompSignal(string signal)
		{
			this.ReceiveCompSignal(signal);
			if (this.comps != null)
			{
				int i = 0;
				int count = this.comps.Count;
				while (i < count)
				{
					this.comps[i].ReceiveCompSignal(signal);
					i++;
				}
			}
		}

		// Token: 0x06005101 RID: 20737 RVA: 0x001287B4 File Offset: 0x00126BB4
		protected virtual void ReceiveCompSignal(string signal)
		{
		}

		// Token: 0x06005102 RID: 20738 RVA: 0x001287B8 File Offset: 0x00126BB8
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostSpawnSetup(respawningAfterLoad);
				}
			}
		}

		// Token: 0x06005103 RID: 20739 RVA: 0x0012880C File Offset: 0x00126C0C
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			base.DeSpawn(mode);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostDeSpawn(map);
				}
			}
		}

		// Token: 0x06005104 RID: 20740 RVA: 0x00128868 File Offset: 0x00126C68
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			base.Destroy(mode);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostDestroy(mode, map);
				}
			}
		}

		// Token: 0x06005105 RID: 20741 RVA: 0x001288C4 File Offset: 0x00126CC4
		public override void Tick()
		{
			if (this.comps != null)
			{
				int i = 0;
				int count = this.comps.Count;
				while (i < count)
				{
					this.comps[i].CompTick();
					i++;
				}
			}
		}

		// Token: 0x06005106 RID: 20742 RVA: 0x00128910 File Offset: 0x00126D10
		public override void TickRare()
		{
			if (this.comps != null)
			{
				int i = 0;
				int count = this.comps.Count;
				while (i < count)
				{
					this.comps[i].CompTickRare();
					i++;
				}
			}
		}

		// Token: 0x06005107 RID: 20743 RVA: 0x0012895C File Offset: 0x00126D5C
		public override void PreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
		{
			base.PreApplyDamage(ref dinfo, out absorbed);
			if (!absorbed)
			{
				if (this.comps != null)
				{
					for (int i = 0; i < this.comps.Count; i++)
					{
						this.comps[i].PostPreApplyDamage(dinfo, out absorbed);
						if (absorbed)
						{
							break;
						}
					}
				}
			}
		}

		// Token: 0x06005108 RID: 20744 RVA: 0x001289D0 File Offset: 0x00126DD0
		public override void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			base.PostApplyDamage(dinfo, totalDamageDealt);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostPostApplyDamage(dinfo, totalDamageDealt);
				}
			}
		}

		// Token: 0x06005109 RID: 20745 RVA: 0x00128A24 File Offset: 0x00126E24
		public override void Draw()
		{
			base.Draw();
			this.Comps_PostDraw();
		}

		// Token: 0x0600510A RID: 20746 RVA: 0x00128A34 File Offset: 0x00126E34
		protected void Comps_PostDraw()
		{
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostDraw();
				}
			}
		}

		// Token: 0x0600510B RID: 20747 RVA: 0x00128A80 File Offset: 0x00126E80
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostDrawExtraSelectionOverlays();
				}
			}
		}

		// Token: 0x0600510C RID: 20748 RVA: 0x00128AD0 File Offset: 0x00126ED0
		public override void Print(SectionLayer layer)
		{
			base.Print(layer);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostPrintOnto(layer);
				}
			}
		}

		// Token: 0x0600510D RID: 20749 RVA: 0x00128B24 File Offset: 0x00126F24
		public virtual void PrintForPowerGrid(SectionLayer layer)
		{
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].CompPrintForPowerGrid(layer);
				}
			}
		}

		// Token: 0x0600510E RID: 20750 RVA: 0x00128B70 File Offset: 0x00126F70
		public override IEnumerable<Gizmo> GetGizmos()
		{
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					foreach (Gizmo com in this.comps[i].CompGetGizmosExtra())
					{
						yield return com;
					}
				}
			}
			yield break;
		}

		// Token: 0x0600510F RID: 20751 RVA: 0x00128B9C File Offset: 0x00126F9C
		public override bool TryAbsorbStack(Thing other, bool respectStackLimit)
		{
			bool result;
			if (!this.CanStackWith(other))
			{
				result = false;
			}
			else
			{
				int count = ThingUtility.TryAbsorbStackNumToTake(this, other, respectStackLimit);
				if (this.comps != null)
				{
					for (int i = 0; i < this.comps.Count; i++)
					{
						this.comps[i].PreAbsorbStack(other, count);
					}
				}
				result = base.TryAbsorbStack(other, respectStackLimit);
			}
			return result;
		}

		// Token: 0x06005110 RID: 20752 RVA: 0x00128C14 File Offset: 0x00127014
		public override Thing SplitOff(int count)
		{
			Thing thing = base.SplitOff(count);
			if (thing != null && this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostSplitOff(thing);
				}
			}
			return thing;
		}

		// Token: 0x06005111 RID: 20753 RVA: 0x00128C78 File Offset: 0x00127078
		public override bool CanStackWith(Thing other)
		{
			bool result;
			if (!base.CanStackWith(other))
			{
				result = false;
			}
			else
			{
				if (this.comps != null)
				{
					for (int i = 0; i < this.comps.Count; i++)
					{
						if (!this.comps[i].AllowStackWith(other))
						{
							return false;
						}
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06005112 RID: 20754 RVA: 0x00128CEC File Offset: 0x001270EC
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			string text = this.InspectStringPartsFromComps();
			if (!text.NullOrEmpty())
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.Append(text);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06005113 RID: 20755 RVA: 0x00128D4C File Offset: 0x0012714C
		protected string InspectStringPartsFromComps()
		{
			string result;
			if (this.comps == null)
			{
				result = null;
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < this.comps.Count; i++)
				{
					string text = this.comps[i].CompInspectStringExtra();
					if (!text.NullOrEmpty())
					{
						if (Prefs.DevMode && char.IsWhiteSpace(text[text.Length - 1]))
						{
							Log.ErrorOnce(this.comps[i].GetType() + " CompInspectStringExtra ended with whitespace: " + text, 25612, false);
							text = text.TrimEndNewlines();
						}
						if (stringBuilder.Length != 0)
						{
							stringBuilder.AppendLine();
						}
						stringBuilder.Append(text);
					}
				}
				result = stringBuilder.ToString();
			}
			return result;
		}

		// Token: 0x06005114 RID: 20756 RVA: 0x00128E28 File Offset: 0x00127228
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
		{
			foreach (FloatMenuOption o in this.<GetFloatMenuOptions>__BaseCallProxy0(selPawn))
			{
				yield return o;
			}
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					foreach (FloatMenuOption o2 in this.comps[i].CompFloatMenuOptions(selPawn))
					{
						yield return o2;
					}
				}
			}
			yield break;
		}

		// Token: 0x06005115 RID: 20757 RVA: 0x00128E5C File Offset: 0x0012725C
		public override void PreTraded(TradeAction action, Pawn playerNegotiator, ITrader trader)
		{
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PrePreTraded(action, playerNegotiator, trader);
				}
			}
			base.PreTraded(action, playerNegotiator, trader);
		}

		// Token: 0x06005116 RID: 20758 RVA: 0x00128EB4 File Offset: 0x001272B4
		public override void PostGeneratedForTrader(TraderKindDef trader, int forTile, Faction forFaction)
		{
			base.PostGeneratedForTrader(trader, forTile, forFaction);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostPostGeneratedForTrader(trader, forTile, forFaction);
				}
			}
		}

		// Token: 0x06005117 RID: 20759 RVA: 0x00128F0C File Offset: 0x0012730C
		protected override void PostIngested(Pawn ingester)
		{
			base.PostIngested(ingester);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostIngested(ingester);
				}
			}
		}

		// Token: 0x06005118 RID: 20760 RVA: 0x00128F60 File Offset: 0x00127360
		public override void Notify_SignalReceived(Signal signal)
		{
			base.Notify_SignalReceived(signal);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].Notify_SignalReceived(signal);
				}
			}
		}
	}
}
