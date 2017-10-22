using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Need_Comfort : Need_Seeker
	{
		private const float MinNormal = 0.1f;

		private const float MinComfortable = 0.6f;

		private const float MinVeryComfortable = 0.7f;

		private const float MinExtremelyComfortablee = 0.8f;

		private const float MinLuxuriantlyComfortable = 0.9f;

		public const int ComfortUseInterval = 10;

		public float lastComfortUsed;

		public int lastComfortUseTick;

		public override float CurInstantLevel
		{
			get
			{
				if (!base.pawn.Spawned)
				{
					return 0.5f;
				}
				if (this.lastComfortUseTick > Find.TickManager.TicksGame - 10)
				{
					return Mathf.Clamp01(this.lastComfortUsed);
				}
				return 0f;
			}
		}

		public ComfortCategory CurCategory
		{
			get
			{
				if (this.CurLevel < 0.10000000149011612)
				{
					return ComfortCategory.Uncomfortable;
				}
				if (this.CurLevel < 0.60000002384185791)
				{
					return ComfortCategory.Normal;
				}
				if (this.CurLevel < 0.699999988079071)
				{
					return ComfortCategory.Comfortable;
				}
				if (this.CurLevel < 0.800000011920929)
				{
					return ComfortCategory.VeryComfortable;
				}
				if (this.CurLevel < 0.89999997615814209)
				{
					return ComfortCategory.ExtremelyComfortable;
				}
				return ComfortCategory.LuxuriantlyComfortable;
			}
		}

		public Need_Comfort(Pawn pawn) : base(pawn)
		{
			base.threshPercents = new List<float>();
			base.threshPercents.Add(0.1f);
			base.threshPercents.Add(0.6f);
			base.threshPercents.Add(0.7f);
			base.threshPercents.Add(0.8f);
			base.threshPercents.Add(0.9f);
		}

		public void ComfortUsed(float comfort)
		{
			this.lastComfortUsed = comfort;
			this.lastComfortUseTick = Find.TickManager.TicksGame;
		}
	}
}
