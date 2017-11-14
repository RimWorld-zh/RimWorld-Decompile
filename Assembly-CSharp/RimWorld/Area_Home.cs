using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Area_Home : Area
	{
		public override string Label
		{
			get
			{
				return "Home".Translate();
			}
		}

		public override Color Color
		{
			get
			{
				return new Color(0.3f, 0.3f, 0.9f);
			}
		}

		public override int ListPriority
		{
			get
			{
				return 10000;
			}
		}

		public Area_Home()
		{
		}

		public Area_Home(AreaManager areaManager)
			: base(areaManager)
		{
		}

		public override bool AssignableAsAllowed(AllowedAreaMode mode)
		{
			return (mode & AllowedAreaMode.Any) != (AllowedAreaMode)0;
		}

		public override string GetUniqueLoadID()
		{
			return "Area_" + base.ID + "_Home";
		}

		protected override void Set(IntVec3 c, bool val)
		{
			if (base[c] != val)
			{
				base.Set(c, val);
				base.Map.listerFilthInHomeArea.Notify_HomeAreaChanged(c);
			}
		}
	}
}
