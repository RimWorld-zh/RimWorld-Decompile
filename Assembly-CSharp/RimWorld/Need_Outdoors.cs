using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Need_Outdoors : Need
	{
		private const float Delta_IndoorsThickRoof = -0.4f;

		private const float Delta_OutdoorsThickRoof = -0.4f;

		private const float Delta_IndoorsThinRoof = -0.3f;

		private const float Minimum_IndoorsThinRoof = 0.2f;

		private const float Delta_OutdoorsThinRoof = 0.7f;

		private const float Delta_IndoorsNoRoof = 2.5f;

		private const float Delta_OutdoorsNoRoof = 5f;

		private const float DeltaFactor_InBed = 0.25f;

		private float lastEffectiveDelta = 0f;

		public Need_Outdoors(Pawn pawn) : base(pawn)
		{
			this.threshPercents = new List<float>();
			this.threshPercents.Add(0.8f);
			this.threshPercents.Add(0.6f);
			this.threshPercents.Add(0.4f);
			this.threshPercents.Add(0.2f);
			this.threshPercents.Add(0.05f);
		}

		public override int GUIChangeArrow
		{
			get
			{
				return Math.Sign(this.lastEffectiveDelta);
			}
		}

		public OutdoorsCategory CurCategory
		{
			get
			{
				OutdoorsCategory result;
				if (this.CurLevel > 0.8f)
				{
					result = OutdoorsCategory.Free;
				}
				else if (this.CurLevel > 0.6f)
				{
					result = OutdoorsCategory.NeedFreshAir;
				}
				else if (this.CurLevel > 0.4f)
				{
					result = OutdoorsCategory.CabinFeverLight;
				}
				else if (this.CurLevel > 0.2f)
				{
					result = OutdoorsCategory.CabinFeverSevere;
				}
				else if (this.CurLevel > 0.05f)
				{
					result = OutdoorsCategory.Trapped;
				}
				else
				{
					result = OutdoorsCategory.Entombed;
				}
				return result;
			}
		}

		public override bool ShowOnNeedList
		{
			get
			{
				return !this.Disabled;
			}
		}

		private bool Disabled
		{
			get
			{
				return this.pawn.story.traits.HasTrait(TraitDefOf.Tunneler);
			}
		}

		public override void SetInitialLevel()
		{
			this.CurLevel = 1f;
		}

		public override void NeedInterval()
		{
			if (this.Disabled)
			{
				this.CurLevel = 1f;
			}
			else if (!base.IsFrozen)
			{
				float b = 0.2f;
				bool flag = !this.pawn.Spawned || this.pawn.Position.UsesOutdoorTemperature(this.pawn.Map);
				RoofDef roofDef = (!this.pawn.Spawned) ? null : this.pawn.Position.GetRoof(this.pawn.Map);
				float num;
				if (!flag)
				{
					if (roofDef == null)
					{
						num = 2.5f;
					}
					else if (!roofDef.isThickRoof)
					{
						num = -0.3f;
					}
					else
					{
						num = -0.4f;
						b = 0f;
					}
				}
				else if (roofDef == null)
				{
					num = 5f;
				}
				else if (roofDef.isThickRoof)
				{
					num = -0.4f;
				}
				else
				{
					num = 0.7f;
				}
				if (this.pawn.InBed() && num < 0f)
				{
					num *= 0.25f;
				}
				num *= 0.0025f;
				float curLevel = this.CurLevel;
				if (num < 0f)
				{
					this.CurLevel = Mathf.Min(this.CurLevel, Mathf.Max(this.CurLevel + num, b));
				}
				else
				{
					this.CurLevel = Mathf.Min(this.CurLevel + num, 1f);
				}
				this.lastEffectiveDelta = this.CurLevel - curLevel;
			}
		}
	}
}
