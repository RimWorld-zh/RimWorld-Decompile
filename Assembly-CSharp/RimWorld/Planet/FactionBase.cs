using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005FD RID: 1533
	public class FactionBase : Settlement
	{
		// Token: 0x04001218 RID: 4632
		private string nameInt;

		// Token: 0x04001219 RID: 4633
		public bool namedByPlayer;

		// Token: 0x0400121A RID: 4634
		private Material cachedMat;

		// Token: 0x06001E7E RID: 7806 RVA: 0x0010AACC File Offset: 0x00108ECC
		public FactionBase()
		{
			this.trader = new FactionBase_TraderTracker(this);
		}

		// Token: 0x17000477 RID: 1143
		// (get) Token: 0x06001E7F RID: 7807 RVA: 0x0010AAE4 File Offset: 0x00108EE4
		// (set) Token: 0x06001E80 RID: 7808 RVA: 0x0010AAFF File Offset: 0x00108EFF
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
		// (get) Token: 0x06001E81 RID: 7809 RVA: 0x0010AB0C File Offset: 0x00108F0C
		public override Texture2D ExpandingIcon
		{
			get
			{
				return base.Faction.def.ExpandingIconTexture;
			}
		}

		// Token: 0x17000479 RID: 1145
		// (get) Token: 0x06001E82 RID: 7810 RVA: 0x0010AB34 File Offset: 0x00108F34
		public override string Label
		{
			get
			{
				return (this.nameInt == null) ? base.Label : this.nameInt;
			}
		}

		// Token: 0x1700047A RID: 1146
		// (get) Token: 0x06001E83 RID: 7811 RVA: 0x0010AB68 File Offset: 0x00108F68
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
		// (get) Token: 0x06001E84 RID: 7812 RVA: 0x0010ABC8 File Offset: 0x00108FC8
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

		// Token: 0x06001E85 RID: 7813 RVA: 0x0010AC00 File Offset: 0x00109000
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

		// Token: 0x06001E86 RID: 7814 RVA: 0x0010AC2A File Offset: 0x0010902A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.nameInt, "nameInt", null, false);
			Scribe_Values.Look<bool>(ref this.namedByPlayer, "namedByPlayer", false, false);
		}

		// Token: 0x06001E87 RID: 7815 RVA: 0x0010AC57 File Offset: 0x00109057
		public override void Tick()
		{
			base.Tick();
			FactionBaseDefeatUtility.CheckDefeated(this);
		}
	}
}
