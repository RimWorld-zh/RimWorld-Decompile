using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005FB RID: 1531
	public class FactionBase : Settlement
	{
		// Token: 0x06001E7B RID: 7803 RVA: 0x0010A714 File Offset: 0x00108B14
		public FactionBase()
		{
			this.trader = new FactionBase_TraderTracker(this);
		}

		// Token: 0x17000477 RID: 1143
		// (get) Token: 0x06001E7C RID: 7804 RVA: 0x0010A72C File Offset: 0x00108B2C
		// (set) Token: 0x06001E7D RID: 7805 RVA: 0x0010A747 File Offset: 0x00108B47
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
		// (get) Token: 0x06001E7E RID: 7806 RVA: 0x0010A754 File Offset: 0x00108B54
		public override Texture2D ExpandingIcon
		{
			get
			{
				return base.Faction.def.ExpandingIconTexture;
			}
		}

		// Token: 0x17000479 RID: 1145
		// (get) Token: 0x06001E7F RID: 7807 RVA: 0x0010A77C File Offset: 0x00108B7C
		public override string Label
		{
			get
			{
				return (this.nameInt == null) ? base.Label : this.nameInt;
			}
		}

		// Token: 0x1700047A RID: 1146
		// (get) Token: 0x06001E80 RID: 7808 RVA: 0x0010A7B0 File Offset: 0x00108BB0
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
		// (get) Token: 0x06001E81 RID: 7809 RVA: 0x0010A810 File Offset: 0x00108C10
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

		// Token: 0x06001E82 RID: 7810 RVA: 0x0010A848 File Offset: 0x00108C48
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

		// Token: 0x06001E83 RID: 7811 RVA: 0x0010A872 File Offset: 0x00108C72
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.nameInt, "nameInt", null, false);
			Scribe_Values.Look<bool>(ref this.namedByPlayer, "namedByPlayer", false, false);
		}

		// Token: 0x06001E84 RID: 7812 RVA: 0x0010A89F File Offset: 0x00108C9F
		public override void Tick()
		{
			base.Tick();
			FactionBaseDefeatUtility.CheckDefeated(this);
		}

		// Token: 0x04001214 RID: 4628
		private string nameInt;

		// Token: 0x04001215 RID: 4629
		public bool namedByPlayer;

		// Token: 0x04001216 RID: 4630
		private Material cachedMat;
	}
}
