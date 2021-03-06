﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace MedERP
{
    /// <summary>
    /// Логика взаимодействия для LoginForm.xaml
    /// </summary>
    public partial class LoginForm : Window
    {
        private Timer logonTimer;
        public LoginForm()
        {
            InitializeComponent();
        }

        private void frmLogin_Initialized(object sender, EventArgs e)
        {
            logonTimer = new Timer(GetTime, null, 0, 500);
            LoadUsers();
        }

        private void LoadUsers()
        {
            try
            {
                using (var CurentContext = new MedDBModelContainer())
                {
                    var users = CurentContext.UsersSet.ToList();
                    if (!users.Any())
                    {
                        var default_user = GetAdmin(CurentContext);
                        CurentContext.UsersSet.Add(default_user);
                        CurentContext.SaveChanges();
                        ExLog.Info("В настройках пользователей программы не обнаружено ни одного пользователя. Создан пользователь по умолчанию `Администратор`");
                    }
                    else
                    {
                        tbLogin.ItemsSource = CurentContext.UsersSet.ToList();
                        tbLogin.SelectedValue = "UserID";
                        tbLogin.DisplayMemberPath = "FirstName";

                    }
                }
            }
            catch (Exception ex)
            {
                ExLog.Error(ex.Message, ex);
                MessageBox.Show(
                    "Возникла ошибка при получении списка пользователей. Обратитесь к вашему системному администратору.", "Опс, ошибка!");
            }
        }

        private Users GetAdmin(MedDBModelContainer CurentContext)
        {
            return new Users()
            {
                IsDismissed = false,
                FirstName = "Администратор",
                LastName = "Администратор",
                MiddleName = "Администратор",
                PhoneNumber = "+71231231212",
                Role = GetRole("Администратор", CurentContext),
                Password = ""
            };
        }

        private Role GetRole(string rolename, MedDBModelContainer CurentContext)
        {
            try
            {
                var Roles = CurentContext.RoleSet.ToList();
                if (Roles.Any())
                {
                    return (CurentContext.RoleSet.Single(x => x.RoleName.Equals(rolename)));
                }
                else
                {
                    var DefaultRole = new Role()
                    {
                        RoleName = "Администратор",
                        AllowAddUsers = true,
                        AllowDeleteClients = true,
                        AllowDeleteVisits = true
                    };
                    CurentContext.RoleSet.Add(DefaultRole);
                    CurentContext.SaveChanges();
                    return DefaultRole;
                }
            }
            catch (Exception ex)
            {
                ExLog.Error(ex.Message, ex);
                MessageBox.Show("Ошибка получения ролей.", "Опс, ошибка!");
            }
            return new Role();
        }

        private void GetTime(object state)
        {
            var CurentDateTime = DateTime.Now;

            this.Dispatcher.Invoke((Action)(() =>
            {
                tbTime.Text = CurentDateTime.ToString("HH:mm");
                tbSec.Content = CurentDateTime.Second.ToString("00");
                tbDate.Text = CurentDateTime.Day + " " + CurentDateTime.ToString("MMMM") + ", " +
                         CurentDateTime.ToString("dddd");
            }));
        }

        public static string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }

        public static bool VerifyHashedPassword(string hashedPassword, string password)
        {
            if (password.Equals("") && hashedPassword.Equals("")) return true;
            byte[] buffer4;
            byte[] src = Convert.FromBase64String(hashedPassword);
            if ((src.Length != 0x31) || (src[0] != 0))
            {
                return false;
            }
            byte[] dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            byte[] buffer3 = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8))
            {
                buffer4 = bytes.GetBytes(0x20);
            }
            return ByteArraysEqual(buffer3, buffer4);
        }

        private static bool ByteArraysEqual(byte[] buffer3, byte[] buffer4)
        {
            bool areEqual = buffer3.SequenceEqual(buffer4);

            return areEqual;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var CurentContext = new MedDBModelContainer())
                {
                    if(tbLogin.Text == "") return;
                    var CurentUser = CurentContext.UsersSet.Single(x => x.FirstName.Equals(tbLogin.Text));
                    if (VerifyHashedPassword(CurentUser.Password, tbPassword.Password))
                    {
                        //АВТОРИЗАЦИЯ УСПЕШНА
                        this.Hide();
                        var ManagerForm = new ManagerForm();
                        ManagerForm.Show();
                    }
                    else MessageBox.Show("Неверный пароль, попробуйте пожалуйста еще раз.", "Ошибка авторизации");
                }
            }
            catch (Exception ex)
            {
                ExLog.Error(ex.Message, ex);
                MessageBox.Show("Произошла ошибка авторизации пользователя. Обратитесь к вашему системному администратору.", "Опс, ошибка!");
            }
        }
    }
}
