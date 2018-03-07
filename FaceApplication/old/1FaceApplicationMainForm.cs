using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommunicationLibrary;
using CustomUserControlsLibrary;
namespace FaceApplication
{
    public partial class FaceApplicationMainForm : Form
    {
        
        FaceApplicationForm face = null;
        Client client = null;
        private const string CLIENT_NAME = "Face";
        private const string DEFAULT_IP_ADDRESS = "127.0.0.1";
        private const int DEFAULT_PORT = 7;
        private string ipAddress = DEFAULT_IP_ADDRESS;
        private int port = DEFAULT_PORT;

        public FaceApplicationMainForm()
        {
            InitializeComponent();
            

            face = new FaceApplicationForm();
            face.TopLevel = false;
            face.AutoScroll = true;
            rightContainer.Panel1.Controls.Add(face);
            face.FormBorderStyle = FormBorderStyle.None;
            face.Show();
            face.Height = rightContainer.Panel1.Height;
            face.Width = rightContainer.Panel1.Width;

            rightContainer.Panel1.SizeChanged += new EventHandler(rightContainer_Panel1_SizeChanged);
            
            Connect();
            

        }

        #region server
        private void Connect()
        {
            client = new Client();
            client.Received += new EventHandler<DataPacketEventArgs>(HandleClientReceived);
            client.Progress += new EventHandler<CommunicationProgressEventArgs>(HandleClientProgress);
            client.Name = CLIENT_NAME;
            client.Connect(ipAddress, port);
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
            item = new ColorListBoxItem(e.Message, face.CommunicationLogListBox.BackColor, face.CommunicationLogListBox.ForeColor);
            face.CommunicationLogListBox.Items.Insert(0, item);

        }


        private void HandleClientReceived(object sender, DataPacketEventArgs e)
        {
            string info = e.DataPacket.Message;
            if (info.ToLower() == "openeyes") { face.OpenEyes(); }


            ColorListBoxItem item = new ColorListBoxItem("handling : " + info, face.CommunicationLogListBox.BackColor, face.CommunicationLogListBox.ForeColor);
            face.CommunicationLogListBox.Items.Insert(0, item);
            // ToDO: Add more actions here

        }
        #endregion



        private void rightContainer_Panel1_SizeChanged(object sender, System.EventArgs e)
        {
            face.Height = rightContainer.Panel1.Height;
            face.Width = rightContainer.Panel1.Width;
        }







    }
}
