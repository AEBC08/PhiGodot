using Godot;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

public partial class Start_Scene_Music : AudioStreamPlayer
{
	public List<string> Music_Names;  // 读取后的Json列表
	public int Music_Number = 0;  // 记录播放了几首歌

	public void Play_Music()  // 播放函数
	{
		if (Music_Number == Music_Names.Count)  // 检测是否播放到头了，如果是，从头开始播放
		{
			Music_Number = 0;
		}
		Stream = (AudioStream)GD.Load($"res://Assets/Audio/Background/{Music_Names[Music_Number]}");  // 设置播放音频
		Play();  // 播放
		Music_Number++;  // 记录播放次数
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// 读取Json
		Music_Names = JsonConvert.DeserializeObject<List<string>>(FileAccess.Open("res://Assets/Audio/Background/Start_Game.json", FileAccess.ModeFlags.Read).GetAsText());
		Play_Music();
		Finished += Play_Music;  // 连接信号
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
