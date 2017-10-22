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

		public override float CurInstantLevel
		{
			get
			{
				return (float)(base.pawn.Spawned ? ((this.lastComfortUseTick <= Find.TickManager.TicksGame - 10) ? 0.0 : Mathf.Clamp01(this.lastComfortUsed)) : 0.5);
			}
		}

		public ComfortCategory CurCategory
		{
			get
			{
				return (ComfortCategory)((!(this.CurLevel < 0.10000000149011612)) ? ((this.CurLevel < 0.60000002384185791) ? 1 : ((!(this.CurLevel < 0.699999988079071)) ? ((!(this.CurLevel < 0.800000011920929)) ? ((!(this.CurLevel < 0.89999997615814209)) ? 5 : 4) : 3) : 2)) : 0);
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
