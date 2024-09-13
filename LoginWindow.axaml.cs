using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using KalugaTradeApp;
using Microsoft.EntityFrameworkCore;
using TradeApp;
using TradeApp.Entities;

namespace TradeApp;

public partial class LoginWindow : Window
{
// Переменная флаг, меняет свое значение, 
        // если пользователь не смог ввести с первого раза пароль и логин
        // если b == true, то отобразим капчу
        bool b = false; 
        // таймер
        DispatcherTimer timer = new DispatcherTimer();
        int seconds = 0; // секунды
        string captcha = ""; // текст капчи
     public List<User> Users {get; set;}

    public LoginWindow()
    {
        InitializeComponent();
        LoadAndInitData();
    }

    /// <summary>
        /// Загружает и инициализирует данные
        /// </summary>
        void LoadAndInitData()
        {
            App.StartWindow = this;
            var ImageCaptcha = this.FindControl<Image>("ImageCaptcha");
            var StackPanelCaptcha = this.FindControl<StackPanel>("StackPanelCaptcha");
            this.Height = 200; // задаем высоту окна
            timer.Tick += timer_Tick; // к событию Tick таймера привязываем обработчик события
            // скрываем строки с капчой
            ImageCaptcha.Height = 0;
            StackPanelCaptcha.IsVisible = false;
            TbLogin.Text = "loginDEpxl2018";
            TbPass.Text = "P6h4Jq";
        }

          // обаботчик события срабатывает через каждые т секунд
        void timer_Tick(object sender, EventArgs e)
        {
            seconds -= 1;
            var TextBlockTime = this.FindControl<TextBlock>("TextBlockTime");
            var BtnOk = this.FindControl<Button>("BtnOk");
            var BtnCancel = this.FindControl<Button>("BtnCancel");
            TextBlockTime.Text = $"Осталось {seconds} секунд до разблокировки";
            if (seconds == 0) // оставливаем таймер, разблокировываем кнопку
            {
                BtnOk.IsEnabled = true;
                BtnCancel.IsEnabled = true;
                TextBlockTime.Text = "";
                timer.Stop();
            }
            
        }

     private async void BtnOkClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
       
        try
            {  //загрузка всех пользователей из БД в список
                TradeContext context = new TradeContext();;
                User user = context.Users.FirstOrDefault(p => p.Password == TbPass.Text && p.Username == TbLogin.Text);
                // удачный вход после ввода пароля и логина или пароля, логина и капчи
                var TbCaptcha = this.FindControl<TextBox>("TbCaptcha");
                if ((user != null && !b) || (user != null && b && TbCaptcha.Text == captcha))
                {
                    App.CurrentUser = user;
                    App.CurrentUser.Role = context.Roles.FirstOrDefault(x=> x.Id == user.RoleId);
                    MainWindow main = new MainWindow();
                    main.Show();
                    this.Hide();
                   // this.IsVisible = false;
                    
                }
                else
                {
                    MessageWindow messageWindow = new MessageWindow("Ошибка", "Не верный логин или пароль!");
                    await messageWindow.ShowDialog(App.StartWindow);
                    // меняем высоту формы
                    this.Height = 350;
                    // вызываем метод отображения капчи
                    ShowCaptcha();

                    if (b) // если неправильно ввели логин, пароль и капчу 
                    {
                        // задаем параметры таймера, событие Tick срабатывает через каждую секунду
                        timer.Interval = TimeSpan.FromSeconds(1);
                        // блокируем кнопку
                        BtnOk.IsEnabled = false;
                        BtnCancel.IsEnabled = false;
                        // на  10 секунд
                        seconds = 10;
                        var TextBlockTime = this.FindControl<TextBlock>("TextBlockTime");
                        // отображает сколько секунд осталось до разблокировки
                        TextBlockTime.Text = $"Осталось {seconds} секунд до разблокировки";
                        // включаем таймер
                        timer.Start(); 
                    }
                    b = true; // становится истинной, если неправильно ввели логин, пароль и капчу 
                    var ImageCaptcha = this.FindControl<Image>("ImageCaptcha");
                    var StackPanelCaptcha = this.FindControl<StackPanel>("StackPanelCaptcha");
                    ImageCaptcha.Height = 100;
                   StackPanelCaptcha.IsVisible = true;
                }
            }
            catch (Exception ex)
            {
                MessageWindow messageWindow = new MessageWindow("Ошибка", ex.ToString());
                    await messageWindow.ShowDialog(App.StartWindow);
            }


    }
     private void BtnCancelClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        this.Close();    }


    private void BtnGuestModeClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        App.CurrentUser = null;
        MainWindow main = new MainWindow();
        main.Show();
        this.Hide();
    }
     private void BtnRenewCaptcha_Click(object sender,  Avalonia.Interactivity.RoutedEventArgs e)
        {
            // кнопка обновить капчу
            ShowCaptcha();
        }
        /// <summary>
        /// отображает капчу
        /// </summary>
        void ShowCaptcha()
        {
            // из класса MakeCaptcha вызываем метод CreateImage
            var tuple = MakeCaptcha.CreateImage(300, 100, 5);
            // получаем ImageSource
            var ImageCaptcha = this.FindControl<Image>("ImageCaptcha");
            ImageCaptcha.Source = tuple.image;
            // получаем текст капчи
            captcha = tuple.captcha;
        }
            
private void Window_Activated(object? senders,  EventArgs e)
    {
        var ImageCaptcha = this.FindControl<Image>("ImageCaptcha");
        var StackPanelCaptcha = this.FindControl<StackPanel>("StackPanelCaptcha");
        ImageCaptcha.Height = 0;
        StackPanelCaptcha.IsVisible = false;
        this.Height = 180;
        var TbLogin = this.FindControl<TextBox>("TbLogin");
        var TbPass = this.FindControl<TextBox>("TbPass");
        TbPass.Text = "";
        TbLogin.Text = "";

    }
}