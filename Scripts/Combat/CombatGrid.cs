using Godot;

public partial class CombatGrid : Node3D
{
	[Export]
	public int Width { get; set; } = 10;

	[Export]
	public int Height { get; set; } = 10;

	[Export]
	public float CellSize { get; set; } = 1.0f;

	[Export]
	public float CellThickness { get; set; } = 0.03f;

	[Export]
	public float GridYOffset { get; set; } = 0.12f;

	public override void _Ready()
	{
		GenerateGrid();
	}

	private void GenerateGrid()
	{
		// Nettoie une éventuelle ancienne grille
		foreach (Node child in GetChildren())
		{
			child.QueueFree();
		}

		float startX = -((Width - 1) * CellSize) / 2.0f;
		float startZ = -((Height - 1) * CellSize) / 2.0f;

		for (int x = 0; x < Width; x++)
		{
			for (int z = 0; z < Height; z++)
			{
				MeshInstance3D cell = new MeshInstance3D();

				BoxMesh mesh = new BoxMesh();
				mesh.Size = new Vector3(
					CellSize * 0.95f,
					CellThickness,
					CellSize * 0.95f
				);

				StandardMaterial3D material = new StandardMaterial3D();
				material.AlbedoColor = new Color(0.2f, 0.7f, 1.0f, 0.35f);
				material.Transparency = BaseMaterial3D.TransparencyEnum.Alpha;

				mesh.Material = material;
				cell.Mesh = mesh;

				cell.Position = new Vector3(
					startX + x * CellSize,
					GridYOffset,
					startZ + z * CellSize
				);

				AddChild(cell);
			}
		}
	}
}
