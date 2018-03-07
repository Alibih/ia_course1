using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AudioLibrary;
using AuxiliaryLibrary;
using CommunicationLibrary;
using CustomUserControlsLibrary;
using SpeechRecognitionLibrary;

namespace ListenerApplication
{
    public partial class ListenerMainForm : Form
    {
        private const string CLIENT_NAME = "Listener";
        private const string DEFAULT_IP_ADDRESS = "127.0.0.1";
        private const int DEFAULT_PORT = 7;
        private const string DATETIME_FORMAT = "yyyyMMdd HH:mm:ss";
        private const int DEFAULT_RECORDING_DEVICE_ID = 0;
        private string[] travelArray = new string[] { "get me", "travel", "take me" };

        private string ipAddress = DEFAULT_IP_ADDRESS;
        private int port = DEFAULT_PORT;
        private Client client = null;

        private WAVRecorder wavRecorder = null;
        private SpeechRecognitionEngine speechRecognitionEngine = null;
        private int recordingDeviceID = DEFAULT_RECORDING_DEVICE_ID;

        private Thread clientThread = null;
        private Boolean clientBusy = false;


        private static string[] travelAddressesArray = new string[] { "Chalmers", "Lindholmen", "Centralstation", "Nordstan", "Majorna", "Lilla-Bommen", "Brunnsparken", "Vasagatan", "Avenyn", "Hisingen", "Domkyrkan", "Warwinsky" };
       private static string[] travelInterestArray = new string[] { "Sushi-Restaurant", "Thai-Restaurant", "Indian-Restaurant", "Amusement-Park", "Restaurant" };
        private static string[] shortcutArray = new string[] { "home", "school", "work", "the gym" };

        public ListenerMainForm()
        {
            InitializeComponent();
            Initialize();
            SetUpSpeechRecognizer();
            Connect();
        }

        private void Initialize()
        {
            Size screenSize = Screen.GetBounds(this).Size;
            this.Location = new Point(screenSize.Width - this.Width, screenSize.Height - this.Height);
            mainTabControl.SelectedTab = inputTabPage;
            continuousSpeechRecognitionControl.SoundRecognized += new EventHandler<StringEventArgs>(HandleContinuousSoundRecognized);
            continuousSpeechRecognitionControl.SoundDetected += new EventHandler<WAVSoundEventArgs>(HandleContinuousSoundDetected);
            List<string> recordingDeviceNameList = WAVRecorder.GetDeviceNames();
            recordingDeviceComboBox.Items.Clear();
            foreach (string recordingDeviceName in recordingDeviceNameList)
            {
                recordingDeviceComboBox.Items.Add(recordingDeviceName);
            }
            if (recordingDeviceNameList.Count > 0)
            {
                recordingDeviceComboBox.SelectedIndex = recordingDeviceID;
            }
            clientBusy = false;


        }

        private void SetUpSpeechRecognizer()
        {
            speechRecognitionEngine = new SpeechRecognitionEngine();
            LoadGrammar();
        }

        private GrammarBuilder GenerateGrammarBuilder(string[] keys, string[] values)
        {
            GrammarBuilder temp = new GrammarBuilder(new Choices(keys));
            temp.Append(new Choices(values));
            return temp;
        }

        private void LoadGrammar()
        {
            

            Choices choices = new Choices();
            List<string> phraseList = new List<string>();
            for (int ii = 0; ii < grammarPhraseListBox.Items.Count; ii++)
            {
                string phrase = grammarPhraseListBox.Items[ii].ToString();
                phraseList.Add(phrase);
            }
            choices.Add(phraseList.ToArray());
            if (phraseList.Count > 0)
            {
                GrammarBuilder grammarBuilder = new GrammarBuilder();
                CultureInfo currentCulture = new CultureInfo("en-US");
                grammarBuilder.Culture = currentCulture;
                grammarBuilder.Append(choices);

                speechRecognitionEngine.LoadGrammar(new Grammar(grammarBuilder));
                TravelGrammar(speechRecognitionEngine);
                speechRecognitionEngine.LoadGrammar(new Grammar(whoisGrammar()));
                speechRecognitionEngine.LoadGrammar(new Grammar(shortcutGrammar()));
                recognizeButton.Enabled = true;
            }
            else
            {
                recognizeButton.Enabled = false;
            }
        }


        private GrammarBuilder TravelGrammar(SpeechRecognitionEngine srecognition)
        {

            string[] combined = travelAddressesArray.Union(shortcutArray).ToArray();
            GrammarBuilder toX = GenerateGrammarBuilder(new[] { "to" }, combined);
            GrammarBuilder fromX = GenerateGrammarBuilder(new[] { "from" }, combined);
            GrammarBuilder atX = GenerateGrammarBuilder(new[] { "at", "i'm at", "i am at" }, combined);

            /*
            Choices toChoices = new Choices(toX);
            toChoices.Add(" ");
            Choices fromChoices = new Choices(fromX);
            fromChoices.Add(" ");
            */
            GrammarBuilder fromXtoX = new GrammarBuilder(new Choices(fromX));
            fromXtoX.Append(toX, 0, 1);

            GrammarBuilder toXfromX = new GrammarBuilder(new Choices(toX));
            toXfromX.Append(fromX, 0, 1);

            GrammarBuilder askBuild = new GrammarBuilder(new Choices(travelArray));
            askBuild.Append(toXfromX);
            GrammarBuilder askBuild2 = new GrammarBuilder(new Choices(travelArray));
            askBuild2.Append(fromXtoX);
            //askBuild.Append(allGrammar2);
            //arbitrary places of interest  (food/amusement etc)
            Choices point = new Choices(travelInterestArray);

            GrammarBuilder interestBuild = new GrammarBuilder(new Choices(new string[] { "take me to a", "find a", "find the", "look for a" }));
            interestBuild.Append(point);

            srecognition.LoadGrammar(new Grammar(askBuild2));
            srecognition.LoadGrammar(new Grammar(askBuild));
            srecognition.LoadGrammar(new Grammar(interestBuild));
            srecognition.LoadGrammar(new Grammar(fromX));
            srecognition.LoadGrammar(new Grammar(toX));
            srecognition.LoadGrammar(new Grammar(atX));
            //GrammarBuilder travelBuild = new GrammarBuilder(new Choices(askBuild2,interestBuild,toX,fromX));
            return interestBuild;
        }
        private GrammarBuilder whoisGrammar()
        {
            Choices lastNames = new Choices(new string[] { "Gates", "Musk", "Einstein", "Smith" });

            GrammarBuilder firstNameBuild = new GrammarBuilder(new Choices(new string[] { "Bill", "Elon", "Albert", "Will" }));
            firstNameBuild.Append(lastNames);

            GrammarBuilder whoisBuild = new GrammarBuilder("who is");
            whoisBuild.Append(firstNameBuild);

            return whoisBuild;
        }
        private GrammarBuilder shortcutGrammar()
        {
            Choices shortcuts = new Choices(shortcutArray);
            GrammarBuilder toScBuild = new GrammarBuilder(new Choices(new string[] { "to","as" }));
            toScBuild.Append(shortcuts);

            GrammarBuilder addrBuild = new GrammarBuilder(new Choices(travelAddressesArray));
            addrBuild.Append(toScBuild);

            GrammarBuilder scBuild = new GrammarBuilder(new Choices("Set"));
            scBuild.Append(addrBuild);
            return scBuild;
        }
        

        private void Connect()
        {
            client = new Client();
            client.Progress += new EventHandler<CommunicationProgressEventArgs>(HandleClientProgress);
            client.Error += new EventHandler<CommunicationErrorEventArgs>(HandleClientError); // 20171214
            client.ConnectionEstablished += new EventHandler(HandleConnectionEstablished);
            client.Name = CLIENT_NAME;
            client.Connect(ipAddress, port);
        }

        private void HandleConnectionEstablished(object sender, EventArgs e)
        {
            // Start continuous recognition once a connection to the server has been established
            grammarPhraseTextBox.Enabled = false;
            grammarPhraseListBox.Enabled = false;
            addToGrammarButton.Enabled = false;
            speechRecognitionEngine = new SpeechRecognitionEngine();
            LoadGrammar();
            recordingDeviceComboBox.Enabled = false;
            int detectionThreshold = int.Parse(detectionThresholdTextBox.Text);
            continuousSpeechRecognitionControl.DetectionThreshold = detectionThreshold;
            continuousSpeechRecognitionControl.RecordingDeviceID = recordingDeviceID;
            continuousSpeechRecognitionControl.SetSpeechRecognitionEngine(speechRecognitionEngine);
            continuousSpeechRecognitionControl.ShowSoundStream = showSoundStreamToolStripMenuItem.Checked;
            continuousSpeechRecognitionControl.ExtractDetectedSounds = extractDetectedSoundsToolStripMenuItem.Checked;
            continuousSpeechRecognitionControl.Start();
            continuousInputButton.Enabled = false;
            continuousInputStopButton.Enabled = true;
        }

        private void HandleClientProgress(object sender, CommunicationProgressEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => ShowProgress(e)));
            }
            else { ShowProgress(e); }
            clientBusy = false; // 20171214 The HandleClientProgress method is executed once a 
                                // message has been succesfully sent.
        }

        private void HandleClientError(object sender, CommunicationErrorEventArgs e)
        {
            clientBusy = false; // 20171214 The HandleClientError methods is executed if the
                                // client fails to send the message.
        }

        private void ShowProgress(CommunicationProgressEventArgs e)
        {
            ColorListBoxItem item;
            item = new ColorListBoxItem(e.Message, communicationLogColorListBox.BackColor, communicationLogColorListBox.ForeColor);
            communicationLogColorListBox.Items.Insert(0, item);
        }

        private void inputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Enter)
            {
                string message = inputTextBox.Text;
                inputTextBox.Text = "";
                e.Handled = true;
                e.SuppressKeyPress = true;
                client.Send(message);
                ColorListBoxItem item = new ColorListBoxItem(DateTime.Now.ToString(DATETIME_FORMAT) + ": " + message, inputMessageColorListBox.BackColor,
                    inputMessageColorListBox.ForeColor);
                inputMessageColorListBox.Items.Insert(0, item);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            continuousSpeechRecognitionControl.Stop();
            Application.Exit();
        }

        private void recordingButton_Click(object sender, EventArgs e)
        {
            if (wavRecorder == null)
            {
                wavRecorder = new WAVRecorder();
                wavRecorder.SampleRate = 16000;
                wavRecorder.StorageDuration = 10;
            }
            if (!wavRecorder.IsRecording)
            {
                recognizeButton.Enabled = false;
                recordingButton.Text = "Stop recording";
                wavRecorder.Start();
            }
            else
            {
                wavRecorder.Stop();
                byte[] recordedBytes = wavRecorder.GetAllRecordedBytes();
                if (recordedBytes != null)
                {
                    if (recordedBytes.Length > 0)
                    {
                        WAVSound recordedSound = new WAVSound("", wavRecorder.SampleRate, wavRecorder.NumberOfChannels, wavRecorder.NumberOfBitsPerSample);
                        recordedSound.AppendSamplesAsBytes(recordedBytes);
                        recordedSound.SetMaximumNonClippingVolume();
                        soundVisualizer.SetSound(recordedSound);
                    }
                }
                recordingButton.Text = "Start recording";
                recognizeButton.Enabled = true;
            }
        }

        private void recognizeButton_Click(object sender, EventArgs e)
        {
            if (soundVisualizer.Sound == null) { return; }
            soundVisualizer.Sound.GenerateMemoryStream();
            speechRecognitionEngine.SetInputToWaveStream(soundVisualizer.Sound.WAVMemoryStream);
            RecognitionResult r = speechRecognitionEngine.Recognize();
            if (r != null)
            {
                inputTextBox.Text = r.Text;
                client.Send(r.Text);
                ColorListBoxItem item = new ColorListBoxItem(DateTime.Now.ToString(DATETIME_FORMAT) + ": " + r.Text, inputMessageColorListBox.BackColor,
                    inputMessageColorListBox.ForeColor);
                inputMessageColorListBox.Items.Insert(0, item);
            }
        }

        private void continuousInputButton_Click(object sender, EventArgs e)
        {
            continuousInputButton.Enabled = false;
            grammarPhraseTextBox.Enabled = false;
            grammarPhraseListBox.Enabled = false;
            addToGrammarButton.Enabled = false;
            speechRecognitionEngine = new SpeechRecognitionEngine();
            LoadGrammar();
            recordingDeviceComboBox.Enabled = false;
            recordingDeviceID = recordingDeviceComboBox.SelectedIndex; // 20171214
            int detectionThreshold = int.Parse(detectionThresholdTextBox.Text);
            continuousSpeechRecognitionControl.DetectionThreshold = detectionThreshold;
            continuousSpeechRecognitionControl.RecordingDeviceID = recordingDeviceID;
            continuousSpeechRecognitionControl.SetSpeechRecognitionEngine(speechRecognitionEngine);
            continuousSpeechRecognitionControl.ShowSoundStream = showSoundStreamToolStripMenuItem.Checked;
            continuousSpeechRecognitionControl.ExtractDetectedSounds = extractDetectedSoundsToolStripMenuItem.Checked;
            continuousSpeechRecognitionControl.Start();
            continuousInputStopButton.Enabled = true;
        }

        private void ShowContinuousRecognizedSound(string recognizedString)
        {
            ColorListBoxItem item = new ColorListBoxItem(DateTime.Now.ToString(DATETIME_FORMAT) + ": " + recognizedString, continuousInputColorListBox.BackColor,
                    continuousInputColorListBox.ForeColor);
            continuousInputColorListBox.Items.Insert(0, item);
        }

        private void SendMessage(string message)
        {
            client.Send(message);
            // clientBusy = false;  // Should not be set here - let the event handlers (see above) handle this. 20171214
        }

        private void HandleContinuousSoundRecognized(object sender, StringEventArgs e)
        {
            if (!clientBusy)
            {
                clientBusy = true;
                clientThread = new Thread(new ThreadStart(() => SendMessage(e.StringValue)));
                clientThread.Start();
            }
            //    client.Send(e.StringValue);
            if (continuousSpeechRecognitionControl.ExtractDetectedSounds == true)
            {
                if (InvokeRequired) { this.BeginInvoke(new MethodInvoker(() => ShowContinuousRecognizedSound(e.StringValue))); }
                else { ShowContinuousRecognizedSound(e.StringValue); }
            }
        }

        private void HandleContinuousSoundDetected(object sender, WAVSoundEventArgs e)
        {
            if (InvokeRequired) { this.BeginInvoke(new MethodInvoker(() => detectedSoundVisualizer.SetSound(e.Sound))); }
            else { detectedSoundVisualizer.SetSound(e.Sound); }
        }

        private void showSoundStreamToolStripMenuItem_Click(object sender, EventArgs e)
        {
            continuousSpeechRecognitionControl.ShowSoundStream = showSoundStreamToolStripMenuItem.Checked;
        }

        private void extractDetectedSoundsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            continuousSpeechRecognitionControl.ExtractDetectedSounds = extractDetectedSoundsToolStripMenuItem.Checked;
        }

        private void addToGrammarButton_Click(object sender, EventArgs e)
        {
            if (grammarPhraseTextBox.Text != "")
            {
                if (!grammarPhraseListBox.Items.Contains(grammarPhraseTextBox.Text))
                {
                    grammarPhraseListBox.Items.Insert(0, grammarPhraseTextBox.Text);
                }
            }
        }

        private void grammarPhraseListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = grammarPhraseListBox.SelectedIndex;
            if (selectedIndex >= 0)
            {
                grammarPhraseListBox.Items.RemoveAt(selectedIndex);
            }
        }

        private void continuousInputStopButton_Click(object sender, EventArgs e)
        {
            continuousInputStopButton.Enabled = false;
            continuousSpeechRecognitionControl.Stop();
            continuousInputButton.Enabled = true;
            recordingDeviceComboBox.Enabled = true;
        }

        private void detectionThresholdTextBox_TextChanged(object sender, EventArgs e)
        {
            int detectionThreshold;
            Boolean ok = int.TryParse(detectionThresholdTextBox.Text, out detectionThreshold);
            if (ok)
            {
                if (detectionThreshold > 0) { continuousSpeechRecognitionControl.DetectionThreshold = detectionThreshold; }
            }
        }

        private void ListenerMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            continuousSpeechRecognitionControl.Stop();
            Application.Exit();
        }
    }
}
