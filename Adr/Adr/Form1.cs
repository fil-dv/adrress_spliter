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

        void Subscribe()
        {
            Finished += Form1_Finished;
        }

        private void Form1_Finished(int i)
        {
            try
            {
                _threadCounter++;
                if (_threadCounter == Convert.ToInt32(numericUpDown_threads.Value))
                {
                    FilesProcessing();
                    File.Copy("convert.csv", _pathToDir + "\\convert.csv", true);
                    StartBatFile("IMPORT.BAT");
                }
                if (_threadCounter == (Convert.ToInt32(numericUpDown_threads.Value) + 1))
                {
                    Loger.AddRecordToLog("Запускаем ImportSplitedAdr.");
                    this.Text = "Идет обновление данных в базе...";
                    string query = File.ReadAllText(@"sql\ImportSplitedAdr.sql", Encoding.Default);
                    SplitAndExecSubQueries(query);
                    Loger.AddRecordToLog("ImportSplitedAdr отработал, запускаем проверку результатов разбивки.");
                    this.Text = "Идет проверка результатов разбивки...";
                    CheckResult();                   
                }
            }
            catch (Exception ex)
            {
                Loger.AddRecordToLog(ex.Message);
            }            
        }

        private void FilesProcessing()
        {
            string[] arr = Directory.GetFiles(Environment.CurrentDirectory, "convert*");
            foreach (var item in arr)
            {
                if (item.Contains("convert.csv"))
                {
                    continue;
                }
                else
                {
                    File.AppendAllText("convert.csv", (File.ReadAllText(item, Encoding.Default) + Environment.NewLine), Encoding.Default);                    
                }
            }
        }

        private void CheckResult()
        {
            try
            {                
                Mediator.ApYes = GetCount("select count(t.unique_id) from import_clnt_example t where t.comment33 = 1 and t.c_p_index is not null");
                Mediator.ApNo = GetCount("select count(t.unique_id) from import_clnt_example t where t.comment33 = 0 and t.c_p_index is not null");
                Mediator.AfYes = GetCount("select count(t.unique_id) from import_clnt_example t where t.comment34 = 1 and t.c_f_index is not null");
                Mediator.AfNo = GetCount("select count(t.unique_id) from import_clnt_example t where t.comment34 = 0 and t.c_f_index is not null");
                Mediator.AwYes = GetCount("select count(t.unique_id) from import_clnt_example t where t.comment35 = 1 and t.c_w_index is not null");
                Mediator.AwNo = GetCount("select count(t.unique_id) from import_clnt_example t where t.comment35 = 0 and t.c_w_index is not null");
                Mediator.AvrYes = GetCount("select count(t.unique_id) from import_clnt_example t where t.comment36 = 1 and t.c_r_index is not null");
                Mediator.AvrNo = GetCount("select count(t.unique_id) from import_clnt_example t where t.comment36 = 0 and t.c_r_index is not null");
                this.Text = "Готово";
                FormRes form = new FormRes();
                form.ShowDialog();
                if (InvokeRequired)
                {
                    Action action = () =>
                    {
                        this.Close();
                    };
                    Invoke(action);
                }
                else
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                Loger.AddRecordToLog(ex.Message);
            }                        
        }

        private int GetCount(string query)
        {
            int i = -1;
            try
            {
                OracleDataReader reader = _con.GetReader(query);
                
                while (reader.Read())
                {
                    i = Convert.ToInt32(reader[0]);
                }
                reader.Close();                
            }
            catch (Exception ex)
            {
                Loger.AddRecordToLog(ex.Message);
            }
            return i;
        }

        void InitData()
        {
            try
            {                
                _con = new OracleConnect("User ID=IMPORT_USER;password=sT7hk9Lm;Data Source=CD_WORK");
                _con.OpenConnect();
                string query = File.ReadAllText(@"sql\CleanDubles.sql", Encoding.Default);
                SplitAndExecSubQueries(query);
                query = File.ReadAllText(@"sql\AddrToSplit.sql", Encoding.Default);
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
                SetThreadCount();
                SetNumericUpDownValue();                
            }
            catch (Exception ex)
            {
                Loger.AddRecordToLog(ex.Message);
            }            
        }

        private void InsertStartDataToLog()
        {
            try
            {
                Loger.AddRecordToLog(Environment.NewLine + Environment.NewLine + "----------------- " + Environment.UserName + " ------------------");
                Loger.AddRecordToLog("Начинаем разбивку, количество потоков: " + numericUpDown_threads.Value + ".");
                string query = File.ReadAllText(@"sql\GetContrAndReg.sql", Encoding.Default);
                OracleDataReader reader = _con.GetReader(query);
                while (reader.Read())
                {
                    Loger.AddRecordToLog("Контрагент :" + reader[0].ToString() + ", " + "реестр: " + reader[1].ToString());
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Loger.AddRecordToLog(ex.Message);
            }
        }

        private void SetThreadCount()
        {
            if (_list.Count <= 100) numericUpDown_threads.Value = 1;
            else if(_list.Count > 100 && _list.Count <= 300) numericUpDown_threads.Value = 2;
            else if (_list.Count > 300 && _list.Count <= 500) numericUpDown_threads.Value = 3;
            else numericUpDown_threads.Value = 4;
        }

        private void SplitAndExecSubQueries(string query)
        {
            try
            {
                string[] subQueries = query.Split(';');
                foreach (var item in subQueries)
                {
                    _con.ExecCommand(item);
                }
            }
            catch (Exception ex)
            {
                Loger.AddRecordToLog(ex.Message);
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
            catch (Exception ex)
            {
                Loger.AddRecordToLog(ex.Message);
            }
        }

        private void FillData()
        {
            try
            {
                label_adr_count.Text = _list.Count.ToString();
                foreach (var item in _list)
                {
                    listBox_adr.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Loger.AddRecordToLog(ex.Message);
            }
        }

        private void Cleaner()
        {
            try
            {
                for (int i = 0; i < _list.Count; ++i)
                {
                    _list[i] = _list[i].Replace(", , , ,", ",");
                    _list[i] = _list[i].Replace(", , ,", ",");
                    _list[i] = _list[i].Replace(", ,", ",");
                    _list[i] = _list[i].Replace("РД", "");
                    _list[i] = _list[i].Replace("РД", ""); 
                    _list[i] = _list[i].Replace("SR", "");
                    _list[i] = _list[i].Replace(" нас.пункт", "");
                    _list[i] = _list[i].Replace("відсутні", "");
                    _list[i] = _list[i].Replace("гр./с.", "");
                    _list[i] = _list[i].Replace("п.код", "");
                    if(!_list[i].Contains("область"))
                        _list[i] = _list[i].Replace("област", "");
                }
            }
            catch (Exception ex)
            {
                Loger.AddRecordToLog(ex.Message);
            }
        }
        
        void StartBatFile(string fileName)
        {
            try
            {
                Process proc = new Process();
                proc.Exited += new EventHandler(FinishHandler);
                proc.StartInfo.CreateNoWindow = true;
                proc.StartInfo.FileName = Environment.CurrentDirectory + "\\" + fileName;
                proc.EnableRaisingEvents = true;
                proc.Start();
            }
            catch (Exception ex)
            {
                Loger.AddRecordToLog(ex.Message);
            }
        }

        void CleanResFiles()
        {
            try
            {
                string[] arr = Directory.GetFiles(Environment.CurrentDirectory, "convert*");
                foreach (var item in arr)
                {
                    File.Delete(item);
                }
            }
            catch (Exception ex)
            {
                Loger.AddRecordToLog(ex.Message);
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
            try
            {
                InsertStartDataToLog();
                GetPathToDir();
                File.WriteAllLines(_pathToDir + "\\adr.csv", _list, Encoding.Default);
                FileCleaning();
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
                Loger.AddRecordToLog("Разбивка закончена.");
            }
            catch (Exception ex)
            {
                Loger.AddRecordToLog(ex.Message);
            }
        }

        private void FileCleaning()
        {
            try
            {
                string[] arrAdr = Directory.GetFiles(Environment.CurrentDirectory, "adr*.csv");
                string[] arrConvert = Directory.GetFiles(Environment.CurrentDirectory, "convert*.csv");
                foreach (var item in arrAdr)
                {
                    File.WriteAllText(item, "");
                }
                foreach (var item in arrConvert)
                {
                    File.WriteAllText(item, "");
                }
            }
            catch (Exception ex)
            {
                Loger.AddRecordToLog(ex.Message);
            }
        }

        private void One()
        {
            try
            {
                WriteToFile(_list, "1");
            }
            catch (Exception ex)
            {
                Loger.AddRecordToLog(ex.Message);
            }
        }

        private void WriteToFile(List<string> list, string str)
        {
            try
            {
                File.WriteAllLines("adr" + str + ".csv", list, Encoding.Default);
                if (_pathToDir.Length < 1)
                {
                    MessageBox.Show("Необходимо указать путь к папке реестра.");
                }
                else
                {
                    StartBatFile("Split_work" + str + ".bat");
                }
            }
            catch (Exception ex)
            {
                Loger.AddRecordToLog(ex.Message);
            }
        }

        private void Two()
        {
            try
            {
                int firstCount = _list.Count / 2;
                int secondCount = _list.Count - firstCount;
                List<string> listOne = new List<string>();
                List<string> listTwo = new List<string>();
                for (int i = 0; i < firstCount; i++)
                {
                    listOne.Add(_list[i]);
                }
                for (int i = firstCount; i < _list.Count; i++)
                {
                    listTwo.Add(_list[i]);
                }
                WriteToFile(listOne, "1");
                WriteToFile(listTwo, "2");
                toolStripStatusLabel1.Text = String.Format("Первый поток - {0}, второй поток - {1}.", firstCount, secondCount);
            }
            catch (Exception ex)
            {
                Loger.AddRecordToLog(ex.Message);
            }
        }

        private void Three()
        {
            try
            {
                int firstCount = _list.Count / 3;
                List<string> listOne = new List<string>();
                List<string> listTwo = new List<string>();
                List<string> listThree = new List<string>();
                for (int i = 0; i < firstCount; i++)
                {
                    listOne.Add(_list[i]);
                }
                for (int i = firstCount; i < firstCount * 2; i++)
                {
                    listTwo.Add(_list[i]);
                }
                for (int i = firstCount * 2; i < _list.Count; i++)
                {
                    listThree.Add(_list[i]);
                }
                WriteToFile(listOne, "1");
                WriteToFile(listTwo, "2");
                WriteToFile(listThree, "3");
            }
            catch (Exception ex)
            {
                Loger.AddRecordToLog(ex.Message);
            }
        }

        private void Four()
        {
            try
            {
                int firstCount = _list.Count / 4;
                List<string> listOne = new List<string>();
                List<string> listTwo = new List<string>();
                List<string> listThree = new List<string>();
                List<string> listFore = new List<string>();
                for (int i = 0; i < firstCount; i++)
                {
                    listOne.Add(_list[i]);
                }
                for (int i = firstCount; i < firstCount * 2; i++)
                {
                    listTwo.Add(_list[i]);
                }
                for (int i = firstCount * 2; i < firstCount * 3; i++)
                {
                    listThree.Add(_list[i]);
                }
                for (int i = firstCount * 3; i < _list.Count; i++)
                {
                    listFore.Add(_list[i]);
                }
                WriteToFile(listOne, "1");
                WriteToFile(listTwo, "2");
                WriteToFile(listThree, "3");
                WriteToFile(listFore, "4");
            }
            catch (Exception ex)
            {
                Loger.AddRecordToLog(ex.Message);
            }
        }

        private void button_path_Click(object sender, EventArgs e)
        {
            GetPathToDir();
        }

        private void numericUpDown_threads_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                SetNumericUpDownValue();                
            }
            catch (Exception ex)
            {
                Loger.AddRecordToLog(ex.Message);
            }
        }

        private void SetNumericUpDownValue()
        {
            try
            {
                int firstCount = _list.Count / Convert.ToInt32(numericUpDown_threads.Value);
                int lastCount = -1;
                switch (numericUpDown_threads.Value)
                {
                    case 2:
                        lastCount = _list.Count - firstCount;
                        toolStripStatusLabel1.Text = String.Format("Первый поток - {0}, второй поток - {1}.", firstCount, lastCount);
                        break;
                    case 3:
                        lastCount = _list.Count - (firstCount * 2);
                        toolStripStatusLabel1.Text = String.Format("Первый поток - {0}, второй поток - {0}, третий поток - {1}.", firstCount, (_list.Count - (firstCount * 2)));
                        break;
                    case 4:
                        lastCount = _list.Count - (firstCount * 3);
                        toolStripStatusLabel1.Text = String.Format("Первый поток - {0}, второй поток - {0}, третий поток - {0}, четвертый поток - {1}.", firstCount, (_list.Count - (firstCount * 3)));
                        break;
                    default:
                        toolStripStatusLabel1.Text = String.Format("Один поток - {0}.", _list.Count);
                        break;
                }
            }
            catch (Exception ex)
            {
                Loger.AddRecordToLog(ex.Message);
            }
        }
    }
}
