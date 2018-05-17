using DbLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Adr.MySettings;
using System.IO;
using Oracle.ManagedDataAccess.Client;
using System.Diagnostics;
using System.Threading;

namespace Adr
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Subscribe();
            InitData();
        }

        OracleConnect _con;
        List<string> _list;
        event Action<int> Finished;
        string _pathToDir = "";
        int _threadCounter = 0;

        private void Subscribe()
        {
            Finished += Form1_Finished;
        }

        private void Form1_Finished(int i)
        {
            File.Copy("convert1.csv", "convert.csv", true);
            //MessageBox.Show("Finished " + i);
            ImportSplitedAdr();
            if (InvokeRequired)
            {
                Action action = () =>
                {
                    button_start.Enabled = false;
                };
                Invoke(action);
            }
            else
            {
                button_start.Enabled = false;
            }
        }

        private void ImportSplitedAdr()
        {
            _threadCounter++;
            if (_threadCounter == Convert.ToInt32(numericUpDown_threads.Value))
            {
                File.Copy("convert.csv", _pathToDir + "\\convert.csv", true);
                StartBatFile("IMPORT.BAT");                
            }
            if (_threadCounter == (Convert.ToInt32(numericUpDown_threads.Value) + 1))
            {
                string query = File.ReadAllText("ImportSplitedAdr.sql", Encoding.Default);
                SplitAndExecSubQueries(query);
                CheckResult();
            }            
        }

        private void CheckResult()
        {            
            Mediator.ApYes = GetCount("select count(*) from import_clnt_example t where t.comment33 = 1");            
            Mediator.ApNo = GetCount("select count(*) from import_clnt_example t where t.comment33 = 0");
            Mediator.AfYes = GetCount("select count(*) from import_clnt_example t where t.comment34 = 1");
            Mediator.AfNo = GetCount("select count(*) from import_clnt_example t where t.comment34 = 0");
            Mediator.AwYes = GetCount("select count(*) from import_clnt_example t where t.comment35 = 1");
            Mediator.AwNo = GetCount("select count(*) from import_clnt_example t where t.comment35 = 0");
            Mediator.AvrYes = GetCount("select count(*) from import_clnt_example t where t.comment36 = 1");
            Mediator.AvrNo = GetCount("select count(*) from import_clnt_example t where t.comment36 = 0");
            
            FormRes form = new FormRes();
            form.ShowDialog();
            this.Close();
        }

        private int GetCount(string query)
        {
            OracleDataReader reader = _con.GetReader(query);
            int i = -1;
            while (reader.Read())
            {
                i = Convert.ToInt32(reader[0]);
            }
            reader.Close();
            return i;
        }

        void InitData()
        {
            _con = new OracleConnect("User ID=IMPORT_USER;password=sT7hk9Lm;Data Source=CD_WORK");
            _con.OpenConnect();
            string query = File.ReadAllText("CleanDubles.sql", Encoding.Default);
            SplitAndExecSubQueries(query);
            query = File.ReadAllText("AddrToSplit.sql", Encoding.Default);           
            OracleDataReader reader = _con.GetReader(query);
            _list = new List<string>();
            while (reader.Read())
            {
                string str = reader[0].ToString() + " ; " + reader[1].ToString().Replace(";", ",");
                _list.Add(str);
            }
            reader.Close();
            Cleaner();
            FillData();            
        }

        private void SplitAndExecSubQueries(string query)
        {
            string[] subQueries = query.Split(';');
            foreach (var item in subQueries)
            {
                _con.ExecCommand(item);
            }
        }

        private void GetPathToDir()
        {
            try
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.SelectedPath = @"x:\Реєстри\ЄАПБ (Факторинг)";
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    _pathToDir = fbd.SelectedPath;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void FillData()
        {
            label_adr_count.Text = _list.Count.ToString();
            foreach (var item in _list)
            {                
                listBox_adr.Items.Add(item);
            }
        }

        private void Cleaner()
        {
            for (int i = 0; i < _list.Count; ++i)
            {
                _list[i] = _list[i].Replace(", , , ,", ",");
                _list[i] = _list[i].Replace(", , ,", ",");
                _list[i] = _list[i].Replace(", ,", ",");                
                _list[i] = _list[i].Replace("РД", "");
                _list[i] = _list[i].Replace("SR", "");
                _list[i] = _list[i].Replace(" нас.пункт", "");
                _list[i] = _list[i].Replace("відсутні", "");
                _list[i] = _list[i].Replace("гр./с.", "");
                _list[i] = _list[i].Replace("п.код", "");
            }
        }
        
        void StartBatFile(string fileName)
        {
            //CleanResFiles();
            Process proc = new Process();
            proc.Exited += new EventHandler(FinishHandler);
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.FileName = Environment.CurrentDirectory + "\\" + fileName;
            proc.EnableRaisingEvents = true;
            proc.Start();
        }

        private void CleanResFiles()
        {
            string[] arr = Directory.GetFiles(Environment.CurrentDirectory, "convert*");
            foreach (var item in arr)
            {
                File.Delete(item);
            }
        }

        private void FinishHandler(object sender, EventArgs e)
        {
            if (Finished != null)
            {
                Finished(1);
            }
        }

        private void button_start_Click(object sender, EventArgs e)
        {
            GetPathToDir();
            switch (numericUpDown_threads.Value)
            {
                case 2:
                    Two();
                    break;
                case 3:
                    Three();
                    break;
                case 4:
                    Four();
                    break;
                default:
                    One();
                    break;
            }
        }

        private void One()
        {
            File.WriteAllLines("adr.csv", _list, Encoding.Default);
            if (_pathToDir.Length < 1)
            {
                MessageBox.Show("Необходимо указать путь к папке реестра.");
            }
            else
            {
                File.WriteAllLines(_pathToDir + "\\adr.csv", _list, Encoding.Default);
                StartBatFile("Split_work.bat");
            }            
        }

        private void Two()
        {
            
        }

        private void Three()
        {
            throw new NotImplementedException();
        }

        private void Four()
        {
            throw new NotImplementedException();
        }

        private void button_path_Click(object sender, EventArgs e)
        {
            GetPathToDir();
        }
    }
}
