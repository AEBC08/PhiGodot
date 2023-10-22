using Godot;
using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading;

// 读谱类
public class Note
{
	public int type { get; set; }
	public int time { get; set; }
	public float positionX { get; set; }
	public float holdTime { get; set; }
	public float speed { get; set; }
	public float floorPosition { get; set; }
}
public class SpeedEvent
{
	public float startTime { get; set; }
	public float endTime { get; set; }
	public float value { get; set; }
}
public class JudgeLineMoveEvent
{
	public float startTime { get; set; }
	public float endTime { get; set; }
	public float start { get; set; }
	public float end { get; set; }
	public float start2 { get; set; }
	public float end2 { get; set; }
}
public class JudgeLineRotateEvent
{
	public float startTime { get; set; }
	public float endTime { get; set; }
	public float start { get; set; }
	public float end { get; set; }
}
public class JudgeLineDisappearEvent
{
	public float startTime { get; set; }
	public float endTime { get; set; }
	public float start { get; set; }
	public float end { get; set; }
}
public class JudgeLineList
{
	public float bpm { get; set; }
	public required List<Note> notesAbove { get; set; }
	public required List<object> notesBelow { get; set; }
	public required List<SpeedEvent> speedEvents { get; set; }
	public required List<JudgeLineMoveEvent> judgeLineMoveEvents { get; set; }
	public required List<JudgeLineRotateEvent> judgeLineRotateEvents { get; set; }
}
public class Root
{
	public int formatVersion { get; set; }
	public float offset { get; set; }
	public required List<JudgeLineList> judgeLineList { get; set; }
}
// 主程序类
public partial class Game_Logic : Node
{
	// 声明变量
	public List<Dictionary<string, object>> Event_List = new List<Dictionary<string, object>>();// 官谱重定义
	public Label Points_Info;
	public int Points = 0;
	public Label Mode_Info;
	public int Game_Mode = 0;
	public Label Combo_Info;
	public int Combo;
	public int Time;

	// 生成函数
	public void Load_Node(int Type, float X = 0, float Y = 0, float Speed = 0, float Long = 0, string Strings = "", int Mode = 0)  // 生成游戏对象函数
	{
		// X.Max = 1152 
		X += 576;
		switch (Type)
		{
			case 0:  // 设置背景图
				{
					GetNode<TextureRect>("Backgroun").Texture = (Texture2D)GD.Load<Texture>(Strings);
					break;
				}
			case 1:  // 生成Tap
				{
					Node2D Node_Info = (Node2D)((PackedScene)ResourceLoader.Load("res://Assets/Scene/Tap.tscn")).Instantiate();
					Node_Info.Position = new Vector2(X, Y - (Node_Info.Scale.Y * 53.34f));
					var Set_Speed = (Tap)Node_Info;
					Set_Speed.Speed = Speed;
					GetNode("Backgroun/Game").CallDeferred("add_child", Node_Info);
					break;
				}
			case 2:  // 生成Drag
				{
					Node2D Node_Info = (Node2D)((PackedScene)ResourceLoader.Load("res://Assets/Scene/Drag.tscn")).Instantiate();
					Node_Info.Position = new Vector2(X, Y - (Node_Info.Scale.Y * 33.34f));
					var Set_Speed = (Drag)Node_Info;
					Set_Speed.Speed = Speed;
					GetNode("Backgroun/Game").CallDeferred("add_child", Node_Info);
					break;
				}
			case 3:  // 生成Flick
				{
					Node2D Node_Info = (Node2D)((PackedScene)ResourceLoader.Load("res://Assets/Scene/Flick.tscn")).Instantiate();
					Node_Info.Position = new Vector2(X, Y - (Node_Info.Scale.Y * 53.34f));
					var Set_Speed = (Flick)Node_Info;
					Set_Speed.Speed = Speed;
					GetNode("Backgroun/Game").CallDeferred("add_child", Node_Info);
					break;
				}
			case 4:  // 生成Hold
				{
					Node2D Node_Info = (Node2D)((PackedScene)ResourceLoader.Load("res://Assets/Scene/Hold.tscn")).Instantiate();
					Node_Info.Scale = new Vector2(0.15f, Long);
					Node_Info.Position = new Vector2(X, Y - (Long * 1000));
					GetNode("Backgroun/Game").CallDeferred("add_child", Node_Info);
					break;
				}
			case 5:  // 生成判定线
				{
					Node2D Node_Info = (Node2D)((PackedScene)ResourceLoader.Load("res://Assets/Scene/Determine_Line.tscn")).Instantiate();
					Node_Info.Position = new Vector2(X, Y);
					GetNode("Backgroun/Game").CallDeferred("add_child", Node_Info);
					break;
				}
			case 6:  // 加载歌曲信息
				{
					GetNode<Label>("Backgroun/UI/UI_Down/UI_Down/UI_Left/UI_Down/Song").Text = Strings;
					break;
				}
			case 7:  // 加载难度信息
				{
					GetNode<Label>("Backgroun/UI/UI_Down/UI_Down/UI_Right/UI_Down/Difficulty").Text = Strings;
					break;
				}
			case 8: // 加载模式
				{
					if (Mode == 0)  // 正常模式
					{
						Game_Mode = Mode;
					}
					else if (Mode == 1)  // 自动模式
					{
						Game_Mode = Mode;
					}
					break;
				}
			case 9:  // 加载音乐
				{
					GetNode<AudioStreamPlayer>("Backgroun/Game/Music").Stream = (AudioStream)GD.Load(Strings);
					break;
				}
		}
	}
	public void Loading_Json(string Json_File)
	{
		string json = File.ReadAllText(Json_File);
		Root root = JsonConvert.DeserializeObject<Root>(json);
		Event_List.Add(new Dictionary<string, object> { { "type", 0 }, { "formatVersion", root.formatVersion }, { "offset", root.offset } });
		foreach (var judgeLine in root.judgeLineList)  // 遍历
		{
			Event_List.Add(new Dictionary<string, object> { { "type", 1 }, { "bpm", judgeLine.bpm } });
			foreach (var note in judgeLine.notesAbove)  // 遍历Note
			{
				if (note.type == 1)  // Tap
				{
					Event_List.Add(new Dictionary<string, object> { { "type", 2 }, { "note", 1 }, { "time", note.time }, { "positionX", note.positionX }, { "holdTime", note.holdTime }, { "speed", note.speed }, { "floorPosition", note.floorPosition } });
				}
				else if (note.type == 2)  // Drag
				{
					Event_List.Add(new Dictionary<string, object> { { "type", 2 }, { "note", 2 }, { "time", note.time }, { "positionX", note.positionX }, { "holdTime", note.holdTime }, { "speed", note.speed }, { "floorPosition", note.floorPosition } });
				}
				else if (note.type == 3)  // Hold
				{
					Event_List.Add(new Dictionary<string, object> { { "type", 2 }, { "note", 4 }, { "time", note.time }, { "positionX", note.positionX }, { "holdTime", note.holdTime }, { "speed", note.speed }, { "floorPosition", note.floorPosition } });
				}
				else  // Flick
				{
					Event_List.Add(new Dictionary<string, object> { { "type", 2 }, { "note", 3 }, { "time", note.time }, { "positionX", note.positionX }, { "holdTime", note.holdTime }, { "speed", note.speed }, { "floorPosition", note.floorPosition } });
				}
			}
			foreach (var speedEvent in judgeLine.speedEvents)  // 遍历判定线速度
			{
				Event_List.Add(new Dictionary<string, object> { { "type", 3 }, { "startTime", speedEvent.startTime }, { "endTime", speedEvent.endTime }, { "value", speedEvent.value } });
			}
			foreach (var moveEvent in judgeLine.judgeLineMoveEvents)  // 遍历判定线移动
			{
				Event_List.Add(new Dictionary<string, object> { { "type", 4 }, { "startTime", moveEvent.startTime }, { "endTime", moveEvent.endTime }, { "start", moveEvent.start }, { "end", moveEvent.end }, { "start2", moveEvent.start2 }, { "end2", moveEvent.end2 } });
			}
			foreach (var rotateEvent in judgeLine.judgeLineRotateEvents)  // 遍历判定线旋转
			{
				Event_List.Add(new Dictionary<string, object> { { "type", 5 }, { "startTime", rotateEvent.startTime }, { "endTime", rotateEvent.endTime }, { "start", rotateEvent.start }, { "end", rotateEvent.end } });
			}
		}
		foreach (var kvp in Event_List)  // Debug
		{
			GD.Print("Key: " + kvp);
			foreach (KeyValuePair<string, object> obj in kvp)
			{
				GD.Print("Value: " + obj.Value);
			}
		}
	}
	public void Timer()
	{
		while (true)
		{
			Time++;
			Thread.Sleep(10);
		}
	}
	public void Load_Note()
	{
		foreach (Dictionary<string, object> Note_Info in Event_List)
		{
			GD.Print(Time);
			GD.Print(Note_Info["time"]);
			GD.Print((int)Note_Info["type"]);
			if ((int)Note_Info["type"] == 2)
			{
				GD.Print(Time);
				if ((int)Note_Info["time"] == Time)
				{
					GD.Print(Time);
					GD.Print((int)Note_Info["note"], (float)Note_Info["positionX"], 0, (float)Note_Info["speed"]);
					Load_Node((int)Note_Info["note"], (float)Note_Info["positionX"], 0, (float)Note_Info["speed"]);
				}
				else
				{
					while ((int)Note_Info["time"] != Time)
					{
						Thread.Sleep(100);
					}
					Load_Node((int)Note_Info["note"], (float)Note_Info["positionX"], 0, (float)Note_Info["speed"]);
				}
			}
		}
	}
	public void Load_Event()
	{
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Points_Info = GetNode<Label>("Backgroun/UI/UI_Up/UI_Up/UI_Right/UI_Up/Points");
		Mode_Info = GetNode<Label>("Backgroun/UI/UI_Up/UI_Up/UI_Middle/UI_Up/Mode");
		Combo_Info = GetNode<Label>("Backgroun/UI/UI_Up/UI_Up/UI_Middle/UI_Up/Combo");
		Load_Node(0, Strings: "res://Assets/Image/Song_Cover/Test.png");
		Load_Node(6, Strings: "测试");
		Load_Node(7, Strings: "EZ Lv.-999");
		Load_Node(8, Mode: 1);
		Load_Node(5, 0, 600);
		Loading_Json(@"F:\bei_fen\QaQ\phigros\1\TextAsset\0.json");
		// new Thread(new ThreadStart(Timer)).Start();
		// new Thread(new ThreadStart(Load_Note)).Start();
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Combo >= 3)  // 显示连击数
		{
			if (Game_Mode == 0)  // 显示COMBO
			{
				Mode_Info.Text = "COMBO";
			}
			else if (Game_Mode == 1)  // 显示Auto Play
			{
				Mode_Info.Text = "Auto Play";
			}
			Combo_Info.Text = Combo.ToString();
		}
		else  // 隐藏连击数
		{
			Mode_Info.Text = "";
			Combo_Info.Text = "";
		}
		Points_Info.Text = Points.ToString("D7");  // 显示分数
	}
}
