using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenGL_lab
{
    public partial class Form1 : Form
    {
        GLgraphics glgraphics = new GLgraphics();
        public Form1()
        {
            InitializeComponent();
        }

        private void glControl1_Load(object sender, EventArgs e)
        {
            int texID = glgraphics.LoadTexture("texture.png");
            int texID1 = glgraphics.LoadTexture("texture1.png");
            int texID2 = glgraphics.LoadTexture("texture2.png");
            int texID3 = glgraphics.LoadTexture("texture3.png");
            int texID4 = glgraphics.LoadTexture("texture4.png");
            int texID5 = glgraphics.LoadTexture("texture5.png");
            int texID6 = glgraphics.LoadTexture("texture6.png");
            int texID7 = glgraphics.LoadTexture("texture7.png");
            glgraphics.texturesIDs.Add(texID);
            glgraphics.texturesIDs.Add(texID1);
            glgraphics.texturesIDs.Add(texID2);
            glgraphics.texturesIDs.Add(texID3);
            glgraphics.texturesIDs.Add(texID4);
            glgraphics.texturesIDs.Add(texID5);
            glgraphics.texturesIDs.Add(texID6);
            glgraphics.texturesIDs.Add(texID7);
            glgraphics.Setup(glControl1.Width, glControl1.Height);
            Application.Idle += Application_Idle;
            glgraphics.position[0] = true;
            for (int i = 0; i < 3; i++)
                glgraphics.isFound[i] = false;
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            label1.Text = "Записок найдено: " + glgraphics.foundNote + "/3";
            glgraphics.Update();
            glControl1.SwapBuffers();
        }

        private void glControl1_MouseMove(object sender, MouseEventArgs e)
        {
            float widthCoef = (-e.X - glControl1.Width * 0.5f) / (float)glControl1.Width;
            float heightCoef = (e.Y + glControl1.Height * 0.3f) / (float)glControl1.Height;
            glgraphics.latitude = heightCoef * 180;
            glgraphics.longitude = widthCoef * 360;
        }

        void Application_Idle(object sender, EventArgs e)
        {
            while (glControl1.IsIdle)
                glControl1.Refresh();
        }

        private void glControl1_KeyDown(object sender, KeyEventArgs e)
        {
            //--------------------------------------------------------------СТАРТОВАЯ КОМНАТА----------------------------------------------------------------------------------
            if (glgraphics.position[0])
            {
                if ((glgraphics.cameraMoveX > 1.5 && glgraphics.cameraMoveX < 8.5 && glgraphics.cameraMoveY > 1.5 && glgraphics.cameraMoveY < 8.5) || (glgraphics.cameraMoveX>=8.5 && (glgraphics.cameraMoveY>=3.5 && glgraphics.cameraMoveY<=6.5) && !glgraphics.DoorIsClose))
                {
                    if (e.KeyCode == Keys.W)
                    {
                        glgraphics.cameraMoveX += (float)(0.15f * Math.Cos(glgraphics.longitude * Math.PI / 180));
                        glgraphics.cameraMoveY += (float)(0.15f * Math.Sin(glgraphics.longitude * Math.PI / 180));
                    }
                    if (e.KeyCode == Keys.S)
                    {
                        glgraphics.cameraMoveX -= (float)(0.15f * Math.Cos(glgraphics.longitude * Math.PI / 180));
                        glgraphics.cameraMoveY -= (float)(0.15f * Math.Sin(glgraphics.longitude * Math.PI / 180));
                    }
                    if (e.KeyCode == Keys.A)
                    {
                        glgraphics.cameraMoveX -= (float)(0.15f * Math.Cos(67.5 + glgraphics.longitude * Math.PI / 180));
                        glgraphics.cameraMoveY -= (float)(0.15f * Math.Sin(67.5 + glgraphics.longitude * Math.PI / 180));
                    }
                    if (e.KeyCode == Keys.D)
                    {
                        glgraphics.cameraMoveX += (float)(0.15f * Math.Cos(67.5 + glgraphics.longitude * Math.PI / 180));
                        glgraphics.cameraMoveY += (float)(0.15f * Math.Sin(67.5 + glgraphics.longitude * Math.PI / 180));
                    }
                    if (glgraphics.cameraMoveX >= 10 && (glgraphics.cameraMoveY >= 3.5 || glgraphics.cameraMoveY <= 6.5) && !glgraphics.DoorIsClose)
                    {
                        glgraphics.position[0] = false;
                        glgraphics.position[1] = true;
                    }
                }
                else
                {
                    if (glgraphics.cameraMoveX <= 1.5)
                        glgraphics.cameraMoveX += 0.005f;
                    if (glgraphics.cameraMoveX >= 8.5)
                        glgraphics.cameraMoveX -= 0.005f;
                    if (glgraphics.cameraMoveY <= 1.5)
                        glgraphics.cameraMoveY += 0.005f;
                    if (glgraphics.cameraMoveY >= 8.5)
                        glgraphics.cameraMoveY -= 0.005f;
                }
            }
            //--------------------------------------------------------------ПЕРВЫЙ КОРИДОР----------------------------------------------------------------------------------
            if (glgraphics.position[1])
            {
                if ((glgraphics.cameraMoveY > 4.65 && glgraphics.cameraMoveY < 5.35))
                {
                    if (e.KeyCode == Keys.W)
                    {
                        glgraphics.cameraMoveX += (float)(0.15f * Math.Cos(glgraphics.longitude * Math.PI / 180));
                        glgraphics.cameraMoveY += (float)(0.15f * Math.Sin(glgraphics.longitude * Math.PI / 180));
                    }
                    if (e.KeyCode == Keys.S)
                    {
                        glgraphics.cameraMoveX -= (float)(0.15f * Math.Cos(glgraphics.longitude * Math.PI / 180));
                        glgraphics.cameraMoveY -= (float)(0.15f * Math.Sin(glgraphics.longitude * Math.PI / 180));
                    }
                    if (e.KeyCode == Keys.A)
                    {
                        glgraphics.cameraMoveX -= (float)(0.15f * Math.Cos(67.5 + glgraphics.longitude * Math.PI / 180));
                        glgraphics.cameraMoveY -= (float)(0.15f * Math.Sin(67.5 + glgraphics.longitude * Math.PI / 180));
                    }
                    if (e.KeyCode == Keys.D)
                    {
                        glgraphics.cameraMoveX += (float)(0.15f * Math.Cos(67.5 + glgraphics.longitude * Math.PI / 180));
                        glgraphics.cameraMoveY += (float)(0.15f * Math.Sin(67.5 + glgraphics.longitude * Math.PI / 180));
                    }
                    if (glgraphics.cameraMoveX < 10 && (glgraphics.cameraMoveY >= 3.5 || glgraphics.cameraMoveY <= 6.5) && !glgraphics.DoorIsClose)
                    {
                        glgraphics.position[0] = true;
                        glgraphics.position[1] = false;
                    }
                    if (glgraphics.cameraMoveX >= 30)
                    {
                        glgraphics.position[1] = false;
                        glgraphics.position[2] = true;
                    }
                }
                else
                {
                    if (glgraphics.cameraMoveY <= 4.75)
                        glgraphics.cameraMoveY += 0.005f;
                    if (glgraphics.cameraMoveY >= 5.25)
                        glgraphics.cameraMoveY -= 0.005f;
                }
            }
            //--------------------------------------------------------------ВТОРОЙ КОРИДОР----------------------------------------------------------------------------------
            if (glgraphics.position[2])
            {
                if ((glgraphics.cameraMoveX > 31.5 && glgraphics.cameraMoveX < 33.5 && glgraphics.cameraMoveY > 4.75 && glgraphics.cameraMoveY < 13.75) || (glgraphics.cameraMoveX <= 31.5 && glgraphics.cameraMoveY > 4.75 && glgraphics.cameraMoveY < 5.25) || (glgraphics.cameraMoveX >= 31 && glgraphics.cameraMoveX <= 34 && glgraphics.cameraMoveY >= 13 && glgraphics.cameraMoveY <= 17 && !glgraphics.DoorIsClose2))
                {
                    if (e.KeyCode == Keys.W)
                    {
                        glgraphics.cameraMoveX += (float)(0.15f * Math.Cos(glgraphics.longitude * Math.PI / 180));
                        glgraphics.cameraMoveY += (float)(0.15f * Math.Sin(glgraphics.longitude * Math.PI / 180));
                    }
                    if (e.KeyCode == Keys.S)
                    {
                        glgraphics.cameraMoveX -= (float)(0.15f * Math.Cos(glgraphics.longitude * Math.PI / 180));
                        glgraphics.cameraMoveY -= (float)(0.15f * Math.Sin(glgraphics.longitude * Math.PI / 180));
                    }
                    if (e.KeyCode == Keys.A)
                    {
                        glgraphics.cameraMoveX -= (float)(0.15f * Math.Cos(67.5 + glgraphics.longitude * Math.PI / 180));
                        glgraphics.cameraMoveY -= (float)(0.15f * Math.Sin(67.5 + glgraphics.longitude * Math.PI / 180));
                    }
                    if (e.KeyCode == Keys.D)
                    {
                        glgraphics.cameraMoveX += (float)(0.15f * Math.Cos(67.5 + glgraphics.longitude * Math.PI / 180));
                        glgraphics.cameraMoveY += (float)(0.15f * Math.Sin(67.5 + glgraphics.longitude * Math.PI / 180));
                    }
                    if (glgraphics.cameraMoveX <= 30 && (glgraphics.cameraMoveY >= 3.5 || glgraphics.cameraMoveY <= 6.5))
                    {
                        glgraphics.position[2] = false;
                        glgraphics.position[1] = true;
                    }
                    if (glgraphics.cameraMoveY> 15)
                    {
                        glgraphics.position[2] = false;
                        glgraphics.position[3] = true;
                    }
                }
                else
                {
                    if (glgraphics.cameraMoveX <= 31.5)
                        glgraphics.cameraMoveX += 0.005f;
                    if (glgraphics.cameraMoveX >= 33.5)
                        glgraphics.cameraMoveX -= 0.005f;
                    if (glgraphics.cameraMoveY <= 4.75)
                        glgraphics.cameraMoveY += 0.005f;
                    if (glgraphics.cameraMoveY >= 13.75)
                        glgraphics.cameraMoveY -= 0.005f;
                }
            }
            if (glgraphics.position[3])
            {
                if (e.KeyCode == Keys.W)
                {
                    glgraphics.cameraMoveX += (float)(0.15f * Math.Cos(glgraphics.longitude * Math.PI / 180));
                    glgraphics.cameraMoveY += (float)(0.15f * Math.Sin(glgraphics.longitude * Math.PI / 180));
                }
                if (e.KeyCode == Keys.S)
                {
                    glgraphics.cameraMoveX -= (float)(0.15f * Math.Cos(glgraphics.longitude * Math.PI / 180));
                    glgraphics.cameraMoveY -= (float)(0.15f * Math.Sin(glgraphics.longitude * Math.PI / 180));
                }
                if (e.KeyCode == Keys.A)
                {
                    glgraphics.cameraMoveX -= (float)(0.15f * Math.Cos(67.5 + glgraphics.longitude * Math.PI / 180));
                    glgraphics.cameraMoveY -= (float)(0.15f * Math.Sin(67.5 + glgraphics.longitude * Math.PI / 180));
                }
                if (e.KeyCode == Keys.D)
                {
                    glgraphics.cameraMoveX += (float)(0.15f * Math.Cos(67.5 + glgraphics.longitude * Math.PI / 180));
                    glgraphics.cameraMoveY += (float)(0.15f * Math.Sin(67.5 + glgraphics.longitude * Math.PI / 180));
                }
            }
            //----------------------ОТКРЫТИЕ ДВЕРИ-----------------------------------------
            if (e.KeyCode == Keys.E)
            {
                if (glgraphics.cameraMoveX >= 7.5 && glgraphics.cameraMoveX <= 12.5 && glgraphics.cameraMoveY >= 3.5 && glgraphics.cameraMoveY <= 6.5)
                {
                    if (glgraphics.DoorIsClose)
                        glgraphics.DoorIsClose = false;
                    else glgraphics.DoorIsClose = true;
                }
                if (glgraphics.cameraMoveX >=31 && glgraphics.cameraMoveX<=34 && glgraphics.cameraMoveY>=13 && glgraphics.cameraMoveY<=17)
                {
                    if (glgraphics.DoorIsClose2)
                        glgraphics.DoorIsClose2 = false;
                    else glgraphics.DoorIsClose2 = true;
                }
                if (glgraphics.cameraMoveX>=18 && glgraphics.cameraMoveX <=22 && glgraphics.cameraMoveY>=21 && glgraphics.cameraMoveY<=25)
                {
                    if (glgraphics.boxDX1<2)
                        glgraphics.boxDX1 += 0.1f;
                }
                if (glgraphics.cameraMoveX >= 38 && glgraphics.cameraMoveX <= 42 && glgraphics.cameraMoveY >= 21 && glgraphics.cameraMoveY <= 25)
                {
                    if (glgraphics.boxDX2 < 2)
                        glgraphics.boxDX2 += 0.1f;
                }
            }
            //----------------------ВЫХОД-----------------------------------------
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
            //----------------------РЕСТАРТ-----------------------------------------
            if (e.KeyCode == Keys.R)
            {
                glgraphics.cameraMoveX = 5;
                glgraphics.cameraMoveY = 5;
                glgraphics.DoorIsClose = true;
                glgraphics.DoorIsClose2 = true;
                glgraphics.boxDX1 = 0;
                glgraphics.boxDX2 = 0;
                glgraphics.foundNote = 0;
                for (int i = 0; i < 3; i++)
                    glgraphics.isFound[i] = false;
                for (int i = 1; i < 4; i++)
                    glgraphics.position[i] = false;
                glgraphics.position[0] = true;
            }
            //----------------------ВЗЯТЬ ЗАПИСКУ-----------------------------------------
            if (e.KeyCode == Keys.Q)
            {
                if (glgraphics.cameraMoveX >= 0 && glgraphics.cameraMoveX <= 2.5 && glgraphics.cameraMoveY >= 3 && glgraphics.cameraMoveY <= 5 && glgraphics.isFound[0] == false)
                {
                    glgraphics.isFound[0] = true;
                    glgraphics.foundNote++;
                }
                if (glgraphics.cameraMoveX >= 30 && glgraphics.cameraMoveX <= 32.5 && glgraphics.cameraMoveY >= 9 && glgraphics.cameraMoveY <= 11 && glgraphics.isFound[1] == false)
                {
                    glgraphics.isFound[1] = true;
                    glgraphics.foundNote++;
                }
                if (glgraphics.cameraMoveX >= 39 && glgraphics.cameraMoveX <= 42 && glgraphics.cameraMoveY >= 22 && glgraphics.cameraMoveY <= 25 && glgraphics.isFound[2] == false)
                {
                    glgraphics.isFound[2] = true;
                    glgraphics.foundNote++;
                }
            }
        }
    }
}
