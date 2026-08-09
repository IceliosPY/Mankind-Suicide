using Godot;

public partial class CameraFollow : Node3D
{
	[Export]
	public Node3D Target { get; set; }

	[Export]
	public float FollowSpeed { get; set; } = 6.0f;

	[Export]
	public Vector3 Offset { get; set; } = Vector3.Zero;

	public override void _Process(double delta)
	{
		if (Target == null)
			return;

		Vector3 desiredPosition = Target.GlobalPosition + Offset;

		GlobalPosition = GlobalPosition.Lerp(
			desiredPosition,
			FollowSpeed * (float)delta
		);
	}
}
