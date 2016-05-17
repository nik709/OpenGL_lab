using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace OpenGL_lab
{
    class GLgraphics
    {
        public bool[] position = new bool[4];
        public bool[] isFound = new bool[3];
        public int foundNote = 0;
        public float latitude = 17.98f; //широта
        public float longitude = 10.41f; //долгота
        public float radius = 5.385f;
        public float cameraMoveX=5, cameraMoveY=5;
        public bool DoorIsClose = true;
        public bool DoorIsClose2 = true;
        public float boxDX1 = 0;
        public float boxDX2 = 0;
        Vector3 cameraPosition = new Vector3(0, 0, 0);
        Vector3 cameraDirection = new Vector3(0, 0, 0);
        Vector3 cameraUp = new Vector3(0, 0, 1);
        public List<int> texturesIDs = new List<int>();
        public float rotateAngle;
        public void Setup(int width, int height)
        {
            GL.ClearColor(Color.Black);
            GL.ShadeModel(ShadingModel.Smooth);
            GL.Enable(EnableCap.DepthTest);
            Matrix4 perspectiveMat = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, width / (float)height, 1, 64);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perspectiveMat);
            SetupLightnig();
        }
        public void Update()
        {
            rotateAngle += 0.5f;
            cameraPosition = new Vector3(cameraMoveX, cameraMoveY, 2.0f);
            cameraDirection = new Vector3(
                -(float)(radius * Math.Cos(Math.PI / 180.0f * latitude) * Math.Cos(Math.PI / 180.0f * longitude)) + cameraMoveX,
                -(float)(radius * Math.Cos(Math.PI / 180.0f * latitude) * Math.Sin(Math.PI / 180.0f * longitude)) + cameraMoveY,
                (float)(radius * Math.Sin(Math.PI / 180.0f * latitude)));
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            Matrix4 viewMat = Matrix4.LookAt(cameraPosition, cameraDirection, cameraUp);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref viewMat);
            Render();
        }
        public void drawTestQuard()
        {
            GL.Begin(PrimitiveType.Quads);
            GL.Color3(Color.Blue);
            GL.Vertex3(-10.0f, -10.0f, -1.0f);
            GL.Color3(Color.Red);
            GL.Vertex3(-10.0f, 10.0f, -1.0f);
            GL.Color3(Color.White);
            GL.Vertex3(10.0f, 10.0f, -1.0f);
            GL.Color3(Color.Green);
            GL.Vertex3(10.0f, -10.0f, -1.0f);
            GL.End();
        }
        public void drawTextureQuard(float x, float y, float z) //7,7,2
        {
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, texturesIDs[2]);
            GL.Begin(PrimitiveType.Quads);
            //GL.Color3(Color.Blue);
            GL.TexCoord2(1.0, 0.0);
            GL.Vertex3(-x, -y, -z);
            //GL.Color3(Color.Red);
            GL.TexCoord2(1.0, 1.0);
            GL.Vertex3(-x, y, -z);
            //GL.Color3(Color.White);
            GL.TexCoord2(0.0, 1.0);
            GL.Vertex3(x, y, -z);
            //GL.Color3(Color.Green);
            GL.TexCoord2(0.0, 0.0);
            GL.Vertex3(x, -y, -z);
            GL.End();
            GL.Disable(EnableCap.Texture2D);
        }
        public void Render()
        {
            DrawStartRoom();
            DrawFirstCorridor();
            DrawSecondCorridor();
            if (!isFound[0])
                DrawNote(0, 4, 2);
            if (!isFound[1])
                DrawNote(30, 10, 2);
            if (!isFound[2])
                DrawNote2(40.5f, 23.5f);
            DrawLastRoom();

            GL.PushMatrix();
            GL.Translate(boxDX2, 0, 0);
            DrawBox(40, 23);
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Translate(boxDX1, 0, 0);
            DrawBox(20, 23);
            GL.PopMatrix();
        }
        public int LoadTexture(String filePath)
        {
            try
            {
                Bitmap image = new Bitmap(filePath);
                int texID = GL.GenTexture();
                GL.BindTexture(TextureTarget.Texture2D, texID);
                System.Drawing.Imaging.BitmapData data = image.LockBits(
                    new System.Drawing.Rectangle(0, 0, image.Width, image.Height),
                    System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                GL.TexImage2D(TextureTarget.Texture2D, 0,
                    PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
                image.UnlockBits(data);
                GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
                return texID;
            }
            catch (System.IO.FileNotFoundException e)
            {
                return -1;
            }
        }
        public void SetupLightnig()
        {
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);
            GL.Enable(EnableCap.ColorMaterial);
            //положение источника света
            //GL.PushMatrix();
            //GL.LoadIdentity();
            //GL.Translate(-9, -9, 6);
            Vector4 lightPosition = new Vector4(1.0f, 1.0f, -4.0f, 0.0f);
            GL.Light(LightName.Light0, LightParameter.Position, lightPosition);
            //GL.PopMatrix();
            //ambient цвет источника – цвет, который будет иметь объект, не освещенный источником
            Vector4 ambientColor = new Vector4(0.2f, 0.2f, 0.2f, 1.0f);
            GL.Light(LightName.Light0, LightParameter.Ambient, ambientColor);
            //diffuse цвет источника – цвет, который будет иметь объект, освещенный источником
            Vector4 diffuseColor = new Vector4(0.6f, 0.6f, 0.6f, 1.0f);
            GL.Light(LightName.Light0, LightParameter.Diffuse, diffuseColor);
            //зеркальная состовляющая
            Vector4 materialSpecular = new Vector4(0.5f, 0.5f, 0.5f, 0.5f);
            GL.Material(MaterialFace.Front, MaterialParameter.Specular, materialSpecular);
            float materialShininess = 10;
            GL.Material(MaterialFace.Front, MaterialParameter.Shininess, materialShininess);
        }
        public void DrawStartRoom()
        {
            //--------------------------------------------------
            //ПОЛ
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, texturesIDs[1]);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0, 0);
            GL.Vertex3(0, 0, 0);
            GL.TexCoord2(0, 1);
            GL.Vertex3(0, 10, 0);
            GL.TexCoord2(1, 1);
            GL.Vertex3(10, 10, 0);
            GL.TexCoord2(1, 0);
            GL.Vertex3(10, 0, 0);
            GL.End();
            GL.Disable(EnableCap.Texture2D);
            //--------------------------------------------------
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, texturesIDs[0]);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0, 0);
            GL.Vertex3(0, 0, 0);
            GL.TexCoord2(0, 1);
            GL.Vertex3(0, 0, 5);
            GL.TexCoord2(1, 1);
            GL.Vertex3(10, 0, 5);
            GL.TexCoord2(1, 0);
            GL.Vertex3(10, 0, 0);
            GL.End();
            GL.Disable(EnableCap.Texture2D);
            //--------------------------------------------------
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, texturesIDs[0]);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0, 0);
            GL.Vertex3(0, 10, 0);
            GL.TexCoord2(0, 1);
            GL.Vertex3(0, 10, 5);
            GL.TexCoord2(1, 1);
            GL.Vertex3(10, 10, 5);
            GL.TexCoord2(1, 0);
            GL.Vertex3(10, 10, 0);
            GL.End();
            GL.Disable(EnableCap.Texture2D);
            //--------------------------------------------------
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, texturesIDs[0]);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0, 0);
            GL.Vertex3(0, 0, 0);
            GL.TexCoord2(1, 0);
            GL.Vertex3(0, 10, 0);
            GL.TexCoord2(1, 1);
            GL.Vertex3(0, 10, 5);
            GL.TexCoord2(0, 1);
            GL.Vertex3(0, 0, 5);
            GL.End();
            GL.Disable(EnableCap.Texture2D);
            //--------------------------------------------------
            //ПОТОЛОК
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, texturesIDs[2]);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0, 0);
            GL.Vertex3(0, 0, 5);
            GL.TexCoord2(0, 1);
            GL.Vertex3(0, 10, 5);
            GL.TexCoord2(1, 1);
            GL.Vertex3(10, 10, 5);
            GL.TexCoord2(1, 0);
            GL.Vertex3(10, 0, 5);
            GL.End();
            GL.Disable(EnableCap.Texture2D);
            //--------------------------------------------------
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, texturesIDs[0]);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0, 1);
            GL.Vertex3(10, 0, 0);
            GL.TexCoord2(3.5 / 10, 1);
            GL.Vertex3(10, 3.5, 0);
            GL.TexCoord2(3.5 / 10, 3 / 5);
            GL.Vertex3(10, 3.5, 3);
            GL.TexCoord2(0, 3 / 5);
            GL.Vertex3(10, 0, 3);
            GL.End();

            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0, 3 / 5);
            GL.Vertex3(10, 0, 3);
            GL.TexCoord2(1, 3 / 5);
            GL.Vertex3(10, 10, 3);
            GL.TexCoord2(1, 0);
            GL.Vertex3(10, 10, 5);
            GL.TexCoord2(0, 0);
            GL.Vertex3(10, 0, 5);
            GL.End();

            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(6.5 / 10, 1);
            GL.Vertex3(10, 6.5, 0);
            GL.TexCoord2(1, 1);
            GL.Vertex3(10, 10, 0);
            GL.TexCoord2(1, 3 / 5);
            GL.Vertex3(10, 10, 3);
            GL.TexCoord2(6.5 / 10, 3 / 5);
            GL.Vertex3(10, 6.5, 3);
            GL.End();
            GL.Disable(EnableCap.Texture2D);
            //--------------------------------------------------
            if (DoorIsClose)
            {
                GL.Enable(EnableCap.Texture2D);
                GL.BindTexture(TextureTarget.Texture2D, texturesIDs[3]);
                GL.Begin(PrimitiveType.Quads);
                GL.TexCoord2(0, 1);
                GL.Vertex3(10, 3.5, 0);
                GL.TexCoord2(1, 1);
                GL.Vertex3(10, 6.5, 0);
                GL.TexCoord2(1, 0);
                GL.Vertex3(10, 6.5, 3);
                GL.TexCoord2(0, 0);
                GL.Vertex3(10, 3.5, 3);
                GL.End();
                GL.Disable(EnableCap.Texture2D);
            }
            else 
            {
                GL.Enable(EnableCap.Texture2D);
                GL.BindTexture(TextureTarget.Texture2D, texturesIDs[3]);
                GL.Begin(PrimitiveType.Quads);
                GL.TexCoord2(0,1);
                GL.Vertex3(10, 6.5, 0);
                GL.TexCoord2(1,1);
                GL.Vertex3(7, 6.5, 0);
                GL.TexCoord2(1,0);
                GL.Vertex3(7, 6.5, 3);
                GL.TexCoord2(0,0);
                GL.Vertex3(10, 6.5, 3);
                GL.End();
                GL.Disable(EnableCap.Texture2D);
            }
        }
        public void DrawFirstCorridor()
        {
            //--------------------------------------------------
            //ЛЕВАЯ СТЕНА
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, texturesIDs[4]);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0, 1);
            GL.Vertex3(10, 6.5, 0);
            GL.TexCoord2(0, 0);
            GL.Vertex3(10, 6.5, 3);
            GL.TexCoord2(1, 0);
            GL.Vertex3(30, 6.5, 3);
            GL.TexCoord2(1, 1);
            GL.Vertex3(30, 6.5, 0);
            GL.End();
            GL.Disable(EnableCap.Texture2D);
            //--------------------------------------------------
            //ПРАВАЯ СТЕНА
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, texturesIDs[4]);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0, 1);
            GL.Vertex3(10, 3.5, 0);
            GL.TexCoord2(0, 0);
            GL.Vertex3(10, 3.5, 3);
            GL.TexCoord2(1, 0);
            GL.Vertex3(35, 3.5, 3);
            GL.TexCoord2(1, 1);
            GL.Vertex3(35, 3.5, 0);
            GL.End();
            GL.Disable(EnableCap.Texture2D);
            //--------------------------------------------------
            //КРАЫША
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, texturesIDs[4]);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0, 0);
            GL.Vertex3(10, 6.5, 3);
            GL.TexCoord2(0, 1);
            GL.Vertex3(30, 6.5, 3);
            GL.TexCoord2(1, 1);
            GL.Vertex3(30, 3.5, 3);
            GL.TexCoord2(1, 0);
            GL.Vertex3(10, 3.5, 3);
            GL.End();
            GL.Disable(EnableCap.Texture2D);
            //--------------------------------------------------
            //ПОТОЛОК
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, texturesIDs[4]);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0, 0);
            GL.Vertex3(10, 6.5, 0);
            GL.TexCoord2(0, 1);
            GL.Vertex3(30, 6.5, 0);
            GL.TexCoord2(1, 1);
            GL.Vertex3(30, 3.5, 0);
            GL.TexCoord2(1, 0);
            GL.Vertex3(10, 3.5, 0);
            GL.End();
            GL.Disable(EnableCap.Texture2D);
        }
        public void DrawSecondCorridor()
        {
            //--------------------------------------------------
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, texturesIDs[4]);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(1, 1);
            GL.Vertex3(35, 3.5, 0);
            GL.TexCoord2(1, 0);
            GL.Vertex3(35, 3.5, 3);
            GL.TexCoord2(0, 0);
            GL.Vertex3(35, 15, 3);
            GL.TexCoord2(0, 1);
            GL.Vertex3(35, 15, 0);
            GL.End();
            GL.Disable(EnableCap.Texture2D);
            //--------------------------------------------------
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, texturesIDs[4]);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0, 0);
            GL.Vertex3(30, 3.5, 0);
            GL.TexCoord2(1, 0);
            GL.Vertex3(35, 3.5, 0);
            GL.TexCoord2(1, 1);
            GL.Vertex3(35, 15, 0);
            GL.TexCoord2(0, 1);
            GL.Vertex3(30, 15, 0);
            GL.End();
            GL.Disable(EnableCap.Texture2D);
            //--------------------------------------------------
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, texturesIDs[4]);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0, 1);
            GL.Vertex3(30, 6.5, 0);
            GL.TexCoord2(0, 0);
            GL.Vertex3(30, 6.5, 3);
            GL.TexCoord2(1, 0);
            GL.Vertex3(30, 15, 3);
            GL.TexCoord2(1, 1);
            GL.Vertex3(30, 15, 0);
            GL.End();
            GL.Disable(EnableCap.Texture2D);
            //--------------------------------------------------
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, texturesIDs[4]);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0, 0);
            GL.Vertex3(30, 3.5, 3);
            GL.TexCoord2(1, 0);
            GL.Vertex3(35, 3.5, 3);
            GL.TexCoord2(1, 1);
            GL.Vertex3(35, 15, 3);
            GL.TexCoord2(0, 1);
            GL.Vertex3(30, 15, 3);
            GL.End();
            GL.Disable(EnableCap.Texture2D);
            //--------------------------------------------------
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, texturesIDs[4]);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0, 1);
            GL.Vertex3(30, 15, 0);
            GL.TexCoord2(0, 0);
            GL.Vertex3(30, 15, 3);
            GL.TexCoord2(1, 0);
            GL.Vertex3(31, 15, 3);
            GL.TexCoord2(1, 1);
            GL.Vertex3(31, 15, 0);
            GL.End();

            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(4 / 5, 1);
            GL.Vertex3(34, 15, 0);
            GL.TexCoord2(4 / 5, 0);
            GL.Vertex3(34, 15, 3);
            GL.TexCoord2(1, 0);
            GL.Vertex3(35, 15, 3);
            GL.TexCoord2(1, 1);
            GL.Vertex3(35, 15, 0);
            GL.End();
            GL.Disable(EnableCap.Texture2D);

            if (DoorIsClose2)
            {
                GL.Enable(EnableCap.Texture2D);
                GL.BindTexture(TextureTarget.Texture2D, texturesIDs[3]);
                GL.Begin(PrimitiveType.Quads);
                GL.TexCoord2(0, 1);
                GL.Vertex3(31, 15, 0);
                GL.TexCoord2(0, 0);
                GL.Vertex3(31, 15, 3);
                GL.TexCoord2(1, 0);
                GL.Vertex3(34, 15, 3);
                GL.TexCoord2(1, 1);
                GL.Vertex3(34, 15, 0);
                GL.End();
                GL.Disable(EnableCap.Texture2D);
            }
            else
            {
                GL.Enable(EnableCap.Texture2D);
                GL.BindTexture(TextureTarget.Texture2D, texturesIDs[3]);
                GL.Begin(PrimitiveType.Quads);
                GL.TexCoord2(0, 1);
                GL.Vertex3(31, 15, 0);
                GL.TexCoord2(0, 0);
                GL.Vertex3(31, 15, 3);
                GL.TexCoord2(1, 0);
                GL.Vertex3(31, 12, 3);
                GL.TexCoord2(1, 1);
                GL.Vertex3(31, 12, 0);
                GL.End();
                GL.Disable(EnableCap.Texture2D);
            }
        }
        public void DrawNote(int x, int y, int z)
        {
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, texturesIDs[5]);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0, 1);
            GL.Vertex3(x + 0.05f, y, z);
            GL.TexCoord2(1, 1);
            GL.Vertex3(x + 0.05f, y + 0.5, z);
            GL.TexCoord2(1, 0);
            GL.Vertex3(x + 0.05f, y + 0.5, z + 0.5);
            GL.TexCoord2(0, 0);
            GL.Vertex3(x + 0.05f, y, z + 0.5);
            GL.End();
            GL.Disable(EnableCap.Texture2D);
        }
        public void DrawLastRoom()
        {
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, texturesIDs[6]);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0, 1);
            GL.Vertex3(15, 15, 0);
            GL.TexCoord2(0, 0);
            GL.Vertex3(15, 30, 0);
            GL.TexCoord2(1, 0);
            GL.Vertex3(50, 30, 0);
            GL.TexCoord2(1, 1);
            GL.Vertex3(50, 15, 0);
            GL.End();
            GL.Disable(EnableCap.Texture2D);
        }
        public void DrawBox(int x, int y)
        {
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, texturesIDs[7]);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0,0);
            GL.Vertex3(x, y, 0);
            GL.TexCoord2(1,0);
            GL.Vertex3(x, y, 1.5);
            GL.TexCoord2(1,1);
            GL.Vertex3(x, y + 1.5, 1.5);
            GL.TexCoord2(0,1);
            GL.Vertex3(x, y + 1.5, 0);
            GL.End();
            GL.Disable(EnableCap.Texture2D);

            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, texturesIDs[7]);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0,0);
            GL.Vertex3(x, y+1.5, 0);
            GL.TexCoord2(1,0);
            GL.Vertex3(x, y+1.5, 1.5);
            GL.TexCoord2(1,1);
            GL.Vertex3(x+1.5, y + 1.5, 1.5);
            GL.TexCoord2(0,1);
            GL.Vertex3(x+1.5, y + 1.5, 0);
            GL.End();
            GL.Disable(EnableCap.Texture2D);

            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, texturesIDs[7]);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0,0);
            GL.Vertex3(x+1.5, y+1.5, 0);
            GL.TexCoord2(1,0);
            GL.Vertex3(x+1.5, y+1.5, 1.5);
            GL.TexCoord2(1,1);
            GL.Vertex3(x+1.5, y, 1.5);
            GL.TexCoord2(0,1);
            GL.Vertex3(x+1.5, y, 0);
            GL.End();
            GL.Disable(EnableCap.Texture2D);

            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, texturesIDs[7]);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0,0);
            GL.Vertex3(x+1.5, y, 0);
            GL.TexCoord2(1,0);
            GL.Vertex3(x+1.5, y, 1.5);
            GL.TexCoord2(1,1);
            GL.Vertex3(x, y, 1.5);
            GL.TexCoord2(0,1);
            GL.Vertex3(x, y, 0);
            GL.End();
            GL.Disable(EnableCap.Texture2D);

            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, texturesIDs[7]);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0, 0);
            GL.Vertex3(x, y, 1.5);
            GL.TexCoord2(1, 0);
            GL.Vertex3(x, y+1.5, 1.5);
            GL.TexCoord2(1, 1);
            GL.Vertex3(x+1.5, y+1.5, 1.5);
            GL.TexCoord2(0, 1);
            GL.Vertex3(x+1.5, y, 1.5);
            GL.End();
            GL.Disable(EnableCap.Texture2D);
        }
        public void DrawNote2(float x, float y)
        {
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, texturesIDs[5]);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(1, 0);
            GL.Vertex3(x, y, 0.05f);
            GL.TexCoord2(1, 1);
            GL.Vertex3(x, y + 0.5, 0.05f);
            GL.TexCoord2(0, 1);
            GL.Vertex3(x + 0.5, y + 0.5, 0.05f);
            GL.TexCoord2(0, 0);
            GL.Vertex3(x+0.5, y, 0.05f);
            GL.End();
            GL.Disable(EnableCap.Texture2D);
        }
    }
}
