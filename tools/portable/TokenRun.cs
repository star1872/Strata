// TokenRun.exe - the double-click start of the Windows package (tools/package_portable.py builds it with the C#
// compiler every Windows has, .NET Framework 4's csc.exe).  It runs the model manager page, app\serve\setup_web.py,
// with the Python inside the package (python\python.exe): no Python, pip or git on the PC is needed.  The page opens
// in the browser; closing this window stops the page (a model started from it keeps running in its own window).
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

static class TokenRun
{
    static int Main(string[] args)
    {
        string root = AppDomain.CurrentDomain.BaseDirectory;
        string python = Path.Combine(root, "python", "python.exe");
        string page = Path.Combine(root, "app", "serve", "setup_web.py");
        Console.Title = "TokenRun";
        if (!File.Exists(python) || !File.Exists(page))
        {
            Console.WriteLine(" This TokenRun folder is incomplete: python\\python.exe or app\\ is missing.");
            Console.WriteLine(" Unzip the whole TokenRun-windows-x64.zip again (not only TokenRun.exe), then double-click TokenRun.exe.");
            Console.WriteLine(" 这个 TokenRun 文件夹不完整。请把 TokenRun-windows-x64.zip 整个解压，再双击 TokenRun.exe。");
            Pause();
            return 1;
        }
        var line = new StringBuilder(Quote(page));
        foreach (string a in args)
            line.Append(' ').Append(Quote(a));
        var start = new ProcessStartInfo(python, line.ToString());
        start.UseShellExecute = false;
        start.WorkingDirectory = Path.Combine(root, "app");
        using (Process p = Process.Start(start))
        {
            p.WaitForExit();
            if (p.ExitCode != 0)
                Pause();
            return p.ExitCode;
        }
    }

    static string Quote(string s)
    {
        return "\"" + s.Replace("\"", "\\\"") + "\"";
    }

    static void Pause()
    {
        Console.WriteLine();
        Console.WriteLine(" Press any key to close this window. / 按任意键关闭窗口。");
        try { Console.ReadKey(true); } catch (InvalidOperationException) { }
    }
}
