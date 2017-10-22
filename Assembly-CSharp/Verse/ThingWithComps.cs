using RimWorld;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Verse
{
	public class ThingWithComps : Thing
	{
		private List<ThingComp> comps;

		private static readonly List<ThingComp> EmptyCompsList = new List<ThingComp>();

		public List<ThingComp> AllComps
		{
			get
			{
				return (this.comps != null) ? this.comps : ThingWithComps.EmptyCompsList;
			}
		}

		public override Color DrawColor
		{
			get
			{
				CompColorable comp = this.GetComp<CompColorable>();
				return (comp == null || !comp.Active) ? base.DrawColor : comp.Color;
			}
			set
			{
				this.SetColor(value, true);
			}
		}

		public override string LabelNoCount
		{
			get
			{
				string text = GenLabel.ThingLabel(this);
				if (this.comps != null)
				{
					int num = 0;
					int count = this.comps.Count;
					while (num < count)
					{
						text = this.comps[num].TransformLabel(text);
						num++;
					}
				}
				return text;
			}
		}

		public override void PostMake()
		{
			base.PostMake();
			this.InitializeComps();
		}

		public T GetComp<T>() where T : ThingComp
		{
			T val;
			if (this.comps != null)
			{
				int num = 0;
				int count = this.comps.Count;
				while (num < count)
				{
					val = (T)(this.comps[num] as T);
					if (val != null)
						goto IL_0043;
					num++;
				}
			}
			T result = (T)null;
			goto IL_0063;
			IL_0043:
			result = val;
			goto IL_0063;
			IL_0063:
			return result;
		}

		public IEnumerable<T> GetComps<T>() where T : ThingComp
		{
			if (this.comps == null)
				yield break;
			int i = 0;
			T cT;
			while (true)
			{
				if (i < this.comps.Count)
				{
					cT = (T)(this.comps[i] as T);
					if (cT == null)
					{
						i++;
						continue;
					}
					break;
				}
				yield break;
			}
			yield return cT;
			/*Error: Unable to find new state assignment for yield return*/;
		}

		public ThingComp GetCompByDef(CompProperties def)
		{
			int num;
			if (this.comps != null)
			{
				num = 0;
				int count = this.comps.Count;
				while (num < count)
				{
					if (this.comps[num].props == def)
						goto IL_0038;
					num++;
				}
			}
			ThingComp result = null;
			goto IL_005e;
			IL_0038:
			result = this.comps[num];
			goto IL_005e;
			IL_005e:
			return result;
		}

		public void InitializeComps()
		{
			if (base.def.comps.Any())
			{
				this.comps = new List<ThingComp>();
				for (int i = 0; i < base.def.comps.Count; i++)
				{
					ThingComp thingComp = (ThingComp)Activator.CreateInstance(base.def.comps[i].compClass);
					thingComp.parent = this;
					this.comps.Add(thingComp);
					thingComp.Initialize(base.def.comps[i]);
				}
			}
		}

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

		public void BroadcastCompSignal(string signal)
		{
			this.ReceiveCompSignal(signal);
			if (this.comps != null)
			{
				int num = 0;
				int count = this.comps.Count;
				while (num < count)
				{
					this.comps[num].ReceiveCompSignal(signal);
					num++;
				}
			}
		}

		protected virtual void ReceiveCompSignal(string signal)
		{
		}

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

		public override void DeSpawn()
		{
			Map map = base.Map;
			base.DeSpawn();
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostDeSpawn(map);
				}
			}
		}

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

		public override void Tick()
		{
			if (this.comps != null)
			{
				int num = 0;
				int count = this.comps.Count;
				while (num < count)
				{
					this.comps[num].CompTick();
					num++;
				}
			}
		}

		public override void TickRare()
		{
			if (this.comps != null)
			{
				int num = 0;
				int count = this.comps.Count;
				while (num < count)
				{
					this.comps[num].CompTickRare();
					num++;
				}
			}
		}

		public override void PreApplyDamage(DamageInfo dinfo, out bool absorbed)
		{
			base.PreApplyDamage(dinfo, out absorbed);
			if (!absorbed && this.comps != null)
			{
				int num = 0;
				while (num < this.comps.Count)
				{
					this.comps[num].PostPreApplyDamage(dinfo, out absorbed);
					if (!absorbed)
					{
						num++;
						continue;
					}
					break;
				}
			}
		}

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

		public override void Draw()
		{
			base.Draw();
			this.Comps_PostDraw();
		}

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

		public override IEnumerable<Gizmo> GetGizmos()
		{
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					using (IEnumerator<Gizmo> enumerator = this.comps[i].CompGetGizmosExtra().GetEnumerator())
					{
						if (enumerator.MoveNext())
						{
							Gizmo com = enumerator.Current;
							yield return com;
							/*Error: Unable to find new state assignment for yield return*/;
						}
					}
				}
			}
			yield break;
			IL_0116:
			/*Error near IL_0117: Unexpected return in MoveNext()*/;
		}

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
							goto IL_003f;
					}
				}
				result = true;
			}
			goto IL_0064;
			IL_0064:
			return result;
			IL_003f:
			result = false;
			goto IL_0064;
		}

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

		protected string InspectStringPartsFromComps()
		{
			string result;
			if (this.comps == null)
			{
				result = (string)null;
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
							Log.ErrorOnce(this.comps[i].GetType() + " CompInspectStringExtra ended with whitespace: " + text, 25612);
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

		public override string GetDescription()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetDescription());
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

		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
		{
			using (IEnumerator<FloatMenuOption> enumerator = this._003CGetFloatMenuOptions_003E__BaseCallProxy0(selPawn).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					FloatMenuOption o2 = enumerator.Current;
					yield return o2;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					using (IEnumerator<FloatMenuOption> enumerator2 = this.comps[i].CompFloatMenuOptions(selPawn).GetEnumerator())
					{
						if (enumerator2.MoveNext())
						{
							FloatMenuOption o = enumerator2.Current;
							yield return o;
							/*Error: Unable to find new state assignment for yield return*/;
						}
					}
				}
			}
			yield break;
			IL_01b6:
			/*Error near IL_01b7: Unexpected return in MoveNext()*/;
		}

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
