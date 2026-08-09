using Godot;

public partial class PlayerController : CharacterBody3D
{
	[Export]
	public float MoveSpeed { get; set; } = 5.0f;

	[Export]
	public float JumpVelocity { get; set; } = 4.5f;

	[Export]
	public float RespawnHeight { get; set; } = -10.0f;

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

		Vector3 direction = new Vector3(
			input.X,
			0.0f,
			input.Y
		).Normalized();

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
