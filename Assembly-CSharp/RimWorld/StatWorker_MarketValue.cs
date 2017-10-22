using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class StatWorker_MarketValue : StatWorker
	{
		public const float ValuePerWork = 0.0036f;

		private const float DefaultGuessStuffCost = 2f;

		public override float GetValueUnfinalized(StatRequest req, bool applyPostProcess = true)
		{
			float result;
			if (req.HasThing && req.Thing is Pawn)
			{
				result = base.GetValueUnfinalized(StatRequest.For(req.Def, req.StuffDef, QualityCategory.Normal), applyPostProcess) * PriceUtility.PawnQualityPriceFactor((Pawn)req.Thing);
			}
			else if (req.Def.statBases.StatListContains(StatDefOf.MarketValue))
			{
				result = base.GetValueUnfinalized(req, true);
			}
			else
			{
				float num = 0f;
				if (req.Def.costList != null)
				{
					for (int i = 0; i < req.Def.costList.Count; i++)
					{
						num += (float)req.Def.costList[i].count * req.Def.costList[i].thingDef.BaseMarketValue;
					}
				}
				if (req.Def.costStuffCount > 0)
				{
					num = (float)((req.StuffDef == null) ? (num + (float)req.Def.costStuffCount * 2.0) : (num + (float)req.Def.costStuffCount / req.StuffDef.VolumePerUnit * req.StuffDef.GetStatValueAbstract(StatDefOf.MarketValue, null)));
				}
				float num2 = Mathf.Max(req.Def.GetStatValueAbstract(StatDefOf.WorkToMake, req.StuffDef), req.Def.GetStatValueAbstract(StatDefOf.WorkToBuild, req.StuffDef));
				num = (result = (float)(num + num2 * 0.003599999938160181));
			}
			return result;
		}

		public override string GetExplanationUnfinalized(StatRequest req, ToStringNumberSense numberSense)
		{
			string result;
			if (req.HasThing && req.Thing is Pawn)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(base.GetExplanationUnfinalized(req, numberSense));
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
				Pawn pawn = req.Thing as Pawn;
				stringBuilder.Append("StatsReport_CharacterQuality".Translate() + ": x" + PriceUtility.PawnQualityPriceFactor(pawn).ToStringPercent());
				result = stringBuilder.ToString();
			}
			else if (req.Def.statBases.StatListContains(StatDefOf.MarketValue))
			{
				result = base.GetExplanationUnfinalized(req, numberSense);
			}
			else
			{
				StringBuilder stringBuilder2 = new StringBuilder();
				stringBuilder2.AppendLine("StatsReport_MarketValueFromStuffsAndWork".Translate());
				result = stringBuilder2.ToString();
			}
			return result;
		}

		public override bool ShouldShowFor(BuildableDef def)
		{
			ThingDef thingDef = def as ThingDef;
			return thingDef != null && (TradeUtility.EverTradeable(thingDef) || thingDef.category == ThingCategory.Building);
		}
	}
}
