using Godot;

public partial class PlayerController : CharacterBody3D
{
	[Export]
	public float MoveSpeed { get; set; } = 5.0f;

	[Export]
	public float JumpVelocity { get; set; } = 4.5f;

	[Export]
	public float RespawnHeight { get; set; } = -10.0f;

	[Export]
	public float RotationSpeed { get; set; } = 10.0f;

	[Export]
	public Camera3D Camera { get; set; }

	private Vector3 _spawnPosition;

	public override void _Ready()
	{
		_spawnPosition = GlobalPosition;
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 input = Input.GetVector(
			"move_left",
			"move_right",
			"move_forward",
            "move_backward"
		);

		Vector3 direction = Vector3.Zero;

		if (Camera != null && input != Vector2.Zero)
		{
			Vector3 cameraForward = -Camera.GlobalTransform.Basis.Z;
			Vector3 cameraRight = Camera.GlobalTransform.Basis.X;

			cameraForward.Y = 0.0f;
			cameraRight.Y = 0.0f;

			cameraForward = cameraForward.Normalized();
			cameraRight = cameraRight.Normalized();

			direction =
				cameraRight * input.X +
				cameraForward * -input.Y;

			direction = direction.Normalized();
		}

		if (direction != Vector3.Zero)
		{
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

		Vector3 velocity = Velocity;

		velocity.X = direction.X * MoveSpeed;
		velocity.Z = direction.Z * MoveSpeed;

		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}
		else
		{
			velocity.Y = 0.0f;
		}

		if (Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		Velocity = velocity;

		MoveAndSlide();

		if (GlobalPosition.Y < RespawnHeight)
		{
			Respawn();
		}
	}

	private void Respawn()
	{
		GlobalPosition = _spawnPosition;
		Velocity = Vector3.Zero;
	}
}
