using Godot;
using System;

public partial class Drag : Sprite2D
{
	[Export] public Area2D Collision;
	[Export] public PackedScene Animation;
	public AudioStreamPlayer Effects;
	public float Speed;

	public void Determine(Area2D Area)  // 判定
	{
		if (Area.Name == "Determine_Line")  // 判断撞到判定线
		{
			// 增加连击
			var Get_Variable = (Game_Logic)GetParent().GetParent().GetParent();
			Get_Variable.Combo++;
			Effects.Play();  // 播放音效
							 // 播放动画
			var animation = Animation.Instantiate() as Node2D;
			GetParent().AddChild(animation);
			animation.GlobalPosition = GlobalPosition;
			// 清除
			QueueFree();
		}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Effects = GetParent().GetNode<AudioStreamPlayer>("Drag");  // 获取音效节点
		Collision.AreaEntered += Determine;  // 连接信号
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// 向下移动
		var Coordinate = GlobalPosition;
		Coordinate.Y += Speed;  // 速度
		GlobalPosition = Coordinate;
	}
}
