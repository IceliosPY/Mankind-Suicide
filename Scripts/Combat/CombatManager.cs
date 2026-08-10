using Godot;

public partial class CombatManager : Node
{
	[Export]
	public Node3D CombatGrid { get; set; }

	[Export]
	public PlayerController Player { get; set; }

	[Export]
	public CompanionFollower Companion { get; set; }

	[Export]
	public PartyTrail PartyTrail { get; set; }

	public bool IsInCombat { get; private set; } = false;

	public override void _Ready()
	{
		SetCombatState(false);
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventKey keyEvent &&
			keyEvent.Pressed &&
			!keyEvent.Echo &&
			keyEvent.Keycode == Key.C)
		{
			if (IsInCombat)
				EndCombat();
			else
				StartCombat();
		}
	}

	public void StartCombat()
	{
		if (IsInCombat)
			return;

		IsInCombat = true;
		SetCombatState(true);

		GD.Print("Combat started");
	}

	public void EndCombat()
	{
		if (!IsInCombat)
			return;

		IsInCombat = false;
		SetCombatState(false);

		GD.Print("Combat ended");
	}

	private void SetCombatState(bool combat)
	{
		if (CombatGrid != null)
			CombatGrid.Visible = combat;

		if (Player != null)
			Player.SetPhysicsProcess(!combat);

		if (Companion != null)
			Companion.SetPhysicsProcess(!combat);

		if (PartyTrail != null)
			PartyTrail.SetPhysicsProcess(!combat);
	}
}
