using Godot;
using System;

public partial class Start_Game : TextureButton
{
	public void Start()
	{
		GetTree().ChangeSceneToFile("res://Assets/Scene/Game.tscn");  // 跳转场景
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Pressed += Start;  // 连接信号
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
