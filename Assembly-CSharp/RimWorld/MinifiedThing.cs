using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000673 RID: 1651
	public class MinifiedThing : ThingWithComps, IThingHolder
	{
		// Token: 0x0600228F RID: 8847 RVA: 0x0012A0B6 File Offset: 0x001284B6
		public MinifiedThing()
		{
			this.innerContainer = new ThingOwner<Thing>(this, true, LookMode.Deep);
		}

		// Token: 0x1700050E RID: 1294
		// (get) Token: 0x06002290 RID: 8848 RVA: 0x0012A0D0 File Offset: 0x001284D0
		// (set) Token: 0x06002291 RID: 8849 RVA: 0x0012A108 File Offset: 0x00128508
		public Thing InnerThing
		{
			get
			{
				Thing result;
				if (this.innerContainer.Count == 0)
				{
					result = null;
				}
				else
				{
					result = this.innerContainer[0];
				}
				return result;
			}
			set
			{
				if (value != this.InnerThing)
				{
					if (value == null)
					{
						this.innerContainer.Clear();
					}
					else
					{
						if (this.innerContainer.Count != 0)
						{
							Log.Warning(string.Concat(new string[]
							{
								"Assigned 2 things to the same MinifiedThing ",
								this.ToStringSafe<MinifiedThing>(),
								" (first=",
								this.innerContainer[0].ToStringSafe<Thing>(),
								" second=",
								value.ToStringSafe<Thing>(),
								")"
							}), false);
							this.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
						}
						this.innerContainer.TryAdd(value, true);
					}
				}
			}
		}

		// Token: 0x1700050F RID: 1295
		// (get) Token: 0x06002292 RID: 8850 RVA: 0x0012A1C4 File Offset: 0x001285C4
		public override Graphic Graphic
		{
			get
			{
				if (this.cachedGraphic == null)
				{
					this.cachedGraphic = this.InnerThing.Graphic.ExtractInnerGraphicFor(this.InnerThing);
					if ((float)this.InnerThing.def.size.x > 1.1f || (float)this.InnerThing.def.size.z > 1.1f)
					{
						Vector2 minifiedDrawSize = this.GetMinifiedDrawSize(this.InnerThing.def.size.ToVector2(), 1.1f);
						Vector2 newDrawSize = new Vector2(minifiedDrawSize.x / (float)this.InnerThing.def.size.x * this.cachedGraphic.drawSize.x, minifiedDrawSize.y / (float)this.InnerThing.def.size.z * this.cachedGraphic.drawSize.y);
						this.cachedGraphic = this.cachedGraphic.GetCopy(newDrawSize);
					}
				}
				return this.cachedGraphic;
			}
		}

		// Token: 0x17000510 RID: 1296
		// (get) Token: 0x06002293 RID: 8851 RVA: 0x0012A2E4 File Offset: 0x001286E4
		public override string LabelNoCount
		{
			get
			{
				return this.InnerThing.LabelNoCount;
			}
		}

		// Token: 0x17000511 RID: 1297
		// (get) Token: 0x06002294 RID: 8852 RVA: 0x0012A304 File Offset: 0x00128704
		public override string DescriptionDetailed
		{
			get
			{
				return this.InnerThing.DescriptionDetailed;
			}
		}

		// Token: 0x17000512 RID: 1298
		// (get) Token: 0x06002295 RID: 8853 RVA: 0x0012A324 File Offset: 0x00128724
		public override string DescriptionFlavor
		{
			get
			{
				return this.InnerThing.DescriptionFlavor;
			}
		}

		// Token: 0x06002296 RID: 8854 RVA: 0x0012A344 File Offset: 0x00128744
		public override void Tick()
		{
			if (this.InnerThing == null)
			{
				Log.Error("MinifiedThing with null InnerThing. Destroying.", false);
				this.Destroy(DestroyMode.Vanish);
			}
			else
			{
				base.Tick();
				if (this.InnerThing is Building_Battery)
				{
					this.innerContainer.ThingOwnerTick(true);
				}
			}
		}

		// Token: 0x06002297 RID: 8855 RVA: 0x0012A398 File Offset: 0x00128798
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x06002298 RID: 8856 RVA: 0x0012A3B3 File Offset: 0x001287B3
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x06002299 RID: 8857 RVA: 0x0012A3C4 File Offset: 0x001287C4
		public override Thing SplitOff(int count)
		{
			MinifiedThing minifiedThing = (MinifiedThing)base.SplitOff(count);
			if (minifiedThing != this)
			{
				minifiedThing.InnerThing = ThingMaker.MakeThing(this.InnerThing.def, this.InnerThing.Stuff);
				ThingWithComps thingWithComps = this.InnerThing as ThingWithComps;
				if (thingWithComps != null)
				{
					for (int i = 0; i < thingWithComps.AllComps.Count; i++)
					{
						thingWithComps.AllComps[i].PostSplitOff(minifiedThing.InnerThing);
					}
				}
			}
			return minifiedThing;
		}

		// Token: 0x0600229A RID: 8858 RVA: 0x0012A45C File Offset: 0x0012885C
		public override bool CanStackWith(Thing other)
		{
			MinifiedThing minifiedThing = other as MinifiedThing;
			return minifiedThing != null && base.CanStackWith(other) && this.InnerThing.CanStackWith(minifiedThing.InnerThing);
		}

		// Token: 0x0600229B RID: 8859 RVA: 0x0012A4A5 File Offset: 0x001288A5
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<ThingOwner>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
		}

		// Token: 0x0600229C RID: 8860 RVA: 0x0012A4C8 File Offset: 0x001288C8
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			Blueprint_Install blueprint_Install = InstallBlueprintUtility.ExistingBlueprintFor(this);
			if (blueprint_Install != null)
			{
				GenDraw.DrawLineBetween(this.TrueCenter(), blueprint_Install.TrueCenter());
			}
		}

		// Token: 0x0600229D RID: 8861 RVA: 0x0012A4FC File Offset: 0x001288FC
		public override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			if (this.crateFrontGraphic == null)
			{
				this.crateFrontGraphic = GraphicDatabase.Get<Graphic_Single>("Things/Item/Minified/CrateFront", ShaderDatabase.Cutout, this.GetMinifiedDrawSize(this.InnerThing.def.size.ToVector2(), 1.1f) * 1.16f, Color.white);
			}
			this.crateFrontGraphic.DrawFromDef(drawLoc + Altitudes.AltIncVect * 0.1f, Rot4.North, null, 0f);
			if (this.Graphic is Graphic_Single)
			{
				this.Graphic.Draw(drawLoc, Rot4.North, this, 0f);
			}
			else
			{
				this.Graphic.Draw(drawLoc, Rot4.South, this, 0f);
			}
		}

		// Token: 0x0600229E RID: 8862 RVA: 0x0012A5C8 File Offset: 0x001289C8
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			bool spawned = base.Spawned;
			Map map = base.Map;
			base.Destroy(mode);
			if (this.InnerThing != null)
			{
				InstallBlueprintUtility.CancelBlueprintsFor(this);
				if (mode == DestroyMode.Deconstruct && spawned)
				{
					SoundDefOf.Building_Deconstructed.PlayOneShot(new TargetInfo(base.Position, map, false));
					GenLeaving.DoLeavingsFor(this.InnerThing, map, mode, this.OccupiedRect(), null);
				}
			}
		}

		// Token: 0x0600229F RID: 8863 RVA: 0x0012A63D File Offset: 0x00128A3D
		public override void PreTraded(TradeAction action, Pawn playerNegotiator, ITrader trader)
		{
			base.PreTraded(action, playerNegotiator, trader);
			InstallBlueprintUtility.CancelBlueprintsFor(this);
		}

		// Token: 0x060022A0 RID: 8864 RVA: 0x0012A650 File Offset: 0x00128A50
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo c in this.<GetGizmos>__BaseCallProxy0())
			{
				yield return c;
			}
			yield return InstallationDesignatorDatabase.DesignatorFor(this.def);
			yield break;
		}

		// Token: 0x060022A1 RID: 8865 RVA: 0x0012A67C File Offset: 0x00128A7C
		public override string GetInspectString()
		{
			string text = "NotInstalled".Translate();
			string inspectString = this.InnerThing.GetInspectString();
			if (!inspectString.NullOrEmpty())
			{
				text += "\n";
				text += inspectString;
			}
			return text;
		}

		// Token: 0x060022A2 RID: 8866 RVA: 0x0012A6CC File Offset: 0x00128ACC
		private Vector2 GetMinifiedDrawSize(Vector2 drawSize, float maxSideLength)
		{
			float num = maxSideLength / Mathf.Max(drawSize.x, drawSize.y);
			Vector2 result;
			if (num >= 1f)
			{
				result = drawSize;
			}
			else
			{
				result = drawSize * num;
			}
			return result;
		}

		// Token: 0x0400138B RID: 5003
		private const float MaxMinifiedGraphicSize = 1.1f;

		// Token: 0x0400138C RID: 5004
		private const float CrateToGraphicScale = 1.16f;

		// Token: 0x0400138D RID: 5005
		private ThingOwner innerContainer;

		// Token: 0x0400138E RID: 5006
		private Graphic cachedGraphic;

		// Token: 0x0400138F RID: 5007
		private Graphic crateFrontGraphic;
	}
}
