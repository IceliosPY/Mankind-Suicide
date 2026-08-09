using Godot;

public partial class CompanionFollower : CharacterBody3D
{
	[Export]
	public PartyTrail Trail { get; set; }

	[Export]
	public float DistanceBehind { get; set; } = 2.0f;

	[Export]
	public float MoveSpeed { get; set; } = 4.8f;

	[Export]
	public float RotationSpeed { get; set; } = 8.0f;

	[Export]
	public float StopDistance { get; set; } = 0.25f;

	public override void _PhysicsProcess(double delta)
	{
		if (Trail == null)
			return;

		Vector3 targetPosition = Trail.GetPositionBehind(DistanceBehind);

		Vector3 direction = targetPosition - GlobalPosition;
		direction.Y = 0.0f;

		Vector3 velocity = Velocity;

		if (direction.Length() > StopDistance)
		{
			direction = direction.Normalized();

			velocity.X = direction.X * MoveSpeed;
			velocity.Z = direction.Z * MoveSpeed;

			float targetAngle = Mathf.Atan2(
				direction.X,
				direction.Z
			);

			Rotation = new Vector3(
				Rotation.X,
				Mathf.LerpAngle(
					Rotation.Y,
					targetAngle,
					RotationSpeed * (float)delta
				),
				Rotation.Z
			);
		}
		else
		{
			velocity.X = 0.0f;
			velocity.Z = 0.0f;
		}

		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}
		else
		{
			velocity.Y = 0.0f;
		}

		Velocity = velocity;

		MoveAndSlide();
	}
}
