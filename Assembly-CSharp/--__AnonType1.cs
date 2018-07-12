using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

[CompilerGenerated]
internal sealed class <>__AnonType1<<Race>__T, <Hits>__T, <DiedDueToDamageThreshold>__T>
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly <Race>__T <Race>;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly <Hits>__T <Hits>;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly <DiedDueToDamageThreshold>__T <DiedDueToDamageThreshold>;

	[DebuggerHidden]
	public <>__AnonType1(<Race>__T Race, <Hits>__T Hits, <DiedDueToDamageThreshold>__T DiedDueToDamageThreshold)
	{
		this.<Race> = Race;
		this.<Hits> = Hits;
		this.<DiedDueToDamageThreshold> = DiedDueToDamageThreshold;
	}

	public <Race>__T Race
	{
		get
		{
			return this.<Race>;
		}
	}

	public <Hits>__T Hits
	{
		get
		{
			return this.<Hits>;
		}
	}

	public <DiedDueToDamageThreshold>__T DiedDueToDamageThreshold
	{
		get
		{
			return this.<DiedDueToDamageThreshold>;
		}
	}

	[DebuggerHidden]
	public override bool Equals(object obj)
	{
		var <>__AnonType = obj as <>__AnonType1<<Race>__T, <Hits>__T, <DiedDueToDamageThreshold>__T>;
		return <>__AnonType != null && (EqualityComparer<<Race>__T>.Default.Equals(this.<Race>, <>__AnonType.<Race>) && EqualityComparer<<Hits>__T>.Default.Equals(this.<Hits>, <>__AnonType.<Hits>)) && EqualityComparer<<DiedDueToDamageThreshold>__T>.Default.Equals(this.<DiedDueToDamageThreshold>, <>__AnonType.<DiedDueToDamageThreshold>);
	}

	[DebuggerHidden]
	public override int GetHashCode()
	{
		int num = (((-2128831035 ^ EqualityComparer<<Race>__T>.Default.GetHashCode(this.<Race>)) * 16777619 ^ EqualityComparer<<Hits>__T>.Default.GetHashCode(this.<Hits>)) * 16777619 ^ EqualityComparer<<DiedDueToDamageThreshold>__T>.Default.GetHashCode(this.<DiedDueToDamageThreshold>)) * 16777619;
		num += num << 13;
		num ^= num >> 7;
		num += num << 3;
		num ^= num >> 17;
		return num + (num << 5);
	}

	[DebuggerHidden]
	public override string ToString()
	{
		string[] array = new string[8];
		array[0] = "{";
		array[1] = " Race = ";
		int num = 2;
		string text;
		if (this.<Race> != null)
		{
			<Race>__T <Race>__T = this.<Race>;
			text = <Race>__T.ToString();
		}
		else
		{
			text = "";
		}
		array[num] = text;
		array[3] = ", Hits = ";
		int num2 = 4;
		string text2;
		if (this.<Hits> != null)
		{
			<Hits>__T <Hits>__T = this.<Hits>;
			text2 = <Hits>__T.ToString();
		}
		else
		{
			text2 = "";
		}
		array[num2] = text2;
		array[5] = ", DiedDueToDamageThreshold = ";
		int num3 = 6;
		string text3;
		if (this.<DiedDueToDamageThreshold> != null)
		{
			<DiedDueToDamageThreshold>__T <DiedDueToDamageThreshold>__T = this.<DiedDueToDamageThreshold>;
			text3 = <DiedDueToDamageThreshold>__T.ToString();
		}
		else
		{
			text3 = "";
		}
		array[num3] = text3;
		array[7] = " }";
		return string.Concat(array);
	}
}
