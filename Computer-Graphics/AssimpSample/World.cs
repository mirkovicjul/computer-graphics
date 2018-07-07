// -----------------------------------------------------------------------
// <file>World.cs</file>
// <copyright>Grupa za Grafiku, Interakciju i Multimediju 2013.</copyright>
// <author>Srđan Mihić</author>
// <author>Aleksandar Josić</author>
// <summary>Klasa koja enkapsulira OpenGL programski kod.</summary>
// -----------------------------------------------------------------------


using System;
using Assimp;
using System.IO;
using System.Reflection;
using SharpGL.SceneGraph;
using SharpGL.SceneGraph.Primitives;
using SharpGL.SceneGraph.Quadrics;
using SharpGL.SceneGraph.Core;
using SharpGL;
using System.Collections.Generic;
using System.Drawing;
using System.Timers;
using System.Windows.Threading;
using System.Drawing.Imaging;




namespace AssimpSample
{


    /// <summary>
    ///  Klasa enkapsulira OpenGL kod i omogucava njegovo iscrtavanje i azuriranje.
    /// </summary>
    public class World : IDisposable
    {
        #region Atributi

        private float fsx = 1;
       

        private float fs = 20;

        private float visina = 350;

        private float[] lightPointPos = { -420.0f, 350.0f, 80.0f, 1.0f };

        private float[] lightRefPos = { -250.0f, 350.0f, 0.0f, 1.0f };

        private float[] truckPos = { 250.0f, 0.0f, 200.0f };
        private float[] contPos = { -250.0f, 0.0f, 0.0f };

        private float[] contRotate1 = { 0.0f, 0.0f, 0.0f };
        private float[] contRotate2 = { 0.0f, 0.0f, 0.0f };
        private float[] contRotate3 = { 0.0f, 0.0f, 0.0f };

        private float brzina = (float)0.5;

        private enum TextureObjects { Asphalt = 0, Metal, Empty, Brick, Lamp, Cont };
        private readonly int m_textureCount = Enum.GetNames(typeof(TextureObjects)).Length;

        private uint[] m_textures = null;

        private string[] m_textureFiles = { "..//..//3D Models//Truck//as.jpg", "..//..//3D Models//Truck//metal.jpg", "..//..//3D Models//Truck//empty.png", "..//..//3D Models//Truck//brick.jpg", "..//..//3D Models//Truck//lamp.jpg", "..//..//3D Models//Truck//kont.jpg" };

        /// <summary>
        ///	 Scena koja se prikazuje.
        /// </summary>
        private AssimpScene m_sceneCont;

        private AssimpScene m_sceneTruck;

        /// <summary>
        ///	 Ugao rotacije sveta oko X ose.
        /// </summary>
        private float m_xRotation = 15.0f;//15.0f;

        /// <summary>
        ///	 Ugao rotacije sveta oko Y ose.
        /// </summary>
        private float m_yRotation = 0.0f; //-55.0f;

        /// <summary>
        ///	 Udaljenost scene od kamere.
        /// </summary>
        private float m_sceneDistance = 1200.0f; //450.0f;

        /// <summary>
        ///	 Sirina OpenGL kontrole u pikselima.
        /// </summary>
        private int m_width;

        /// <summary>
        ///	 Visina OpenGL kontrole u pikselima.
        /// </summary>
        private int m_height;

        private Boolean flag = true;

        public OpenGL ogl;

        private DispatcherTimer timer;

        List<string> text = new List<string>() { "Predmet: Racunarska grafika", "Sk.god: 2017/18.", "Ime: Julija", "Prezime: Mirkovic", "Sifra zad: 1.1" };

        #endregion Atributi

        #region Properties


        public float[] getSetLightPointPos
        {
            get { return lightPointPos; }
            set { lightPointPos = value; }
        }

        public Boolean getSetFlag
        {
            get { return flag; }
            set { flag = value; }
        }

        public float getSetBrzina
        {
            get { return brzina; }
            set { brzina = value; }
        }
        /// <summary>
        ///	 Scena koja se prikazuje.
        /// </summary>
        public AssimpScene Scene
        {
            get { return m_sceneCont; }
            set { m_sceneCont = value; }
        }

        public AssimpScene SceneTruck
        {
            get { return m_sceneTruck; }
            set { m_sceneTruck = value; }
        }

        public float getSetVisina
        {
            get { return visina; }
            set { visina = value; }
        }

        public float getSetfsx 
        {
            get { return fsx; }
            set { fsx = value; }
        }

        public float getSetfs
        {
            get { return fs; }
            set { fs = value; }
        }
        /// <summary>
        ///	 Ugao rotacije sveta oko X ose.
        /// </summary>
        public float RotationX
        {
            get { return m_xRotation; }
            set { m_xRotation = value; }
        }

        /// <summary>
        ///	 Ugao rotacije sveta oko Y ose.
        /// </summary>
        public float RotationY
        {
            get { return m_yRotation; }
            set { m_yRotation = value; }
        }

        /// <summary>
        ///	 Udaljenost scene od kamere.
        /// </summary>
        public float SceneDistance
        {
            get { return m_sceneDistance; }
            set { m_sceneDistance = value; }
        }

        /// <summary>
        ///	 Sirina OpenGL kontrole u pikselima.
        /// </summary>
        public int Width
        {
            get { return m_width; }
            set { m_width = value; }
        }

        /// <summary>
        ///	 Visina OpenGL kontrole u pikselima.
        /// </summary>
        public int Height
        {
            get { return m_height; }
            set { m_height = value; }
        }


        public Boolean animacija
        {
            get { return flag; }
            set { flag = value; }
        }

      

        #endregion Properties

        #region Konstruktori

        /// <summary>
        ///  Konstruktor klase World.
        /// </summary>
       
        public World(String scenePath, String sceneFileNameCont, String sceneFileNameTruck, int width, int height, OpenGL gl)
        {
            this.m_sceneCont = new AssimpScene(scenePath, sceneFileNameCont, gl);
            this.m_sceneTruck = new AssimpScene(scenePath, sceneFileNameTruck, gl);
            this.m_width = width;
            this.m_height = height;
            m_textures = new uint[m_textureCount];
        }

        /// <summary>
        ///  Destruktor klase World.
        /// </summary>
        ~World()
        {
            this.Dispose(false);
        }

        #endregion Konstruktori

        #region Metode

        /// <summary>
        ///  Korisnicka inicijalizacija i podesavanje OpenGL parametara.
        /// </summary>
        public void Initialize(OpenGL gl)
        {
            ogl = gl;
            gl.ClearColor(0.0f,0.0f, 0.0f, 1.0f);
            //gl.Color(0f, 1f, 0f);
            // Model sencenja na flat (konstantno)
            gl.ShadeModel(OpenGL.GL_SMOOTH);
            gl.Enable(OpenGL.GL_DEPTH_TEST);
            gl.Enable(OpenGL.GL_CULL_FACE);
            
            gl.Enable(OpenGL.GL_NORMALIZE);
            gl.Enable(OpenGL.GL_COLOR_MATERIAL);
            gl.ColorMaterial(OpenGL.GL_FRONT, OpenGL.GL_AMBIENT_AND_DIFFUSE);



            // Teksture se primenjuju sa parametrom decal
            gl.Enable(OpenGL.GL_TEXTURE_2D);
            gl.TexEnv(OpenGL.GL_TEXTURE_ENV, OpenGL.GL_TEXTURE_ENV_MODE, OpenGL.GL_ADD);

            // Ucitaj slike i kreiraj teksture
            gl.GenTextures(m_textureCount, m_textures);
            for (int i = 0; i < m_textureCount; ++i)
            {
                // Pridruzi teksturu odgovarajucem identifikatoru
                gl.BindTexture(OpenGL.GL_TEXTURE_2D, m_textures[i]);

                // Ucitaj sliku i podesi parametre teksture
                Bitmap image = new Bitmap(m_textureFiles[i]);
                // rotiramo sliku zbog koordinantog sistema opengl-a
                image.RotateFlip(RotateFlipType.RotateNoneFlipY);
                Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);
                // RGBA format (dozvoljena providnost slike tj. alfa kanal)
                BitmapData imageData = image.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly,
                                                      System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                gl.Build2DMipmaps(OpenGL.GL_TEXTURE_2D, (int)OpenGL.GL_RGBA8, image.Width, image.Height, OpenGL.GL_BGRA, OpenGL.GL_UNSIGNED_BYTE, imageData.Scan0);
                gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MIN_FILTER, OpenGL.GL_LINEAR_MIPMAP_LINEAR);
                gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MAG_FILTER, OpenGL.GL_LINEAR_MIPMAP_LINEAR);

                gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_S, OpenGL.GL_REPEAT);
                gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_T, OpenGL.GL_REPEAT);

                gl.Enable(OpenGL.GL_TEXTURE_GEN_S);
                gl.Enable(OpenGL.GL_TEXTURE_GEN_T);

                image.UnlockBits(imageData);
                image.Dispose();
            }

            m_sceneCont.LoadScene();
            m_sceneCont.Initialize();
            m_sceneTruck.LoadScene();
            m_sceneTruck.Initialize();
        }

        public void Setup_Lighting(OpenGL gl)
        {

            float[] global_ambient = new float[] { 0.2f, 0.2f, 0.2f, 1.0f };
            //gl.LightModel(OpenGL.GL_LIGHT_MODEL_AMBIENT, global_ambient);

            float[] ambMaterial = { 0.11f, 0.06f, 0.11f, 1.0f };
            float[] difMaterial = { 0.43f, 0.47f, 0.54f, 1.0f };
            float[] spcMaterial = { 0.67f, 0.56f, 0.52f, 1.0f };
            float[] emMaterial = { 0.0f, 0.0f, 0.0f, 0.0f };
            float shMaterial = 10;

            gl.Material(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_AMBIENT, ambMaterial);
            gl.Material(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_DIFFUSE, difMaterial);
            gl.Material(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_SPECULAR, spcMaterial);
            gl.Material(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_EMISSION, emMaterial);
            gl.Material(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_SHININESS, shMaterial);


            float[] ambLight = { 0.3f, 0.3f, 0.3f, 1.0f };
            float[] difLight = { 0.6f, 0.6f, 0.6f, 1.0f };
            float[] spcLight = { 1.0f, 1.0f, 1.0f, 1.0f };

            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_AMBIENT, ambLight);
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_DIFFUSE, difLight);
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_SPECULAR, spcLight);

            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_SPOT_CUTOFF, 180.0f);
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_POSITION, lightPointPos);

            gl.Enable(OpenGL.GL_LIGHTING);
            gl.Enable(OpenGL.GL_LIGHT0);



            //reflektorski svjetlosni izvor
            //float[] ambLightR = { 0.3f, 0.3f, 0.0f, 1.0f };
            float[] difLightR = { 0.7f, 0.7f, 0.0f, 1.0f };
            float[] spcLightR = { 1.0f, 1.0f, 0.0f, 1.0f };
            float[] direction = { 0.0f, -1.0f, 0.0f };
            //gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_AMBIENT, ambLightR);
            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_DIFFUSE, difLightR);
            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_SPECULAR, spcLightR);

            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_SPOT_DIRECTION, direction);
            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_SPOT_CUTOFF, 35.0f);
           
            
            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_POSITION, lightRefPos);
            
            gl.Enable(OpenGL.GL_LIGHT1);
        }
      
        /// <summary>
        ///  Iscrtavanje OpenGL kontrole.
        /// </summary>
        public void Draw(OpenGL gl)
        {
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            gl.MatrixMode(OpenGL.GL_PROJECTION);
            gl.LoadIdentity();
            gl.Perspective(45f, (double)m_width / m_height, 1f, 20000f);
            gl.LookAt(0.0f, 10.0f, 100.0f, 0.0f, 0.0f, 0.0f, 0.0f, 350.0f, 0.0f);
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
            gl.PushMatrix();

            gl.Translate(0.0f, 0.0f, -m_sceneDistance);
            gl.Rotate(m_xRotation, 1.0f, 0.0f, 0.0f);
            gl.Rotate(m_yRotation, 0.0f, 1.0f, 0.0f);

            Setup_Lighting(gl);

            //kontejner
            gl.PushMatrix();
            gl.BindTexture(OpenGL.GL_TEXTURE_2D, m_textures[(int)TextureObjects.Empty]);
            gl.Translate(contPos[0], contPos[1], contPos[2]);
            gl.Rotate(contRotate1[0], contRotate1[1], contRotate1[2]);
            gl.Rotate(contRotate2[0], contRotate2[1], contRotate2[2]);
            gl.Rotate(contRotate3[0], contRotate3[1], contRotate3[2]);
            m_sceneCont.Draw();
            gl.PopMatrix();

            //kamion        
            gl.PushMatrix();
            gl.BindTexture(OpenGL.GL_TEXTURE_2D, m_textures[(int)TextureObjects.Empty]);
            gl.Translate(truckPos[0], truckPos[1], truckPos[2]);
            gl.Rotate(0, 90, 0);
            m_sceneTruck.Draw();
            gl.PopMatrix();
            
            //podloga od quada
            gl.PushMatrix();
            gl.BindTexture(OpenGL.GL_TEXTURE_2D, m_textures[(int)TextureObjects.Asphalt]);
            gl.MatrixMode(OpenGL.GL_TEXTURE);
            gl.LoadIdentity();
            gl.Scale(0.02f, 0.02f, 0.02f);
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
            gl.Rotate(-90, 0, 0);
            gl.Begin(OpenGL.GL_QUADS);
            gl.Normal(LightingUtilities.FindFaceNormal(-550.0f, -550.0f, 0, 550.0f, -550.0f, 0, 550.0f, 550.0f,0));
            gl.TexCoord(0.0f, 0.0f);
            gl.Vertex(-550.0f, -550.0f, 0);
            gl.TexCoord(0.0f, 10.0f);
            gl.Vertex(550.0f, -550.0f, 0);
            gl.TexCoord(15.0f, 15.0f);
            gl.Vertex(550.0f, 550.0f, 0);
            gl.TexCoord(15.0f, 0.0f);
            gl.Vertex(-550.0f, 550.0f, 0);
            gl.End();
            gl.PopMatrix();

            //desni zid           
            gl.PushMatrix();            
            gl.Color(0.7f, 0.0f, 0.0f); 
            gl.Translate(-110, 60, 0);
            gl.Scale(10f, 60f, 70f);
            
            gl.BindTexture(OpenGL.GL_TEXTURE_2D, m_textures[(int)TextureObjects.Empty]);
            Cube zd = new Cube();
            zd.Render(gl, RenderMode.Render);
            gl.PopMatrix();

            //zid iza
            gl.PushMatrix();
            //gl.Color(0.7f, 0.0f, 0.0f);   
            gl.Translate(-250, 60, -80);
            gl.Rotate(0, 90, 0);
            gl.Scale(10f, 60f, 150f);
            gl.Enable(OpenGL.GL_NORMALIZE);
            Cube zi = new Cube();
            zi.Render(gl, RenderMode.Render);
            gl.PopMatrix();

            //zid lijevo
            gl.PushMatrix();
            //gl.Color(0.7f, 0.0f, 0.0f);
            gl.Translate(-390, 60, 0);
            gl.Scale(10f, 60f, 70f);
            Cube zl = new Cube();
            zl.Render(gl, RenderMode.Render);
            gl.PopMatrix();

            //cube svjetlo  
            gl.PushMatrix();  
            gl.Color(1f, 1f, 1f);            
            gl.Translate(lightPointPos[0], lightPointPos[1], lightPointPos[2]);
            gl.Scale(fs, fs, fs);
            //gl.TexEnv(OpenGL.GL_TEXTURE_ENV, OpenGL.GL_TEXTURE_ENV_MODE, OpenGL.GL_ADD);
            gl.BindTexture(OpenGL.GL_TEXTURE_2D, m_textures[(int)TextureObjects.Empty]);            
            Cube sijalica = new Cube();
            sijalica.Render(gl, SharpGL.SceneGraph.Core.RenderMode.Render);
            gl.PopMatrix();


            //bandera
            gl.PushMatrix();
            //gl.Color(0.0f, 0.0f, 0.0f);             
            gl.Translate(-420, 0, 80);
            gl.Rotate(-90, 0, 0);
            gl.Scale(fsx, fsx, fsx);
                   
            gl.BindTexture(OpenGL.GL_TEXTURE_2D, m_textures[(int)TextureObjects.Metal]);
            gl.TexEnv(OpenGL.GL_TEXTURE_ENV, OpenGL.GL_TEXTURE_ENV_MODE, OpenGL.GL_MODULATE);
            
            Cylinder st = new Cylinder();
            st.Height = lightPointPos[1];
            st.BaseRadius = 7f;
            st.TopRadius = 7f;
            st.CreateInContext(gl);
            st.Render(gl, SharpGL.SceneGraph.Core.RenderMode.Render);
            gl.PopMatrix();


            TextDraw3D(gl);

            gl.PopMatrix();
            gl.Flush();
        }


        public void zapocniAnimaciju()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1);
            timer.Tick += new EventHandler(Animacija);
            timer.Start();
        }

        public void Animacija(object sender, EventArgs e)
        {


            if (truckPos[0] >= 75.0f && contRotate1[1] <= 0.0f)
            {
                truckPos[0] -= 2.0f; 
            }

            if (truckPos[0] <= 75.0f && contPos[2] <= 200)
            {
                contPos[2] += 2.0f;
            }

            if (contPos[2] >= 200 && contRotate1[1] <= 90)
            {
                contRotate1[1] += 1;
            }

            if (contRotate1[1] >= 90 && contPos[1] <= 200 && contRotate2[0] == 0)
            {
                contPos[1] += 2;
            }

            if (contPos[1] >= 200 && contPos[0] <= -130 && contRotate2[0] <= 90)
            {
                contPos[0] += 2;
            }

            if (contPos[0] >= -130 && contRotate2[0] <= 90)
            {
                contRotate2[0] += brzina;
            }

            if (contRotate2[0] >= 90 && contRotate3[0] >= -90)
            {
                contRotate3[0] -= brzina;
            }

            if (contRotate3[0] <= -90.0f && contPos[0] >= -250.0f)
            {
                contPos[0] -= 2;
            }

            if (contPos[0] <= -250 && contPos[1] >= 0 && contRotate3[0] <= -90)
            {
                contPos[1] -= 2;
            }

            if (contPos[1] <= 0 && contRotate3[0] <= -90 && truckPos[0] <= 300)
            {
                truckPos[0] += 2;
            }

            if (truckPos[0] >= 300)
            {
                getSetFlag = true;
                timer.Stop();

                truckPos = new float[] { 250.0f, 0.0f, 200.0f };
                contPos = new float[] { -250.0f, 0.0f, 0.0f };

                contRotate1 = new float[] { 0.0f, 0.0f, 0.0f };
                contRotate2 = new float[] { 0.0f, 0.0f, 0.0f };
                contRotate3 = new float[] { 0.0f, 0.0f, 0.0f };

            }
        }

        public void TextDraw3D(OpenGL gl) {
            
            float x = 2.0f;
            float y = -10f;
            float z = 1200.5f;
            float d = 0f;
           
            gl.MatrixMode(OpenGL.GL_PROJECTION);
            gl.LoadIdentity();
            gl.Ortho2D(-15.0f, 15.0f, -15.0f, 15.0f);
            gl.MatrixMode(OpenGL.GL_MODELVIEW);

            gl.PushMatrix();
            gl.Color(1f, 1f, 0f);
            
            foreach (string s in text)
            {
                gl.PushMatrix();
                gl.Translate(x, y - d, z);
                gl.DrawText3D("Helvetica bold", 14f, 0f, 0f, s);
                gl.PopMatrix();
                d++;
            }
            gl.PopMatrix();
        }

       
        /// <summary>
        /// Podesava viewport i projekciju za OpenGL kontrolu.
        /// </summary>
        public void Resize(OpenGL gl, int width, int height)
        {
            m_width = width;
            m_height = height;
            gl.MatrixMode(OpenGL.GL_PROJECTION);      // selektuj Projection Matrix
            gl.LoadIdentity();
            gl.Perspective(45f, (double)width / height, 1f, 20000f);
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
            gl.LoadIdentity();                // resetuj ModelView Matrix
       

        }

        /// <summary>
        ///  Implementacija IDisposable interfejsa.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                m_sceneCont.Dispose();
            }
        }

        #endregion Metode

        #region IDisposable metode

        /// <summary>
        ///  Dispose metoda.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable metode
    }
}
