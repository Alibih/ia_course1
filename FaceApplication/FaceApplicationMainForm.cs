using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommunicationLibrary;
using CustomUserControlsLibrary;
using ObjectSerializerLibrary;
using ThreeDimensionalVisualizationLibrary;
using ThreeDimensionalVisualizationLibrary.Faces;
using ThreeDimensionalVisualizationLibrary.Objects;
using OpenTK.Graphics.OpenGL;
using FaceApplication.AddedClasses;

namespace AgentApplication
{
    // =======================================================
    //
    // Note to students: Unlike most other code in the IPASrc
    // code collection, this simple demonstration application
    // contains a lot of ugly parameter hard-coding. 
    // However, the parameters in question concern the detailed
    // shape of the agent's facial features (e.g. eyes, eyebrows etc.)
    // which would normally be defined in a separate editor and
    // then simply be loaded into the application. Here, only
    // the skull (i.e. excluding eyes etc.) is loaded from file
    // since the FaceEditor does not yet have the capability of
    // generating eyes and other features.

    // The application is meant as a simple demonstration of
    // 3D visualization and animation. In your own code,
    // make sure that you parameterize constants properly.
    //
    // ========================================================


    public partial class FaceApplicationMainForm : Form
    {
        private const string DATETIME_FORMAT = "yyyyMMdd HH:mm:ss";
        private const string CLIENT_NAME = "Face";
        private const string DEFAULT_IP_ADDRESS = "127.0.0.1";
        private const int DEFAULT_PORT = 7;
        private const string DEFAULT_RELATIVE_FACE_FILE_NAME = "\\..\\..\\..\\Data\\Face.xml";

        private const string FROG_RELATIVE_FACE_FILE_NAME = "\\..\\..\\..\\Data\\froghead.xml";
        private const string FROG_RELATIVE_EYE_FILE_NAME = "\\..\\..\\..\\Data\\frogeye.xml";

        private const string FROG_RELATIVE_MOUTH_FILE_NAME = "\\..\\..\\..\\Data\\frogmouth.xml";

        private const double DEFAULT_SECOND_BLINK_PROBABILITY = 0.3;

        private string ipAddress = DEFAULT_IP_ADDRESS;
        private int port = DEFAULT_PORT;
        private Client client = null;

        private Thread blinkThread = null;
        private Thread squintThread = null;
        private Thread rotateLeftThread = null;
        private Thread rotateHeadThread = null;
        private Thread nodThread = null;
        private Thread tiltThread = null;
        private Thread openEyesThread = null;
        private Thread shakeThread = null;

        private System.Boolean normalThreadRunning = false;
        private Thread normalThread = null; // Handles normal face actions, e.g. blinking.

        private System.Boolean blinking = false;
        private System.Boolean eyesClosed = false;
        private System.Boolean nodding = false;
        private System.Boolean shaking = false;
        private System.Boolean squinting = false;

        private Random randomNumberGenerator;
        private double secondBlinkProbability = DEFAULT_SECOND_BLINK_PROBABILITY;

        private Face face;

        public FaceApplicationMainForm()
        {
            InitializeComponent();
            Initialize();
            Connect();
        }

        private void Connect()
        {
            client = new Client();
            client.Received += new EventHandler<DataPacketEventArgs>(HandleClientReceived);
            client.Progress += new EventHandler<CommunicationProgressEventArgs>(HandleClientProgress);
            client.Name = CLIENT_NAME;
            client.Connect(ipAddress, port);
        }

        private void HandleClientReceived(object sender, DataPacketEventArgs e)
        {
            string faceAction = e.DataPacket.Message;
            if (faceAction.ToLower() == "openeyes") { OpenEyes(); }
            else if (faceAction.ToLower() == "squint") { Squint(); }
            else if (faceAction.ToLower() == "nodhead") { Nod();}
            else if (faceAction.ToLower() == "shakehead"){ShakeHead();}
            // ToDO: Add more actions here

        }

        private void HandleClientProgress(object sender, CommunicationProgressEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => ShowProgress(e)));
            }
            else { ShowProgress(e); }
        }

        private void ShowProgress(CommunicationProgressEventArgs e)
        {
            ColorListBoxItem item;
            item = new ColorListBoxItem(e.Message, communicationLogListBox.BackColor, communicationLogListBox.ForeColor);
            communicationLogListBox.Items.Insert(0, item);
        }

        private void Initialize()
        {
            randomNumberGenerator = new Random();
            string faceFileName = Path.GetDirectoryName(Application.ExecutablePath) + FROG_RELATIVE_FACE_FILE_NAME;
            string mouthFileName = Path.GetDirectoryName(Application.ExecutablePath) + FROG_RELATIVE_MOUTH_FILE_NAME;
            string eyeFileName = Path.GetDirectoryName(Application.ExecutablePath) + FROG_RELATIVE_EYE_FILE_NAME;
            if (File.Exists(faceFileName))
            {
                face = (Face)ObjectSerializerLibrary.ObjectXmlSerializer.ObtainSerializedObject(faceFileName, typeof(Face));
                face.Name = "Face";
                face.Generate(new List<double> {128});
                face.Visible = true; 
                face.SpecularColor = Color.White;
                face.Shininess = 50;
                face.AmbientColor = Color.Green;
                // A lot of ugly hard-coding here, but OK - just for demonstration:
                face.Object3DList = new List<Object3D>();

                SphereSegment3D mouth = new SphereSegment3D();
                mouth.Name = "Mouth";
                mouth.Generate(new List<double>() { 0.85, 16, 16, 0.3 });
                mouth.AmbientColor = Color.White;  
                mouth.DiffuseColor = Color.White; 
                mouth.SpecularColor = Color.White;
                mouth.RotateX(180);
                mouth.Shininess = 50;
                mouth.ShowSurfaces = true;
                //mouth.RotateX(-36);
                mouth.Move(0, -0.1f, 0.85f);
                face.Object3DList.Add(mouth);




                #region generateLeftEye
                Sphere3D leftEyeBulb = new Sphere3D();
                leftEyeBulb.Generate(new List<double>() { 0.20, 64, 64 });
                leftEyeBulb.AmbientColor = Color.Yellow;
                leftEyeBulb.DiffuseColor = Color.White;
                leftEyeBulb.SpecularColor = Color.White;
                leftEyeBulb.Shininess = 50;
                leftEyeBulb.ShowSurfaces = true;
                // x z y
                leftEyeBulb.Move(-0.60, 0.34, 0.29);
                leftEyeBulb.ShadingModel = ShadingModel.Smooth;
                leftEyeBulb.UseLight = true;
                leftEyeBulb.Name = "LeftEye";
                leftEyeBulb.Object3DList = new List<Object3D>();

                
                SphereSegment3D leftEyePupil = new SphereSegment3D();
                leftEyePupil.Generate(new List<double>() { 0.25, 16, 32, Math.PI / 2 - 0.2 });
                leftEyePupil.AmbientColor = Color.Black;
                leftEyePupil.DiffuseColor = Color.Black;
                leftEyePupil.SpecularColor = Color.White;
                leftEyePupil.Shininess = 50;
                leftEyePupil.ShowSurfaces = true;
                leftEyePupil.Move(0, 0.05, 0);
                leftEyePupil.RotateX(90); // , 0, 0);
                                          //   leftEyePupil.RotateX(Math.PI / 2);
                                          //  leftEyePupil.Translate(-0.1375, -0.4758, 0.0725);
                leftEyePupil.ShadingModel = ShadingModel.Smooth;
                leftEyePupil.UseLight = true;


                SphereSegment3D leftEyePupilExtended = new SphereSegment3D();
                leftEyePupilExtended.Generate(new List<double>() { 0.25, 16, 32, Math.PI / 2 - 0.2 });
                leftEyePupilExtended.AmbientColor = Color.Black;
                leftEyePupilExtended.DiffuseColor = Color.Black;
                leftEyePupilExtended.SpecularColor = Color.White;
                leftEyePupilExtended.Shininess = 50;
                leftEyePupilExtended.ShowSurfaces = true;
                leftEyePupilExtended.Move(0, 0.05, 0);
                leftEyePupilExtended.RotateZ(13);
                leftEyePupilExtended.RotateX(87); // , 0, 0);
                                          //   leftEyePupilExtended.RotateX(Math.PI / 2);
                                          //  leftEyePupilExtended.Translate(-0.1375, -0.4758, 0.0725);
                leftEyePupilExtended.ShadingModel = ShadingModel.Smooth;
                leftEyePupilExtended.UseLight = true;


                SphereSegment3D leftEyeLid = new SphereSegment3D();
                leftEyeLid.Name = "LeftEyeLid";
                leftEyeLid.Generate(new List<double>() { 0.21, 32, 32, 0.0 });
                leftEyeLid.AmbientColor = Color.Green;  // Typical skin color approximation
                leftEyeLid.DiffuseColor = Color.Green;  // Typical skin color approximation
                leftEyeLid.SpecularColor = Color.White;
                leftEyeLid.Shininess = 50;
                leftEyeLid.ShowSurfaces = true;
                leftEyeLid.RotateX(0);
                leftEyeLid.Move(0, 0, 0);
                leftEyeBulb.Object3DList.Add(leftEyePupil);
                leftEyeBulb.Object3DList.Add(leftEyePupilExtended);
                leftEyeBulb.Object3DList.Add(leftEyeLid);
                face.Object3DList.Add(leftEyeBulb);

                
                SphereSegment3D lowerLeftEyeLid = new SphereSegment3D();
                lowerLeftEyeLid.Name = "LowerLeftEyeLid";
                lowerLeftEyeLid.Generate(new List<double>() { 0.21, 32, 32, 0.0 });
                lowerLeftEyeLid.AmbientColor = Color.Green;  // Typical skin color approximation
                lowerLeftEyeLid.DiffuseColor = Color.Green;  // Typical skin color approximation
                lowerLeftEyeLid.SpecularColor = Color.White;
                lowerLeftEyeLid.Shininess = 50;
                lowerLeftEyeLid.ShowSurfaces = true;
                lowerLeftEyeLid.RotateX(0);
                lowerLeftEyeLid.RotateY(180);
                lowerLeftEyeLid.Move(0, 0, 0);
                leftEyeBulb.Object3DList.Add(lowerLeftEyeLid);
                
                //face.Object3DList.Add(leftEyeBulb);
                #endregion
                #region generateRightEye
                Sphere3D rightEyeBulb = new Sphere3D();
                rightEyeBulb.Generate(new List<double>() { 0.20, 64, 64 });
                rightEyeBulb.AmbientColor = Color.Yellow;
                rightEyeBulb.DiffuseColor = Color.White;
                rightEyeBulb.SpecularColor = Color.White;
                rightEyeBulb.Shininess = 50;
                rightEyeBulb.ShowSurfaces = true;
                // x z y
                rightEyeBulb.Move(0.60, 0.34, 0.29);
                rightEyeBulb.ShadingModel = ShadingModel.Smooth;
                rightEyeBulb.UseLight = true;
                rightEyeBulb.Name = "RightEye";
                rightEyeBulb.Object3DList = new List<Object3D>();


                SphereSegment3D rightEyePupil = new SphereSegment3D();
                rightEyePupil.Generate(new List<double>() { 0.25, 16, 32, Math.PI / 2 - 0.2 });
                rightEyePupil.AmbientColor = Color.Black;
                rightEyePupil.DiffuseColor = Color.Black;
                rightEyePupil.SpecularColor = Color.White;
                rightEyePupil.Shininess = 50;
                rightEyePupil.ShowSurfaces = true;
                rightEyePupil.Move(0, 0.05, 0);
                rightEyePupil.RotateX(90); // , 0, 0);
                                          //   rightEyePupil.RotateX(Math.PI / 2);
                                          //  rightEyePupil.Translate(-0.1375, -0.4758, 0.0725);
                rightEyePupil.ShadingModel = ShadingModel.Smooth;
                rightEyePupil.UseLight = true;


                SphereSegment3D rightEyePupilExtended = new SphereSegment3D();
                rightEyePupilExtended.Generate(new List<double>() { 0.25, 16, 32, Math.PI / 2 - 0.2 });
                rightEyePupilExtended.AmbientColor = Color.Black;
                rightEyePupilExtended.DiffuseColor = Color.Black;
                rightEyePupilExtended.SpecularColor = Color.White;
                rightEyePupilExtended.Shininess = 50;
                rightEyePupilExtended.ShowSurfaces = true;
                rightEyePupilExtended.Move(0, 0.05, 0);
                rightEyePupilExtended.RotateZ(13);
                rightEyePupilExtended.RotateX(93); // , 0, 0);
                                                  //   rightEyePupilExtended.RotateX(Math.PI / 2);
                                                  //  rightEyePupilExtended.Translate(-0.1375, -0.4758, 0.0725);
                rightEyePupilExtended.ShadingModel = ShadingModel.Smooth;
                rightEyePupilExtended.UseLight = true;


                SphereSegment3D rightEyeLid = new SphereSegment3D();
                rightEyeLid.Name = "RightEyeLid";
                rightEyeLid.Generate(new List<double>() { 0.21, 32, 32, 0.0 });
                rightEyeLid.AmbientColor = Color.Green;  // Typical skin color approximation
                rightEyeLid.DiffuseColor = Color.Green;  // Typical skin color approximation
                rightEyeLid.SpecularColor = Color.White;
                rightEyeLid.Shininess = 50;
                rightEyeLid.ShowSurfaces = true;
                rightEyeLid.RotateX(0);
                rightEyeLid.Move(0, 0, 0);
                rightEyeBulb.Object3DList.Add(rightEyePupil);
                rightEyeBulb.Object3DList.Add(rightEyePupilExtended);
                rightEyeBulb.Object3DList.Add(rightEyeLid);
                face.Object3DList.Add(rightEyeBulb);


                SphereSegment3D lowerRightEyeLid = new SphereSegment3D();
                lowerRightEyeLid.Name = "LowerRightEyeLid";
                lowerRightEyeLid.Generate(new List<double>() { 0.21, 32, 32, 0.0 });
                lowerRightEyeLid.AmbientColor = Color.Green;  // Typical skin color approximation
                lowerRightEyeLid.DiffuseColor = Color.Green;  // Typical skin color approximation
                lowerRightEyeLid.SpecularColor = Color.White;
                lowerRightEyeLid.Shininess = 50;
                lowerRightEyeLid.ShowSurfaces = true;
                lowerRightEyeLid.RotateX(0);
                lowerRightEyeLid.RotateY(180);
                lowerRightEyeLid.Move(0, 0, 0);
                rightEyeBulb.Object3DList.Add(lowerRightEyeLid);

                #endregion
                face.Move(0, 0, -0.5);
                leftEyeBulb.Move(-0.1375, -0.403, 0.645);
                rightEyeBulb.Move(0.1375, -0.403, 0.645);

                // Close eyes to start with:
                leftEyeLid.RotateX(0);
                rightEyeLid.RotateX(0);
                eyesClosed = true;
                

                Scene3D scene = new Scene3D();
                Light light = new Light();
                light.IsOn = true;
                light.Position = new List<float>() { 0.0f, -3.0f, 1f, 1.0f };
                scene.LightList.Add(light);
                scene.AddObject(face);



                viewer3D.Scene = scene;
                viewer3D.ShowWorldAxes = false; // true;
                viewer3D.CameraDistance = 1.8;
                viewer3D.CameraLatitude = 0.3 ;
                viewer3D.CameraLongitude = -1.3 ;//-Math.PI / 2;

                //viewer3D.Invalidate();

                viewer3D.StartAnimation();


            }

            normalThreadRunning = true;
            normalThread = new Thread(new ThreadStart(NormalLoop));
            normalThread.Start();
        }

        private void NormalLoop()
        {
            while (normalThreadRunning)
            {
                Thread.Sleep(1000);
                double r = randomNumberGenerator.NextDouble();
                if (r < secondBlinkProbability)
                {
                    if ((!blinking) && (!eyesClosed))
                    {
                        Blink();
                    }
                }
            }
        }




        
        private const float eyeBend = 0.05f;
        private const float upperLidStrength = 3f;
        private const float lowerLidStrength = 4f;

        int totalEyeValue = 0;
        private Object eyeLock = new Object(); 


        //open/close eyes (decided if NUMBER_OF_OPENING_STEPS is positive or negative)
        private void MoveEyeBy(int NUMBER_OF_OPENING_STEPS, int OPENING_STEP_MILLISECOND_DURATION){
            lock(eyeLock){
                Object3D leftEyeLid = viewer3D.Scene.GetObject("LeftEyeLid");
                Object3D rightEyeLid = viewer3D.Scene.GetObject("RightEyeLid");
                Object3D lowerLeftEyeLid = viewer3D.Scene.GetObject("LowerLeftEyeLid");
                Object3D lowerRightEyeLid = viewer3D.Scene.GetObject("LowerRightEyeLid");
                float sign = Math.Sign(NUMBER_OF_OPENING_STEPS);
                for (int ii = 0; ii < NUMBER_OF_OPENING_STEPS*sign; ii++)
                {
                    //keeping track of how eyes are at the moment
                    totalEyeValue += (int)sign;
                    if (totalEyeValue < 0) //no overlap of eyelids
                        continue;
                    Thread.Sleep(OPENING_STEP_MILLISECOND_DURATION);                
                    leftEyeLid.RotateX((-upperLidStrength) *sign);
                    rightEyeLid.RotateX((-upperLidStrength) *sign);
                
                    lowerLeftEyeLid.RotateX((-lowerLidStrength) *sign);
                    lowerRightEyeLid.RotateX((-lowerLidStrength) *sign);

                    leftEyeLid.RotateY((-eyeBend) *sign);
                    lowerLeftEyeLid.RotateY((-eyeBend) *sign);

                    rightEyeLid.RotateY((eyeBend) *sign);
                    lowerRightEyeLid.RotateY((eyeBend) *sign);
                }
            }
        }
           

        private void OpenEyesLoop()
        {
            const int NUMBER_OF_OPENING_STEPS = 15;
            const int OPENING_STEP_MILLISECOND_DURATION = 10;

            MoveEyeBy(NUMBER_OF_OPENING_STEPS,OPENING_STEP_MILLISECOND_DURATION);

            eyesClosed = false;
        }

        private void OpenEyes()
        {
            if (eyesClosed)
            {
                openEyesThread = new Thread(new ThreadStart(() => OpenEyesLoop()));
                openEyesThread.Start();
            }
        }

        private void openEyesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (eyesClosed)
            {
                OpenEyes();
            }
            
        }


        private void SquintLoop()
        {
            const int NUMBER_OF_BLINK_STEPS = 11;
            const int BLINK_STEP_MILLISECOND_DURATION = 10;
            
            MoveEyeBy(-NUMBER_OF_BLINK_STEPS,BLINK_STEP_MILLISECOND_DURATION);
            Thread.Sleep(3000);
            MoveEyeBy(NUMBER_OF_BLINK_STEPS,BLINK_STEP_MILLISECOND_DURATION);
            squinting = false;
        }
        private void BlinkLoop()
        {
            const int NUMBER_OF_BLINK_STEPS = 15;
            const int BLINK_STEP_MILLISECOND_DURATION = 10;
            
            MoveEyeBy(-NUMBER_OF_BLINK_STEPS,BLINK_STEP_MILLISECOND_DURATION);
            MoveEyeBy(NUMBER_OF_BLINK_STEPS,BLINK_STEP_MILLISECOND_DURATION);

            blinking = false;
        }

        private void Blink()
        {
            if (!eyesClosed)
            {
                if (!blinking)
                {
                    blinking = true;
                    blinkThread = new Thread(new ThreadStart(() => BlinkLoop()));
                    blinkThread.Start();
                }
            }
        }
        
        private void Squint()
        {
            if (!eyesClosed)
            {
                if (!squinting)
                {
                    squinting = true;
                    squintThread = new Thread(new ThreadStart(() => SquintLoop()));
                    squintThread.Start();
                }
            }
        }
        private void blinkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Blink();
        }

        private void NodLoop()
        {
            const int NUMBER_OF_NOD_STEPS = 20;
            const int NOD_STEP_MILLISECOND_DURATION = 10;

            Object3D face = viewer3D.Scene.GetObject("Face");
            for (int ii = 0; ii < NUMBER_OF_NOD_STEPS; ii++)
            {
                Thread.Sleep(NOD_STEP_MILLISECOND_DURATION);
                face.RotateX(0.5);
            }
            for (int ii = 0; ii < NUMBER_OF_NOD_STEPS; ii++)
            {
                Thread.Sleep(NOD_STEP_MILLISECOND_DURATION);
                face.RotateX(-0.5);
            }
            nodding = false;
        }

        private void Nod()
        {
            if ((!nodding) && (!shaking))
            {
                nodding = true;
                nodThread = new Thread(new ThreadStart(NodLoop));
                nodThread.Start();
            }
        }

        private void nodToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Nod();
        }

        private void ShakeLoop()
        {
            const int NUMBER_OF_SHAKE_STEPS = 15;
            const int SHAKE_STEP_MILLISECOND_DURATION = 10;

            Object3D face = viewer3D.Scene.GetObject("Face");
            for (int ii = 0; ii < NUMBER_OF_SHAKE_STEPS; ii++)
            {
                Thread.Sleep(SHAKE_STEP_MILLISECOND_DURATION);
                face.RotateZ(0.5);
            }
            for (int ii = 0; ii < 2* NUMBER_OF_SHAKE_STEPS; ii++)
            {
                Thread.Sleep(10);
                face.RotateZ(-0.5);
            }
            for (int ii = 0; ii < NUMBER_OF_SHAKE_STEPS; ii++)
            {
                Thread.Sleep(SHAKE_STEP_MILLISECOND_DURATION);
                face.RotateZ(0.5);
            }
            shaking = false;
        }

        private void ShakeHead()
        {
            if ((!nodding) && (!shaking))
            {
                shaking = true;
                shakeThread = new Thread(new ThreadStart(ShakeLoop));
                shakeThread.Start();
            }
        }

        private void shakeHeadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShakeHead();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            viewer3D.Stop();
            if (normalThread != null)
            {
                normalThread.Abort();
            }
            Application.Exit();
        }

        private void FaceApplicationMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            viewer3D.Stop();
            if (normalThread != null)
            {
                normalThread.Abort();
            }
            Application.Exit();
        }

        private float Lerp(float value1,float value2, float amount){
            
            return value1 + (value2 - value1) * amount;

        }

        private void squintToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Squint();
        }
    }
}
