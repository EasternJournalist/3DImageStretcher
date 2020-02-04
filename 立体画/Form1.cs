using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/* ========================================================================= *
 *                                                                           *
 *                                 立体画                                    *
 *                     Copyright (c) Wang Ruicheng                           *
 *           University of Science and Technology of China                   *
 *                          All rights reserved.                             *
 *                                                                           *
 *---------------------------------------------------------------------------*
 * This file is part of 立体画.                                              *
 *---------------------------------------------------------------------------*
 *                                                                           *
 * Redistribution and use in source and binary forms, with or without        *
 * modification, are permitted provided that the following conditions        *
 * are met:                                                                  *
 *                                                                           *
 * 1. Redistributions of source code must retain the above copyright notice, *
 *    this list of conditions and the following disclaimer.                  *
 *                                                                           *
 * 2. Redistributions in binary form must reproduce the above copyright      *
 *    notice, this list of conditions and the following disclaimer in the    *
 *    documentation and/or other materials provided with the distribution.   *
 *                                                                           *
 * 3. Neither the name of the copyright holder nor the names of its          *
 *    contributors may be used to endorse or promote products derived from   *
 *    this software without specific prior written permission.               *
 *                                                                           *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS       *
 * "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED *
 * TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A           *
 * PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER *
 * OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,  *
 * EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,       *
 * PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR        *
 * PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF    *
 * LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING      *
 * NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS        *
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.              *
 *                                                                           *
 * ========================================================================= */

//上面是非常沙雕而且没有什么用的版权申明


//如果在运行时遇到不清晰、不能正常显示的问题，请更改应用程序的高DPI设置

namespace 立体画
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 转换前后的图片
        /// </summary>
        public Bitmap PicBefore, PicTemp, PicAfter;
        public Graphics GTemp, GAfter;
        Vector2[] ControlPoint;
        public int ControlPointNum = 0;
        public int SelectedControlPoint = -1;
        /// <summary>
        /// 加载图片和初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                PicBefore = new Bitmap(TextBoxFile.Text);
                PicTemp = new Bitmap(PicBefore);
                pictureBox1.Image = PicTemp;
                
                GTemp = Graphics.FromImage(PicTemp);
                GTemp.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                ControlPoint = new Vector2[4] { new Vector2(PicBefore.Width*0.25, PicBefore.Height * 0.25), new Vector2(PicBefore.Width * 0.25, PicBefore.Height * 0.75), new Vector2(PicBefore.Width * 0.75, PicBefore.Height * 0.75), new Vector2(PicBefore.Width * 0.75, PicBefore.Height * 0.25) };
                DrawControlGrid();
            }
            catch(Exception ex)
            {
                MessageBox.Show("图片加载失败"+ex.ToString());
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            
        }

        private void splitContainer2_Resize(object sender, EventArgs e)
        {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 绘制控制点和网格线
        /// </summary>
        public void DrawControlGrid()
        {
            double a0 = (ControlPoint[0] - ControlPoint[1]).Mod(), b0 = (ControlPoint[1] - ControlPoint[2]).Mod(), c0 = (ControlPoint[2] - ControlPoint[3]).Mod(), d0 = (ControlPoint[3] - ControlPoint[0]).Mod();
            double a = a0 * Math.Pow(a0 / c0, 0.3), c = c0 * Math.Pow(c0 / a0, 0.3), b = b0 * Math.Pow(b0 / d0, 0.3), d = d0 * Math.Pow(d0 / b0, 0.3);
            //绘制网格线
            GTemp.DrawImage(PicBefore, 0, 0,PicBefore.Width,PicBefore.Height);
            int Hcount, Vcount;     //水平和竖直网格线个数
            int step = (int)(numericUpDown1.Value < numericUpDown2.Value ? numericUpDown1.Value : numericUpDown2.Value) / 16;
            Hcount = (int)numericUpDown1.Value / step;
            Vcount = (int)numericUpDown2.Value / step;

            Pen pen1 = new Pen(Color.FromArgb(210,220,10,10), 3), pen2 = new Pen(Color.FromArgb(180,10,10,220), 1.5f);
            for (int i = 1; i < Hcount; i++)
            {
                //double sH = ((2 - (double)i / Hcount) * a + (double)i / Hcount * c) * i / ((a + c) * Hcount);
                double sH = ((Math.Pow(c / a, (double)i / Hcount) - 1) / (c / a - 1));
                Vector2 p1 = (sH * ControlPoint[3] + (1 - sH) * ControlPoint[0]), p2 = (sH * ControlPoint[2] + (1 - sH) * ControlPoint[1]);
                GTemp.DrawLine(pen2, p1, p2);
            }
            for (int j = 1; j < Vcount; j++)
            {
                //double sV = ((2 - (double)j / Vcount) * d + (double)j / Vcount * b) * j / ((d + b) * Vcount);
                double sV = ((Math.Pow(b / d, (double)j / Vcount) - 1) / (b / d - 1));
                Vector2 p1 = (1 - sV) * ControlPoint[0] + sV * ControlPoint[1];
                Vector2 p2 = (1 - sV) * ControlPoint[3] + sV * ControlPoint[2];
                GTemp.DrawLine(pen2, p1, p2);
            }
            GTemp.DrawLine(pen1, ControlPoint[0], ControlPoint[1]);
            GTemp.DrawLine(pen1, ControlPoint[1], ControlPoint[2]);
            GTemp.DrawLine(pen1, ControlPoint[2], ControlPoint[3]);
            GTemp.DrawLine(pen1, ControlPoint[3], ControlPoint[0]);
            //绘制控制点
            Pen pen3 = new Pen(Color.Orange, 3);
            for (int i = 0; i < 4; i++)
            {
                GTemp.DrawEllipse(pen3, (float)ControlPoint[i].X - 8, (float)ControlPoint[i].Y - 8, 16, 16);
                GTemp.FillEllipse(new SolidBrush(Color.Orange), (float)ControlPoint[i].X - 3, (float)ControlPoint[i].Y - 3, 6, 6);

            }
            pictureBox1.Refresh();
        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (SelectedControlPoint > -1)
            {
                ControlPoint[SelectedControlPoint].X = e.X;
                ControlPoint[SelectedControlPoint].Y = e.Y;

                DrawControlGrid();
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            SelectedControlPoint = -1;
        }

        /// <summary>
        /// 点击生成按钮开始生成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click_2(object sender, EventArgs e)
        {
            //生成转换的图像
            double d1 = 1.0 / (double)numericUpDown1.Value, d2 = 1.0 / (double)numericUpDown2.Value;
            PicAfter = new Bitmap((int)numericUpDown1.Value, (int)numericUpDown2.Value);
            double a0 = (ControlPoint[0] - ControlPoint[1]).Mod(), b0 = (ControlPoint[1] - ControlPoint[2]).Mod(), c0 = (ControlPoint[2] - ControlPoint[3]).Mod(), d0 = (ControlPoint[3] - ControlPoint[0]).Mod();
            double a = a0 * Math.Pow(a0 / c0, 0.3), c = c0 * Math.Pow(c0 / a0, 0.3), b = b0 * Math.Pow(b0 / d0, 0.3), d = d0 * Math.Pow(d0 / b0, 0.3);
            for (int i = 0; i < PicAfter.Width; i++) 
            {
                for (int j = 0; j < PicAfter.Height; j++)
                {
                    double sH = ((Math.Pow(c / a, (double)i / PicAfter.Width) - 1) / (c / a - 1));
                    double sV = ((Math.Pow(b / d, (double)j / PicAfter.Height) - 1) / (b / d - 1));
                    Vector2 p = (1 - sV) * (sH * ControlPoint[3] + (1 - sH) * ControlPoint[0]) + sV * (sH * ControlPoint[2] + (1 - sH) * ControlPoint[1]);
                    if (p.X > 0 && p.X < PicBefore.Width && p.Y > 0 && p.Y < PicBefore.Height)
                        PicAfter.SetPixel(i, j, PicBefore.GetPixel((int)p.X, (int)p.Y));
                }
            }
            pictureBox2.Image = PicAfter;
            pictureBox2.Refresh();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
            try
            {
                PicAfter.Save(saveFileDialog1.FileName);
                MessageBox.Show("保存成功到" + saveFileDialog1.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败" + ex.ToString());
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            for (int i = 0; i <= 3; i++)
            {
                if ((e.X - ControlPoint[i].X) * (e.X - ControlPoint[i].X) + (e.Y - ControlPoint[i].Y) * (e.Y - ControlPoint[i].Y) < 65)
                {
                    SelectedControlPoint = i;
                }
            }
            
            
        }

        private void ButtonViewFiles_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            TextBoxFile.Text = openFileDialog1.FileName;
        }
    }
}
