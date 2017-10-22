namespace Verse
{
	public abstract class Listing_Lines : Listing
	{
		public float lineHeight = 20f;

		protected void EndLine()
		{
			base.curY += this.lineHeight + base.verticalSpacing;
		}
	}
}
