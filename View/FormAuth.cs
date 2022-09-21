using OOOCooker_493_Lukyanov.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOOCooker_493_Lukyanov
{
    public partial class FormAuth : Form
    {
        public FormAuth()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Возврат
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonReturn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Загрузка формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormAuth_Load(object sender, EventArgs e)
        {
            Random rnd = new Random();

            tableLayoutPanelTop.BackColor = Color.FromArgb(255, 204, 153);
            tableLayoutPanelBottom.BackColor = Color.FromArgb(255, 204, 153);
            tableLayoutPanelMain.BackColor = Color.FromArgb(255, 255, 255);

            try
            {
                Helper.SqlConnection.Open();
            }
            catch (SqlException)
            {
                MessageBox.Show("Ошибка в подключении к БД", "Подключение к БД", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            MessageBox.Show("Связь с БД установлена", "Подключение к БД", MessageBoxButtons.OK, MessageBoxIcon.Information);

            loginAttempts = 0;
        }

        String login, password, generatedCaptcha, inputCaptcha;
        bool authSuccess;

        private void timerBlock_Tick(object sender, EventArgs e)
        {
            timerBlock.Stop();
        }

        int loginAttempts = 0;

        /// <summary>
        /// Авторизация
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonLogIn_Click(object sender, EventArgs e)
        {
            if (timerBlock.Enabled)
            {
                MessageBox.Show("Вы заблокированы на 10 секунд за ввод неверных данных", "Авторизация", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Button button = (Button)sender;
            authSuccess = true;
            if (button.Text == buttonLogIn.Text)
            {
                login = textBoxLogin.Text;
                password = textBoxPass.Text;
                inputCaptcha = textBoxCaptcha.Text;

                if (String.IsNullOrEmpty(login) || String.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Вы ввели не все данные", "Авторизация", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                SqlCommand sqlCommand = Helper.SqlConnection.CreateCommand();
                String sql = $"SELECT * FROM [User] WHERE UserLogin = '{login}' AND UserPassword = '{password}'";
                sqlCommand.CommandText = sql;
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                if (sqlDataReader.Read())
                {
                    Helper.UserID = (int)sqlDataReader["UserID"];
                    Helper.UserRoleID = (int)sqlDataReader["UserRoleID"];
                    if (loginAttempts > 0)
                    {
                        if (inputCaptcha != generatedCaptcha)
                        {
                            loginAttempts++;
                            authSuccess = false;
                        }
                    }
                }
                else
                {
                    authSuccess = false;
                    if (loginAttempts == 0)
                    {
                        labelCaptcha.Visible = true;
                        textBoxCaptcha.Visible = true;
                        pictureBoxCaptcha.Visible = true;
                    }
                    loginAttempts++;
                }
                sqlDataReader.Close();
            }
            else
            {
                Helper.UserID = 0;
                Helper.UserRoleID = 0;
            }
            if (!authSuccess)
            {
                MessageBox.Show("Вы ввели неверный логин, пароль или капчу", "Авторизация", MessageBoxButtons.OK, MessageBoxIcon.Error);
                pictureBoxCaptcha.Image = CreateCaptcha(out generatedCaptcha);
                if (loginAttempts > 1) timerBlock.Enabled = true;
                return;
            }
            else
            {
                MessageBox.Show("Вы вошли как " + (Roles)Helper.UserRoleID, "Авторизация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        public Bitmap CreateCaptcha(out string captcha)
        {
            Random rnd = new Random();
            int i = 0;

            string context = String.Empty;
            string ALF = "1234567890QWERTYUIOPASDFGHJKLZXCVBNM";
            for (i = 0; i < 5; i++)
            {
                context += ALF[rnd.Next(ALF.Length)];
            }

            captcha = context;

            int iHeight = 80;
            int iWidth = 190;
            Random oRandom = new Random();

            //int[] aBackgroundNoiseColor = new int[] { 150, 150, 150 };
            //int[] aTextColor = new int[] { 0, 0, 0 };
            int[] aFontEmSizes = new int[] { 15, 20, 25, 30, 35 };

            string[] aFontNames = new string[]
            {
                 "Comic Sans MS",
                 "Arial",
                 "Times New Roman",
                 "Georgia",
                 "Verdana",
                 "Geneva"
            };
            FontStyle[] aFontStyles = new FontStyle[]
            {
                 FontStyle.Bold,
                 FontStyle.Italic,
                 FontStyle.Regular,
                 FontStyle.Strikeout,
                  FontStyle.Underline
            };
            HatchStyle[] aHatchStyles = new HatchStyle[]
            {
 HatchStyle.BackwardDiagonal, HatchStyle.Cross,
    HatchStyle.DashedDownwardDiagonal, HatchStyle.DashedHorizontal,
 HatchStyle.DashedUpwardDiagonal, HatchStyle.DashedVertical,
    HatchStyle.DiagonalBrick, HatchStyle.DiagonalCross,
 HatchStyle.Divot, HatchStyle.DottedDiamond, HatchStyle.DottedGrid,
    HatchStyle.ForwardDiagonal, HatchStyle.Horizontal,
 HatchStyle.HorizontalBrick, HatchStyle.LargeCheckerBoard,
    HatchStyle.LargeConfetti, HatchStyle.LargeGrid,
 HatchStyle.LightDownwardDiagonal, HatchStyle.LightHorizontal,
    HatchStyle.LightUpwardDiagonal, HatchStyle.LightVertical,
 HatchStyle.Max, HatchStyle.Min, HatchStyle.NarrowHorizontal,
    HatchStyle.NarrowVertical, HatchStyle.OutlinedDiamond,
 HatchStyle.Plaid, HatchStyle.Shingle, HatchStyle.SmallCheckerBoard,
    HatchStyle.SmallConfetti, HatchStyle.SmallGrid,
 HatchStyle.SolidDiamond, HatchStyle.Sphere, HatchStyle.Trellis,
    HatchStyle.Vertical, HatchStyle.Wave, HatchStyle.Weave,
 HatchStyle.WideDownwardDiagonal, HatchStyle.WideUpwardDiagonal, HatchStyle.ZigZag
            };

            //Get Captcha in Session
            string sCaptchaText = context;

            //Creates an output Bitmap
            Bitmap oOutputBitmap = new Bitmap(iWidth, iHeight, PixelFormat.Format24bppRgb);
            Graphics oGraphics = Graphics.FromImage(oOutputBitmap);
            oGraphics.TextRenderingHint = TextRenderingHint.AntiAlias;

            //Create a Drawing area
            RectangleF oRectangleF = new RectangleF(0, 0, iWidth, iHeight);
            Brush oBrush = default(Brush);

            //Draw background (Lighter colors RGB 100 to 255)
            oBrush = new HatchBrush(aHatchStyles[oRandom.Next
                (aHatchStyles.Length - 1)], Color.FromArgb((oRandom.Next(100, 255)),
                (oRandom.Next(100, 255)), (oRandom.Next(100, 255))), Color.White);
            oGraphics.FillRectangle(oBrush, oRectangleF);

            System.Drawing.Drawing2D.Matrix oMatrix = new System.Drawing.Drawing2D.Matrix();
            
            for (i = 0; i <= sCaptchaText.Length - 1; i++)
            {
                oMatrix.Reset();
                int iChars = sCaptchaText.Length;
                int x = iWidth / (iChars + 1) * i;
                int y = iHeight / 2;

                //Rotate text Random
                oMatrix.RotateAt(oRandom.Next(-40, 40), new PointF(x, y));
                oGraphics.Transform = oMatrix;

                //Draw the letters with Random Font Type, Size and Color
                oGraphics.DrawString
                (
                //Text
                sCaptchaText.Substring(i, 1),
                //Random Font Name and Style
                new Font(aFontNames[oRandom.Next(aFontNames.Length - 1)],
                   aFontEmSizes[oRandom.Next(aFontEmSizes.Length - 1)],
                   FontStyle.Strikeout),
                   //aFontStyles[oRandom.Next(aFontStyles.Length - 1)]),
                //Random Color (Darker colors RGB 0 to 100)
                new SolidBrush(Color.FromArgb(oRandom.Next(0, 100),
                   oRandom.Next(0, 100), oRandom.Next(0, 100))),
                x,
                oRandom.Next(10, 40)
                );
                oGraphics.ResetTransform();
            }
            return oOutputBitmap;
        }
    }
}
