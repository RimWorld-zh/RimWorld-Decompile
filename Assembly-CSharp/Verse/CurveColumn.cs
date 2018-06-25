using System;

namespace Verse
{
	public struct CurveColumn
	{
		public float x;

		public SimpleCurve y;

		public CurveColumn(float x, SimpleCurve y)
		{
			this.x = x;
			this.y = y;
		}
	}
}
