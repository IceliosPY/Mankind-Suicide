using Godot;
using System.Collections.Generic;

public partial class PartyTrail : Node
{
	[Export]
	public Node3D Leader { get; set; }

	[Export]
	public float PointSpacing { get; set; } = 0.35f;

	[Export]
	public int MaxPoints { get; set; } = 200;

	public List<Vector3> Points { get; } = new();

	public override void _Ready()
	{
		if (Leader != null)
		{
			Points.Add(Leader.GlobalPosition);
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (Leader == null)
			return;

		Vector3 currentPosition = Leader.GlobalPosition;

		if (Points.Count == 0)
		{
			Points.Add(currentPosition);
			return;
		}

		Vector3 lastPoint = Points[^1];

		if (currentPosition.DistanceTo(lastPoint) >= PointSpacing)
		{
			Points.Add(currentPosition);

			if (Points.Count > MaxPoints)
			{
				Points.RemoveAt(0);
			}
		}
	}

	public Vector3 GetPositionBehind(float distanceBehind)
	{
		if (Points.Count == 0)
			return Leader != null ? Leader.GlobalPosition : Vector3.Zero;

		float accumulatedDistance = 0.0f;

		for (int i = Points.Count - 1; i > 0; i--)
		{
			accumulatedDistance += Points[i].DistanceTo(Points[i - 1]);

			if (accumulatedDistance >= distanceBehind)
			{
				return Points[i - 1];
			}
		}

		return Points[0];
	}
}
