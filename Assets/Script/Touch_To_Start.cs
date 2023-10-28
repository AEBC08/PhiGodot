using Godot;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

public partial class Touch_To_Start : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// 读取Json
		List<string> Image_Names = JsonConvert.DeserializeObject<List<string>>(FileAccess.Open("res://Assets/Image/Background/Start_Game.json", FileAccess.ModeFlags.Read).GetAsText());
		// 随机设置Json中的图片
		GetNode<TextureRect>("Background").Texture = (Texture2D)GD.Load<Texture>($"res://Assets/Image/Background/{Image_Names[new Random().Next(Image_Names.Count)]}");
		// 读取Json
		Dictionary<string, string> Game_Infos = JsonConvert.DeserializeObject<Dictionary<string, string>>(FileAccess.Open("res://Assets/Bin/Game_Info.Json", FileAccess.ModeFlags.Read).GetAsText());
		// 显示版本号
		GetNode<Label>("Background/UI/UI_Down/UI_Down/UI_Left/UI_Down/Version").Text = $"Version: {Game_Infos["Game_Version"]}";
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
