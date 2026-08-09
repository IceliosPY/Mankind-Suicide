using Godot;

public partial class CameraFollow : Node3D
{
	[Export]
	public Node3D Target { get; set; }

	[Export]
	public float FollowSpeed { get; set; } = 8.0f;

	[Export]
	public float MouseSensitivity { get; set; } = 0.003f;

	[Export]
	public float MinPitchDegrees { get; set; } = -70.0f;

	[Export]
	public float MaxPitchDegrees { get; set; } = -20.0f;

	[Export]
	public Node3D PitchPivot { get; set; }

	private float _pitch;

	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Captured;

		if (PitchPivot != null)
		{
			_pitch = PitchPivot.Rotation.X;
		}
	}

	public override void _Process(double delta)
	{
		if (Target == null)
			return;

		GlobalPosition = GlobalPosition.Lerp(
			Target.GlobalPosition,
			FollowSpeed * (float)delta
		);
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_cancel"))
		{
			Input.MouseMode =
				Input.MouseMode == Input.MouseModeEnum.Captured
					? Input.MouseModeEnum.Visible
					: Input.MouseModeEnum.Captured;

			return;
		}

		if (@event is InputEventMouseMotion mouseMotion &&
			Input.MouseMode == Input.MouseModeEnum.Captured)
		{
			// Rotation horizontale autour du joueur
			RotateY(-mouseMotion.Relative.X * MouseSensitivity);

			// Rotation verticale
			if (PitchPivot != null)
			{
				_pitch -= mouseMotion.Relative.Y * MouseSensitivity;

				float minPitch = Mathf.DegToRad(MinPitchDegrees);
				float maxPitch = Mathf.DegToRad(MaxPitchDegrees);

				_pitch = Mathf.Clamp(
					_pitch,
					minPitch,
					maxPitch
				);

				Vector3 rotation = PitchPivot.Rotation;
				rotation.X = _pitch;
				PitchPivot.Rotation = rotation;
			}
		}
	}
}
