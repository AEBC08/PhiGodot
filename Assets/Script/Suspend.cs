using Godot;
using System.Threading;

public partial class Suspend : TextureButton
{
    private int Click = 0;  // 计时器缓存

    public void Timer()  // 计时器函数
    {
        GD.Print("计时器");
        Thread.Sleep(500);
        if (Click != 0)
        {
            GD.Print("清空");
            Click = 0;
        }
    }

    public void Suspend_Game()  // 暂停游戏函数
    {
        Click++;
        if (Click >= 2)
        {
            GD.Print("暂停");
            Click = 0;
        }
        else
        {
            new Thread(new ThreadStart(Timer)).Start();
        }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Pressed += Suspend_Game;  // 连接事件信号
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}
