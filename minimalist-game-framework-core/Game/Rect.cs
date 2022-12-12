using System;

class Rect
{
	public Range X;
	public Range Y;

	public Rect(Range X, Range Y)
	{
		this.X = X;
		this.Y = Y;
	}

	public float XRange()
	{
		return X.max - X.min;
	}
	public float YRange()
	{
		return Y.max - Y.min;
	}

	public Bounds2 ToBounds()
    {
		return new Bounds2(new Vector2(X.min, Y.min), new Vector2(XRange(), YRange()));

	}
	static public Rect GetSpriteBounds(Vector2 position, Vector2 spriteSize)
	{ // a -1 makes the boundaries even
		Rect sBound = new Rect(new Range(position.X, position.X + spriteSize.X),
								   new Range(position.Y, position.Y + spriteSize.Y));

		// for debug use only - bounding boxes
		//Engine.DrawRectEmpty(sBound.ToBounds(), Color.Black);
		return sBound;
	}

	public static bool CheckRectIntersect(Rect rect, Rect playerBounds)
	{
		return Range.CheckIntervalIntersect(rect.X, playerBounds.X)
			 && Range.CheckIntervalIntersect(rect.Y, playerBounds.Y);
	}
}
