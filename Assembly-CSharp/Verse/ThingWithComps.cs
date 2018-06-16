using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DFE RID: 3582
	public class ThingWithComps : Thing
	{
		// Token: 0x17000D3C RID: 3388
		// (get) Token: 0x060050DE RID: 20702 RVA: 0x00128054 File Offset: 0x00126454
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
		// (get) Token: 0x060050DF RID: 20703 RVA: 0x00128088 File Offset: 0x00126488
		// (set) Token: 0x060050E0 RID: 20704 RVA: 0x001280C7 File Offset: 0x001264C7
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
		// (get) Token: 0x060050E1 RID: 20705 RVA: 0x001280D4 File Offset: 0x001264D4
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
		// (get) Token: 0x060050E2 RID: 20706 RVA: 0x00128134 File Offset: 0x00126534
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

		// Token: 0x060050E3 RID: 20707 RVA: 0x001281D0 File Offset: 0x001265D0
		public override void PostMake()
		{
			base.PostMake();
			this.InitializeComps();
		}

		// Token: 0x060050E4 RID: 20708 RVA: 0x001281E0 File Offset: 0x001265E0
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

		// Token: 0x060050E5 RID: 20709 RVA: 0x00128254 File Offset: 0x00126654
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

		// Token: 0x060050E6 RID: 20710 RVA: 0x00128280 File Offset: 0x00126680
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

		// Token: 0x060050E7 RID: 20711 RVA: 0x001282EC File Offset: 0x001266EC
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

		// Token: 0x060050E8 RID: 20712 RVA: 0x0012838C File Offset: 0x0012678C
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

		// Token: 0x060050E9 RID: 20713 RVA: 0x001283EC File Offset: 0x001267EC
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

		// Token: 0x060050EA RID: 20714 RVA: 0x00128450 File Offset: 0x00126850
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

		// Token: 0x060050EB RID: 20715 RVA: 0x001284A4 File Offset: 0x001268A4
		protected virtual void ReceiveCompSignal(string signal)
		{
		}

		// Token: 0x060050EC RID: 20716 RVA: 0x001284A8 File Offset: 0x001268A8
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

		// Token: 0x060050ED RID: 20717 RVA: 0x001284FC File Offset: 0x001268FC
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

		// Token: 0x060050EE RID: 20718 RVA: 0x00128558 File Offset: 0x00126958
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

		// Token: 0x060050EF RID: 20719 RVA: 0x001285B4 File Offset: 0x001269B4
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

		// Token: 0x060050F0 RID: 20720 RVA: 0x00128600 File Offset: 0x00126A00
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

		// Token: 0x060050F1 RID: 20721 RVA: 0x0012864C File Offset: 0x00126A4C
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

		// Token: 0x060050F2 RID: 20722 RVA: 0x001286C0 File Offset: 0x00126AC0
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

		// Token: 0x060050F3 RID: 20723 RVA: 0x00128714 File Offset: 0x00126B14
		public override void Draw()
		{
			base.Draw();
			this.Comps_PostDraw();
		}

		// Token: 0x060050F4 RID: 20724 RVA: 0x00128724 File Offset: 0x00126B24
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

		// Token: 0x060050F5 RID: 20725 RVA: 0x00128770 File Offset: 0x00126B70
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

		// Token: 0x060050F6 RID: 20726 RVA: 0x001287C0 File Offset: 0x00126BC0
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

		// Token: 0x060050F7 RID: 20727 RVA: 0x00128814 File Offset: 0x00126C14
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

		// Token: 0x060050F8 RID: 20728 RVA: 0x00128860 File Offset: 0x00126C60
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

		// Token: 0x060050F9 RID: 20729 RVA: 0x0012888C File Offset: 0x00126C8C
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

		// Token: 0x060050FA RID: 20730 RVA: 0x00128904 File Offset: 0x00126D04
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

		// Token: 0x060050FB RID: 20731 RVA: 0x00128968 File Offset: 0x00126D68
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

		// Token: 0x060050FC RID: 20732 RVA: 0x001289DC File Offset: 0x00126DDC
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

		// Token: 0x060050FD RID: 20733 RVA: 0x00128A3C File Offset: 0x00126E3C
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

		// Token: 0x060050FE RID: 20734 RVA: 0x00128B18 File Offset: 0x00126F18
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

		// Token: 0x060050FF RID: 20735 RVA: 0x00128B4C File Offset: 0x00126F4C
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

		// Token: 0x06005100 RID: 20736 RVA: 0x00128BA4 File Offset: 0x00126FA4
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

		// Token: 0x06005101 RID: 20737 RVA: 0x00128BFC File Offset: 0x00126FFC
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

		// Token: 0x06005102 RID: 20738 RVA: 0x00128C50 File Offset: 0x00127050
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

		// Token: 0x04003538 RID: 13624
		private List<ThingComp> comps;

		// Token: 0x04003539 RID: 13625
		private static readonly List<ThingComp> EmptyCompsList = new List<ThingComp>();
	}
}
