using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace eggdoglog
{
    public partial class eggdoglog : Form
    {
        /*
         * Hecho en menos de una tarde
         * Faltaria optimizar y añadir más clientes
         * Por ahora solo soporta Lunar Client y MC default
         */ 
        string path;
        string request;
        List<string> searchRequest = new List<string>();
        List<string> clientPaths = new List<string>();
        List<string> appLog = new List<string>();

        public eggdoglog()
        {
            InitializeComponent();
            getProcess();
            initTime();
        }

        public void initTime()
        {
            foreach (string path in clientPaths)
            {
                Stream s = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader sr = new StreamReader(s);
                while ((request = sr.ReadLine()) != null)
                {
                    if (request.Contains("Setting user:"))
                    {
                        if (request.Length >= 10)
                        {
                            string time = request.Substring(0, 10);

                            switch (true)
                            {
                                case bool _ when path.Contains("multiver"):
                                    inittime.Text = time;
                                    break;
                                case bool _ when path.Contains(@".minecraft\logs\latest.log"):
                                    inittime2.Text = time;
                                    break;
                            }
                        }
                    }
                }
                s.Close();
                sr.Close();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = selectorBox.Text;
            switch (selected)
            {
                case "Disconnect logs":
                    searchRequest.Clear();
                    Results.Items.Clear();
                    searchRequest.Add("removeEntityFromWorld(int)");
                    appLog.Add("Searching disconnect logs");
                    break;
                case "Connecting server logs":
                    searchRequest.Clear();
                    Results.Items.Clear();
                    searchRequest.Add("Connecting to");
                    appLog.Add("Searching connecting server logs");
                    break;
                case "Error logs":
                    searchRequest.Clear();
                    Results.Items.Clear();
                    searchRequest.Add("java.");
                    appLog.Add("Searching error logs");
                    break;
                case "Dont log messages":
                    searchRequest.Clear();
                    Results.Items.Clear();
                    searchRequest.Add("do not log");
                    searchRequest.Add("don't log");
                    searchRequest.Add("dont log");
                    appLog.Add("Searching dont log");
                    break;
            }
        }

        protected void getProcess()
        {
            Process[] processes = Process.GetProcesses();
            string[] target = { "Lunar", "Badlion", "javaw" };
            appLog.Add("Getting processes...");

            foreach (Process process in processes)
            {
                foreach (string targetName in target)
                {
                    if (process.ProcessName.Contains(targetName))
                    {    
                        {
                            switch (targetName)
                            {
                                case "Lunar":
                                    clientPaths.Add(@"C:\Users\" + Environment.UserName + @"\.lunarclient\offline\multiver\logs\latest.log");
                                    appLog.Add("Added lunar client log path");
                                    if (!Clients.Items.Contains("Lunar Client"))
                                    {
                                        Clients.Items.Add("Lunar Client");
                                    } 
                                    break;
                                case "javaw":
                                    clientPaths.Add(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\.minecraft\logs\latest.log");
                                    appLog.Add("Added minecraft log path");
                                    if (!Clients.Items.Contains("Minecrat Launcher"))
                                    {
                                        Clients.Items.Add("Minecrat Launcher");
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (searchRequest.Equals(null))
            {
                MessageBox.Show("Select an option to continue ^_^");
            }
            if (Clients.Items.Count == 0)
            {
                MessageBox.Show("sad eggdog :(");
                appLog.Add("Error: Not client found");
                return;
            }
            try
            {
                foreach (string path in clientPaths) {
                    Stream s = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    StreamReader sr = new StreamReader(s);
                    appLog.Add("Searching in path: "+path);
                    while ((request = sr.ReadLine()) != null)
                    {
                        foreach (string searchstr in searchRequest) {
                            if (request.Contains(searchstr))
                            {
                                Results.Items.Add(request);
                            }
                        }
                    }
                    s.Close();
                    sr.Close();
                    MessageBox.Show("✔️");
                    return;
                }
            }
            catch 
            {
                MessageBox.Show("Can't read this file");
                appLog.Add("Error: Cant read this file");
                return;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Perrohuevo");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string log = string.Join(", ", appLog);
            MessageBox.Show(log);
        }
    }
}
