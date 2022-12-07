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

	public Bounds2 toBounds()
    {
		return new Bounds2(new Vector2(X.min, Y.min), new Vector2(XRange(), YRange()));

	}
}
