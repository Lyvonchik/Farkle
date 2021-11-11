using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Windows.Media;
using AxNetwork;
using System.IO;

namespace Farkle_WinForm
{
    public partial class Form1 : Form
    {
        //
        private Tcp objSocketServer = new Tcp();
        private Tcp objSocketClient = new Tcp();
        private NwConstants objConstants = new NwConstants();
        static int ctlServerPort = 1500;
        static int ctlClientPort = 1500;
        static string ctlClientHost = "127.0.0.1";

        //

        static bool[] rerollPosition = new bool[6];
        static bool[] d_ = new bool[6];
        static bool[] Full = new bool[6];
        static int[] dice = new int[6];
        static int oneCount = 0, twoCount = 0, threeCount = 0, fourCount = 0, fiveCount = 0, sixCount = 0, full = 0;
        static int score, firstScore, cot = 0, midleScore = 0;
        static int Farklecount = 0;
        static int rejim = 0, playerScore = 0, playerTurns = 0, train=0, enemyTurns = 0;
        static bool t = true, c = false, m = false, conn= false, O = false; 
        Win win = new Win();
        Lose lose = new Lose();
        Combo combo = new Combo();
        static string path = Directory.GetCurrentDirectory();
        FileInfo fi = new FileInfo(path + @"\Resources\");

        static int scoring(int playerScore)
        {
            score = 0;
            full = 0;
            for (int i = 0; i < 6; i++)
            {
                Full[i] = true;
            }

            if (twoCount == 2 && Full[2] == true)
            {
                full++;
                Full[2] = false;
            }

            if (threeCount == 2 && Full[3] == true)
            {
                full++;
                Full[3] = false;
            }

            if (fourCount == 2 && Full[4] == true)
            {
                full++;
                Full[4] = false;
            }

            if (sixCount == 2 && Full[5] == true)
            {
                full++;
                Full[5] = false;
            }

            if (oneCount == 1 && twoCount == 1 && threeCount == 1 && fourCount == 1 && fiveCount == 1 && sixCount == 1)
            {
                score = (score - 150) + 1000;
                for (int i = 0; i < 6; i++)
                {
                    if (dice[i] == 1 || dice[i] == 2 || dice[i] == 3 || dice[i] == 4 || dice[i] == 5 || dice[i] == 6) { rerollPosition[i] = false; }
                }
            }
            else
            {
                if (oneCount == 1)
                {
                    score += 100;
                }

                if (oneCount == 2)
                {
                    score += 200;
                    if (Full[1] == true)
                        full++;
                    Full[1] = false;
                }

                if (oneCount == 3)
                {
                    score += 1000;
                }

                else if (oneCount == 4)
                {
                    score += 2000;
                }

                else if (oneCount == 5)
                {
                    score += 4000;
                }

                else if (oneCount == 6)
                {
                    score += 8000;
                }

                if (twoCount == 3)
                {
                    score += 200;
                }

                if (twoCount == 4)
                {
                    score += 400;
                }

                if (twoCount == 5)
                {
                    score += 800;
                }

                if (twoCount == 6)
                {
                    score += 1600;
                }

                if (threeCount == 3)
                {
                    score += 300;
                }

                if (threeCount == 4)
                {
                    score += 600;
                }

                if (threeCount == 5)
                {
                    score += 1200;
                }

                if (threeCount == 6)
                {
                    score += 2400;
                }

                if (fourCount == 3)
                {
                    score += 400;
                }

                if (fourCount == 4)
                {
                    score += 800;
                }

                if (fourCount == 5)
                {
                    score += 1600;
                }

                if (fourCount == 6)
                {
                    score += 3200;
                }

                if (fiveCount == 1)
                {
                    score += 50;
                }

                if (fiveCount == 2)
                {
                    score += 100;
                    if (Full[5] == true)
                        full++;
                    Full[5] = false;
                }

                if (fiveCount == 3)
                {
                    score += 500;
                }

                if (fiveCount == 4)
                {
                    score += 1000;
                }

                if (fiveCount == 5)
                {
                    score += 2000;
                }

                if (fiveCount == 6)
                {
                    score += 4000;
                }

                if (sixCount == 3)
                {
                    score += 600;
                }

                if (sixCount == 4)
                {
                    score += 1200;
                }

                if (sixCount == 5)
                {
                    score += 2400;
                }

                if (sixCount == 6)
                {
                    score += 4800;
                }
            }

            if (full == 3)
            {
                if (oneCount == 2) { score -= 200; }
                if (fiveCount == 2) { score -= 100; }
                score += 750;
            }

            firstScore = score;    //исходное число
            return playerScore;
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Farkle()
        {
            if (pictureBox0.Visible == true) pictureBox0_Click(null, null);
            if (pictureBox1.Visible == true) pictureBox1_Click(null, null);
            if (pictureBox2.Visible == true) pictureBox2_Click(null, null);
            if (pictureBox3.Visible == true) pictureBox3_Click(null, null);
            if (pictureBox4.Visible == true) pictureBox4_Click(null, null);
            if (pictureBox5.Visible == true) pictureBox5_Click(null, null);
            scoring(playerScore);//proverka na zonk
            if (score == 0)
            {
                train = 8;
                Train_Text();
                MessageBox.Show("Зонк!\nВы не выбросили ниодного стоящего кубика", "Зонк", MessageBoxButtons.OK);
                if (Farklecount == 3)
                {
                    playerScore -= 1000;
                    Farklecount = 0;
                    Pscore.Text = playerScore.ToString();
                }
                score = 0;
                midleScore = 0;
                Clear_desk();
                Dice_Roll.Show();
                full = 0;
                c = false;
            }
            if (pictureBox0.Visible == true) pictureBox0_Click(null, null);
            if (pictureBox1.Visible == true) pictureBox1_Click(null, null);
            if (pictureBox2.Visible == true) pictureBox2_Click(null, null);
            if (pictureBox3.Visible == true) pictureBox3_Click(null, null);
            if (pictureBox4.Visible == true) pictureBox4_Click(null, null);
            if (pictureBox5.Visible == true) pictureBox5_Click(null, null);
        }

        private void Dice_Roll_Click(object sender, EventArgs e)
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(fi + @"Music\roll.wav");//звук броска
            player.Play();
            System.Threading.Thread.Sleep(4000);
            train = 2;
            Train_Text();
            for (int i = 0; i < 6; i++)
            {
                d_[i] = false;
                rerollPosition[i] = false;
                dice[i] = 0;
            }
            oneCount = 0;
            twoCount = 0;
            threeCount = 0;
            fourCount = 0;
            fiveCount = 0;
            sixCount = 0;
            cot = 0;
            full = 0;
            diceRoll();
            printRoll();
            Rscore.Text = score.ToString();
            Dice_Roll.Hide();
            playerTurns++;
            Pturns.Image = Image.FromFile(fi + "t" + playerTurns + ".png");
            Farklecount++;
            Farkle();
        }

        private void Skip_Click(object sender, EventArgs e)
        {
            if (rejim == 4 && c==false)
            {
                if (t == true)
                {
                    train = 7;
                    Train_Text();
                    Skip.Top = 390;
                    Skip.Left = 100;
                    t = false;
                }
                else
                {
                    train = 7;
                    Train_Text();
                    Skip.Top = 390;
                    Skip.Left = 225;
                    t = true;
                }
            }
            else
            {
                System.Media.SoundPlayer player = new System.Media.SoundPlayer(fi + @"Music\poker-chips.wav");//звук сбора очков
                player.Play();
                System.Threading.Thread.Sleep(1000);
                Clear_desk();
                train = 6;
                Train_Text();
                for (int i = 0; i < 6; i++)
                {
                    rerollPosition[i] = false;
                    dice[i] = 0;
                    d_[i] = false;
                }
                oneCount = 0;
                twoCount = 0;
                threeCount = 0;
                fourCount = 0;
                fiveCount = 0;
                sixCount = 0;
                Farklecount = 0;
                playerScore += score + midleScore;
                score = 0;
                midleScore = 0;
                Rscore.Text = score.ToString();
                Pscore.Text = playerScore.ToString();
                Mscore.Text = midleScore.ToString();
                Skip.Hide();
                Re_Roll.Hide();
                Dice_Roll.Show();
                if (rejim == 6)
                {
                    objSocketClient.SendString(playerTurns.ToString());
                    objSocketClient.SendString(Pscore.Text);
                }
            }
            switch (rejim)
            {
                case 1:
                    {
                        if (playerScore >= 3000)
                        {
                            System.Media.SoundPlayer player = new System.Media.SoundPlayer(fi + @"Music\happy-win.wav");//звук победы
                            player.Play();
                            System.Threading.Thread.Sleep(1000);
                            Menu.PerformClick();
                            win.ShowDialog();
                        }
                        break;
                    }
                case 2:
                    {
                        if (playerScore >= 5000)
                        {
                            System.Media.SoundPlayer player = new System.Media.SoundPlayer(fi + @"Music\happy-win.wav");//звук победы
                            player.Play();
                            System.Threading.Thread.Sleep(1000);
                            Menu.PerformClick();
                            win.ShowDialog();
                        }
                        break;
                    }
                case 3:
                    {
                        if (playerTurns == 10)
                        {
                            System.Media.SoundPlayer player = new System.Media.SoundPlayer(fi + @"Music\happy-win.wav");//звук победы
                            player.Play();
                            System.Threading.Thread.Sleep(1000);
                            win.ShowDialog();
                            Menu.PerformClick();
                        }
                        break;
                    }
                case 5:
                    {
                        if (playerTurns == 5)
                        {
                            System.Media.SoundPlayer player = new System.Media.SoundPlayer(fi + @"Music\happy-win.wav");//звук победы
                            player.Play();
                            System.Threading.Thread.Sleep(1000);
                            win.ShowDialog();
                            Menu.PerformClick();
                        }
                        break;
                    }
                case 6:
                    {
                        if (playerTurns>=2)
                        {
                            Dice_Roll.Hide();
                        }
                        if (enemyTurns >= 2 && playerTurns >= 2 && Int32.Parse(Escore.Text) < Int32.Parse(Pscore.Text)) 
                        {
                            System.Media.SoundPlayer player = new System.Media.SoundPlayer(fi + @"Music\happy-win.wav");//звук победы
                            player.Play();
                            win.ShowDialog();
                            System.Threading.Thread.Sleep(2000);
                            Menu.PerformClick();
                        }
                        else if (enemyTurns >= 2 && playerTurns >= 2 && Int32.Parse(Escore.Text) > Int32.Parse(Pscore.Text))
                        {
                            System.Media.SoundPlayer player = new System.Media.SoundPlayer(fi + @"Music\loose.wav");//звук победы
                            player.Play();
                            lose.ShowDialog();
                            System.Threading.Thread.Sleep(2000);
                            Menu.PerformClick();
                        }
                        break;
                    }
                default: break;
            }
        }

        private void Re_Roll_Click(object sender, EventArgs e)
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(fi + @"Music\roll.wav");//звук броска
            player.Play();
            c = true;
            oneCount = 0;
            twoCount = 0;
            threeCount = 0;
            fourCount = 0;
            fiveCount = 0;
            sixCount = 0;
            System.Threading.Thread.Sleep(4000);
            reroll();
            printRoll();
            midleScore += score;
            Farkle();
            Mscore.Text = midleScore.ToString();
            Rscore.Text = score.ToString();
            Re_Roll.Hide();
            Skip.Hide();
        }

        private void Menu_Click(object sender, EventArgs e)
        {
            Menuxa();
            System.Threading.Thread.Sleep(100);
            Menu.Hide();
            To3000.Hide();
            To5000.Hide();
            Single.Show();
            Train.Show();
            Turns_10.Show();
            Online.Show();
            Dice_Roll.Hide();
            Skip.Hide();
            Re_Roll.Hide();
            Pscore.Hide();
            Rscore.Hide();
            Mscore.Hide();
            Train_txt.Hide();
        }

        private void Single_Click(object sender, EventArgs e)
        {
            Single.Hide();
            Turns_10.Hide();
            Online.Hide();
            Menu.Show();
            To3000.Show();
            if (rejim != 4) { To5000.Show(); }
        }

        private void To3000_Click(object sender, EventArgs e)
        {
            if (rejim != 4) { rejim = 1; }
            else
            {
                train = 1;
                Train_Text();
            }
            To3000.Hide();
            To5000.Hide();
            Menu.Show();
            Dice_Roll.Show();
            Pscore.Show();
            Rscore.Show();
            Mscore.Show();
        }

        private void To5000_Click(object sender, EventArgs e)
        {
            rejim = 2;
            To3000.Hide();
            To5000.Hide();
            Menu.Show();
            Dice_Roll.Show();
            Pscore.Show();
            Rscore.Show();
            Mscore.Show();
        }

        private void Turns_10_Click(object sender, EventArgs e)
        {
            if (rejim != 4) { rejim = 3; }
            else
            {
                train = 1;
                Train_Text();
            }
            Single.Hide();
            Online.Hide();
            Turns_10.Hide();
            Menu.Show();  
            Dice_Roll.Show();
            Pscore.Show();
            Rscore.Show();
            Mscore.Show();
            Pturns.Show();
        }

        private void Train_Click(object sender, EventArgs e)
        {
            if (rejim != 0)
            {
                combo.Show();
            }
            else
            {
                rejim = 4;
                Train.Hide();
                Online.Hide();
                Menu.Show();
                train = 0;
                Train_Text();
                pictureBox0.Image = Image.FromFile(fi + "Right.png");
                pictureBox0.Left = 150;
                pictureBox0.Top = 100;
                pictureBox0.Enabled = false;
                pictureBox0.Show();
            }
        }

        static int diceRoll()
        {
            Random rand = new Random();
            //dice[0] = 5; dice[1] = 2; dice[2] = 6; dice[3] = 2; dice[4] = 3; dice[5] = 4;
            for (int i = 0; i < 6; i++)
            {
                dice[i] = (rand.Next() % 6 + 1);
            }
            return 0;
        }

        static int reroll()
        {
            Random rand = new Random();
            //dice[0] = 1; dice[1] = 6; dice[2] = 6; dice[3] = 6; dice[4] = 3; dice[5] = 3;
            for (int i = 0; i < 6; i++)
            {
                if (rerollPosition[i] == false)
                {
                    dice[i] = (rand.Next() % 6 + 1);
                }
            }
            return 0;
        }

        //
        private void Online_Click(object sender, EventArgs e)
        {
            Single.Hide();
            Turns_10.Hide();
            Online.Hide();
            Menu.Show();
            Connect.Show();
            Host.Show();
            rejim = 6;
        }
        private void Host_Click(object sender, EventArgs e)
        {
            timer.Enabled = true;
            //
            Escore.Show();
            Host.Hide();
            Connect.Hide();

            //Хостование
            objSocketServer.Clear();
            objSocketServer.StartListening(ctlServerPort);
            ShowServerResult();
            //
        }
        private void Connect_Click(object sender, EventArgs e)
        {
            timer.Enabled = true;
            //
            Escore.Show();
            Host.Hide();
            Connect.Hide();

            //присоединиться
            objSocketClient.Clear();
            objSocketClient.Connect(ctlClientHost,ctlClientPort);
            ShowClientResult();
            //отправить подтверждение
            objSocketClient.SendString("0");
            ShowClientResult();
            //

            System.Threading.Thread.Sleep(1000);
            //Хостование
            objSocketServer.Clear();
            objSocketServer.StartListening(ctlServerPort);
            ShowServerResult();
            //
        }
        private long ShowServerResult()
        {
            return objSocketServer.LastError;
        }
        private long ShowClientResult()
        {
            return objSocketClient.LastError;
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            if (objSocketServer.ConnectionState == objConstants.nwSOCKET_CONNSTATE_CONNECTED)
            {
                while (objSocketServer.HasData())
                {
                    string temp = objSocketServer.ReceiveString();
                    
                    if (conn == false)
                    {
                        //присоединиться
                        objSocketClient.Clear();
                        objSocketClient.Connect(ctlClientHost, ctlClientPort);
                        ShowClientResult();
                        //отправить подтверждение
                        objSocketClient.SendString("0");
                        ShowClientResult();
                        //
                        conn = true;
                    }
                    if (temp == "0")
                    {
                        Eturns.Show();
                        Pturns.Show();
                        Dice_Roll.Show();
                        Mscore.Show();
                        Pscore.Show();
                        Rscore.Show();
                    }
                    if (temp == null)
                    {
                        if (enemyTurns >= 2 && playerTurns >= 2 && Int32.Parse(Escore.Text) < Int32.Parse(Pscore.Text))
                        {
                            System.Media.SoundPlayer player = new System.Media.SoundPlayer(fi + @"Music\happy-win.wav");//звук победы
                            player.Play();
                            win.ShowDialog();
                            System.Threading.Thread.Sleep(2000);
                            Menu.PerformClick();
                        }
                        else if (enemyTurns >= 2 && playerTurns >= 2 && Int32.Parse(Escore.Text) > Int32.Parse(Pscore.Text))
                        {
                            System.Media.SoundPlayer player = new System.Media.SoundPlayer(fi + @"Music\loose.wav");//звук победы
                            player.Play();
                            lose.ShowDialog();
                            System.Threading.Thread.Sleep(2000);
                            Menu.PerformClick();
                        }
                    }
                    else if (Int32.Parse(temp)>=0 && Int32.Parse(temp) <= 10)
                    {
                        enemyTurns = Int32.Parse(temp);
                        Eturns.Image = Image.FromFile(fi + "t" + temp + ".png");
                    }
                    else
                    {
                        Escore.Text = temp;
                    }
                }
            }
        }
        //

        private void printRoll()
        {
            pictureBox0.Left = 100;
            pictureBox0.Top = 225;
            pictureBox1.Left = 225;
            pictureBox1.Top = 225;
            pictureBox0.Enabled = true;
            pictureBox1.Enabled = true;
            if (rerollPosition[0] == false) { pictureBox0.Show(); }
            else { pictureBox0.Hide(); }
            if (rerollPosition[1] == false) { pictureBox1.Show(); }
            else { pictureBox1.Hide(); }
            if (rerollPosition[2] == false) { pictureBox2.Show(); }
            else { pictureBox2.Hide(); }
            if (rerollPosition[3] == false) { pictureBox3.Show(); }
            else { pictureBox3.Hide(); }
            if (rerollPosition[4] == false) { pictureBox4.Show(); }
            else { pictureBox4.Hide(); }
            if (rerollPosition[5] == false) { pictureBox5.Show(); }
            else { pictureBox5.Hide(); }
            pictureBox0.Image = Image.FromFile(fi + "dice" + dice[0] + ".png");
            pictureBox1.Image = Image.FromFile(fi + "dice" + dice[1] + ".png");
            pictureBox2.Image = Image.FromFile(fi + "dice" + dice[2] + ".png");
            pictureBox3.Image = Image.FromFile(fi + "dice" + dice[3] + ".png");
            pictureBox4.Image = Image.FromFile(fi + "dice" + dice[4] + ".png");
            pictureBox5.Image = Image.FromFile(fi + "dice" + dice[5] + ".png");
        }

        private void Clear_desk()
        {
            pictureBox0.Hide();
            pictureBox1.Hide();
            pictureBox2.Hide();
            pictureBox3.Hide();
            pictureBox4.Hide();
            pictureBox5.Hide();
        }
        
        private void pictureBox0_Click(object sender, EventArgs e)
        {
            if (d_[0] == false) { pictureBox0.Image = Image.FromFile(fi + "dice_click" + dice[0] + ".png"); d_[0] = true; }
            else { pictureBox0.Image = Image.FromFile(fi + "dice" + dice[0] + ".png"); d_[0] = false; }
            if (rerollPosition[0] == true)
            {
                switch (dice[0])
                {
                    case 1:
                        if (oneCount != 0)
                            oneCount--;
                        rerollPosition[0] = false;
                        break;
                    case 2:
                        if (twoCount != 0)
                            twoCount--;
                        rerollPosition[0] = false;
                        break;
                    case 3:
                        if (threeCount != 0)
                            threeCount--;
                        rerollPosition[0] = false;
                        break;
                    case 4:
                        if (fourCount != 0)
                            fourCount--;
                        rerollPosition[0] = false;
                        break;
                    case 5:
                        if (fiveCount != 0)
                            fiveCount--;
                        rerollPosition[0] = false;
                        break;
                    case 6:
                        if (sixCount != 0)
                            sixCount--;
                        rerollPosition[0] = false;
                        break;
                }

                scoring(playerScore);
                Rscore.Text = score.ToString();
            }
            else
            {
                switch (dice[0])
                {
                    case 1:
                        oneCount++;
                        rerollPosition[0] = true;
                        break;
                    case 2:
                        twoCount++;
                        rerollPosition[0] = true;
                        break;
                    case 3:
                        threeCount++;
                        rerollPosition[0] = true;
                        break;
                    case 4:
                        fourCount++;
                        rerollPosition[0] = true;
                        break;
                    case 5:
                        fiveCount++;
                        rerollPosition[0] = true;
                        break;
                    case 6:
                        sixCount++;
                        rerollPosition[0] = true;
                        break;
                }
                scoring(playerScore);
                Rscore.Text = score.ToString();
            }

            if (score != 0)
            {
                if ((twoCount < 3 && twoCount != 0) || (threeCount < 3 && threeCount != 0) || (fourCount < 3 && fourCount != 0) || (sixCount < 3 && sixCount != 0))
                {
                    Re_Roll.Hide();
                    Skip.Hide();

                    if (rerollPosition[0] == true && rerollPosition[1] == true && rerollPosition[2] == true && rerollPosition[3] == true && rerollPosition[4] == true && rerollPosition[5] == true && score >= 750)
                    {
                        Re_Roll.Hide();
                        Skip.Show();
                        if (rejim == 4 && c == false)
                        {
                            train = 4;
                            Train_Text();
                        }
                    }
                }
                else
                {
                    if (rerollPosition[1] == true && rerollPosition[2] == true && rerollPosition[3] == true && rerollPosition[4] == true && rerollPosition[0] == true) { }
                    else { Re_Roll.Show(); }
                    if (rerollPosition[0] == true && rerollPosition[1] == true && rerollPosition[2] == true && rerollPosition[3] == true && rerollPosition[4] == true && rerollPosition[5] == true) { Re_Roll.Hide(); }
                    else { Re_Roll.Show(); }
                    Skip.Show();
                    if (rejim == 4 && c==false)
                    {
                        train = 3;
                        Train_Text();
                    }
                    else
                    {
                        train = 5;
                        Train_Text();
                    }
                }
            }
            else if (score == 0)
            {
                Re_Roll.Hide();
                Skip.Hide();
                train = 2;
                Train_Text();
                if (c == true)
                {
                    Train_txt.Hide();
                    Mscore.Show();
                    Pscore.Show();
                }
            }
        }
    
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (rejim != 0)
            {
                if (d_[1] == false) { pictureBox1.Image = Image.FromFile(fi + "dice_click" + dice[1] + ".png"); d_[1] = true; }
                else { pictureBox1.Image = Image.FromFile(fi + "dice" + dice[1] + ".png"); d_[1] = false; }
                if (rerollPosition[1] == true)
                {
                    switch (dice[1])
                    {
                        case 1:
                            if (oneCount != 0)
                                oneCount--;
                            rerollPosition[1] = false;
                            break;
                        case 2:
                            if (twoCount != 0)
                                twoCount--;
                            rerollPosition[1] = false;
                            break;
                        case 3:
                            if (threeCount != 0)
                                threeCount--;
                            rerollPosition[1] = false;
                            break;
                        case 4:
                            if (fourCount != 0)
                                fourCount--;
                            rerollPosition[1] = false;
                            break;
                        case 5:
                            if (fiveCount != 0)
                                fiveCount--;
                            rerollPosition[1] = false;
                            break;
                        case 6:
                            if (sixCount != 0)
                                sixCount--;
                            rerollPosition[1] = false;
                            break;
                    }

                    scoring(playerScore);
                    Rscore.Text = score.ToString();
                }
                else
                {
                    switch (dice[1])
                    {
                        case 1:
                            oneCount++;
                            rerollPosition[1] = true;
                            break;
                        case 2:
                            twoCount++;
                            rerollPosition[1] = true;
                            break;
                        case 3:
                            threeCount++;
                            rerollPosition[1] = true;
                            break;
                        case 4:
                            fourCount++;
                            rerollPosition[1] = true;
                            break;
                        case 5:
                            fiveCount++;
                            rerollPosition[1] = true;
                            break;
                        case 6:
                            sixCount++;
                            rerollPosition[1] = true;
                            break;
                    }
                    scoring(playerScore);
                    Rscore.Text = score.ToString();
                }

                if (score != 0)
                {
                    if ((twoCount < 3 && twoCount != 0) || (threeCount < 3 && threeCount != 0) || (fourCount < 3 && fourCount != 0) || (sixCount < 3 && sixCount != 0))
                    {
                        Re_Roll.Hide();
                        Skip.Hide();

                        if (rerollPosition[0] == true && rerollPosition[1] == true && rerollPosition[2] == true && rerollPosition[3] == true && rerollPosition[4] == true && rerollPosition[5] == true && score >= 750)
                        {
                            Re_Roll.Hide();
                            Skip.Show();
                            if (rejim == 4 && c == false)
                            {
                                train = 4;
                                Train_Text();
                            }
                        }
                    }
                    else
                    {
                        if (rerollPosition[1] == true && rerollPosition[2] == true && rerollPosition[3] == true && rerollPosition[4] == true && rerollPosition[0] == true) { }
                        else { Re_Roll.Show(); }
                        if (rerollPosition[0] == true && rerollPosition[1] == true && rerollPosition[2] == true && rerollPosition[3] == true && rerollPosition[4] == true && rerollPosition[5] == true) { Re_Roll.Hide(); }
                        else { Re_Roll.Show(); }
                        Skip.Show();
                        if (rejim == 4 && c == false)
                        {
                            train = 3;
                            Train_Text();
                        }
                        else
                        {
                            train = 5;
                            Train_Text();
                        }
                    }
                }
                else if (score == 0)
                {
                    Re_Roll.Hide();
                    Skip.Hide();
                    train = 2;
                    Train_Text();
                    if (c == true)
                    {
                        Train_txt.Hide();
                        Mscore.Show();
                        Pscore.Show();
                    }
                }
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (rejim != 0)
            {
                if (d_[2] == false) { pictureBox2.Image = Image.FromFile(fi + "dice_click" + dice[2] + ".png"); d_[2] = true; }
                else { pictureBox2.Image = Image.FromFile(fi + "dice" + dice[2] + ".png"); d_[2] = false; }
                if (rerollPosition[2] == true)
                {
                    switch (dice[2])
                    {
                        case 1:
                            if (oneCount != 0)
                                oneCount--;
                            rerollPosition[2] = false;
                            break;
                        case 2:
                            if (twoCount != 0)
                                twoCount--;
                            rerollPosition[2] = false;
                            break;
                        case 3:
                            if (threeCount != 0)
                                threeCount--;
                            rerollPosition[2] = false;
                            break;
                        case 4:
                            if (fourCount != 0)
                                fourCount--;
                            rerollPosition[2] = false;
                            break;
                        case 5:
                            if (fiveCount != 0)
                                fiveCount--;
                            rerollPosition[2] = false;
                            break;
                        case 6:
                            if (sixCount != 0)
                                sixCount--;
                            rerollPosition[2] = false;
                            break;
                    }

                    scoring(playerScore);
                    Rscore.Text = score.ToString();
                }
                else
                {
                    switch (dice[2])
                    {
                        case 1:
                            oneCount++;
                            rerollPosition[2] = true;
                            break;
                        case 2:
                            twoCount++;
                            rerollPosition[2] = true;
                            break;
                        case 3:
                            threeCount++;
                            rerollPosition[2] = true;
                            break;
                        case 4:
                            fourCount++;
                            rerollPosition[2] = true;
                            break;
                        case 5:
                            fiveCount++;
                            rerollPosition[2] = true;
                            break;
                        case 6:
                            sixCount++;
                            rerollPosition[2] = true;
                            break;
                    }
                    scoring(playerScore);
                    Rscore.Text = score.ToString();
                }

                if (score != 0)
                {
                    if ((twoCount < 3 && twoCount != 0) || (threeCount < 3 && threeCount != 0) || (fourCount < 3 && fourCount != 0) || (sixCount < 3 && sixCount != 0))
                    {
                        Re_Roll.Hide();
                        Skip.Hide();

                        if (rerollPosition[0] == true && rerollPosition[1] == true && rerollPosition[2] == true && rerollPosition[3] == true && rerollPosition[4] == true && rerollPosition[5] == true && score >= 750)
                        {
                            Re_Roll.Hide();
                            Skip.Show();
                            if (rejim == 4 && c == false)
                            {
                                train = 4;
                                Train_Text();
                            }
                        }
                    }
                    else
                    {
                        if (rerollPosition[1] == true && rerollPosition[2] == true && rerollPosition[3] == true && rerollPosition[4] == true && rerollPosition[0] == true) { }
                        else { Re_Roll.Show(); }
                        if (rerollPosition[0] == true && rerollPosition[1] == true && rerollPosition[2] == true && rerollPosition[3] == true && rerollPosition[4] == true && rerollPosition[5] == true) { Re_Roll.Hide(); }
                        else { Re_Roll.Show(); }
                        Skip.Show();
                        if (rejim == 4 && c == false)
                        {
                            train = 3;
                            Train_Text();
                        }
                        else
                        {
                            train = 5;
                            Train_Text();
                        }
                    }
                }
                else if (score == 0)
                {
                    Re_Roll.Hide();
                    Skip.Hide();
                    train = 2;
                    Train_Text();
                    if (c == true)
                    {
                        Train_txt.Hide();
                        Mscore.Show();
                        Pscore.Show();
                    }
                }
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (rejim != 0)
            {
                if (d_[3] == false) { pictureBox3.Image = Image.FromFile(fi + "dice_click" + dice[3] + ".png"); d_[3] = true; }
                else { pictureBox3.Image = Image.FromFile(fi + "dice" + dice[3] + ".png"); d_[3] = false; }
                if (rerollPosition[3] == true)
                {
                    switch (dice[3])
                    {
                        case 1:
                            if (oneCount != 0)
                                oneCount--;
                            rerollPosition[3] = false;
                            break;
                        case 2:
                            if (twoCount != 0)
                                twoCount--;
                            rerollPosition[3] = false;
                            break;
                        case 3:
                            if (threeCount != 0)
                                threeCount--;
                            rerollPosition[3] = false;
                            break;
                        case 4:
                            if (fourCount != 0)
                                fourCount--;
                            rerollPosition[3] = false;
                            break;
                        case 5:
                            if (fiveCount != 0)
                                fiveCount--;
                            rerollPosition[3] = false;
                            break;
                        case 6:
                            if (sixCount != 0)
                                sixCount--;
                            rerollPosition[3] = false;
                            break;
                    }

                    scoring(playerScore);
                    Rscore.Text = score.ToString();
                }
                else
                {
                    switch (dice[3])
                    {
                        case 1:
                            oneCount++;
                            rerollPosition[3] = true;
                            break;
                        case 2:
                            twoCount++;
                            rerollPosition[3] = true;
                            break;
                        case 3:
                            threeCount++;
                            rerollPosition[3] = true;
                            break;
                        case 4:
                            fourCount++;
                            rerollPosition[3] = true;
                            break;
                        case 5:
                            fiveCount++;
                            rerollPosition[3] = true;
                            break;
                        case 6:
                            sixCount++;
                            rerollPosition[3] = true;
                            break;
                    }
                    scoring(playerScore);
                    Rscore.Text = score.ToString();
                }

                if (score != 0)
                {
                    if ((twoCount < 3 && twoCount != 0) || (threeCount < 3 && threeCount != 0) || (fourCount < 3 && fourCount != 0) || (sixCount < 3 && sixCount != 0))
                    {
                        Re_Roll.Hide();
                        Skip.Hide();

                        if (rerollPosition[0] == true && rerollPosition[1] == true && rerollPosition[2] == true && rerollPosition[3] == true && rerollPosition[4] == true && rerollPosition[5] == true && score >= 750)
                        {
                            Re_Roll.Hide();
                            Skip.Show();
                            if (rejim == 4 && c == false)
                            {
                                train = 4;
                                Train_Text();
                            }
                        }
                    }
                    else
                    {
                        if (rerollPosition[1] == true && rerollPosition[2] == true && rerollPosition[3] == true && rerollPosition[4] == true && rerollPosition[0] == true) { }
                        else { Re_Roll.Show(); }
                        if (rerollPosition[0] == true && rerollPosition[1] == true && rerollPosition[2] == true && rerollPosition[3] == true && rerollPosition[4] == true && rerollPosition[5] == true) { Re_Roll.Hide(); }
                        else { Re_Roll.Show(); }
                        Skip.Show();
                        if (rejim == 4 && c == false)
                        {
                            train = 3;
                            Train_Text();
                        }
                        else
                        {
                            train = 5;
                            Train_Text();
                        }
                    }
                }
                else if (score == 0)
                {
                    Re_Roll.Hide();
                    Skip.Hide();
                    train = 2;
                    Train_Text();
                    if (c == true)
                    {
                        Train_txt.Hide();
                        Mscore.Show();
                        Pscore.Show();
                    }
                }
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            if (rejim != 0)
            {
                if (d_[4] == false) { pictureBox4.Image = Image.FromFile(fi + "dice_click" + dice[4] + ".png"); d_[4] = true; }
                else { pictureBox4.Image = Image.FromFile(fi + "dice" + dice[4] + ".png"); d_[4] = false; }
                if (rerollPosition[4] == true)
                {
                    switch (dice[4])
                    {
                        case 1:
                            if (oneCount != 0)
                                oneCount--;
                            rerollPosition[4] = false;
                            break;
                        case 2:
                            if (twoCount != 0)
                                twoCount--;
                            rerollPosition[4] = false;
                            break;
                        case 3:
                            if (threeCount != 0)
                                threeCount--;
                            rerollPosition[4] = false;
                            break;
                        case 4:
                            if (fourCount != 0)
                                fourCount--;
                            rerollPosition[4] = false;
                            break;
                        case 5:
                            if (fiveCount != 0)
                                fiveCount--;
                            rerollPosition[4] = false;
                            break;
                        case 6:
                            if (sixCount != 0)
                                sixCount--;
                            rerollPosition[4] = false;
                            break;
                    }
                    scoring(playerScore);
                    Rscore.Text = score.ToString();
                }
                else
                {
                    switch (dice[4])
                    {
                        case 1:
                            oneCount++;
                            rerollPosition[4] = true;
                            break;
                        case 2:
                            twoCount++;
                            rerollPosition[4] = true;
                            break;
                        case 3:
                            threeCount++;
                            rerollPosition[4] = true;
                            break;
                        case 4:
                            fourCount++;
                            rerollPosition[4] = true;
                            break;
                        case 5:
                            fiveCount++;
                            rerollPosition[4] = true;
                            break;
                        case 6:
                            sixCount++;
                            rerollPosition[4] = true;
                            break;
                    }
                    scoring(playerScore);
                    Rscore.Text = score.ToString();
                }

                if (score != 0)
                {
                    if ((twoCount < 3 && twoCount != 0) || (threeCount < 3 && threeCount != 0) || (fourCount < 3 && fourCount != 0) || (sixCount < 3 && sixCount != 0))
                    {
                        Re_Roll.Hide();
                        Skip.Hide();

                        if (rerollPosition[0] == true && rerollPosition[1] == true && rerollPosition[2] == true && rerollPosition[3] == true && rerollPosition[4] == true && rerollPosition[5] == true && score >= 750)
                        {
                            Re_Roll.Hide();
                            Skip.Show();
                            if (rejim == 4 && c == false)
                            {
                                train = 4;
                                Train_Text();
                            }
                        }
                    }
                    else
                    {
                        if (rerollPosition[1] == true && rerollPosition[2] == true && rerollPosition[3] == true && rerollPosition[4] == true && rerollPosition[0] == true) { }
                        else { Re_Roll.Show(); }
                        if (rerollPosition[0] == true && rerollPosition[1] == true && rerollPosition[2] == true && rerollPosition[3] == true && rerollPosition[4] == true && rerollPosition[5] == true) { Re_Roll.Hide(); }
                        else { Re_Roll.Show(); }
                        Skip.Show();
                        if (rejim == 4 && c == false)
                        {
                            train = 3;
                            Train_Text();
                        }
                        else
                        {
                            train = 5;
                            Train_Text();
                        }
                    }
                }
                else if (score == 0)
                {
                    Re_Roll.Hide();
                    Skip.Hide();
                    train = 2;
                    Train_Text();
                    if (c == true)
                    {
                        Train_txt.Hide();
                        Mscore.Show();
                        Pscore.Show();
                    }
                }
            }
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            if (rejim != 0)
            {
                if (d_[5] == false) { pictureBox5.Image = Image.FromFile(fi + "dice_click" + dice[5] + ".png"); d_[5] = true; }
                else { pictureBox5.Image = Image.FromFile(fi + "dice" + dice[5] + ".png"); d_[5] = false; }
                if (rerollPosition[5] == true)
                {
                    switch (dice[5])
                    {
                        case 1:
                            if (oneCount != 0)
                                oneCount--;
                            rerollPosition[5] = false;
                            break;
                        case 2:
                            if (twoCount != 0)
                                twoCount--;
                            rerollPosition[5] = false;
                            break;
                        case 3:
                            if (threeCount != 0)
                                threeCount--;
                            rerollPosition[5] = false;
                            break;
                        case 4:
                            if (fourCount != 0)
                                fourCount--;
                            rerollPosition[5] = false;
                            break;
                        case 5:
                            if (fiveCount != 0)
                                fiveCount--;
                            rerollPosition[5] = false;
                            break;
                        case 6:
                            if (sixCount != 0)
                                sixCount--;
                            rerollPosition[5] = false;
                            break;
                    }
                    scoring(playerScore);
                    Rscore.Text = score.ToString();
                }
                else
                {
                    switch (dice[5])
                    {
                        case 1:
                            oneCount++;
                            rerollPosition[5] = true;
                            break;
                        case 2:
                            twoCount++;
                            rerollPosition[5] = true;
                            break;
                        case 3:
                            threeCount++;
                            rerollPosition[5] = true;
                            break;
                        case 4:
                            fourCount++;
                            rerollPosition[5] = true;
                            break;
                        case 5:
                            fiveCount++;
                            rerollPosition[5] = true;
                            break;
                        case 6:
                            sixCount++;
                            rerollPosition[5] = true;
                            break;
                    }
                    scoring(playerScore);
                    Rscore.Text = score.ToString();
                }

                if (score != 0)
                {
                    if ((twoCount < 3 && twoCount != 0) || (threeCount < 3 && threeCount != 0) || (fourCount < 3 && fourCount != 0) || (sixCount < 3 && sixCount != 0))
                    {
                        Re_Roll.Hide();
                        Skip.Hide();

                        if (rerollPosition[0] == true && rerollPosition[1] == true && rerollPosition[2] == true && rerollPosition[3] == true && rerollPosition[4] == true && rerollPosition[5] == true && score == 750)
                        {
                            Re_Roll.Hide();
                            Skip.Show();
                            if (rejim == 4 && c == false)
                            {
                                train = 4;
                                Train_Text();
                            }
                        }
                    }
                    else
                    {
                        if (rerollPosition[1] == true && rerollPosition[2] == true && rerollPosition[3] == true && rerollPosition[4] == true && rerollPosition[0] == true) {  }
                        else { Re_Roll.Show(); }
                        if (rerollPosition[0] == true && rerollPosition[1] == true && rerollPosition[2] == true && rerollPosition[3] == true && rerollPosition[4] == true && rerollPosition[5] == true) { Re_Roll.Hide(); }
                        else { Re_Roll.Show(); }
                        Skip.Show();
                        if (rejim == 4 && c == false)
                        {
                            train = 3;
                            Train_Text();
                        }
                        else
                        {
                            train = 5;
                            Train_Text();
                        }
                    }
                }
                else if (score == 0)
                {
                    Re_Roll.Hide();
                    Skip.Hide();
                    train = 2;
                    Train_Text();
                    if (c == true)
                    {
                        Train_txt.Hide();
                        Mscore.Show();
                        Pscore.Show();
                    }
                }
            }
        }

        private MediaPlayer music = new MediaPlayer();

        private void Music_Click(object sender, EventArgs e)
        {
            music.Open(new Uri(fi + @"Music\m1.wav"));
            if (m == false)
            {
                music.Play();
                Music.Image = Image.FromFile(fi + "m_play_move.png");
                m = true;
            }
            else
            {
                music.Stop();
                Music.Image = Image.FromFile(fi + "m_stop_move.png");
                m = false;
            }
        }

        private void Music_MouseEnter(object sender, EventArgs e)
        {
            if (m == false)
            {
                Music.Image = Image.FromFile(fi + "m_stop_move.png");
            }
            else
            {
                Music.Image = Image.FromFile(fi + "m_play_move.png");
            }
        }

        private void Music_MouseLeave(object sender, EventArgs e)
        {
            if (m == false)
            {
                Music.Image = Image.FromFile(fi + "m_stop.png");
            }
            else
            {
                Music.Image = Image.FromFile(fi + "m_play.png");
            }
        }

        void Menuxa()
        {
            Clear_desk();
            for (int i = 0; i < 6; i++)
            {
                rerollPosition[i] = false;
                d_[i] = false;
            }
            Connect.Hide();
            Re_Roll.Hide();
            Skip.Hide();
            Pscore.Hide();
            Dice_Roll.Hide();
            Menu.Hide();
            To3000.Hide();
            To5000.Hide();
            Host.Hide();
            Connect.Hide();
            Escore.Hide();
            Eturns.Hide();
            Pturns.Hide();
            Rscore.Text = "Счёт за раунд";
            Pscore.Text = "Счёт игрока";
            Mscore.Text = "Промежуточный счёт";
            Escore.Text = "Счёт противника";
            Pturns.Image = Image.FromFile(fi + "t0.png");
            Eturns.Image = Image.FromFile(fi + "t0.png");
            full = 0;
            cot = 0;
            score = 0;
            midleScore = 0;
            firstScore = 0;
            oneCount = 0;
            twoCount = 0;
            threeCount = 0;
            fourCount = 0;
            fiveCount = 0;
            sixCount = 0;
            playerScore = 0;
            Farklecount = 0;
            playerTurns = 0;
            enemyTurns = 0;
            conn = false;
            O = false;
            t = false;
            c = false;
            m = false;
            if (rejim == 4)
            {
                Train_txt.Hide();
                c = false;
                Skip.Top = 390;
                Skip.Left = 225;
            }
            rejim = 0;

            //остановить
            objSocketServer.StopListening();
            ShowServerResult();
            //отключиться
            objSocketServer.Disconnect();
            ShowServerResult();
            //отключться
            objSocketClient.Disconnect();
            ShowClientResult();
            //
            timer.Enabled = false;
        }

        void Train_Text()
        {
            if (rejim == 4)
            {
                Train_txt.Show();
                switch (train)
                {
                    case 0:
                        Train_txt.Top = 270;
                        Train_txt.Left = 80;
                        Train_txt.Text = "Приветствую в обучении, выбирай режим:\n" +
                            "+ Одиночная игра подразумевает, игру до\n" +
                            "определённого количества баллов\n" +
                            "+ 10 ходов - это игра, как следует из названия, до 10 ходов,\n" +
                            "где нужно набрать\n" +
                            "наибольшее количество баллов за эти ходы";
                        break;

                    case 1:
                        pictureBox0.Left = 440;
                        pictureBox0.Top = 65;
                        Train_txt.Top = 95;
                        Train_txt.Left = 100;
                        Train_txt.Text = "Здесь будут считаться\nтвои баллы\n\n\n" +
                            "+ Первая строка - это баллы, которые влияют на исход игры\n" +
                            "+ Вторая строка - баллы которые ты можешь получить\n" +
                            "за текущий бросок, или переброс кубиков\n" +
                            "+ В третью строку будут переноситься заработанные баллы,\n" +
                            "но будь осторожен, их можно легко потерять\n\n" +
                            "                    Давай бросим кубик";
                        break;

                    case 2:
                        Train_txt.Top = 100;
                        Train_txt.Left = 80;
                        Train_txt.Text = "Посмотрим что выпало\n\nКстати если не знаешь комбинации,\n\n\n\n\n\n" +
                            "выбери все кубики подряд и следи за баллами,\n" +
                            "а потом просто убери те что не принесли баллов.\n" +
                            "В обычной игре будет доступна кнопка \"?\"\n" +
                            "показывающяя комбинации";
                        break;

                    case 3:
                        Mscore.Hide();
                        Pscore.Hide();
                        Train_txt.Top = 80;
                        Train_txt.Left = 80;
                        Train_txt.Text = "Ну вот пару баллов набрали, для новичка самое оно,\n" +
                            "теперь только 2 пути\n" +
                            "Быть или не бы... Эм простите, забрать или не забрать?\n" +
                            "Если перебросить, то есть шанс потерять набранные очки,\n" +
                            "а забирать: \"маловато будет\"\n\n\n\n\n" +
                            "Давай перебросим?!";
                        break;

                    case 4:
                        Train_txt.Top = 70;
                        Train_txt.Left = 80;
                        Train_txt.Text = "Очень даже не плохо,\n" +
                            "а если ты новичок, то о чём вообще речь?\n\n\n\n\n\n\n\n" +
                            "Скорее забирай баллы";
                        break;

                    case 5:
                        Train_txt.Top = 65;
                        Train_txt.Left = 80;
                        Train_txt.Text = "Замечательно!\n" +
                            "Тебе повезло и ты не выбросил Зонк\n" +
                            "рискни перебросить кубик ещё раз\n" +
                            "или же забирай набранные баллы\n";
                        break;

                    case 6:
                        Pscore.Show();
                        Mscore.Show();
                        Skip.Top = 390;
                        Skip.Left = 225;
                        Train_txt.Top = 65;
                        Train_txt.Left = 80;
                        Train_txt.Text = "Великолепно!\nНа этом обучение закончено!\n" +
                            "Можешь повторить пройденный материал\nили вернуться в меню";
                        rejim = 5;
                        break;

                    case 7:
                        Train_txt.Top = 350;
                        Train_txt.Left = 80;
                        Random rand = new Random();
                        int i = (rand.Next() % 6 + 1);
                        switch (i)
                        {
                            case 1: Train_txt.Text = "Эм, давай перебросим!";
                                break;
                            case 2: Train_txt.Text = "Перебросим!";
                                break;
                            case 3: Train_txt.Text = "Зачем ты это делаешь?";
                                break;
                            case 4: Train_txt.Text = "Ну перебрось!";
                                break;
                            case 5: Train_txt.Text = "Ну не эту!";
                                break;
                            case 6: Train_txt.Text = "!";
                                break;
                        }
                        break;

                    case 8:
                        Train_txt.Top = 120;
                        Train_txt.Left = 80;
                        Train_txt.Text = "Ай! Не повезло то как...\n\nНу, ничего страшного!\n" +
                            "Это всего лишь \"зонк\", пока у тебя нет трёх подряд,\n" +
                            "серьёзного тебе ничего не грозит, но к сожалению,\n" +
                            "промежуточные очки ты будешь терять после каждого зонка!\n" +
                            "Будь осторожен";
                        break;

                    default:
                        break;
                }
            }
        }
    }
}
