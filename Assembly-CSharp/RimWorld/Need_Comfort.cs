using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Need_Comfort : Need_Seeker
	{
		public float lastComfortUsed;

		public int lastComfortUseTick;

		private const float MinNormal = 0.1f;

		private const float MinComfortable = 0.6f;

		private const float MinVeryComfortable = 0.7f;

		private const float MinExtremelyComfortablee = 0.8f;

		private const float MinLuxuriantlyComfortable = 0.9f;

		public const int ComfortUseInterval = 10;

		public Need_Comfort(Pawn pawn) : base(pawn)
		{
			this.threshPercents = new List<float>();
			this.threshPercents.Add(0.1f);
			this.threshPercents.Add(0.6f);
			this.threshPercents.Add(0.7f);
			this.threshPercents.Add(0.8f);
			this.threshPercents.Add(0.9f);
		}

		public override float CurInstantLevel
		{
			get
			{
				float result;
				if (!this.pawn.Spawned)
				{
					result = 0.5f;
				}
				else if (this.lastComfortUseTick > Find.TickManager.TicksGame - 10)
				{
					result = Mathf.Clamp01(this.lastComfortUsed);
				}
				else
				{
					result = 0f;
				}
				return result;
			}
		}

		public ComfortCategory CurCategory
		{
			get
			{
				ComfortCategory result;
				if (this.CurLevel < 0.1f)
				{
					result = ComfortCategory.Uncomfortable;
				}
				else if (this.CurLevel < 0.6f)
				{
					result = ComfortCategory.Normal;
				}
				else if (this.CurLevel < 0.7f)
				{
					result = ComfortCategory.Comfortable;
				}
				else if (this.CurLevel < 0.8f)
				{
					result = ComfortCategory.VeryComfortable;
				}
				else if (this.CurLevel < 0.9f)
				{
					result = ComfortCategory.ExtremelyComfortable;
				}
				else
				{
					result = ComfortCategory.LuxuriantlyComfortable;
				}
				return result;
			}
		}

		public void ComfortUsed(float comfort)
		{
			this.lastComfortUsed = comfort;
			this.lastComfortUseTick = Find.TickManager.TicksGame;
		}
	}
}
