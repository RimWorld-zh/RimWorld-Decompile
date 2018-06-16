using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005FF RID: 1535
	public class FactionBase : Settlement
	{
		// Token: 0x06001E82 RID: 7810 RVA: 0x0010A654 File Offset: 0x00108A54
		public FactionBase()
		{
			this.trader = new FactionBase_TraderTracker(this);
		}

		// Token: 0x17000477 RID: 1143
		// (get) Token: 0x06001E83 RID: 7811 RVA: 0x0010A66C File Offset: 0x00108A6C
		// (set) Token: 0x06001E84 RID: 7812 RVA: 0x0010A687 File Offset: 0x00108A87
		public string Name
		{
			get
			{
				return this.nameInt;
			}
			set
			{
				this.nameInt = value;
			}
		}

		// Token: 0x17000478 RID: 1144
		// (get) Token: 0x06001E85 RID: 7813 RVA: 0x0010A694 File Offset: 0x00108A94
		public override Texture2D ExpandingIcon
		{
			get
			{
				return base.Faction.def.ExpandingIconTexture;
			}
		}

		// Token: 0x17000479 RID: 1145
		// (get) Token: 0x06001E86 RID: 7814 RVA: 0x0010A6BC File Offset: 0x00108ABC
		public override string Label
		{
			get
			{
				return (this.nameInt == null) ? base.Label : this.nameInt;
			}
		}

		// Token: 0x1700047A RID: 1146
		// (get) Token: 0x06001E87 RID: 7815 RVA: 0x0010A6F0 File Offset: 0x00108AF0
		public override Material Material
		{
			get
			{
				if (this.cachedMat == null)
				{
					this.cachedMat = MaterialPool.MatFrom(base.Faction.def.homeIconPath, ShaderDatabase.WorldOverlayTransparentLit, base.Faction.Color, WorldMaterials.WorldObjectRenderQueue);
				}
				return this.cachedMat;
			}
		}

		// Token: 0x1700047B RID: 1147
		// (get) Token: 0x06001E88 RID: 7816 RVA: 0x0010A750 File Offset: 0x00108B50
		public override MapGeneratorDef MapGeneratorDef
		{
			get
			{
				MapGeneratorDef result;
				if (base.Faction == Faction.OfPlayer)
				{
					result = MapGeneratorDefOf.Base_Player;
				}
				else
				{
					result = MapGeneratorDefOf.Base_Faction;
				}
				return result;
			}
		}

		// Token: 0x06001E89 RID: 7817 RVA: 0x0010A788 File Offset: 0x00108B88
		public override IEnumerable<IncidentTargetTypeDef> AcceptedTypes()
		{
			foreach (IncidentTargetTypeDef type in this.<AcceptedTypes>__BaseCallProxy0())
			{
				yield return type;
			}
			if (base.Faction == Faction.OfPlayer)
			{
				yield return IncidentTargetTypeDefOf.Map_PlayerHome;
			}
			else
			{
				yield return IncidentTargetTypeDefOf.Map_Misc;
			}
			yield break;
		}

		// Token: 0x06001E8A RID: 7818 RVA: 0x0010A7B2 File Offset: 0x00108BB2
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.nameInt, "nameInt", null, false);
			Scribe_Values.Look<bool>(ref this.namedByPlayer, "namedByPlayer", false, false);
		}

		// Token: 0x06001E8B RID: 7819 RVA: 0x0010A7DF File Offset: 0x00108BDF
		public override void Tick()
		{
			base.Tick();
			FactionBaseDefeatUtility.CheckDefeated(this);
		}

		// Token: 0x04001217 RID: 4631
		private string nameInt;

		// Token: 0x04001218 RID: 4632
		public bool namedByPlayer;

		// Token: 0x04001219 RID: 4633
		private Material cachedMat;
	}
}
