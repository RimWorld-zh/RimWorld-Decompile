using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DFD RID: 3581
	public class ThingWithComps : Thing
	{
		// Token: 0x17000D3B RID: 3387
		// (get) Token: 0x060050DC RID: 20700 RVA: 0x001280CC File Offset: 0x001264CC
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

		// Token: 0x17000D3C RID: 3388
		// (get) Token: 0x060050DD RID: 20701 RVA: 0x00128100 File Offset: 0x00126500
		// (set) Token: 0x060050DE RID: 20702 RVA: 0x0012813F File Offset: 0x0012653F
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

		// Token: 0x17000D3D RID: 3389
		// (get) Token: 0x060050DF RID: 20703 RVA: 0x0012814C File Offset: 0x0012654C
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

		// Token: 0x17000D3E RID: 3390
		// (get) Token: 0x060050E0 RID: 20704 RVA: 0x001281AC File Offset: 0x001265AC
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

		// Token: 0x060050E1 RID: 20705 RVA: 0x00128248 File Offset: 0x00126648
		public override void PostMake()
		{
			base.PostMake();
			this.InitializeComps();
		}

		// Token: 0x060050E2 RID: 20706 RVA: 0x00128258 File Offset: 0x00126658
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

		// Token: 0x060050E3 RID: 20707 RVA: 0x001282CC File Offset: 0x001266CC
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

		// Token: 0x060050E4 RID: 20708 RVA: 0x001282F8 File Offset: 0x001266F8
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

		// Token: 0x060050E5 RID: 20709 RVA: 0x00128364 File Offset: 0x00126764
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

		// Token: 0x060050E6 RID: 20710 RVA: 0x00128404 File Offset: 0x00126804
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

		// Token: 0x060050E7 RID: 20711 RVA: 0x00128464 File Offset: 0x00126864
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

		// Token: 0x060050E8 RID: 20712 RVA: 0x001284C8 File Offset: 0x001268C8
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

		// Token: 0x060050E9 RID: 20713 RVA: 0x0012851C File Offset: 0x0012691C
		protected virtual void ReceiveCompSignal(string signal)
		{
		}

		// Token: 0x060050EA RID: 20714 RVA: 0x00128520 File Offset: 0x00126920
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

		// Token: 0x060050EB RID: 20715 RVA: 0x00128574 File Offset: 0x00126974
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

		// Token: 0x060050EC RID: 20716 RVA: 0x001285D0 File Offset: 0x001269D0
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

		// Token: 0x060050ED RID: 20717 RVA: 0x0012862C File Offset: 0x00126A2C
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

		// Token: 0x060050EE RID: 20718 RVA: 0x00128678 File Offset: 0x00126A78
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

		// Token: 0x060050EF RID: 20719 RVA: 0x001286C4 File Offset: 0x00126AC4
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

		// Token: 0x060050F0 RID: 20720 RVA: 0x00128738 File Offset: 0x00126B38
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

		// Token: 0x060050F1 RID: 20721 RVA: 0x0012878C File Offset: 0x00126B8C
		public override void Draw()
		{
			base.Draw();
			this.Comps_PostDraw();
		}

		// Token: 0x060050F2 RID: 20722 RVA: 0x0012879C File Offset: 0x00126B9C
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

		// Token: 0x060050F3 RID: 20723 RVA: 0x001287E8 File Offset: 0x00126BE8
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

		// Token: 0x060050F4 RID: 20724 RVA: 0x00128838 File Offset: 0x00126C38
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

		// Token: 0x060050F5 RID: 20725 RVA: 0x0012888C File Offset: 0x00126C8C
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

		// Token: 0x060050F6 RID: 20726 RVA: 0x001288D8 File Offset: 0x00126CD8
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

		// Token: 0x060050F7 RID: 20727 RVA: 0x00128904 File Offset: 0x00126D04
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

		// Token: 0x060050F8 RID: 20728 RVA: 0x0012897C File Offset: 0x00126D7C
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

		// Token: 0x060050F9 RID: 20729 RVA: 0x001289E0 File Offset: 0x00126DE0
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

		// Token: 0x060050FA RID: 20730 RVA: 0x00128A54 File Offset: 0x00126E54
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

		// Token: 0x060050FB RID: 20731 RVA: 0x00128AB4 File Offset: 0x00126EB4
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

		// Token: 0x060050FC RID: 20732 RVA: 0x00128B90 File Offset: 0x00126F90
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

		// Token: 0x060050FD RID: 20733 RVA: 0x00128BC4 File Offset: 0x00126FC4
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

		// Token: 0x060050FE RID: 20734 RVA: 0x00128C1C File Offset: 0x0012701C
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

		// Token: 0x060050FF RID: 20735 RVA: 0x00128C74 File Offset: 0x00127074
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

		// Token: 0x06005100 RID: 20736 RVA: 0x00128CC8 File Offset: 0x001270C8
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

		// Token: 0x04003536 RID: 13622
		private List<ThingComp> comps;

		// Token: 0x04003537 RID: 13623
		private static readonly List<ThingComp> EmptyCompsList = new List<ThingComp>();
	}
}
