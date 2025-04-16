using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace Floating_Clipboard
{
    public partial class MainWindow : Window
{
    private string saveFilePath = "notes.json"; // 保存文件路径

    public MainWindow()
    {
        InitializeComponent();
        LoadNotes(); // 启动时加载保存的便利贴

    }

    // 复制文本的方法
    private void CopyText(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Parent is StackPanel buttonPanel &&
            buttonPanel.Parent is StackPanel notePanel)
        {
            TextBox textBox = notePanel.Children[0] as TextBox;
            if (textBox != null)
            {
                Clipboard.SetText(textBox.Text);
            }
        }
    }

    // 删除文本框
    private void DeleteTextBox(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Parent is StackPanel buttonPanel &&
            buttonPanel.Parent is StackPanel notePanel)
        {
            NotesPanel.Children.Remove(notePanel);
        }
    }

    // 添加新文本框
    private void AddNewNote(string text = "")
    {
        StackPanel notePanel = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            Margin = new Thickness(0, 5, 0, 0)
        };

        TextBox newTextBox = new TextBox
        {
            Width = 240,
            Height = 100,
            AcceptsReturn = true,
            TextWrapping = TextWrapping.Wrap,
            VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
            Text = text // 载入时自动填充内容
        };

        StackPanel buttonPanel = new StackPanel
        {
            Orientation = Orientation.Vertical,
            Margin = new Thickness(10, 0, 0, 0)
        };

        Button copyButton = new Button
        {
            Content = "Copy",
            Width = 60,
            Height = 40
        };
        copyButton.Click += CopyText;

        Button deleteButton = new Button
        {
            Content = "Delete",
            Width = 60,
            Height = 20,
            Margin = new Thickness(0, 5, 0, 0)
        };
        deleteButton.Click += DeleteTextBox;

        buttonPanel.Children.Add(copyButton);
        buttonPanel.Children.Add(deleteButton);

        notePanel.Children.Add(newTextBox);
        notePanel.Children.Add(buttonPanel);

        NotesPanel.Children.Add(notePanel);
    }

    // 窗口关闭时自动保存
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        SaveNotes();
    }

    // 保存所有便利贴内容
    private void SaveNotes()
    {
        List<string> notes = new List<string>();

        foreach (var child in NotesPanel.Children)
        {
            if (child is StackPanel notePanel && notePanel.Children[0] is TextBox textBox)
            {
                if (textBox.Text != "") notes.Add(textBox.Text);
            }
        }

        string json = JsonSerializer.Serialize(notes);
        File.WriteAllText(saveFilePath, json);
    }

    // 读取并加载便利贴
    private void LoadNotes()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            List<string> notes = JsonSerializer.Deserialize<List<string>>(json);

            foreach (var note in notes)
            {
                if (note != "") AddNewNote(note);
            }
        }
    }

        // 按钮点击新增文本框
        private void AddNewNote(object sender, RoutedEventArgs e)
        {
            AddNewNote(""); // 默认添加空白便利贴
        }

    }
}